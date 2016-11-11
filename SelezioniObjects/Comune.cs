using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SelezioniObjects
{
    public class clsStato
    {
        public string Stato
        {
            get;
            set;
        }
        public string DescrStato
        {
            get;
            set;
        }
    }
    public class clsAnno
    {
        public string Anno
        {
            get;
            set;
        }
    }
    public class clsCategoria
    {
        public string Categoria
        {
            get;
            set;
        }
    }
    public class clsTipoCtrl
    {
        public int TipoCtrl
        {
            get;
            set;
        }
        public string DescrTipo
        {
            get;
            set;
        }
    }
    public class clsTipoRiga
    {
        public int TipoRiga
        {
            get;
            set;
        }
        public string DescrRiga
        {
            get;
            set;
        }
    }
    public class clsUsrData
    {
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
    }
}
