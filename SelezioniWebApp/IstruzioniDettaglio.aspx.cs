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

namespace SelezioniWebApp
{
    public partial class IstruzioniDettaglio : System.Web.UI.Page
    {
        ClsDB DBUtility = new ClsDB();

        ClsUtility Utility = new ClsUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                string pID = "";
                if (Request.QueryString["id"] != null)
                {
                    pID = Request.QueryString["id"].ToString();
                    Session["id"] = pID;
                  
                    dtv_dettaglio.DataSource = DBUtility.CaricaDettaglioIstruzioni(int.Parse(pID.ToString()));
                    dtv_dettaglio.DataBind();
                    if (dtv_dettaglio.CurrentMode == DetailsViewMode.ReadOnly)
                    {
                        int NRow = 0;
                        NRow = dtv_dettaglio.Rows.Count - 1;
                        //((System.Web.UI.WebControls.LinkButton)dtv_dettaglio.Rows[NRow].Cells[0].Controls[2]).Text = "Cancel";
                        ((System.Web.UI.WebControls.LinkButton)dtv_dettaglio.Rows[NRow].Cells[0].Controls[2]).Text = "Ritorna";
                        dtv_dettaglio.FindControl("lblVis").Visible = true;
                        dtv_dettaglio.FindControl("lblIns").Visible = false;
                        dtv_dettaglio.FindControl("lblUpd").Visible = false;
                        /*
                        if (dtv_dettaglio.DataItem[Bytes_curr] != 0)
                        {
                            dtv_dettaglio.FindControl("ImageButton1").Visible = true;
                            //dtv_dettaglio.FindControl("blb_chb_del").Visible = true;
                            dtv_dettaglio.FindControl("lblCaricatoInizio").Visible = true;
                            dtv_dettaglio.FindControl("lblCaricatoFine").Visible = true;
                            dtv_dettaglio.FindControl("lblData").Visible = true;

                        }
                        else
                        {
                            dtv_dettaglio.FindControl("ImageButton1").Visible = false;
                        }
                         */
                    }
                    else if (dtv_dettaglio.CurrentMode == DetailsViewMode.Edit)
                    {
                    }
                }
                if (Request.QueryString["ope"] == "new")
                {
                    int index = 0;
                    dtv_dettaglio.ChangeMode(DetailsViewMode.Insert);
                    DropDownList ddl_categoria_cod = (DropDownList)dtv_dettaglio.FindControl("ddl_ins_categoria_cod");
                    ddl_categoria_cod.DataSource = DBUtility.PopolaDDLCategoria();
                    ddl_categoria_cod.DataValueField = "categoria";
                    ddl_categoria_cod.DataTextField = "des_categoria";
                    ddl_categoria_cod.DataBind();
                }
            }
            if (dtv_dettaglio.CurrentMode == (DetailsViewMode.Edit))
            {
            }

        }
        protected void dtv_dettaglio_DataBind()
        {
        }
        protected void dtv_dettaglio_DataBound(object sender, EventArgs e)
        {

        }
        protected void dtv_dettaglio_ItemCommand(object sender, DetailsViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Edit"))
            {
                string pID = "";
                pID = (string)Session["id"];

                dtv_dettaglio.ChangeMode(DetailsViewMode.Edit);
                dtv_dettaglio.DataSource = DBUtility.CaricaDettaglioIstruzioni(int.Parse(pID.ToString()));
                dtv_dettaglio.DataBind();

                DropDownList ddl_categoria_cod = (DropDownList)dtv_dettaglio.FindControl("ddl_upd_categoria_cod");
                ddl_categoria_cod.DataSource = DBUtility.PopolaDDLCategoria();
                ddl_categoria_cod.DataValueField = "categoria";
                ddl_categoria_cod.DataTextField = "des_categoria";
                ddl_categoria_cod.DataBind();
                ddl_categoria_cod.SelectedValue = DBUtility.CaricaDettaglioIstruzioni(int.Parse(pID.ToString())).Rows[0]["categoria_cod"].ToString();

                dtv_dettaglio.FindControl("lblUpd").Visible = true;
                dtv_dettaglio.FindControl("lblVis").Visible = false;


            }
            else if (e.CommandName.Equals("Visualizza"))
            {
                int nID = int.Parse((string)Session["id"]);
                string url = "IstruzioniSF.aspx?id=" + nID;
                Page.Response.Redirect(url);
            }
        }
        protected void dtv_dettaglio_ModeChanging1(object sender, DetailsViewModeEventArgs e)
        {
            /*
            TextBox File = (TextBox)dtv_dettaglio.FindControl("blb_upd");
            //FileUpload FileUpload1 = (FileUpload)dtv_dettaglio.FindControl("blb_upd");
            if (File.Text == "")
            {
                dtv_dettaglio.FindControl("blb_chb_del").Visible = false;
            }
             * */
        }
        protected void dtv_dettaglio_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
        {
            string strem = "";
            // controlli campo blob
            int intDocLen = 0;
            Stream objStream;
            TextBox txtExt_al = (TextBox)dtv_dettaglio.FindControl("blb_upd_ext_d");
            TextBox txtBtye_al = (TextBox)dtv_dettaglio.FindControl("blb_upd_bytes_d");

            CheckBox chk_del_curr = (CheckBox)dtv_dettaglio.FindControl("blb_chb_del");
            FileUpload FileUpload1 = (FileUpload)dtv_dettaglio.FindControl("blb_upd");
            try
            {
                if (FileUpload1.FileName != "")
                {
                    string extension = Path.GetExtension(FileUpload1.PostedFile.FileName).ToLower();

                    Byte imageBytes = (Byte)FileUpload1.PostedFile.InputStream.Length;
                    intDocLen = (int)FileUpload1.PostedFile.InputStream.Length;
                    byte[] Docbuffer = new byte[intDocLen];
                    //if (!chk_del_curr.Checked)
                    //{
                    if (FileUpload1.PostedFile.FileName != "")
                    {
                        if (FileUpload1.PostedFile.ToString() == "" || FileUpload1.PostedFile.FileName.ToString() == "" || FileUpload1.PostedFile.InputStream.ToString() == "")
                        {
                            strem = "Upload fallito";
                        }
                        // ........................................ verifica tipo file.........................
                        txtExt_al.Text = "";
                        switch (extension.ToLower())
                        {
                            case ".pdf":
                            case ".doc":
                            case ".docx":
                            case ".html":
                            case ".htm":
                                txtExt_al.Text = extension.Substring(1, extension.Length - 1);
                                break;
                            default:
                                strem = "Tipo file errato";

                                break;
                        }
                        if (strem == "Tipo file errato")
                        {
                            strem = "";
                            throw new ArgumentException("Il file può essere di tipo: pdf, doc,docx,htm,html");
                        }
                        if (FileUpload1.PostedFile.InputStream.Length > 1000000)
                        {
                            strem = "Dimensione del File troppo grande";
                            throw new ArgumentException("La dimensione del file è troppo grande");
                        }

                        //load FileUploaded input stream into byte array
                        txtBtye_al.Text = imageBytes.ToString();
                        //txtBtye_al.Text = imageBytes.Length;
                        objStream = FileUpload1.PostedFile.InputStream;
                        //FileUpload1.PostedFile.InputStream.Read((byte)imageBytes.ToString(), 0, int.Parse(imageBytes.ToString()));
                        objStream.Read(Docbuffer, 0, intDocLen);
                    }
                    else
                    {
                        txtExt_al.Text = "";
                        txtBtye_al.Text = "";
                    }
                    //}
                }
                //else
                //{
                //    if (chk_del_curr.Checked)
                //    {
                //        txtExt_al.Text = "";
                //        txtBtye_al.Text = "";
                //    }
                //}


                // fine controlli blob

                try
                {
                    Dictionary<string, object> coll = new Dictionary<string, object>();



                    DropDownList ddl_categoria_cod = (DropDownList)dtv_dettaglio.FindControl("ddl_upd_categoria_cod");
                    string sCategoria_cod = "";
                    sCategoria_cod = ddl_categoria_cod.SelectedValue;
                    coll.Add("categoria_cod", sCategoria_cod);

                    TextBox anno = (TextBox)dtv_dettaglio.FindControl("txt_anno");
                    string sAnno = "";
                    sAnno = anno.Text;
                    sAnno = Server.HtmlEncode(sAnno);
                    if (sAnno != "")
                    {
                        coll.Add("anno", sAnno);
                    }

                    TextBox descrizione = (TextBox)dtv_dettaglio.FindControl("txt_descrizione");
                    string sDescrizione = "";
                    sDescrizione = descrizione.Text;
                    sDescrizione = HttpUtility.HtmlEncode(sDescrizione);
                    if (sDescrizione != "")
                    {
                        coll.Add("descrizione", sDescrizione);
                    }

                    TextBox titolo = (TextBox)dtv_dettaglio.FindControl("txt_titolo");
                    string sTitolo = "";
                    sTitolo = titolo.Text;
                    sTitolo = HttpUtility.HtmlEncode(sTitolo);
                    if (sTitolo == "")
                    {
                        sTitolo = FileUpload1.PostedFile.FileName.ToString();

                    }
                    coll.Add("titolo", sTitolo);
                    //if (!chk_del_curr.Checked)
                    //{
                    if (FileUpload1 != null)
                    {
                        FileUpload blob = (FileUpload)dtv_dettaglio.FindControl("blb_upd");
                        coll.Add("istruzioni", blob.FileBytes);

                        TextBox bytes_curr = (TextBox)dtv_dettaglio.FindControl("blb_upd_bytes_d");
                        string sBytes_curr = "";
                        sBytes_curr = bytes_curr.Text;
                        coll.Add("bytes", blob.PostedFile.InputStream.Length);

                        TextBox ext_curr = (TextBox)dtv_dettaglio.FindControl("blb_upd_ext_d");
                        string sExt_curr = "";
                        sExt_curr = ext_curr.Text;
                        coll.Add("ext", sExt_curr);

                        coll.Add("titolo_file", HttpUtility.HtmlEncode(blob.PostedFile.FileName.ToString()));



                    }
                    //}
                    //else
                    //{
                    //    coll.Add("ext", "");
                    //    coll.Add("bytes", "");
                    //    coll.Add("istruzioni", "");
                    //}

                    Dictionary<String, Object> coll_w = new Dictionary<String, Object>();
                    //coll_w.Add("id", int.Parse(Session["id"].ToString()));
                    Label id = (Label)dtv_dettaglio.FindControl("lbl_upd_id");
                    string sId = "";
                    sId = id.Text;
                    if (sId != "")
                    {
                        coll_w.Add("id", sId);
                    }
                    else
                    {
                        strem = "Errore aggiornamento";
                        throw new ArgumentException("Impossibile aggiornare il file istruzioni");
                    }
                    DBUtility.upd_FileIstruzioni(coll, coll_w);

                    Response.Redirect("Istruzioni.aspx");
                }
                catch (Exception ex)
                {
                    if (ex.Message == "Il file può essere di tipo: pdf, doc,docx,html,htm")
                    {
                        MyMdlDlg.Show(MyControls.myModalDialog.MessageType.Error, "Errore", ex.Message, 100, 300, 60, 20);
                    }
                    else if (ex.Message == "La dimensione del file è troppo grande")
                    {
                        MyMdlDlg.Show(MyControls.myModalDialog.MessageType.Error, "Errore", ex.Message, 100, 300, 60, 20);
                    }
                    else if (ex.Message == "Impossibile aggiornare il file istruzioni")
                    {
                        MyMdlDlg.Show(MyControls.myModalDialog.MessageType.Error, "Errore", ex.Message, 100, 300, 60, 20);
                    }
                    else
                    {
                        MyMdlDlg.Show(MyControls.myModalDialog.MessageType.Error, "Errore", "Aggiornamento non riuscito. Contattare I.S.I.", 100, 300, 60, 20);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "Il file può essere di tipo: pdf, doc,docx,html,htm")
                {
                    MyMdlDlg.Show(MyControls.myModalDialog.MessageType.Error, "Errore", ex.Message, 100, 300, 60, 20);
                }
                else if (ex.Message == "La dimensione del file è troppo grande")
                {
                    MyMdlDlg.Show(MyControls.myModalDialog.MessageType.Error, "Errore", ex.Message, 100, 300, 60, 20);
                }
                else if (ex.Message == "Impossibile aggiornare il file istruzioni")
                {
                    MyMdlDlg.Show(MyControls.myModalDialog.MessageType.Error, "Errore", ex.Message, 100, 300, 60, 20);
                }
                else
                {
                    MyMdlDlg.Show(MyControls.myModalDialog.MessageType.Error, "Errore", "Aggiornamento non riuscito. Contattare I.S.I.", 100, 300, 60, 20);
                }
            }
        }

        protected void dtv_dettaglio_ItemInserting(object sender, DetailsViewInsertEventArgs e)
        {
            string strem = "";
            // controlli campo blob
            int intDocLen = 0;
            Stream objStream;

            //CheckBox chk_del_curr = (CheckBox)dtv_dettaglio.FindControl("blb_chb_del");
            FileUpload FileUpload1 = (FileUpload)dtv_dettaglio.FindControl("blb_ins");
            //if (FileUpload1 != null)
            try
            {
                if (FileUpload1.HasFile)
                {
                    string extension = Path.GetExtension(FileUpload1.PostedFile.FileName).ToLower();
                    TextBox txtExt_al = (TextBox)dtv_dettaglio.FindControl("blb_ins_ext_d");
                    Byte imageBytes = (Byte)FileUpload1.PostedFile.InputStream.Length;
                    intDocLen = (int)FileUpload1.PostedFile.InputStream.Length;
                    byte[] Docbuffer = new byte[intDocLen];
                    TextBox txtBtye_al = (TextBox)dtv_dettaglio.FindControl("blb_ins_bytes_d");

                    //if (!chk_del_curr.Checked)
                    //{
                    if (FileUpload1.PostedFile.FileName != "")
                    {
                        if (FileUpload1.PostedFile.ToString() == "" || FileUpload1.PostedFile.FileName.ToString() == "" || FileUpload1.PostedFile.InputStream.ToString() == "")
                        {
                            strem = "Upload fallito";
                        }
                        // ........................................ verifica tipo file.........................
                        txtExt_al.Text = "";
                        switch (extension.ToLower())
                        {
                            case ".pdf":
                            case ".doc":
                            case ".docx":
                            case ".html":
                            case ".htm":
                                txtExt_al.Text = extension.Substring(1, extension.Length - 1);
                                break;
                            default:
                                strem = "Tipo file errato";

                                break;
                        }
                        if (strem == "Tipo file errato")
                        {
                            strem = "";
                            throw new ArgumentException("Il file può essere di tipo: pdf, doc,docx,htm,html");
                        }
                        if (FileUpload1.PostedFile.InputStream.Length > 1000000)
                        {
                            strem = "Dimensione del File troppo grande";
                            throw new ArgumentException("La dimensione del file è troppo grande");
                        }

                        //load FileUploaded input stream into byte array
                        txtBtye_al.Text = imageBytes.ToString();
                        //txtBtye_al.Text = imageBytes.Length;
                        objStream = FileUpload1.PostedFile.InputStream;
                        //FileUpload1.PostedFile.InputStream.Read((byte)imageBytes.ToString(), 0, int.Parse(imageBytes.ToString()));
                        objStream.Read(Docbuffer, 0, intDocLen);
                    }
                }
                // fine controlli blob
                try
                {
                    Dictionary<string, object> coll = new Dictionary<string, object>();

                    DropDownList ddl_ins_categoria_cod = (DropDownList)dtv_dettaglio.FindControl("ddl_ins_categoria_cod");
                    string sCategoria_cod = "";
                    sCategoria_cod = ddl_ins_categoria_cod.SelectedValue;
                    coll.Add("categoria_cod", sCategoria_cod);

                    TextBox anno = (TextBox)dtv_dettaglio.FindControl("ins_anno");
                    string sAnno = "";
                    sAnno = anno.Text;
                    sAnno = Server.HtmlEncode(sAnno);
                    if (sAnno != "")
                    {
                        coll.Add("anno", sAnno);
                    }

                    TextBox descrizione = (TextBox)dtv_dettaglio.FindControl("ins_descrizione");
                    string sDescrizione = "";
                    sDescrizione = descrizione.Text;
                    sDescrizione = HttpUtility.HtmlEncode(sDescrizione);
                    if (sDescrizione != "")
                    {
                        coll.Add("descrizione", sDescrizione);
                    }

                    TextBox titolo = (TextBox)dtv_dettaglio.FindControl("ins_titolo");
                    string sTitolo = "";
                    sTitolo = titolo.Text;
                    sTitolo = HttpUtility.HtmlEncode(sTitolo);
                    if (sTitolo == "")
                    {
                        sTitolo = FileUpload1.PostedFile.FileName.ToString();

                    }
                    coll.Add("titolo", sTitolo);

                    if (FileUpload1 != null)
                    {
                        FileUpload blob = (FileUpload)dtv_dettaglio.FindControl("blb_ins");
                        coll.Add("istruzioni", blob.FileBytes);
                        coll.Add("titolo_file", blob.FileName);

                        TextBox bytes_curr = (TextBox)dtv_dettaglio.FindControl("blb_ins_bytes_d");
                        string sBytes_curr = "";
                        sBytes_curr = bytes_curr.Text;
                        coll.Add("bytes", blob.PostedFile.InputStream.Length);

                        TextBox ext_curr = (TextBox)dtv_dettaglio.FindControl("blb_ins_ext_d");
                        string sExt_curr = "";
                        sExt_curr = ext_curr.Text;
                        coll.Add("ext", sExt_curr);

                        
                    }

                    DBUtility.ins_FileIstruzioni(coll);
                    Response.Redirect("Istruzioni.aspx");
                }
                catch (Exception ex)
                {
                    if (ex.Message == "Il file può essere di tipo: pdf, doc,docx,html,htm")
                    {
                        MyMdlDlg.Show(MyControls.myModalDialog.MessageType.Error, "Errore", ex.Message, 100, 300, 60, 20);
                    }
                    else if (ex.Message == "La dimensione del file è troppo grande")
                    {
                        MyMdlDlg.Show(MyControls.myModalDialog.MessageType.Error, "Errore", ex.Message, 100, 300, 60, 20);
                    }

                    else
                    {
                        MyMdlDlg.Show(MyControls.myModalDialog.MessageType.Error, "Errore", "Aggiornamento non riuscito. Contattare I.S.I.", 100, 300, 60, 20);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "Il file può essere di tipo: pdf, doc,docx,html,htm")
                {
                    MyMdlDlg.Show(MyControls.myModalDialog.MessageType.Error, "Errore", ex.Message, 100, 300, 60, 20);
                }
                else if (ex.Message == "La dimensione del file è troppo grande")
                {
                    MyMdlDlg.Show(MyControls.myModalDialog.MessageType.Error, "Errore", ex.Message, 100, 300, 60, 20);
                }
                else
                {
                    MyMdlDlg.Show(MyControls.myModalDialog.MessageType.Error, "Errore", "Aggiornamento non riuscito. Contattare I.S.I.", 100, 300, 60, 20);
                }
            }

        }
        protected void dtv_dettaglio_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
        {

        }
        protected void dtv_dettaglio_ItemUpdated(object sender, DetailsViewInsertedEventArgs e)
        {

        }
        protected void mnu_main_MenuItemClick(object sender, MenuEventArgs e)
        {
            if (e.Item.Value == "Esc")
            {
                Page.Response.Redirect("Istruzioni.aspx");
            }


        }
    }
}