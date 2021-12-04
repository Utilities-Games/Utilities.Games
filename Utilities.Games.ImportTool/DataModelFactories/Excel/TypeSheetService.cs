using ConsoulLibrary;
using OfficeOpenXml;
using OfficeOpenXml.DataValidation.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Xml;
using Utilities.Games.Models.Contracts.Attributes;

namespace Utilities.Games.ImportTool.DataModelFactories.Excel
{
    /// <summary>
    /// A utility instance class that is capable of constructing a <see cref="ExcelWorksheet"/> based on the properties of a <see cref="SourceType"/>.
    /// </summary>
    public class TypeSheetUtility
    {
        /// <summary>
        /// Defines the number of columns between each table for separation.
        /// </summary>
        protected const int SpaceBetweenTables = 3;

        /// <summary>
        /// Display name for the <see cref="ExcelWorksheet"/>.
        /// </summary>
        public string WorksheetName { get; }

        /// <summary>
        /// Reference to the <see cref="Type"/> for which the <see cref="ExcelWorksheet"/> is primarily built for.
        /// </summary>
        public Type SourceType { get; }

        /// <summary>
        /// Reference to the XML documentation file for the <see cref="Utilities.Games.Models"/> project
        /// </summary>
        protected XmlDocument CodeSummary { get; set; }

        /// <summary>
        /// Queue of recursive object references that ultimately need separate format tables constructed to manage the data.
        /// </summary>
        public Queue<SupplementalTableReference> SupplementalPropertyTypes;

        public TypeSheetUtility(Type type)
        {
            SourceType = type;
            WorksheetName = SourceType.Name;

            var xmlFilePath = SourceType.Assembly.Location.Replace(".dll", ".xml");
            CodeSummary = new XmlDocument();
            CodeSummary.Load(xmlFilePath);
        }

