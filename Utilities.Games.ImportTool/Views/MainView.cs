using ConsoulLibrary;
using ConsoulLibrary.Views;

namespace Utilities.Games.ImportTool.Views
{
    public class MainView : StaticView
    {
        public MainView()
        {
            Title = (new BannerEntry("Utilities.Games Import Tool")).Message;
        }

        [ViewOption("Subsites")]
        public void ChooseSubsites() {
            var view = new SubsitesView();
            view.Run();
        }
    }
}
