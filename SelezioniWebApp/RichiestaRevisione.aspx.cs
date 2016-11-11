using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using clDB;
using SelezioniObjects;
using System.Net.Mail;
using System.Data;

namespace SelezioniWebApp
{
    public partial class RichiestaRevisione : System.Web.UI.Page
    {
        private int SelezioneId;
        private int CompId;
        private Selezione oSelezione;
        private clsUtenteLogin oUtente;
        private clsUtenteLogin oUtenteDip;
        private StatoValutazione oStatoValutazione;

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
                else
                {
                    if (!Page.IsPostBack)
                    {
                        if ((string)Session["param"] != "com")
                        {
                            btnInviaEmail.Enabled = false;
                            btnAnnulla.Enabled = false;
                            txtAreaRevisione.Enabled = false;
                        }

                        oUtenteDip = (clsUtenteLogin)Session["UtenteDip"];
                        oUtente = (clsUtenteLogin)Session["Utente"];



                        txtUsername.Text = oUtenteDip.UserId + " " + oUtenteDip.Nome.ToString() + " " + oUtenteDip.Cognome.ToString();

                        SelezioneId = DBUtility.RicercaSelezione(oUtenteDip, "PEO", (string)Session["Param"], null);

                        // se lo stato della compilazione non è 40, inibisco l'invio delle email, ma permetto di visualizzare il testo eventualmente inviato
                        int codStatoAttuale = DBUtility.LeggiStatoCompilazioneCod(SelezioneId, oUtenteDip.UserId);
                        //codStatoAttuale = 40; //SOLO PER PROVE!!!!
                        if (codStatoAttuale != 40)
                        {
                            btnInviaEmail.Enabled = false;
                            btnAnnulla.Enabled = false;
                            txtAreaRevisione.Enabled = false;
                            lblAlert.Visible = true;
                        }

                        if (SelezioneId == 0 && oUtenteDip.Archivio != 1)
                            throw new CustomExceptions.SelezioneNonTrovata();
                        else if (SelezioneId == 0 && oUtenteDip.Archivio == 1)
                            throw new CustomExceptions.SelezioneArchiviata();  // utente con selezioni archiviate
                        oSelezione = DBUtility.LeggiSelezione(SelezioneId);

                        txtTitolo.Text = oSelezione.SelezioneId + " Anno " + oSelezione.Anno + " Categoria " + oSelezione.CategoriaCod.ToString();
                        // verifico se l'utente è abilitato alla selezione attiva
                        if (oSelezione.Anno != oUtenteDip.Anno)
                            throw new CustomExceptions.SelezioneArchiviata();
                        CompId = DBUtility.RicercaCompilazione(oUtenteDip.UserId, oSelezione.SelezioneId, null);

                        if (CompId == 0 && oUtenteDip.Archivio != 1)
                            throw new CustomExceptions.SelezioneNonTrovata();
                        else
                            txtCompId.Text = CompId.ToString();

                        oStatoValutazione = DBUtility.AggiornaStatoValutazione(SelezioneId, CompId, (string)Session["Param"]);

                        // verifico se è già stata inviata una email
                        DataTable richiestarevisione = new DataTable();
                        try
                        {
                            richiestarevisione = DBUtility.EstraiRichiestaRevisione(SelezioneId, oSelezione.CategoriaCod, CompId, oUtenteDip.UserId);

                            if (richiestarevisione.Rows.Count > 0)
                            {
                                btnInviaEmail.Enabled = false;
                                btnAnnulla.Enabled = false;
                                txtAreaRevisione.Text = richiestarevisione.Rows[0].ItemArray[0].ToString();
                                txtAreaRevisione.Enabled = false;
                                txtInvioEmail.Visible = true;
                                txtInvioEmail.Text = String.Format("{0:d}", richiestarevisione.Rows[0].ItemArray[1]);
                                lblInvioEmail.Visible = true;

                            }
                        }
                        catch (CustomExceptions.OperazioneFallita ex)
                        {
                            lblErr.Text = "Procedura Verifica esistenza richiesta revisione: ERRORE nella estrazione dei dati dalla tabella val_revisionepunteggi. Avvisa I.S.I.";
                            this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + lblErr.Text + "');", true);
                            btnInviaEmail.Enabled = false;
                            btnAnnulla.Enabled = false;
                            txtAreaRevisione.Enabled = false;

                        }
                    }
                    else
                    {
                        oUtenteDip = (clsUtenteLogin)Session["UtenteDip"];
                        oUtente = (clsUtenteLogin)Session["Utente"];
                        SelezioneId = DBUtility.RicercaSelezione(oUtenteDip, "PEO", (string)Session["Param"], null);
                        oSelezione = DBUtility.LeggiSelezione(SelezioneId);
                        CompId = DBUtility.RicercaCompilazione(oUtenteDip.UserId, oSelezione.SelezioneId, null);
                    }
                   
                }
            }
            catch (CustomExceptions.SessioneScaduta ex)
            {
            }
        }
        protected void mnu_RichiestaRevisione_MenuItemClick(object sender, MenuEventArgs e)
        {
            //string IdQuestionario = "";
            if (e.Item.Value == "Rit" )
            {
                Page.Response.Redirect("Compilazione.aspx");
            }

            
        }

        protected void InviaEmailRevisione(object sender, EventArgs e)
        {
            string sMessaggio = "";
            lblErr.Text = "";
            // aggiunto questo try per poter gestire i catch nested nella procedura invio email
            // errore invio email +
            // errore nella cancellazione del record dalla tabella val_revisionepunteggi
            try 
            {
                try
                {
                    // controllo che sia stato inserito del testo e che il messaggio non sia più lungo di 4000 caratteri
                    Int32 LunghezzaRisposta = 0;
                    if (txtAreaRevisione != null)
                        LunghezzaRisposta = System.Text.Encoding.UTF8.GetByteCount(txtAreaRevisione.Text.ToString());
                    else
                        LunghezzaRisposta = 0;
                    // il campo é un CLOB , non serve più la conta dei caratteri
                    //if (LunghezzaRisposta > 4000)
                    //{
                        //throw new CustomExceptions.NullORBigLenghtCampoVarChar("4000 caratteri");
                    //}
                    if (LunghezzaRisposta == 0)
                    {
                        throw new CustomExceptions.NullORBigLenghtCampoVarChar();
                    }
                    // inserimento richiesta nel db tabella val_revisionepunteggi
                    Dictionary<String, Object> coll = new Dictionary<String, Object>();

                    coll.Add("selezione_id", oSelezione.SelezioneId);
                    coll.Add("comp_id", CompId);
                    coll.Add("categoria_cod", oSelezione.CategoriaCod);
                    coll.Add("usr_ins", oUtenteDip.UserId);
                    coll.Add("data_ins", System.DateTime.Today.Date);
                    coll.Add("testoemail", txtAreaRevisione.Text);
                    try
                    {
                        DBUtility.ins_ricrev(coll);
                    }
                    catch (Exception ex)
                    {
                        throw new CustomExceptions.InserimentoRichiestaRevisione(ex.Message);
                    }

                    // invio email
                    string esitomail = "";
                    String TestoEmail = "";
                    String EmailUffAmm = "";
                    if (lblEmailUffAmm != null)
                    {
                        EmailUffAmm = lblEmailUffAmm.Text;
                    }
                    else
                    {
                        EmailUffAmm = "rel.sindacali@amm.units.it";
                    }
                    TestoEmail = txtAreaRevisione.Text.Replace("\r\n", "<br />");
                    try
                    {
                        esitomail = DBUtility.spedisci_mailRevisione(oSelezione, CompId, oUtenteDip, TestoEmail, EmailUffAmm);
                        sMessaggio = "E-Mail richiesta revisione punteggi è stata inviata con successo.";
                        this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + sMessaggio + "');", true);
                        btnInviaEmail.Enabled = false;
                        btnAnnulla.Enabled = false;
                        txtAreaRevisione.Enabled = false;
                        lblInvioEmail.Visible = true;
                        txtInvioEmail.Visible = true;
                        //txtInvioEmail.Text = String.Format("{0:d}", DateTime.Today.ToString());
                        txtInvioEmail.Text = DateTime.Today.ToString("dd/MM/yyyy");

                    }
                    catch
                    {
                        lblErr.Text = "Fallito invio E-Mail richiesta revisione punteggi.";
                        throw new CustomExceptions.InvioEmailRicRev();
                    }
                }
                catch (CustomExceptions.NullORBigLenghtCampoVarChar ex)
                {
                    lblErr.Text = "Controlla il campo Richiesta revisione punteggi";
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + lblErr.Text + "\\n" + ex.CustomMessage + "');", true);
                    lblErr.Text = lblErr.Text + "<br />" + ex.CustomMessage;
                }
                catch (CustomExceptions.InserimentoRichiestaRevisione ex)
                {
                    lblErr.Text = "Si è verificato un errore nella procedura di invio email. Avvisa I.S.I.";
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + lblErr.Text + "\\n" + ex.CustomMessage + "');", true);
                    lblErr.Text = "Si è verificato un errore nella procedura di invio email. Avvisa I.S.I." + "<br />" + ex.CustomMessage.Replace("\\n", "<br />");

                }
                catch (CustomExceptions.InvioEmailRicRev ex)
                {
                    // invio email fallito
                    // devo cancellare il record inserito in val_revisionepunteggi
                    //try
                    //{
                    Dictionary<String, Object> colldel = new Dictionary<String, Object>();

                    colldel.Add("selezione_id", oSelezione.SelezioneId);
                    colldel.Add("comp_id", CompId);
                    colldel.Add("categoria_cod", oSelezione.CategoriaCod);
                    colldel.Add("usr_ins", oUtenteDip.UserId);
                    try
                    {
                        DBUtility.del_ricrev(colldel);
                        lblErr.Text = "Invio Email fallito. Contattare ISI";
                        this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + ex.CustomMessage + "');", true);
                    }
                    catch (Exception exdel)
                    {
                        throw new CustomExceptions.GraveAnomalia();
                    }
                }
            }
            catch (CustomExceptions.GraveAnomalia ex)
            {
                lblErr.Text = "Invio Email fallito: Non è stato possibile ripristinare una situazione corretta per inviare una nuova email. Contattare ISI";
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + ex.CustomMessage + "');", true);
                btnInviaEmail.Enabled = false;
                btnAnnulla.Enabled = false;
                txtAreaRevisione.Enabled = false;
            }
        }
        
        protected void Annulla(object sender, EventArgs e)
        {
            //Page.Response.Redirect("Compilazione.aspx");
            txtAreaRevisione.Text = "";
        }
    }
}