using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DB_ODP;
using clDB;
using SelezioniObjects;
using System.IO;
using System.Data;


namespace SelezioniWebApp
{
    public partial class IstruzioniSF : System.Web.UI.Page
    {
        ClsDB DBUtility = new ClsDB();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["id"] != null)
            {
                MemoryStream filecontent = new MemoryStream();
                int nID = int.Parse(Request.QueryString["id"].ToString());
                System.Data.DataRow dr = DBUtility.LeggiFileIstruzioni(nID);

                byte[] data;
                //Dim data() As Byte
                if (dr[0].ToString() != "")
                {
                    data = (byte[])dr[0];
                    string ftype = dr[2].ToString().ToUpper();
                    Response.Buffer = false;
                    if (ftype == "PDF" || ftype == "PDF ")
                    {
                        Response.ContentType = "application/pdf";
                        Response.AddHeader("content-disposition", "inline;filename=tr.pdf");
                    }
                    else if (ftype == "DOC" || ftype == "DOC " || ftype == "DOCX")
                    {
                        Response.ContentType = "application/octet-stream";
                        Response.AddHeader("content-disposition", "inline;filename=tr"+ "." + ftype);                                          
                    }

                    else if (ftype == "HTML" || ftype == "HTM" || ftype == "HTM ")
                    {
                        Response.ContentType = "text/HTML";
                        Response.AddHeader("content-disposition", "inline;filename=tr.html");
                    }
                 
                    Context.Response.BinaryWrite(data);
                    Response.End();
                    Response.Flush(); 

                    /* ORI non funziona per i file docx
                    Response.OutputStream.Write(data, 0, data.Length - 2);
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.BinaryWrite(data);
                    Response.End();
                    Response.Flush();
                     */
                   
                }
            }
        }
    }
}