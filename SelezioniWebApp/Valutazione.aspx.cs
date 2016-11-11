using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using clDB;
using SelezioniObjects;
using System.Data;
using System.Linq.Expressions;


namespace SelezioniWebApp
{
    public partial class Valutazione : System.Web.UI.Page
    {
        private clsUtenteLogin oUtente;
        private clsUtenteLogin oUtenteDip;
        ClsDB DBUtility = new ClsDB();
        ClsUtility Utility = new ClsUtility(); //>>ga04032012<<

        protected void Page_Init(object sender, EventArgs e)
        {
            //if (Session.Count == 0) // sessione scaduta
            //{
            //    Response.Redirect("SessioneScaduta.aspx");
            //}
            if (Context.Session != null)
            {
                if (Session.IsNewSession)
                {
                    HttpCookie newSessionIdCookie = Request.Cookies["ASP.NET_SessionId"];
                    if (newSessionIdCookie != null)
                    {
                        string newSessionIdCookieValue = newSessionIdCookie.Value;
                        if (newSessionIdCookieValue != string.Empty)
                        {
                            // This means Session was timed Out and New Session was started 
                            Response.Redirect("SessioneScaduta.aspx");
                        }
                    }
                }
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ExpiresAbsolute = DateTime.Now.AddMonths(-1);
            try
            {
                //Controllo esistenza variabili di sessione
                if (Session["Utente"] == null || Session["UtenteDip"] == null || Session["Param"] == null)
                {
                    throw new CustomExceptions.SessioneScaduta();
                }
                else
                {
                    if (!Page.IsPostBack)
                    {
                        Session.Remove("griglia");
                        // verifico da dove arrivo
                        //Verifica utente abilitato
                        string sUser = "";
                        sUser = ((clsUtenteLogin)Session["Utente"]).UserId;
                        oUtente = (clsUtenteLogin)Session["Utente"];
                        Session["Utente"] = oUtente;
                        if (Request.UrlReferrer != null && Request.UrlReferrer.AbsolutePath.ToString().IndexOf("Compilazione.aspx") != -1)
                        {
                            ddlTipoSelezione.DataSource = DBUtility.PopolaDDLSelezioneCod();
                            ddlTipoSelezione.DataValueField = "cod_selezione";
                            ddlTipoSelezione.DataTextField = "des_selezione";
                            ddlTipoSelezione.DataBind();
                            ddlTipoSelezione.SelectedValue = (string)Session["TipoSelezione"];
                            ddlAnno.DataSource = DBUtility.PopolaDDLAnno(ddlTipoSelezione.SelectedValue);
                            ddlAnno.DataValueField = "anno";
                            ddlAnno.DataTextField = "des_anno";
                            ddlAnno.DataBind();
                            //>>ga082015<< inizio
                            if (Session["AnnoAmm"] != null  && (Session["Param"].ToString() == "amm" || Session["Param"].ToString() == "val"))
                            {
                                ddlAnno.SelectedValue = (string)Session["AnnoAmm"];
                                Session["AnnoAmm"] = null; ;
                            }
                            else
                            {
                            //>>ga082015<< fine
                                ddlAnno.SelectedValue = (string)Session["Anno"];
                            }//>>ga082015<<
                            ddlCategoria.DataSource = DBUtility.PopolaDDLCategoria(Session["TipoSelezione"].ToString(), int.Parse(ddlAnno.SelectedValue.ToString()));
                            ddlCategoria.DataValueField = "categoria";
                            ddlCategoria.DataTextField = "des_categoria";
                            ddlCategoria.DataBind();
                            ddlCategoria.SelectedValue = (string)Session["ddlCategoria"];
                            ddlAfferenza.DataSource = DBUtility.PopolaDDLAfferenza(int.Parse(ddlAnno.SelectedValue.ToString()));
                            ddlAfferenza.DataValueField = "cod";
                            ddlAfferenza.DataTextField = "descrizione";
                            ddlAfferenza.DataBind();
                            ddlAfferenza.SelectedValue = (string)Session["ddlAfferenza"];

                            ddlMatricola.DataSource = DBUtility.PopolaDDLMatricola(int.Parse(ddlAnno.SelectedValue.ToString()));
                            ddlMatricola.DataValueField = "cod";
                            ddlMatricola.DataTextField = "descrizione";
                            ddlMatricola.DataBind();
                            ddlMatricola.SelectedValue = (string)Session["ddlMatricola"];

                            ddlStato.DataSource = DBUtility.PopolaDDLStato();
                            ddlStato.DataValueField = "cod";
                            ddlStato.DataTextField = "descrizione";
                            ddlStato.DataBind();
                            ddlStato.SelectedValue = (string)Session["ddlStato"];

                            ddlCognome.DataSource = DBUtility.PopolaDDLCognome(int.Parse(ddlAnno.SelectedValue.ToString()));
                            ddlCognome.DataValueField = "cod";
                            ddlCognome.DataTextField = "descrizione";
                            ddlCognome.DataBind();
                            ddlCognome.SelectedValue = (string)Session["ddlCognome"];

                            caricagriglia((string)Session["Param"]);
                            grdValutazioni.Visible = true;

                        }
                        ddlTipoSelezione.DataSource = DBUtility.PopolaDDLSelezioneCod();
                        ddlTipoSelezione.DataValueField = "cod_selezione";
                        ddlTipoSelezione.DataTextField = "des_selezione";
                        ddlTipoSelezione.DataBind();
                        try
                        {
                            if ((string)Session["Param"] == "val")
                            {
                                oUtente = DBUtility.LeggiUtenteResp(sUser);
                                lblMenu.Text = "Valutazione Responsabile";
                                if (oUtente == null && (string)Session["Param"] == "val")
                                {
                                    throw new CustomExceptions.UtenteNonAbilitato(sUser);
                                }
                            }
                            else if ((string)Session["Param"] == "amm")
                            {
                                oUtente = DBUtility.VerificaUtenteValAmm(sUser);
                                lblMenu.Text = "Valutazione Amministrazione";

                                if (oUtente == null && (string)Session["Param"] == "amm")
                                {
                                    throw new CustomExceptions.UtenteNonAbilitato(sUser);
                                }
                            }
                            else if ((string)Session["Param"] == "vis")
                            {
                                oUtente = DBUtility.VerificaUtenteDG(sUser);
                                lblMenu.Text = "Visualizzazione schede";

                                if (oUtente == null && (string)Session["Param"] == "vis")
                                {
                                    throw new CustomExceptions.UtenteNonAbilitato(sUser);
                                }
                            }
                            Session["Utente"] = oUtente;


                        }
                        catch (CustomExceptions.SessioneScaduta)
                        {
                            throw new CustomExceptions.SessioneScaduta();
                        }
                        catch (CustomExceptions.UtenteNonAbilitato)
                        {
                            throw new CustomExceptions.UtenteNonAbilitato(sUser);
                        }
                        catch (CustomExceptions.SelezioneNonTrovata)
                        {
                            throw new CustomExceptions.SelezioneNonTrovata();
                        }
                        //caricagriglia((string)Session["Param"]);
                    }

                    else
                    {
                        oUtenteDip = (clsUtenteLogin)Session["UtenteDip"];
                        oUtente = (clsUtenteLogin)Session["Utente"];
                        if (Session["TipoSelezione"] != null)
                        {
                            ddlTipoSelezione.SelectedValue = Session["TipoSelezione"].ToString();
                        }
                        

                    }
                }
            }
            catch (CustomExceptions.SessioneScaduta ex)
            {
                tblCmd.Visible = false;
                tblParametri.Visible = false;
                lblParametri.Visible = false;
                lblParamOpz.Visible = false;
                tblParametri1.Visible = false;

                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + ex.CustomMessage + "');", true);
                Session["Utente"] = null;
                Session["Param"] = null;

            }
            catch (CustomExceptions.UtenteNonAbilitato ex)
            {
                tblCmd.Visible = false;
                tblParametri.Visible = false;
                lblParametri.Visible = false;
                lblParamOpz.Visible = false;
                tblParametri1.Visible = false;
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + ex.CustomMessage + "');", true);
                Session["Utente"] = null;
                Session["Param"] = null;
            }

