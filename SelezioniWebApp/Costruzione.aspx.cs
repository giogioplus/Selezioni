using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using clDB;
using SelezioniObjects;

namespace SelezioniWebApp
{
    public partial class Costruzione : System.Web.UI.Page
    {
        private int SelezioneId;
        private Selezione oSelezione;
        private string CategoriaCod;
        private int IndId;
        private Indicatore oIndicatore;
        ClsDB DBUtility = new ClsDB();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["SelezioneId"] != null)  // modifica testata
                {
                    SelezioneId = int.Parse(Request.QueryString["SelezioneId"].ToString());
                }
                else
                {
                    throw new CustomExceptions.PaginaNonRichiamataCorrettamente("SelezioneId", null);
                }
                oSelezione = DBUtility.LeggiSelezione(SelezioneId);
                if (Request.QueryString["CategoriaCod"] != null && Request.QueryString["IndId"] != null)
                {
                    CategoriaCod = Request.QueryString["CategoriaCod"].ToString();
                    IndId = int.Parse(Request.QueryString["IndId"].ToString());
                    oIndicatore = DBUtility.LeggiIndicatori(SelezioneId, CategoriaCod, IndId)[0];
                }
                if (!Page.IsPostBack)
                {

                    lbl_categoriacod.Text = oSelezione.CategoriaCod;
                    lbl_anno.Text = oSelezione.Anno.ToString();
                    lbl_titolo.Text = oSelezione.Titolo;
                    lbl_descrizione.Text = oSelezione.Descrizione;
                    grdIndicatori.DataSource = DBUtility.LeggiIndicatori(oSelezione.SelezioneId, oSelezione.CategoriaCod, null);
                    grdIndicatori.DataBind();
                    if (Request.QueryString["CategoriaCod"] != null && Request.QueryString["IndId"] != null)
                    {
                        hdnIndId.Value = IndId.ToString();
                        grdIndicatoriDet.DataSource = DBUtility.LeggiIndicatoriDet(SelezioneId, CategoriaCod, IndId, null);
                        grdIndicatoriDet.DataBind();
                    }
                    this.ClientScript.GetPostBackEventReference(this, string.Empty);
                }
            }
            catch (CustomExceptions.PaginaNonRichiamataCorrettamente ex)
            {
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + ex.CustomMessage + "');", true);
            }
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            if ((string)Session["Operazione"] == TipoOp.DuplInd.ToString())
            {
                try
                {
                    DBUtility.dupl_indicatori(oSelezione.SelezioneId, oSelezione.SelezioneId, int.Parse(hdnIndId.Value), oSelezione.Anno, oSelezione.CategoriaCod, null, ((clsUtenteLogin)Session["Utente"]).UserId);
                }
                catch (Exception ex)
                {
                    string Message = "Duplicazione indicatore fallita: " + ex.Message;
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + Message + "');", true);
                }
                Session["Operazione"] = TipoOp.NoOp.ToString();
                grdIndicatori.DataSource = DBUtility.LeggiIndicatori(oSelezione.SelezioneId, oSelezione.CategoriaCod, null);
                grdIndicatori.DataBind();
            }
            if ((string)Session["Operazione"] == TipoOp.DelInd.ToString())
            {
                try
                {
                    Indicatore oIndicatore = DBUtility.LeggiIndicatori(oSelezione.SelezioneId, oSelezione.CategoriaCod, int.Parse(hdnIndId.Value))[0];
                    DBUtility.del_indicatore(oIndicatore, null, ((clsUtenteLogin)Session["Utente"]).UserId);
                }
                catch (Exception ex)
                {
                    string Message = "Cancellazione indicatore fallita: " + ex.Message;
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + Message + "');", true);
                }
                Session["Operazione"] = TipoOp.NoOp.ToString();
                grdIndicatori.DataSource = DBUtility.LeggiIndicatori(oSelezione.SelezioneId, oSelezione.CategoriaCod, null);
                grdIndicatori.DataBind();
                hdnRigaIndicatoreSel.Value = "";
                hdnIndId.Value = "0";
            }
            if ((string)Session["Operazione"] == TipoOp.DuplIndDet.ToString())
            {
                try
                {
                    DBUtility.dupl_indicatori_det(oSelezione.SelezioneId, oSelezione.CategoriaCod, int.Parse(hdnIndId.Value), int.Parse(hdnIndDetId.Value), null, ((clsUtenteLogin)Session["Utente"]).UserId);
                }
                catch (Exception ex)
                {
                    string Message = "Duplicazione dettaglio fallita: " + ex.Message;
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + Message + "');", true);
                }
                Session["Operazione"] = TipoOp.NoOp.ToString();
                grdIndicatoriDet.DataSource = DBUtility.LeggiIndicatoriDet(oSelezione.SelezioneId, oSelezione.CategoriaCod, int.Parse(hdnIndId.Value), null);
                grdIndicatoriDet.DataBind();
            }
            if ((string)Session["Operazione"] == TipoOp.DelIndDet.ToString())
            {
                try
                {
                    IndicatoreDet oIndicatoreDet = DBUtility.LeggiIndicatoriDet(oSelezione.SelezioneId, oSelezione.CategoriaCod, int.Parse(hdnIndId.Value), int.Parse(hdnIndDetId.Value))[0];
                    DBUtility.del_indicatore_det(oIndicatoreDet, null, ((clsUtenteLogin)Session["Utente"]).UserId);
                }
                catch (Exception ex)
                {
                    string Message = "Cancellazione dettaglio fallita: " + ex.Message;
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + Message + "');", true);
                }
                Session["Operazione"] = TipoOp.NoOp.ToString();
                grdIndicatoriDet.DataSource = DBUtility.LeggiIndicatoriDet(oSelezione.SelezioneId, oSelezione.CategoriaCod, int.Parse(hdnIndId.Value), null);
                grdIndicatoriDet.DataBind();
                hdnRigaIndicatoreDetSel.Value = "";
                hdnIndDetId.Value = "0";
            }

            //Gestione menù in funzione dello Stato
            //abilita_disabilita_menu(bool pModificaTestata, bool pOperInd, bool pSoloInsInd, bool pSoloModInd, bool pOperIndDet, bool pSoloInsIndDet, bool pSoloModIndDet, bool pRitorna)
            Session["Stato"] = DBUtility.LeggiStato(SelezioneId, int.Parse(hdnIndId.Value), 0).ToString();
            if ((string)Session["Stato"] == Stato.Tst.ToString())
                abilita_disabilita_menu(true, false, true, false, false, false, false, true);
            else if ((string)Session["Stato"] == Stato.Ind.ToString())
                //abilita_disabilita_menu(true, true, true, true, false, false, true, true);
                abilita_disabilita_menu(true, true, true, true, false, true, true, true); //>>ga022015<<
            else if ((string)Session["Stato"] == Stato.Det.ToString())
                abilita_disabilita_menu(true, true, true, true, true, true, true, true);
            else if ((string)Session["Stato"] == Stato.SoloTst.ToString())
                abilita_disabilita_menu(true, false, false, true, false, false, true, true);
        }

        protected void mnu_CostruisciSelezione_MenuItemClick(object sender, MenuEventArgs e)
        {
            if (e.Item.Value == "ModT")
            {
                Response.Redirect("CreaSelezione.aspx?SelezioneId=" + SelezioneId.ToString());
            }
            else if (e.Item.Value == "InsInd")
            {

                Session["Operazione"] = TipoOp.InsInd.ToString();
                Response.Redirect("CreaIndicatore.aspx?SelezioneId=" + oSelezione.SelezioneId.ToString());
            }
            else if (e.Item.Value == "ModInd")
            {
                if (hdnIndId.Value == "0")
                {
                    string Message = "Devi selezionare un indicatore per modificarlo";
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + Message + "');", true);
                }
                else
                {
                    Session["Operazione"] = TipoOp.ModInd.ToString();
                    Response.Redirect("CreaIndicatore.aspx?SelezioneId=" + oSelezione.SelezioneId.ToString() + "&CategoriaCod=" + oSelezione.CategoriaCod + "&IndId=" + hdnIndId.Value);
                }
            }
            else if (e.Item.Value == "DuplInd")
            {
                if (hdnIndId.Value == "0")
                {
                    string Message = "Devi selezionare un indicatore per duplicarlo";
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + Message + "');", true);
                }
                else
                {
                    Session["Operazione"] = TipoOp.DuplInd.ToString();
                }
            }
            else if (e.Item.Value == "DelInd")
            {
                if (hdnIndId.Value == "0")
                {
                    string Message = "Devi selezionare un indicatore per cancellarlo";
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + Message + "');", true);
                }
                else
                {
                    Session["Operazione"] = TipoOp.DelInd.ToString();
                }
            }
            else if (e.Item.Value == "InsIndDet")
            {
                Session["Operazione"] = TipoOp.InsIndDet.ToString();
                Response.Redirect("CreaIndicatoreDet.aspx?SelezioneId=" + oSelezione.SelezioneId.ToString() + "&CategoriaCod=" + oSelezione.CategoriaCod + "&IndId=" + hdnIndId.Value);
            }
            else if (e.Item.Value == "ModIndDet")
            {
                if (hdnIndDetId.Value == "0")
                {
                    string Message = "Devi selezionare un dettaglio per modificarlo";
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + Message + "');", true);
                }
                else
                {
                    Session["Operazione"] = TipoOp.NoOp.ToString();
                    Response.Redirect("CreaIndicatoreDet.aspx?SelezioneId=" + oSelezione.SelezioneId.ToString() + "&CategoriaCod=" + oSelezione.CategoriaCod + "&IndId=" + hdnIndId.Value + "&IndDetId=" + hdnIndDetId.Value);
                }
            }
            else if (e.Item.Value == "DuplIndDet")
            {
                if (hdnIndDetId.Value == "0")
                {
                    string Message = "Devi selezionare un dettaglio per duplicarlo";
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + Message + "');", true);
                }
                else
                {
                    Session["Operazione"] = TipoOp.DuplIndDet.ToString();
                }
            }
            else if (e.Item.Value == "DelIndDet")
            {
                if (hdnIndDetId.Value == "0")
                {
                    string Message = "Devi selezionare un dettaglio per cancellarlo";
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + Message + "');", true);
                }
                else
                {
                    Session["Operazione"] = TipoOp.DelIndDet.ToString();
                }
            }
            else if (e.Item.Value == "Rit")
            {
                Session["Operazione"] = TipoOp.NoOp.ToString();
                Response.Redirect("SelSelezione.aspx");
            }
        }

        protected void grdIndicatori_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Indicatore selInd = e.Row.DataItem as Indicatore;
                e.Row.Attributes.Add("onclick", "GrdIndicatori_selectRow(this,'" + selInd.IndId + "')");
                e.Row.Attributes.Add("onmouseover", "GridView_mouseHover(this)");
            }
        }

        protected void grdIndicatoriDeti_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                IndicatoreDet oRDet = e.Row.DataItem as IndicatoreDet;
                e.Row.Attributes.Add("onclick", "GrdIndicatoriDet_selectRow(this,'" + oRDet.IndDetId + "')");
                e.Row.Attributes.Add("onmouseover", "GridView_mouseHover(this)");
            }
        }

        protected void btnPopulate_Click(object sender, EventArgs e)
        {
            int intIndex = Convert.ToInt16(Request.Form["__EVENTARGUMENT"].ToString());
            FillDestinationGrid(intIndex);
        }

        private void FillDestinationGrid(int intIndex)
        {
            grdIndicatoriDet.DataSource = DBUtility.LeggiIndicatoriDet(oSelezione.SelezioneId, oSelezione.CategoriaCod, intIndex, null);
            grdIndicatoriDet.DataBind();
            if (grdIndicatoriDet.Rows.Count > 0)
            {
                string TitInd = DBUtility.LeggiIndicatori(oSelezione.SelezioneId, oSelezione.CategoriaCod, intIndex)[0].Descr;
                //TitInd = (TitInd.Length < 80) ? TitInd : TitInd.Substring(0,80) + "...";
                panel2.GroupingText = "Dettaglio: " + TitInd;
            }
        }

        protected bool ConvertiFlg(int pFlg)
        {
            return ((pFlg == 1) ? true : false);
        }

        private void abilita_disabilita_menu(bool pModificaTestata, bool pOperInd, bool pSoloInsInd, bool pSoloModInd, bool pOperIndDet, bool pSoloInsIndDet, bool pSoloModIndDet, bool pRitorna)
        {
            foreach (MenuItem i in mnu_CostruisciSelezione.Items)
            {
                if (i.Value == "ModT")
                {
                    i.Selectable = pModificaTestata;
                }
                if ((i.Value != "Rit") && (i.Value != "ModT"))
                {
                    if (i.Text == "Indicatori")
                    {
                        foreach (MenuItem j in i.ChildItems)
                        {
                            if (j.Value == "InsInd")
                            {
                                j.Enabled = pSoloInsInd;
                            }
                            else if (j.Value == "ModInd")
                            {
                                j.Enabled = pSoloModInd;
                            }
                            else
                            {
                                j.Enabled = pOperInd;
                            }
                        }
                    }
                    if (i.Text == "Dettagli")
                    {
                        foreach (MenuItem j in i.ChildItems)
                        {
                            if (j.Value == "InsIndDet")
                            {
                                j.Enabled = pSoloInsIndDet;
                            }
                            else if (j.Value == "ModIndDet")
                            {
                                j.Enabled = pSoloModIndDet;
                            }
                            else
                            {
                                j.Enabled = pOperIndDet;
                            }
                        }
                    }
                }
                else if (i.Value == "Rit")
                {
                    i.Selectable = pRitorna;
                }
            }
        }
    }
}