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
    public partial class ShowFile : System.Web.UI.Page
    {
        ClsDB DBUtility = new ClsDB();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Request.QueryString["ftype"] != null)
            {
                DataRow dr = (DataRow)Session["file"];
                byte[] data;
                //Dim data() As Byte
                if (dr[0].ToString() != "")
                {
                    data = (byte[])dr[0];
                    string ftype = Request.QueryString["ftype"].ToUpper();
                    Response.Buffer = false;
                    if (ftype == "PDF")
                    {
                        Response.ContentType = "application/pdf";
                        Response.AddHeader("content-disposition", "inline;filename=tr.pdf");
                    }

                    Response.OutputStream.Write(data, 0, data.Length);
                    //Response.Charset = "";
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.BinaryWrite(data);
                    Response.End();
                }
                else
                {
                    Page.Response.Redirect("Risposte.aspx");
                }
            }
        }
    }
}