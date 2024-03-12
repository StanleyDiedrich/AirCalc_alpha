using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace AirCalc_alpha
{
   
        [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
        public class Command : IExternalCommand
        {
            static AddInId AddInId = new AddInId(new Guid("648A5B92-70F8-48B2-A21A-8C5489A36CE3"));

            public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
            {
                UIDocument uidoc = commandData.Application.ActiveUIDocument;
                Document document = uidoc.Document;
                Form1 window = new Form1(document);
                window.ShowDialog();
                
                

                return Result.Succeeded;
            }
        }
    
}
