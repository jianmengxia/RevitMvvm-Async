using Revit.Async;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using Autodesk.Windows;
using System;
using System.Windows.Interop;

namespace RevitAsyncTest
{
    [Transaction(TransactionMode.Manual)]
    public class RevitAynacTestCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            RevitTask.Initialize(commandData.Application);
            IntPtr rvtHandle = commandData.Application.MainWindowHandle;

            MainView view = new MainView();
            WindowInteropHelper window = new WindowInteropHelper(view);
            window.Owner = rvtHandle;
            view.Show();

            return Result.Succeeded;
        }
    }
}
