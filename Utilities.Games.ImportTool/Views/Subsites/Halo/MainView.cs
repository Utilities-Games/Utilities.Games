using ConsoulLibrary.Views;
using System;
using System.Collections.Generic;
using Utilities.Games.Models.Subsites.Halo;

namespace Utilities.Games.ImportTool.Views.Subsites.Halo
{
    public class MainView : SubsiteView<MainView.Model>
    {
        public MainView() : base("Halo")
        {
        }

        [DynamicViewOption(nameof(_saveDataModelWorkbook_message), nameof(_saveDataModelWorkbook_color))]
        public override void SaveDataModelWorkbook()
        {
            base.SaveDataModelWorkbook();
        }

        protected override IEnumerable<Type> InitializeDatasetTypes()
        {
            return new List<Type>
            {
                typeof(Installment),
            };
        }

        public class Model : ISubsiteViewModel
        {
            public string Title { get; set; }

            public IEnumerable<Type> DatasetTypes { get; set; }
        }
    }
}
