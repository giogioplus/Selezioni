using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SelezioniObjects
{
    public class CompRisposte
    {
        public CompRisposte()
        {
            RigheRisposta = new List<Risposta>();
        }
        public int CompId
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
        public Int32 MatriDip
        {
            get;
            set;
        }
        public Int32 MatriRsp
        {
            get;
            set;
        }
        public List<Risposta> RigheRisposta
        {
            get;
            set;
        }
    }
}
