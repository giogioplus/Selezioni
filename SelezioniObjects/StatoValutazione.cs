using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SelezioniObjects
{
    public class StatoValutazione
    {
        public string RisposteVis
        {
            get;
            set;
        }
        public string RisposteMod
        {
            get;
            set;
        }
        public string NoteVis
        {
            get;
            set;
        }
        public string NoteMod
        {
            get;
            set;
        }
        public string ValutazioniVis
        {
            get;
            set;
        }
        public string ValutazioniMod
        {
            get;
            set;
        }
        public bool Stampa
        {
            get;
            set;
        }
        public bool Chiusura
        {
            get;
            set;
        }
        public override string ToString()
        {
            return RisposteVis + RisposteMod + NoteVis + NoteMod + ValutazioniVis + ValutazioniMod + (Stampa==true ? "T" : "F") + (Chiusura == true ? "T" : "F");
        }
    }
}
