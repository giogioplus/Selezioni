using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SelezioniObjects
{
    public class Valutazioni
    {
        public int CompId
        {
            get;
            set;
        }
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
        public int IndId
        {
            get;
            set;
        }
        public int IndDetId
        {
            get;
            set;
        }
        public string Descrizione
        {
            get;
            set;
        }
        public float Punteggio
        {
            get;
            set;
        }
        public string UsrIns
        {
            get;
            set;
        }
        public DateTime? DataIns
        {
            get;
            set;
        }
        public string UsrMod
        {
            get;
            set;
        }
        public DateTime? DataMod
        {
            get;
            set;
        }
    }
}
