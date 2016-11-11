using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SelezioniObjects
{
    public class StatoArchivio
    {
        public bool InArchivio
        {
            get;
            set;
        }
        public override string ToString()
        {
            return (InArchivio == true ? "T" : "F");
        }
    }
}
