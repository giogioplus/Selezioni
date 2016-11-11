using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using clDB;
using SelezioniObjects;
using System.Web.Security;
using System.Net;


namespace SelezioniWebApp
{
    public partial class Gestione : System.Web.UI.Page
    {
        ClsDB DBUtility = new ClsDB();
        private  clsUtenteLogin oUtente;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    if (Session["Utente"] == null || Session["Param"] == null)
                    {
                        throw new CustomExceptions.SessioneScaduta();
                    }
                    string sUser = "";
                    sUser = ((clsUtenteLogin)Session["Utente"]).UserId;
                    string tmpUser = "";
                    try
                    {
                        tmpUser = DBUtility.VerificaUtente(sUser);
                        if (tmpUser == "0")
                        {
                            abilita_disabilita_menu(false, false, true);
                            throw new CustomExceptions.UtenteNonAbilitato(sUser);
                        }
                    }
                    catch
                    {
                        throw new CustomExceptions.UtenteNonAbilitato(sUser);
                    }
                }
                else
                {
                    if (Session["Utente"] == null)
                        throw new CustomExceptions.SessioneScaduta();
                    else
                    {
                        oUtente = (clsUtenteLogin)Session["Utente"];
                    }
                }
            }
            catch (CustomExceptions.SessioneScaduta ex)
            {
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + ex.CustomMessage + "');", true);
                abilita_disabilita_menu(false, false, true);
            }
            catch (CustomExceptions.UtenteNonAbilitato ex)
            {
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + ex.CustomMessage + "');", true);
                abilita_disabilita_menu(false, false, true);
            }
        }
        protected void mnu_main_MenuItemClick(object sender, MenuEventArgs e)
        {
            if (e.Item.Value == "Sel")
            {
                Page.Response.Redirect("SelSelezione.aspx");
            }
            else if (e.Item.Value == "Add")
            {
                Page.Response.Redirect("CreaSelezione.aspx");
            }
            else if (e.Item.Value == "Upl")
            {
                //Page.Response.Redirect("Upload.aspx");
                Page.Response.Redirect("Istruzioni.aspx");
            }
            else if (e.Item.Value == "Esc")
            {
                Page.Response.Redirect("http://www.units.it");
            }
           
        }
        private void abilita_disabilita_menu(bool pNuovaSel, bool pGestSel, bool pRitorna)
        {
            foreach (MenuItem i in mnu_main.Items)
            {
                if (i.Value == "Add")
                {
                    i.Selectable = pNuovaSel;
                }
                else if (i.Value == "Sel")
                {
                    i.Selectable = pGestSel;
                }
                else if (i.Value == "Esc")
                {
                    i.Selectable = pRitorna;
                }
            }
        }
        
    }
}