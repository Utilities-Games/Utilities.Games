using ConsoulLibrary;
using EPPlus.TableSheet;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Utilities.Games.ImportTool.DataModelFactories.Excel
{
    public class TypeSheet
    {
        public string WorksheetName { get; }

        public Type SourceType { get; }

        protected XmlDocument CodeSummary { get; set; }

        public Queue<PropertyInfo> SupplementalPropertyTypes;

        public TypeSheet(Type type)
        {
            SourceType = type;
            WorksheetName = SourceType.Name;

            var xmlFilePath = SourceType.Assembly.Location.Replace(".dll", ".xml");
            CodeSummary = new XmlDocument();
            CodeSummary.Load(xmlFilePath);
        }

        public ExcelWorksheet BuildWorksheet(ExcelPackage package) {
            SupplementalPropertyTypes = new Queue<PropertyInfo>(); // Re-initialize the list

            PropertyInfo sourceKeyProperty = SourceType.GetProperties().FirstOrDefault(o => o.GetCustomAttribute<KeyAttribute>() != null);

            int startRow = 1, startCol = 1;
            int row = startRow, col = startCol;
            var sheet = package.Workbook.Worksheets.Add(WorksheetName);

            var headerEndPoint = BuildHeader(sheet, SourceType, row, col);
            var bodyEndPoint = BuildBody(sheet, SourceType, headerEndPoint.Item1 + 1, col);

            var tableAddress = new ExcelAddressBase(headerEndPoint.Item1, col, bodyEndPoint.Item1, bodyEndPoint.Item2);
            var table = sheet.Tables.Add(tableAddress, SourceType.Name);

            if (SupplementalPropertyTypes.Any())
            {
                do
                {
                    PropertyInfo secondaryInfo = SupplementalPropertyTypes.Dequeue();
                    Type enumerationType = secondaryInfo.PropertyType.GetElementType();
                    if (enumerationType != null)
                    {
                        col = headerEndPoint.Item2 + 3;
                        string secondarySummary = null;
                        if (enumerationType == typeof(string)) {
                            secondarySummary = GetPropertySummary(secondaryInfo)?.InnerText;
                        }

                        headerEndPoint = BuildHeader(sheet, enumerationType, row, col, secondarySummary);

                        bodyEndPoint = BuildBody(sheet, enumerationType, headerEndPoint.Item1 + 1, col);

                        // Add SourceType reference to table
                        sheet.InsertColumn(col, 1);
                        headerEndPoint = (headerEndPoint.Item1, headerEndPoint.Item2 + 1);
                        bodyEndPoint = (bodyEndPoint.Item1, bodyEndPoint.Item2 + 1);
                        var secondarySourceKeyHeaderCell = sheet.Cells[headerEndPoint.Item1, col];
                        secondarySourceKeyHeaderCell.Value = SourceType.Name;

                        AddCellComment(secondarySourceKeyHeaderCell, $"Link to the parent {SourceType.Name}.");

                        if (sourceKeyProperty != null)
                        {
                            secondarySourceKeyHeaderCell.Value += "." + sourceKeyProperty.Name;
                            var secondarySourceKeyValueCell = sheet.Cells[bodyEndPoint.Item1, col];
                            var sourceKeyValidation = secondarySourceKeyValueCell.DataValidation.AddListDataValidation();
                            sourceKeyValidation.AllowBlank = true;
                            sourceKeyValidation.Formula.ExcelFormula = $"=INDIRECT(\"{SourceType.Name}[{sourceKeyProperty.Name}]\")";
                        }

                        // Create format table
                        tableAddress = new ExcelAddressBase(headerEndPoint.Item1, col, bodyEndPoint.Item1, bodyEndPoint.Item2);
                        table = sheet.Tables.Add(tableAddress, $"{SourceType.Name}.{(enumerationType == typeof(string) ? secondaryInfo.Name : enumerationType.Name)}");
                    }
                } while (SupplementalPropertyTypes.Count > 0);
            }


            for (int i = 1; i <= headerEndPoint.Item2; i++)
            {
                try
                {
                    sheet.Column(i).AutoFit();
                }
                catch (Exception ex)
                {
                    Consoul.Write("\r\n\t" + ex.ToString() + "\r\n\t", ConsoleColor.Red);
                }
            }

            return sheet;
        }

        protected virtual (int,int) BuildHeader(ExcelWorksheet worksheet, Type sourceType, int startRow = 1, int startColumn = 1, string summary = null) {
            int row = startRow, col = startColumn;

            var typeSummary = summary ?? GetTypeSummary(sourceType)?.InnerText;
            ExcelRange typeSummaryCell = null;
            if (typeSummary != null)
            {
                typeSummaryCell = worksheet.Cells[row, col];
                typeSummaryCell.Value = typeSummary;
                row++;
            }

            if (sourceType == typeof(string))
            {
                worksheet.Cells[row, col].Value = "Name";
                return (row, col);
            } else
            {
                Tuple<int, int> buildPropertyHeaders(Type type, Tuple<int, int> startPoint, string prefix = "")
                {
                    string formattedPrefix = !string.IsNullOrEmpty(prefix) ? $"{prefix}." : string.Empty;
                    int row = startPoint.Item1, col = startPoint.Item2;

                    var properties = GetProperties(type);
                    foreach (var property in properties)
                    {
                        if (!property.PropertyType.IsArray && property.PropertyType.Namespace == sourceType.Namespace)
                        {
                            var endPoint = buildPropertyHeaders(property.PropertyType, new Tuple<int, int>(row, col), formattedPrefix + property.Name);
                            col = endPoint.Item2;
                        }
                        else
                        {
                            string label = formattedPrefix + property.Name;
                            Consoul.Write($"\r\n\t\t{label}...", writeLine: false);

                            var cell = worksheet.Cells[row, col];

                            DescriptionAttribute descriptionAttribute;
                            if ((descriptionAttribute = property.GetCustomAttribute<DescriptionAttribute>()) != null)
                            {
                                label = formattedPrefix + descriptionAttribute.Description;
                            }

                            // If array, then turn this into a formula column with navigation to the appropriate sheet.
                            if (property.PropertyType.IsArray)
                            {
                                label += " Count";
                                Type secondaryType;
                                if ((secondaryType = property.PropertyType.GetElementType()) != null)
                                {
                                    if (secondaryType == typeof(string) || secondaryType.Namespace == sourceType.Namespace)
                                    {
                                        SupplementalPropertyTypes.Enqueue(property);
                                    }
                                }
                            }

                            cell.Value = label;

                            var xPropertySummary = GetPropertySummary(property);
                            if (xPropertySummary != null)
                            {
                                AddCellComment(cell, xPropertySummary.InnerText.Trim());
                            }

                            Consoul.Write("Done!", ConsoleColor.Green);
                        }

                        col++;
                    }
                    col--;


                    return new Tuple<int, int>(row, col);
                }

                var endPoint = buildPropertyHeaders(sourceType, new Tuple<int, int>(row, col));
                Consoul.Write("\t\t", writeLine: false);// Re-align console for Type
                row = endPoint.Item1;
                col = endPoint.Item2;
            }

            if (typeSummaryCell != null)
            {
                typeSummaryCell = worksheet.Cells[startRow, startColumn, startRow, col];
                typeSummaryCell.Merge = true;
                typeSummaryCell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                typeSummaryCell.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                typeSummaryCell.Style.Font.Bold = true;
            }

            return (row, col);
        }

        protected virtual (int, int) BuildBody(ExcelWorksheet worksheet, Type sourceType, int startRow = 2, int startColumn = 1)
        {
            int row = startRow, col = startColumn;
            if (sourceType == typeof(string))
            {
                return (row, col);
            }

            Tuple<int, int> buildPropertyValues(Type type, Tuple<int, int> startPoint)
            {
                int row = startPoint.Item1, col = startPoint.Item2;

                var properties = GetProperties(type);
                foreach (var property in properties)
                {
                    if (!property.PropertyType.IsArray && property.PropertyType.Namespace == sourceType.Namespace)
                    {
                        var endPoint = buildPropertyValues(property.PropertyType, new Tuple<int, int>(row, col));
                        col = endPoint.Item2;
                    }
                    else
                    {
                        var cell = worksheet.Cells[row, col];

                        if (property.PropertyType.IsArray)
                        {
                            var arrayType = property.PropertyType.GetElementType();
                            if (arrayType != null)
                            {
                                var keyProperty = GetTypeKey(arrayType);
                                if (keyProperty != null)
                                {
                                    cell.Formula = $"=COUNTA({type.Name}.{arrayType.Name}[{keyProperty.Name}])";
                                }
                                else if (arrayType == typeof(string))
                                {
                                    cell.Formula = $"=COUNTA({type.Name}.{property.Name}[Name])";
                                    // Maybe primitive type?, should we create a simple lookup table off to the side of this one for string[], int[], etc.?
                                }
                            }
                        }
                    }

                    col++;
                }
                col--;


                return new Tuple<int, int>(row, col);
            }
            var endPoint = buildPropertyValues(sourceType, new Tuple<int, int>(row, col));
            row = endPoint.Item1;
            col = endPoint.Item2;

            return (row, col);
        }

        protected virtual ExcelComment AddCellComment(ExcelRange cell, string comment)
        {
            var cellComment = cell.AddComment(comment, "Utilities.Games");
            cellComment.AutoFit = true;
            cellComment.BackgroundColor = System.Drawing.Color.White;
            return cellComment;
        }

        private PropertyInfo GetTypeKey(Type type) {
            var properties = type?.GetProperties();
            return properties?.FirstOrDefault(o => o.GetCustomAttribute<KeyAttribute>() != null);
        }

        protected XmlNode GetTypeSummary(Type type, string extraPath = "summary") {
            return CodeSummary.SelectSingleNode($"//member[starts-with(@name, 'T:{type.FullName}')]{(string.IsNullOrEmpty(extraPath) ? "" : $"/{extraPath}")}");
        }

        protected XmlNode GetPropertySummary(PropertyInfo property, string extraPath = "summary") {
            return CodeSummary.SelectSingleNode($"//member[starts-with(@name, 'P:{property.DeclaringType.FullName}.{property.Name}')]{(string.IsNullOrEmpty(extraPath) ? "" : $"/{extraPath}")}");
        }

        protected IEnumerable<PropertyInfo> GetProperties(Type type) => type.GetProperties();
    }
}
