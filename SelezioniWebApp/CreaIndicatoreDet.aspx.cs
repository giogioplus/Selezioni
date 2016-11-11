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
    public partial class CreaIndicatoreDet : System.Web.UI.Page
    {
        private static int SelezioneId;
        private static Selezione oSelezione;
        private static string CategoriaCod;
        private static int IndId;
        private static Indicatore oIndicatore;
        private static int IndDetId;
        private static int TipoRiga;
        private static int RigaId;
        private static int PuntId;
        ClsDB DBUtility = new ClsDB();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    //La pagina può essere chiamata con 3 parametri (Inserimento dettaglio) o con 4 parametri (Modifica dettaglio)
                    if (Request.QueryString.Count != 3 && Request.QueryString.Count != 4)
                    {
                        throw new CustomExceptions.PaginaNonRichiamataCorrettamente();
                    }
                    else if (Request.QueryString["SelezioneId"] == null || Request.QueryString["CategoriaCod"] == null || Request.QueryString["IndId"] == null)
                    {
                        throw new CustomExceptions.PaginaNonRichiamataCorrettamente(null, "Neccesari i parametri: SelezioneId, CategoriaCod, IndId");
                    }
                    else if (Request.QueryString.Count == 4 && Request.QueryString["IndDetId"] == null)
                    {
                        throw new CustomExceptions.PaginaNonRichiamataCorrettamente("IndDetId", null);
                    }
                    SelezioneId = int.Parse(Request.QueryString["SelezioneId"].ToString());
                    oSelezione = DBUtility.LeggiSelezione(SelezioneId);
                    CategoriaCod = Request.QueryString["CategoriaCod"].ToString();
                    lbl_categoriacod.Text = CategoriaCod;
                    lbl_anno.Text = oSelezione.Anno.ToString();
                    lbl_titolo.Text = oSelezione.Titolo;
                    IndId = int.Parse(Request.QueryString["IndId"].ToString());
                    oIndicatore = DBUtility.LeggiIndicatori(SelezioneId, CategoriaCod, IndId)[0];
                    lbl_descr.Text = oIndicatore.Descr;
                    IndDetId = 0;
                    if (Request.QueryString.Count == 4)
                    {
                        IndDetId = int.Parse(Request.QueryString["IndDetId"].ToString());
                        IndicatoreDet oIndicatoreDet = DBUtility.LeggiIndicatoriDet(SelezioneId, CategoriaCod, IndId, IndDetId)[0];
                        txt_DescrDet.Text = oIndicatoreDet.DescrDet;
                        txt_NoteDetDip.Text = oIndicatoreDet.NoteDetDip;
                        txt_NoteDetVal.Text = oIndicatoreDet.NoteDetVal;
                        txt_Ord.Text = oIndicatoreDet.Ord.ToString();
                        txt_MaxRighe.Text = oIndicatoreDet.MaxRighe.ToString();
                    }
                }
                else
                {
                }
                LblErr.Text = "";
                if (IndDetId > 0)
                {
                    grdIndicatoriDetRiga.DataSource = DBUtility.LeggiIndicatoriDetRiga(SelezioneId, CategoriaCod, IndId, IndDetId, null, null);
                    grdIndicatoriDetRiga.DataBind();
                    grdIndicatoriDetPunt.DataSource = DBUtility.LeggiIndicatoriDetPunt(SelezioneId, CategoriaCod, IndId, IndDetId, null);
                    grdIndicatoriDetPunt.DataBind();
                }
            }
            catch (CustomExceptions.PaginaNonRichiamataCorrettamente ex)
            {
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + ex.CustomMessage + "');", true);
                abilita_disabilita_menu(false, false, false, false, false, true);
            }
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            if ((string)Session["Operazione"] == TipoOp.InsIndDet.ToString() || (string)Session["Operazione"] == TipoOp.ModIndDet.ToString())
            {
                btnConfermaIndDet.Enabled = true;
                btnAnnullaIndDet.Enabled = true;
                btnConfermaIndDetRiga.Enabled = false;
                btnAnnullaIndDetRiga.Enabled = false;
                btnConfermaIndDetPunt.Enabled = false;
                btnAnnullaIndDetPunt.Enabled = false;
                indicatoredet_riga_grd.Visible = true;
                indicatoredet_punt_grd.Visible = true;
                indicatoredet_riga_creamod.Visible = false;
                indicatoredet_punt_creamod.Visible = false;
                abilita_disabilita_menu(false, false, false, false, false, true);
                abilita_disabilita_controlli(true);

            }
            else if ((string)Session["Operazione"] == TipoOp.InsRiga.ToString() || (string)Session["Operazione"] == TipoOp.ModRiga.ToString())
            {
                btnConfermaIndDet.Enabled = false;
                btnAnnullaIndDet.Enabled = false;
                btnConfermaIndDetRiga.Enabled = true;
                btnAnnullaIndDetRiga.Enabled = true;
                btnConfermaIndDetPunt.Enabled = false;
                btnAnnullaIndDetPunt.Enabled = false;
                indicatoredet_riga_grd.Visible = true;
                indicatoredet_punt_grd.Visible = true;
                indicatoredet_riga_creamod.Visible = true;
                indicatoredet_punt_creamod.Visible = false;

                //Quando viene richiamato il metodo per un controllo ko in fase di ins/mod non inizializzo i campi
                if (Session["CorrezioneControlli"] == null)
                {
                    IList<clsTipoCtrl> lTipoCtrl = new List<clsTipoCtrl>();
                    lTipoCtrl = DBUtility.PopolaDDLTipoCtrl();
                    ddl_TipoCtrl.DataSource = lTipoCtrl;
                    ddl_TipoCtrl.DataValueField = "TipoCtrl";
                    ddl_TipoCtrl.DataTextField = "DescrTipo";
                    ddl_TipoCtrl.DataBind();
                    IList<clsTipoRiga> lTipoRiga = new List<clsTipoRiga>();
                    lTipoRiga = DBUtility.PopolaDDLTipoRiga();
                    ddl_TipoRiga.DataSource = lTipoRiga;
                    ddl_TipoRiga.DataValueField = "TipoRiga";
                    ddl_TipoRiga.DataTextField = "DescrRiga";
                    ddl_TipoRiga.DataBind();
                    if ((string)Session["Operazione"] == TipoOp.InsRiga.ToString())
                    {
                        txt_DescrRiga.Text = "";
                        txt_OrdRiga.Text = "";
                        ddl_TipoCtrl.SelectedIndex = 0;
                        ddl_TipoRiga.SelectedIndex = 0;
                    }
                    else
                    {
                        IndicatoreDetRiga oIndicatoreDetRiga = DBUtility.LeggiIndicatoriDetRiga(SelezioneId, CategoriaCod, IndId, IndDetId, TipoRiga, RigaId)[0];
                        ddl_TipoRiga.SelectedIndex = oIndicatoreDetRiga.TipoRiga;
                        //In mod non lascio modificare TipoRiga perchè chiave
                        ddl_TipoRiga.Enabled = false;
                        txt_DescrRiga.Text = oIndicatoreDetRiga.DescrRiga;
                        txt_OrdRiga.Text = oIndicatoreDetRiga.OrdRiga.ToString();
                        ddl_TipoCtrl.SelectedIndex = oIndicatoreDetRiga.TipoCtrl;
                    }
                }
                Session["CorrezioneControlli"] = null;
                abilita_disabilita_menu(false, false, false, false, false, true);
            }
            else if ((string)Session["Operazione"] == TipoOp.DelRiga.ToString())
            {
                IndicatoreDetRiga oIndicatoreDetRiga = DBUtility.LeggiIndicatoriDetRiga(SelezioneId, CategoriaCod, IndId, IndDetId, TipoRiga, RigaId)[0];
                DBUtility.del_indicatore_det_riga(oIndicatoreDetRiga, null, ((clsUtenteLogin)Session["Utente"]).UserId);
                Session["Operazione"] = TipoOp.NoOp.ToString();
                btnConfermaIndDet.Enabled = false;
                btnAnnullaIndDet.Enabled = false;
                btnConfermaIndDetRiga.Enabled = false;
                btnAnnullaIndDetRiga.Enabled = false;
                btnConfermaIndDetPunt.Enabled = false;
                btnAnnullaIndDetPunt.Enabled = false;
                indicatoredet_riga_grd.Visible = true;
                indicatoredet_punt_grd.Visible = true;
                indicatoredet_riga_creamod.Visible = false;
                indicatoredet_punt_creamod.Visible = false;
                grdIndicatoriDetRiga.DataSource = DBUtility.LeggiIndicatoriDetRiga(SelezioneId, CategoriaCod, IndId, IndDetId, null, null);
                grdIndicatoriDetRiga.DataBind();
                hdnRigaIndicatoreDetRigaSel.Value = "";
                hdnTipoRiga.Value = "0";
                hdnRigaId.Value = "0";
            }
            else if ((string)Session["Operazione"] == TipoOp.DelPunt.ToString())
            {
                IndicatoreDetPunt oIndicatoreDetPunt = DBUtility.LeggiIndicatoriDetPunt(SelezioneId, CategoriaCod, IndId, IndDetId, PuntId)[0];
                DBUtility.del_indicatore_det_punt(oIndicatoreDetPunt, null, ((clsUtenteLogin)Session["Utente"]).UserId);
                Session["Operazione"] = TipoOp.NoOp.ToString();
                btnConfermaIndDet.Enabled = false;
                btnAnnullaIndDet.Enabled = false;
                btnConfermaIndDetRiga.Enabled = false;
                btnAnnullaIndDetRiga.Enabled = false;
                btnConfermaIndDetPunt.Enabled = false;
                btnAnnullaIndDetPunt.Enabled = false;
                indicatoredet_riga_grd.Visible = true;
                indicatoredet_punt_grd.Visible = true;
                indicatoredet_riga_creamod.Visible = false;
                indicatoredet_punt_creamod.Visible = false;
                grdIndicatoriDetPunt.DataSource = DBUtility.LeggiIndicatoriDetPunt(SelezioneId, CategoriaCod, IndId, IndDetId, null);
                grdIndicatoriDetPunt.DataBind();
                hdnRigaIndicatoreDetPuntSel.Value = "";
                hdnPuntId.Value = "0";
            }
            else if ((string)Session["Operazione"] == TipoOp.InsPunt.ToString() || (string)Session["Operazione"] == TipoOp.ModPunt.ToString())
            {
                btnConfermaIndDet.Enabled = false;
                btnAnnullaIndDet.Enabled = false;
                btnConfermaIndDetRiga.Enabled = false;
                btnAnnullaIndDetRiga.Enabled = false;
                btnConfermaIndDetPunt.Enabled = true;
                btnAnnullaIndDetPunt.Enabled = true;
                indicatoredet_riga_grd.Visible = true;
                indicatoredet_punt_grd.Visible = true;
                indicatoredet_riga_creamod.Visible = false;
                indicatoredet_punt_creamod.Visible = true;

                if ((string)Session["Operazione"] == TipoOp.InsPunt.ToString())
                {
                    txt_DescrPunt.Text = "";
                    txt_Punt.Text = "0,00";
                }
                else
                {
                    IndicatoreDetPunt oIndicatoreDetPunt = DBUtility.LeggiIndicatoriDetPunt(SelezioneId, CategoriaCod, IndId, IndDetId, PuntId)[0];
                    txt_DescrPunt.Text = oIndicatoreDetPunt.DescrPunt;
                    txt_Punt.Text = oIndicatoreDetPunt.Punt.ToString();
                }
                abilita_disabilita_menu(false, false, false, false, false, true);
            }
            else if ((string)Session["Operazione"] == TipoOp.NoOp.ToString())
            {
                btnConfermaIndDet.Enabled = false;
                btnAnnullaIndDet.Enabled = false;
                btnConfermaIndDetRiga.Enabled = false;
                btnAnnullaIndDetRiga.Enabled = false;
                btnConfermaIndDetPunt.Enabled = false;
                btnAnnullaIndDetPunt.Enabled = false;
                indicatoredet_riga_grd.Visible = true;
                indicatoredet_punt_grd.Visible = true;
                indicatoredet_riga_creamod.Visible = false;
                indicatoredet_punt_creamod.Visible = false;

                abilita_disabilita_controlli(false);

                //Gestione menù in funzione dello Stato
                Session["Stato"] = DBUtility.LeggiStato(SelezioneId, IndId, IndDetId).ToString();
                if ((string)Session["Stato"] == Stato.Ind.ToString())
                    abilita_disabilita_menu(true, false, false, false, false, true);
                else if ((string)Session["Stato"] == Stato.Det.ToString())
                    abilita_disabilita_menu(true, false, true, false, true, true);
                else if ((string)Session["Stato"] == Stato.Riga.ToString())
                    abilita_disabilita_menu(true, true, true, false, true, true);
                else if ((string)Session["Stato"] == Stato.Punt.ToString())
                    abilita_disabilita_menu(true, false, true, true, true, true);
                else if ((string)Session["Stato"] == Stato.RigaPunt.ToString())
                    abilita_disabilita_menu(true, true, true, true, true, true);
                else if ((string)Session["Stato"] == Stato.SoloTst.ToString())
                    abilita_disabilita_menu(false, false, false, false, false, true);
            }


        }

        private void abilita_disabilita_menu(bool pModificaDettaglio, bool pOperRighe, bool pSoloInsRighe, bool pOperPunt, bool pSoloInsPunt, bool pRitorna)
        {
            foreach (MenuItem i in mnu_CreaIndicatoreDet.Items)
            {
                if (i.Value == "ModDet")
                {
                    i.Selectable = pModificaDettaglio;
                }
                if ((i.Value != "Rit") && (i.Value != "Ins"))
                {
                    if (i.Text == "Righe")
                    {
                        foreach (MenuItem j in i.ChildItems)
                        {
                            if (j.Value == "InsRiga")
                            {
                                j.Enabled = pSoloInsRighe;
                            }
                            else
                            {
                                j.Enabled = pOperRighe;
                            }
                        }
                    }
                    if (i.Text == "Punteggi")
                    {
                        foreach (MenuItem j in i.ChildItems)
                        {
                            if (j.Value == "InsPunt")
                            {
                                j.Enabled = pSoloInsPunt;
                            }
                            else
                            {
                                j.Enabled = pOperPunt;
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

        private void abilita_disabilita_controlli(bool pAbilita)
        {
            txt_DescrDet.Enabled = pAbilita;
            txt_NoteDetDip.Enabled = pAbilita;
            txt_NoteDetVal.Enabled = pAbilita;
            txt_Ord.Enabled = pAbilita;
            txt_MaxRighe.Enabled = pAbilita;
        }

        protected void mnu_CreaIndicatoreDet_MenuItemClick(object sender, MenuEventArgs e)
        {
            if (e.Item.Value == "Rit")
            {
                Page.Response.Redirect("Costruzione.aspx?SelezioneId=" + SelezioneId.ToString() + "&CategoriaCod=" + CategoriaCod + "&IndId=" + IndId.ToString());
            }
            else if (e.Item.Value == "ModDet")
            {
                //txt_DescrDet.Text = "";
                //txt_NoteDetDip.Text = "";
                //txt_NoteDetVal.Text = "";
                //txt_Ord.Text = "";
                //txt_MaxRighe.Text = "";
                Session["Operazione"] = TipoOp.ModIndDet.ToString();
            }
            else if (e.Item.Value == "InsRiga")
            {
                Session["Operazione"] = TipoOp.InsRiga.ToString();
            }
            else if (e.Item.Value == "ModRiga")
            {
                if (hdnRigaId.Value == "0")
                {
                    string Message = "Devi selezionare una riga per modificarla";
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + Message + "');", true);
                }
                else
                {
                    TipoRiga = int.Parse(hdnTipoRiga.Value);
                    RigaId = int.Parse(hdnRigaId.Value);
                    Session["Operazione"] = TipoOp.ModRiga.ToString();
                }
            }
            else if (e.Item.Value == "DelRiga")
            {
                if (hdnRigaId.Value == "0")
                {
                    string Message = "Devi selezionare una riga per cancellarla";
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + Message + "');", true);
                }
                else
                {
                    TipoRiga = int.Parse(hdnTipoRiga.Value);
                    RigaId = int.Parse(hdnRigaId.Value);
                    Session["Operazione"] = TipoOp.DelRiga.ToString();
                }
            }
            else if (e.Item.Value == "InsPunt")
            {
                Session["Operazione"] = TipoOp.InsPunt.ToString();
            }
            else if (e.Item.Value == "ModPunt")
            {
                if (hdnPuntId.Value == "0")
                {
                    string Message = "Devi selezionare un punteggio per modificarlo";
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + Message + "');", true);
                }
                else
                {
                    PuntId = int.Parse(hdnPuntId.Value);
                    Session["Operazione"] = TipoOp.ModPunt.ToString();
                }
            }
            else if (e.Item.Value == "DelPunt")
            {
                if (hdnPuntId.Value == "0")
                {
                    string Message = "Devi selezionare un punteggio per cancellarlo";
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + Message + "');", true);
                }
                else
                {
                    PuntId = int.Parse(hdnPuntId.Value);
                    Session["Operazione"] = TipoOp.DelPunt.ToString();
                }
            }
        }

        protected void btnConfermaIndDet_Click(object sender, EventArgs e)
        {
            if (((Button)sender).ID == "btnConfermaIndDet")
            {
                IndicatoreDet oIndicatoreDet = new IndicatoreDet();
                oIndicatoreDet.SelezioneId = oSelezione.SelezioneId;
                oIndicatoreDet.CategoriaCod = CategoriaCod;
                oIndicatoreDet.IndId = IndId;
                if ((string)Session["Operazione"] == TipoOp.ModIndDet.ToString())
                    oIndicatoreDet.IndDetId = IndDetId;
                else if ((string)Session["Operazione"] == TipoOp.InsIndDet.ToString())
                    oIndicatoreDet.IndDetId = DBUtility.rec_inddetid(oSelezione.SelezioneId, CategoriaCod, IndId, null);
                oIndicatoreDet.DescrDet = txt_DescrDet.Text;
                oIndicatoreDet.NoteDetDip = txt_NoteDetDip.Text;
                oIndicatoreDet.NoteDetVal = txt_NoteDetVal.Text;
                oIndicatoreDet.Ord = int.Parse(txt_Ord.Text);
                oIndicatoreDet.MaxRighe = int.Parse(txt_MaxRighe.Text);
                try
                {
                    if ((string)Session["Operazione"] == TipoOp.InsIndDet.ToString())
                    {
                        DBUtility.ins_indicatore_det(oIndicatoreDet, null, ((clsUtenteLogin)Session["Utente"]).UserId);
                        LblErr.Text = "Indicatore inserito con successo";
                        // Solo se sono in inserimento e l'operazione è ok assegno la var statica IndDetId
                        IndDetId = oIndicatoreDet.IndDetId;
                    }
                    else if ((string)Session["Operazione"] == TipoOp.ModIndDet.ToString())
                    {
                        DBUtility.upd_indicatore_det(oIndicatoreDet, null, ((clsUtenteLogin)Session["Utente"]).UserId);
                        LblErr.Text = "Dettaglio modificato con successo";
                    }
                }
                catch (Exception ex)
                {
                    string Message = "Operazione di inserimento/modifica Dettaglio fallita: " + ex.Message;
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + Message + "');", true);
                }
                finally
                {
                    Session["Operazione"] = TipoOp.NoOp.ToString();
                }
            }
            else if (((Button)sender).ID == "btnAnnullaIndDet")
            {
                Page.Response.Redirect("Costruzione.aspx?SelezioneId=" + SelezioneId.ToString());
            }
        }

        protected void btnConfermaIndDetRiga_Click(object sender, EventArgs e)
        {
            if (((Button)sender).ID == "btnConfermaIndDetRiga")
            {
                try
                {
                    IndicatoreDetRiga oIndicatoreDetRiga = new IndicatoreDetRiga();
                    oIndicatoreDetRiga.SelezioneId = oSelezione.SelezioneId;
                    oIndicatoreDetRiga.CategoriaCod = CategoriaCod;
                    oIndicatoreDetRiga.IndId = IndId;
                    oIndicatoreDetRiga.IndDetId = IndDetId;
                    if (ddl_TipoRiga.SelectedIndex == 0)
                        throw new CustomExceptions.Controlli("Occorre selezionare una Tipologia della riga");
                    oIndicatoreDetRiga.TipoRiga = ddl_TipoRiga.SelectedIndex;
                    if ((string)Session["Operazione"] == TipoOp.ModRiga.ToString())
                        oIndicatoreDetRiga.RigaId = RigaId;
                    else if ((string)Session["Operazione"] == TipoOp.InsRiga.ToString())
                        oIndicatoreDetRiga.RigaId = DBUtility.rec_inddetrigaid(oSelezione.SelezioneId, CategoriaCod, IndId, IndDetId, ddl_TipoRiga.SelectedIndex, null);
                    if (ddl_TipoCtrl.SelectedIndex == 0)
                        throw new CustomExceptions.Controlli("Occorre selezionare una Tipologia del controllo da inserire");
                    oIndicatoreDetRiga.TipoCtrl = ddl_TipoCtrl.SelectedIndex;
                    oIndicatoreDetRiga.DescrRiga = txt_DescrRiga.Text;
                    oIndicatoreDetRiga.OrdRiga = int.Parse(txt_OrdRiga.Text);
                    if (DBUtility.leggi_inddetrigaord(oSelezione.SelezioneId, CategoriaCod, IndId, IndDetId, oIndicatoreDetRiga.TipoRiga, null, oIndicatoreDetRiga.OrdRiga, null) == 2)
                        //Tento di inserire una terza riga di stesso tipo con lo stesso Ordinamento
                        if ((string)Session["Operazione"] == TipoOp.InsRiga.ToString())
                            throw new CustomExceptions.Controlli("Si possono inserire al massimo due righe con lo stesso Ordine di comparsa della riga");
                        else
                        {
                            //Sto modificando una riga che aveva un altro ordinamento
                            if (DBUtility.leggi_inddetrigaord(oSelezione.SelezioneId, CategoriaCod, IndId, IndDetId, oIndicatoreDetRiga.TipoRiga, oIndicatoreDetRiga.RigaId, oIndicatoreDetRiga.OrdRiga, null) == 0)
                                throw new CustomExceptions.Controlli("Si possono inserire al massimo due righe con lo stesso Ordine di comparsa della riga");
                        }
                    //LblErr.Text = "Utente: " + ((clsUtenteLogin)Session["Utente"]).UserId.ToString();
                    if ((string)Session["Operazione"] == TipoOp.InsRiga.ToString())
                    {
                        DBUtility.ins_indicatore_det_riga(oIndicatoreDetRiga, null, ((clsUtenteLogin)Session["Utente"]).UserId);
                        LblErr.Text = "Riga inserita con successo";
                        // Solo se sono in inserimento e l'operazione è ok assegno la var statica RigaId
                        RigaId = oIndicatoreDetRiga.RigaId;
                    }
                    else if ((string)Session["Operazione"] == TipoOp.ModRiga.ToString())
                    {
                        DBUtility.upd_indicatore_det_riga(oIndicatoreDetRiga, null, ((clsUtenteLogin)Session["Utente"]).UserId);
                        LblErr.Text = "Riga modificata con successo";
                        ddl_TipoRiga.Enabled = true;
                    }
                    Session["Operazione"] = TipoOp.NoOp.ToString();
                    grdIndicatoriDetRiga.DataSource = DBUtility.LeggiIndicatoriDetRiga(SelezioneId, CategoriaCod, IndId, IndDetId, null, null);
                    grdIndicatoriDetRiga.DataBind();
                }
                catch (CustomExceptions.Controlli ex)
                {
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + ex.CustomMessage + "');", true);
                    Session["CorrezioneControlli"] = true;
                }
                catch (Exception ex)
                {
                    string Message = "Operazione di inserimento/modifica Riga fallita: " + ex.Message;
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + Message + "');", true);
                    Session["CorrezioneControlli"] = true;
                }
            }
            else if (((Button)sender).ID == "btnAnnullaIndDetRiga")
            {
                Session["Operazione"] = TipoOp.NoOp.ToString();
            }
        }

        protected void btnConfermaIndDetPunt_Click(object sender, EventArgs e)
        {
            if (((Button)sender).ID == "btnConfermaIndDetPunt")
            {
                IndicatoreDetPunt oIndicatoreDetPunt = new IndicatoreDetPunt();
                oIndicatoreDetPunt.SelezioneId = oSelezione.SelezioneId;
                oIndicatoreDetPunt.CategoriaCod = CategoriaCod;
                oIndicatoreDetPunt.IndId = IndId;
                oIndicatoreDetPunt.IndDetId = IndDetId;
                if ((string)Session["Operazione"] == TipoOp.ModPunt.ToString())
                    oIndicatoreDetPunt.PuntId = PuntId;
                else if ((string)Session["Operazione"] == TipoOp.InsPunt.ToString())
                    oIndicatoreDetPunt.PuntId = DBUtility.rec_inddetpuntid(oSelezione.SelezioneId, CategoriaCod, IndId, IndDetId, null);
                oIndicatoreDetPunt.DescrPunt = txt_DescrPunt.Text;
                oIndicatoreDetPunt.Punt = float.Parse(txt_Punt.Text);
                try
                {
                    if ((string)Session["Operazione"] == TipoOp.InsPunt.ToString())
                    {
                        DBUtility.ins_indicatore_det_punt(oIndicatoreDetPunt, null, ((clsUtenteLogin)Session["Utente"]).UserId);
                        LblErr.Text = "Punteggio inserito con successo";
                        // Solo se sono in inserimento e l'operazione è ok assegno la var statica PuntId
                        PuntId = oIndicatoreDetPunt.PuntId;
                    }
                    else if ((string)Session["Operazione"] == TipoOp.ModPunt.ToString())
                    {
                        DBUtility.upd_indicatore_det_punt(oIndicatoreDetPunt, null, ((clsUtenteLogin)Session["Utente"]).UserId);
                        LblErr.Text = "Punteggio modificato con successo";
                    }
                }
                catch (Exception ex)
                {
                    string Message = "Operazione di inserimento/modifica Punteggio fallita: " + ex.Message;
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + Message + "');", true);
                }
                finally
                {
                    Session["Operazione"] = TipoOp.NoOp.ToString();
                    grdIndicatoriDetPunt.DataSource = DBUtility.LeggiIndicatoriDetPunt(SelezioneId, CategoriaCod, IndId, IndDetId, null);
                    grdIndicatoriDetPunt.DataBind();
                }
            }
            else if (((Button)sender).ID == "btnAnnullaIndDetPunt")
            {
                Session["Operazione"] = TipoOp.NoOp.ToString();
            }
        }

        protected void grdIndicatoriDetRiga_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                IndicatoreDetRiga selIndicatoreDetRiga = e.Row.DataItem as IndicatoreDetRiga;
                e.Row.Attributes.Add("onclick", "GridView_Riga_selectRow(this,'" + selIndicatoreDetRiga.RigaId + "','" + selIndicatoreDetRiga.TipoRiga + "')");
                e.Row.Attributes.Add("onmouseover", "GridView_mouseHover(this)");
            }
        }

        protected void grdIndicatoriDetPunt_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                IndicatoreDetPunt selIndicatoreDetPunt = e.Row.DataItem as IndicatoreDetPunt;
                e.Row.Attributes.Add("onclick", "GridView_Punt_selectRow(this,'" + selIndicatoreDetPunt.PuntId + "')");
                e.Row.Attributes.Add("onmouseover", "GridView_mouseHover(this)");
            }
        }

        protected void ddl_TipoCtrl_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddl_TipoRiga_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}