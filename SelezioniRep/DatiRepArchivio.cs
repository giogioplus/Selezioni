using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using clDB;
using SelezioniObjects;
using System.Data;
using System.Web;

namespace SelezioniRep
{
    class DatiRepArchivio
    {
        static private ClsDB DBUtility = new ClsDB();
        static public DataSet ds;
        public static DataSet prepara_info(int pSelId, string pTipo, int pCompId, string pUtente)
        {
            ds = new DataSet();
            ds.Tables.Add(DBUtility.DtRiepilogoArchivio(pSelId, pTipo, pCompId));
            ds.Tables.Add(DBUtility.DtSchedaArchivio(pSelId, pTipo, pCompId));
            if (pCompId != 0)
                ds.Tables.Add(DBUtility.DtAnagArchivio(pSelId, pCompId));
            else
                ds.Tables.Add(DBUtility.DtAnagArchivio(pSelId, pUtente));
            return ds;
        }
        public static string EliminaLinkArchivio(string pvalue)
        {
            string newvalue = pvalue;
            // verifico se è stato inserito un link
            int initialString = pvalue.IndexOf("&lt;");
            if (initialString != -1)
            {
                int finalString = pvalue.IndexOf("/a&gt;");
                if (finalString != -1)
                {
                    int LengthString = (finalString + 5) - initialString + 1;
                    newvalue = pvalue.Remove(initialString, LengthString);
                }
            }
            // verifico se esistono diacritici
            newvalue = HttpUtility.HtmlDecode(newvalue);

            return newvalue;
        }
    }
}
