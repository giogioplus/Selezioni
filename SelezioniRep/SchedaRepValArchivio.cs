namespace SelezioniRep
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;
    using clDB;
    using SelezioniObjects;
    using System.Data;

    /// <summary>
    /// Summary description for SchedaRepVal.
    /// </summary>
    public partial class SchedaRepValArchivio : Telerik.Reporting.Report
    {
        private int SelezioneId;
        private Selezione oSelezione;
        ClsDB DBUtility = new ClsDB();
        DataSet ds;
        public SchedaRepValArchivio(int pSelId, string pTipo, int pCompId, string pUtente)
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            SelezioneId = pSelId;
            oSelezione = DBUtility.LeggiSelezione(SelezioneId);
            this.DataSource = oSelezione;
            txt_intest.Value = "(COMPILAZIONE A CURA DELL'UFFICIO SERV. RELAZIONI SINDACALI E DEL RESPONSABILE DI STRUTTURA)";
            txt_intest1.Value = "SCHEDA DI VALUTAZIONE AI FINI DELLA PROGRESSIONE ECONOMICA ORIZZONTALE " + oSelezione.Anno.ToString();
            ds = DatiRepArchivio.prepara_info(pSelId, pTipo, pCompId, pUtente);
            txt_NomeCognome.Value = ds.Tables[2].Rows[0]["Nome"].ToString() + " " + ds.Tables[2].Rows[0]["Cognome"].ToString();
            txt_Struttura.Value = ds.Tables[2].Rows[0]["Afferenza"].ToString();
            txt_Stato.Value = ds.Tables[2].Rows[0]["Stato"].ToString();

            tbl_riepilogo.DataSource = ds.Tables[0];
            if (pCompId == 0)
            {
                dettaglioRepArchivio1.Visible = false;
            }
        }
    }
}