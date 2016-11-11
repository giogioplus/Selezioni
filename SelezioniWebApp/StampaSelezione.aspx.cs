using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SelezioniRep;
using SelezioniObjects;
using clDB;

namespace SelezioniWebApp
{
    public partial class StampaSelezione : System.Web.UI.Page
    {
        private int SelezioneId;
        private int CompId;
        private StatoValutazione oStatoValutazione;
        ClsDB DBUtility = new ClsDB();
        protected void Page_Load(object sender, EventArgs e)
        {
            bool flgArc = false;
            try
            {
                if (Session["Param"] == null)
                {
                    throw new CustomExceptions.SessioneScaduta();
                }
                else
                {
                    if (Request.QueryString["SelezioneId"] == null || (Request.QueryString.Count == 2 && Request.QueryString["CompId"] == null))
                    {
                        throw new CustomExceptions.PaginaNonRichiamataCorrettamente("SelezioneId", null);
                    }
                    else
                    {
                        SelezioneId = int.Parse(Request.QueryString["SelezioneId"].ToString());
                        if (Request.QueryString["CompId"] != null)
                        {
                            CompId = int.Parse(Request.QueryString["CompId"].ToString());
                        }
                        else
                        {
                            CompId = 0;
                        }
                        if (Request.QueryString.Count == 3)
                            flgArc = true;
                    }
                    //StatoValutazione

                    oStatoValutazione = DBUtility.AggiornaStatoValutazione(SelezioneId, CompId, (string)Session["Param"]);
                    if (oStatoValutazione.RisposteVis == "T")
                        abilita_disabilita_menu(true);
                    else
                        abilita_disabilita_menu(false);
                }
            }
            catch (CustomExceptions.SessioneScaduta ex)
            {
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + ex.CustomMessage + "');", true);
            }
        }

        protected void mnu_main_MenuItemClick(object sender, MenuEventArgs e)
        {
            bool flgArc = false;
            if (Request.QueryString.Count == 3)
                flgArc = true;

            if (e.Item.Value == "Rit")
            {
                if ((string)Session["Param"] == "ges")
                {
                    Page.Response.Redirect("SelSelezione.aspx");
                }
                else
                {
                    if (!flgArc)
                        Page.Response.Redirect("Compilazione.aspx");
                    else
                        Page.Response.Redirect("Archivio.aspx");
                }
            }
            else if (e.Item.Value == "Dom" || e.Item.Value == "Val")
            {
                Telerik.Reporting.Report oReport;
                if (!flgArc)
                {
                    if (e.Item.Value == "Dom")
                        oReport = new SchedaRepDom(SelezioneId, e.Item.Value, CompId, ((clsUtenteLogin)Session["UtenteDip"]).UserId);
                    else
                        oReport = new SchedaRepVal(SelezioneId, e.Item.Value, CompId, ((clsUtenteLogin)Session["UtenteDip"]).UserId);
                }
                else // stampo una compilazione archiviata //>>ga24072013<<
                {
                    if (e.Item.Value == "Dom")
                        oReport = new SchedaRepDomArchivio(SelezioneId, e.Item.Value, CompId, ((clsUtenteLogin)Session["UtenteDip"]).UserId);
                    else
                        oReport = new SchedaRepValArchivio(SelezioneId, e.Item.Value, CompId, ((clsUtenteLogin)Session["UtenteDip"]).UserId);
                }

                
                oReport.PageSettings.Landscape = true;
                oReport.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
                Telerik.Reporting.Drawing.Unit w;
                if (oReport.PageSettings.Landscape)
                {
                    w = oReport.PageSettings.PaperSize.Height - oReport.PageSettings.Margins.Top - oReport.PageSettings.Margins.Bottom;
                }
                else
                {
                    w = oReport.PageSettings.PaperSize.Width - oReport.PageSettings.Margins.Left - oReport.PageSettings.Margins.Right;
                }

                oReport.Width = w;
                rpv_StampaSelezione.Report = oReport;
                rpv_StampaSelezione.RefreshReport();
            }
        }

        private void abilita_disabilita_menu(bool pVal)
        {
            foreach (MenuItem i in mnu_main.Items)
            {
                if (i.Value == "Val")
                {
                    i.Selectable = pVal;
                }
                
            }
        }


    }
}