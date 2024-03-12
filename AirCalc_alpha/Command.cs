using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
            private SelectedSystem SelectedSystem { get; set; }
        public Element FindStartConnector(Document doc, string selectedsystem)
        {
            Element startElement = null;
            IList<Element> airterminals = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_DuctTerminal).WhereElementIsNotElementType().ToElements();
            foreach (Element airterminal in airterminals)
            {
                FamilyInstance familyInstance = airterminal as FamilyInstance;
                string start = "1";
                if (familyInstance != null)
                {
                    if (familyInstance.LookupParameter("Имя системы").AsString() == selectedsystem)
                    {
                        string check = familyInstance.LookupParameter("Старт_расчета").AsString();
                        if (check == start)
                        {
                            startElement = airterminal;
                        }
                    }
                    
                }
            }
            return startElement;
        }

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
            {
                UIDocument uidoc = commandData.Application.ActiveUIDocument;
                Document document = uidoc.Document;
                Form1 window = new Form1(document);
                window.ShowDialog();
                
                SelectedSystem selectedsystem = new SelectedSystem();
                selectedsystem.selectedSystem = window._selectedSystem.selectedSystem;
                Element selectedelement = FindStartConnector(document, selectedsystem.selectedSystem);
                TaskDialog.Show("Res", selectedelement.Id.ToString());

                return Result.Succeeded;
            }
        }
    
}