            catch (CustomExceptions.SelezioneNonTrovata ex)
            {
                tblCmd.Visible = false;
                tblParametri.Visible = false;
                lblParametri.Visible = false;
                lblParamOpz.Visible = false;
                tblParametri1.Visible = false;
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + ex.CustomMessage + "');", true);
                Session["Utente"] = null;
                Session["Param"] = null;
            }
        }
        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            if ((string)Session["Param"] == "val" || (string)Session["Param"] == null)
                abilita_disabilita_menu(false, true);
            else if ((string)Session["Param"] == "amm")
                abilita_disabilita_menu(true, true);

        }
        protected void mnu_CompilaSelezione_MenuItemClick(object sender, MenuEventArgs e)
        {
            DataTable dtTemp = new DataTable();
            int SelezioneId = 0;
            if (e.Item.Value == "Rit")
            {
                //Page.Response.Redirect("http://www.units.it/intra/modulistica/peo2011/");
                //Page.Response.Redirect("http://www.units.it/intra/modulistica/peo2015/");
                //>>27092016<< inizio
                //Page.Response.Redirect("https://www.units.it/intra/modulistica/peo/");
                Session.Clear(); //>>27092016<<
                Session.Abandon(); //>>27092016<<
                if (Request.Cookies["ASP.NET_SessionId"] != null)
                {
                    HttpCookie myCookie = Request.Cookies["ASP.NET_SessionId"];
                    //myCookie.Expires = DateTime.Now.AddDays(-2d);
                    myCookie.Value = "ExitApplication";
                    Response.Cookies.Set(myCookie);
                }
                if (Request.Url.AbsoluteUri.Substring(0, 16) == "http://webtest02")
                {
                    Page.Response.Redirect("http://dipartimento.cicmsdev.units.it/it/content/peo");
                }
                else
                {
                    Page.Response.Redirect("https://www.units.it/intra/modulistica/peo/");
                }
                Context.ApplicationInstance.CompleteRequest();
                //>>27092016<< fine
            }
            else if (e.Item.Value == "ammprev") //>>ga16102012<< inizio
            {
                if (ddlTipoSelezione.SelectedIndex == -1 || ddlTipoSelezione.SelectedIndex == 0 || ddlAnno.SelectedIndex == -1 || ddlAnno.SelectedIndex == 0)
                {
                    string Message = "Devi selezionare Tipo selezione / Anno ";
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + Message + "');", true);
                }
                else
                {
                    try
                    {
                        
                        dtTemp = (DataTable)Session["griglia"]; 
                        if (dtTemp == null || dtTemp.Rows.Count != 1)
                        {
                            string Message = "Devi selezionare un solo dipendente ";
                            this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + Message + "');", true);
                        }
                        else 
                        {
                            // oUtenteDip = DBUtility.LeggiUtenteLogin(ddlMatricola.SelectedValue);
                            oUtenteDip = DBUtility.LeggiUtenteLogin(dtTemp.Rows[0]["matri_dip"].ToString());
                            //SelezioneId = DBUtility.RicercaSelezione(oUtenteDip, "PEO", null, null);
                            SelezioneId = int.Parse(dtTemp.Rows[0]["selezione_id"].ToString());
                            //int tmpCompId = DBUtility.RicercaCompilazione(ddlMatricola.SelectedValue, SelezioneId, null);
                            int tmpCompId = int.Parse(dtTemp.Rows[0]["comp_id"].ToString());
                            if (tmpCompId != 0)
                            {
                                try
                                {
                                    DBUtility.CambiaStatoPrev(SelezioneId, tmpCompId, oUtenteDip.UserId, null);
                                    caricagriglia((string)Session["Param"]);
                                    grdValutazioni.Visible = true;
                                }
                                catch (Exception ex)
                                {
                                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + "Modifica stato non riuscito" + "');", true);
                                    throw ex;
                                }
                            }
                            else
                            {
                                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + "Non esistono compilazioni da inoltrare" + "');", true);
                            }
                        }
                       
                    }
                    catch (Exception ex)
                    {
                        string stremess = "Aggiornamento stato fallito" + ex.Message;
                        this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + stremess + "');", true);

                    }
                }
                
            }
            else if (e.Item.Value == "ammnext") 
            {
                if (ddlTipoSelezione.SelectedIndex == -1 || ddlTipoSelezione.SelectedIndex == 0 || ddlAnno.SelectedIndex == -1 || ddlAnno.SelectedIndex == 0)
                {
                    string Message = "Devi selezionare Tipo selezione / Anno ";
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + Message + "');", true);
                }
                else
                {
                    try
                    {
                        dtTemp = (DataTable)Session["griglia"];
                        if (dtTemp == null || dtTemp.Rows.Count == 0)
                        {
                            string Message = "Devi selezionare almeno un dipendente ";
                            this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + Message + "');", true);
                        }
                        else if (dtTemp.DefaultView.ToTable(true, "stato").Rows.Count != 1) // verifico lo stato delle domande
                        {
                             string Message = "Le domande selezionate si trovano in stati diversi! ";
                             this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + Message + "');", true);
                        }
                        //>>ga082016<<
                        //else if (dtTemp.DefaultView.ToTable(true, "selezione_id").Rows.Count > 0) >>ga13092016<< 
                        else if (dtTemp.DefaultView.ToTable(true, "selezione_id").Rows.Count > 1) // verifico lo stato delle selezioni coinvolte 
                        {
                            foreach (DataRow dr in dtTemp.DefaultView.ToTable(true, "selezione_id").Rows)
                            {
                                
                                string Message = "Le domande selezionate si trovano in stati diversi! ";
                                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + Message + "');", true);
                            }
                        }
                        else
                        {
                            foreach (DataRow dr in dtTemp.Rows)
                            {
                                oUtenteDip = DBUtility.LeggiUtenteLogin(dr["matri_dip"].ToString()); // leggo
                                SelezioneId = int.Parse(dr["selezione_id"].ToString());
                                int tmpCompId = int.Parse(dr["comp_id"].ToString());
                                if (tmpCompId != 0)
                                {
                                    try
                                    {
                                        DBUtility.CambiaStato(SelezioneId, tmpCompId, oUtenteDip.UserId, null);
                                        grdValutazioni.Visible = true;
                                    }
                                    catch (Exception ex)
                                    {
                                        this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + "Inoltro domanda non riuscito" + "');", true);
                                        throw ex;
                                    }
                                }
                                else
                                {
                                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + "Non esistono compilazioni da inoltrare" + "');", true);
                                }
                            }
                            caricagriglia((string)Session["Param"]);
                        }

                    }
                    catch (Exception ex)
                    {
                        string stremess = "Aggiornamento stato fallito" + ex.Message;
                        this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + stremess + "');", true);

                    }
                }
                
            }
            //>>ga16102012<< fine          
            else if (e.Item.Value == "exc")
            {
                if (ddlTipoSelezione.SelectedIndex == -1 || ddlTipoSelezione.SelectedIndex == 0 || ddlAnno.SelectedIndex == -1 || ddlAnno.SelectedIndex == 0)
                {
                    string Message = "Devi selezionare Tipo selezione / Anno ";
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + Message + "');", true);
                }
                else
                {
                    try
                    {
                        DataTable dtCompilazioni = new DataTable();
                        dtCompilazioni = DBUtility.EstraiElencoFinale(0, int.Parse(ddlAnno.SelectedValue.ToString()));
                        Response.ClearContent();
                        Response.ClearHeaders();
                        Response.Buffer = true;
                        Response.Charset = "";
                        Response.ContentType = "application/vnd.ms-excel";
                        Response.AppendHeader("Content-Disposition", "inline; filename=ExportExcel.xls");
                        Utility.crea_excel(dtCompilazioni, 0, Response.OutputStream);
                        Response.Flush();
                        Response.ClearContent();
                        Response.End();
                    }
                    catch (Exception ex)
                    {
                        string stremess = "Estrazione FALLITA Avvisa I.S.I. (file completo)" + ex.Message;
                        this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + stremess + "');", true);

                    }
                }
            }
            else if (e.Item.Value == "exc_c") //estrazione per categoria
            {
                if (ddlTipoSelezione.SelectedIndex == -1 || ddlTipoSelezione.SelectedIndex == 0 || ddlAnno.SelectedIndex == -1 || ddlAnno.SelectedIndex == 0
                    || ddlCategoria.SelectedIndex == -1 || ddlCategoria.SelectedIndex == 0)
                {
                    string Message = "Devi selezionare Tipo selezione / Anno / Categoria ";
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + Message + "');", true);
                }
                else
                {
                    try
                    {
                        DataTable dtCompilazioni = new DataTable();
                        IList<Selezione> lSelezione = new List<Selezione>();
                        lSelezione = DBUtility.LeggiSelezioni(null, ddlAnno.SelectedValue.ToString(), ddlCategoria.SelectedValue.ToString(), 0);
                        if (lSelezione.Count > 1)
                        {
                            throw new Exception("Errore estrazione file categoria - " + ddlCategoria.SelectedValue.ToString());
                        }
                        dtCompilazioni = DBUtility.EstraiElencoFinale(int.Parse(lSelezione[0].SelezioneId.ToString()), 0);
                        Response.ClearContent();
                        Response.ClearHeaders();
                        Response.Buffer = true;
                        Response.Charset = "";
                        Response.ContentType = "application/vnd.ms-excel";
                        Response.AppendHeader("Content-Disposition", "inline; filename=ExportExcel.xls");
                        Utility.crea_excel(dtCompilazioni, 0, Response.OutputStream);
                        Response.Flush();
                        Response.ClearContent();
                        Response.End();
                    }
                    catch (Exception ex)
                    {
                        //string stremess = "Estrazione FALLITA Avvisa I.S.I." + ex.Message;
                        //this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + stremess + "');", true);
                        this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + ex.Message.ToString() + "');", true);
                    }
                }
            }
        }
        protected void grdValutazioni_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Sel")
            {
                int index = int.Parse(e.CommandArgument.ToString());
                //int matri = int.Parse(((Label)grdValutazioni.Rows[index].FindControl("lblMatri")).Text.ToString());
                string matri = ((Label)grdValutazioni.Rows[index].FindControl("lblMatri")).Text.ToString();
                oUtenteDip = DBUtility.LeggiUtenteLogin(matri.ToString());
                Session["UtenteDip"] = oUtenteDip;
                //>>ga082015<< inzizio
                if ((Session["param"].ToString() == "amm" || Session["param"].ToString() == "val") && ddlAnno.SelectedValue != null && (Session["AnnoAmm"] == null ||Session["AnnoAmm"].ToString() == ""))
                {
                    Session["AnnoAmm"] = ddlAnno.SelectedValue.ToString();
                }
                //>>ga082015<< fine
                Page.Response.Redirect("Compilazione.aspx");
            }
        }
        protected void grdValutazioni_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
        private void caricagriglia(string pParam)
        {
            ////if (pParam == "amm")
            ////{
            try
            {
                grdValutazioni.DataSource = DBUtility.CaricaUtentiAmm(int.Parse(oUtente.UserId), pParam, ddlTipoSelezione.SelectedValue.ToString(),
                                                                      int.Parse(ddlAnno.SelectedValue.ToString()), ddlAfferenza.SelectedValue,
                                                                      ddlCategoria.SelectedValue, ddlStato.SelectedValue,
                                                                      int.Parse(ddlMatricola.SelectedValue.ToString()), int.Parse(ddlCognome.SelectedValue.ToString()));
                /*
                grdValutazioni.DataSource = DBUtility.CaricaUtentiAmm(3444, pParam, ddlTipoSelezione.SelectedValue.ToString(),
                                                                      int.Parse(ddlAnno.SelectedValue.ToString()), ddlAfferenza.SelectedValue,
                                                                      ddlCategoria.SelectedValue, ddlStato.SelectedValue,
                                                                      int.Parse(ddlMatricola.SelectedValue.ToString()), int.Parse(ddlCognome.SelectedValue.ToString()));
                */
                grdValutazioni.DataBind();
                Session["griglia"] = grdValutazioni.DataSource;
                Session["ddlTipoSelezione"] = (ddlTipoSelezione.SelectedIndex != -1) ? ddlTipoSelezione.SelectedValue.ToString() : null;
                Session["ddlAfferenza"] = (ddlAfferenza.SelectedIndex != -1) ? ddlAfferenza.SelectedValue.ToString() : null;
                Session["ddlCategoria"] = (ddlCategoria.SelectedIndex != -1) ? ddlCategoria.SelectedValue.ToString() : null;
                Session["ddlStato"] = (ddlStato.SelectedIndex != -1) ? ddlStato.SelectedValue.ToString() : null;
                Session["ddlMatricola"] = (ddlMatricola.SelectedIndex != -1) ? ddlMatricola.SelectedValue.ToString() : null;
                Session["ddlCognome"] = (ddlCognome.SelectedIndex != -1) ? ddlCognome.SelectedValue.ToString() : null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void ddlAnno_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (ddlAnno.SelectedIndex != -1 && Session["TipoSelezione"] != null)
            {
                if (Session["Anno"] != null && ddlAnno.SelectedValue.ToString() != Session["Anno"].ToString())
                {
                    ddlCategoria.Items.Clear();
                    ddlAfferenza.Items.Clear();
                    ddlMatricola.Items.Clear();
                    ddlStato.Items.Clear();
                    ddlCognome.Items.Clear();
                }
                ddlCategoria.DataSource = DBUtility.PopolaDDLCategoria(Session["TipoSelezione"].ToString(), int.Parse(ddlAnno.SelectedValue.ToString()));
                ddlCategoria.DataValueField = "categoria";
                ddlCategoria.DataTextField = "des_categoria";
                ddlCategoria.DataBind();
                Session["Anno"] = ddlAnno.SelectedValue.ToString();
                ddlAfferenza.DataSource = DBUtility.PopolaDDLAfferenza(int.Parse(ddlAnno.SelectedValue.ToString()));
                ddlAfferenza.DataValueField = "cod";
                ddlAfferenza.DataTextField = "descrizione";
                ddlAfferenza.DataBind();
                ddlMatricola.DataSource = DBUtility.PopolaDDLMatricola(int.Parse(ddlAnno.SelectedValue.ToString()));
                ddlMatricola.DataValueField = "cod";
                ddlMatricola.DataTextField = "descrizione";
                ddlMatricola.DataBind();
                ddlStato.DataSource = DBUtility.PopolaDDLStato();
                ddlStato.DataValueField = "cod";
                ddlStato.DataTextField = "descrizione";
                ddlStato.DataBind();
                ddlCognome.DataSource = DBUtility.PopolaDDLCognome(int.Parse(ddlAnno.SelectedValue.ToString()));
                ddlCognome.DataValueField = "cod";
                ddlCognome.DataTextField = "descrizione";
                ddlCognome.DataBind();

            }
        }
        protected void ddlTipoSelezione_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTipoSelezione.SelectedIndex != -1)
            {

                if (Session["TipoSelezione"] != null && ddlTipoSelezione.SelectedValue.ToString() != Session["TipoSelezione"].ToString())
                {
                    ddlAnno.Items.Clear();
                    ddlCategoria.Items.Clear();
                    ddlAfferenza.Items.Clear();
                    ddlMatricola.Items.Clear();
                    ddlStato.Items.Clear();
                    ddlCognome.Items.Clear();
                }
                ddlAnno.DataSource = DBUtility.PopolaDDLAnno(ddlTipoSelezione.SelectedValue.ToString());
                ddlAnno.DataValueField = "anno";
                ddlAnno.DataTextField = "des_anno";
                ddlAnno.DataBind();
                Session["TipoSelezione"] = ddlTipoSelezione.SelectedValue.ToString();
            }
        }
        protected void btnCerca_Click(object sender, EventArgs e)
        {
            lblErr.Text = "";
            if (Session["TipoSelezione"] == null || Session["Anno"] == null)
            {
                // TODO: errore
                lblErr.Text = "Valorizzare i campi obbligatori";
                ;
            }
            else
            {
                grdValutazioni.Visible = true;
                try
                {
                    caricagriglia((string)Session["Param"]);
                }
                catch (Exception ex)
                {
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + ex.Message + "');", true);
                }

            }
        }
        protected void btnAnnulla_Click(object sender, EventArgs e)
        {
            ddlTipoSelezione.ClearSelection();
            ddlAnno.Items.Clear();
            ddlCategoria.Items.Clear();
            ddlAfferenza.Items.Clear();
            ddlMatricola.Items.Clear();
            ddlStato.Items.Clear();
            ddlCognome.Items.Clear();
            grdValutazioni.Visible = false;
            lblErr.Text = "";
        }
        protected void grdValutazioni_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dt = new DataTable();
            dt = (DataTable)Session["griglia"];
            if (dt != null)
            {
                dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                grdValutazioni.DataSource = dt;
                grdValutazioni.DataBind();
                Session["griglia"] = dt;
            }
        }
        private string GetSortDirection(string column)
        {

            // By default, set the sort direction to ascending.
            string sortDirection = "ASC";

            // Retrieve the last column that was sorted.
            string sortExpression = ViewState["SortExpression"] as string;

            if (sortExpression != null)
            {
                // Check if the same column is being sorted.
                // Otherwise, the default value can be returned.
                if (sortExpression == column)
                {
                    string lastDirection = ViewState["SortDirection"] as string;
                    if ((lastDirection != null) && (lastDirection == "ASC"))
                    {
                        sortDirection = "DESC";
                    }
                }
            }

            // Save new values in ViewState.
            ViewState["SortDirection"] = sortDirection;
            ViewState["SortExpression"] = column;

            return sortDirection;
        }
        protected bool ConvertiStato(int pStato)
        {
            switch (pStato)
            {
                case 0:
                    return false;
                    break;

                default:
                    return true;
                    break;
            }
        }
        private void abilita_disabilita_menu(bool pEstrazione, bool pRitorna)
        {
            foreach (MenuItem i in mnu_CompilaSelezione.Items)
            {
               // if (i.Value == "exc" || i.Value == "exc_c")  //>>ga16102012<<
                if (i.Value == "exc" || i.Value == "exc_c" || i.Value == "ammnext" || i.Value == "ammprev") //>>ga16102012<<
                {
                    i.Selectable = pEstrazione;
                }
                else if (i.Value == "Rit")
                {
                    i.Selectable = pRitorna;
                }
            }
        }
    }
}