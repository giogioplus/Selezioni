using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SelezioniObjects
{
    public class CompFile
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
        public string Descrizione
        {
            get;
            set;
        }
        public byte[] Blob
        {
            get;
            set;
        }
        public Int32 Bytes_curr
        {
            get;
            set;
        }
        public string Ext_curr
        {
            get;
            set;
        }
        public string UsrIns
        {
            get;
            set;
        }
        public DateTime DataIns
        {
            get;
            set;
        }
        public string UsrMod
        {
            get;
            set;
        }
        public DateTime DataMod
        {
            get;
            set;
        }
    }
}
