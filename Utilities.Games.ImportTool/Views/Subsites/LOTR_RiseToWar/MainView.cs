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
                typeof(AttackMethod),
                typeof(BuildingType),
                typeof(Commander),
                typeof(CommanderClass),
                typeof(CommanderSkill),
                typeof(EquipmentItem),
                typeof(Faction),
                typeof(ItemType),
                typeof(RaceType),
                typeof(RingPowerLevel),
                typeof(RingSkill),
                typeof(RingSkillCategory),
                typeof(SignificantStructure),
                typeof(Skill),
                typeof(UnitType),
            };
        }

        public class Model : ISubsiteViewModel
        {
            public string Title { get; set; }

            public IEnumerable<Type> DatasetTypes { get; set; }
        }
    }
}
