using ConsoulLibrary.Views;
using System;
using System.Collections.Generic;
using Utilities.Games.Models.Subsites.TheLegendOfZelda;

namespace Utilities.Games.ImportTool.Views.Subsites.TheLegendOfZelda
{
    public class MainView : SubsiteView<MainView.Model>
    {
        public MainView() : base("The Legend of Zelda")
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
                typeof(Race),
                typeof(IngredientEffect),
                typeof(Ingredient),
                typeof(Enemy)
            };
        }

        public class Model : ISubsiteViewModel
        {
            public string Title { get; set; }

            public IEnumerable<Type> DatasetTypes { get; set; }
        }
    }
}
