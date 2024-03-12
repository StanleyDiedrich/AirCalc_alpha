using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI;

namespace AirCalc_alpha
{
    public partial class Form1 : System.Windows.Forms.Form
    {
        Document Doc;
        public SelectedSystem _selectedSystem { get; set; }
        public Form1(Document document)
        {
            Doc = document;
            InitializeComponent();
            

        }
        
        private void systembox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedSystem SelectedSystem = new SelectedSystem();
            _selectedSystem = SelectedSystem;
            IList<string> systemnames = new List<string>();
            IList<Element> ducts = new FilteredElementCollector(Doc).OfCategory(BuiltInCategory.OST_DuctCurves).WhereElementIsNotElementType().ToElements();
            foreach (Element duct in ducts)
            {
                var newduct = duct as Duct;
                var fi = duct as MEPCurve;
                string system = fi.MEPSystem.Name;
                systemnames.Add(system);
            }
            string selectedsystem = systembox.SelectedItem.ToString();
            
            foreach (var system in systemnames)
            {
                if (system == selectedsystem)
                {

                     _selectedSystem.selectedSystem = selectedsystem;
                    
                    
                }
            }
            
        }

        private void calc_button_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            IList<Element> ducts = new FilteredElementCollector(Doc).OfCategory(BuiltInCategory.OST_DuctCurves).WhereElementIsNotElementType().ToElements();

            try
            {
                IList<string> systemnames = new List<string>();
                foreach (Element duct in ducts)
                {
                    var newduct = duct as Duct;
                    var fi = duct as MEPCurve;
                    string system = fi.MEPSystem.Name;
                    systemnames.Add(system);
                }
                systemnames = systemnames.Distinct().ToList();
                /*string text = "";
                foreach (string systemname in systemnames)
                {
                    string a = systemname;
                    text += a;
                }
                TaskDialog.Show("R", text);*/
                foreach (var system in systemnames)
                {
                    systembox.Items.Add(system);
                }
                
                //
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", ex.ToString());
            }

        }
    }
}