        /// <summary>
        /// Constructs a new <see cref="ExcelWorksheet"/> based on the definition of the <see cref="SourceType"/>.
        /// </summary>
        /// <param name="package">Reference to the root <see cref="ExcelPackage"/> for which the <see cref="ExcelWorksheet"/> will be added.</param>
        /// <returns>Reference to the newly constructed <see cref="ExcelWorksheet"/>.</returns>
        public ExcelWorksheet BuildWorksheet(ExcelPackage package) {
            SupplementalPropertyTypes = new Queue<SupplementalTableReference>(); // Re-initialize the list

            int startRow = 1, startCol = 1;
            int row = startRow, col = startCol;
            var sheet = package.Workbook.Worksheets.Add(WorksheetName);

            // Build table header for SourceType
            var headerEndPoint = BuildHeader(sheet, SourceType, row, col);
            // Build table body placeholder for SourceType
            var bodyEndPoint = BuildBody(sheet, SourceType, headerEndPoint.Item1 + 1, col);
            // Build and format table
            var tableAddress = new ExcelAddressBase(headerEndPoint.Item1, col, bodyEndPoint.Item1, bodyEndPoint.Item2);
            var table = sheet.Tables.Add(tableAddress, SourceType.Name);

            // Add supplemental table sources to the primary source type
            // For example, a simple string array on the source type can be defined in a separate format table and linked back to the parent object's row.
            if (SupplementalPropertyTypes.Any())
            {
                do
                {
                    SupplementalTableReference supplementalTable = SupplementalPropertyTypes.Dequeue();

                    PropertyInfo sourceKeyProperty = GetTypeKey(supplementalTable.SourceType);

                    Type enumerationType = supplementalTable.TargetProperty.PropertyType.GetElementType();
                    if (enumerationType != null)
                    {
                        // Add space between last table and this one
                        col = headerEndPoint.Item2 + (SpaceBetweenTables + 1);

                        // Prepare summary for supplementary entity table
                        string supplementarySummary = null;
                        if (enumerationType == typeof(string)) {
                            supplementarySummary = GetPropertySummary(supplementalTable.TargetProperty)?.InnerText;
                        }

                        headerEndPoint = BuildHeader(sheet, enumerationType, row, col, supplementarySummary);

                        bodyEndPoint = BuildBody(sheet, enumerationType, headerEndPoint.Item1 + 1, col);


                        // Update endpoints (Row, Column)
                        headerEndPoint = (headerEndPoint.Item1, headerEndPoint.Item2 + 1);
                        bodyEndPoint = (bodyEndPoint.Item1, bodyEndPoint.Item2 + 1);

                        // Add link to parent sourceType table
                        sheet.InsertColumn(col, 1);

                        // Establish the link between this table and the sourceType table
                        using (var supplementarySourceKeyHeaderCell = sheet.Cells[headerEndPoint.Item1, col])
                        {
                            supplementarySourceKeyHeaderCell.Value = supplementalTable.SourceType.Name;

                            if (sourceKeyProperty != null)
                            {
                                supplementarySourceKeyHeaderCell.Value += "." + sourceKeyProperty.Name;

                                using (var supplementarySourceKeyValueCell = sheet.Cells[bodyEndPoint.Item1, col])
                                {
                                    AddDataListValidation(supplementarySourceKeyValueCell, $"=INDIRECT(\"{supplementalTable.SourceType.Name}[{sourceKeyProperty.Name}]\")");
                                }
                            }
                            // This results in corrupt file which Excel repairs by removing corrupt comments.
                            //AddCellComment(supplementarySourceKeyHeaderCell, $"Link {(enumerationType == typeof(string) ? secondaryInfo.Name : enumerationType.Name)} to the parent {SourceType.Name} by selecting it in the dropdown.");
                        }

                        // Create structured table
                        tableAddress = new ExcelAddressBase(headerEndPoint.Item1, col, bodyEndPoint.Item1, bodyEndPoint.Item2);
                        table = sheet.Tables.Add(tableAddress, $"{supplementalTable.SourceType.Name}.{(enumerationType == typeof(string) ? supplementalTable.TargetProperty.Name : enumerationType.Name)}");
                    }
                } while (SupplementalPropertyTypes.Count > 0);
            }

            // Try to autofit each column in the sheet
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

        /// <summary>
        /// Constructs the structured table header row(s) recursively starting with the provided <paramref name="sourceType"/>.
        /// </summary>
        /// <param name="worksheet">Reference to the <see cref="ExcelWorksheet"/> for which to add the header.</param>
        /// <param name="sourceType">Reference to the <see cref="Type"/> for which to construct a table header for, based on properties of the type.</param>
        /// <param name="startRow">Reference to the starting row position for which to begin constructing the table header. Note, this is a 1-based value in Excel.</param>
        /// <param name="startColumn">Reference to the ending column position for which to begin constructing the table header. Note, this is a 1-based value in Excel.</param>
        /// <param name="summary">An override description for the provided <see cref="SourceType"/>. If not provided, the XML documentation for the source code is referenced and the <c>&lt;summary /&gt;</c> text is used.</param>
        /// <returns>Tuple of the ending Row and Column references.</returns>
        protected virtual (int,int) BuildHeader(ExcelWorksheet worksheet, Type sourceType, int startRow = 1, int startColumn = 1, string summary = null) {
            // Initialize the running row and column references.
            int row = startRow, col = startColumn;

            // Try to ensure a summary text is found for the row above the table header. This helps explain what the type is within the document.
            var typeSummary = summary ?? GetTypeSummary(sourceType)?.InnerText;
            ExcelRange typeSummaryCell = null;
            if (typeSummary != null)
            {
                typeSummaryCell = worksheet.Cells[row, col];
                typeSummaryCell.Value = typeSummary;
                row++;
            }

            // If the sourceType is string, then there only needs to be one column and that can just be "Name" for generics purposes.
            if (sourceType == typeof(string))
            {
                worksheet.Cells[row, col].Value = "Name";
                return (row, col);
            } else
            {
                // Internal recursive method for constructing header columns based on property types. This will flatten properties into columns.
                (int, int) buildPropertyHeaders(Type type, (int, int) startPoint, string prefix = "")
                {
                    // Prepare a prefix for columns names/reference if necessary
                    string formattedPrefix = !string.IsNullOrEmpty(prefix) ? $"{prefix}." : string.Empty;
                    // Initialize the running row and column references based on the current recursion.
                    int row = startPoint.Item1, col = startPoint.Item2;

                    var properties = GetProperties(type);
                    foreach (var property in properties)
                    {
                        // If the property is not a simple array and within the namespace of the Utilities.Games.Models, then flatten the property.
                        if (!property.PropertyType.IsArray && property.PropertyType.Namespace == sourceType.Namespace)
                        {
                            var endPoint = buildPropertyHeaders(property.PropertyType, (row, col), formattedPrefix + property.Name);
                            col = endPoint.Item2;
                        }
                        else
                        {
                            // Create a table header label based on the available prefix and the current property name.
                            string label = formattedPrefix + property.Name;
                            Consoul.Write($"\r\n\t\t{label}...", writeLine: false);

                            using (var cell = worksheet.Cells[row, col])
                            {
                                // If array, then dedicate this column as a reference to another structured table
                                if (property.PropertyType.IsArray)
                                {
                                    label += " Count";
                                    Type secondaryType;
                                    if ((secondaryType = property.PropertyType.GetElementType()) != null)
                                    {
                                        if (secondaryType == typeof(string) || secondaryType.Namespace == sourceType.Namespace)
                                        {
                                            SupplementalPropertyTypes.Enqueue(new SupplementalTableReference { TargetProperty = property, SourceType = sourceType });
                                        }
                                    }
                                }

                                cell.Value = label;

                                var xPropertySummary = GetPropertySummary(property);
                                if (xPropertySummary != null)
                                {
                                    AddCellComment(cell, xPropertySummary.InnerText.Trim());
                                }
                            }

                            Consoul.Write("Done!", ConsoleColor.Green);
                        }

                        // TODO: Ensure that a column was added before attempting to increment the column position
                        col++;
                    }
                    col--;


                    return (row, col);
                }

                // Begin (potentially) recursive operation
                var endPoint = buildPropertyHeaders(sourceType, (row, col));

                Consoul.Write("\t\t", writeLine: false);// Re-align console for Type

                // Update running row and column positions based on the results of the recursion
                row = endPoint.Item1;
                col = endPoint.Item2;
            }

            // If a class summary exists, then place it above the table headers
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

        /// <summary>
        /// Constructs the structured table body row(s) recursively starting with the provided <paramref name="sourceType"/>.
        /// </summary>
        /// <param name="worksheet"><inheritdoc cref="BuildHeader" path="/param[@name='worksheet']"/></param>
        /// <param name="sourceType"><inheritdoc cref="BuildHeader" path="/param[@name='sourceType']"/></param>
        /// <param name="startRow"><inheritdoc cref="BuildHeader" path="/param[@name='startRow']"/></param>
        /// <param name="startColumn"><inheritdoc cref="BuildHeader" path="/param[@name='startColumn']"/></param>
        /// <returns><inheritdoc cref="BuildHeader" path="/returns"/></returns>
        protected virtual (int, int) BuildBody(ExcelWorksheet worksheet, Type sourceType, int startRow = 2, int startColumn = 1)
        {
            int row = startRow, col = startColumn;
            if (sourceType == typeof(string))
            {
                return (row, col);
            }

            Tuple<int, int> buildPropertyValues(Type type, Tuple<int, int> startPoint)
            {
                PropertyInfo sourceKeyProperty = GetTypeKey(type);
                int row = startPoint.Item1, col = startPoint.Item2;

                var properties = GetProperties(type);
                foreach (var property in properties)
                {
                    PseudoForeignKeyAttribute pseudoFKAttribute = property.GetCustomAttribute<PseudoForeignKeyAttribute>();

                    if (!property.PropertyType.IsArray && property.PropertyType.Namespace == sourceType.Namespace)
                    {
                        var endPoint = buildPropertyValues(property.PropertyType, new Tuple<int, int>(row, col));
                        col = endPoint.Item2;
                    }
                    else
                    {
                        using (var cell = worksheet.Cells[row, col])
                        {
                            if (property.PropertyType.IsArray)
                            {
                                var arrayType = property.PropertyType.GetElementType();
                                if (arrayType != null)
                                {
                                    if (sourceKeyProperty != null)
                                    {
                                        if (arrayType == typeof(string))
                                        {
                                            cell.Formula = $"=COUNTIF(INDIRECT(\"{type.Name}.{property.Name}[{type.Name}.{sourceKeyProperty.Name}]\"), INDIRECT(\"[@{sourceKeyProperty.Name}]\"))";
                                        } else
                                        {
                                            cell.Formula = $"=COUNTIF(INDIRECT(\"{type.Name}.{arrayType.Name}[{type.Name}.{sourceKeyProperty.Name}]\"), INDIRECT(\"[@{sourceKeyProperty.Name}]\"))";
                                        }
                                    }
                                }
                            } else if (pseudoFKAttribute != null) {
                                AddDataListValidation(cell, $"=INDIRECT(\"{pseudoFKAttribute.ForeignType.Name}[{pseudoFKAttribute.ForeignKey}]\")");
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

        /// <summary>
        /// Adds a comment to the Excel range.
        /// </summary>
        /// <param name="range">Reference to the range for which the comment should be added.</param>
        /// <param name="comment">Display text for the comment.</param>
        /// <returns>Reference to the comment that was added. Returns null if the comment is empty.</returns>
        protected virtual ExcelComment? AddCellComment(ExcelRange range, string comment)
        {
            if (string.IsNullOrEmpty(comment)) return null;

            var cellComment = range.AddComment(comment, "Utilities.Games");
            cellComment.LockText = true;
            cellComment.AutoFit = true;
            cellComment.BackgroundColor = System.Drawing.Color.White;
            return cellComment;
        }

        /// <summary>
        /// Adds Data Validation to the provided Excel <paramref name="range"/> in the form of a list validation with the specified <paramref name="formula"/>
        /// </summary>
        /// <param name="range">Reference to the range for which the data validation should be added.</param>
        /// <param name="formula">Represents an Excel formula to be used for validating the contents of the specified <paramref name="range"/>.</param>
        /// <returns></returns>
        protected virtual IExcelDataValidationList AddDataListValidation(ExcelRange range, string formula)
        {
            var validation = range.DataValidation.AddListDataValidation();
            validation.AllowBlank = true;
            validation.Formula.ExcelFormula = formula;
            return validation;
        }

        /// <summary>
        /// Gets the first property within the provided <paramref name="type"/> that is decorated with a <see cref="KeyAttribute"/>.
        /// </summary>
        /// <param name="type">Reference to the type for which to search its properties.</param>
        /// <returns>Reference to the property that is decorated with the <see cref="KeyAttribute"/>. Returns null if not found.</returns>
        private PropertyInfo GetTypeKey(Type type) {
            var properties = type?.GetProperties();
            return properties?.FirstOrDefault(o => o.GetCustomAttribute<KeyAttribute>() != null);
        }

        /// <summary>
        /// Gets the XML documentation <c>&lt;summary /&gt;</c> for the given <paramref name="type"/>.
        /// </summary>
        /// <param name="type">Reference to the <see cref="Type"/> for which to search the XML documentation by.</param>
        /// <param name="extraPath">Override of which XML path to choose the return node.</param>
        /// <returns>The results of <see cref="XmlNode.SelectSingleNode"/> using the provided <paramref name="extraPath"/>.</returns>
        protected XmlNode GetTypeSummary(Type type, string extraPath = "summary") {
            return CodeSummary.SelectSingleNode($"//member[starts-with(@name, 'T:{type.FullName}')]{(string.IsNullOrEmpty(extraPath) ? "" : $"/{extraPath}")}");
        }

        /// <summary>
        /// Gets the XML documentation <c>&lt;summary /&gt;</c> for the given <paramref name="property"/>.
        /// </summary>
        /// <param name="property">Reference to the <see cref="PropertyInfo"/> for which to search the XML documentation by.</param>
        /// <param name="extraPath"><inheritdoc cref="GetTypeSummary" path="/param[@name='extraPath']"/></param>
        /// <returns><inheritdoc cref="GetTypeSummary" path="/returns"/></returns>
        protected XmlNode GetPropertySummary(PropertyInfo property, string extraPath = "summary") {
            return CodeSummary.SelectSingleNode($"//member[starts-with(@name, 'P:{property.DeclaringType.FullName}.{property.Name}')]{(string.IsNullOrEmpty(extraPath) ? "" : $"/{extraPath}")}");
        }

        /// <summary>
        /// Shortcut method for returning the properties of the given <paramref name="type"/>. This is in-case there needs to be special handling for which types to process.
        /// </summary>
        /// <param name="type">Reference to the <see cref="Type"/> for which to get properties.</param>
        /// <returns>Collection of <see cref="PropertyInfo"/> discovered within the provided <paramref name="type"/>.</returns>
        protected IEnumerable<PropertyInfo> GetProperties(Type type) => type.GetProperties();

        /// <summary>
        /// Represents a supplementary reference from one (source) object and it's property for which is enumerably referenced.
        /// </summary>
        public class SupplementalTableReference {
            /// <summary>
            /// Reference to the <see cref="SourceType"/>'s property that should have its own structured table constructed for it.
            /// </summary>
            public PropertyInfo TargetProperty { get; set; }
            
            /// <summary>
            /// Backwards reference to the original type that the <see cref="TargetProperty"/> is referenced by.
            /// </summary>
            public Type SourceType { get; set; }
        }
    }
}
