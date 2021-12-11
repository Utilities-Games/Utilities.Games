using ConsoulLibrary;
using ConsoulLibrary.Views;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utilities.Games.ImportTool.DataModelFactories.Excel;

namespace Utilities.Games.ImportTool.Views.Subsites
{
    public abstract class SubsiteView<T> : DynamicView<T> where T : SubsiteView<T>.ISubsiteViewModel, new()
    {
        public SubsiteView(string gameName)
        {
            Title = (new BannerEntry($"{gameName} - Import Tool")).Message;
            Source = new();
            Source.Title = (new BannerEntry($"{gameName} - Import Tool")).Message;
            Source.DatasetTypes = InitializeDatasetTypes();
        }

        protected abstract IEnumerable<Type> InitializeDatasetTypes();

        protected string _saveDataModelWorkbook_message() => "Save Data Model Template Workbook";
        protected ConsoleColor _saveDataModelWorkbook_color() => ConsoleColor.Green;
        [DynamicViewOption(nameof(_saveDataModelWorkbook_message), nameof(_saveDataModelWorkbook_color))]
        public virtual void SaveDataModelWorkbook() {
            Consoul.Write("Constructing package...");
            var fi = new FileInfo($"Utilities-Games Data Model Workbook.xlsx");
            using (var excelUtility = new ExcelUtility(Source.DatasetTypes.ToArray())) {
                excelUtility.Package.SaveAs(fi);
            }
            Consoul.Write("Done!", ConsoleColor.Green);
            //using (var package = new ExcelPackage())
            //{
            //    foreach (var datasetType in Source.DatasetTypes)
            //    {
            //        var typeSheet = new ExcelUtility(datasetType);
            //        typeSheet.BuildWorksheet(package);
            //    }
            //    Consoul.Write("Saving...", writeLine: false);
            //    package.SaveAs(fi);
            //}
            //Consoul.Write("Done!", ConsoleColor.Green);
            Consoul.Write("Saved to\r\n\t" + fi.FullName, ConsoleColor.Green);
            Consoul.Wait();

        }

        /// <summary>
        /// Generic definition of a new Consoul view for a Subsite
        /// </summary>
        public interface ISubsiteViewModel {
            /// <summary>
            /// Reference to the view title/banner.
            /// </summary>
            string Title { get; set; }

            /// <summary>
            /// Collection of data model types that can be processed into an Excel workbook for easy manual entry.
            /// </summary>
            IEnumerable<Type> DatasetTypes { get; set; }
        }
    }
}
