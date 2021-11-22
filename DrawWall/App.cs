using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace DrawWall
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class App : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            Document doc = uiapp.ActiveUIDocument.Document;
            Selection sel = uiapp.ActiveUIDocument.Selection;
            TaskDialog mainDialog = new TaskDialog("Please pick points to draw a wall!");
            mainDialog.Show();

            try
            {
                XYZ startingPoint = sel.PickPoint("Please pick a point to start wall");
                XYZ endPoint = sel.PickPoint("Please pick a point to end wall");

                Transaction trans = new Transaction(doc);
                trans.Start("Drawing Wall");
                CreateWall(doc, Level.Create(doc, 1), startingPoint, endPoint);
                trans.Commit();
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                return Result.Cancelled;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Result.Failed;
            }

            return Result.Succeeded;
        }
        public Wall CreateWall(Document document, Level level, XYZ start, XYZ end)
        {
            Line straightLine = Line.CreateBound(start, end);
            
            return Wall.Create(document, straightLine, level.Id, true);
        }
    }

}