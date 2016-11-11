using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CustomExceptions
/// </summary>
public class CustomExceptions
{
    public class BaseStepException : ApplicationException
    {
        private string _customMessage;

        public string CustomMessage
        {
            get
            {
                return this._customMessage;
            }
            set
            {
                this._customMessage = value;
            }
        }
    }


    public class SelezioneEsistente : BaseStepException
    {
        public SelezioneEsistente(int pAnno, string pCatCod)
        {
            this.CustomMessage = "Selezione già esistente!";
            Anno = pAnno;
            CatCod = pCatCod;
        }

        public int Anno
        {
            get;
            set;
        }
        public string CatCod
        {
            get;
            set;
        }
    }
    public class SelezioneConValutazioni : BaseStepException
    {
        public SelezioneConValutazioni()
        {
            this.CustomMessage = "Esistono compilazioni per questa selezione!";
        }
    }
    public class SelezioneStatoErrato : BaseStepException
    {
        public SelezioneStatoErrato()
        {
            this.CustomMessage = "Stato non coerente per la selezione!";
        }
    }
    public class PaginaNonRichiamataCorrettamente : BaseStepException
    {
        public PaginaNonRichiamataCorrettamente()
        {
            this.CustomMessage = "Pagina non richiamata correttamente!";
        }
        public PaginaNonRichiamataCorrettamente(string pParam, string pMessage)
        {
            if (pMessage != null && pParam != null)
                this.CustomMessage = "Pagina non richiamata correttamente: " + pMessage + ", parametro/i " + pParam + " mancante/i!";
            else if (pMessage == null && pParam == null)
                this.CustomMessage = "Pagina non richiamata correttamente!";
            else if (pParam != null)
                this.CustomMessage = "Pagina non richiamata correttamente: parametro/i " + pParam + " mancante/i!";
            else
                this.CustomMessage = "Pagina non richiamata correttamente: " + pMessage;
        }
    }
    public class SessioneScaduta : BaseStepException
    {
        public SessioneScaduta()
        {
            this.CustomMessage = "La sessione è scaduta, si prega di uscire dalla procedura!";
        }
    }
    public class ChiudereSessione : BaseStepException
    {
        public ChiudereSessione()
        {
            this.CustomMessage = "Precedentemente sei uscito dall\\'applicativo PEO senza chiudere il BROWSER utilizzato." + "\\n" + "Per uscire correttamente dalla procedura chiudi il browser " + "\\n" + "e rientra nell\\'applicativo PEO";
        }
    }

    public class UtenteNonAbilitato : BaseStepException
    {
        public UtenteNonAbilitato()
        {
            this.CustomMessage = "Utente non abilitato alla procedura!";
        }
        public UtenteNonAbilitato(string pUser)
        {
            this.CustomMessage = "Utente " + pUser + " non abilitato alla procedura!";
        }
    }
    public class SelezioneArchiviata : BaseStepException
    {
        public SelezioneArchiviata()
        {
            this.CustomMessage = "Utente non abilitato alla selezione attiva, ci sono selezioni archiviate!";
        }
        public SelezioneArchiviata(string pUser)
        {
            this.CustomMessage = "Utente " + pUser + " non abilitato alla selezione attiva, ci sono selezioni archiviate!";
        }
    }
    public class SelezioniAncoraAttive : BaseStepException
    {
        public SelezioniAncoraAttive()
        {
            this.CustomMessage = "Non è possibile attivare questa selezione, se prima non si chiude quella passata!";
        }
        public SelezioniAncoraAttive(int pSelId, string pSelCod, int pAnno)
        {
            this.CustomMessage = "Non è possibile attivare questa selezione, se prima non si chiude la selezione " + pSelId + " - " + pSelCod + " - " + pAnno;
        }
    }
    public class Controlli : BaseStepException
    {
        public Controlli()
        {
            this.CustomMessage = "Controlli effettuati falliti";
        }
        public Controlli(string pMessage)
        {
            this.CustomMessage = "Controlli effettuati falliti: " + pMessage;
        }
    }
    public class SelezioneNonTrovata : BaseStepException
    {
        public SelezioneNonTrovata()
        {
            this.CustomMessage = "Non ci sono selezione Attive";
        }
        public SelezioneNonTrovata(string pMessage)
        {
            this.CustomMessage = "Non ci sono selezione Attive " + pMessage;
        }
    }

    public class TipoFileErrato : BaseStepException
    {
        public TipoFileErrato()
        {
            this.CustomMessage = "Tipo file errato: è possibile caricare solo file pdf";
        }
        public TipoFileErrato(string pMessage)
        {
            this.CustomMessage = "Tipo file errato: è possibile caricare solo file pdf " + pMessage;
        }
    }
    public class BigFile : BaseStepException
    {
        public BigFile()
        {
            this.CustomMessage = "Dimensione del File troppo grande";
        }
        public BigFile(string pMessage)
        {
            this.CustomMessage = "Dimensione del File troppo grande " + pMessage;
        }
    }
    public class OperazioneFallita : BaseStepException
    {
        public OperazioneFallita()
        {
            this.CustomMessage = "Operazione fallita";
        }
        public OperazioneFallita(string pMessage)
        {
            this.CustomMessage = "Operazione fallita " + pMessage;
        }
    }
    public class InserimentoRichiestaRevisione : BaseStepException
    {
        public InserimentoRichiestaRevisione()
        {
            this.CustomMessage = "Inserimento richiesta fallito, riprovare più tardi - ";
        }
        public InserimentoRichiestaRevisione(string pMessage)
        {
            this.CustomMessage = "Inserimento richiesta fallito, riprovare più tardi "+"\\n" + "Inserimento record nella tabella VAL_REVISIONEPUNTEGGI -> " + pMessage;
        }
    }
    public class InvioEmailRicRev : BaseStepException
    {
        public InvioEmailRicRev()
        {
            this.CustomMessage = "Invio Email fallito, riprovare più tardi";
        }
        public InvioEmailRicRev(string pMessage)
        {
            this.CustomMessage = "Invio Email fallito, riprovare più tardi " + pMessage;
        }
    }

    public class NullORBigLenghtCampoVarChar : BaseStepException{
        
        public NullORBigLenghtCampoVarChar()
        {
            this.CustomMessage = "Il campo non contiene caratteri";
        }
        public NullORBigLenghtCampoVarChar(string pMessage)
        {
            this.CustomMessage = "Il campo ha più di " + pMessage;
        }

    }
    public class GraveAnomalia : BaseStepException
    {

        public GraveAnomalia()
        {
            this.CustomMessage = "Invio Email fallito: Non è stato possibile ripristinare una situazione corretta per invio di una nuova email. Contattare ISI";
        }
        public GraveAnomalia(string pMessage)
        {
            this.CustomMessage = "Invio Email fallito: Non è stato possibile ripristinare una situazione corretta per invio di una nuova email. Contattare ISI" + pMessage;
        }

    }
}
