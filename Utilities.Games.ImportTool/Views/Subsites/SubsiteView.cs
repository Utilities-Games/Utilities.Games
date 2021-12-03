using ConsoulLibrary;
using ConsoulLibrary.Views;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
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
            var package = new ExcelPackage();
            foreach (var datasetType in Source.DatasetTypes)
            {
                Consoul.Write($"\t{datasetType.Name}...", writeLine: false);
                var typeSheet = new TypeSheet(datasetType);
                typeSheet.BuildWorksheet(package);
                Consoul.Write("Done!", ConsoleColor.Green);
            }
            var fi = new FileInfo($"Utilities-Games Data Model Workbook.xlsx");
            Consoul.Write("Saving...", writeLine: false);
            package.SaveAs(fi);
            Consoul.Write("Done!", ConsoleColor.Green);
            Consoul.Write("Saved to\r\n\t" + fi.FullName, ConsoleColor.Green);
            Consoul.Wait();

        }

        public interface ISubsiteViewModel {
            string Title { get; set; }

            IEnumerable<Type> DatasetTypes { get; set; }
        }
    }
}
