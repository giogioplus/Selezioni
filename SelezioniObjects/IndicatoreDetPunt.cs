using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SelezioniObjects
{
    public class IndicatoreDetPunt 
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
        public int PuntId
        {
            get;
            set;
        }
        public string DescrPunt
        {
            get;
            set;
        }
        public float Punt
        {
            get;
            set;
        }
    }
    public class IndicatoreDetPuntMinMax
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

        public float PuntMin
        {
            get;
            set;
        }
        public float PuntMax
        {
            get;
            set;
        }
    }
}
