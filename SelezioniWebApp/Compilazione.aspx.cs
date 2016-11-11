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
    public partial class Compilazione : System.Web.UI.Page
    {
        private int SelezioneId;
        private int CompId;
        private int IndId;
        private int IndDetId;
        private Selezione oSelezione;
        private clsUtenteLogin oUtente;
        private clsUtenteLogin oUtenteDip;
        private StatoValutazione oStatoValutazione;
        private StatoArchivio oStatoArchivio;
        private Indicatore oIndicatore;
        ClsDB DBUtility = new ClsDB();
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
                if (!Page.IsPostBack)
                {
                    
                    string sUser = "";
                    if ((string)Session["Param"] == "com")
                    {
                        sUser = ((clsUtenteLogin)Session["UtenteDip"]).UserId;
                        oUtente = (clsUtenteLogin)Session["Utente"];
                    }
                    try
                    {
                        if ((string)Session["Param"] == "com")
                        {
                            oUtenteDip = DBUtility.LeggiUtenteLogin(sUser);
                            Session["UtenteDip"] = oUtenteDip;
                        }
                        else
                        {
                            oUtenteDip = (clsUtenteLogin)Session["UtenteDip"];
                        }
                        if (oUtenteDip == null && (string)Session["Param"] == "com")
                        {
                            throw new CustomExceptions.UtenteNonAbilitato(sUser);
                        }

                        

                        //Cerca la selezione attiva con la Categoria dell'utente
                        SelezioneId = DBUtility.RicercaSelezione(oUtenteDip, "PEO", (string)Session["Param"], null);
                        if (SelezioneId == 0 && oUtenteDip.Archivio != 1)
                            throw new CustomExceptions.SelezioneNonTrovata();
                        else if (SelezioneId == 0 && oUtenteDip.Archivio == 1)
                            throw new CustomExceptions.SelezioneArchiviata();  // utente con selezioni archiviate
                        oSelezione = DBUtility.LeggiSelezione(SelezioneId);
                        // verifico se l'utente è abilitato alla selezione attiva
                        if (oSelezione.Anno != oUtenteDip.Anno)
                            throw new CustomExceptions.SelezioneArchiviata();
                        CompId = DBUtility.RicercaCompilazione(oUtenteDip.UserId, oSelezione.SelezioneId, null);

                       

                        //lblErr.Text = DBUtility.AggiornaStatoValutazione(SelezioneId, CompId, (string)Session["Param"]).ToString();
                        //Testata
                        lbl_anno.Text = oSelezione.Anno.ToString();
                        lbl_categoriacod.Text = oSelezione.CategoriaCod;
                        lbl_titolo.Text = oSelezione.Titolo;
                        lbl_descrizione.Text = oSelezione.Descrizione;
                        //Stato
                        lbl_descrstato.Text = DBUtility.LeggiStatoCompilazione(oSelezione.SelezioneId, oUtenteDip.UserId);
                        
                        //Date
                        lbl_inizval_datatermpres.Text = ((DateTime)oSelezione.DataInizVal).ToString("dd/MM/yyyy") + " - " + ((DateTime)oSelezione.DataTermPres).ToString("dd/MM/yyyy");
                        lbl_datatermcontrolloamm.Text = ((DateTime)oSelezione.DataTermCtrlAmm).ToString("dd/MM/yyyy");
                        lbl_datatermvalresp.Text = ((DateTime)oSelezione.DataTermValResp).ToString("dd/MM/yyyy");
                        lbl_datatermcontrollodip.Text = ((DateTime)oSelezione.DataTermCtrlDip).ToString("dd/MM/yyyy");
                        lbl_datatermvalamm.Text = ((DateTime)oSelezione.DataTermValAmm).ToString("dd/MM/yyyy");
                        //Anagrafica
                        lbl_NomeCognome.Text = oUtenteDip.Nome + " " + oUtenteDip.Cognome;
                        lbl_Nascita.Text = oUtenteDip.DataNascita.ToShortDateString() + " - " + oUtenteDip.LuogoNascita;
                        lbl_Residenza.Text = oUtenteDip.IndirizzoResidenza;
                        lbl_Categoria.Text = oUtenteDip.Categoria;
                        lbl_Afferenza.Text = oUtenteDip.Afferenza;
                        lbl_TelInterno.Text = oUtenteDip.TelInterno;
                        lbl_Email.Text = oUtenteDip.Email;
                        lbl_Valutatore.Text = oUtenteDip.CognomeResp.ToString() + " " + oUtenteDip.NomeResp.ToString(); //>>ga22072013<<
                        //StatoValutazione
                        oStatoValutazione = DBUtility.AggiornaStatoValutazione(SelezioneId, CompId, (string)Session["Param"]);
                        //>>ga2013agosto<< inizio
                        if (Request.QueryString.Count == 1 && Request.QueryString["Param"] == "Chd")
                        {
                            this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + "Domanda TRASMESSA con successo" + "');", true);
                            int CodStatoSel = DBUtility.LeggiStatoCompilazioneCod(oSelezione.SelezioneId, oUtenteDip.UserId);
                            if (CodStatoSel != 20)
                            {
                                DBUtility.InviaEmail(oUtenteDip, oSelezione, lbl_descrstato.Text, CompId, CodStatoSel);
                            }
                        }
                        //>>ga2013agosto<< fine
                        //Dettaglio
                        grdDomande.DataSource = DBUtility.LeggiIndicatoriDetComp(oSelezione.SelezioneId, oUtenteDip.UserId, (string)Session["Param"]);
                        grdDomande.DataBind();
                    }
                    catch (CustomExceptions.UtenteNonAbilitato)
                    {
                        throw new CustomExceptions.UtenteNonAbilitato(sUser);
                    }
                    catch (CustomExceptions.SelezioneArchiviata)
                    {
                        throw new CustomExceptions.SelezioneArchiviata(sUser);
                    }
                    catch (CustomExceptions.SelezioneNonTrovata)
                    {
                        throw new CustomExceptions.SelezioneNonTrovata();
                    }
                }
                else
                {
                    try
                    {
                        oUtenteDip = (clsUtenteLogin)Session["UtenteDip"];
                        oUtente = (clsUtenteLogin)Session["Utente"];
                        //Cerca la selezione attiva con la Categoria dell'utente
                        SelezioneId = DBUtility.RicercaSelezione(oUtenteDip, "PEO", (string)Session["Param"], null);
                       /*
                        if (SelezioneId == 0)
                            throw new CustomExceptions.SelezioneNonTrovata();
                        oSelezione = DBUtility.LeggiSelezione(SelezioneId);
                        if (oSelezione.Anno != oUtenteDip.Anno)
                            throw new CustomExceptions.SelezioneArchiviata();
                        */
                        if (SelezioneId == 0 && oUtenteDip.Archivio != 1)
                            throw new CustomExceptions.SelezioneNonTrovata();
                        else if (SelezioneId == 0 && oUtenteDip.Archivio == 1)
                            throw new CustomExceptions.SelezioneArchiviata();  // utente con selezioni archiviate
                        oSelezione = DBUtility.LeggiSelezione(SelezioneId);
                        // verifico se l'utente è abilitato alla selezione attiva
                        if (oSelezione.Anno != oUtenteDip.Anno)
                            throw new CustomExceptions.SelezioneArchiviata();
                        CompId = DBUtility.RicercaCompilazione(oUtenteDip.UserId, oSelezione.SelezioneId, null);//StatoValutazione
                        oStatoValutazione = DBUtility.AggiornaStatoValutazione(SelezioneId, CompId, (string)Session["Param"]);
                    }
                    catch (CustomExceptions.SelezioneNonTrovata)
                    {
                        throw new CustomExceptions.SelezioneNonTrovata();
                    }
                    catch (CustomExceptions.SelezioneArchiviata)
                    {
                        throw new CustomExceptions.SelezioneArchiviata(oUtenteDip.UserId);
                    }
                }
            }
            catch (CustomExceptions.SessioneScaduta ex)
            {
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + ex.CustomMessage + "');", true);
                abilita_disabilita_menu(false, false, true, false, false);
                testata.Visible = false;
                anagrafica.Visible = false;
            }
            catch (CustomExceptions.UtenteNonAbilitato ex)
            {
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + ex.CustomMessage + "');", true);
                abilita_disabilita_menu(false, false, true, false, false);
                testata.Visible = false;
                anagrafica.Visible = false;
            }
            catch (CustomExceptions.SelezioneNonTrovata ex)
            {
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + ex.CustomMessage + "');", true);
                abilita_disabilita_menu(false, false, true, false, false);
                testata.Visible = false;
                anagrafica.Visible = false;
            }
            catch (CustomExceptions.SelezioneArchiviata ex)
            {
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + ex.CustomMessage + "');", true);
                abilita_disabilita_menu(false, false, true, true, false);
                testata.Visible = false;
                anagrafica.Visible = false;
            }
        }
        protected void Page_LoadComplete(object sender, EventArgs e)
        {
           
            oUtenteDip = (clsUtenteLogin)Session["UtenteDip"];
            bool visualizzaRicRev = false;
            if (oUtenteDip != null)
            {
                oStatoArchivio = DBUtility.AggiornaStatoArchivio(oUtenteDip, null);
                if (SelezioneId != 0 || oStatoArchivio.InArchivio)
                {
                    if (SelezioneId != 0)
                    {
                        oStatoValutazione = DBUtility.AggiornaStatoValutazione(SelezioneId, CompId, (string)Session["Param"]);
                        //abilita_disabilita_menu(oStatoValutazione.Stampa, oStatoValutazione.Chiusura, true, oStatoArchivio.InArchivio); >>ga082016<<


                        switch (DBUtility.LeggiStatoCompilazioneCod(SelezioneId, CompId))
                        {
                            case 15:
                                modifica_item_menu("Chd", "Trasmetti al responsabile");
                                break;
                            case 20:
                                modifica_item_menu("Chd", "Trasmetti al dipendente");
                                break;
                            case 40:
                                modifica_item_menu("Chd", "Trasmetti all'ufficio amministrativo");
                                visualizzaRicRev = true; // visualizzo la voce di menù Richiesta revisione punteggi
                                break;
                            case 50:
                            case 90:
                                visualizzaRicRev = true; // visualizzo la voce di menù Richiesta revisione punteggi
                                modifica_item_menu("Chd", "Chiudi scheda");
                                break;
                            default:
                                break;
                        }
                        abilita_disabilita_menu(oStatoValutazione.Stampa, oStatoValutazione.Chiusura, true, oStatoArchivio.InArchivio, visualizzaRicRev); 

                    }
                    else
                        abilita_disabilita_menu(true, false, true, oStatoArchivio.InArchivio, visualizzaRicRev);
                }
            }
            else
                abilita_disabilita_menu(false, false, true, false, false);

            if (Session["Param"] != null && Session["Param"].ToString() != "com")
            {
                modifica_item_menu("Rit", "Ritorna");
            }
        }
        protected void mnu_CompilaSelezione_MenuItemClick(object sender, MenuEventArgs e)
        {
            int idFile;
            if (e.Item.Value == "Rit" && Session["Param"].ToString() == "com")
            {
                //Page.Response.Redirect("http://www.units.it/intra/modulistica/peo2011/");
                //Page.Response.Redirect("http://www.units.it/intra/modulistica/peo2015/");
                Page.Response.Redirect("https://www.units.it/intra/modulistica/peo/");
            }
            else if (e.Item.Value == "Rit" && Session["Param"].ToString() != "com")
            {
                Page.Response.Redirect("Valutazione.aspx");
            }
            else if (e.Item.Value == "Stm")
            {
                Page.Response.Redirect("StampaSelezione.aspx?SelezioneId=" + SelezioneId.ToString() + "&CompId=" + DBUtility.RicercaCompilazione(oUtenteDip.UserId, SelezioneId, null));
            }
            else if (e.Item.Value == "Chd")
            {
                bool esitoResponsabile = false;
                int tmpCompId = DBUtility.RicercaCompilazione(oUtenteDip.UserId, SelezioneId, null);
                if (tmpCompId != 0)
                {
                    try
                    {
                        // >>ga2013agosto<< inizio
                        if (Session["Param"].ToString() == "val")
                        {

                            esitoResponsabile = DBUtility.controlli_responsabile(SelezioneId, tmpCompId);
                            if (!esitoResponsabile)
                                throw new CustomExceptions.Controlli("La Scheda non è completa: Verificare se mancano risposte e/o punteggi. Verificare anche se i punteggi sono corretti");

                        }
                        // >>ga2013agosto<< fine
                        DBUtility.CambiaStato(SelezioneId, tmpCompId, oUtenteDip.UserId, null);
                        //if ((string)Session["Param"] == "com")
                        Response.Redirect("Compilazione.aspx?param=Chd");
                        //else
                        //    Response.Redirect("Compilazione.aspx");
                    }
                    catch (CustomExceptions.Controlli ex)
                    {
                        this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + ex.CustomMessage + "');", true);
                    }
                    catch
                    {
                        this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + "Inoltro domanda non riuscito" + "');", true);
                    }
                }
                else
                {
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + "Non esistono compilazioni da inoltrare" + "');", true);
                }
            }
			//>>ga22072013<<
			else if (e.Item.Value == "Faq")
            {
                idFile = DBUtility.VisualizzaIstruzioniCompilazione(oSelezione.CategoriaCod, oSelezione.Anno);
                //Page.Response.Redirect("https://apps.units.it/sitedirectory/SelezioniPersonale/Peo2012.html");
                if (idFile != 0)
                {
                    Response.Redirect("IstruzioniSF.aspx?Id=" + idFile);
                }
                else
                {
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + "Non ci sono istruzioni legate alla PEO" + "');", true);
                }
            }
            //>>ga23072013<<
            else if (e.Item.Value == "Arc")
            {
                  Response.Redirect("Archivio.aspx");
            }
            //>>ga082016<<
            else if (e.Item.Value == "RicRev")
            {
                  Response.Redirect("RichiestaRevisione.aspx");
            }
            
        }
        protected void grdDomande_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            oStatoValutazione = DBUtility.AggiornaStatoValutazione(SelezioneId, CompId, (string)Session["Param"]);
            if (grdDomande.Rows.Count > 0 && ((ImageButton)grdDomande.Rows[grdDomande.Rows.Count - 1].Cells[1].Controls[1]).ID == "btnDel")
            {
                string img = "~/Images/S_B_DISP.gif";
                oIndicatore = DBUtility.LeggiIndicatori(SelezioneId, oSelezione.CategoriaCod, int.Parse(((Label)grdDomande.Rows[grdDomande.Rows.Count - 1].Cells[4].Controls[1]).Text))[0];
                if (!DBUtility.ExistsNote(SelezioneId, oSelezione.CategoriaCod, int.Parse(((Label)grdDomande.Rows[grdDomande.Rows.Count - 1].Cells[4].Controls[1]).Text), int.Parse(((Label)grdDomande.Rows[grdDomande.Rows.Count - 1].Cells[5].Controls[1]).Text)))
                {
                    //Indicatore non di Note (TipoRiga!=6)
                    //Visualizzazione bidone
//                    if (oStatoValutazione.RisposteMod == "D" && (oIndicatore.DipFlg == 1 || oIndicatore.AmmFlg == 1) ||
                    if ((oStatoValutazione.RisposteMod == "D" && oIndicatore.DipFlg == 1 && (string)Session["Param"] == "com") ||
                        (oStatoValutazione.RisposteMod == "R" && (oIndicatore.DipFlg == 0 && oIndicatore.AmmFlg == 0 && oIndicatore.RspFlg == 1)))
                    {
                        grdDomande.Rows[grdDomande.Rows.Count - 1].Cells[1].Controls[1].Visible = true;
                    }
                    if ((string)Session["Param"] == "com")
                    {
                        if (oStatoValutazione.RisposteMod == "D" || oStatoValutazione.RisposteMod == "T")
                            img = "~/Images/S_B_DPCH.gif";
                    }
                    else if ((string)Session["Param"] == "amm")
                    {
                        if ((oStatoValutazione.ValutazioniMod == "A" && oIndicatore.AmmFlg == 1) ||
                            (oStatoValutazione.ValutazioniMod == "T") ||
                            (oStatoValutazione.RisposteMod == "D" && DBUtility.ExistsTipoRiga(SelezioneId, oSelezione.CategoriaCod, int.Parse(((Label)grdDomande.Rows[grdDomande.Rows.Count - 1].Cells[4].Controls[1]).Text), int.Parse(((Label)grdDomande.Rows[grdDomande.Rows.Count - 1].Cells[5].Controls[1]).Text), 1)) ||
                            (oStatoValutazione.RisposteMod == "A" && DBUtility.ExistsTipoRiga(SelezioneId, oSelezione.CategoriaCod, int.Parse(((Label)grdDomande.Rows[grdDomande.Rows.Count - 1].Cells[4].Controls[1]).Text), int.Parse(((Label)grdDomande.Rows[grdDomande.Rows.Count - 1].Cells[5].Controls[1]).Text), 3)) ||
                            (oStatoValutazione.RisposteMod == "T"))
                            img = "~/Images/S_B_DPCH.gif";
                    }
                    else if ((string)Session["Param"] == "val")
                    {
                        if ((oStatoValutazione.ValutazioniMod == "R" && oIndicatore.RspFlg == 1) ||
                            (oStatoValutazione.ValutazioniMod == "T") ||
                            (oStatoValutazione.RisposteMod == "R" && DBUtility.ExistsTipoRiga(SelezioneId, oSelezione.CategoriaCod, int.Parse(((Label)grdDomande.Rows[grdDomande.Rows.Count - 1].Cells[4].Controls[1]).Text), int.Parse(((Label)grdDomande.Rows[grdDomande.Rows.Count - 1].Cells[5].Controls[1]).Text), 2)) ||
                            (oStatoValutazione.RisposteMod == "T"))
                            img = "~/Images/S_B_DPCH.gif";
                    }
                }
                else
                {
                    //Indicatore di Note (TipoRiga=6)
                    if ((string)Session["Param"] == "com")
                    {
                        if (oStatoValutazione.NoteMod == "D" || oStatoValutazione.NoteMod == "T")
                            img = "~/Images/S_B_DPCH.gif";
                    }
                    else if ((string)Session["Param"] == "amm")
                    {
                        if (oStatoValutazione.NoteMod == "A" || oStatoValutazione.NoteMod == "T")
                            img = "~/Images/S_B_DPCH.gif";
                    }
                    else if ((string)Session["Param"] == "val")
                    {
                        if (oStatoValutazione.NoteMod == "R" || oStatoValutazione.NoteMod == "T")
                            img = "~/Images/S_B_DPCH.gif";
                    }
                }
                ((ImageButton)grdDomande.Rows[grdDomande.Rows.Count - 1].Cells[0].Controls[1]).ImageUrl = img;
            }
        }
        protected void btnChiudiCmp_Click(object sender, EventArgs e)
        {

        }
        protected void btnStampaCmp_Click(object sender, EventArgs e)
        {

        }
        private void abilita_disabilita_menu(bool pStampa, bool pChiudi, bool pRitorna, bool pArchivio, bool pRevisione)
        {
            foreach (MenuItem i in mnu_CompilaSelezione.Items)
            {
                if (i.Value == "Stm")
                {
                    i.Selectable = pStampa;
                }
                else if (i.Value == "Chd")
                {
                    i.Selectable = pChiudi;
                }
                else if (i.Value == "Rit")
                {
                    i.Selectable = pRitorna;
                }
                else if (i.Value == "Arc")
                {
                    //>>ga082015<< inizio
                    // una peo archiviata non è visibile al valutatore
                    //if (Session["param"] != null && Session["param"].ToString() == "val")
                    //{
                    //    i.Selectable = false;
                    //}
                   // else
                    //{
                       // //>>ga082015<< fine
                     //   i.Selectable = pArchivio;
                   // }//>>ga082015<<
                    //>>ga082015<< in archivio:anche il valutatore può vedere la scheda completa del dipendente vedi email del 27/08/2015 Alessandra
                    i.Selectable = pArchivio;
                }
                
            }
            if (!pRevisione && (mnu_CompilaSelezione.FindItem("RicRev") != null))
            {
                mnu_CompilaSelezione.Items.Remove(mnu_CompilaSelezione.FindItem("RicRev"));
            }
        }
        private void modifica_item_menu(string pItem, string pTitolo)
        {
            foreach (MenuItem i in mnu_CompilaSelezione.Items)
            {
                if (i.Value == pItem)
                {
                    i.Text = pTitolo;
                }
            }
        }
        protected void grdDomande_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Session.Remove("btnSav");
            Session["btnSav"] = null;
            if (e.CommandName == "Sel")
            {
                IndId = int.Parse(((Label)grdDomande.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lblIndId")).Text.ToString());
                IndDetId = int.Parse(((Label)grdDomande.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lblIndDetId")).Text.ToString());
                if (CompId > 0)
                    Page.Response.Redirect("Risposte.aspx?SelezioneId=" + oSelezione.SelezioneId.ToString() + "&CategoriaCod=" + oSelezione.CategoriaCod + "&IndId=" + IndId.ToString() + "&IndDetId=" + IndDetId.ToString() + "&CompId=" + CompId.ToString());
                else
                    Page.Response.Redirect("Risposte.aspx?SelezioneId=" + oSelezione.SelezioneId.ToString() + "&CategoriaCod=" + oSelezione.CategoriaCod + "&IndId=" + IndId.ToString() + "&IndDetId=" + IndDetId.ToString());
            }
            else if (e.CommandName == "Del")
            {
                IndId = int.Parse(((Label)grdDomande.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lblIndId")).Text.ToString());
                IndDetId = int.Parse(((Label)grdDomande.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lblIndDetId")).Text.ToString());
                if (DBUtility.ExistsCompilazioni(oSelezione.SelezioneId, oSelezione.CategoriaCod, IndId, IndDetId, CompId, null))
                {
                    try
                    {
                        DBUtility.del_risp_IndicatoreDet(oSelezione.SelezioneId, oSelezione.CategoriaCod, CompId, IndId, IndDetId, oUtente.UserId, null);
                        Response.Redirect("Compilazione.aspx");
                    }
                    catch
                    {
                        throw new CustomExceptions.OperazioneFallita();
                    }
                }
                else
                {
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + "Non ci sono compilazioni per questa domanda" + "');", true);
                }
            }
        }

    }
}