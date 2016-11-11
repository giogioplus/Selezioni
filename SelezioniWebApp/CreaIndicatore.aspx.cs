using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DB_ODP;
using clDB;
using SelezioniObjects;

namespace SelezioniWebApp
{
    public partial class CreaIndicatore : System.Web.UI.Page
    {
        private static int SelezioneId;
        private static Selezione oSelezione;
        private static string CategoriaCod;
        private static int IndId;
        ClsDB DBUtility = new ClsDB();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    if (Request.QueryString.Count != 1 && Request.QueryString.Count != 3)
                    {
                        throw new CustomExceptions.PaginaNonRichiamataCorrettamente();
                    }
                    else if (Request.QueryString["SelezioneId"] == null)
                    {
                        throw new CustomExceptions.PaginaNonRichiamataCorrettamente("SelezioneId", null);
                    }
                    else if (Request.QueryString.Count == 3 && (Request.QueryString["CategoriaCod"] == null || Request.QueryString["IndId"] == null))
                    {
                        throw new CustomExceptions.PaginaNonRichiamataCorrettamente("CategoriaCod, IndId", null);
                    }
                    else if (Request.QueryString.Count == 3 && (string)Session["Operazione"] != TipoOp.ModInd.ToString())
                    {
                        throw new CustomExceptions.PaginaNonRichiamataCorrettamente("IndId", null);
                    }
                    //TODO: Gestire la chiamata della pagina dall'esterno del sito
                    //else if (Request.ServerVariables["HTTP_REFERER"]==null || Request.ServerVariables["HTTP_REFERER"].IndexOf("Costruzione.aspx") == 0)
                    //{
                    //    throw new CustomExceptions.PaginaNonRichiamataCorrettamente();
                    //}
                    SelezioneId = int.Parse(Request.QueryString["SelezioneId"].ToString());
                    oSelezione = DBUtility.LeggiSelezione(SelezioneId);
                    lbl_categoriacod.Text = oSelezione.CategoriaCod;
                    lbl_anno.Text = oSelezione.Anno.ToString();
                    lbl_titolo.Text = oSelezione.Titolo;
                    if (Request.QueryString.Count == 3)
                    {
                        CategoriaCod = Request.QueryString["CategoriaCod"].ToString();
                        IndId = int.Parse(Request.QueryString["IndId"].ToString());
                        Indicatore oIndicatore = DBUtility.LeggiIndicatori(oSelezione.SelezioneId, oSelezione.CategoriaCod, IndId)[0];
                        txt_Descr.Text = oIndicatore.Descr;
                        txt_NoteDip.Text = oIndicatore.NoteDip;
                        txt_NoteVal.Text = oIndicatore.NoteVal;
                        if (oIndicatore.DipFlg == 1)
                            chk_DipFlg.Checked = true;
                        if (oIndicatore.RspFlg == 1)
                            chk_RspFlg.Checked = true;
                        if (oIndicatore.AmmFlg == 1)
                            chk_AmmFlg.Checked = true;
                        txt_Ord.Text = oIndicatore.Ord.ToString();
                    }
                }
                LblErr.Text = "";
                if (Request.QueryString.Count == 3)
                {
                    Session["Stato"] = DBUtility.LeggiStato(SelezioneId, IndId, 0).ToString();
                    if ((string)Session["Stato"] == Stato.SoloTst.ToString())
                    {
                        btnConfermaInd.Enabled = false;
                        btnAnnullaInd.Enabled = false;
                    }
                    else
                    {

                        btnConfermaInd.Enabled = true;
                        btnAnnullaInd.Enabled = true;
                    }
                }
                else
                {
                    btnConfermaInd.Enabled = true;
                    btnAnnullaInd.Enabled = true;
                }
            }
            catch (CustomExceptions.PaginaNonRichiamataCorrettamente ex)
            {
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + ex.CustomMessage + "');", true);
                Page.Response.Redirect("Costruzione.aspx?SelezioneId=" + SelezioneId.ToString());
            }
        }

        protected void mnu_CreaIndicatore_MenuItemClick(object sender, MenuEventArgs e)
        {
            if (e.Item.Value == "Rit")
            {
                if (IndId == 0)
                {
                    Page.Response.Redirect("Costruzione.aspx?SelezioneId=" + SelezioneId.ToString());
                }
                else
                {
                    Page.Response.Redirect("Costruzione.aspx?SelezioneId=" + SelezioneId.ToString() + "&CategoriaCod=" + CategoriaCod + "&IndId=" + IndId.ToString());
                }
            }
            //else if (e.Item.Value == "Ins")
            //{
            //    txt_Descr.Text = "";
            //    txt_NoteDip.Text = "";
            //    txt_NoteVal.Text = "";
            //    chk_DipFlg.Checked = false;
            //    chk_RspFlg.Checked = false;
            //    chk_AmmFlg.Checked = false;
            //    Session["Operazione"] = TipoOp.InsInd.ToString();
            //}
        }

        protected void btnConfermaInd_Click(object sender, EventArgs e)
        {
            if (((Button)sender).ID == "btnConfermaInd")
            {
                Indicatore oIndicatore = new Indicatore();
                oIndicatore.SelezioneId = oSelezione.SelezioneId;
                oIndicatore.CategoriaCod = oSelezione.CategoriaCod;
                if ((string)Session["Operazione"] == TipoOp.ModInd.ToString())
                    oIndicatore.IndId = IndId;
                oIndicatore.Descr = txt_Descr.Text;
                oIndicatore.NoteDip = txt_NoteDip.Text;
                oIndicatore.NoteVal = txt_NoteVal.Text;
                oIndicatore.DipFlg = ((chk_DipFlg.Checked == true) ? 1 : 0);
                oIndicatore.RspFlg = ((chk_RspFlg.Checked == true) ? 1 : 0);
                oIndicatore.AmmFlg = ((chk_AmmFlg.Checked == true) ? 1 : 0);
                oIndicatore.Ord = int.Parse(txt_Ord.Text);
                try
                {
                    if ((string)Session["Operazione"] == TipoOp.InsInd.ToString())
                    {
                        DBUtility.ins_indicatore(oIndicatore, null, ((clsUtenteLogin)Session["Utente"]).UserId);
                        LblErr.Text = "Indicatore inserito con successo";
                    }
                    else if ((string)Session["Operazione"] == TipoOp.ModInd.ToString())
                    {
                        DBUtility.upd_indicatore(oIndicatore, null, ((clsUtenteLogin)Session["Utente"]).UserId);
                        LblErr.Text = "Indicatore modificato con successo";
                    }
                }
                catch (Exception ex)
                {
                    string Message = "Operazione di inserimento/modifica Indicatore fallita: " + ex.Message;
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + Message + "');", true);
                }
                finally
                {
                    Session["Operazione"] = TipoOp.NoOp.ToString();
                    btnConfermaInd.Enabled = false;
                    btnAnnullaInd.Enabled = false;
                }
            }
            else if (((Button)sender).ID == "btnAnnullaInd")
            {
                Session["Operazione"] = TipoOp.NoOp.ToString();
                if (IndId == 0)
                {
                    Page.Response.Redirect("Costruzione.aspx?SelezioneId=" + SelezioneId.ToString());
                }
                else
                {
                    Page.Response.Redirect("Costruzione.aspx?SelezioneId=" + SelezioneId.ToString() + "&CategoriaCod=" + CategoriaCod + "&IndId=" + IndId.ToString());
                }
            }
        }
    }
}