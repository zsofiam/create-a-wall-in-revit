using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace DrawWall
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Class1 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Get application and documnet objects
            UIApplication uiapp = commandData.Application;
            Document doc = uiapp.ActiveUIDocument.Document;
            
            //Define a reference Object to accept the pick result
            Transaction trans = new Transaction(doc);
            trans.Start("Wall");
            CreateWallUsingCurve1(doc, Level.Create(doc, 1));
            trans.Commit();

            return Result.Succeeded;
        }
        public Wall CreateWallUsingCurve1(Document document, Level level)
        {
            // Build a location line for the wall creation
            XYZ start = new XYZ(0, 0, 0);
            XYZ end = new XYZ(10, 10, 0);
            Line geomLine = Line.CreateBound(start, end);
            
            // Create a wall using the location line
            return Wall.Create(document, geomLine, level.Id, true);
        }
    }

}