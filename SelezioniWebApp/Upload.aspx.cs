using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace SelezioniWebApp
{
    public partial class Upload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            FileInfo fi = new FileInfo(Server.MapPath("/SelezioniPersonale/DocumentiPeo/"));
            lbl_elencofile.Text = "<strong>" + "Lista file presenti" + "</strong>" + "<br>" + "<br>";
            if (Request.Url.AbsoluteUri.Substring(0, 16) == "https://apps.uni")
            {
               // fi = new FileInfo(Server.MapPath("/SelezioniPersonale/DocumentiPeo/"));
                fi = new FileInfo(Server.MapPath("~/App_Data/"));
            }
            else if (Request.Url.AbsoluteUri.Substring(0, 16) == "http://webtest02")
            {
               // fi = new FileInfo(Server.MapPath("/SelezioniPersonale/DocumentiPeo/"));
                fi = new FileInfo(Server.MapPath("~/App_Data/"));
            }           
            else
            {
               // fi = new FileInfo(Server.MapPath("/DocumentiPeo/"));
                fi = new FileInfo(Server.MapPath("~/App_Data/"));
            }
           // this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + fi.Directory  + "');", true);
            DirectoryInfo di = fi.Directory;
          //  lbl_messaggi.Text = "Directory " + fi.Directory + " - " + di.Exists + " - " + " - " + di.FullName + " - " + di.Name + " - " + di.Parent.Name + " - " + di.Root.Name;
          //  this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + di.Exists + " - " + di.Name + " - " + di.Parent.Name + " - " + di.Root.Name + "');", true);
            FileSystemInfo[] fsi = di.GetFiles();
            //Response.Write("Lista file e directory:" + di.FullName + "<hr>");
            foreach (FileSystemInfo info in fsi)
            {
                if (info.Extension != ".MDF" && info.Extension != ".ldf")
                {
                    //Response.Write(info.Name + "<br>");
                    lbl_elencofile.Text = lbl_elencofile.Text + info.Name + "<br>";
                }
            }


        }
        protected void mnu_main_MenuItemClick(object sender, MenuEventArgs e)
        {
            if (e.Item.Value == "Upl")
            {
                carica_file();
            }
            else if (e.Item.Value == "Del")
            {
                delete_file();
            }
            else if (e.Item.Value == "Esc")
            {
                Page.Response.Redirect("Gestione.aspx");
            }


        }
        private void carica_file()
        {
            string fileName = fuFile.PostedFile.FileName;

            fileName = fileName.Substring(fileName.LastIndexOf("\\") + 1);
            string fileExtension = System.IO.Path.GetExtension(fileName);
            string fileMimeType = fuFile.PostedFile.ContentType;
            int fileLengthInKB = fuFile.PostedFile.ContentLength / 1024;

            string[] matchExtension = { ".docx", ".doc", ".html", ".pdf", ".txt" };
            //string[] matchMimeType = { "image/jpeg", "image/png", "image/gif" };

            if (fuFile.HasFile)
            {

                // if (matchExtension.Contains(fileExtension) && matchMimeType.Contains(fileMimeType))
                if (matchExtension.Contains(fileExtension))
                {
                    if (fileLengthInKB <= 1024)
                    {
                        //Server.MapPath("/foo/bar.txt");
                        if (Request.Url.AbsoluteUri.Substring(0, 16) == "https://apps.uni")
                        {
                            //fuFile.SaveAs(Server.MapPath("/SelezioniPersonale/DocumentiPeo/" + fileName));
                            fuFile.SaveAs(Server.MapPath("~/App_Data/" + fileName));
                        }
                        else if (Request.Url.AbsoluteUri.Substring(0, 16) == "http://webtest02")
                        {
                            //fuFile.SaveAs(Server.MapPath("/SelezioniPersonale/DocumentiPeo/" + fileName));
                            fuFile.SaveAs(Server.MapPath("~/App_Data/" + fileName));
                        }
                        else
                        {
                            //fuFile.SaveAs(Server.MapPath("/DocumentiPeo/" + fileName));
                            fuFile.SaveAs(Server.MapPath("~/App_Data/" + fileName));
                        }
                        //fuFile.SaveAs(Server.MapPath("/DocumentiPeo/" + fileName));
                        // lbl_messaggi.Text = "File Uploaded Successfully";
                        this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + "File caricato con successo" + "');", true);
                        Page.Response.Redirect("Upload.aspx", true);
                    }
                    else
                    {
                        //lbl_messaggi.Text = "Il file deve essere più piccolo di 1 Mb";
                        this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + "Il file deve essere più piccolo di 1 Mb" + "');", true);
                    }

                }
                else
                {
                    //lbl_messaggi.Text = "Il file può essere docx, doc, html, pdf";
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + "Il file può essere docx, doc, html, pdf" + "');", true);

                }
            }
            else
            {
                //lbl_messaggi.Text = "Seleziona un file";
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + "Seleziona un file" + "');", true);

            }
        }

        private void delete_file()
        {
            string fName = txt_ins_deletefile.Text;
            if (fName != "")
            {
                string curFile;
                if (Request.Url.AbsoluteUri.Substring(0, 16) == "https://apps.uni")
                {
                    //curFile = Server.MapPath("/SelezioniPersonale/DocumentiPeo/") + fName;
                    curFile = Server.MapPath("~/App_Data/") + fName;
                }
                else if (Request.Url.AbsoluteUri.Substring(0, 16) == "http://webtest02")
                {
                    //curFile = Server.MapPath("/SelezioniPersonale/DocumentiPeo/") + fName;
                    curFile = Server.MapPath("~/App_Data/") + fName;
                }
                else
                {
                    //curFile = Server.MapPath("/DocumentiPeo/") + fName;
                    curFile = Server.MapPath("~/App_Data/") + fName;
                }
                //string curFile = Server.MapPath("/DocumentiPeo/") + fName;
                //Console.WriteLine(File.Exists(curFile) ? "File exists." : "File does not exist.");
                if (File.Exists(curFile))
                {
                    txt_ins_deletefile.Text = "";
//                    File.Delete((Server.MapPath("/DocumentiPeo/" + fName)));
                    File.Delete(curFile);
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + "File cancellato con successo" + "');", true);
                    Page.Response.Redirect("Upload.aspx");
                }
                else
                {
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + "Il file non esiste" + "');", true);
                }
            }
            else
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + "Inserisci il nome del file da cancellare" + "');", true);
        }
    }
}