using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using Prism.Commands;
using Prism.Mvvm;
using Revit.Async;

namespace RevitAsyncTest
{
    public class MainViewModel : BindableBase
    {
        public DelegateCommand DrawWallCommand => new DelegateCommand(async () =>
        {
            await RevitTask.RunAsync(app =>
            {
                var modelLine = app.ActiveUIDocument.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element);
                DetailLine curve = app.ActiveUIDocument.Document.GetElement(modelLine.ElementId) as DetailLine;
                using (Transaction trans = new Transaction(app.ActiveUIDocument.Document,"CreateWall"))
                {
                    try
                    {
                        trans.Start();
                        Wall.Create(app.ActiveUIDocument.Document, curve.GeometryCurve, app.ActiveUIDocument.Document.ActiveView.GenLevel.Id, false);
                        app.ActiveUIDocument.Document.Delete(modelLine.ElementId);
                        trans.Commit();
                    }
                    catch(Exception ex) 
                    {
                        if(trans.HasStarted())
                            trans.RollBack();
                        MessageBox.Show(ex.Message);
                    }
                }
            });
        });
        public DelegateCommand DeleteWallCommand => new DelegateCommand(() =>
        {
            RevitTask.RunAsync(app =>
            {
                var wallRef = app.ActiveUIDocument.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element);
                var wall = app.ActiveUIDocument.Document.GetElement(wallRef.ElementId) as Wall;
                using (Transaction trans = new Transaction(app.ActiveUIDocument.Document, "DeleteWall"))
                {
                    try
                    {
                        trans.Start();
                        app.ActiveUIDocument.Document.Create.NewDetailCurve(app.ActiveUIDocument.Document.ActiveView, (wall.Location as LocationCurve).Curve);
                        app.ActiveUIDocument.Document.Delete(wallRef.ElementId);
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        if (trans.HasStarted())
                            trans.RollBack();
                        MessageBox.Show(ex.Message);
                    }
                }
            });
        });
        public DelegateCommand ModifyWallCommand => new DelegateCommand(() =>
        {
            RevitTask.RunAsync(app =>
            {
                var wall = app.ActiveUIDocument.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element);
                using (Transaction trans = new Transaction(app.ActiveUIDocument.Document, "MoveWall"))
                {
                    try
                    {
                        trans.Start();
                        ElementTransformUtils.MoveElement(app.ActiveUIDocument.Document, wall.ElementId, new XYZ(0, 10, 0));
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        if (trans.HasStarted())
                            trans.RollBack();
                        MessageBox.Show(ex.Message);
                    }
                }
            });
        });
        public DelegateCommand LookupWallCommand => new DelegateCommand(() =>
        {
            RevitTask.RunAsync(app =>
            {
                var wallRef = app.ActiveUIDocument.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element);
                var  wall = app.ActiveUIDocument.Document.GetElement(wallRef.ElementId) as Wall;
                using (Transaction trans = new Transaction(app.ActiveUIDocument.Document, "LookWall"))
                {
                    try
                    {
                        trans.Start();
                        MessageBox.Show(wall.Name);
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        if (trans.HasStarted())
                            trans.RollBack();
                        MessageBox.Show(ex.Message);
                    }
                }
            });
        });



    }
}
