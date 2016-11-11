using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Xml;
using System.Text;

namespace SelezioniWebApp
{
    public class ClsUtility
    {
        private static List<string> lCtrl
        {
            get;
            set;
        }
        public List<string> FindControlByNamePart(Control ctrl, string id)
        {
            lCtrl = new List<string>();
            FindControlRecursive(ctrl, id);
            return lCtrl;
        }

        private static void FindControlRecursive(Control root, string id)
        {
            //lCtrl.Add(root.ID);
            if (root.ID != null && root.ID.IndexOf(id, 0) >= 0)
            {
                lCtrl.Add(root.ID);
            }

            foreach (Control c in root.Controls)
            {
                FindControlRecursive(c, id);
            }
        }

        public void crea_excel(DataTable pDtCompilazioni, int pSelId, System.IO.Stream strFileName)
        {
            int ncol = 0;
            int nrow = 0;
            // Create XMLWriter
            XmlTextWriter xtwWriter = new XmlTextWriter(strFileName, Encoding.UTF8);
            //Format the output file for reading easier
            xtwWriter.Formatting = Formatting.Indented;

            // <?xml version="1.0"?>
            xtwWriter.WriteStartDocument();

            // <?mso-application progid="Excel.Sheet"?>
            xtwWriter.WriteProcessingInstruction("mso-application", "progid=Excel.Sheet");

            // <Workbook xmlns="urn:schemas-microsoft-com:office:spreadsheet >"
            xtwWriter.WriteStartElement("Workbook", "urn:schemas-microsoft-com:office:spreadsheet");

            //Write definition of namespace
            xtwWriter.WriteAttributeString("xmlns", "o", null, "urn:schemas-microsoft-com:office:office");
            xtwWriter.WriteAttributeString("xmlns", "x", null, "urn:schemas-microsoft-com:office:excel");
            xtwWriter.WriteAttributeString("xmlns", "ss", null, "urn:schemas-microsoft-com:office:spreadsheet");
            xtwWriter.WriteAttributeString("xmlns", "html", null, "http:'www.w3.org/TR/REC-html40");

            //<DocumentProperties xmlns="urn:schemas-microsoft-com:office:office">
            xtwWriter.WriteStartElement("DocumentProperties", "urn:schemas-microsoft-com:office:office");

            //Write document properties
            xtwWriter.WriteElementString("Author", Environment.UserName);
            xtwWriter.WriteElementString("LastAuthor", Environment.UserName);
            xtwWriter.WriteElementString("Created", DateTime.Now.ToString("u") + "Z");
            xtwWriter.WriteElementString("Company", "Unknown");
            xtwWriter.WriteElementString("Version", "11.8122");

            //</DocumentProperties>
            xtwWriter.WriteEndElement();

            //<ExcelWorkbook xmlns="urn:schemas-microsoft-com:office:excel">
            xtwWriter.WriteStartElement("ExcelWorkbook", "urn:schemas-microsoft-com:office:excel");

            //Write settings of workbook
            xtwWriter.WriteElementString("WindowHeight", "13170");
            xtwWriter.WriteElementString("WindowWidth", "17580");
            xtwWriter.WriteElementString("WindowTopX", "120");
            xtwWriter.WriteElementString("WindowTopY", "60");
            xtwWriter.WriteElementString("ProtectStructure", "False");
            xtwWriter.WriteElementString("ProtectWindows", "False");

            //</ExcelWorkbook>
            xtwWriter.WriteEndElement();

            //<Styles>
            xtwWriter.WriteStartElement("Styles");

            //<Style ss:ID="Default" ss:Name="Normal">
            xtwWriter.WriteStartElement("Style");
            xtwWriter.WriteAttributeString("ss", "ID", null, "Default");
            xtwWriter.WriteAttributeString("ss", "Name", null, "Normal");
            //<Alignment ss:Vertical="Bottom"/>
            xtwWriter.WriteStartElement("Alignment");
            xtwWriter.WriteAttributeString("ss", "Vertical", null, "Bottom");
            xtwWriter.WriteEndElement();

            //Write null on the other properties
            xtwWriter.WriteElementString("Borders", null);
            xtwWriter.WriteElementString("Font", null);
            xtwWriter.WriteElementString("Interior", null);
            xtwWriter.WriteElementString("NumberFormat", null);
            xtwWriter.WriteElementString("Protection", null);

            //</Style>
            xtwWriter.WriteEndElement();
            // celle
            xtwWriter.WriteStartElement("Style");
            xtwWriter.WriteAttributeString("ss", "ID", null, "sscell");
            xtwWriter.WriteAttributeString("ss", "Name", null, "sscell");
            //<Alignment ss:Vertical="Bottom"/>
            xtwWriter.WriteStartElement("Alignment");
            xtwWriter.WriteAttributeString("ss", "Vertical", null, "Bottom");
            xtwWriter.WriteEndElement();

            xtwWriter.WriteStartElement("Borders");
            xtwWriter.WriteStartElement("Border");
            xtwWriter.WriteAttributeString("ss", "Position", null, "Left");
            xtwWriter.WriteAttributeString("ss", "LineStyle", null, "Continuous");
            xtwWriter.WriteAttributeString("ss", "Weight", null, "1");
            xtwWriter.WriteEndElement();
            xtwWriter.WriteStartElement("Border");
            xtwWriter.WriteAttributeString("ss", "Position", null, "Right");
            xtwWriter.WriteAttributeString("ss", "LineStyle", null, "Continuous");
            xtwWriter.WriteAttributeString("ss", "Weight", null, "1");
            xtwWriter.WriteEndElement();
            xtwWriter.WriteStartElement("Border");
            xtwWriter.WriteAttributeString("ss", "Position", null, "Top");
            xtwWriter.WriteAttributeString("ss", "LineStyle", null, "Continuous");
            xtwWriter.WriteAttributeString("ss", "Weight", null, "1");
            xtwWriter.WriteEndElement();
            xtwWriter.WriteStartElement("Border");
            xtwWriter.WriteAttributeString("ss", "Position", null, "Bottom");
            xtwWriter.WriteAttributeString("ss", "LineStyle", null, "Continuous");
            xtwWriter.WriteAttributeString("ss", "Weight", null, "1");
            xtwWriter.WriteEndElement();
            xtwWriter.WriteEndElement();//borders

            //Write null on the other properties
            xtwWriter.WriteElementString("Font", null);
            xtwWriter.WriteElementString("Interior", null);
            xtwWriter.WriteElementString("NumberFormat", null);
            xtwWriter.WriteElementString("Protection", null);

            //</Style>
            xtwWriter.WriteEndElement();
            ////intestazione colonne
            xtwWriter.WriteStartElement("Style");
            xtwWriter.WriteAttributeString("ss", "ID", null, "ssbold");
            xtwWriter.WriteAttributeString("ss", "Name", null, "ssbold");
            xtwWriter.WriteStartElement("Alignment");
            xtwWriter.WriteAttributeString("ss", "Vertical", null, "Bottom");
            xtwWriter.WriteEndElement(); //alignment
            //xtwWriter.WriteElementString("Borders", null);
            xtwWriter.WriteStartElement("Borders");
            xtwWriter.WriteStartElement("Border");
            xtwWriter.WriteAttributeString("ss", "Position", null, "Left");
            xtwWriter.WriteAttributeString("ss", "LineStyle", null, "Continuous");
            xtwWriter.WriteAttributeString("ss", "Weight", null, "1");
            xtwWriter.WriteEndElement();
            xtwWriter.WriteStartElement("Border");
            xtwWriter.WriteAttributeString("ss", "Position", null, "Right");
            xtwWriter.WriteAttributeString("ss", "LineStyle", null, "Continuous");
            xtwWriter.WriteAttributeString("ss", "Weight", null, "1");
            xtwWriter.WriteEndElement();
            xtwWriter.WriteStartElement("Border");
            xtwWriter.WriteAttributeString("ss", "Position", null, "Top");
            xtwWriter.WriteAttributeString("ss", "LineStyle", null, "Continuous");
            xtwWriter.WriteAttributeString("ss", "Weight", null, "1");
            xtwWriter.WriteEndElement();
            xtwWriter.WriteStartElement("Border");
            xtwWriter.WriteAttributeString("ss", "Position", null, "Bottom");
            xtwWriter.WriteAttributeString("ss", "LineStyle", null, "Continuous");
            xtwWriter.WriteAttributeString("ss", "Weight", null, "1");
            xtwWriter.WriteEndElement();
            xtwWriter.WriteEndElement();//borders
            xtwWriter.WriteStartElement("Font");
            xtwWriter.WriteAttributeString("ss", "Bold", null, "1");
            xtwWriter.WriteEndElement(); //end Font
            //xtwWriter.WriteElementString("Interior", null);
            xtwWriter.WriteStartElement("Interior");
            xtwWriter.WriteAttributeString("ss", "Color", null, "#D9D9D9");
            xtwWriter.WriteAttributeString("ss", "Pattern", null, "Solid");
            xtwWriter.WriteEndElement(); //end Interior
            xtwWriter.WriteElementString("NumberFormat", null);
            xtwWriter.WriteElementString("Protection", null);

            //        </Style>
            xtwWriter.WriteEndElement();
            // fine intestazione colonne

            //</Styles>
            xtwWriter.WriteEndElement();
           
            // scrittura intestazione colonne
            //dt1 = dtSource.Tables[2];
            xtwWriter.WriteStartElement("Worksheet");
            xtwWriter.WriteAttributeString("ss", "Name", null, "EstrazioneCompilazioni");
            // compilazioni
            ncol = pDtCompilazioni.Columns.Count;
            xtwWriter.WriteStartElement("Table");
            nrow = pDtCompilazioni.Rows.Count + 1;
            
            xtwWriter.WriteAttributeString("ss", "ExpandedColumnCount", null, ncol.ToString());
            xtwWriter.WriteAttributeString("ss", "ExpandedRowCount", null, nrow.ToString());
            xtwWriter.WriteAttributeString("x", "FullColumns", null, "1");
            xtwWriter.WriteAttributeString("x", "FullRows", null, "1");
            xtwWriter.WriteAttributeString("ss", "DefaultColumnWidth", null, "60");
            
            // intestazione colonne
            xtwWriter.WriteStartElement("Row");
            foreach (DataColumn dc in pDtCompilazioni.Columns)
            {
                xtwWriter.WriteStartElement("Cell");
                xtwWriter.WriteAttributeString("ss", "StyleID", null, "ssbold");
                //         <Data ss:Type="String">xxx</Data>
                xtwWriter.WriteStartElement("Data");
                xtwWriter.WriteAttributeString("ss", "Type", null, "String");
                xtwWriter.WriteValue(dc.ColumnName.ToString());
                xtwWriter.WriteEndElement();
                //         </Cell>
                xtwWriter.WriteEndElement();
            }
            xtwWriter.WriteEndElement();
            // fine intestazione colonne

            foreach (DataRow row in pDtCompilazioni.Rows)
            {
                xtwWriter.WriteStartElement("Row");
                //     Run through all cell of current rows

                foreach (Object cellValue in row.ItemArray)
                {
                        //         <Cell>
                        xtwWriter.WriteStartElement("Cell");
                        //xtwWriter.WriteAttributeString("ss", "StyleID", null, "ssbold");
                        xtwWriter.WriteAttributeString("ss", "StyleID", null, "sscell");
                        xtwWriter.WriteStartElement("Data");
                        xtwWriter.WriteAttributeString("ss", "Type", null, "String");

                        //         Write content of cell
                        if (cellValue != DBNull.Value)
                        {
                            xtwWriter.WriteValue(cellValue);
                        }

                        //         </Data>
                        xtwWriter.WriteEndElement();

                        //         </Cell>
                        xtwWriter.WriteEndElement();
                }
                xtwWriter.WriteEndElement();  //row
            }
            //</Row>
            xtwWriter.WriteEndElement();
            // </Table>
            xtwWriter.WriteEndElement();
            // Write file on hard disk
            xtwWriter.Flush();
            xtwWriter.Close();
        }
    }
}