using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SelezioniObjects
{
    public class Selezione
    {
        public int SelezioneId
        {
            get;
            set;
        }
        public string CategoriaCod
        {
            get;
            set;
        }
        public string SelezioneCod
        {
            get;
            set;
        }
        public string Titolo
        {
            get;
            set;
        }
        public int Anno
        {
            get;
            set;
        }
        public string Descrizione
        {
            get;
            set;
        }
        public int Stato
        {
            get;
            set;
        }
        public string DescrStato
        {
            get;
            set;
        }
        public DateTime DataInizVal
        {
            get;
            set;
        }
        public DateTime DataFineVal
        {
            get;
            set;
        }
        public DateTime? DataTermPres
        {
            get;
            set;
        }
        public DateTime? DataTermCtrlAmm
        {
            get;
            set;
        }
        public DateTime? DataTermValResp
        {
            get;
            set;
        }
        public DateTime? DataTermCtrlDip
        {
            get;
            set;
        }
        public DateTime? DataTermValAmm
        {
            get;
            set;
        }
        public string Utente
        {
            get;
            set;
        }
    }
}
