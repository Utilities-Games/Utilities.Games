using ConsoulLibrary;
using ConsoulLibrary.Views;
using System;
using System.Collections.Generic;
using Utilities.Games.Models.Subsites.LOTR_RiseToWar;

namespace Utilities.Games.ImportTool.Views.Subsites.LOTR_RiseToWar
{
    public class MainView : SubsiteView<MainView.Model>
    {
        public MainView() : base("The Lord of the Rings: Rise to War")
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
                typeof(AlignmentType),
                typeof(Commander),
                typeof(CommanderClass),
                typeof(Faction),
                typeof(Race),
                typeof(Skill),
            };
        }

        public class Model : ISubsiteViewModel
        {
            public string Title { get; set; }

            public IEnumerable<Type> DatasetTypes { get; set; }
        }
    }
}
