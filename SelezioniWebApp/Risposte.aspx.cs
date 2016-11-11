using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using DB_ODP;
using clDB;
using SelezioniObjects;
using System.Data;
using System.IO;

namespace SelezioniWebApp
{
    public partial class Risposte : System.Web.UI.Page
    {
        private int SelezioneId;
        private Selezione oSelezione;
        private string CategoriaCod;
        private int IndId;
        private Indicatore oIndicatore;
        private IndicatoreDet oIndicatoreDet;
        private int IndDetId;
        private clsUtenteLogin oUtente;
        private clsUtenteLogin oUtenteDip;
        ClsDB DBUtility = new ClsDB();
        ClsUtility Utility = new ClsUtility();
        private string Param;
        private int CompId;
        private CompRisposte oCompRisposte;
        private CompFile oCompRisposteNewFile;
        private CompFile oCompDelFile;
        private Valutazioni oValutazione;
        private StatoValutazione oStatoValutazione;
        private int OpId;

        //Controlli costruiti dinamicamente
        HtmlTable tblRisp;
        HtmlTableRow rigaRisp;
        HtmlTableCell colRisp;
        TextBox tb;
        LiteralControl spacer;
        HtmlTable tblCtrl;
        HtmlTableRow rigaCtrl;
        HtmlTableCell colCtrl;
        RadioButton rb;
        CheckBox cb;
        Label lb;
        ImageButton imb;
        FileUpload upl;
        HtmlGenericControl div;

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
            try
            {
                if (Session["Utente"] == null || Session["UtenteDip"] == null || Session["Param"] == null)
                {
                    throw new CustomExceptions.SessioneScaduta();
                }
                else
                {
                    oUtenteDip = (clsUtenteLogin)Session["UtenteDip"];
                    oUtente = (clsUtenteLogin)Session["Utente"];
                }
                if (Request.QueryString.Count != 4 && Request.QueryString.Count != 5)
                {
                    throw new CustomExceptions.PaginaNonRichiamataCorrettamente();
                }
                else if (Request.QueryString["SelezioneId"] == null || Request.QueryString["CategoriaCod"] == null || Request.QueryString["IndId"] == null || Request.QueryString["IndDetId"] == null)
                {
                    throw new CustomExceptions.PaginaNonRichiamataCorrettamente(null, "Necessari i parametri: SelezioneId, CategoriaCod, IndId, IndDetId");
                }
                else if (Request.QueryString.Count == 5 && Request.QueryString["CompId"] == null)
                {
                    throw new CustomExceptions.PaginaNonRichiamataCorrettamente(null, "Necessario il parametro: CompId");
                }

                SelezioneId = int.Parse(Request.QueryString["SelezioneId"].ToString());

                
                oSelezione = DBUtility.LeggiSelezione(SelezioneId);
                CategoriaCod = oSelezione.CategoriaCod;
                IndId = int.Parse(Request.QueryString["IndId"].ToString());
                oIndicatore = DBUtility.LeggiIndicatori(SelezioneId, CategoriaCod, IndId)[0];
                IndDetId = int.Parse(Request.QueryString["IndDetId"].ToString());
                oIndicatoreDet = DBUtility.LeggiIndicatoriDet(SelezioneId, CategoriaCod, IndId, IndDetId)[0];

                if (Request.QueryString["CompId"] != null)
                {
                    CompId = int.Parse(Request.QueryString["CompId"].ToString());
                    // verifico che la selezione richiesta sia effettivamente dell'utente loggato >>ga15092016<<
                    //if (!DBUtility.VerificaIns(SelezioneId, CompId, oUtenteDip.UserId, null))
                    //    System.Web.HttpContext.Current.Response.Redirect("SessioneScaduta.aspx");
                    if(!DBUtility.ExistsCompilazioniUtente(SelezioneId, CategoriaCod, null, null, CompId, null, oUtenteDip.UserId, null))
                        System.Web.HttpContext.Current.Response.Redirect("SessioneScaduta.aspx");

                    oCompRisposte = DBUtility.LeggiRisposte(CompId, SelezioneId, CategoriaCod, IndId, IndDetId, null, null, null, null);
                    if (DBUtility.LeggiValutazioni(CompId, SelezioneId, CategoriaCod, IndId, IndDetId, null).Count >= 1)
                        oValutazione = DBUtility.LeggiValutazioni(CompId, SelezioneId, CategoriaCod, IndId, IndDetId, null)[0];
                    else
                        oValutazione = null;
                }
                else
                {
                    CompId = 0;
                    oCompRisposte = null;
                    oValutazione = null;
                }
                Param = (string)Session["Param"];

                
                if (!Page.IsPostBack)
                {
                    grdPunteggi.DataSource = DBUtility.LeggiIndicatoriDetPunt(SelezioneId, CategoriaCod, IndId, IndDetId, null);
                    grdPunteggi.DataBind();
                    lbl_anno.Text = oSelezione.Anno.ToString();
                    lbl_categoriacod.Text = oSelezione.CategoriaCod;
                    lbl_titolo.Text = oSelezione.Titolo;
                    lbl_descrind.Text = oIndicatore.Descr;
                    lbl_notaind.Text = oIndicatore.NoteDip;
                    lbl_descrinddet.Text = oIndicatoreDet.DescrDet;
                    lbl_notainddet.Text = oIndicatoreDet.NoteDetDip;
                }

                dettaglio.Controls.Clear();
                oStatoValutazione = DBUtility.AggiornaStatoValutazione(SelezioneId, CompId, (string)Session["Param"]);
                IList<IndicatoreDetRiga> lIndicatoreDetRiga = DBUtility.LeggiIndicatoriDetRigaComp(SelezioneId, CategoriaCod, IndId, IndDetId, null, null, oStatoValutazione);
                dettaglio.Controls.Add(CostruisciRigheDettaglio(lIndicatoreDetRiga, oCompRisposte, oValutazione));
                if (Session["btnSav"] != null && !(bool)Session["btnSav"])
                {
                    btnSav.Enabled = false;
                }
            }
            catch (CustomExceptions.SessioneScaduta ex)
            {
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + ex.CustomMessage + "');", true);
                btnSav.Enabled = false;
                testata.Visible = false;
            }
            catch (CustomExceptions.PaginaNonRichiamataCorrettamente ex)
            {
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + ex.CustomMessage + "');", true);
                btnSav.Enabled = false;
                testata.Visible = false;
            }
        }

        protected HtmlTable CostruisciRigheDettaglio(IList<IndicatoreDetRiga> pIndicatoreDetRiga, CompRisposte pCompRisposte, Valutazioni pValutazione)
        {
            //spacer = new LiteralControl("<div class=\"rigabottombig\"></div>");
            //dettaglio.Controls.Add(spacer);

            tblRisp = new HtmlTable();
            tblRisp.Attributes.Add("class", "tabellarisposte");

            //Punteggio complessivo
            var flNote = from idr in pIndicatoreDetRiga
                         where (idr.TipoCtrl == 6)
                         select idr;
            if (flNote.Count() == 0 && oStatoValutazione.ValutazioniVis != "N")
            {
                if ((oStatoValutazione.ValutazioniVis == "D" && oIndicatore.DipFlg == 1) ||
                    //(oStatoValutazione.ValutazioniVis == "R" && oIndicatore.RspFlg == 1 && oIndicatore.AmmFlg != 1) ||
                    (oStatoValutazione.ValutazioniVis == "R" && oIndicatore.RspFlg == 1) ||
                    (oStatoValutazione.ValutazioniVis == "A" && oIndicatore.AmmFlg == 1) ||
                    (oStatoValutazione.ValutazioniVis == "T"))
                {
                    rigaRisp = new HtmlTableRow();
                    colRisp = new HtmlTableCell();
                    colRisp.Attributes.Add("class", "punteggio");
                    colRisp.InnerHtml = "Punteggio complessivo assegnato: ";
                    tb = new TextBox();
                    int tmpTipoRiga = 0;
                    if (oIndicatore.AmmFlg == 1)
                        tmpTipoRiga = 3;
                    else if (oIndicatore.RspFlg == 1)
                        tmpTipoRiga = 2;
                    tb.ID = "txt_ins_punteggio" + "/" +
                            oIndicatoreDet.SelezioneId.ToString() + "_" +
                            oIndicatoreDet.CategoriaCod + "_" +
                            oIndicatoreDet.IndId.ToString() + "_" +
                            oIndicatoreDet.IndDetId.ToString() + "_" +
                            tmpTipoRiga.ToString() + "_0/0";
                    tb.AutoPostBack = false;
                    tb.Attributes.Add("onChange", "this.value=this.value.replace(\".\",\",\");");
                    tb.Attributes.Add("onBlur", "ValidateNumber(this)");
                    //Se Modifica recupero il valore da db
                    if (pValutazione != null)
                    {
                        tb.Text = pValutazione.Punteggio.ToString();
                    }
                    if ((oStatoValutazione.ValutazioniMod == "R" && oIndicatore.RspFlg == 1 && oIndicatore.AmmFlg != 1) ||
                        (oStatoValutazione.ValutazioniMod == "A" && oIndicatore.AmmFlg == 1) ||
                        (oStatoValutazione.ValutazioniMod == "T"))
                    {
                        tb.Enabled = true;
                        btnSav.Enabled = true;
                    }
                    else
                        tb.Enabled = false;
                    colRisp.Controls.Add(tb);
                    rigaRisp.Cells.Add(colRisp);
                    tblRisp.Rows.Add(rigaRisp);
                    rigaRisp = new HtmlTableRow();
                    colRisp = new HtmlTableCell();
                    colRisp.Attributes.Add("class", "tabellarisposte bordomax");
                    rigaRisp.Cells.Add(colRisp);
                    tblRisp.Rows.Add(rigaRisp);
                }
            }

            var lIndicatoreDetRigaDip = from idr in pIndicatoreDetRiga
                                        where (idr.TipoRiga == 1)
                                        select idr;
            var lIndicatoreDetRigaRsp = from idr in pIndicatoreDetRiga
                                        where (idr.TipoRiga != 1)
                                        select idr;
            for (int i = 1; i <= oIndicatoreDet.MaxRighe; i++)
            {
                if (i > 1)
                {
                    rigaRisp = new HtmlTableRow();
                    colRisp = new HtmlTableCell();
                    colRisp.Attributes.Add("class", "tabellarisposte bordomax");
                    spacer = new LiteralControl("&nbsp;");
                    colRisp.Controls.Add(spacer);
                    rigaRisp.Cells.Add(colRisp);
                    tblRisp.Rows.Add(rigaRisp);
                }

                bool InsUpl = false;//flag inserimento controllo FileUpload solo nella PEO 2011 sono previsti allegati
                bool InsRsp = true;//flag inserimento testata valutazione responsabile
                tblCtrl = new HtmlTable();

                //ciclo righe dipendente
                foreach (IndicatoreDetRiga iDetRiga in lIndicatoreDetRigaDip)
                {
                    rigaRisp = new HtmlTableRow();
                    colRisp = new HtmlTableCell();
                    // InsUpl = true; >>ga23072013<<
                    if (oSelezione.Anno == 2011) //>>ga23072013<< solo nel 2011 sono previsti allegati
                    {
                        InsUpl = true;
                    }

                    //Controlli diversi da scelta e spunta
                    if (iDetRiga.TipoCtrl != 3 && iDetRiga.TipoCtrl != 4)
                    {
                        colRisp.Attributes.Add("class", "tabellarisposte descr");
                        colRisp.InnerHtml = iDetRiga.DescrRiga.ToString();
                        spacer = new LiteralControl("<br/>");
                        colRisp.Controls.Add(spacer);
                        tb = new TextBox();
                        //Chiave di val_indicatori_det_riga
                        tb.ID = "txt_ins_risp" + "/" +
                                oIndicatoreDet.SelezioneId.ToString() + "_" +
                                oIndicatoreDet.CategoriaCod + "_" +
                                oIndicatoreDet.IndId.ToString() + "_" +
                                oIndicatoreDet.IndDetId.ToString() + "_" +
                                iDetRiga.TipoRiga.ToString() + "_" +
                                iDetRiga.RigaId.ToString() + "/" + i.ToString();
                        tb.AutoPostBack = false;
                        //Se Modifica recupero il valore da db
                        if (pCompRisposte != null)
                        {
                            var oRisposta = from rr in pCompRisposte.RigheRisposta
                                            where (rr.IndId == oIndicatoreDet.IndId &&
                                                   rr.IndDetId == oIndicatoreDet.IndDetId &&
                                                   rr.RispId == i &&
                                                   rr.TipoRiga == iDetRiga.TipoRiga &&
                                                   rr.RigaId == iDetRiga.RigaId)
                                            select rr.Risp;
                            if (oRisposta.Count() != 0)
                            {
                                tb.Text = HttpUtility.HtmlDecode(oRisposta.First());
                            }
                        }
                        if (iDetRiga.TipoCtrl != 6)
                        {
                            if (oStatoValutazione.RisposteMod == "D" || oStatoValutazione.RisposteMod == "T")
                            {
                                tb.Enabled = true;
                                btnSav.Enabled = true;
                            }
                            else
                                tb.Enabled = false;
                        }
                        else
                        {
                            if (oStatoValutazione.NoteMod == "D" || oStatoValutazione.NoteMod == "T")
                            {
                                tb.Enabled = true;
                                btnSav.Enabled = true;
                            }
                            else
                                tb.Enabled = false;
                        }
                        switch (iDetRiga.TipoCtrl)
                        {
                            //Testo
                            case 1:
                                tb.Attributes.Add("class", "RispTesto");
                                break;
                            //Data
                            case 2:
                                tb.Attributes.Add("class", "RispData");
                                tb.Attributes.Add("onBlur", "ValidateDate(this, event.keyCode)");
                                tb.Attributes.Add("onkeydown", "return DateFormat(this, event.keyCode)");
                                tb.MaxLength = 10;
                                break;
                            //Area di testo e Campo Note
                            case 5:
                            case 6:
                                tb.Attributes.Add("class", "RispAreaTesto");
                                tb.Rows = 4;
                                tb.Columns = 110;
                                tb.MaxLength = 2000;
                                //tb.Wrap = true;
                                tb.TextMode = System.Web.UI.WebControls.TextBoxMode.MultiLine;
                                break;
                            default:
                                break;
                        }
                        colRisp.Controls.Add(tb);
                        rigaRisp.Cells.Add(colRisp);
                        tblRisp.Rows.Add(rigaRisp);
                    }
                    else
                    {
                        rigaCtrl = new HtmlTableRow();
                        colCtrl = new HtmlTableCell();
                        colCtrl.Attributes.Add("class", "tabellarisposte descr");
                        lb = new Label();
                        lb.Text = iDetRiga.DescrRiga.ToString();
                        colCtrl.Controls.Add(lb);
                        rigaCtrl.Cells.Add(colCtrl);
                        colCtrl = new HtmlTableCell();
                        switch (iDetRiga.TipoCtrl)
                        {
                            //Scelta
                            case 3:
                                rb = new RadioButton();
                                rb.GroupName = "GRP" + "/" + i.ToString();
                                rb.ID = "rbt_ins_rigaid" + "/" +
                                        oIndicatoreDet.SelezioneId.ToString() + "_" +
                                        oIndicatoreDet.CategoriaCod + "_" +
                                        oIndicatoreDet.IndId.ToString() + "_" +
                                        oIndicatoreDet.IndDetId.ToString() + "_" +
                                        iDetRiga.TipoRiga.ToString() + "_" +
                                        iDetRiga.RigaId.ToString() + "/" + i.ToString();
                                rb.AutoPostBack = false;
                                //Se Modifica recupero il valore da db
                                if (pCompRisposte != null)
                                {
                                    var oRisposta = from rr in pCompRisposte.RigheRisposta
                                                    where (rr.IndId == oIndicatoreDet.IndId &&
                                                           rr.IndDetId == oIndicatoreDet.IndDetId &&
                                                           rr.RispId == i &&
                                                           rr.TipoRiga == iDetRiga.TipoRiga &&
                                                           rr.RigaId == iDetRiga.RigaId)
                                                    select rr.Risp;
                                    if (oRisposta.Count() != 0)
                                    {
                                        rb.Checked = true;
                                    }
                                }
                                if (oStatoValutazione.RisposteMod == "D" || oStatoValutazione.RisposteMod == "T")
                                {
                                    rb.Enabled = true;
                                    btnSav.Enabled = true;
                                }
                                else
                                    rb.Enabled = false;
                                colCtrl.Attributes.Add("class", "scelta");
                                colCtrl.Controls.Add(rb);
                                break;
                            //Spunta
                            case 4:
                                cb = new CheckBox();
                                cb.ID = "ckb_ins_rigaid" + "/" +
                                        oIndicatoreDet.SelezioneId.ToString() + "_" +
                                        oIndicatoreDet.CategoriaCod + "_" +
                                        oIndicatoreDet.IndId.ToString() + "_" +
                                        oIndicatoreDet.IndDetId.ToString() + "_" +
                                        iDetRiga.TipoRiga.ToString() + "_" +
                                        iDetRiga.RigaId.ToString() + "/" + i.ToString();
                                cb.AutoPostBack = false;
                                //Se Modifica recupero il valore da db
                                if (pCompRisposte != null)
                                {
                                    var oRisposta = from rr in pCompRisposte.RigheRisposta
                                                    where (rr.IndId == oIndicatoreDet.IndId &&
                                                           rr.IndDetId == oIndicatoreDet.IndDetId &&
                                                           rr.RispId == i &&
                                                           rr.TipoRiga == iDetRiga.TipoRiga &&
                                                           rr.RigaId == iDetRiga.RigaId)
                                                    select rr.Risp;
                                    if (oRisposta.Count() != 0)
                                    {
                                        cb.Checked = true;
                                    }
                                }
                                if (oStatoValutazione.RisposteMod == "D" || oStatoValutazione.RisposteMod == "T")
                                {
                                    cb.Enabled = true;
                                    btnSav.Enabled = true;
                                }
                                else
                                    cb.Enabled = false;
                                colCtrl.Attributes.Add("class", "scelta");
                                colCtrl.Controls.Add(cb);
                                break;
                            default:
                                break;
                        }
                        rigaCtrl.Cells.Add(colCtrl);
                        tblCtrl.Rows.Add(rigaCtrl);
                    }
                }
                if (tblCtrl.Rows.Count > 0)
                {
                    colRisp.Controls.Add(tblCtrl);
                    rigaRisp.Cells.Add(colRisp);
                    tblRisp.Rows.Add(rigaRisp);
                }

                //File Upload
                if (InsUpl == true && flNote.Count() == 0)
                {
                    rigaRisp = new HtmlTableRow();
                    colRisp = new HtmlTableCell();
                    colRisp.Attributes.Add("class", "tabellarisposte descr");
                    if (CompId > 0)
                    {
                        DataRow dr = DBUtility.LeggiFile(CompId, SelezioneId, CategoriaCod, IndId, IndDetId, i);

                        if (dr.Table.Rows.Count > 0)
                        {
                            colRisp.InnerHtml = "Sostituisci allegato";
                        }
                        else
                        {
                            colRisp.InnerHtml = "Inserisci allegato";
                        }
                    }
                    spacer = new LiteralControl("<br/>");
                    colRisp.Controls.Add(spacer);
                    upl = new FileUpload();
                    upl.ID = "blb_ins_allegato" + "/" +
                             oIndicatoreDet.SelezioneId.ToString() + "_" +
                             oIndicatoreDet.CategoriaCod + "_" +
                             oIndicatoreDet.IndId.ToString() + "_" +
                             oIndicatoreDet.IndDetId.ToString() + "/" +
                             i.ToString();
                    upl.Attributes.Add("class", "RispUpl");
                    if (oStatoValutazione.RisposteMod == "D" || oStatoValutazione.RisposteMod == "T")
                    {
                        upl.Enabled = true;
                        btnSav.Enabled = true;
                    }
                    else
                        upl.Enabled = false;
                    colRisp.Controls.Add(upl);
                    if (CompId > 0)
                    {
                        DataRow dr = DBUtility.LeggiFile(CompId, SelezioneId, CategoriaCod, IndId, IndDetId, i);

                        if (dr.Table.Rows.Count != 0)
                        {
                            //TODO: la seguente parte solo se in modifica

                            imb = new ImageButton();
                            imb.ID = "imb_vis_allegato" + "/" +
                                     oIndicatoreDet.SelezioneId.ToString() + "_" +
                                     oIndicatoreDet.CategoriaCod + "_" +
                                     oIndicatoreDet.IndId.ToString() + "_" +
                                     oIndicatoreDet.IndDetId.ToString() + "/" +
                                     i.ToString();
                            imb.ImageUrl = "~/Images/S_B_UPLO.gif";
                            imb.CommandArgument = imb.ID.ToString();
                            imb.Click += new System.Web.UI.ImageClickEventHandler(vis_Click);

                            // label
                            lb = new Label();
                            lb.Text = "Visualizza allegato";

                            colRisp.Controls.Add(imb);
                            colRisp.Controls.Add(lb);

                            if (Session["btnSav"] == null || (bool)Session["btnSav"])
                            {
                                cb = new CheckBox();
                                cb.ID = "ckb_ins_allegato" + "/" +
                                         oIndicatoreDet.SelezioneId.ToString() + "_" +
                                         oIndicatoreDet.CategoriaCod + "_" +
                                         oIndicatoreDet.IndId.ToString() + "_" +
                                         oIndicatoreDet.IndDetId.ToString() + "/" +
                                         i.ToString();
                                cb.Text = "Cancella file allegato";
                                cb.CheckedChanged += new System.EventHandler(cb_change);
                                if (oStatoValutazione.RisposteMod == "D" || oStatoValutazione.RisposteMod == "T")
                                {
                                    cb.Enabled = true;
                                    btnSav.Enabled = true;
                                }
                                else
                                {
                                    cb.Enabled = false;
                                }
                                if (oStatoValutazione.RisposteVis != "N")
                                {
                                    imb.Enabled = true;
                                }
                                else
                                {
                                    imb.Enabled = false;
                                }
                                colRisp.Controls.Add(cb);
                            }
                        }
                    }
                    rigaRisp.Cells.Add(colRisp);
                    tblRisp.Rows.Add(rigaRisp);
                }

                tblCtrl = new HtmlTable();
                //ciclo righe responsabile
                foreach (IndicatoreDetRiga iDetRiga in lIndicatoreDetRigaRsp)
                {
                    if (InsRsp == true)
                    {
                        InsRsp = false;
                        //bordo
                        rigaRisp = new HtmlTableRow();
                        colRisp = new HtmlTableCell();
                        colRisp.Attributes.Add("class", "tabellarisposte bordo");
                        rigaRisp.Cells.Add(colRisp);
                        tblRisp.Rows.Add(rigaRisp);
                        //descrizione
                        rigaRisp = new HtmlTableRow();
                        colRisp = new HtmlTableCell();
                        colRisp.Attributes.Add("class", "tabellarisposte descr");
                        if (iDetRiga.TipoCtrl != 6)
                            colRisp.InnerHtml = "Valutazione del responsabile";
                        else
                            colRisp.InnerHtml = "Note del responsabile e dell'ufficio amministrativo";
                        spacer = new LiteralControl("<br/>");
                        colRisp.Controls.Add(spacer);
                        rigaRisp.Cells.Add(colRisp);
                        tblRisp.Rows.Add(rigaRisp);
                        colRisp.Attributes.Add("class", "tabellarisposte valutazione");
                    }
                    rigaRisp = new HtmlTableRow();
                    colRisp = new HtmlTableCell();
                   // InsUpl = true; >>ga23072013<<
                    if (oSelezione.Anno == 2011) //>>ga23072013<< solo nel 2011 sono previsti allegati
                    {
                        InsUpl = true;
                    }
                    //Controlli diversi da scelta e spunta
                    if (iDetRiga.TipoCtrl != 3 && iDetRiga.TipoCtrl != 4)
                    {
                        colRisp.Attributes.Add("class", "tabellarisposte descr");
                        colRisp.InnerHtml = iDetRiga.DescrRiga.ToString();
                        spacer = new LiteralControl("<br/>");
                        colRisp.Controls.Add(spacer);
                        tb = new TextBox();
                        //Chiave di val_indicatori_det_riga
                        tb.ID = "txt_ins_risp" + "/" +
                                oIndicatoreDet.SelezioneId.ToString() + "_" +
                                oIndicatoreDet.CategoriaCod + "_" +
                                oIndicatoreDet.IndId.ToString() + "_" +
                                oIndicatoreDet.IndDetId.ToString() + "_" +
                                iDetRiga.TipoRiga.ToString() + "_" +
                                iDetRiga.RigaId.ToString() + "/" + i.ToString();
                        tb.AutoPostBack = false;
                        //Se Modifica recupero il valore da db
                        if (pCompRisposte != null)
                        {
                            var oRisposta = from rr in pCompRisposte.RigheRisposta
                                            where (rr.IndId == oIndicatoreDet.IndId &&
                                                   rr.IndDetId == oIndicatoreDet.IndDetId &&
                                                   rr.RispId == i &&
                                                   rr.TipoRiga == iDetRiga.TipoRiga &&
                                                   rr.RigaId == iDetRiga.RigaId)
                                            select rr.Risp;
                            if (oRisposta.Count() != 0)
                            {
                                tb.Text = HttpUtility.HtmlDecode(oRisposta.First());
                            }
                        }
                        if (iDetRiga.TipoCtrl != 6)
                        {
                            if ((oStatoValutazione.RisposteMod == "R" && iDetRiga.TipoRiga == 2) ||
                                (oStatoValutazione.RisposteMod == "A" && iDetRiga.TipoRiga == 3) ||
                                (oStatoValutazione.RisposteMod == "T"))
                            {
                                tb.Enabled = true;
                                btnSav.Enabled = true;
                            }
                            else
                                tb.Enabled = false;
                        }
                        else
                        {
                            if ((oStatoValutazione.NoteMod == "R" && iDetRiga.TipoRiga == 2) ||
                                (oStatoValutazione.NoteMod == "A" && iDetRiga.TipoRiga == 3) ||
                                (oStatoValutazione.NoteMod == "T"))
                            {
                                tb.Enabled = true;
                                btnSav.Enabled = true;
                            }
                            else
                                tb.Enabled = false;
                        }
                        switch (iDetRiga.TipoCtrl)
                        {
                            //Testo
                            case 1:
                                tb.Attributes.Add("class", "RispTesto");
                                break;
                            //Data
                            case 2:
                                tb.Attributes.Add("class", "RispData");
                                tb.Attributes.Add("onBlur", "ValidateDate(this, event.keyCode)");
                                tb.Attributes.Add("onkeydown", "return DateFormat(this, event.keyCode)");
                                tb.MaxLength = 10;
                                break;
                            //Area di testo o campo Note
                            case 5:
                            case 6:
                                tb.Attributes.Add("class", "RispAreaTesto");
                                tb.Rows = 4;
                                tb.Columns = 110;
                                tb.MaxLength = 2000;
                                //tb.Wrap = true;
                                tb.TextMode = System.Web.UI.WebControls.TextBoxMode.MultiLine;
                                break;
                            default:
                                break;
                        }
                        colRisp.Controls.Add(tb);
                        rigaRisp.Cells.Add(colRisp);
                        tblRisp.Rows.Add(rigaRisp);
                    }
                    else
                    {
                        rigaCtrl = new HtmlTableRow();
                        colCtrl = new HtmlTableCell();
                        colCtrl.Attributes.Add("class", "tabellarisposte descr");
                        lb = new Label();
                        lb.Text = iDetRiga.DescrRiga.ToString();
                        colCtrl.Controls.Add(lb);
                        rigaCtrl.Cells.Add(colCtrl);
                        colCtrl = new HtmlTableCell();
                        switch (iDetRiga.TipoCtrl)
                        {
                            //Scelta
                            case 3:
                                rb = new RadioButton();
                                rb.GroupName = "GRP" + "/" + i.ToString();
                                rb.ID = "rbt_ins_rigaid" + "/" +
                                        oIndicatoreDet.SelezioneId.ToString() + "_" +
                                        oIndicatoreDet.CategoriaCod + "_" +
                                        oIndicatoreDet.IndId.ToString() + "_" +
                                        oIndicatoreDet.IndDetId.ToString() + "_" +
                                        iDetRiga.TipoRiga.ToString() + "_" +
                                        iDetRiga.RigaId.ToString() + "/" + i.ToString();
                                rb.AutoPostBack = false;
                                //Se Modifica recupero il valore da db
                                if (pCompRisposte != null)
                                {
                                    var oRisposta = from rr in pCompRisposte.RigheRisposta
                                                    where (rr.IndId == oIndicatoreDet.IndId &&
                                                           rr.IndDetId == oIndicatoreDet.IndDetId &&
                                                           rr.RispId == i &&
                                                           rr.TipoRiga == iDetRiga.TipoRiga &&
                                                           rr.RigaId == iDetRiga.RigaId)
                                                    select rr.Risp;
                                    if (oRisposta.Count() != 0)
                                    {
                                        rb.Checked = true;
                                    }
                                }
                                if ((oStatoValutazione.RisposteMod == "R" && iDetRiga.TipoRiga == 2) ||
                                    (oStatoValutazione.RisposteMod == "A" && iDetRiga.TipoRiga == 3) ||
                                    (oStatoValutazione.RisposteMod == "T"))
                                {
                                    rb.Enabled = true;
                                    btnSav.Enabled = true;
                                }
                                else
                                    rb.Enabled = false;
                                colCtrl.Attributes.Add("class", "scelta");
                                colCtrl.Controls.Add(rb);
                                break;
                            //Spunta
                            case 4:
                                cb = new CheckBox();
                                cb.ID = "ckb_ins_rigaid" + "/" +
                                        oIndicatoreDet.SelezioneId.ToString() + "_" +
                                        oIndicatoreDet.CategoriaCod + "_" +
                                        oIndicatoreDet.IndId.ToString() + "_" +
                                        oIndicatoreDet.IndDetId.ToString() + "_" +
                                        iDetRiga.TipoRiga.ToString() + "_" +
                                        iDetRiga.RigaId.ToString() + "/" + i.ToString();
                                cb.AutoPostBack = false;
                                //Se Modifica recupero il valore da db
                                if (pCompRisposte != null)
                                {
                                    var oRisposta = from rr in pCompRisposte.RigheRisposta
                                                    where (rr.IndId == oIndicatoreDet.IndId &&
                                                           rr.IndDetId == oIndicatoreDet.IndDetId &&
                                                           rr.RispId == i &&
                                                           rr.TipoRiga == iDetRiga.TipoRiga &&
                                                           rr.RigaId == iDetRiga.RigaId)
                                                    select rr.Risp;
                                    if (oRisposta.Count() != 0)
                                    {
                                        cb.Checked = true;
                                    }
                                }
                                if ((oStatoValutazione.RisposteMod == "R" && iDetRiga.TipoRiga == 2) ||
                                    (oStatoValutazione.RisposteMod == "A" && iDetRiga.TipoRiga == 3) ||
                                    (oStatoValutazione.RisposteMod == "T"))
                                {
                                    cb.Enabled = true;
                                    btnSav.Enabled = true;
                                }
                                else
                                    cb.Enabled = false;
                                colCtrl.Attributes.Add("class", "scelta");
                                colCtrl.Controls.Add(cb);
                                break;
                            default:
                                break;
                        }
                        rigaCtrl.Cells.Add(colCtrl);
                        tblCtrl.Rows.Add(rigaCtrl);
                    }
                }
                if (tblCtrl.Rows.Count > 0)
                {
                    colRisp.Controls.Add(tblCtrl);
                    rigaRisp.Cells.Add(colRisp);
                    tblRisp.Rows.Add(rigaRisp);
                }
            }
            Session["TblRisp"] = tblRisp;
            return tblRisp;
        }

        protected void btnSav_Click(object sender, EventArgs e)
        {
            CompRisposte oCompRisposteNew = new CompRisposte();
            Valutazioni oValutazioneNew = new Valutazioni();
            Risposta oRisposta;
            int TipoRiga = 0;
            int RigaId = 0;
            int RispId = 0;
            bool InsUpdComp;
            bool InsUpdVal;
            // controlli campo blob
            string strem = "";
            int intDocLen = 0;
            Stream objStream;
            List<CompFile> lCompFile = new List<CompFile>();
            List<CompFile> lCompFileDel = new List<CompFile>();
            List<string> lCtrl = new List<string>();
            foreach (Control c in dettaglio.Controls)
            {
                if (c.GetType().Name != "LiteralControl")
                {
                    lCtrl = Utility.FindControlByNamePart(c, "_ins_");
                }
            }
            try
            {
                InsUpdVal = false;
                foreach (string item in lCtrl)
                {
                    //Recupero delle chiavi
                    string[] words = item.Split('/');
                    if (words.Count() < 2)
                        throw new CustomExceptions.Controlli("Campo " + item + " errato");
                    string[] keys = words[1].Split('_');
                    if (keys.Count() < 4)
                        throw new CustomExceptions.Controlli("Campo " + item + " errato");
                    if (keys.Count() == 6)
                    {
                        TipoRiga = int.Parse(keys[4]);
                        RigaId = int.Parse(keys[5]);
                    }
                    if (words.Count() == 3)
                        RispId = int.Parse(words[2]);

                    oRisposta = new Risposta();

                    InsUpdComp = false;
                    if (RigaId != 0)
                    {
                        if (DBUtility.ExistsNoteRiga(oIndicatoreDet.SelezioneId, oIndicatoreDet.CategoriaCod, oIndicatoreDet.IndId, oIndicatoreDet.IndDetId, TipoRiga, RigaId))
                        {
                            if (oStatoValutazione.NoteMod == "D" && TipoRiga != 1)
                                continue;
                            if (oStatoValutazione.NoteMod == "R" && TipoRiga != 2)
                                continue;
                            if (oStatoValutazione.NoteMod == "A" && TipoRiga != 3)
                                continue;
                            if (oStatoValutazione.NoteMod == "N")
                                continue;
                        }
                        else
                        {
                            if (oStatoValutazione.RisposteMod == "D" && TipoRiga != 1)
                                continue;
                            if (oStatoValutazione.RisposteMod == "R" && TipoRiga != 2)
                                continue;
                            if (oStatoValutazione.RisposteMod == "A" && TipoRiga != 3)
                                continue;
                            if (oStatoValutazione.RisposteMod == "N")
                                continue;
                        }
                        //>>ga2013agosto<< inizio
                        IList<IndicatoreDetRiga> lIndicatoreDetRiga = DBUtility.LeggiIndicatoriDetRigaComp(oIndicatoreDet.SelezioneId, oIndicatoreDet.CategoriaCod, oIndicatoreDet.IndId, oIndicatoreDet.IndDetId, TipoRiga, RigaId, oStatoValutazione);
                        //if (lIndicatoreDetRiga[0].TipoCtrl != 6)
                        // if (lIndicatoreDetRiga[0].TipoCtrl == 1 || lIndicatoreDetRiga[0].TipoCtrl == 5) //>>ga092015 Non si riesce a modificare la risposta di un campo note
                        if (lIndicatoreDetRiga[0].TipoCtrl == 1 || lIndicatoreDetRiga[0].TipoCtrl == 5 || lIndicatoreDetRiga[0].TipoCtrl == 6)  //>>ga092015
                        {
                            string tmpRisposta = HttpUtility.HtmlEncode(((TextBox)dettaglio.FindControl(item)).Text);
                            if (tmpRisposta.Length > 2000)
                            {
                                throw new CustomExceptions.Controlli("la lunghezza del campo eccede i 2000 caratteri");
                            }
                        }
                        //>>ga2013agosto<< fine
                    }
                    else
                    {
                        //if (!((oStatoValutazione.ValutazioniMod == "R" && oIndicatore.RspFlg == 1 && oIndicatore.AmmFlg != 1) ||
                        if (!((oStatoValutazione.ValutazioniMod == "R" && oIndicatore.RspFlg == 1) ||
                            (oStatoValutazione.ValutazioniMod == "A" && oIndicatore.AmmFlg == 1) ||
                            (oStatoValutazione.ValutazioniMod == "T")))
                            continue;
                        if (oStatoValutazione.ValutazioniMod == "N")
                            continue;
                    }
                    if (object.Equals("TextBox", dettaglio.FindControl(item).GetType().Name) && ((TextBox)dettaglio.FindControl(item)).Text != "")
                    {
                        if (RigaId == 0)
                        {
                            oValutazioneNew.Punteggio = float.Parse(((TextBox)dettaglio.FindControl(item)).Text);
                            InsUpdVal = true;
                        }
                        else
                        {
                            oRisposta.Risp = HttpUtility.HtmlEncode(((TextBox)dettaglio.FindControl(item)).Text);
                            oRisposta.RigaId = RigaId;
                            InsUpdComp = true;
                        }
                    }
                    else if (object.Equals("RadioButton", dettaglio.FindControl(item).GetType().Name) && ((RadioButton)dettaglio.FindControl(item)).Checked == true)
                    {
                        oRisposta.RigaId = RigaId;
                        InsUpdComp = true;
                    }
                     else if (object.Equals("CheckBox", dettaglio.FindControl(item).GetType().Name) && ((CheckBox)dettaglio.FindControl(item)).Checked == true) 
                    //else if (object.Equals("CheckBox", dettaglio.FindControl(item).GetType().Name))//>>ga092016<< provare se con questo if risolvo!!!
                    {
                        
                        if (item.ToString().IndexOf("allegato") == -1)  //flag di cancellazione file
                        {
                            oRisposta.RigaId = RigaId;
                            InsUpdComp = true;
                            //oRisposta.Risp = ((CheckBox)dettaglio.FindControl(item)).Checked.ToString(); //vedere se aggiungerlo !!! >>ga092016<<
                        }
                        else
                        {
                            CheckBox cb = (CheckBox)dettaglio.FindControl(item);
                            if (cb.Checked)
                            {
                                oCompDelFile = new CompFile();
                                oCompDelFile.SelezioneId = oIndicatoreDet.SelezioneId;
                                oCompDelFile.CategoriaCod = oIndicatoreDet.CategoriaCod;
                                oCompDelFile.IndId = oIndicatoreDet.IndId;
                                oCompDelFile.IndDetId = oIndicatoreDet.IndDetId;
                                oCompDelFile.RispId = RispId;
                                oCompDelFile.UsrIns = oUtenteDip.UserId;
                                lCompFileDel.Add(oCompDelFile);
                                cb.Checked = false;
                            }
                        }
                    }
                    else if (object.Equals("FileUpload", dettaglio.FindControl(item).GetType().Name))
                    {
                        oCompRisposteNewFile = new CompFile();
                        FileUpload FileTmp = new FileUpload();
                        FileTmp = (FileUpload)dettaglio.FindControl(item);
                        string extension = Path.GetExtension(FileTmp.PostedFile.FileName).ToLower();
                        if (FileTmp.HasFile)
                        {

                            TextBox txtExt_al = new TextBox();
                            Byte imageBytes = (Byte)FileTmp.PostedFile.InputStream.Length;
                            intDocLen = (int)FileTmp.PostedFile.InputStream.Length;
                            byte[] Docbuffer = new byte[intDocLen];
                            TextBox txtBtye_al = new TextBox();

                            if (FileTmp.PostedFile.FileName != "")
                            {
                                if (FileTmp.PostedFile.ToString() == "" || FileTmp.PostedFile.FileName.ToString() == "" || FileTmp.PostedFile.InputStream.ToString() == "")
                                {
                                    strem = "Upload fallito";
                                }
                                // ........................................ verifica tipo file.........................
                                txtExt_al.Text = "";
                                switch (extension.ToLower())
                                {
                                    case ".pdf":
                                        txtExt_al.Text = extension.Substring(1, extension.Length - 1);
                                        break;
                                    default:
                                        strem = "ERRFILE";
                                        break;
                                }
                                if (strem == "ERRFILE")
                                {
                                    strem = "";
                                    throw new CustomExceptions.TipoFileErrato();
                                }
                                if (FileTmp.PostedFile.InputStream.Length > 10000000)  //1000000
                                {
                                    throw new CustomExceptions.BigFile();
                                }

                                //load FileUploaded input stream into byte array
                                txtBtye_al.Text = imageBytes.ToString();
                                objStream = FileTmp.PostedFile.InputStream;
                                objStream.Read(Docbuffer, 0, intDocLen);
                            }
                        }

                        // inserimento file
                        if (FileTmp.HasFile)
                        {
                            FileUpload blob = (FileUpload)dettaglio.FindControl(item);
                            oCompRisposteNewFile.Bytes_curr = (int)blob.PostedFile.InputStream.Length;

                            TextBox ext_curr = (TextBox)dettaglio.FindControl("item");
                            oCompRisposteNewFile.Descrizione = blob.FileName.ToString();
                            oCompRisposteNewFile.Blob = blob.FileBytes;
                            oCompRisposteNewFile.Ext_curr = extension.Substring(1, extension.Length - 1);
                            oCompRisposteNewFile.SelezioneId = oIndicatoreDet.SelezioneId;
                            oCompRisposteNewFile.CategoriaCod = oIndicatoreDet.CategoriaCod;
                            oCompRisposteNewFile.IndId = oIndicatoreDet.IndId;
                            oCompRisposteNewFile.IndDetId = oIndicatoreDet.IndDetId;
                            oCompRisposteNewFile.RispId = RispId;
                            oCompRisposteNewFile.DataIns = System.DateTime.Today.Date;
                            oCompRisposteNewFile.UsrIns = oUtente.UserId;
                            lCompFile.Add(oCompRisposteNewFile);
                        }
                    }
                    if (InsUpdComp)
                    {
                        oRisposta.SelezioneId = oIndicatoreDet.SelezioneId;
                        oRisposta.CategoriaCod = oIndicatoreDet.CategoriaCod;
                        oRisposta.IndId = oIndicatoreDet.IndId;
                        oRisposta.IndDetId = oIndicatoreDet.IndDetId;
                        oRisposta.TipoRiga = TipoRiga;
                        oRisposta.RispId = RispId;
                        if (CompId > 0)
                        {
                            clsUsrData oclsUsrData = DBUtility.LeggiUsrData(oIndicatoreDet.SelezioneId, oIndicatoreDet.CategoriaCod, oIndicatoreDet.IndId, oIndicatoreDet.IndDetId, RispId, TipoRiga, RigaId, CompId, "ris");
                            if (oclsUsrData.UsrIns != null)
                            {
                                oRisposta.UsrIns = oclsUsrData.UsrIns;
                                oRisposta.DataMod = System.DateTime.Today.Date;
                                oRisposta.UsrMod = oUtente.UserId;
                            }
                            else
                                oRisposta.UsrIns = oUtente.UserId;
                            if (oclsUsrData.DataIns != null)
                                oRisposta.DataIns = oclsUsrData.DataIns;
                            else
                                oRisposta.DataIns = System.DateTime.Today.Date;
                        }
                        else
                        {
                            oRisposta.UsrIns = oUtente.UserId;
                            oRisposta.DataIns = System.DateTime.Today.Date;
                        }
                        oCompRisposteNew.RigheRisposta.Add(oRisposta);
                        if (CompId > 0)
                            oCompRisposteNew.Stato = DBUtility.LeggiStatoCompilazioneCod(oIndicatoreDet.SelezioneId, CompId);
                        else
                            oCompRisposteNew.Stato = 10;
                        oCompRisposteNew.MatriDip = Int32.Parse(oUtenteDip.UserId);
                        oCompRisposteNew.MatriRsp = Int32.Parse(oUtenteDip.UserIdResp);
                        oCompRisposteNew.CompId = CompId;
                    }
                    if (InsUpdVal)
                    {
                        oValutazioneNew.SelezioneId = oIndicatoreDet.SelezioneId;
                        oValutazioneNew.CategoriaCod = oIndicatoreDet.CategoriaCod;
                        oValutazioneNew.IndId = oIndicatoreDet.IndId;
                        oValutazioneNew.IndDetId = oIndicatoreDet.IndDetId;
                        oValutazioneNew.CompId = CompId;
                        if (oValutazione == null)
                        {
                            oValutazioneNew.DataIns = System.DateTime.Today.Date;
                            oValutazioneNew.UsrIns = oUtente.UserId;
                        }
                        else
                        {
                            clsUsrData oclsUsrData = DBUtility.LeggiUsrData(oIndicatoreDet.SelezioneId, oIndicatoreDet.CategoriaCod, oIndicatoreDet.IndId, oIndicatoreDet.IndDetId, null, null, null, CompId, "val");
                            if (oclsUsrData.UsrIns != null)
                            {
                                oValutazioneNew.UsrIns = oclsUsrData.UsrIns;
                                oValutazioneNew.DataMod = System.DateTime.Today.Date;
                                oValutazioneNew.UsrMod = oUtente.UserId;
                            }
                            else
                                oValutazioneNew.UsrIns = oUtente.UserId;
                            if (oclsUsrData.DataIns != null)
                                oValutazioneNew.DataIns = oclsUsrData.DataIns;
                            else
                                oValutazioneNew.DataIns = System.DateTime.Today.Date;
                        }
                    }
                }
                if (oCompRisposteNew != null && oCompRisposteNew.RigheRisposta.Count != 0)
                {
                    //>>ga082015<< verifico se non esiste già una compilazione: situazione possibile: 
                    // se non si è ancora salvata alcuna risposta e si aprono due finestre: due comandi salva creano due comp_id
                    // caso Orsini peo 2015
                                      
                    try
                    {
                        if (oCompRisposte == null || oCompRisposte.RigheRisposta.Count == 0)
                        {
                            if (oCompRisposte == null)
                                oCompRisposteNew.CompId = DBUtility.rec_compid(null);
                            if (DBUtility.VerificaIns(oCompRisposteNew.RigheRisposta[0].SelezioneId, oCompRisposteNew.CompId, oCompRisposteNew.MatriDip.ToString(), null))
                            {//>>ga082015<<
                                DBUtility.ins_compilazione(oCompRisposteNew, null);
                                OpId = DBUtility.ins_log(oCompRisposteNew, "INSERT", oUtente.UserId, null);
                                btnSav.Enabled = false;
                                Session["btnSav"] = btnSav.Enabled;
                                CompId = oCompRisposteNew.CompId;
                            }
                            else
                            {
                                throw new CustomExceptions.OperazioneFallita("Attenzione: tentativo di duplicazione della compilazione, chiudere tutte le schede PEO e rientrare");
                            }
                        }
                        else
                        {
                            if (oCompRisposteNew.CompId != 0)
                            {
                                //DBUtility.upd_compilazione(oCompRisposteNew, null); >>ga13092016<<
                                DBUtility.upd_compilazione(oCompRisposte, oCompRisposteNew, null); //>>ga13092016<<

                                OpId = DBUtility.ins_log(oCompRisposteNew, "UPDATE", oUtente.UserId, null);
                                btnSav.Enabled = false;
                                Session["btnSav"] = btnSav.Enabled;
                            }
                        }
                    }
                        //>>ga082015<< inizio
                    catch (CustomExceptions.OperazioneFallita ex)
                    {
                        //this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + ex.CustomMessage + "');", true);
                        throw new CustomExceptions.OperazioneFallita("Attenzione: tentativo di duplicazione della compilazione, chiudere tutte le schede PEO e rientrare");
                        
                    }
                    //>>ga082015<< fine
                    catch (Exception ex)
                    {
                        string Message = "Operazione di inserimento/modifica Dettaglio fallita: " + ex.Message;
                        this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + Message + "');", true);
                    }
                }
                else
                {
                    try
                    {
                        if (oCompRisposte != null && CompId > 0)
                        {
                            foreach (Risposta tmpRisposta in oCompRisposte.RigheRisposta)
                            {
                                if (tmpRisposta.TipoRiga == 1 && (string)Session["Param"] == "com")
                                {
                                    OpId = DBUtility.del_risp_Nota(tmpRisposta.SelezioneId, tmpRisposta.CategoriaCod, CompId, tmpRisposta.IndId, tmpRisposta.IndDetId, tmpRisposta.TipoRiga, oUtente.UserId, null);
                                }
                                if (tmpRisposta.TipoRiga == 2 && (string)Session["Param"] == "val")
                                {
                                    OpId = DBUtility.del_risp_Nota(tmpRisposta.SelezioneId, tmpRisposta.CategoriaCod, CompId, tmpRisposta.IndId, tmpRisposta.IndDetId, tmpRisposta.TipoRiga, oUtente.UserId, null);
                                }
                                if (tmpRisposta.TipoRiga == 3 && (string)Session["Param"] == "amm")
                                {
                                    OpId = DBUtility.del_risp_Nota(tmpRisposta.SelezioneId, tmpRisposta.CategoriaCod, CompId, tmpRisposta.IndId, tmpRisposta.IndDetId, tmpRisposta.TipoRiga, oUtente.UserId, null);
                                }
                            }
                            btnSav.Enabled = false;
                            Session["btnSav"] = btnSav.Enabled;
                        }

                    }
                    catch (Exception ex)
                    {
                        string Message = "Operazione di inserimento/modifica Dettaglio fallita: " + ex.Message;
                        this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + Message + "');", true);
                    }
                }

                // insert / update / delete file 
                foreach (CompFile oCompFiledel in lCompFileDel)
                {
                    try
                    {
                        oCompFiledel.CompId = CompId;
                        DBUtility.del_compilazioneFile(oCompFiledel, OpId, oUtente.UserId, null);
                    }
                    catch (Exception ex)
                    {
                        string Message = "Operazione di cancellazione File fallita: " + ex.Message;
                        this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + Message + "');", true);
                    }
                }

                foreach (CompFile oCompfile in lCompFile)
                {
                    // inserisco il file se l'utente ha risposto alla domanda associata
                    if (DBUtility.ExistsCompilazioniUtente(oCompfile.SelezioneId, oCompfile.CategoriaCod, oCompfile.IndId, oCompfile.IndDetId, null, oCompfile.RispId, oCompfile.UsrIns, null))
                    {
                        try
                        {
                            oCompfile.CompId = CompId;
                            string TipoOp = DBUtility.ins_compilazioneFile(oCompfile, null);
                            if (TipoOp != null)
                                DBUtility.ins_compilazioneFile_log(oCompfile, OpId, TipoOp, oUtente.UserId, null);

                        }
                        catch (Exception ex)
                        {
                            //string Message = "Operazione di inserimento/modifica File fallita: " + ex.Message;
                            //this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + Message + "');", true);
                            throw new CustomExceptions.OperazioneFallita("Operazione di inserimento/modifica File fallita");
                        }
                    }
                    else
                    {
                        throw new CustomExceptions.OperazioneFallita("Occorre rispondere alla domanda prima di allegare il file");
                    }
                }
                if (oValutazioneNew != null && oValutazioneNew.CompId != 0)
                {
                    try
                    {
                        IndicatoreDetPuntMinMax oIndicatorePunteggi = new IndicatoreDetPuntMinMax();
                        oIndicatorePunteggi = DBUtility.leggi_inddetpuntid(oValutazioneNew.SelezioneId, oValutazioneNew.CategoriaCod, oValutazioneNew.IndId, oValutazioneNew.IndDetId, null);
                        // if (oValutazioneNew.Punteggio > oIndicatorePunteggi.PuntMax || oValutazioneNew.Punteggio < oIndicatorePunteggi.PuntMin)>>ga16102012>>
                        if (oValutazioneNew.Punteggio > oIndicatorePunteggi.PuntMax)
                            throw new CustomExceptions.Controlli("Punteggio assegnato non coerente");
                        if (oValutazione == null)
                        {
                            DBUtility.ins_valutazione(oValutazioneNew, null);
                            btnSav.Enabled = false;
                            Session["btnSav"] = btnSav.Enabled;
                        }
                        else if (oValutazioneNew.Punteggio != 0.0)
                        {
                            DBUtility.upd_valutazione(oValutazioneNew, null);
                            btnSav.Enabled = false;
                            Session["btnSav"] = btnSav.Enabled;
                        }
                        else if (oValutazioneNew.Punteggio == 0.0 && oValutazione.Punteggio != 0.0) // cancello punteggio da val_valutazioni
                        {
                            DBUtility.del_valutazione(oValutazioneNew, null);
                            btnSav.Enabled = false;
                            Session["btnSav"] = btnSav.Enabled;
                        }
                    }
                    catch (CustomExceptions.Controlli)
                    {
                        throw new CustomExceptions.Controlli("Punteggio assegnato non coerente");
                    }
                    catch (Exception ex)
                    {
                        string Message = "Operazione di inserimento/modifica Dettaglio fallita: " + ex.Message;
                        this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + Message + "');", true);
                        throw new Exception();
                    }
                }
                else if (oValutazione != null && oValutazione.CompId != 0)
                {
                    //>>pl-082012<< aggiunta condizione che venga cancellata la valutazione solo se
                    //il pulsante punteggio è abilitato
                    if ((oStatoValutazione.ValutazioniMod == "R" && oIndicatore.RspFlg == 1 && oIndicatore.AmmFlg != 1) ||
                        (oStatoValutazione.ValutazioniMod == "A" && oIndicatore.AmmFlg == 1) ||
                        (oStatoValutazione.ValutazioniMod == "T"))
                    {
                        try
                        {
                            DBUtility.del_valutazione(oValutazione, null);
                            btnSav.Enabled = false;
                            Session["btnSav"] = btnSav.Enabled;
                        }
                        catch (Exception ex)
                        {
                            string Message = "Operazione di inserimento/modifica Dettaglio fallita: " + ex.Message;
                            this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + Message + "');", true);
                        }
                    }
                }
                int tmpCompiId = DBUtility.RicercaCompilazione(oUtenteDip.UserId, SelezioneId, null);
                //Page.Response.Redirect("Risposte.aspx?SelezioneId=" + SelezioneId + "&CategoriaCod=" + CategoriaCod + "&IndId=" + IndId + "&IndDetId=" + IndDetId + "&CompId=" + CompiId);
                if (tmpCompiId == 0)
                    Page.Response.Redirect("Risposte.aspx?SelezioneId=" + SelezioneId + "&CategoriaCod=" + CategoriaCod + "&IndId=" + IndId + "&IndDetId=" + IndDetId);
                else
                    Page.Response.Redirect("Risposte.aspx?SelezioneId=" + SelezioneId + "&CategoriaCod=" + CategoriaCod + "&IndId=" + IndId + "&IndDetId=" + IndDetId + "&CompId=" + tmpCompiId);
            }

            catch (CustomExceptions.Controlli ex)
            {
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + ex.CustomMessage + "');", true);
            }
            catch (CustomExceptions.SessioneScaduta ex)
            {
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + ex.CustomMessage + "');", true);
            }
            catch (CustomExceptions.BigFile ex)
            {
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + ex.CustomMessage + "');", true);
            }
            catch (CustomExceptions.TipoFileErrato ex)
            {
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + ex.CustomMessage + "');", true);
            }
            catch (CustomExceptions.OperazioneFallita ex)
            {
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + ex.CustomMessage + "');", true);
            }
            catch (Exception ex)
            {
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + ex.Message + "');", true);
            }

        }

        protected void btnRit_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect("Compilazione.aspx");
        }

        protected void cb_change(object sender, System.EventArgs e)
        {
            //Page.Response.Redirect("Risposte.aspx?SelezioneId=" + SelezioneId + "&CategoriaCod=" + CategoriaCod + "&IndId=" +IndId+ "&IndDetId=" +IndDetId + "&CompId=" + CompId);
        }

        protected void vis_Click(object sender, EventArgs e)
        {

            ImageButton btn = sender as ImageButton;
            string cmdArg = btn.CommandArgument;
            string[] words = cmdArg.Split('/');
            string[] keys = words[1].Split('_');

            if (CompId != 0) //esiste una compilazione
            {
                DataRow dr = DBUtility.LeggiFile(CompId, SelezioneId, CategoriaCod, IndId, IndDetId, int.Parse(words[2]));
                try
                {
                    if (dr[0].ToString() != "")
                    {
                        string url = "ShowFile.aspx?ftype=" + dr[2].ToString().ToUpper();
                        Session["File"] = "";
                        Session["File"] = dr;
                        Page.Response.Redirect(url);
                    }
                    else
                    {
                        throw new CustomExceptions.TipoFileErrato();
                    }
                }
                catch (CustomExceptions.TipoFileErrato ex)
                {
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + ex.CustomMessage + "');", true);
                }

            }

        }
    }
}