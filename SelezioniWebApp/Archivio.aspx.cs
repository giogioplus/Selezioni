using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using clDB;
using SelezioniObjects;
using System.Data;

namespace SelezioniWebApp
{
    public partial class Archivio : System.Web.UI.Page
    {
        private clsUtenteLogin oUtente;
        private clsUtenteLogin oUtenteDip;
        ClsDB DBUtility = new ClsDB();
        ClsUtility Utility = new ClsUtility(); 

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
            string sUser = ""; 
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
                        sUser = ((clsUtenteLogin)Session["Utente"]).UserId;
                        oUtente = (clsUtenteLogin)Session["UtenteDip"];
                        Session["Utente"] = oUtente;
                        //>>ga082015<< inizio
                        if (Session["param"].ToString() == "amm" || Session["param"].ToString() == "val")
                        {
                            if (Session["UtenteArchivio"] == null )
                            {
                                Session["UtenteArchivio"] = sUser;
                            }
                        }
                        //>>ga082015<< fine
                        if (Request.UrlReferrer != null && Request.UrlReferrer.AbsolutePath.ToString().IndexOf("Compilazione.aspx") != -1)
                        {
                            ddlTipoSelezione.DataSource = DBUtility.PopolaDDLSelezioneCod();
                            ddlTipoSelezione.DataValueField = "cod_selezione";
                            ddlTipoSelezione.DataTextField = "des_selezione";
                            ddlTipoSelezione.DataBind();
                            ddlTipoSelezione.SelectedValue = (string)Session["TipoSelezione"];
                            ddlAnno.DataSource = DBUtility.PopolaDDLAnnoArchivio(ddlTipoSelezione.SelectedValue);
                            ddlAnno.DataValueField = "anno";
                            ddlAnno.DataTextField = "des_anno";
                            ddlAnno.DataBind();
                            ddlAnno.SelectedValue = (string)Session["Anno"];


                            // caricagriglia((string)Session["Param"]);
                            // grdArchivio.Visible = true;

                        }
                        ddlTipoSelezione.DataSource = DBUtility.PopolaDDLSelezioneCod();
                        ddlTipoSelezione.DataValueField = "cod_selezione";
                        ddlTipoSelezione.DataTextField = "des_selezione";
                        ddlTipoSelezione.DataBind();
                    }
                    else
                    {
                        sUser = ((clsUtenteLogin)Session["Utente"]).UserId;
                        oUtente = (clsUtenteLogin)Session["Utente"];
                        Session["Utente"] = oUtente;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void caricagriglia(string pParam)
        {
            ////if (pParam == "amm")
            ////{
            try
            {
                grdArchivio.DataSource = DBUtility.CaricaArchivio(int.Parse(oUtente.UserId), pParam, ddlTipoSelezione.SelectedValue.ToString(),
                                                                  int.Parse(ddlAnno.SelectedValue.ToString()));
                //grdValutazioni.DataSource = DBUtility.CaricaUtentiAmm(3444, pParam, ddlTipoSelezione.SelectedValue.ToString(),
                //                                                      int.Parse(ddlAnno.SelectedValue.ToString()));
                grdArchivio.DataBind();
                Session["griglia"] = grdArchivio.DataSource;
                Session["ddlTipoSelezione"] = (ddlTipoSelezione.SelectedIndex != -1) ? ddlTipoSelezione.SelectedValue.ToString() : null;
                
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
                
                Session["Anno"] = ddlAnno.SelectedValue.ToString();
                
            }
        }
        protected void ddlTipoSelezione_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTipoSelezione.SelectedIndex != -1)
            {

                if (Session["TipoSelezione"] != null && ddlTipoSelezione.SelectedValue.ToString() != Session["TipoSelezione"].ToString())
                {
                    ddlAnno.Items.Clear();
                    
                }
                ddlAnno.DataSource = DBUtility.PopolaDDLAnnoArchivio(ddlTipoSelezione.SelectedValue.ToString());
                ddlAnno.DataValueField = "anno";
                ddlAnno.DataTextField = "des_anno";
                ddlAnno.DataBind();
                Session["TipoSelezione"] = ddlTipoSelezione.SelectedValue.ToString();
            }
        }
        protected void btnCerca_Click(object sender, EventArgs e)
        {
            lblErr.Text = "";
            if (Session["TipoSelezione"] == null)
            {
                // TODO: errore
                lblErr.Text = "Valorizzare i campi obbligatori";
                ;
            }
            else
            {
                grdArchivio.Visible = true;
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
            grdArchivio.Visible = false;
            lblErr.Text = "";
        }
        protected void grdArchivio_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dt = new DataTable();
            dt = (DataTable)Session["griglia"];
            if (dt != null)
            {
                dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                grdArchivio.DataSource = dt;
                grdArchivio.DataBind();
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
        /*
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
         */
        protected void mnu_Archivio_MenuItemClick(object sender, MenuEventArgs e)
        {
            if (e.Item.Value == "Rit" && Session["Param"].ToString() == "com")
            {
                Response.Redirect("Compilazione.aspx");
            }
            //>>ga082015<< inizio
            if (e.Item.Value == "Rit" && (Session["Param"].ToString() == "amm" || Session["Param"].ToString() == "val"))
            {
               clsUtenteLogin oUtenteAmm = new clsUtenteLogin();
               oUtenteAmm.UserId = Session["UtenteArchivio"].ToString();
               Session["Utente"] = oUtenteAmm;
               Session["UtenteArchivio"] = null;
               Response.Redirect("Compilazione.aspx");
            }
            //>>ga082015<< fine
        }
        protected void grdArchivio_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Sel")
            {
                
                int index = int.Parse(e.CommandArgument.ToString());
                /*
                //int matri = int.Parse(((Label)grdValutazioni.Rows[index].FindControl("lblMatri")).Text.ToString());
                string matri = ((Label)grdArchivio.Rows[index].FindControl("lblMatri")).Text.ToString();
                oUtenteDip = DBUtility.LeggiUtenteLogin(matri.ToString());
                Session["UtenteDip"] = oUtenteDip;
                Page.Response.Redirect("Compilazione.aspx");
                 * */
                int SelId = int.Parse(((Label)grdArchivio.Rows[index].FindControl("lblSelezione")).Text.ToString());
                int CompilazioneId = int.Parse(((Label)grdArchivio.Rows[index].FindControl("lblCompilazione")).Text.ToString());
                Page.Response.Redirect("StampaSelezione.aspx?SelezioneId=" + SelId.ToString() + "&CompId=" + CompilazioneId.ToString() + "&Arc=1");
            }
        }
        protected void grdArchivio_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
    }
}