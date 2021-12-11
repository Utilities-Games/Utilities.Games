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
    public class ExcelUtility : IDisposable
    {
        /// <summary>
        /// Defines the number of columns between each table for separation.
        /// </summary>
        protected const int SpaceBetweenTables = 3;

        private List<TypeTable> _typeTables = new List<TypeTable>();
        /// <summary>
        /// Queue of recursive object references that ultimately need separate format tables constructed to manage the data.
        /// </summary>
        public IEnumerable<TypeTable> SourceTypeTables => _typeTables;

        protected IList<(Type, ExcelTableAddress)> FormattedTables { get; set; } = new List<(Type, ExcelTableAddress)>();

        /// <summary>
        /// Reference to the XML documentation file for the <see cref="Utilities.Games.Models"/> project
        /// </summary>
        protected IDictionary<Type, XmlDocument> CodeSummaries { get; set; } = new Dictionary<Type, XmlDocument>();

        /// <summary>
        /// Reference to the the Excel workbook package.
        /// </summary>
        public ExcelPackage Package { get; private set; }

        public ExcelUtility(params Type[] sourceTypes)
        {
            foreach (var sourceType in sourceTypes)
            {
                InitializeTableStructureFromType(sourceType);
            }

            Package = new ExcelPackage();
            foreach (var table in SourceTypeTables)
            {
                var sheet = BuildWorksheet(Package, table);
            }
        }

        /// <summary>
        /// Constructs a new <see cref="TypeTable"/> based on the provided <paramref name="sourceType"/>.
        /// </summary>
        /// <param name="sourceType">Reference to the primary <see cref="Type"/> the <see cref="TypeTable"/> should be constructed from.</param>
        protected virtual void InitializeTableStructureFromType(Type sourceType)
        {
            // Internal recursive method for constructing header columns based on property types. This will flatten properties into columns.
            TypeTable getTypeTableFromType(Type type, TypeTable parentTable = null)
            {
                TypeTable table = new TypeTable(type);

                var properties = GetProperties(type);
                foreach (var property in properties)
                {
                    PropertyHeader header = null;
                    // If the property is not a simple array and within the namespace of the Utilities.Games.Models, then flatten the property.
                    if (property.PropertyType.Namespace.StartsWith(type.Namespace))
                    {
                        if (!property.PropertyType.IsArray)
                        {
                            getTypeTableFromType(property.PropertyType, table);
                        }
                        else
                        {
                            var arrayType = property.PropertyType.GetElementType();
                            header = AddHeader(table, arrayType, property);
                        }
                    }
                    else
                    {
                        header = AddHeader(table, type, property);
                    }

                    if (header != null) {
                        table.AddHeader(header);
                    }
                }

                return table;


                PropertyHeader AddHeader(TypeTable table, Type type, PropertyInfo property)
                {
                    PropertyHeader header = new PropertyHeader(table, property);

                    // If array, then dedicate this column as a reference to another structured table
                    if (property.PropertyType.IsArray)
                    {
                        Type supplementalType;
                        if ((supplementalType = property.PropertyType.GetElementType()) != null)
                        {
                            if (supplementalType == typeof(string) || supplementalType.Namespace == type.Namespace)
                            {
                                var subTable = getTypeTableFromType(supplementalType, table);
                                table.AddSupplementalTable(property, subTable);
                                //table.AddSupplementalType(supplementalType, property);
                            }
                        }
                    }

                    return header;
                }
            }

            var sourceTable = getTypeTableFromType(sourceType);
            if (sourceTable != null) {
                _typeTables.Add(sourceTable);

                var xmlFilePath = sourceType.Assembly.Location.Replace(".dll", ".xml");
                if (xmlFilePath != null)
                {
                    var codeSummary = new XmlDocument();
                    codeSummary.Load(xmlFilePath);
                    CodeSummaries.Add(sourceType, codeSummary);
                }
            }
        }

        /// <summary>
        /// Constructs a new <see cref="ExcelWorksheet"/> based on the definition of the <see cref="SourceType"/>.
        /// </summary>
        /// <param name="package">Reference to the root <see cref="ExcelPackage"/> for which the <see cref="ExcelWorksheet"/> will be added.</param>
        /// <returns>Reference to the newly constructed <see cref="ExcelWorksheet"/>.</returns>
        protected virtual ExcelWorksheet BuildWorksheet(ExcelPackage package, TypeTable sourceTable)
        {
            int startRow = 1, startCol = 1;
            int row = startRow, col = startCol;
            var sheet = package.Workbook.Worksheets.Add(sourceTable.SourceType.Name);

            // Build table header for SourceType
            var headerEndPoint = BuildHeader(sheet, sourceTable.SourceType, row, col);
            // Build table body placeholder for SourceType
            var bodyEndPoint = BuildBody(sheet, sourceTable.SourceType, headerEndPoint.Item1 + 1, col);
            // Build and format table
            var tableAddress = new ExcelAddressBase(headerEndPoint.Item1, col, bodyEndPoint.Item1, bodyEndPoint.Item2);
            var table = sheet.Tables.Add(tableAddress, sourceTable.SourceType.Name);
            AddSupplementalTables(sourceTable, row, ref col, sheet, ref headerEndPoint, ref bodyEndPoint);

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

            void AddSupplementalTables(TypeTable sourceTable, int row, ref int col, ExcelWorksheet sheet, ref (int, int) headerEndPoint, ref (int, int) bodyEndPoint)
            {
                // Add supplemental table sources to the primary source type
                // For example, a simple string array on the source type can be defined in a separate format table and linked back to the parent object's row.
                if (sourceTable.SupplementalTables.Any())
                {
                    foreach (var supplementalType in sourceTable.SupplementalTables)
                    {
                        var remoteTable = FormattedTables.FirstOrDefault(o => o.Item1 == supplementalType.Item1);
                        if (remoteTable != default)
                        {
                            // Update any properties to point to this supplemental table
                            var sourceProperty = sourceTable.Headers.FirstOrDefault(o => o.Property == supplementalType.Item1);
                            if (sourceProperty != null)
                            {
                                sourceProperty.RemoteReference = new CrossTableReference()
                                {
                                    ReferenceHeader = new PropertyHeader(supplementalType.Item2, supplementalType.Item1),
                                    ReferenceTable = null
                                };
                            }
                        }

                        PropertyInfo sourceKeyProperty = GetTypeKey(supplementalType.Item1.DeclaringType);

                        Type enumerationType = supplementalType.Item1.PropertyType.GetElementType();
                        if (enumerationType != null)
                        {
                            // Add space between last table and this one
                            col = headerEndPoint.Item2 + (SpaceBetweenTables + 1);

                            // Prepare summary for supplementary entity table
                            string supplementarySummary = null;
                            if (enumerationType == typeof(string))
                            {
                                supplementarySummary = GetPropertySummary(supplementalType.Item1)?.InnerText;
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
                                supplementarySourceKeyHeaderCell.Value = supplementalType.Item1.DeclaringType.Name;

                                if (sourceKeyProperty != null)
                                {
                                    supplementarySourceKeyHeaderCell.Value += "." + sourceKeyProperty.Name;

                                    using (var supplementarySourceKeyValueCell = sheet.Cells[bodyEndPoint.Item1, col])
                                    {
                                        AddDataListValidation(supplementarySourceKeyValueCell, $"=INDIRECT(\"{supplementalType.Item1.DeclaringType.Name}[{sourceKeyProperty.Name}]\")");
                                    }
                                }
                                // This results in corrupt file which Excel repairs by removing corrupt comments.
                                //AddCellComment(supplementarySourceKeyHeaderCell, $"Link {(enumerationType == typeof(string) ? secondaryInfo.Name : enumerationType.Name)} to the parent {SourceType.Name} by selecting it in the dropdown.");
                            }

                            // Create structured table
                            tableAddress = new ExcelAddressBase(headerEndPoint.Item1, col, bodyEndPoint.Item1, bodyEndPoint.Item2);
                            sheet.Tables.Add(tableAddress, $"{supplementalType.Item1.DeclaringType.Name}.{(enumerationType == typeof(string) ? supplementalType.Item1.Name : enumerationType.Name)}");
                        }

                        if (supplementalType.Item2.SupplementalTables.Any())
                        {
                            AddSupplementalTables(supplementalType.Item2, row, ref col, sheet, ref headerEndPoint, ref bodyEndPoint);
                        }
                    }
                }
            }
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

            Consoul.Write($"\r\n\t{sourceType.Name}...", writeLine: false);
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
                        if (property.PropertyType.Namespace.StartsWith(sourceType.Namespace))
                        {
                            if (!property.PropertyType.IsArray)
                            {
                                var endPoint = buildPropertyHeaders(property.PropertyType, (row, col), formattedPrefix + property.Name);
                                col = endPoint.Item2;
                            }
                            else
                            {
                                var arrayType = property.PropertyType.GetElementType();
                                AddHeaderColumn(worksheet, arrayType, formattedPrefix, row, col, property);
                            }
                        }
                        else
                        {
                            AddHeaderColumn(worksheet, sourceType, formattedPrefix, row, col, property);
                        }

                        // TODO: Ensure that a column was added before attempting to increment the column position
                        col++;
                    }
                    col--;


                    return (row, col);

                    bool TryFormatArrayType(Type sourceType, PropertyInfo property, ref string label)
                    {
                        // If array, then dedicate this column as a reference to another structured table
                        if (property.PropertyType.IsArray)
                        {
                            label += " Count";
                            //Type secondaryType;
                            //if ((secondaryType = property.PropertyType.GetElementType()) != null)
                            //{
                            //    if (secondaryType == typeof(string) || secondaryType.Namespace == sourceType.Namespace)
                            //    {
                            //        SupplementalPropertyTypes.Enqueue(new SupplementalTableReference { TargetProperty = property, SourceType = sourceType, Prefix = formattedPrefix });
                            //    }
                            //}
                            return true;
                        }

                        return false;
                    }

                    void AddHeaderColumn(ExcelWorksheet worksheet, Type sourceType, string formattedPrefix, int row, int col, PropertyInfo property)
                    {
                        // Create a table header label based on the available prefix and the current property name.
                        string label = formattedPrefix + property.Name;

                        using (var cell = worksheet.Cells[row, col])
                        {
                            TryFormatArrayType(sourceType, property, ref label);
                            Consoul.Write($"\r\n\t\t{label}...", writeLine: false);

                            cell.Value = label;

                            var xPropertySummary = GetPropertySummary(property);
                            if (xPropertySummary != null)
                            {
                                AddCellComment(cell, xPropertySummary.InnerText.Trim());
                            }
                        }

                        Consoul.Write("Done!", ConsoleColor.Green);
                    }
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
            Consoul.Write($"\r\n\t\tDone!", ConsoleColor.Green, writeLine: false);

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

            (int, int) buildPropertyValues(Type type, (int, int) startPoint)
            {
                PropertyInfo sourceKeyProperty = GetTypeKey(type);
                int row = startPoint.Item1, col = startPoint.Item2;

                var properties = GetProperties(type);
                foreach (var property in properties)
                {
                    PseudoForeignKeyAttribute pseudoFKAttribute = property.GetCustomAttribute<PseudoForeignKeyAttribute>();

                    if (!property.PropertyType.IsArray && property.PropertyType.Namespace.StartsWith(sourceType.Namespace))
                    {
                        var endPoint = buildPropertyValues(property.PropertyType, (row, col));
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
                                        string remoteTableName = $"{type.Name}.";
                                        string remoteTableColumnName = $"{type.Name}.{sourceKeyProperty.Name}";
                                        if (arrayType == typeof(string))
                                        {
                                            remoteTableName += property.Name;
                                            cell.Formula = $"=COUNTIF(INDIRECT(\"{remoteTableName}[{remoteTableColumnName}]\"), INDIRECT(\"{type.Name}[@{sourceKeyProperty.Name}]\"))";
                                        } else
                                        {
                                            remoteTableName += arrayType.Name;
                                            cell.Formula = $"=COUNTIF(INDIRECT(\"{remoteTableName}[{remoteTableColumnName}]\"), INDIRECT(\"{type.Name}[@{sourceKeyProperty.Name}]\"))";
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


                return (row, col);
            }
            var endPoint = buildPropertyValues(sourceType, (row, col));
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
            return CodeSummaries
                .Select(o => o.Value
                    .SelectSingleNode($"//member[starts-with(@name, 'T:{type.FullName}')]{(string.IsNullOrEmpty(extraPath) ? "" : $"/{extraPath}")}")
                )
                .FirstOrDefault(o => o != null);
        }

        /// <summary>
        /// Gets the XML documentation <c>&lt;summary /&gt;</c> for the given <paramref name="property"/>.
        /// </summary>
        /// <param name="property">Reference to the <see cref="PropertyInfo"/> for which to search the XML documentation by.</param>
        /// <param name="extraPath"><inheritdoc cref="GetTypeSummary" path="/param[@name='extraPath']"/></param>
        /// <returns><inheritdoc cref="GetTypeSummary" path="/returns"/></returns>
        protected XmlNode GetPropertySummary(PropertyInfo property, string extraPath = "summary") {
            return CodeSummaries
                .Select(o => o.Value
                    .SelectSingleNode($"//member[starts-with(@name, 'P:{property.DeclaringType.FullName}.{property.Name}')]{(string.IsNullOrEmpty(extraPath) ? "" : $"/{extraPath}")}")
                )
                .FirstOrDefault();
        }

        /// <summary>
        /// Shortcut method for returning the properties of the given <paramref name="type"/>. This is in-case there needs to be special handling for which types to process.
        /// </summary>
        /// <param name="type">Reference to the <see cref="Type"/> for which to get properties.</param>
        /// <returns>Collection of <see cref="PropertyInfo"/> discovered within the provided <paramref name="type"/>.</returns>
        protected IEnumerable<PropertyInfo> GetProperties(Type type) => type.GetProperties();

        public void Dispose()
        {
            Package?.Dispose();
        }

        /// <summary>
        /// Represents a supplementary reference from one (source) object and it's property for which is enumerably referenced.
        /// </summary>
        //public class SupplementalTableReference {
        //    /// <summary>
        //    /// Reference to the <see cref="SourceType"/>'s property that should have its own structured table constructed for it.
        //    /// </summary>
        //    public PropertyInfo TargetProperty { get; set; }

        //    /// <summary>
        //    /// Backwards reference to the original type that the <see cref="TargetProperty"/> is referenced by.
        //    /// </summary>
        //    public Type SourceType { get; set; }

        //    /// <summary>
        //    /// Raw prefix for the supplemental type. This should help keep tables unique for re-usable classes.
        //    /// </summary>
        //    public string Prefix { get; set; }
        //}

        public class TypeTable
        {
            /// <summary>
            /// Reference tot he source table type.
            /// </summary>
            public Type SourceType { get; }

            private List<PropertyHeader> _headers = new List<PropertyHeader>();
            /// <summary>
            /// Collection of property-related headers for this table.
            /// </summary>
            public IEnumerable<PropertyHeader> Headers => _headers;

            private List<(PropertyInfo, TypeTable)> _supplementalTables = new List<(PropertyInfo, TypeTable)>();
            public IEnumerable<(PropertyInfo, TypeTable)> SupplementalTables => _supplementalTables;

            public TypeTable(Type sourceType)
            {
                SourceType = sourceType;
            }

            /// <summary>
            /// Adds the property header to the list of headers.
            /// </summary>
            /// <param name="header">Reference to a property header to add to this table.</param>
            public void AddHeader(PropertyHeader header) => _headers.Add(header);

            public void AddSupplementalTable(PropertyInfo sourceProperty, TypeTable supplementalTable) => _supplementalTables.Add((sourceProperty, supplementalTable));
        }

        public class PropertyHeader {
            /// <summary>
            /// Reference to the parent <see cref="TypeTable"/>.
            /// </summary>
            public TypeTable Table { get; }

            /// <summary>
            ///  Reference to the source entity's property associated with this header.
            /// </summary>
            public PropertyInfo Property { get; }

            /// <summary>
            /// Display text for this table column header.
            /// </summary>
            public string Label { get; set; }

            /// <summary>
            /// Reference to a remote table header.
            /// </summary>
            public CrossTableReference RemoteReference { get; set; }

            public PropertyHeader(TypeTable table, PropertyInfo property)
            {
                Table = table;
                Property = property;
            }
        }

        /// <summary>
        /// Defines a relationship to another table.
        /// </summary>
        public class CrossTableReference {
            /// <summary>
            /// Reference to the source table
            /// </summary>
            public TypeTable ReferenceTable { get; set; }

            /// <summary>
            /// Reference to the source header of the ReferenceTable.
            /// </summary>
            public PropertyHeader ReferenceHeader { get; set; }

            public CrossTableReference() { }
        }
    }
}
