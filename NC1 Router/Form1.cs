using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NC1_Router
{
    public partial class Form1 : Form
    {
        string ksspath;
        string searchfolder;
        DSTVParser dstvparser = new DSTVParser();
        KSSParser kssparser = new KSSParser();
        public KSSFile kiss;
        public bool multiplyBoltQTYs = true;

        public Form1()
        {
            InitializeComponent();
        }

        private void SelectKSS(object sender, EventArgs e)
        {
            var result = this.openKSS.ShowDialog();
            if (result == DialogResult.OK)
            {
                ksspath = this.openKSS.FileName;
            }
            else
                return;
            kiss = kssparser.ParseKSS(ksspath, multiplyBoltQTYs);
            searchfolder = kiss.FindJobDSTVDirectory();
            this.btnParseDSTVs.Enabled = true;
        }

        public void ParseDSTVs(object sender, EventArgs e)
        {
            string[] keys = kiss.PartsDictionary.Keys.ToArray<string>();
            for (int i = 0; i < keys.Length; i++)
            {
                kiss.PartsDictionary[keys[i]] = dstvparser.ParseDSTV(kiss.PartsDictionary[keys[i]], searchfolder);
            }
            this.btnWriteKSS.Enabled = true;


            this.dgvSmallHoles.DataSource = dstvparser.smallHoles;
            this.dgvSmallHoles.Update();

            var dgvPLHalfDataSource = new List<PLHalfList>();
            foreach (string piecemark in kssparser.kssPLHalf)
            {
                var PLHalfLog = new PLHalfList { Piecemark = piecemark };
                dgvPLHalfDataSource.Add(PLHalfLog);
            }
            this.dgvPLHalf.DataSource = dgvPLHalfDataSource;
            this.dgvPLHalf.Update();

            this.dgvHeaderLengthChanges.DataSource = dstvparser.lengthchanges;
            this.dgvHeaderLengthChanges.Update();
        }
        private class PLHalfList
        {
            public string Piecemark { get; set; }
        }
        private void WriteKSS(object sender, EventArgs e)
        {
            var newkissfile = new StringBuilder();
            newkissfile.AppendLine(kiss.Heading);
            newkissfile.AppendLine($"H,{kiss.JobNumber},{kiss.JobDescription},,01/01/0001,rightnow,F,Final Bill");
            foreach (KeyValuePair<string, Part> entry in kiss.PartsDictionary)
            {
                if (entry.Value.IsMain == true)
                {
                    string routing = FormatRouting(entry.Value.RoutingCodes);
                    newkissfile.AppendLine($"D,{entry.Value.DrawingNumber},0,{entry.Value.PieceMark},,{entry.Value.Quantity},{entry.Value.ShapeType},{entry.Value.ShapeDetails},{entry.Value.Grade},{entry.Value.Length},Finish,{routing}");
                    foreach (Tuple<string,int> assemblymark in entry.Value.AssemblyMarks)
                    {
                        if (kiss.PartsDictionary[assemblymark.Item1].RoutingCodes.Count > 0)
                            routing = FormatRouting(kiss.PartsDictionary[assemblymark.Item1].RoutingCodes);
                        if (entry.Value.PieceMark.Contains("bolt"))
                            newkissfile.AppendLine($"D,{entry.Value.DrawingNumber},0,,{assemblymark.Item1},{assemblymark.Item2},{kiss.PartsDictionary[assemblymark.Item1].ShapeType},{kiss.PartsDictionary[assemblymark.Item1].ShapeDetails},{kiss.PartsDictionary[assemblymark.Item1].Grade},{kiss.PartsDictionary[assemblymark.Item1].Length},Finish,{routing}");
                        else
                            newkissfile.AppendLine($"D,{entry.Value.DrawingNumber},0,{entry.Value.PieceMark},{assemblymark.Item1},{assemblymark.Item2},{kiss.PartsDictionary[assemblymark.Item1].ShapeType},{kiss.PartsDictionary[assemblymark.Item1].ShapeDetails},{kiss.PartsDictionary[assemblymark.Item1].Grade},{kiss.PartsDictionary[assemblymark.Item1].Length},Finish,{routing}");
                    }
                    newkissfile.AppendLine($"S,{entry.Value.Sequence},{entry.Value.Quantity}");
                }
            }
            File.AppendAllText(Path.Combine(Path.GetDirectoryName(ksspath),"newkiss.kss"), newkissfile.ToString());
            this.lblComplete.Visible = true;
        }

        private string FormatRouting(List<string> routinglist)
        {
            string formatedrouting = "";
            if (routinglist.Contains("NO FILE"))
            {
                formatedrouting += "NO FILE";
                return formatedrouting.Trim();
            }
            if (routinglist.Contains("P") && routinglist.Contains("M"))
                formatedrouting += "MP";
            else if (routinglist.Contains("P"))
                formatedrouting += "P";
            else if (routinglist.Contains("M"))
                formatedrouting += "M";
            if (routinglist.Contains("F"))
                formatedrouting += "F ";
            else
                formatedrouting += " ";
            if (routinglist.Contains("BT"))
                formatedrouting += "BT ";
            if (routinglist.Contains("CO"))
                formatedrouting += "CO ";
            if (routinglist.Contains("BNT"))
                formatedrouting += "BNT ";
            if (routinglist.Contains("NS"))
                formatedrouting += "NS ";
            if (routinglist.Contains("ST"))
                formatedrouting += "ST ";
            if (formatedrouting == " " && !routinglist.Contains("bolt"))
                formatedrouting += "PM";
            if (routinglist.Contains("GAL"))
                formatedrouting += "GAL";
            formatedrouting = Regex.Replace(formatedrouting, "MP", "M P");
            return formatedrouting.Trim();

        }

        private void ToggleMutliplyBoltQTY(object sender, EventArgs e)
        {
            if (this.checkMultiplyBoltQuantities.CheckState == CheckState.Checked)
                multiplyBoltQTYs = true;
            else
                multiplyBoltQTYs = false;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
