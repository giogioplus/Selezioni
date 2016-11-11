using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SelezioniObjects;


namespace SelezioniWebApp
{
    public partial class Default : System.Web.UI.Page
    {
        string Param = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["Utente"] = null;
            Session["UtenteDip"] = null;
            try
            {
                //Pagina deve essere richiamata con 1 parametro
                if (Request.QueryString.Count != 1)
                {
                    throw new CustomExceptions.PaginaNonRichiamataCorrettamente(null, "Parametri mancanti");
                }
                //Deve sempre essere presente il parametro Param
                else if (Request.QueryString["Param"] == null)
                {
                    throw new CustomExceptions.PaginaNonRichiamataCorrettamente("Param", null);
                }
                //Param può valere ges, com, val o amm
                else if (Request.QueryString["Param"] != "ges" && Request.QueryString["Param"] != "com" && Request.QueryString["Param"] != "val"
                      && Request.QueryString["Param"] != "amm" && Request.QueryString["Param"] != "vis")
                {
                    throw new CustomExceptions.PaginaNonRichiamataCorrettamente(null, "Parametri errati");
                }
                else
                {

                    if (Session["Utente"] == null)
                    {
                        clsUtenteLogin oUtente = new clsUtenteLogin();
                        oUtente.UserId = User.Identity.Name.Substring(User.Identity.Name.IndexOf("\\") + 1);
                        //oUtente.UserId = "3444";
                        Session["Utente"] = oUtente;
                        Session["UtenteDip"] = oUtente;
                    }

                    Param = Request.QueryString["Param"];
                    Session["Param"] = Param;
                    switch (Param)
                    {
                        case "ges":
                            Response.Redirect("Gestione.aspx");
                            break;
                        case "com":
                            Response.Redirect("Compilazione.aspx");
                            break;
                        case "val":
                        case "amm":
                        case "vis":
                            Response.Cache.SetCacheability(HttpCacheability.NoCache);
                            Response.ExpiresAbsolute = DateTime.Now.AddMonths(-1);
                            Response.Redirect("Valutazione.aspx");
                            break;
                        default:
                            LblErr.Text = "Pagina non richiamata correttamente: parametri errati.";
                            break;
                    }
                }
            }
            catch (CustomExceptions.PaginaNonRichiamataCorrettamente ex)
            {
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + ex.CustomMessage + "');", true);
            }
        }
    }
}