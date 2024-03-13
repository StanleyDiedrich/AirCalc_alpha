using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
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
                            return startElement;
                        }
                    }

                }
            }
            return startElement;
        }
        public string GetSystemType (Element startelement)
        {
            string systemtype = "";
            if (startelement != null)
            {
                FamilyInstance fI = startelement as FamilyInstance;
                MEPModel mepModel = fI.MEPModel;
                ConnectorSet connectorSet = mepModel.ConnectorManager.Connectors;
                foreach (Connector connector in connectorSet)
                {
                    systemtype = connector.DuctSystemType.ToString();

                }

               
            }
            return systemtype;
        }
        public ElementId FindNextElement(Document doc, ElementId elementId, List<ElementId> foundedelements, string systemtype)
        {

            ElementId ownerId = elementId;
            Element element = doc.GetElement(ownerId);
            Element foundedElement = null;
            MEPModel mepModel = null;
            ConnectorSet connectorSet = null;
            ElementId foundedelementId = null;
            double maxvolume = 0;
            try
            {
                
                    if (element is FamilyInstance)
                    {
                        FamilyInstance FI = element as FamilyInstance;
                        mepModel = FI.MEPModel;
                        connectorSet = mepModel.ConnectorManager.Connectors;

                    }
                    if (element is Duct)
                    {
                        Duct pipe = element as Duct;
                        connectorSet = pipe.ConnectorManager.Connectors;
                    }

                foreach (Connector connector in connectorSet)
                {
                    ConnectorSet nextconnectors = connector.AllRefs;
                    foreach (Connector nextconnector in nextconnectors)
                    {

                        if (doc.GetElement(nextconnector.Owner.Id) is MechanicalSystem)
                        {

                            continue;
                        }
                        else if (nextconnector.Owner.Id == ownerId)
                        {

                            continue;
                        }



                        else
                        {
                             
                            
                             if (systemtype == "SupplyAir")
                            {
                                if (nextconnector.Direction is FlowDirectionType.Out )
                                {
                                    foundedelementId =  nextconnector.Owner.Id;
                                }
                               else  if (nextconnector.Direction is FlowDirectionType.Bidirectional)
                                {
                                    if (!nextconnector.Owner.Id.Equals(ownerId) || !foundedelements.Contains(nextconnector.Owner.Id))
                                    {
                                        foundedelementId = nextconnector.Owner.Id;
                                    }
                                    else
                                    { continue; }
                                   
                                }

                            }
                            
                            else
                            {
                                if (nextconnector.Direction is FlowDirectionType.In)
                                {
                                    foundedelementId = nextconnector.Owner.Id;
                                }
                            }
                            

                        }







                    }
                }



            }
            catch
            {
                TaskDialog.Show("Error", $"{element.Id} не отработал");

            }
            
            return foundedelementId;





          




        }
        public bool IsThreeWay(Document doc, ElementId elementId)
        {

            Element element = doc.GetElement(elementId);
            bool isthreeway = false;
            if (element is FamilyInstance)
            {
                if (element is FamilyInstance)
                {
                    FamilyInstance fI = element as FamilyInstance;
                    MEPModel mepMoidel = fI.MEPModel;
                    ConnectorSet connectorSet = mepMoidel.ConnectorManager.Connectors;
                    List<Connector> connectors = new List<Connector>();
                    foreach (Connector connector in connectorSet)
                    {
                        connectors.Add(connector);
                    }
                    if (connectors.Count >= 3)
                    {
                        isthreeway = true;
                    }
                    elementId = element.Id;
                }
                if (element is Duct)
                {
                    Duct fI = element as Duct;

                    ConnectorSet connectorSet = fI.ConnectorManager.Connectors;
                    List<Connector> connectors = new List<Connector>();
                    foreach (Connector connector in connectorSet)
                    {
                        connectors.Add(connector);
                    }
                    if (connectors.Count >= 3)
                    {
                        isthreeway = true;
                    }
                    elementId = element.Id;
                }
            }
            return isthreeway;
        }


        public ElementId GetDirection(Document doc, ElementId elementId, string selectedsystem)
        {
            ElementId selectedpipe = null;
            List<ElementId> pipes = new List<ElementId>();

            Element element = doc.GetElement(elementId);
            if (element != null)
            {
                if (element is FamilyInstance)
                {
                    FamilyInstance familyInstance = element as FamilyInstance;
                    MEPModel mepmodel = familyInstance.MEPModel;
                    ConnectorSet connectorSet = mepmodel.ConnectorManager.Connectors;
                    foreach (Connector connector in connectorSet)
                    {
                        ConnectorSet nextconnectorset = connector.AllRefs;
                        foreach (Connector nextconnector in nextconnectorset)
                        {
                            if (!nextconnector.Owner.Name.Equals(selectedsystem))
                            {
                                ElementId nextelementId = nextconnector.Owner.Id;
                                pipes.Add(nextelementId);
                            }    
                            

                        }
                    }
                }
                if (element is Duct)
                {
                    Duct duct = (Duct) element;
                    MEPCurve mepcurve = duct as MEPCurve;
                    ConnectorSet connectorSet = mepcurve.ConnectorManager.Connectors;
                    foreach (Connector connector in connectorSet)
                    {
                        ConnectorSet nextconnectorset = connector.AllRefs;
                        foreach (Connector nextconnector in nextconnectorset)
                        {
                            if (!nextconnector.Owner.Name.Equals(selectedsystem))
                            {
                                ElementId nextelementId = nextconnector.Owner.Id;
                                pipes.Add(nextelementId);
                            }

                        }
                    }

                }
                
            }

            double maxVolume = 0;
            foreach (ElementId pipe in pipes)
            {

                Element el = doc.GetElement(pipe);
                Duct pipe1 = el as Duct;
                if (pipe1 != null)
                {
                    foreach (Parameter parameter in pipe1.Parameters)
                    {
                        if (parameter.Definition.Name == "Расход")
                        {
                            double volume = parameter.AsDouble();
                            if (volume > maxVolume)
                            {
                                maxVolume = volume;
                                selectedpipe = pipe1.Id;
                            }
                        }

                    }

                }
            }
            return selectedpipe;
        }

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            Form1 window = new Form1(doc);
            window.ShowDialog();

            SelectedSystem selectedsystem = new SelectedSystem();
            selectedsystem.selectedSystem = window._selectedSystem.selectedSystem;
            Element selectedelement = FindStartConnector(doc, selectedsystem.selectedSystem);
            ElementId elementId = selectedelement.Id;
            List<ElementId> foundedelements = new List<ElementId>();
            foundedelements.Add(elementId);
            string systemtype = GetSystemType(selectedelement);
            var foundedelement = FindNextElement(doc, selectedelement.Id, foundedelements, systemtype);
            foundedelements.Add(foundedelement);




            int index = foundedelements.Count - 1;
            ElementId nextelement = null;

            ElementId f = null;
            string name = "";
            int counter = 0;
            try
            {
                do
                {
                    
                    nextelement = foundedelements.Last();
  
                        f = FindNextElement(doc, nextelement, foundedelements, systemtype);
                        if (f != null)
                        {


                            if (!foundedelements.Contains(f))
                            {

                                if (f != nextelement)
                                {
                                    foundedelements.Add(f);
                                }
                                else
                                {
                                    continue;
                                }
                            }



                        }
                        else
                        {
                            break;
                           
                        }
                    


                    
                   

                }
                while ( f!= nextelement || f ==null);
                //TaskDialog.Show("Res", selectedelement.Id.ToString());


            }
            catch (Exception ex)
            {

            }
            /* string text = "";

             foreach (var foundedelement2 in foundedelements)
             {
                 string a = $"{foundedelement2}  \n";
                 text += a;
             }
             TaskDialog.Show("R", text);*/







            foreach (var foundedelement2 in foundedelements)
            {
                
                if (foundedelement2 != null)
                {
                    Element element = doc.GetElement(foundedelement2);
                    if (element is FamilyInstance)
                    {
                        FamilyInstance familyInstance = element as FamilyInstance;

                        using (Transaction t = new Transaction(doc, "MainBranch"))
                        {
                            try
                            {
                                t.Start();
                                familyInstance.LookupParameter("ADSK_Примечание").Set("MainWay");
                                t.Commit();
                            }
                            catch (Exception ex)
                            {
                                continue;
                            }
                        }
                    }
                    if (element is Duct)
                    {
                        Duct familyInstance = element as Duct;

                        using (Transaction t = new Transaction(doc, "MainBranch"))
                        {
                            try
                            {
                                t.Start();
                                familyInstance.LookupParameter("ADSK_Примечание").Set("MainWay");
                                t.Commit();
                            }
                            catch (Exception ex)
                            {
                                continue;
                            }
                        }
                    }
                }
                
                
            }
            return Result.Succeeded;
        }

         
    }


}  

