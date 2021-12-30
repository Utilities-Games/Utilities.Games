using ConsoulLibrary;
using ConsoulLibrary.Views;

namespace Utilities.Games.ImportTool.Views
{
    public class SubsitesView : StaticView {
        public SubsitesView()
        {
            Title = (new BannerEntry("Subsites - Utilities.Games Import Tool")).Message;
        }

        [ViewOption("The Lord of the Rings: Rise to War")]
        public void LOTR_RiseToWar(){
            var view = new Subsites.LOTR_RiseToWar.MainView();
            view.Run();
        }

        [ViewOption("The Legend of Zelda")]
        public void TheLegendOfZelda()
        {
            var view = new Subsites.TheLegendOfZelda.MainView();
            view.Run();
        }
    }
}
