using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SelezioniObjects
{
    public class IndicatoreDetComp
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
        public int CompId
        {
            get;
            set;
        }
        public string DescrInd
        {
            get;
            set;
        }
        public string DescrDet
        {
            get;
            set;
        }
        public int MaxRighe
        {
            get;
            set;
        }
        public bool FlRisposte
        {
            get;
            set;
        }
        public float Punteggio
        {
            get;
            set;
        }
    }
}
