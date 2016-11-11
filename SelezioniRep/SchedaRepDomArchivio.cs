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
    using DB_ODP;

    /// <summary>
    /// Summary description for SelezioneRep.
    /// </summary>
    public partial class SchedaRepDomArchivio : Telerik.Reporting.Report
    {
        private int SelezioneId;
        private Selezione oSelezione;
        ClsDB DBUtility = new ClsDB();
        DataSet ds;
        public SchedaRepDomArchivio(int pSelId, string pTipo, int pCompId, string pUtente)
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
            // verifico che la selezione richiesta sia effettivamente dell'utente loggato >>ga15092016<<
            if (!DBUtility.ExistsCompilazioniUtenteArchivio(SelezioneId, oSelezione.CategoriaCod, null, null, pCompId, null, pUtente, null))
                System.Web.HttpContext.Current.Response.Redirect("SessioneScaduta.aspx");
            this.DataSource = oSelezione;
            txt_intest.Value = "DOMANDA DI PARTECIPAZIONE AI FINI DELLA PROGRESSIONE ECONOMICA ORIZZONTALE " + oSelezione.Anno.ToString();
            ds = DatiRepArchivio.prepara_info(pSelId, pTipo, pCompId, pUtente);
            tbl_riepilogo.DataSource = ds.Tables[0];
            txt_NomeCognome.Value = ds.Tables[2].Rows[0]["Nome"].ToString() + " " + ds.Tables[2].Rows[0]["Cognome"].ToString();
            txt_Struttura.Value = ds.Tables[2].Rows[0]["Afferenza"].ToString();
            txt_Stato.Value = ds.Tables[2].Rows[0]["Stato"].ToString();
        }
    }
}