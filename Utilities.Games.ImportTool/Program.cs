using System;
using ConsoulLibrary;

namespace Utilities.Games.ImportTool
{
    class Program
    {
        static void Main(string[] args)
        {
            Routines.MonitorInputs = true;
            
            var view = new Views.MainView();
            view.Run();

            // Save inputs to local routine file
            var routine = new XmlRoutine();
            routine.SaveInputs("Last Inputs.xml");
        }
    }
}
