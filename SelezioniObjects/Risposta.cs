using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SelezioniObjects
{
    public class Risposta
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
        public int RispId
        {
            get;
            set;
        }
        public int TipoRiga
        {
            get;
            set;
        }
        public int RigaId
        {
            get;
            set;
        }
        public string Risp
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
