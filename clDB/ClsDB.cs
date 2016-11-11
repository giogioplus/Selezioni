using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DB_ODP;
using System.Data;
using System.Web;
using SelezioniObjects;
using System.Net.Mail;
namespace clDB
{
    public class ClsDB
    {
        accesso_db accDB;
        public ClsDB()
        {
            accDB = new accesso_db();
        }
        public Exception exception
        {
            get;
            set;
        }
        //Autenticazione
        public string VerificaUtente(string pUser)
        {
            string sResult = "";
            string sUser = "";
            if (pUser != null)
            {
                if (pUser.IndexOf("\\") != -1)
                {
                    sUser = pUser.Substring(pUser.IndexOf("\\") + 1);
                }
                else
                {
                    sUser = pUser;
                }
            }
            String sql = "select matri " +
                           " from val_autorizzazioni" +
                           " where matri = :sUser" +
                           " and tipo_utente in ('02', '04') ";
            Dictionary<String, Object> coll = new Dictionary<String, Object>();
            coll.Add("matri", sUser);
            try
            {
                sResult = accDB.sel_dati(sql, coll).Rows.Count.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return sResult;
        }
        //Selezione
        public DataTable PopolaDDLCategoria()
        {
            String sql = "select des_categoria as des_categoria, categoria as categoria from val_categorie" +
                         " union select ' -- Seleziona Categoria --' as des_categoria, '0' as categoria" +
                         " from dual order by 1";
            return accDB.sel_dati(sql, null);
        }
        public DataTable PopolaDDLSelezioneCod()
        {
            String sql = "select des_selezione as des_selezione, cod_selezione as cod_selezione from val_cod_selezione" +
                         " union select ' -- Seleziona tipologia selezione --' as des_selezione, '0' as cod_selezione" +
                         " from dual order by 1";
            return accDB.sel_dati(sql, null);
        }
        public Selezione LeggiSelezione(int pSelId)
        {
            Dictionary<String, Object> coll;
            coll = new Dictionary<string, object>();
            String sql = "select q.selezione_id, q.categoria_cod, q.anno, q.titolo, q.descrizione, " +
                          " q.stato," +
                          " q.data_iniz_val, q.data_fine_val, s.descr descr_stato, q.usr_ins," +
                          " q.selezione_cod, q.data_term_pres, q.data_term_val_resp, q.data_term_val_amm," +
                          " q.data_term_controllo_amm, q.data_term_controllo_dip " +
                          " from val_selezioni q, val_stati s" +
                          " where q.selezione_id = :pSelId " +
                          " and s.stato = q.stato";
            coll.Add("selezione_id", pSelId);

            DataTable dt;
            dt = accDB.sel_dati(sql, coll);
            Selezione oSelezione = new Selezione();
            oSelezione.SelezioneId = Int32.Parse(dt.Rows[0]["selezione_id"].ToString());
            oSelezione.CategoriaCod = dt.Rows[0]["categoria_cod"].ToString();
            oSelezione.Anno = Int32.Parse(dt.Rows[0]["anno"].ToString());
            oSelezione.Titolo = dt.Rows[0]["titolo"].ToString();
            oSelezione.Descrizione = dt.Rows[0]["descrizione"].ToString();
            oSelezione.DataInizVal = DateTime.Parse(dt.Rows[0]["data_iniz_val"].ToString());
            oSelezione.DataFineVal = DateTime.Parse(dt.Rows[0]["data_fine_val"].ToString());
            oSelezione.DescrStato = dt.Rows[0]["descr_stato"].ToString();
            oSelezione.SelezioneCod = dt.Rows[0]["selezione_cod"].ToString();
            if (dt.Rows[0]["data_term_pres"].ToString() != "")
                oSelezione.DataTermPres = DateTime.Parse(dt.Rows[0]["data_term_pres"].ToString());
            if (dt.Rows[0]["data_term_controllo_amm"].ToString() != "")
                oSelezione.DataTermCtrlAmm = DateTime.Parse(dt.Rows[0]["data_term_controllo_amm"].ToString());
            if (dt.Rows[0]["data_term_val_resp"].ToString() != "")
                oSelezione.DataTermValResp = DateTime.Parse(dt.Rows[0]["data_term_val_resp"].ToString());
            if (dt.Rows[0]["data_term_controllo_dip"].ToString() != "")
                oSelezione.DataTermCtrlDip = DateTime.Parse(dt.Rows[0]["data_term_controllo_dip"].ToString());
            if (dt.Rows[0]["data_term_val_amm"].ToString() != "")
                oSelezione.DataTermValAmm = DateTime.Parse(dt.Rows[0]["data_term_val_amm"].ToString());
            oSelezione.Stato = Int32.Parse(dt.Rows[0]["stato"].ToString());
            return oSelezione;
        }
        public void ins_selezione(Dictionary<string, object> p_coll_s)
        {
            clsTransaction clsTrx = new clsTransaction();
            try
            {
                accDB.insert_row("val_selezioni", p_coll_s);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ins_indicatore(Dictionary<string, object> p_coll_s)
        {
            clsTransaction clsTrx = new clsTransaction();
            try
            {
                accDB.insert_row("val_indicatori", p_coll_s);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void upd_selezione(Dictionary<string, object> p_coll_s, Dictionary<string, object> p_coll_w)
        {
            try
            {
                accDB.update_row("val_selezioni", p_coll_s, p_coll_w);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void del_selezione(int pSelId)
        {
            Dictionary<String, Object> coll;
            clsTransaction clsTrx = new clsTransaction();
            try
            {
                try
                {
                    coll = new Dictionary<string, object>();
                    coll.Add("selezione_id", pSelId);
                    clsTrx.del_row_trx("val_indicatori_det_punteggi", coll);
                    clsTrx.del_row_trx("val_indicatori_det_riga", coll);
                    clsTrx.del_row_trx("val_indicatori_det", coll);
                    clsTrx.del_row_trx("val_indicatori", coll);
                    clsTrx.del_row_trx("val_selezioni", coll);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                clsTrx.rollback_trx();
                clsTrx = null;
                throw ex;
            }
            clsTrx.commit_trx();
            clsTrx = null;
        }
        public Stato LeggiStato(int pSelId, int pIndId, int pIndDetId)
        {
            int oTestata = 0;
            int oIndicatori = 0;
            int oDettagli = 0;
            int oRiga = 0;
            int oPunt = 0;
            Dictionary<String, Object> coll;
            coll = new Dictionary<string, object>();
            String sql = "select count(*)" +
                          " from val_selezioni" +
                          " where selezione_id = :pSelId ";
            coll.Add("selezione_id", pSelId);
            oTestata = int.Parse(accDB.sel_dati(sql, coll).Rows[0][0].ToString());
            sql = "select count(*)" +
                          " from val_indicatori" +
                          " where selezione_id = :pSelId ";
            oIndicatori = int.Parse(accDB.sel_dati(sql, coll).Rows[0][0].ToString());
            sql = "select count(*)" +
                          " from val_indicatori_det" +
                          " where selezione_id = :pSelId ";
            if (pIndId != 0)
            {
                coll.Add("ind_id", pIndId);
                sql += " and ind_id = :pIndId";
                oDettagli = int.Parse(accDB.sel_dati(sql, coll).Rows[0][0].ToString());
            }
            if (pIndId != 0 && pIndDetId != 0)
            {
                coll.Add("ind_det_id", pIndDetId);
                sql = "select count(*)" +
                         " from val_indicatori_det_riga" +
                         " where selezione_id = :pSelId " +
                         " and ind_id = :pIndId" +
                         " and ind_det_id = :pIndDetId";
                oRiga = int.Parse(accDB.sel_dati(sql, coll).Rows[0][0].ToString());

                sql = "select count(*)" +
                         " from val_indicatori_det_punteggi" +
                         " where selezione_id = :pSelId " +
                         " and ind_id = :pIndId" +
                         " and ind_det_id = :pIndDetId";
                oPunt = int.Parse(accDB.sel_dati(sql, coll).Rows[0][0].ToString());
            }
            if (LeggiSelezione(pSelId).Stato == 1)
            {
                return Stato.SoloTst;
            }
            else
            {
                if (oRiga != 0 && oPunt != 0)
                    return Stato.RigaPunt;
                else if (oRiga != 0)
                    return Stato.Riga;
                else if (oPunt != 0)
                    return Stato.Punt;
                else if (oDettagli != 0)
                    return Stato.Det;
                else if (oIndicatori != 0)
                    return Stato.Ind;
                else if (oTestata != 0)
                    return Stato.Tst;
                else
                    return Stato.NoTst;
            }

        }
        //SelSelezione
        public IList<clsStato> PopolaDDLFiltroStato()
        {
            String sql = "select to_char(stato) stato, descr, 1 ord  from val_stati" +
                         " union select 'Tutti' stato, 'Tutti' descr, 0 ord from dual" +
                         " order by 1";
            clsStato oStato;
            IList<clsStato> lStato = new List<clsStato>();
            DataTable dt = accDB.sel_dati(sql, null);
            foreach (DataRow riga in dt.Rows)
            {
                oStato = new clsStato();
                oStato.Stato = riga["stato"].ToString();
                oStato.DescrStato = riga["descr"].ToString();
                lStato.Add(oStato);
            }
            return lStato;
        }
        public IList<clsAnno> PopolaDDLFiltroAnno()
        {
            String sql = "select distinct to_char(anno) anno, 1 ord from val_selezioni" +
                         " union select 'Tutti' anno, 0 ord from dual" +
                         " order by 2";
            clsAnno oAnno;
            IList<clsAnno> lAnno = new List<clsAnno>();
            DataTable dt = accDB.sel_dati(sql, null);
            foreach (DataRow riga in dt.Rows)
            {
                oAnno = new clsAnno();
                oAnno.Anno = riga["anno"].ToString();
                lAnno.Add(oAnno);
            }
            return lAnno;
        }
        public IList<clsCategoria> PopolaDDLFiltroCategoria()
        {
            String sql = "select distinct categoria_cod, 1 ord from val_selezioni" +
                         " union select 'Tutti' categoria_cod, 0 ord from dual" +
                         " order by 2";
            clsCategoria oCategoria;
            IList<clsCategoria> lCategoria = new List<clsCategoria>();
            DataTable dt = accDB.sel_dati(sql, null);
            foreach (DataRow riga in dt.Rows)
            {
                oCategoria = new clsCategoria();
                oCategoria.Categoria = riga["categoria_cod"].ToString();
                lCategoria.Add(oCategoria);
            }
            return lCategoria;
        }
        public IList<Selezione> LeggiSelezioni(string pStato, string pAnno, string pCategoria, int pSelId)
        {
            Dictionary<String, Object> coll;
            coll = new Dictionary<string, object>();
            String sql = "select q.selezione_id, q.categoria_cod, q.anno, q.titolo, q.descrizione," +
                          " q.data_iniz_val, q.data_fine_val, s.descr descr_stato, q.usr_ins," +
                          " q.selezione_cod, q.data_term_pres, q.data_term_val_resp, q.data_term_val_amm," +
                          " q.data_term_controllo_amm, q.data_term_controllo_dip " +
                          " from val_selezioni q, val_stati s" +
                          " where s.stato = q.stato ";
            if (pStato != null && pStato != "Tutti")
            {
                sql += " and s.stato = :pStato ";
                coll.Add("stato", pStato);
            }
            if (pAnno != null && pAnno != "Tutti")
            {
                sql += " and q.anno = :pAnno ";
                coll.Add("anno", pAnno);
            }
            if (pCategoria != null && pCategoria != "Tutti")
            {
                sql += " and q.categoria_cod = :pCategoria ";
                coll.Add("categoria", pCategoria);
            }
            if (pSelId != 0)
            {
                sql += " and q.selezione_id = :pSelId ";
                coll.Add("selezione_id", pSelId);
            }

            sql += " order by 3 desc, 2, 1 desc";

            Selezione oSelezione;
            IList<Selezione> lSelezioni = new List<Selezione>();
            DataTable dt;
            dt = accDB.sel_dati(sql, coll);
            foreach (DataRow riga in dt.Rows)
            {
                oSelezione = new Selezione();
                oSelezione.SelezioneId = Int32.Parse(riga["selezione_id"].ToString());
                oSelezione.CategoriaCod = riga["categoria_cod"].ToString();
                oSelezione.Anno = Int32.Parse(riga["anno"].ToString());
                oSelezione.Titolo = riga["titolo"].ToString();
                oSelezione.Descrizione = riga["descrizione"].ToString();
                oSelezione.DataInizVal = DateTime.Parse(riga["data_iniz_val"].ToString());
                oSelezione.DataFineVal = DateTime.Parse(riga["data_fine_val"].ToString());
                oSelezione.DescrStato = riga["descr_stato"].ToString();
                oSelezione.SelezioneCod = riga["selezione_cod"].ToString();
                if (riga["data_term_pres"].ToString() != "")
                    oSelezione.DataTermPres = DateTime.Parse(riga["data_term_pres"].ToString());
                if (riga["data_term_controllo_amm"].ToString() != "")
                    oSelezione.DataTermCtrlAmm = DateTime.Parse(riga["data_term_controllo_amm"].ToString());
                if (riga["data_term_val_resp"].ToString() != "")
                    oSelezione.DataTermValResp = DateTime.Parse(riga["data_term_val_resp"].ToString());
                if (riga["data_term_controllo_dip"].ToString() != "")
                    oSelezione.DataTermCtrlDip = DateTime.Parse(riga["data_term_controllo_dip"].ToString());
                if (riga["data_term_val_amm"].ToString() != "")
                    oSelezione.DataTermValAmm = DateTime.Parse(riga["data_term_val_amm"].ToString());
                lSelezioni.Add(oSelezione);
            }
            return lSelezioni;
        }
        public void dupl_selezione(int pSelId, int pAnno, string pCategoria, string pUtente)
        {
            Dictionary<String, Object> coll;
            coll = new Dictionary<string, object>();
            string sSelId = "";
            int OldSelId = pSelId;
            int NewSelId = 0;
            int newAnno = 0;
            string newCatCod = "";
            clsTransaction clsTrx = new clsTransaction();
            try
            {

                // val_selezioni
                IList<Selezione> iSelezione = LeggiSelezioni(null, null, null, pSelId);
                Selezione oSelezione = iSelezione[0];
                coll.Add("titolo", oSelezione.Titolo);
                if (pCategoria != null)
                {
                    coll.Add("categoria_cod", pCategoria);
                    newCatCod = pCategoria;
                }
                else
                {
                    coll.Add("categoria_cod", oSelezione.CategoriaCod);
                    newCatCod = oSelezione.CategoriaCod;

                }
                coll.Add("selezione_cod", oSelezione.SelezioneCod);
                if (pAnno != 0)
                {
                    coll.Add("anno", pAnno);
                    newAnno = pAnno;
                }
                else
                {
                    coll.Add("anno", oSelezione.Anno);
                    newAnno = oSelezione.Anno;
                }

                coll.Add("descrizione", oSelezione.Descrizione);
                coll.Add("stato", 0);
                coll.Add("data_iniz_val", oSelezione.DataInizVal);
                coll.Add("data_fine_val", oSelezione.DataFineVal);
                coll.Add("data_term_pres", oSelezione.DataTermPres);
                coll.Add("data_term_controllo_amm", oSelezione.DataTermCtrlAmm);
                coll.Add("data_term_val_resp", oSelezione.DataTermValResp);
                coll.Add("data_term_controllo_dip", oSelezione.DataTermCtrlDip);
                coll.Add("data_term_val_amm", oSelezione.DataTermValAmm);
                coll.Add("usr_ins", pUtente);
                coll.Add("data_ins", System.DateTime.Today.Date);
                try
                {
                    clsTrx.insert_row_trx("val_selezioni", coll);

                    string sqlVal = "SELECT val_selezione_id.currval  FROM dual";
                    sSelId = clsTrx.sel_dati(sqlVal, null).Rows[0][0].ToString();
                    NewSelId = int.Parse(sSelId.ToString());
                    // val_indicatori

                    IList<Indicatore> lIndicatori = LeggiIndicatori(OldSelId, null, null);
                    foreach (var oIndicatore in lIndicatori)
                    {
                        try
                        {
                            dupl_indicatori(OldSelId, NewSelId, oIndicatore.IndId, newAnno, newCatCod, clsTrx, pUtente);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }

                }
                catch (Exception)
                {
                    CustomExceptions.SelezioneEsistente ex = new CustomExceptions.SelezioneEsistente(newAnno, newCatCod);
                    throw ex;
                }
            }
            catch (CustomExceptions.SelezioneEsistente ex)
            {
                clsTrx.rollback_trx();
                clsTrx = null;
                throw ex;
            }
            catch (Exception ex)
            {
                clsTrx.rollback_trx();
                clsTrx = null;
                throw ex;
            }

            clsTrx.commit_trx();
            clsTrx = null;
        }
        //Archivia selezione
        public void ArchiviaSelezione(int pSelId, int pAnno, string pCategoria, string pUtente)
        {
            Dictionary<String, Object> coll;
            coll = new Dictionary<string, object>();
            string sSelId = "";
            int OldSelId = pSelId;
            int NewSelId = 0;
            int newAnno = 0;
            string newCatCod = "";
            clsTransaction clsTrx = new clsTransaction();
            clsTransaction OldClsTrx = new clsTransaction();
            try
            {

                // val_selezioni
                IList<Selezione> iSelezione = LeggiSelezioni(null, null, null, pSelId);
                Selezione oSelezione = iSelezione[0];
                coll.Add("titolo", oSelezione.Titolo);
                if (pCategoria != null)
                {
                    coll.Add("categoria_cod", pCategoria);
                    newCatCod = pCategoria;
                }
                else
                {
                    coll.Add("categoria_cod", oSelezione.CategoriaCod);
                    newCatCod = oSelezione.CategoriaCod;

                }
                coll.Add("selezione_cod", oSelezione.SelezioneCod);
                if (pAnno != 0)
                {
                    coll.Add("anno", pAnno);
                    newAnno = pAnno;
                }
                else
                {
                    coll.Add("anno", oSelezione.Anno);
                    newAnno = oSelezione.Anno;
                }

                coll.Add("descrizione", oSelezione.Descrizione);
                coll.Add("stato", 99);
                coll.Add("data_iniz_val", oSelezione.DataInizVal);
                coll.Add("data_fine_val", oSelezione.DataFineVal);
                coll.Add("data_term_pres", oSelezione.DataTermPres);
                coll.Add("data_term_controllo_amm", oSelezione.DataTermCtrlAmm);
                coll.Add("data_term_val_resp", oSelezione.DataTermValResp);
                coll.Add("data_term_controllo_dip", oSelezione.DataTermCtrlDip);
                coll.Add("data_term_val_amm", oSelezione.DataTermValAmm);
                coll.Add("usr_ins", pUtente);
                coll.Add("data_ins", System.DateTime.Today.Date);
                coll.Add("selezione_id", int.Parse(oSelezione.SelezioneId.ToString()));
                try
                {
                    clsTrx.insert_row_trx("val_selezioni_bck", coll);
                    OldClsTrx = clsTrx;

                    IList<Indicatore> lIndicatori = LeggiIndicatori(OldSelId, null, null);
                    foreach (var oIndicatore in lIndicatori)
                    {
                        try
                        {
                            archivia_indicatori(OldSelId, OldSelId, oIndicatore.IndId, newAnno, newCatCod, clsTrx, pUtente);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                    // copio le tabelle risposte e valutazione
                    try
                    {
                        archivia_risposte(OldSelId, oSelezione.CategoriaCod, clsTrx);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    try
                    {
                        archivia_valutazioni(OldSelId, oSelezione.CategoriaCod, clsTrx);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }



                }
                catch (Exception)
                {
                    CustomExceptions.SelezioneEsistente ex = new CustomExceptions.SelezioneEsistente(newAnno, newCatCod);
                    throw ex;
                }

            }
            catch (CustomExceptions.SelezioneEsistente ex)
            {
                clsTrx.rollback_trx();
                clsTrx = null;
                throw ex;
            }
            catch (Exception ex)
            {
                clsTrx.rollback_trx();
                clsTrx = null;
                throw ex;
            }

            clsTrx.commit_trx();
            clsTrx = null;
        }
        public void archivia_indicatori(int pSelIdOld, int pSelIdNew, Int32 pIndicatore, int pAnno, string pCatCod, clsTransaction p_clsTrx, string pUtente)
        {
            Dictionary<String, Object> coll = new Dictionary<string, object>();
            clsTransaction clsTrx = p_clsTrx;
            if (clsTrx == null)
            {
                clsTrx = new clsTransaction();
            }
            else
            {

            }
            try
            {
                try
                {
                    IList<Indicatore> lIndicatore = new List<Indicatore>();
                    Indicatore NewIndicatore = new Indicatore();
                    IList<IndicatoreDet> lIndicatoreDet = new List<IndicatoreDet>();
                    IList<IndicatoreDetRiga> lIndicatoreDetRiga = new List<IndicatoreDetRiga>();
                    IList<IndicatoreDetPunt> lIndicatoreDetPunt = new List<IndicatoreDetPunt>();

                    lIndicatore = LeggiIndicatori(pSelIdOld, null, pIndicatore);
                    int IndId = rec_indid(pSelIdOld, pCatCod, null);
                    foreach (Indicatore OldIndicatore in lIndicatore)
                    {
                        try
                        {
                            coll.Add("selezione_id", pSelIdNew);
                            coll.Add("CATEGORIA_COD", pCatCod);
                            coll.Add("IND_ID", int.Parse(OldIndicatore.IndId.ToString()));
                            coll.Add("DESCR", OldIndicatore.Descr.ToString());
                            coll.Add("NOTE_DIP", OldIndicatore.NoteDip.ToString());
                            coll.Add("NOTE_VAL", OldIndicatore.NoteVal.ToString());
                            coll.Add("DIP_FLG", int.Parse(OldIndicatore.DipFlg.ToString()));
                            coll.Add("RSP_FLG", int.Parse(OldIndicatore.RspFlg.ToString()));
                            coll.Add("AMM_FLG", int.Parse(OldIndicatore.AmmFlg.ToString()));
                            coll.Add("ORD", int.Parse(OldIndicatore.Ord.ToString()));
                            coll.Add("USR_INS", pUtente.ToString());
                            coll.Add("data_ins", System.DateTime.Today.Date);
                            clsTrx.insert_row_trx("val_indicatori_bck", coll);

                            lIndicatoreDet = LeggiIndicatoriDet(pSelIdOld, null, int.Parse(OldIndicatore.IndId.ToString()), null);
                            try
                            {
                                foreach (IndicatoreDet oIndicatoreDet in lIndicatoreDet)
                                {
                                    // insert indicatore det 
                                    coll.Clear();
                                    coll.Add("selezione_id", pSelIdNew);
                                    coll.Add("CATEGORIA_COD", pCatCod);
                                    coll.Add("IND_ID", int.Parse(OldIndicatore.IndId.ToString()));
                                    coll.Add("IND_DET_ID", int.Parse(oIndicatoreDet.IndDetId.ToString()));
                                    coll.Add("DESCR", oIndicatoreDet.DescrDet.ToString());
                                    coll.Add("NOTE_DIP", oIndicatoreDet.NoteDetDip.ToString());
                                    coll.Add("NOTE_VAL", oIndicatoreDet.NoteDetVal.ToString());
                                    coll.Add("ORD", int.Parse(oIndicatoreDet.Ord.ToString()));
                                    coll.Add("MAX_RIGHE", int.Parse(oIndicatoreDet.MaxRighe.ToString()));
                                    coll.Add("USR_INS", pUtente.ToString());
                                    coll.Add("data_ins", System.DateTime.Today.Date);
                                    clsTrx.insert_row_trx("val_indicatori_det_bck", coll);

                                    lIndicatoreDetRiga = LeggiIndicatoriDetRiga(pSelIdOld, null, int.Parse(oIndicatoreDet.IndId.ToString()), int.Parse(oIndicatoreDet.IndDetId.ToString()), null, null);
                                    try
                                    {
                                        foreach (IndicatoreDetRiga oIndicatoreDetRiga in lIndicatoreDetRiga)
                                        {
                                            // insert indicatore detriga
                                            coll.Clear();
                                            coll.Add("selezione_id", pSelIdNew);
                                            coll.Add("CATEGORIA_COD", pCatCod);
                                            coll.Add("IND_ID", int.Parse(OldIndicatore.IndId.ToString()));
                                            coll.Add("IND_DET_ID", int.Parse(oIndicatoreDetRiga.IndDetId.ToString()));
                                            coll.Add("TIPO_RIGA", int.Parse(oIndicatoreDetRiga.TipoRiga.ToString()));
                                            coll.Add("RIGA_ID", int.Parse(oIndicatoreDetRiga.RigaId.ToString()));
                                            if (oIndicatoreDetRiga.OrdRiga.ToString() != "")
                                            {
                                                coll.Add("ORD", int.Parse(oIndicatoreDetRiga.OrdRiga.ToString()));
                                            }
                                            coll.Add("TIPO_CTRL", int.Parse(oIndicatoreDetRiga.TipoCtrl.ToString()));
                                            coll.Add("DESCR", oIndicatoreDetRiga.DescrRiga.ToString());
                                            coll.Add("USR_INS", pUtente.ToString());
                                            coll.Add("data_ins", System.DateTime.Today.Date);
                                            clsTrx.insert_row_trx("val_indicatori_det_riga_bck", coll);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        throw ex;
                                    }

                                    lIndicatoreDetPunt = LeggiIndicatoriDetPunt(pSelIdOld, null, int.Parse(oIndicatoreDet.IndId.ToString()), int.Parse(oIndicatoreDet.IndDetId.ToString()), null);
                                    try
                                    {
                                        foreach (IndicatoreDetPunt oIndicatoreDetPunt in lIndicatoreDetPunt)
                                        {
                                            // insert indicatore detrigapunti
                                            coll.Clear();
                                            coll.Add("selezione_id", pSelIdNew);
                                            coll.Add("CATEGORIA_COD", pCatCod);
                                            coll.Add("IND_ID", int.Parse(OldIndicatore.IndId.ToString()));
                                            coll.Add("IND_DET_ID", int.Parse(oIndicatoreDetPunt.IndDetId.ToString()));
                                            coll.Add("PUNT_ID", int.Parse(oIndicatoreDetPunt.PuntId.ToString()));
                                            coll.Add("DESCR", oIndicatoreDetPunt.DescrPunt.ToString());
                                            coll.Add("PUNT", float.Parse(oIndicatoreDetPunt.Punt.ToString()));
                                            coll.Add("USR_INS", pUtente.ToString());
                                            coll.Add("data_ins", System.DateTime.Today.Date);
                                            clsTrx.insert_row_trx("val_indicatori_det_punteggi_bk", coll);
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        throw ex;
                                    }

                                }
                            }

                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }


                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                if (p_clsTrx == null)
                {
                    clsTrx.rollback_trx();
                    clsTrx = null;
                }
                throw ex;
            }
            if (p_clsTrx == null)
            {
                clsTrx.commit_trx();
                clsTrx = null;
            }
        }
        public void archivia_risposte(int pSelId, string pCatCod, clsTransaction p_clsTrx)
        {
            Dictionary<String, Object> coll = new Dictionary<string, object>();
            clsTransaction clsTrx = p_clsTrx;

            IList<RispostaArchivio> lRisposte = new List<RispostaArchivio>();
            if (clsTrx == null)
            {
                clsTrx = new clsTransaction();
            }
            else
            {

            }
            try
            {
                coll.Clear();
                coll.Add("selezione_id", pSelId);
                coll.Add("CATEGORIA_COD", pCatCod);
                lRisposte = LeggiRisposteArchivio(pSelId, pCatCod, null);
                try
                {

                    foreach (RispostaArchivio oRisposta in lRisposte)
                    {

                        // insert risposte
                        coll.Clear();
                        coll.Add("COMP_ID", int.Parse(oRisposta.CompId.ToString()));
                        coll.Add("SELEZIONE_ID", pSelId);
                        coll.Add("CATEGORIA_COD", pCatCod);
                        coll.Add("IND_ID", int.Parse(oRisposta.IndId.ToString()));
                        coll.Add("IND_DET_ID", int.Parse(oRisposta.IndDetId.ToString()));
                        coll.Add("RISP_ID", int.Parse(oRisposta.RispId.ToString()));
                        coll.Add("TIPO_RIGA", int.Parse(oRisposta.TipoRiga.ToString()));
                        coll.Add("RIGA_ID", int.Parse(oRisposta.RigaId.ToString()));
                        coll.Add("RISP", oRisposta.Risp.ToString());
                        coll.Add("MATRI_DIP", int.Parse(oRisposta.MatriDip.ToString()));
                        coll.Add("MATRI_RSP", int.Parse(oRisposta.MatriResp.ToString()));
                        coll.Add("STATO", int.Parse(oRisposta.Stato.ToString()));
                        if (oRisposta.UsrIns != null && oRisposta.UsrIns != "")
                            coll.Add("USR_INS", int.Parse(oRisposta.UsrIns.ToString()));
                        if (oRisposta.DataIns != null)
                            coll.Add("DATA_INS", oRisposta.DataIns);
                        if (oRisposta.UsrMod != null && oRisposta.UsrMod != "")
                            coll.Add("USR_MOD", int.Parse(oRisposta.UsrMod.ToString()));
                        if (oRisposta.DataMod != null)
                            coll.Add("DATA_MOD", oRisposta.DataMod);
                        clsTrx.insert_row_trx("val_risposte_bck", coll);

                    }


                }
                catch (Exception ex)
                {
                    throw ex;
                }
                try
                {
                    coll.Clear();
                    coll.Add("selezione_id", pSelId);
                    coll.Add("CATEGORIA_COD", pCatCod);
                    clsTrx.del_row_trx("val_risposte", coll);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }


            catch (Exception ex)
            {
                if (p_clsTrx == null)
                {
                    clsTrx.rollback_trx();
                    clsTrx = null;
                }
                throw ex;
            }
            if (p_clsTrx == null)
            {
                clsTrx.commit_trx();
                clsTrx = null;
            }

        }
        public void archivia_valutazioni(int pSelId, string pCatCod, clsTransaction p_clsTrx)
        {
            Dictionary<String, Object> coll = new Dictionary<string, object>();
            clsTransaction clsTrx = p_clsTrx;

            IList<Valutazioni> lValutazioni = new List<Valutazioni>();
            if (clsTrx == null)
            {
                clsTrx = new clsTransaction();
            }
            else
            {

            }
            try
            {
                coll.Clear();
                coll.Add("selezione_id", pSelId);
                coll.Add("CATEGORIA_COD", pCatCod);
                lValutazioni = LeggiValutazioniArchivio(pSelId, pCatCod, null);
                try
                {

                    foreach (Valutazioni oValutazioni in lValutazioni)
                    {

                        // insert risposte
                        coll.Clear();
                        coll.Add("COMP_ID", int.Parse(oValutazioni.CompId.ToString()));
                        coll.Add("selezione_id", pSelId);
                        coll.Add("CATEGORIA_COD", pCatCod);
                        coll.Add("IND_ID", int.Parse(oValutazioni.IndId.ToString()));
                        coll.Add("IND_DET_ID", int.Parse(oValutazioni.IndDetId.ToString()));
                        coll.Add("DESCRIZIONE", oValutazioni.Descrizione.ToString());
                        coll.Add("PUNTEGGIO", float.Parse(oValutazioni.Punteggio.ToString()));
                        if (oValutazioni.UsrIns != null && oValutazioni.UsrIns != "")
                            coll.Add("USR_INS", int.Parse(oValutazioni.UsrIns.ToString()));
                        if (oValutazioni.DataIns != null)
                            coll.Add("data_ins", oValutazioni.DataIns);
                        if (oValutazioni.UsrMod != null && oValutazioni.UsrMod != "")
                            coll.Add("USR_MOD", int.Parse(oValutazioni.UsrMod.ToString()));
                        if (oValutazioni.DataMod != null)
                            coll.Add("data_mod", oValutazioni.DataMod);
                        clsTrx.insert_row_trx("val_valutazioni_bck", coll);

                    }


                }
                catch (Exception ex)
                {
                    throw ex;
                }
                try
                {
                    coll.Clear();
                    coll.Add("selezione_id", pSelId);
                    coll.Add("CATEGORIA_COD", pCatCod);
                    clsTrx.del_row_trx("val_valutazioni", coll);

                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }


            catch (Exception ex)
            {
                if (p_clsTrx == null)
                {
                    clsTrx.rollback_trx();
                    clsTrx = null;
                }
                throw ex;
            }
            if (p_clsTrx == null)
            {
                clsTrx.commit_trx();
                clsTrx = null;
            }

        }

        //Indicatore
        public IList<Indicatore> LeggiIndicatori(int pSelId, string sCatcod, int? pIndId)
        {
            Dictionary<String, Object> coll;
            coll = new Dictionary<string, object>();
            String sql = "select SELEZIONE_ID, CATEGORIA_COD, IND_ID, DESCR, " +
                         " NOTE_DIP, NOTE_VAL,DIP_FLG, RSP_FLG, AMM_FLG, ORD" +
                          " from val_indicatori" +
                          " where selezione_id = :pSelId ";
            coll.Add("selezione_id", pSelId);
            if (sCatcod != null)
            {
                sql += " and categoria_cod = :sCatcod";
                coll.Add("categoria_cod", sCatcod);
            }

            if (pIndId != null)
            {
                sql += " and ind_id = :pIndId";
                coll.Add("ind_id", pIndId);
            }
            sql += " order by ORD";
            Indicatore oIndicatore;
            IList<Indicatore> lIndicatori = new List<Indicatore>();
            DataTable dt;
            dt = accDB.sel_dati(sql, coll);
            foreach (DataRow riga in dt.Rows)
            {
                oIndicatore = new Indicatore();
                oIndicatore.SelezioneId = Int32.Parse(riga["selezione_id"].ToString());
                oIndicatore.CategoriaCod = riga["categoria_cod"].ToString();
                oIndicatore.IndId = Int32.Parse(riga["ind_id"].ToString());
                oIndicatore.Descr = riga["descr"].ToString();
                oIndicatore.NoteDip = riga["note_dip"].ToString();
                oIndicatore.NoteVal = riga["note_val"].ToString();
                oIndicatore.DipFlg = int.Parse(riga["dip_flg"].ToString());
                oIndicatore.RspFlg = int.Parse(riga["rsp_flg"].ToString());
                oIndicatore.AmmFlg = int.Parse(riga["amm_flg"].ToString());
                oIndicatore.Ord = int.Parse(riga["ord"].ToString());
                lIndicatori.Add(oIndicatore);
            }
            return lIndicatori;
        }
        public void ins_indicatore(Indicatore pIndicatore, clsTransaction p_clsTrx, string pUtente)
        {
            Dictionary<String, Object> coll;
            clsTransaction clsTrx = p_clsTrx;
            if (clsTrx == null)
            {
                clsTrx = new clsTransaction();
            }
            try
            {
                coll = new Dictionary<String, Object>();
                coll.Add("selezione_id", pIndicatore.SelezioneId);
                coll.Add("categoria_cod", pIndicatore.CategoriaCod);
                coll.Add("descr", pIndicatore.Descr);
                coll.Add("ind_id", rec_indid(pIndicatore.SelezioneId, pIndicatore.CategoriaCod, clsTrx));
                coll.Add("note_dip", pIndicatore.NoteDip);
                coll.Add("note_val", pIndicatore.NoteVal);
                coll.Add("dip_flg", pIndicatore.DipFlg);
                coll.Add("rsp_flg", pIndicatore.RspFlg);
                coll.Add("amm_flg", pIndicatore.AmmFlg);
                coll.Add("ord", pIndicatore.Ord);
                coll.Add("usr_ins", pUtente);
                coll.Add("data_ins", System.DateTime.Today.Date);
                try
                {
                    clsTrx.insert_row_trx("val_indicatori", coll);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                if (p_clsTrx == null)
                {
                    clsTrx.rollback_trx();
                    clsTrx = null;
                }
                throw ex;
            }
            if (p_clsTrx == null)
            {
                clsTrx.commit_trx();
                clsTrx = null;
            }
        }
        public void upd_indicatore(Indicatore pIndicatore, clsTransaction p_clsTrx, string pUtente)
        {
            Dictionary<String, Object> coll;
            Dictionary<String, Object> coll_w;
            clsTransaction clsTrx = p_clsTrx;
            if (clsTrx == null)
            {
                clsTrx = new clsTransaction();
            }
            try
            {
                coll = new Dictionary<String, Object>();
                coll.Add("descr", pIndicatore.Descr);
                coll.Add("note_dip", pIndicatore.NoteDip);
                coll.Add("note_val", pIndicatore.NoteVal);
                coll.Add("dip_flg", pIndicatore.DipFlg);
                coll.Add("rsp_flg", pIndicatore.RspFlg);
                coll.Add("amm_flg", pIndicatore.AmmFlg);
                coll.Add("ord", pIndicatore.Ord);
                coll.Add("usr_mod", pUtente);
                coll.Add("data_mod", System.DateTime.Today.Date);
                coll_w = new Dictionary<String, Object>();
                coll_w.Add("selezione_id", pIndicatore.SelezioneId);
                coll_w.Add("categoria_cod", pIndicatore.CategoriaCod);
                coll_w.Add("ind_id", pIndicatore.IndId);
                clsTrx.update_row_trx("val_indicatori", coll, coll_w);
            }
            catch (Exception ex)
            {
                if (p_clsTrx == null)
                {
                    clsTrx.rollback_trx();
                    clsTrx = null;
                }
                throw ex;
            }
            if (p_clsTrx == null)
            {
                clsTrx.commit_trx();
                clsTrx = null;
            }
        }
        public void del_indicatore(Indicatore pIndicatore, clsTransaction p_clsTrx, string pUtente)
        {
            Dictionary<String, Object> coll;
            clsTransaction clsTrx = p_clsTrx;
            if (clsTrx == null)
            {
                clsTrx = new clsTransaction();
            }
            try
            {
                try
                {
                    foreach (IndicatoreDet oIndicatoreDet in LeggiIndicatoriDet(pIndicatore.SelezioneId, pIndicatore.CategoriaCod, pIndicatore.IndId, null))
                    {
                        del_indicatore_det(oIndicatoreDet, clsTrx, pUtente);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                coll = new Dictionary<String, Object>();
                coll.Add("selezione_id", pIndicatore.SelezioneId);
                coll.Add("categoria_cod", pIndicatore.CategoriaCod);
                coll.Add("ind_id", pIndicatore.IndId);
                clsTrx.del_row_trx("val_indicatori", coll);
            }
            catch (Exception ex)
            {
                if (p_clsTrx == null)
                {
                    clsTrx.rollback_trx();
                    clsTrx = null;
                }
                throw ex;
            }
            if (p_clsTrx == null)
            {
                clsTrx.commit_trx();
                clsTrx = null;
            }
        }
        public void dupl_indicatori(int pSelIdOld, int pSelIdNew, Int32 pIndicatore, int pAnno, string pCatCod, clsTransaction p_clsTrx, string pUtente)
        {
            Dictionary<String, Object> coll = new Dictionary<string, object>();
            clsTransaction clsTrx = p_clsTrx;
            if (clsTrx == null)
            {
                clsTrx = new clsTransaction();
            }
            else
            {

            }
            try
            {
                try
                {
                    IList<Indicatore> lIndicatore = new List<Indicatore>();
                    Indicatore NewIndicatore = new Indicatore();
                    IList<IndicatoreDet> lIndicatoreDet = new List<IndicatoreDet>();
                    IList<IndicatoreDetRiga> lIndicatoreDetRiga = new List<IndicatoreDetRiga>();
                    IList<IndicatoreDetPunt> lIndicatoreDetPunt = new List<IndicatoreDetPunt>();

                    lIndicatore = LeggiIndicatori(pSelIdOld, null, pIndicatore);
                    int IndId = rec_indid(pSelIdOld, pCatCod, clsTrx);
                    foreach (Indicatore OldIndicatore in lIndicatore)
                    {
                        clsTransaction tclsTrx = new clsTransaction();
                        try
                        {
                            coll.Add("selezione_id", pSelIdNew);
                            coll.Add("CATEGORIA_COD", pCatCod);
                            if (pSelIdOld != pSelIdNew)
                                coll.Add("IND_ID", int.Parse(OldIndicatore.IndId.ToString()));
                            else
                                coll.Add("IND_ID", IndId);
                            coll.Add("DESCR", OldIndicatore.Descr.ToString());
                            coll.Add("NOTE_DIP", OldIndicatore.NoteDip.ToString());
                            coll.Add("NOTE_VAL", OldIndicatore.NoteVal.ToString());
                            coll.Add("DIP_FLG", int.Parse(OldIndicatore.DipFlg.ToString()));
                            coll.Add("RSP_FLG", int.Parse(OldIndicatore.RspFlg.ToString()));
                            coll.Add("AMM_FLG", int.Parse(OldIndicatore.AmmFlg.ToString()));
                            coll.Add("ORD", int.Parse(OldIndicatore.Ord.ToString()));
                            coll.Add("USR_INS", pUtente.ToString());
                            coll.Add("data_ins", System.DateTime.Today.Date);
                            clsTrx.insert_row_trx("val_indicatori", coll);

                            lIndicatoreDet = LeggiIndicatoriDet(pSelIdOld, null, int.Parse(OldIndicatore.IndId.ToString()), null);
                            try
                            {
                                foreach (IndicatoreDet oIndicatoreDet in lIndicatoreDet)
                                {
                                    // insert indicatore det 
                                    coll.Clear();
                                    coll.Add("selezione_id", pSelIdNew);
                                    coll.Add("CATEGORIA_COD", pCatCod);
                                    if (pSelIdOld != pSelIdNew)
                                        coll.Add("IND_ID", int.Parse(OldIndicatore.IndId.ToString()));
                                    else
                                        coll.Add("IND_ID", IndId);
                                    coll.Add("IND_DET_ID", int.Parse(oIndicatoreDet.IndDetId.ToString()));
                                    coll.Add("DESCR", oIndicatoreDet.DescrDet.ToString());
                                    coll.Add("NOTE_DIP", oIndicatoreDet.NoteDetDip.ToString());
                                    coll.Add("NOTE_VAL", oIndicatoreDet.NoteDetVal.ToString());
                                    coll.Add("ORD", int.Parse(oIndicatoreDet.Ord.ToString()));
                                    coll.Add("MAX_RIGHE", int.Parse(oIndicatoreDet.MaxRighe.ToString()));
                                    coll.Add("USR_INS", pUtente.ToString());
                                    coll.Add("data_ins", System.DateTime.Today.Date);
                                    clsTrx.insert_row_trx("val_indicatori_det", coll);

                                    lIndicatoreDetRiga = LeggiIndicatoriDetRiga(pSelIdOld, null, int.Parse(oIndicatoreDet.IndId.ToString()), int.Parse(oIndicatoreDet.IndDetId.ToString()), null, null);
                                    try
                                    {
                                        foreach (IndicatoreDetRiga oIndicatoreDetRiga in lIndicatoreDetRiga)
                                        {
                                            // insert indicatore detriga
                                            coll.Clear();
                                            coll.Add("selezione_id", pSelIdNew);
                                            coll.Add("CATEGORIA_COD", pCatCod);
                                            if (pSelIdOld != pSelIdNew)
                                                coll.Add("IND_ID", int.Parse(OldIndicatore.IndId.ToString()));
                                            else
                                                coll.Add("IND_ID", IndId);
                                            coll.Add("IND_DET_ID", int.Parse(oIndicatoreDetRiga.IndDetId.ToString()));
                                            coll.Add("TIPO_RIGA", int.Parse(oIndicatoreDetRiga.TipoRiga.ToString()));
                                            coll.Add("RIGA_ID", int.Parse(oIndicatoreDetRiga.RigaId.ToString()));
                                            if (oIndicatoreDetRiga.OrdRiga.ToString() != "")
                                            {
                                                coll.Add("ORD", int.Parse(oIndicatoreDetRiga.OrdRiga.ToString()));
                                            }
                                            coll.Add("TIPO_CTRL", int.Parse(oIndicatoreDetRiga.TipoCtrl.ToString()));
                                            coll.Add("DESCR", oIndicatoreDetRiga.DescrRiga.ToString());
                                            coll.Add("USR_INS", pUtente.ToString());
                                            coll.Add("data_ins", System.DateTime.Today.Date);
                                            clsTrx.insert_row_trx("val_indicatori_det_riga", coll);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        throw ex;
                                    }

                                    lIndicatoreDetPunt = LeggiIndicatoriDetPunt(pSelIdOld, null, int.Parse(oIndicatoreDet.IndId.ToString()), int.Parse(oIndicatoreDet.IndDetId.ToString()), null);
                                    try
                                    {
                                        foreach (IndicatoreDetPunt oIndicatoreDetPunt in lIndicatoreDetPunt)
                                        {
                                            // insert indicatore detrigapunti
                                            coll.Clear();
                                            coll.Add("selezione_id", pSelIdNew);
                                            coll.Add("CATEGORIA_COD", pCatCod);
                                            if (pSelIdOld != pSelIdNew)
                                                coll.Add("IND_ID", int.Parse(OldIndicatore.IndId.ToString()));
                                            else
                                                coll.Add("IND_ID", IndId);
                                            coll.Add("IND_DET_ID", int.Parse(oIndicatoreDetPunt.IndDetId.ToString()));
                                            coll.Add("PUNT_ID", int.Parse(oIndicatoreDetPunt.PuntId.ToString()));
                                            coll.Add("DESCR", oIndicatoreDetPunt.DescrPunt.ToString());
                                            coll.Add("PUNT", float.Parse(oIndicatoreDetPunt.Punt.ToString()));
                                            coll.Add("USR_INS", pUtente.ToString());
                                            coll.Add("data_ins", System.DateTime.Today.Date);
                                            clsTrx.insert_row_trx("val_indicatori_det_punteggi", coll);
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        throw ex;
                                    }

                                }
                            }

                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }


                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                if (p_clsTrx == null)
                {
                    clsTrx.rollback_trx();
                    clsTrx = null;
                }
                throw ex;
            }
            if (p_clsTrx == null)
            {
                clsTrx.commit_trx();
                clsTrx = null;
            }
        }

        private int rec_indid(int pSelId, string pCatcod, clsTransaction p_clsTrx)
        {
            String sql = "select nvl(max(ind_id),0) + 1 from val_indicatori" +
                         " where selezione_id = :selezione_id " +
                         " and categoria_cod = :categoria_cod";
            Dictionary<String, Object> coll = new Dictionary<String, Object>();
            coll.Add("selezione_id", pSelId);
            coll.Add("categoria_cod", pCatcod);
            if (p_clsTrx == null)
            {
                return int.Parse(accDB.sel_dati(sql, coll).Rows[0][0].ToString());
            }
            else
            {
                return int.Parse(p_clsTrx.sel_dati(sql, coll).Rows[0][0].ToString());
            }
        }

        //IndicatoreDet
        public IList<IndicatoreDet> LeggiIndicatoriDet(int pSelId, string sCatcod, int? pIndId, int? pIndDetId)
        {
            Dictionary<String, Object> coll;
            coll = new Dictionary<string, object>();
            String sql = "select SELEZIONE_ID, CATEGORIA_COD, IND_ID, " +
                         " IND_DET_ID, DESCR, NOTE_DIP, NOTE_VAL, ORD, " +
                         " MAX_RIGHE, USR_INS, DATA_INS, USR_MOD, DATA_MOD" +
                         " from val_indicatori_det" +
                         " where selezione_id = :pSelId ";
            coll.Add("selezione_id", pSelId);
            if (sCatcod != null)
            {
                sql += " and categoria_cod = :sCatcod";
                coll.Add("categoria_cod", sCatcod);
            }

            if (pIndId != null)
            {
                sql += " and ind_id = :pIndId";
                coll.Add("ind_id", pIndId);
            }
            if (pIndDetId != null)
            {
                sql += " and ind_det_id = :pIndDetId";
                coll.Add("ind_det_id", pIndDetId);
            }
            IList<IndicatoreDet> lIndicatoriDet = new List<IndicatoreDet>();
            IndicatoreDet oIndicatoreDet;
            DataTable dt;
            dt = accDB.sel_dati(sql, coll);
            foreach (DataRow riga in dt.Rows)
            {
                oIndicatoreDet = new IndicatoreDet();
                oIndicatoreDet.SelezioneId = Int32.Parse(riga["selezione_id"].ToString());
                oIndicatoreDet.CategoriaCod = riga["categoria_cod"].ToString();
                oIndicatoreDet.IndId = Int32.Parse(riga["ind_id"].ToString());
                oIndicatoreDet.IndDetId = Int32.Parse(riga["ind_det_id"].ToString());
                //oIndicatoreDet.DescrDet = riga["descr"].ToString();
                oIndicatoreDet.DescrDet = HttpUtility.HtmlDecode(riga["descr"].ToString());//>>ga22072013<<
                oIndicatoreDet.NoteDetDip = riga["NOTE_DIP"].ToString();
                oIndicatoreDet.NoteDetVal = riga["NOTE_VAL"].ToString();
                if (riga["ord"].ToString() != "")
                {
                    oIndicatoreDet.Ord = Int32.Parse(riga["ord"].ToString());
                }
                else
                {
                    oIndicatoreDet.Ord = 0;
                }
                oIndicatoreDet.MaxRighe = Int32.Parse(riga["max_righe"].ToString());
                lIndicatoriDet.Add(oIndicatoreDet);

            }
            return lIndicatoriDet;
        }
        public int rec_inddetid(int pSelId, string pCatcod, int pIndId, clsTransaction p_clsTrx)
        {
            String sql = "select nvl(max(ind_det_id),0) + 1 from val_indicatori_det" +
                         " where selezione_id = :selezione_id " +
                         " and categoria_cod = :categoria_cod" +
                         " and ind_id = :ind_id";
            Dictionary<String, Object> coll = new Dictionary<String, Object>();
            coll.Add("selezione_id", pSelId);
            coll.Add("categoria_cod", pCatcod);
            coll.Add("ind_id", pIndId);
            if (p_clsTrx == null)
            {
                return int.Parse(accDB.sel_dati(sql, coll).Rows[0][0].ToString());
            }
            else
            {
                return int.Parse(p_clsTrx.sel_dati(sql, coll).Rows[0][0].ToString());
            }
        }
        public void ins_indicatore_det(IndicatoreDet pIndicatoreDet, clsTransaction p_clsTrx, string pUtente)
        {
            Dictionary<String, Object> coll;
            clsTransaction clsTrx = p_clsTrx;
            if (clsTrx == null)
            {
                clsTrx = new clsTransaction();
            }
            try
            {
                coll = new Dictionary<String, Object>();
                coll.Add("selezione_id", pIndicatoreDet.SelezioneId);
                coll.Add("categoria_cod", pIndicatoreDet.CategoriaCod);
                coll.Add("ind_id", pIndicatoreDet.IndId);
                coll.Add("ind_det_id", rec_inddetid(pIndicatoreDet.SelezioneId, pIndicatoreDet.CategoriaCod, pIndicatoreDet.IndId, clsTrx));
                coll.Add("descr", HttpUtility.HtmlEncode(pIndicatoreDet.DescrDet));//>>ga22072013<<
                coll.Add("note_dip", pIndicatoreDet.NoteDetDip);
                coll.Add("note_val", pIndicatoreDet.NoteDetVal);
                coll.Add("ord", pIndicatoreDet.Ord);
                coll.Add("max_righe", pIndicatoreDet.MaxRighe);
                coll.Add("usr_ins", pUtente);
                coll.Add("data_ins", System.DateTime.Today.Date);
                try
                {
                    clsTrx.insert_row_trx("val_indicatori_det", coll);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                if (p_clsTrx == null)
                {
                    clsTrx.rollback_trx();
                    clsTrx = null;
                }
                throw ex;
            }
            if (p_clsTrx == null)
            {
                clsTrx.commit_trx();
                clsTrx = null;
            }
        }
        public void upd_indicatore_det(IndicatoreDet pIndicatoreDet, clsTransaction p_clsTrx, string pUtente)
        {
            Dictionary<String, Object> coll;
            Dictionary<String, Object> coll_w;
            clsTransaction clsTrx = p_clsTrx;
            if (clsTrx == null)
            {
                clsTrx = new clsTransaction();
            }
            try
            {
                coll = new Dictionary<String, Object>();
                coll.Add("descr", HttpUtility.HtmlEncode(pIndicatoreDet.DescrDet)); //>>ga22072013<<
                coll.Add("note_dip", pIndicatoreDet.NoteDetDip);
                coll.Add("note_val", pIndicatoreDet.NoteDetVal);
                coll.Add("ord", pIndicatoreDet.Ord);
                coll.Add("max_righe", pIndicatoreDet.MaxRighe);
                coll.Add("usr_mod", pUtente);
                coll.Add("data_mod", System.DateTime.Today.Date);
                coll_w = new Dictionary<String, Object>();
                coll_w.Add("selezione_id", pIndicatoreDet.SelezioneId);
                coll_w.Add("categoria_cod", pIndicatoreDet.CategoriaCod);
                coll_w.Add("ind_id", pIndicatoreDet.IndId);
                coll_w.Add("ind_det_id", pIndicatoreDet.IndDetId);
                clsTrx.update_row_trx("val_indicatori_det", coll, coll_w);
            }
            catch (Exception ex)
            {
                if (p_clsTrx == null)
                {
                    clsTrx.rollback_trx();
                    clsTrx = null;
                }
                throw ex;
            }
            if (p_clsTrx == null)
            {
                clsTrx.commit_trx();
                clsTrx = null;
            }
        }
        public void del_indicatore_det(IndicatoreDet pIndicatoreDet, clsTransaction p_clsTrx, string pUtente)
        {
            Dictionary<String, Object> coll;
            clsTransaction clsTrx = p_clsTrx;
            if (clsTrx == null)
            {
                clsTrx = new clsTransaction();
            }
            try
            {
                try
                {
                    foreach (IndicatoreDetRiga oIndicatoreDetRiga in LeggiIndicatoriDetRiga(pIndicatoreDet.SelezioneId, pIndicatoreDet.CategoriaCod, pIndicatoreDet.IndId, pIndicatoreDet.IndDetId, null, null))
                    {
                        del_indicatore_det_riga(oIndicatoreDetRiga, clsTrx, pUtente);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                try
                {
                    foreach (IndicatoreDetPunt oIndicatoreDetPunt in LeggiIndicatoriDetPunt(pIndicatoreDet.SelezioneId, pIndicatoreDet.CategoriaCod, pIndicatoreDet.IndId, pIndicatoreDet.IndDetId, null))
                    {
                        del_indicatore_det_punt(oIndicatoreDetPunt, clsTrx, pUtente);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                coll = new Dictionary<String, Object>();
                coll.Add("selezione_id", pIndicatoreDet.SelezioneId);
                coll.Add("categoria_cod", pIndicatoreDet.CategoriaCod);
                coll.Add("ind_id", pIndicatoreDet.IndId);
                coll.Add("ind_det_id", pIndicatoreDet.IndDetId);
                clsTrx.del_row_trx("val_indicatori_det", coll);
            }
            catch (Exception ex)
            {
                if (p_clsTrx == null)
                {
                    clsTrx.rollback_trx();
                    clsTrx = null;
                }
                throw ex;
            }
            if (p_clsTrx == null)
            {
                clsTrx.commit_trx();
                clsTrx = null;
            }
        }
        public void dupl_indicatori_det(int pSelId, string pCatCod, int pIndId, int pIndDetId, clsTransaction p_clsTrx, string pUtente)
        {
            Dictionary<String, Object> coll = new Dictionary<string, object>();
            clsTransaction clsTrx = p_clsTrx;
            if (clsTrx == null)
            {
                clsTrx = new clsTransaction();
            }
            try
            {
                IndicatoreDet oIndicatoreDet = new IndicatoreDet();
                IList<IndicatoreDetRiga> lIndicatoreDetRiga = new List<IndicatoreDetRiga>();
                IList<IndicatoreDetPunt> lIndicatoreDetPunt = new List<IndicatoreDetPunt>();
                oIndicatoreDet = LeggiIndicatoriDet(pSelId, pCatCod, pIndId, pIndDetId)[0];
                try
                {
                    // insert indicatore det 
                    coll.Clear();
                    int IndDetId = rec_inddetid(pSelId, pCatCod, pIndId, clsTrx);
                    coll.Add("selezione_id", pSelId);
                    coll.Add("CATEGORIA_COD", pCatCod);
                    coll.Add("IND_ID", pIndId);
                    coll.Add("IND_DET_ID", IndDetId);
                    coll.Add("DESCR", oIndicatoreDet.DescrDet.ToString());
                    coll.Add("NOTE_DIP", oIndicatoreDet.NoteDetDip.ToString());
                    coll.Add("NOTE_VAL", oIndicatoreDet.NoteDetVal.ToString());
                    coll.Add("ORD", int.Parse(oIndicatoreDet.Ord.ToString()));
                    coll.Add("MAX_RIGHE", int.Parse(oIndicatoreDet.MaxRighe.ToString()));
                    coll.Add("USR_INS", pUtente.ToString());
                    coll.Add("data_ins", System.DateTime.Today.Date);
                    clsTrx.insert_row_trx("val_indicatori_det", coll);

                    lIndicatoreDetRiga = LeggiIndicatoriDetRiga(pSelId, null, pIndId, pIndDetId, null, null);
                    try
                    {
                        foreach (IndicatoreDetRiga oIndicatoreDetRiga in lIndicatoreDetRiga)
                        {
                            // insert indicatore detriga
                            coll.Clear();
                            coll.Add("selezione_id", pSelId);
                            coll.Add("CATEGORIA_COD", pCatCod);
                            coll.Add("IND_ID", pIndId);
                            coll.Add("IND_DET_ID", IndDetId);
                            coll.Add("TIPO_RIGA", int.Parse(oIndicatoreDetRiga.TipoRiga.ToString()));
                            coll.Add("RIGA_ID", int.Parse(oIndicatoreDetRiga.RigaId.ToString()));
                            if (oIndicatoreDetRiga.OrdRiga.ToString() != "")
                            {
                                coll.Add("ORD", int.Parse(oIndicatoreDetRiga.OrdRiga.ToString()));
                            }
                            coll.Add("TIPO_CTRL", int.Parse(oIndicatoreDetRiga.TipoCtrl.ToString()));
                            coll.Add("DESCR", oIndicatoreDetRiga.DescrRiga.ToString());
                            coll.Add("USR_INS", pUtente.ToString());
                            coll.Add("data_ins", System.DateTime.Today.Date);
                            clsTrx.insert_row_trx("val_indicatori_det_riga", coll);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                    lIndicatoreDetPunt = LeggiIndicatoriDetPunt(pSelId, null, pIndId, pIndDetId, null);
                    try
                    {
                        foreach (IndicatoreDetPunt oIndicatoreDetPunt in lIndicatoreDetPunt)
                        {
                            // insert indicatore detrigapunti
                            coll.Clear();
                            coll.Add("selezione_id", pSelId);
                            coll.Add("CATEGORIA_COD", pCatCod);
                            coll.Add("IND_ID", pIndId);
                            coll.Add("IND_DET_ID", IndDetId);
                            coll.Add("PUNT_ID", int.Parse(oIndicatoreDetPunt.PuntId.ToString()));
                            coll.Add("DESCR", oIndicatoreDetPunt.DescrPunt.ToString());
                            coll.Add("PUNT", float.Parse(oIndicatoreDetPunt.Punt.ToString()));
                            coll.Add("USR_INS", pUtente.ToString());
                            coll.Add("data_ins", System.DateTime.Today.Date);
                            clsTrx.insert_row_trx("val_indicatori_det_punteggi", coll);
                        }

                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                if (p_clsTrx == null)
                {
                    clsTrx.rollback_trx();
                    clsTrx = null;
                }
                throw ex;
            }
            if (p_clsTrx == null)
            {
                clsTrx.commit_trx();
                clsTrx = null;
            }
        }
        public bool ExistsNote(int pSelId, string pCategoriaCod, int pIndId, int pIndDetId)
        {
            String sql = "select count(*) from val_indicatori_det_riga" +
                              " where selezione_id = :selezione_id " +
                              " and categoria_cod = :categoria_cod " +
                              " and ind_id = :ind_id " +
                              " and ind_det_id = :ind_det_id" +
                              " and tipo_ctrl = 6";
            Dictionary<String, Object> coll = new Dictionary<String, Object>();
            coll.Add("selezione_id", pSelId);
            coll.Add("categoria_cod", pCategoriaCod);
            coll.Add("ind_id", pIndId);
            coll.Add("ind_det_id", pIndDetId);
            return (int.Parse(accDB.sel_dati(sql, coll).Rows[0][0].ToString()) != 0);
        }
        public bool ExistsTipoRiga(int pSelId, string pCategoriaCod, int pIndId, int pIndDetId, int pTipoRiga)
        {
            String sql = "select count(*) from val_indicatori_det_riga" +
                              " where selezione_id = :selezione_id " +
                              " and categoria_cod = :categoria_cod " +
                              " and ind_id = :ind_id " +
                              " and ind_det_id = :ind_det_id" +
                              " and tipo_riga = :tipo_riga";
            Dictionary<String, Object> coll = new Dictionary<String, Object>();
            coll.Add("selezione_id", pSelId);
            coll.Add("categoria_cod", pCategoriaCod);
            coll.Add("ind_id", pIndId);
            coll.Add("ind_det_id", pIndDetId);
            coll.Add("tipo_riga", pTipoRiga);
            return (int.Parse(accDB.sel_dati(sql, coll).Rows[0][0].ToString()) != 0);
        }
        //IndicatoreDetRiga
        public IList<IndicatoreDetRiga> LeggiIndicatoriDetRiga(int pSelId, string sCatcod, int? pIndId, int? pIndDetId, int? pTipoRiga, int? pRigaId)
        {
            Dictionary<String, Object> coll;
            coll = new Dictionary<string, object>();
            String sql = "select SELEZIONE_ID, CATEGORIA_COD, IND_ID, " +
                         " IND_DET_ID, a.TIPO_RIGA, RIGA_ID, ORD, a.TIPO_CTRL, " +
                         " a.DESCR, USR_INS, DATA_INS, USR_MOD, DATA_MOD, " +
                         " b.DESCR DESCR_TIPO_RIGA, c.DESCR DESCR_TIPO_CTRL " +
                         " from val_indicatori_det_riga a, " +
                         " val_tipi_riga b, val_tipi_ctrl c" +
                         " where selezione_id = :pSelId " +
                         " and a.tipo_riga = b.tipo_riga " +
                         " and a.tipo_ctrl = c.tipo_ctrl ";
            coll.Add("selezione_id", pSelId);
            if (sCatcod != null)
            {
                sql += " and categoria_cod = :sCatcod";
                coll.Add("categoria_cod", sCatcod);
            }
            if (pIndId != null)
            {
                sql += " and ind_id = :pIndId";
                coll.Add("ind_id", pIndId);
            }
            if (pIndDetId != null)
            {
                sql += " and ind_det_id = :pIndDetId";
                coll.Add("ind_det_id", pIndDetId);
            }
            if (pTipoRiga != null)
            {
                sql += " and a.tipo_riga = :pTipoRiga";
                coll.Add("tipo_riga", pTipoRiga);
            }
            if (pRigaId != null)
            {
                sql += " and riga_id = :pRigaId";
                coll.Add("riga_id", pRigaId);
            }
            sql += " order by selezione_id, categoria_cod, ind_id, ind_det_id, b.descr, tipo_riga, ord, riga_id";
            IList<IndicatoreDetRiga> lIndicatoreDetRiga = new List<IndicatoreDetRiga>();
            IndicatoreDetRiga oIndicatoreDetRiga;
            DataTable dt;
            dt = accDB.sel_dati(sql, coll);
            foreach (DataRow riga in dt.Rows)
            {
                oIndicatoreDetRiga = new IndicatoreDetRiga();
                oIndicatoreDetRiga.SelezioneId = Int32.Parse(riga["selezione_id"].ToString());
                oIndicatoreDetRiga.CategoriaCod = riga["categoria_cod"].ToString();
                oIndicatoreDetRiga.IndId = Int32.Parse(riga["ind_id"].ToString());
                oIndicatoreDetRiga.IndDetId = Int32.Parse(riga["ind_det_id"].ToString());
                oIndicatoreDetRiga.TipoRiga = Int32.Parse(riga["tipo_riga"].ToString());
                oIndicatoreDetRiga.RigaId = Int32.Parse(riga["riga_id"].ToString());
                oIndicatoreDetRiga.OrdRiga = Int32.Parse(riga["ord"].ToString());

                oIndicatoreDetRiga.TipoCtrl = Int32.Parse(riga["tipo_ctrl"].ToString());
                oIndicatoreDetRiga.DescrRiga = riga["descr"].ToString();
                oIndicatoreDetRiga.DescrTipoRiga = riga["descr_tipo_riga"].ToString();
                oIndicatoreDetRiga.DescrTipoCtrl = riga["descr_tipo_ctrl"].ToString();
                lIndicatoreDetRiga.Add(oIndicatoreDetRiga);
            }
            return lIndicatoreDetRiga;
        }
        public IList<clsTipoCtrl> PopolaDDLTipoCtrl()
        {
            String sql = "select tipo_ctrl, descr, 1 ord  from val_tipi_ctrl" +
                         " union select 0 tipo_ctrl, '-- scegli un tipo --' descr, 0 ord from dual" +
                         " order by 1";
            clsTipoCtrl oTipoCtrl;
            IList<clsTipoCtrl> lTipoCtrl = new List<clsTipoCtrl>();
            DataTable dt = accDB.sel_dati(sql, null);
            foreach (DataRow riga in dt.Rows)
            {
                oTipoCtrl = new clsTipoCtrl();
                oTipoCtrl.TipoCtrl = int.Parse(riga["tipo_ctrl"].ToString());
                oTipoCtrl.DescrTipo = riga["descr"].ToString();
                lTipoCtrl.Add(oTipoCtrl);
            }
            return lTipoCtrl;
        }
        public IList<clsTipoRiga> PopolaDDLTipoRiga()
        {
            String sql = "select tipo_riga, descr, 1 ord  from val_tipi_riga" +
                         " union select 0 tipo_riga, '-- scegli un tipo --' descr, 0 ord from dual" +
                         " order by 1";
            clsTipoRiga oTipRiga;
            IList<clsTipoRiga> lTipoRiga = new List<clsTipoRiga>();
            DataTable dt = accDB.sel_dati(sql, null);
            foreach (DataRow riga in dt.Rows)
            {
                oTipRiga = new clsTipoRiga();
                oTipRiga.TipoRiga = int.Parse(riga["tipo_riga"].ToString());
                oTipRiga.DescrRiga = riga["descr"].ToString();
                lTipoRiga.Add(oTipRiga);
            }
            return lTipoRiga;
        }
        public void ins_indicatore_det_riga(IndicatoreDetRiga pIndicatoreDetRiga, clsTransaction p_clsTrx, string pUtente)
        {
            Dictionary<String, Object> coll;
            clsTransaction clsTrx = p_clsTrx;
            if (clsTrx == null)
            {
                clsTrx = new clsTransaction();
            }
            try
            {
                coll = new Dictionary<String, Object>();
                coll.Add("selezione_id", pIndicatoreDetRiga.SelezioneId);
                coll.Add("categoria_cod", pIndicatoreDetRiga.CategoriaCod);
                coll.Add("ind_id", pIndicatoreDetRiga.IndId);
                coll.Add("ind_det_id", pIndicatoreDetRiga.IndDetId);
                coll.Add("tipo_riga", pIndicatoreDetRiga.TipoRiga);
                coll.Add("riga_id", rec_inddetrigaid(pIndicatoreDetRiga.SelezioneId, pIndicatoreDetRiga.CategoriaCod, pIndicatoreDetRiga.IndId, pIndicatoreDetRiga.IndDetId, pIndicatoreDetRiga.TipoRiga, clsTrx));
                coll.Add("ord", pIndicatoreDetRiga.OrdRiga);
                coll.Add("tipo_ctrl", pIndicatoreDetRiga.TipoCtrl);
                coll.Add("descr", HttpUtility.HtmlEncode(pIndicatoreDetRiga.DescrRiga));//>>ga22072013<<
                coll.Add("usr_ins", pUtente);
                coll.Add("data_ins", System.DateTime.Today.Date);
                try
                {
                    clsTrx.insert_row_trx("val_indicatori_det_riga", coll);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                if (p_clsTrx == null)
                {
                    clsTrx.rollback_trx();
                    clsTrx = null;
                }
                throw ex;
            }
            if (p_clsTrx == null)
            {
                clsTrx.commit_trx();
                clsTrx = null;
            }
        }
        public void upd_indicatore_det_riga(IndicatoreDetRiga pIndicatoreDetRiga, clsTransaction p_clsTrx, string pUtente)
        {
            Dictionary<String, Object> coll;
            Dictionary<String, Object> coll_w;
            clsTransaction clsTrx = p_clsTrx;
            if (clsTrx == null)
            {
                clsTrx = new clsTransaction();
            }
            try
            {
                coll = new Dictionary<String, Object>();
                coll.Add("descr", HttpUtility.HtmlEncode(pIndicatoreDetRiga.DescrRiga)); //>>ga22072013<<
                coll.Add("ord", pIndicatoreDetRiga.OrdRiga);
                coll.Add("tipo_ctrl", pIndicatoreDetRiga.TipoCtrl);
                coll.Add("usr_mod", pUtente);
                coll.Add("data_mod", System.DateTime.Today.Date);
                coll_w = new Dictionary<String, Object>();
                coll_w.Add("selezione_id", pIndicatoreDetRiga.SelezioneId);
                coll_w.Add("categoria_cod", pIndicatoreDetRiga.CategoriaCod);
                coll_w.Add("ind_id", pIndicatoreDetRiga.IndId);
                coll_w.Add("ind_det_id", pIndicatoreDetRiga.IndDetId);
                coll_w.Add("tipo_riga", pIndicatoreDetRiga.TipoRiga);
                coll_w.Add("riga_id", pIndicatoreDetRiga.RigaId);
                clsTrx.update_row_trx("val_indicatori_det_riga", coll, coll_w);
            }
            catch (Exception ex)
            {
                if (p_clsTrx == null)
                {
                    clsTrx.rollback_trx();
                    clsTrx = null;
                }
                throw ex;
            }
            if (p_clsTrx == null)
            {
                clsTrx.commit_trx();
                clsTrx = null;
            }
        }
        public void del_indicatore_det_riga(IndicatoreDetRiga pIndicatoreDetRiga, clsTransaction p_clsTrx, string pUtente)
        {
            Dictionary<String, Object> coll;
            clsTransaction clsTrx = p_clsTrx;
            if (clsTrx == null)
            {
                clsTrx = new clsTransaction();
            }
            try
            {
                coll = new Dictionary<String, Object>();
                coll.Add("selezione_id", pIndicatoreDetRiga.SelezioneId);
                coll.Add("categoria_cod", pIndicatoreDetRiga.CategoriaCod);
                coll.Add("ind_id", pIndicatoreDetRiga.IndId);
                coll.Add("ind_det_id", pIndicatoreDetRiga.IndDetId);
                coll.Add("tipo_riga", pIndicatoreDetRiga.TipoRiga);
                coll.Add("riga_id", pIndicatoreDetRiga.RigaId);
                clsTrx.del_row_trx("val_indicatori_det_riga", coll);
            }
            catch (Exception ex)
            {
                if (p_clsTrx == null)
                {
                    clsTrx.rollback_trx();
                    clsTrx = null;
                }
                throw ex;
            }
            if (p_clsTrx == null)
            {
                clsTrx.commit_trx();
                clsTrx = null;
            }
        }
        public int rec_inddetrigaid(int pSelId, string pCatcod, int pIndId, int pIndDetId, int pTipoRiga, clsTransaction p_clsTrx)
        {
            String sql = "select nvl(max(riga_id),0) + 1 from val_indicatori_det_riga" +
                         " where selezione_id = :selezione_id " +
                         " and categoria_cod = :categoria_cod" +
                         " and ind_id = :ind_id" +
                         " and ind_det_id = :ind_det_id" +
                         " and tipo_riga = :tipo_riga";
            Dictionary<String, Object> coll = new Dictionary<String, Object>();
            coll.Add("selezione_id", pSelId);
            coll.Add("categoria_cod", pCatcod);
            coll.Add("ind_id", pIndId);
            coll.Add("ind_det_id", pIndDetId);
            coll.Add("tipo_riga", pTipoRiga);
            if (p_clsTrx == null)
            {
                return int.Parse(accDB.sel_dati(sql, coll).Rows[0][0].ToString());
            }
            else
            {
                return int.Parse(p_clsTrx.sel_dati(sql, coll).Rows[0][0].ToString());
            }
        }
        public int leggi_inddetrigaord(int pSelId, string pCatcod, int pIndId, int pIndDetId, int pTipoRiga, int? pRigaId, int pOrd, clsTransaction p_clsTrx)
        {
            String sql = "select count(*) from val_indicatori_det_riga" +
                         " where selezione_id = :selezione_id " +
                         " and categoria_cod = :categoria_cod" +
                         " and ind_id = :ind_id" +
                         " and ind_det_id = :ind_det_id" +
                         " and tipo_riga = :tipo_riga" +
                         " and ord = :ord";
            Dictionary<String, Object> coll = new Dictionary<String, Object>();
            coll.Add("selezione_id", pSelId);
            coll.Add("categoria_cod", pCatcod);
            coll.Add("ind_id", pIndId);
            coll.Add("ind_det_id", pIndDetId);
            coll.Add("tipo_riga", pTipoRiga);
            coll.Add("ord", pOrd);
            if (pRigaId != null)
            {
                sql += " and riga_id = :riga_id";
                coll.Add("riga_id", pRigaId);
            }
            if (p_clsTrx == null)
            {
                return int.Parse(accDB.sel_dati(sql, coll).Rows[0][0].ToString());
            }
            else
            {
                return int.Parse(p_clsTrx.sel_dati(sql, coll).Rows[0][0].ToString());
            }
        }
        public bool ExistsNoteRiga(int pSelId, string pCategoriaCod, int pIndId, int pIndDetId, int pTipoRiga, int pRigaId)
        {
            String sql = "select count(*) from val_indicatori_det_riga" +
                              " where selezione_id = :selezione_id " +
                              " and categoria_cod = :categoria_cod " +
                              " and ind_id = :ind_id " +
                              " and ind_det_id = :ind_det_id" +
                              " and tipo_riga = :tipo_riga" +
                              " and riga_id = :riga_id" +
                              " and tipo_ctrl = 6";
            Dictionary<String, Object> coll = new Dictionary<String, Object>();
            coll.Add("selezione_id", pSelId);
            coll.Add("categoria_cod", pCategoriaCod);
            coll.Add("ind_id", pIndId);
            coll.Add("ind_det_id", pIndDetId);
            coll.Add("tipo_riga", pTipoRiga);
            coll.Add("riga_id", pRigaId);
            return (int.Parse(accDB.sel_dati(sql, coll).Rows[0][0].ToString()) != 0);
        }//IndicatoreDetPunt
        public IList<IndicatoreDetPunt> LeggiIndicatoriDetPunt(int pSelId, string sCatcod, int? pIndId, int? pIndDetId, int? pPuntId)
        {
            Dictionary<String, Object> coll;
            coll = new Dictionary<string, object>();
            String sql = "select SELEZIONE_ID, CATEGORIA_COD, IND_ID, " +
                         " IND_DET_ID, PUNT_ID, DESCR, PUNT, " +
                         " USR_INS, DATA_INS, USR_MOD, DATA_MOD " +
                         " from val_indicatori_det_punteggi" +
                         " where selezione_id = :pSelId ";
            coll.Add("selezione_id", pSelId);
            if (sCatcod != null)
            {
                sql += " and categoria_cod = :sCatcod";
                coll.Add("categoria_cod", sCatcod);
            }
            if (pIndId != null)
            {
                sql += " and ind_id = :pIndId";
                coll.Add("ind_id", pIndId);
            }
            if (pIndDetId != null)
            {
                sql += " and ind_det_id = :pIndDetId";
                coll.Add("ind_det_id", pIndDetId);
            }
            if (pPuntId != null)
            {
                sql += " and punt_id = :pPuntId";
                coll.Add("punt_id", pPuntId);
            }
            sql += " order by selezione_id, categoria_cod, ind_id, ind_det_id, punt_id";
            IList<IndicatoreDetPunt> lIndicatoreDetPunt = new List<IndicatoreDetPunt>();
            IndicatoreDetPunt oIndicatoreDetPunt;
            DataTable dt;
            dt = accDB.sel_dati(sql, coll);
            foreach (DataRow riga in dt.Rows)
            {
                // indicatore dettaglio punteggi
                oIndicatoreDetPunt = new IndicatoreDetPunt();
                oIndicatoreDetPunt.SelezioneId = Int32.Parse(riga["selezione_id"].ToString());
                oIndicatoreDetPunt.CategoriaCod = riga["categoria_cod"].ToString();
                oIndicatoreDetPunt.IndId = Int32.Parse(riga["ind_id"].ToString());
                oIndicatoreDetPunt.IndDetId = Int32.Parse(riga["ind_det_id"].ToString());
                oIndicatoreDetPunt.PuntId = Int32.Parse(riga["punt_id"].ToString());
                oIndicatoreDetPunt.DescrPunt = riga["descr"].ToString();
                oIndicatoreDetPunt.Punt = float.Parse(riga["punt"].ToString());
                lIndicatoreDetPunt.Add(oIndicatoreDetPunt);
            }
            return lIndicatoreDetPunt;
        }
        public IndicatoreDetPuntMinMax leggi_inddetpuntid(int pSelId, string pCatcod, int pIndId, int pIndDetId, clsTransaction p_clsTrx)
        {
            IndicatoreDetPuntMinMax oIndicatore = new IndicatoreDetPuntMinMax();
            DataTable dt = new DataTable();
            String sql = "select ind_id, ind_det_id, max(punt) as max, min(punt) as min from val_indicatori_det_punteggi " +
                         " where selezione_id = :selezione_id " +
                         " and categoria_cod = :categoria_cod" +
                         " and ind_id = :ind_id" +
                         " and ind_det_id = :ind_det_id" +
                         " group by ind_id, ind_det_id ";
            Dictionary<String, Object> coll = new Dictionary<String, Object>();
            coll.Add("selezione_id", pSelId);
            coll.Add("categoria_cod", pCatcod);
            coll.Add("ind_id", pIndId);
            coll.Add("ind_det_id", pIndDetId);
            dt = accDB.sel_dati(sql, coll);
            foreach (DataRow riga in dt.Rows)
            {
                oIndicatore.SelezioneId = pSelId;
                oIndicatore.IndDetId = pIndId;
                oIndicatore.IndDetId = pIndDetId;
                oIndicatore.CategoriaCod = pCatcod;
                oIndicatore.PuntMax = float.Parse(riga["max"].ToString());
                oIndicatore.PuntMin = float.Parse(riga["min"].ToString());

            }

            if (p_clsTrx == null)
            {
                return oIndicatore;
            }
            else
            {
                return oIndicatore;
            }
        }
        public int rec_inddetpuntid(int pSelId, string pCatcod, int pIndId, int pIndDetId, clsTransaction p_clsTrx)
        {
            String sql = "select nvl(max(punt_id),0) + 1 from val_indicatori_det_punteggi" +
                         " where selezione_id = :selezione_id " +
                         " and categoria_cod = :categoria_cod" +
                         " and ind_id = :ind_id" +
                         " and ind_det_id = :ind_det_id";
            Dictionary<String, Object> coll = new Dictionary<String, Object>();
            coll.Add("selezione_id", pSelId);
            coll.Add("categoria_cod", pCatcod);
            coll.Add("ind_id", pIndId);
            coll.Add("ind_det_id", pIndDetId);
            if (p_clsTrx == null)
            {
                return int.Parse(accDB.sel_dati(sql, coll).Rows[0][0].ToString());
            }
            else
            {
                return int.Parse(p_clsTrx.sel_dati(sql, coll).Rows[0][0].ToString());
            }
        }
        public void ins_indicatore_det_punt(IndicatoreDetPunt pIndicatoreDetPunt, clsTransaction p_clsTrx, string pUtente)
        {
            Dictionary<String, Object> coll;
            clsTransaction clsTrx = p_clsTrx;
            if (clsTrx == null)
            {
                clsTrx = new clsTransaction();
            }
            try
            {
                coll = new Dictionary<String, Object>();
                coll.Add("selezione_id", pIndicatoreDetPunt.SelezioneId);
                coll.Add("categoria_cod", pIndicatoreDetPunt.CategoriaCod);
                coll.Add("ind_id", pIndicatoreDetPunt.IndId);
                coll.Add("ind_det_id", pIndicatoreDetPunt.IndDetId);
                coll.Add("punt_id", rec_inddetpuntid(pIndicatoreDetPunt.SelezioneId, pIndicatoreDetPunt.CategoriaCod, pIndicatoreDetPunt.IndId, pIndicatoreDetPunt.IndDetId, clsTrx));
                coll.Add("punt", pIndicatoreDetPunt.Punt);
                coll.Add("descr", pIndicatoreDetPunt.DescrPunt);
                coll.Add("usr_ins", pUtente);
                coll.Add("data_ins", System.DateTime.Today.Date);
                try
                {
                    clsTrx.insert_row_trx("val_indicatori_det_punteggi", coll);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                if (p_clsTrx == null)
                {
                    clsTrx.rollback_trx();
                    clsTrx = null;
                }
                throw ex;
            }
            if (p_clsTrx == null)
            {
                clsTrx.commit_trx();
                clsTrx = null;
            }
        }
        public void upd_indicatore_det_punt(IndicatoreDetPunt pIndicatoreDetPunt, clsTransaction p_clsTrx, string pUtente)
        {
            Dictionary<String, Object> coll;
            Dictionary<String, Object> coll_w;
            clsTransaction clsTrx = p_clsTrx;
            if (clsTrx == null)
            {
                clsTrx = new clsTransaction();
            }
            try
            {
                coll = new Dictionary<String, Object>();
                coll.Add("descr", pIndicatoreDetPunt.DescrPunt);
                coll.Add("punt", pIndicatoreDetPunt.Punt);
                coll.Add("usr_mod", pUtente);
                coll.Add("data_mod", System.DateTime.Today.Date);
                coll_w = new Dictionary<String, Object>();
                coll_w.Add("selezione_id", pIndicatoreDetPunt.SelezioneId);
                coll_w.Add("categoria_cod", pIndicatoreDetPunt.CategoriaCod);
                coll_w.Add("ind_id", pIndicatoreDetPunt.IndId);
                coll_w.Add("ind_det_id", pIndicatoreDetPunt.IndDetId);
                coll_w.Add("punt_id", pIndicatoreDetPunt.PuntId);
                clsTrx.update_row_trx("val_indicatori_det_punteggi", coll, coll_w);
            }
            catch (Exception ex)
            {
                if (p_clsTrx == null)
                {
                    clsTrx.rollback_trx();
                    clsTrx = null;
                }
                throw ex;
            }
            if (p_clsTrx == null)
            {
                clsTrx.commit_trx();
                clsTrx = null;
            }
        }
        public void del_indicatore_det_punt(IndicatoreDetPunt pIndicatoreDetPunt, clsTransaction p_clsTrx, string pUtente)
        {
            Dictionary<String, Object> coll;
            clsTransaction clsTrx = p_clsTrx;
            if (clsTrx == null)
            {
                clsTrx = new clsTransaction();
            }
            try
            {
                coll = new Dictionary<String, Object>();
                coll.Add("selezione_id", pIndicatoreDetPunt.SelezioneId);
                coll.Add("categoria_cod", pIndicatoreDetPunt.CategoriaCod);
                coll.Add("ind_id", pIndicatoreDetPunt.IndId);
                coll.Add("ind_det_id", pIndicatoreDetPunt.IndDetId);
                coll.Add("punt_id", pIndicatoreDetPunt.PuntId);
                clsTrx.del_row_trx("val_indicatori_det_punteggi", coll);
            }
            catch (Exception ex)
            {
                if (p_clsTrx == null)
                {
                    clsTrx.rollback_trx();
                    clsTrx = null;
                }
                throw ex;
            }
            if (p_clsTrx == null)
            {
                clsTrx.commit_trx();
                clsTrx = null;
            }
        }
        //Stato
        public void ModificaStatoSelezione(int pSelId, string pOper, clsTransaction p_clsTrx, string pUtente)
        {
            Dictionary<String, Object> coll;
            Dictionary<String, Object> coll_w;
            int StatoSel;
            clsTransaction clsTrx = p_clsTrx;
            IList<Selezione> lSelezioni = new List<Selezione>();
            if (clsTrx == null)
            {
                clsTrx = new clsTransaction();
            }
            try
            {
                try
                {
                    Selezione oSelezione = LeggiSelezione(pSelId);
                    if (pOper == "Pub")
                    {
                        if (oSelezione.Stato == 0)
                        {
                            // non è possibile attivare una nuova selezione (per categoria cod) se prima non si chiude la precedente
                            lSelezioni = LeggiSelezioni("1", null, oSelezione.CategoriaCod, 0);
                            if (lSelezioni.Count() == 0) // non deve esistere una selezione attiva rivolta alla stessa categoria_cod
                                StatoSel = 1;
                            else
                            {
                                throw new CustomExceptions.SelezioniAncoraAttive(lSelezioni[0].SelezioneId, lSelezioni[0].CategoriaCod, lSelezioni[0].Anno);
                            }
                        }
                        // da rivedere: occorre verificare se ci sono già delle compilazioni
                        else if (oSelezione.Stato == 1 && !ExistsCompilazioni(pSelId, null, null, null, null, null))
                        {
                            StatoSel = 0;
                        }
                        else
                        {
                            throw new CustomExceptions.SelezioneConValutazioni();
                        }
                    }
                    else if (pOper == "Chd")  //>>ga24072013<<
                    {
                        if (oSelezione.Stato == 1)
                        {
                            //StatoSel = 9; secondo me non è corretto se chiudo la selezione passo in stato 8
                            StatoSel = 8;
                        }
                        //else if (oSelezione.Stato == 9)
                        else if (oSelezione.Stato == 8)
                        {
                            StatoSel = 1;
                        }
                        else
                        {
                            throw new CustomExceptions.SelezioneStatoErrato();
                        }
                    }
                    else if (pOper == "Arc")
                    {
                        if (oSelezione.Stato == 8)
                        {
                            StatoSel = 99;
                        }
                        else
                        {
                            throw new CustomExceptions.SelezioneStatoErrato();
                        }
                    }
                    else
                        throw new CustomExceptions.SelezioneStatoErrato();

                    coll = new Dictionary<string, object>();
                    coll.Add("stato", StatoSel);
                    if (pUtente != "")
                    {
                        coll.Add("usr_mod", pUtente);
                    }
                    coll.Add("data_mod", System.DateTime.Today.Date);
                    coll_w = new Dictionary<string, object>();
                    coll_w.Add("selezione_id", pSelId);
                    try
                    {
                        clsTrx.update_row_trx("val_selezioni", coll, coll_w);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                catch (CustomExceptions.SelezioniAncoraAttive)
                {
                    throw new CustomExceptions.SelezioniAncoraAttive(lSelezioni[0].SelezioneId, lSelezioni[0].CategoriaCod, lSelezioni[0].Anno);
                }
                catch (CustomExceptions.SelezioneConValutazioni)
                {
                    throw new CustomExceptions.SelezioneConValutazioni();
                }
                catch (CustomExceptions.SelezioneStatoErrato)
                {
                    throw new CustomExceptions.SelezioneStatoErrato();
                }
                catch (Exception)
                {
                    CustomExceptions.SelezioneConValutazioni ex = new CustomExceptions.SelezioneConValutazioni();
                    throw ex;
                }
            }
            catch (CustomExceptions.SelezioniAncoraAttive ex)
            {
                clsTrx.rollback_trx();
                clsTrx = null;
                throw new CustomExceptions.SelezioniAncoraAttive(lSelezioni[0].SelezioneId, lSelezioni[0].CategoriaCod, lSelezioni[0].Anno);
            }
            catch (CustomExceptions.SelezioneConValutazioni ex)
            {
                clsTrx.rollback_trx();
                clsTrx = null;
                throw new CustomExceptions.SelezioneConValutazioni();
            }
            catch (CustomExceptions.SelezioneStatoErrato ex)
            {
                clsTrx.rollback_trx();
                clsTrx = null;
                throw new CustomExceptions.SelezioneStatoErrato();
            }
            catch (Exception ex)
            {
                clsTrx.rollback_trx();
                clsTrx = null;
                throw ex;
            }

            clsTrx.commit_trx();
            clsTrx = null;
        }
        //Report  
        public DataTable DtAnag(int pSelId, int pCompId)
        {
            Dictionary<String, Object> coll = new Dictionary<string, object>();
            string sql = " select distinct r.matri_dip, 'Nome' Nome, 'Cognome' Cognome, 'Afferenza' Afferenza, d.descr stato from val_risposte r, val_stati d" +
                        " where r.selezione_id = :selezione_id " +
                        " and r.comp_id = :comp_id " +
                        " and r.stato = d.stato";
            coll.Add("selezione_id", pSelId);
            coll.Add("comp_id", pCompId);

            DataTable dt = new DataTable();
            dt = accDB.sel_dati(sql, coll);
            foreach (DataRow riga in dt.Rows)
            {
                clsUtenteLogin oUtenteDip = LeggiUtenteLogin(riga["matri_dip"].ToString());
                riga["Nome"] = oUtenteDip.Nome;
                riga["Cognome"] = oUtenteDip.Cognome;
                riga["Afferenza"] = oUtenteDip.Afferenza;
            }
            return dt;
        }
        public DataTable DtAnag(int pSelId, string pUtente)
        {
            DataTable dt = new DataTable();
            DataColumn dc;
            clsUtenteLogin oUtenteDip = LeggiUtenteLogin(pUtente);
            if (oUtenteDip != null)
            {
                dc = new DataColumn("matri_dip", typeof(System.String));
                dt.Columns.Add(dc);
                dc = new DataColumn("Nome", typeof(System.String));
                dt.Columns.Add(dc);
                dc = new DataColumn("Cognome", typeof(System.String));
                dt.Columns.Add(dc);
                dc = new DataColumn("Afferenza", typeof(System.String));
                dt.Columns.Add(dc);
                dc = new DataColumn("Stato", typeof(System.String));
                dt.Columns.Add(dc);
            }
            DataRow dr = dt.NewRow();
            dr["matri_dip"] = oUtenteDip.UserId;
            dr["Nome"] = oUtenteDip.Nome;
            dr["Cognome"] = oUtenteDip.Cognome;
            dr["Afferenza"] = oUtenteDip.Afferenza;
            dr["Stato"] = "Senza compilazione";
            dt.Rows.Add(dr);
            return dt;
        }
        public DataTable DtStruttura(int pSelId, string pTipo)
        {
            Dictionary<String, Object> coll;
            coll = new Dictionary<string, object>();
            string sql = "SELECT i.selezione_id, i.CATEGORIA_COD, i.ind_id, id.ind_det_id, id.max_righe " +
                          " FROM val_indicatori i, val_indicatori_det id " +
                          " WHERE     i.selezione_id = :pSelId " +
                          " AND i.selezione_id = id.selezione_id " +
                          " AND i.categoria_cod = id.categoria_cod " +
                          " AND i.ind_id = id.ind_id ";
            if (pTipo == "Dom")
            {
                sql += " AND i.dip_flg = 1 ";
            }
            else
            {
                sql += " AND (i.rsp_flg = 1 OR i.amm_flg = 1) ";
            }
            sql += " ORDER BY i.ord, id.ord";
            coll.Add("selezione_id", pSelId);
            DataTable dt;
            dt = accDB.sel_dati(sql, coll);
            return dt;
        }
        public DataTable DtRiepilogo(int pSelId, string pTipo, int pCompId)
        {
            Dictionary<String, Object> coll;
            coll = new Dictionary<string, object>();
            string sql = "";
            if (pTipo == "Dom")
            {
                sql = "SELECT i.selezione_id, i.CATEGORIA_COD, i.ind_id, id.ind_det_id, idp.punt_id, " +
                      " i.DESCR descr_ind, id.DESCR descr_ind_det, " +
                      " IDP.DESCR descr_ind_det_punt, idp.punt " +
                      " FROM val_indicatori i, val_indicatori_det id, val_indicatori_det_punteggi idp  " +
                      " WHERE     idp.selezione_id = :pSelId " +
                      " AND i.selezione_id = id.selezione_id " +
                      " AND i.categoria_cod = id.categoria_cod " +
                      " AND i.ind_id = id.ind_id " +
                      " AND i.selezione_id = idp.selezione_id " +
                      " AND i.categoria_cod = idp.categoria_cod " +
                      " AND i.ind_id = idp.ind_id " +
                      " AND id.ind_det_id = idp.ind_det_id " +
                      " AND i.dip_flg = 1 " +
                      " ORDER BY i.ord, id.ord, idp.punt_id";
                coll.Add("selezione_id", pSelId);
            }
            else
            {
                coll.Add("selezione_id", pSelId);
                if (pCompId != 0)
                {
                    sql = "SELECT i.selezione_id, i.CATEGORIA_COD, i.ind_id, id.ind_det_id, idp.punt_id, " +
                          " i.DESCR descr_ind, id.DESCR descr_ind_det, " +
                          " IDP.DESCR descr_ind_det_punt, idp.punt, v.punteggio " +
                          " FROM val_indicatori i, val_indicatori_det id, " +
                          " val_indicatori_det_punteggi idp, val_valutazioni v " +
                          " WHERE     idp.selezione_id = :pSelId " +
                          " AND i.selezione_id = id.selezione_id " +
                          " AND i.categoria_cod = id.categoria_cod " +
                          " AND i.ind_id = id.ind_id " +
                          " AND i.selezione_id = idp.selezione_id " +
                          " AND i.categoria_cod = idp.categoria_cod " +
                          " AND i.ind_id = idp.ind_id " +
                          " AND id.ind_det_id = idp.ind_det_id " +
                          " AND idp.selezione_id = v.selezione_id (+) " +
                          " AND idp.categoria_cod = v.categoria_cod (+) " +
                          " AND idp.ind_id = v.ind_id (+) " +
                          " AND idp.ind_det_id = v.ind_det_id (+) " +
                          " AND (i.rsp_flg = 1 OR i.amm_flg = 1) ";
                    sql += " AND v.comp_id (+) = :pCompId ";
                    coll.Add("comp_id", pCompId);
                }
                else
                {
                    sql = "SELECT i.selezione_id, i.CATEGORIA_COD, i.ind_id, id.ind_det_id, idp.punt_id, " +
                                          " i.DESCR descr_ind, id.DESCR descr_ind_det, " +
                                          " IDP.DESCR descr_ind_det_punt, idp.punt, 0 punteggio " +
                                          " FROM val_indicatori i, val_indicatori_det id, " +
                                          " val_indicatori_det_punteggi idp " +
                                          " WHERE     idp.selezione_id = :pSelId " +
                                          " AND i.selezione_id = id.selezione_id " +
                                          " AND i.categoria_cod = id.categoria_cod " +
                                          " AND i.ind_id = id.ind_id " +
                                          " AND i.selezione_id = idp.selezione_id " +
                                          " AND i.categoria_cod = idp.categoria_cod " +
                                          " AND i.ind_id = idp.ind_id " +
                                          " AND id.ind_det_id = idp.ind_det_id " +
                                          " AND (i.rsp_flg = 1 OR i.amm_flg = 1) ";
                }
                sql += " ORDER BY i.ord, id.ord, idp.punt_id";
            }
            DataTable dt;
            dt = accDB.sel_dati(sql, coll);
            return dt;
        }
        public DataTable DtScheda(int pSelId, string pTipo, int pCompId)
        {
            Dictionary<String, Object> coll;
            DataTable dt = new DataTable();
            DataTable dts = DtStruttura(pSelId, pTipo);
            foreach (DataRow item in dts.Rows)
            {
                DataTable dtr;
                coll = new Dictionary<string, object>();
                string sql = "";
                if (pTipo == "Dom")
                {
                    /*sql = "SELECT i.SELEZIONE_ID, i.CATEGORIA_COD, i.IND_ID, id.IND_DET_ID, idr.TIPO_RIGA, idr.RIGA_ID, " +
                                     " id.max_righe, nvl(idr.ORD,0) ord, cast( nvl(idr.TIPO_CTRL, 0) as INT) tipo_ctrl, I.DESCR descr_ind, id.descr descr_ind_det, idr.DESCR descr_ind_det_riga, " +
                                 " case when (i.dip_flg = 1) then i.note_dip else i.note_val end note_ind, " +
                                 " case when (i.dip_flg = 1) then id.note_dip else id.note_val end note_ind_det, " +
                                 " 0 num_det, 0 risp_id, null risp, 0 risposta_flg" +
                                 " FROM val_indicatori i, val_indicatori_det id, val_indicatori_det_riga idr " +
                                 " WHERE     i.selezione_id = :selezione_id " +
                                 " AND i.ind_id = :ind_id " +
                                 " AND id.ind_det_id = :ind_det_id " +
                                 " AND i.selezione_id = id.selezione_id " +
                                 " AND i.categoria_cod = id.categoria_cod " +
                                 " AND i.ind_id = id.ind_id " +
                                 " AND id.selezione_id = idr.selezione_id (+) " +
                                 " AND id.categoria_cod = IDR.CATEGORIA_COD (+) " +
                                 " AND id.ind_id = IDR.IND_ID (+) " +
                                 " AND id.ind_det_id = idr.ind_det_id (+) " +
                                 " AND i.dip_flg = 1 AND idr.tipo_riga (+) = 1"; *///>>ga092016<< da commentare ritorna decimal con l'istruzione nvl
                   //>>ga092016<< inizio
                    sql = "SELECT i.SELEZIONE_ID, i.CATEGORIA_COD, i.IND_ID, id.IND_DET_ID, idr.TIPO_RIGA, idr.RIGA_ID, " +
                                    " id.max_righe, cast(idr.ORD as varchar(10)) ord, nvl(idr.TIPO_CTRL, 0) tipo_ctrl, I.DESCR descr_ind, id.descr descr_ind_det, idr.DESCR descr_ind_det_riga, " +
                                " case when (i.dip_flg = 1) then i.note_dip else i.note_val end note_ind, " +
                                " case when (i.dip_flg = 1) then id.note_dip else id.note_val end note_ind_det, " +
                                " 0 num_det, 0 risp_id, null risp, 0 risposta_flg" +
                                " FROM val_indicatori i, val_indicatori_det id, val_indicatori_det_riga idr " +
                                " WHERE     i.selezione_id = :selezione_id " +
                                " AND i.ind_id = :ind_id " +
                                " AND id.ind_det_id = :ind_det_id " +
                                " AND i.selezione_id = id.selezione_id " +
                                " AND i.categoria_cod = id.categoria_cod " +
                                " AND i.ind_id = id.ind_id " +
                                " AND id.selezione_id = idr.selezione_id (+) " +
                                " AND id.categoria_cod = IDR.CATEGORIA_COD (+) " +
                                " AND id.ind_id = IDR.IND_ID (+) " +
                                " AND id.ind_det_id = idr.ind_det_id (+) " +
                                " AND i.dip_flg = 1 AND idr.tipo_riga (+) = 1";
                    // >>ga092016<< fine nuova query senza nvl
                    
                    coll.Add("selezione_id", pSelId);
                    coll.Add("ind_id", int.Parse(item["ind_id"].ToString()));
                    coll.Add("ind_det_id", int.Parse(item["ind_det_id"].ToString()));
                    sql += " ORDER BY i.ord, id.ord, risp_id(+), idr.ord, idr.riga_id(+)";
                    dtr = accDB.sel_dati(sql, coll);

                    for (int i = 0; i < int.Parse(item["max_righe"].ToString()); i++)
                    {
                        foreach (DataRow riga in dtr.Rows)
                        {
                            //riga["ord"] = i.ToString() + riga["ord"]; //>>ga092016<< ori
                            riga["num_det"] = (i + 1).ToString(); //>>ga092016<< ori
                            riga["ord"] = i.ToString() + riga["ord"].ToString(); //>>ga092016<< new
                            //riga["num_det"] = i + int.Parse(riga["ord"].ToString()); //>>ga092016<< new
                            if (pCompId != 0)
                            {

                                CompRisposte oCompRisposte = new CompRisposte();
                                oCompRisposte = LeggiRisposte(pCompId, pSelId, item["categoria_cod"].ToString(), int.Parse(item["ind_id"].ToString()), int.Parse(item["ind_det_id"].ToString()), i + 1, int.Parse(riga["tipo_riga"].ToString()), int.Parse(riga["riga_id"].ToString()), null);
                                riga["risp_id"] = (i + 1).ToString();
                                if (oCompRisposte.RigheRisposta.Count > 0)
                                {
                                    riga["risp"] = oCompRisposte.RigheRisposta[0].Risp;
                                    riga["risposta_flg"] = 1;
                                }
                                else
                                {
                                    riga["risp"] = null;
                                    riga["risposta_flg"] = 0;
                                }
                            }

                        }
                        dt.Merge(dtr);
                    }
                }
                else
                {
                    //sql = "SELECT i.SELEZIONE_ID, i.CATEGORIA_COD, i.IND_ID, id.IND_DET_ID, r.COMP_ID, " +
                    //      " r.TIPO_RIGA, r.RIGA_ID, r.RISP_ID, nvl(r.RISP_ID,0) ord, 0 tipo_ctrl, id.max_righe, I.DESCR descr_ind, " +
                    //      " id.descr descr_ind_det, r.RISP, " +
                    //      " (SELECT descr " +
                    //      "    FROM val_indicatori_det_riga idr " +
                    //      "   WHERE     IDR.SELEZIONE_ID = r.SELEZIONE_ID " +
                    //      "         AND idr.categoria_cod = r.categoria_cod " +
                    //      "         AND idr.ind_id = r.ind_id " +
                    //      "         AND idr.ind_det_id = r.ind_det_id " +
                    //      "         AND idr.tipo_riga = R.TIPO_RIGA " +
                    //      "         AND idr.riga_id = r.riga_id) ||  r.RISP descr_ind_det_riga, " +
                    //      " CASE WHEN (i.dip_flg = 1) THEN i.note_dip ELSE i.note_val END note_ind, " +
                    //      " CASE WHEN (i.dip_flg = 1) THEN id.note_dip ELSE id.note_val END note_ind_det, " +
                    //      " nvl(r.RISP_ID,0) num_det" +
                    //      " FROM val_indicatori i, val_indicatori_det id, val_risposte r " +
                    //      " WHERE     i.selezione_id = :selezione_id " +
                    //      "       AND i.ind_id = :ind_id " +
                    //      "       AND id.ind_det_id = :ind_det_id " +
                    //      "       AND i.selezione_id = id.selezione_id " +
                    //      "       AND i.categoria_cod = id.categoria_cod " +
                    //      "       AND i.ind_id = id.ind_id " +
                    //      "       AND id.selezione_id = r.selezione_id(+) " +
                    //      "       AND id.categoria_cod = R.CATEGORIA_COD(+) " +
                    //      "       AND id.ind_id = R.IND_ID(+) " +
                    //      "       AND id.ind_det_id = r.ind_det_id(+) " +
                    //      "       AND (i.rsp_flg = 1 OR i.amm_flg = 1) AND r.tipo_riga (+) <> 1 ";
                    //coll.Add("selezione_id", pSelId);
                    //coll.Add("ind_id", int.Parse(item["ind_id"].ToString()));
                    //coll.Add("ind_det_id", int.Parse(item["ind_det_id"].ToString()));
                    //if (pCompId != 0)
                    //{
                    //    sql += " AND r.comp_id (+) = :pCompId ";
                    //    coll.Add("comp_id", pCompId);
                    //}
                    //sql += " ORDER BY i.ord, id.ord, r.risp_id(+), r.riga_id(+)";
                    //dtr = accDB.sel_dati(sql, coll);
                    //dt.Merge(dtr);
                    // >>ga092016<< inizio
                    /*
                    sql = "SELECT i.SELEZIONE_ID, i.CATEGORIA_COD, i.IND_ID, id.IND_DET_ID, idr.TIPO_RIGA, idr.RIGA_ID, " +
                                     " id.max_righe, nvl(idr.ORD,0) ord, nvl(idr.TIPO_CTRL, 0) tipo_ctrl, I.DESCR descr_ind, id.descr descr_ind_det, idr.DESCR descr_ind_det_riga, " +
                                 " case when (i.dip_flg = 1) then i.note_dip else i.note_val end note_ind, " +
                                 " case when (i.dip_flg = 1) then id.note_dip else id.note_val end note_ind_det, " +
                        // " 0 num_det, 0 risp_id, null risp, 0 risposta_flg" + >>ga21082012<<
                                 " 0 num_det, 0 risp_id, null risp, 0 risposta_flg, 0.0 punt" + //>>ga21082012<<
                                 " FROM val_indicatori i, val_indicatori_det id, val_indicatori_det_riga idr " +
                                 " WHERE     i.selezione_id = :selezione_id " +
                                 " AND i.ind_id = :ind_id " +
                                 " AND id.ind_det_id = :ind_det_id " +
                                 " AND i.selezione_id = id.selezione_id " +
                                 " AND i.categoria_cod = id.categoria_cod " +
                                 " AND i.ind_id = id.ind_id " +
                                 " AND id.selezione_id = idr.selezione_id (+) " +
                                 " AND id.categoria_cod = IDR.CATEGORIA_COD (+) " +
                                 " AND id.ind_id = IDR.IND_ID (+) " +
                                 " AND id.ind_det_id = idr.ind_det_id (+) ";
                     * */
                    sql = "SELECT i.SELEZIONE_ID, i.CATEGORIA_COD, i.IND_ID, id.IND_DET_ID, idr.TIPO_RIGA, idr.RIGA_ID, " +
                                 " id.max_righe, cast(idr.ORD as varchar(10)) ord, nvl(idr.TIPO_CTRL, 0) tipo_ctrl, I.DESCR descr_ind, id.descr descr_ind_det, idr.DESCR descr_ind_det_riga, " +
                                 " case when (i.dip_flg = 1) then i.note_dip else i.note_val end note_ind, " +
                                 " case when (i.dip_flg = 1) then id.note_dip else id.note_val end note_ind_det, " +
                                 " 0 num_det, 0 risp_id, null risp, 0 risposta_flg, 0.0 punt" + 
                                 " FROM val_indicatori i, val_indicatori_det id, val_indicatori_det_riga idr " +
                                 " WHERE     i.selezione_id = :selezione_id " +
                                 " AND i.ind_id = :ind_id " +
                                 " AND id.ind_det_id = :ind_det_id " +
                                 " AND i.selezione_id = id.selezione_id " +
                                 " AND i.categoria_cod = id.categoria_cod " +
                                 " AND i.ind_id = id.ind_id " +
                                 " AND id.selezione_id = idr.selezione_id (+) " +
                                 " AND id.categoria_cod = IDR.CATEGORIA_COD (+) " +
                                 " AND id.ind_id = IDR.IND_ID (+) " +
                                 " AND id.ind_det_id = idr.ind_det_id (+) ";
                    // >>ga092016<< fine
                    coll.Add("selezione_id", pSelId);
                    coll.Add("ind_id", int.Parse(item["ind_id"].ToString()));
                    coll.Add("ind_det_id", int.Parse(item["ind_det_id"].ToString()));
                    sql += " ORDER BY i.ord, id.ord, risp_id(+), idr.ord, idr.riga_id(+)";
                    dtr = accDB.sel_dati(sql, coll);
                    for (int i = 0; i < int.Parse(item["max_righe"].ToString()); i++)
                    {
                        foreach (DataRow riga in dtr.Rows)
                        {
                            riga["ord"] = i.ToString() + riga["ord"];
                            riga["num_det"] = (i + 1).ToString();
                            if (pCompId != 0)
                            {
                                //>>ga21082012<< inizio
                                IList<Valutazioni> oValutazione = new List<Valutazioni>();
                                oValutazione = LeggiValutazioni(pCompId, pSelId, item["categoria_cod"].ToString(), int.Parse(item["ind_id"].ToString()), int.Parse(item["ind_det_id"].ToString()), null);
                                if (oValutazione.Count > 0)
                                    riga["punt"] = float.Parse(oValutazione[0].Punteggio.ToString());
                                //>>ga21082012<< fine
                                if (riga["riga_id"].ToString() != "")
                                {
                                    CompRisposte oCompRisposte = new CompRisposte();
                                    oCompRisposte = LeggiRisposte(pCompId, pSelId, item["categoria_cod"].ToString(), int.Parse(item["ind_id"].ToString()), int.Parse(item["ind_det_id"].ToString()), i + 1, int.Parse(riga["tipo_riga"].ToString()), int.Parse(riga["riga_id"].ToString()), null);
                                    riga["risp_id"] = (i + 1).ToString();
                                    if (oCompRisposte.RigheRisposta.Count > 0)
                                    {
                                        riga["risp"] = oCompRisposte.RigheRisposta[0].Risp;
                                        riga["risposta_flg"] = 1;
                                    }
                                    else
                                    {
                                        riga["risp"] = null;
                                        riga["risposta_flg"] = 0;
                                    }
                                }
                            }

                        }
                        dt.Merge(dtr);
                    }
                }
            }
            return dt;
        }

        //Report Archivio
        public DataTable DtAnagArchivio(int pSelId, int pCompId)
        {
            Dictionary<String, Object> coll = new Dictionary<string, object>();
            string sql = " select distinct r.matri_dip, 'Nome' Nome, 'Cognome' Cognome, 'Afferenza' Afferenza, d.descr stato from val_risposte_bck r, val_stati d" +
                        " where r.selezione_id = :selezione_id " +
                        " and r.comp_id = :comp_id " +
                        " and r.stato = d.stato";
            coll.Add("selezione_id", pSelId);
            coll.Add("comp_id", pCompId);

            DataTable dt = new DataTable();
            dt = accDB.sel_dati(sql, coll);
            foreach (DataRow riga in dt.Rows)
            {
                clsUtenteLogin oUtenteDip = LeggiUtenteLogin(riga["matri_dip"].ToString());
                riga["Nome"] = oUtenteDip.Nome;
                riga["Cognome"] = oUtenteDip.Cognome;
                riga["Afferenza"] = oUtenteDip.Afferenza;
            }
            return dt;
        }
        public DataTable DtAnagArchivio(int pSelId, string pUtente)
        {
            DataTable dt = new DataTable();
            DataColumn dc;
            clsUtenteLogin oUtenteDip = LeggiUtenteLogin(pUtente);
            if (oUtenteDip != null)
            {
                dc = new DataColumn("matri_dip", typeof(System.String));
                dt.Columns.Add(dc);
                dc = new DataColumn("Nome", typeof(System.String));
                dt.Columns.Add(dc);
                dc = new DataColumn("Cognome", typeof(System.String));
                dt.Columns.Add(dc);
                dc = new DataColumn("Afferenza", typeof(System.String));
                dt.Columns.Add(dc);
                dc = new DataColumn("Stato", typeof(System.String));
                dt.Columns.Add(dc);
            }
            DataRow dr = dt.NewRow();
            dr["matri_dip"] = oUtenteDip.UserId;
            dr["Nome"] = oUtenteDip.Nome;
            dr["Cognome"] = oUtenteDip.Cognome;
            dr["Afferenza"] = oUtenteDip.Afferenza;
            dr["Stato"] = "Senza compilazione";
            dt.Rows.Add(dr);
            return dt;
        }
        public DataTable DtStrutturaArchivio(int pSelId, string pTipo)
        {
            Dictionary<String, Object> coll;
            coll = new Dictionary<string, object>();
            string sql = "SELECT i.selezione_id, i.CATEGORIA_COD, i.ind_id, id.ind_det_id, id.max_righe " +
                          " FROM val_indicatori_bck i, val_indicatori_det_bck id " +
                          " WHERE     i.selezione_id = :pSelId " +
                          " AND i.selezione_id = id.selezione_id " +
                          " AND i.categoria_cod = id.categoria_cod " +
                          " AND i.ind_id = id.ind_id ";
            if (pTipo == "Dom")
            {
                sql += " AND i.dip_flg = 1 ";
            }
            else
            {
                sql += " AND (i.rsp_flg = 1 OR i.amm_flg = 1) ";
            }
            sql += " ORDER BY i.ord, id.ord";
            coll.Add("selezione_id", pSelId);
            DataTable dt;
            dt = accDB.sel_dati(sql, coll);
            return dt;
        }
        public DataTable DtRiepilogoArchivio(int pSelId, string pTipo, int pCompId)
        {
            Dictionary<String, Object> coll;
            coll = new Dictionary<string, object>();
            string sql = "";
            if (pTipo == "Dom")
            {
                sql = "SELECT i.selezione_id, i.CATEGORIA_COD, i.ind_id, id.ind_det_id, idp.punt_id, " +
                      " i.DESCR descr_ind, id.DESCR descr_ind_det, " +
                      " IDP.DESCR descr_ind_det_punt, idp.punt " +
                      " FROM val_indicatori_bck i, val_indicatori_det_bck id, val_indicatori_det_punteggi_bk idp  " +
                      " WHERE     idp.selezione_id = :pSelId " +
                      " AND i.selezione_id = id.selezione_id " +
                      " AND i.categoria_cod = id.categoria_cod " +
                      " AND i.ind_id = id.ind_id " +
                      " AND i.selezione_id = idp.selezione_id " +
                      " AND i.categoria_cod = idp.categoria_cod " +
                      " AND i.ind_id = idp.ind_id " +
                      " AND id.ind_det_id = idp.ind_det_id " +
                      " AND i.dip_flg = 1 " +
                      " ORDER BY i.ord, id.ord, idp.punt_id";
                coll.Add("selezione_id", pSelId);
            }
            else
            {
                coll.Add("selezione_id", pSelId);
                if (pCompId != 0)
                {
                    sql = "SELECT i.selezione_id, i.CATEGORIA_COD, i.ind_id, id.ind_det_id, idp.punt_id, " +
                          " i.DESCR descr_ind, id.DESCR descr_ind_det, " +
                          " IDP.DESCR descr_ind_det_punt, idp.punt, v.punteggio " +
                          " FROM val_indicatori_bck i, val_indicatori_det_bck id, " +
                          " val_indicatori_det_punteggi_bk idp, val_valutazioni_bck v " +
                          " WHERE     idp.selezione_id = :pSelId " +
                          " AND i.selezione_id = id.selezione_id " +
                          " AND i.categoria_cod = id.categoria_cod " +
                          " AND i.ind_id = id.ind_id " +
                          " AND i.selezione_id = idp.selezione_id " +
                          " AND i.categoria_cod = idp.categoria_cod " +
                          " AND i.ind_id = idp.ind_id " +
                          " AND id.ind_det_id = idp.ind_det_id " +
                          " AND idp.selezione_id = v.selezione_id (+) " +
                          " AND idp.categoria_cod = v.categoria_cod (+) " +
                          " AND idp.ind_id = v.ind_id (+) " +
                          " AND idp.ind_det_id = v.ind_det_id (+) " +
                          " AND (i.rsp_flg = 1 OR i.amm_flg = 1) ";
                    sql += " AND v.comp_id (+) = :pCompId ";
                    coll.Add("comp_id", pCompId);
                }
                else
                {
                    sql = "SELECT i.selezione_id, i.CATEGORIA_COD, i.ind_id, id.ind_det_id, idp.punt_id, " +
                                          " i.DESCR descr_ind, id.DESCR descr_ind_det, " +
                                          " IDP.DESCR descr_ind_det_punt, idp.punt, 0 punteggio " +
                                          " FROM val_indicatori_bck i, val_indicatori_det_bck id, " +
                                          " val_indicatori_det_punteggi_bk idp " +
                                          " WHERE     idp.selezione_id = :pSelId " +
                                          " AND i.selezione_id = id.selezione_id " +
                                          " AND i.categoria_cod = id.categoria_cod " +
                                          " AND i.ind_id = id.ind_id " +
                                          " AND i.selezione_id = idp.selezione_id " +
                                          " AND i.categoria_cod = idp.categoria_cod " +
                                          " AND i.ind_id = idp.ind_id " +
                                          " AND id.ind_det_id = idp.ind_det_id " +
                                          " AND (i.rsp_flg = 1 OR i.amm_flg = 1) ";
                }
                sql += " ORDER BY i.ord, id.ord, idp.punt_id";
            }
            DataTable dt;
            dt = accDB.sel_dati(sql, coll);
            return dt;
        }
        public DataTable DtSchedaArchivio(int pSelId, string pTipo, int pCompId)
        {
            Dictionary<String, Object> coll;
            DataTable dt = new DataTable();
            DataTable dts = DtStrutturaArchivio(pSelId, pTipo);
            foreach (DataRow item in dts.Rows)
            {
                DataTable dtr;
                coll = new Dictionary<string, object>();
                string sql = "";
                if (pTipo == "Dom")
                {
                    sql = "SELECT i.SELEZIONE_ID, i.CATEGORIA_COD, i.IND_ID, id.IND_DET_ID, idr.TIPO_RIGA, idr.RIGA_ID, " +
                                     " id.max_righe, nvl(idr.ORD,0) ord, nvl(idr.TIPO_CTRL, 0) tipo_ctrl, I.DESCR descr_ind, id.descr descr_ind_det, idr.DESCR descr_ind_det_riga, " +
                                 " case when (i.dip_flg = 1) then i.note_dip else i.note_val end note_ind, " +
                                 " case when (i.dip_flg = 1) then id.note_dip else id.note_val end note_ind_det, " +
                                 " 0 num_det, 0 risp_id, null risp, 0 risposta_flg" +
                                 " FROM val_indicatori_bck i, val_indicatori_det_bck id, val_indicatori_det_riga_bck idr " +
                                 " WHERE     i.selezione_id = :selezione_id " +
                                 " AND i.ind_id = :ind_id " +
                                 " AND id.ind_det_id = :ind_det_id " +
                                 " AND i.selezione_id = id.selezione_id " +
                                 " AND i.categoria_cod = id.categoria_cod " +
                                 " AND i.ind_id = id.ind_id " +
                                 " AND id.selezione_id = idr.selezione_id (+) " +
                                 " AND id.categoria_cod = IDR.CATEGORIA_COD (+) " +
                                 " AND id.ind_id = IDR.IND_ID (+) " +
                                 " AND id.ind_det_id = idr.ind_det_id (+) " +
                                 " AND i.dip_flg = 1 AND idr.tipo_riga (+) = 1";
                    coll.Add("selezione_id", pSelId);
                    coll.Add("ind_id", int.Parse(item["ind_id"].ToString()));
                    coll.Add("ind_det_id", int.Parse(item["ind_det_id"].ToString()));
                    sql += " ORDER BY i.ord, id.ord, risp_id(+), idr.ord, idr.riga_id(+)";
                    dtr = accDB.sel_dati(sql, coll);

                    for (int i = 0; i < int.Parse(item["max_righe"].ToString()); i++)
                    {
                        foreach (DataRow riga in dtr.Rows)
                        {
                            riga["ord"] = i.ToString() + riga["ord"];
                            riga["num_det"] = (i + 1).ToString();
                            if (pCompId != 0)
                            {

                                CompRisposte oCompRisposte = new CompRisposte();
                                oCompRisposte = LeggiRisposteArchivioComp(pCompId, pSelId, item["categoria_cod"].ToString(), int.Parse(item["ind_id"].ToString()), int.Parse(item["ind_det_id"].ToString()), i + 1, int.Parse(riga["tipo_riga"].ToString()), int.Parse(riga["riga_id"].ToString()), null);
                                riga["risp_id"] = (i + 1).ToString();
                                if (oCompRisposte.RigheRisposta.Count > 0)
                                {
                                    riga["risp"] = oCompRisposte.RigheRisposta[0].Risp;
                                    riga["risposta_flg"] = 1;
                                }
                                else
                                {
                                    riga["risp"] = null;
                                    riga["risposta_flg"] = 0;
                                }
                            }

                        }
                        dt.Merge(dtr);
                    }
                }
                else
                {
                    sql = "SELECT i.SELEZIONE_ID, i.CATEGORIA_COD, i.IND_ID, id.IND_DET_ID, idr.TIPO_RIGA, idr.RIGA_ID, " +
                                     " id.max_righe, nvl(idr.ORD,0) ord, nvl(idr.TIPO_CTRL, 0) tipo_ctrl, I.DESCR descr_ind, id.descr descr_ind_det, idr.DESCR descr_ind_det_riga, " +
                                 " case when (i.dip_flg = 1) then i.note_dip else i.note_val end note_ind, " +
                                 " case when (i.dip_flg = 1) then id.note_dip else id.note_val end note_ind_det, " +
                        // " 0 num_det, 0 risp_id, null risp, 0 risposta_flg" + >>ga21082012<<
                                 " 0 num_det, 0 risp_id, null risp, 0 risposta_flg, 0.0 punt" + //>>ga21082012<<
                                 " FROM val_indicatori_bck i, val_indicatori_det_bck id, val_indicatori_det_riga_bck idr " +
                                 " WHERE     i.selezione_id = :selezione_id " +
                                 " AND i.ind_id = :ind_id " +
                                 " AND id.ind_det_id = :ind_det_id " +
                                 " AND i.selezione_id = id.selezione_id " +
                                 " AND i.categoria_cod = id.categoria_cod " +
                                 " AND i.ind_id = id.ind_id " +
                                 " AND id.selezione_id = idr.selezione_id (+) " +
                                 " AND id.categoria_cod = IDR.CATEGORIA_COD (+) " +
                                 " AND id.ind_id = IDR.IND_ID (+) " +
                                 " AND id.ind_det_id = idr.ind_det_id (+) ";
                    coll.Add("selezione_id", pSelId);
                    coll.Add("ind_id", int.Parse(item["ind_id"].ToString()));
                    coll.Add("ind_det_id", int.Parse(item["ind_det_id"].ToString()));
                    sql += " ORDER BY i.ord, id.ord, risp_id(+), idr.ord, idr.riga_id(+)";
                    dtr = accDB.sel_dati(sql, coll);
                    for (int i = 0; i < int.Parse(item["max_righe"].ToString()); i++)
                    {
                        foreach (DataRow riga in dtr.Rows)
                        {
                            riga["ord"] = i.ToString() + riga["ord"];
                            riga["num_det"] = (i + 1).ToString();
                            if (pCompId != 0)
                            {
                                //>>ga21082012<< inizio
                                IList<Valutazioni> oValutazione = new List<Valutazioni>();
                                oValutazione = LeggiValutazioniArchivioComp(pCompId, pSelId, item["categoria_cod"].ToString(), int.Parse(item["ind_id"].ToString()), int.Parse(item["ind_det_id"].ToString()), null);
                                if (oValutazione.Count > 0)
                                    riga["punt"] = float.Parse(oValutazione[0].Punteggio.ToString());
                                //>>ga21082012<< fine
                                if (riga["riga_id"].ToString() != "")
                                {
                                    CompRisposte oCompRisposte = new CompRisposte();
                                    oCompRisposte = LeggiRisposteArchivioComp(pCompId, pSelId, item["categoria_cod"].ToString(), int.Parse(item["ind_id"].ToString()), int.Parse(item["ind_det_id"].ToString()), i + 1, int.Parse(riga["tipo_riga"].ToString()), int.Parse(riga["riga_id"].ToString()), null);
                                    riga["risp_id"] = (i + 1).ToString();
                                    if (oCompRisposte.RigheRisposta.Count > 0)
                                    {
                                        riga["risp"] = oCompRisposte.RigheRisposta[0].Risp;
                                        riga["risposta_flg"] = 1;
                                    }
                                    else
                                    {
                                        riga["risp"] = null;
                                        riga["risposta_flg"] = 0;
                                    }
                                }
                            }

                        }
                        dt.Merge(dtr);
                    }
                }
            }
            return dt;
        }

        //Valutazioni
        public clsUtenteLogin LeggiUtenteResp(string pUser)
        {
            String sql = "select matri_resp, nome_resp, cognome_resp, e_mail_resp, aff_org_resp" +
                         " from val_resp_dip_vw " +
                " where matri_resp = :pUser";
            Dictionary<String, Object> coll = new Dictionary<String, Object>();
            coll.Add("user_id", pUser);
            clsUtenteLogin oclsUtenteResp = new clsUtenteLogin();
            DataTable dt = accDB.sel_dati(sql, coll);
            try
            {
                if (dt.Rows.Count == 0)
                {
                    throw new CustomExceptions.UtenteNonAbilitato();

                }
                else
                {
                    oclsUtenteResp.UserId = dt.Rows[0]["matri_resp"].ToString();
                    oclsUtenteResp.Nome = dt.Rows[0]["nome_resp"].ToString();
                    oclsUtenteResp.Cognome = dt.Rows[0]["cognome_resp"].ToString();
                    oclsUtenteResp.Email = dt.Rows[0]["e_mail_resp"].ToString();
                    oclsUtenteResp.Afferenza = dt.Rows[0]["aff_org_resp"].ToString();
                }
            }
            catch (CustomExceptions.UtenteNonAbilitato ex)
            {
                throw new CustomExceptions.UtenteNonAbilitato();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oclsUtenteResp;
        }
        public DataTable CaricaUtenti(int pUser, string pParam)
        {
            Dictionary<string, object> coll = new Dictionary<string, object>();
            if (pParam == "val")
            {
                coll.Add("matri_resp", pUser);
            }
            String sql = " select  v.anno, v.matricola_dip, v.cognome_dip, v.nome_dip, v.inquadr_dip, aff_org_dip, " +
                         " nvl((select distinct r.stato from val_risposte r where r.matri_dip = v.matri_dip), 0) as stato" +
                         " from val_resp_dip_vw v ";
            //" from dauts.val_resp_dip_vw v ";
            if (pParam == "val")
            {
                sql = sql + " where v.matri_resp = :pUser";
            }

            return accDB.sel_dati(sql, coll);
        }
        //public DataTable CaricaUtentiAmm(int pUser, string pParam, string pTipoSelezione, int pAnno, string pAfferenza, string pCategoria, string pStato, int? pMatricola, string pCognome)
        public DataTable CaricaUtentiAmm(int pUser, string pParam, string pTipoSelezione, int pAnno, string pAfferenza, string pCategoria, string pStato, int? pMatricola, int? pMatriCognome)
        {
            // >>ga082015<< ricerco se esiste una compilazione in archivio, richiesta email 18/08 di Valentina
            Dictionary<string, object> coll = new Dictionary<string, object>();
            int pAnnoS = pAnno;
            int pAnnoDS = pAnno;
            coll.Add("anno", pAnno);
            String sql = " select  v.anno, v.matri_dip, v.cognome_dip, v.nome_dip, v.inquadr_dip, v.aff_org_dip, TRIM (SUBSTR (v.inquadr_dip, 1, 2)) categoria, " +
                //   " nvl((select distinct r.stato from val_risposte r, val_selezioni s where r.matri_dip = v.matri_dip and r.selezione_id = s.selezione_id and s.anno = v.anno), 0 ) as stato, " + //>>ga082015<<
    " nvl((select distinct r.stato from val_risposte r, val_selezioni s where r.matri_dip = v.matri_dip and r.selezione_id = s.selezione_id and s.anno = v.anno), NVL ((  SELECT 88 FROM val_risposte_bck var, val_selezioni vs WHERE var.matri_dip = v.matri_dip AND var.selezione_id = vs.selezione_id AND vs.anno = v.anno GROUP BY var.matri_dip HAVING COUNT (*) > 0),0)) as stato, " + //>>ga082015<<
                      " nvl((select distinct r.selezione_id from val_risposte r, val_selezioni s where r.matri_dip = v.matri_dip and r.selezione_id = s.selezione_id and s.anno = v.anno), 0 ) as selezione_id, " +
                         " nvl((select distinct r.comp_id from val_risposte r, val_selezioni s where r.matri_dip = v.matri_dip and r.selezione_id = s.selezione_id and s.anno = v.anno), 0 ) as comp_id, " +
                         " nvl((select distinct r.stato from val_risposte r where r.matri_dip = v.matri_dip), 0)  as statovalutazione, " +
                //           " nvl((select descr from val_stati where stato = (select distinct r.stato from val_risposte r where r.matri_dip = v.matri_dip)), 'Senza compilazione') AS descstato " + //>>ga082015<<
    " nvl((select descr from val_stati where stato = (select distinct r.stato from val_risposte r where r.matri_dip = v.matri_dip)), nvl((select 'Selezione archiviata' from val_risposte_bck ar , val_selezioni s  where ar.matri_dip = v.matri_dip and ar.selezione_id = s.selezione_id and s.anno = v.anno group by matri_dip having count(*) > 0),'senza compilazione')) AS descstato  " + //>>ga082015<<
                " from val_resp_dip_vw v " +
                         " where v.anno = :pAnno ";
            if (pAfferenza.ToString() != "0")
            {
                coll.Add("aff_org_dip", pAfferenza);
                sql = sql + " and v.aff_org_dip = :pAfferenza";
            }
            if (pCategoria.ToString() != "0")
            {
                coll.Add("TRIM (SUBSTR (v.inquadr_dip, 1, 2))", pCategoria);
                sql = sql + " and TRIM (SUBSTR (v.inquadr_dip, 1, 2)) = :pCategoria";
            }
            //if (int.Parse(pStato.ToString()) != 0)
            if (int.Parse(pStato.ToString()) != -2)
            {
                coll.Add("stato", pStato);
                sql = sql + " and nvl((select distinct r.stato from val_risposte r where r.matri_dip = v.matri_dip), 0) = :pStato";
            }
            if (int.Parse(pMatricola.ToString()) != 0)
            {
                coll.Add("matri_dip", pMatricola);
                sql = sql + " and v.matri_dip = :pMatricola";
            }
            if (int.Parse(pMatriCognome.ToString()) != 0)
            {
                coll.Add("matri_dip", pMatriCognome);
                sql = sql + " and v.matri_dip = :pMatriCognome";
            }
            if (pParam == "val")
            {
                coll.Add("matri_resp", pUser);
                sql = sql + " and v.matri_resp = :pUser";
                if (int.Parse(pStato.ToString()) == -2)
                {
                    sql = sql + " AND  NVL ((SELECT   DISTINCT r.stato FROM   val_risposte r, val_selezioni s " +
                                " WHERE r.matri_dip = v.matri_dip AND r.selezione_id = s.selezione_id AND s.anno = v.anno), " +
                                " 0) >= 20";
                }
            }
            DataTable dt = new DataTable();
            try
            {
                dt = accDB.sel_dati(sql, coll);
                foreach (DataRow riga in dt.Rows)
                {
                    if (int.Parse(riga["selezione_id"].ToString()) == 0)
                    {
                        riga["statovalutazione"] = 0;

                    }
                    else
                    {
                        StatoValutazione oStatoValutazione = AggiornaStatoValutazione(int.Parse(riga["selezione_id"].ToString()), int.Parse(riga["comp_id"].ToString()), pParam);
                        riga["statovalutazione"] = (oStatoValutazione.RisposteVis != "N" ? 1 : 0);
                        riga["descstato"] = LeggiStatoCompilazione(int.Parse(riga["selezione_id"].ToString()), riga["matri_dip"].ToString());
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable CaricaArchivio(int pUser, string pParam, string pTipoSelezione, int? pAnno)
        {
            Dictionary<string, object> coll = new Dictionary<string, object>();

            coll.Add("matri_dip", pUser);
            String sql = " select  v.anno, v.matri_dip, v.cognome_dip, v.nome_dip, v.inquadr_dip, v.aff_org_dip, TRIM (SUBSTR (v.inquadr_dip, 1, 2)) categoria, " +
                         " nvl((select distinct r.stato from val_risposte_bck r, val_selezioni_bck s where r.matri_dip = v.matri_dip and r.selezione_id = s.selezione_id and s.anno = v.anno), 0 ) as stato, " +
                         " nvl((select distinct r.selezione_id from val_risposte_bck r, val_selezioni_bck s where r.matri_dip = v.matri_dip and r.selezione_id = s.selezione_id and s.anno = v.anno), 0 ) as selezione_id, " +
                         " nvl((select distinct r.comp_id from val_risposte_bck r, val_selezioni_bck s where r.matri_dip = v.matri_dip and r.selezione_id = s.selezione_id and s.anno = v.anno), 0 ) as comp_id, " +
                         //" nvl((select distinct r.stato from val_risposte_bck r where r.matri_dip = v.matri_dip), 0)  as statovalutazione, " + //>>ga082016<<
                         " nvl((select distinct r.stato from val_risposte_bck r, val_selezioni_bck s where r.matri_dip = v.matri_dip and r.selezione_id = s.selezione_id and s.anno = v.anno), 0 ) as statovalutazione, " + //>>ga082016<<

                //" nvl((select descr from val_stati where stato = (select distinct r.stato from val_risposte_bck r where r.matri_dip = v.matri_dip)), 'Senza compilazione') AS descstato " + //>>ga082016<<
                " nvl((select descr from val_stati where stato = (select distinct r.stato from val_risposte_bck r, val_selezioni_bck s where r.matri_dip = v.matri_dip and r.selezione_id = s.selezione_id and s.anno = v.anno)), 'Senza compilazione') AS descstato " + //>>ga082016<<
                         " from val_resp_dip_vw v " +
                         " where v.matri_dip = :pUser ";

            if (int.Parse(pAnno.ToString()) != 0)
            {
                coll.Add("anno", pAnno);
                sql = sql + " and v.anno = :pAnno";
            }
            DataTable dt = new DataTable();
            try
            {
                dt = accDB.sel_dati(sql, coll);
                /*
                foreach (DataRow riga in dt.Rows)
                {
                    if (int.Parse(riga["selezione_id"].ToString()) == 0)
                        riga["statovalutazione"] = 0;
                    else
                    {
                        StatoValutazione oStatoValutazione = AggiornaStatoValutazione(int.Parse(riga["selezione_id"].ToString()), int.Parse(riga["comp_id"].ToString()), pParam);
                        riga["statovalutazione"] = (oStatoValutazione.RisposteVis != "N" ? 1 : 0);
                        riga["descstato"] = LeggiStatoCompilazione(int.Parse(riga["selezione_id"].ToString()), riga["matri_dip"].ToString());
                    }
                }
                 * */
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public clsUtenteLogin VerificaUtenteValAmm(string pUser)
        {
            string sResult = "";
            clsUtenteLogin oclsUtenteAmm = new clsUtenteLogin();
            String sql = "select matri " +
                           " from val_autorizzazioni" +
                           " where matri = :pUser" +
                           " and tipo_utente in ('03', '04') ";
            Dictionary<String, Object> coll = new Dictionary<String, Object>();
            coll.Add("matri", pUser);
            try
            {
                sResult = accDB.sel_dati(sql, coll).Rows.Count.ToString();
                if (int.Parse(sResult) > 0)
                {
                    coll.Clear();
                    coll.Add("user_id", pUser);
                    sql = "select user_id, nome, cognome, AFF_org_CDS, EMAIL_ATE " +
                        //" from DAUTS.PP_PERS_STU_TUTTI_VW" +
                          " from PP_PERS_STU_TUTTI_VW" +
                          " where user_id = :pUser";
                    //sql = "select matricola, cognome, nome, aff_org" +
                    //      " from MNT_PERS_TABLE " +
                    //      //" from DAUTS.MNT_PERS_TABLE " +
                    //      " where matricola = :pUser";
                    DataTable dt = accDB.sel_dati(sql, coll);
                    if (dt.Rows.Count == 0)
                    {
                        return null;
                    }
                    else
                    {
                        oclsUtenteAmm.UserId = dt.Rows[0]["user_id"].ToString();
                        oclsUtenteAmm.Nome = dt.Rows[0]["nome"].ToString();
                        oclsUtenteAmm.Cognome = dt.Rows[0]["cognome"].ToString();
                        oclsUtenteAmm.Afferenza = dt.Rows[0]["aff_org_cds"].ToString();
                    }
                    return oclsUtenteAmm;
                }
                else
                {
                    throw new CustomExceptions.UtenteNonAbilitato();
                }
            }
            catch (CustomExceptions.UtenteNonAbilitato ex)
            {
                throw new CustomExceptions.UtenteNonAbilitato();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oclsUtenteAmm;
        }
        public clsUtenteLogin VerificaUtenteDG(string pUser)
        {
            string sResult = "";
            DataTable dt1 = new DataTable();
            clsUtenteLogin oclsUtenteAmm = new clsUtenteLogin();
            String sql = "select matri " +
                           " from val_autorizzazioni" +
                           " where matri = :pUser" +
                           " and tipo_utente in ('01') ";
            Dictionary<String, Object> coll = new Dictionary<String, Object>();
            coll.Add("matri", pUser);
            try
            {
                //sResult = accDB.sel_dati(sql, coll).Rows.Count.ToString();
                dt1 = accDB.sel_dati(sql, coll);
                sResult = dt1.Rows.Count.ToString();
                if (int.Parse(sResult) > 0)
                {
                    coll.Clear();
                    coll.Add("user_id", pUser);
                    sql = "select user_id, nome, cognome, AFF_org_CDS, EMAIL_ATE " +
                        //" from DAUTS.PP_PERS_STU_TUTTI_VW" +
                          " from PP_PERS_STU_TUTTI_VW" +
                          " where user_id = :pUser";
                    //sql = "select matricola, cognome, nome, aff_org" +
                    //      " from MNT_PERS_TABLE " +
                    //      //" from DAUTS.MNT_PERS_TABLE " +
                    //      " where matricola = :pUser";
                    DataTable dt = accDB.sel_dati(sql, coll);
                    if (dt.Rows.Count == 0)
                    {
                        return null;
                    }
                    else
                    {
                        oclsUtenteAmm.UserId = dt.Rows[0]["user_id"].ToString();
                        oclsUtenteAmm.Nome = dt.Rows[0]["nome"].ToString();
                        oclsUtenteAmm.Cognome = dt.Rows[0]["cognome"].ToString();
                        oclsUtenteAmm.Afferenza = dt.Rows[0]["aff_org_cds"].ToString();
                    }
                    return oclsUtenteAmm;
                }
                else
                {
                    throw new CustomExceptions.UtenteNonAbilitato();
                }
            }
            catch (CustomExceptions.UtenteNonAbilitato ex)
            {
                throw new CustomExceptions.UtenteNonAbilitato();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oclsUtenteAmm;
        }
        public DataTable PopolaDDLAnno(string pSelezione)
        {
            Dictionary<string, object> coll = new Dictionary<string, object>();
            coll.Add("selezione_cod", pSelezione);
            String sql = "select anno, CAST(anno as varchar(4)) as des_anno from val_selezioni" +
                         " where selezione_cod = :pSelezione and stato <> 9 " +
                         " union select 0 tipo_anno, '-- scegli un anno --' des_anno from dual" +
                         " order by 1";

            return accDB.sel_dati(sql, coll);
        }
        public DataTable PopolaDDLAnnoArchivio(string pSelezione)
        {
            Dictionary<string, object> coll = new Dictionary<string, object>();
            coll.Add("selezione_cod", pSelezione);
            String sql = "select anno, CAST(anno as varchar(4)) as des_anno from val_selezioni_bck" +
                         " where selezione_cod = :pSelezione and stato <> 9 " +
                         " union select 0 tipo_anno, '-- scegli un anno --' des_anno from dual" +
                         " order by 1";

            return accDB.sel_dati(sql, coll);
        }
        public DataTable PopolaDDLCategoria(string pSelezione, int pAnno)
        {
            Dictionary<string, object> coll = new Dictionary<string, object>();
            coll.Add("selezione_cod", pSelezione);
            coll.Add("anno", pAnno);
            String sql = "select categoria_cod as des_categoria, categoria_cod as categoria from val_selezioni" +
                         " where selezione_cod  = :pSelezione and anno = :pAnno" +
                         " union select ' -- Seleziona Categoria --' as des_categoria, '0' as categoria" +
                         " from dual order by 1";
            return accDB.sel_dati(sql, coll);
        }
        public DataTable PopolaDDLAfferenza(int pAnno)
        {
            Dictionary<string, object> coll = new Dictionary<string, object>();
            coll.Add("anno", pAnno);
            //String sql = "select descr_sede_dip as descrizione, sede_dip cod  from dauts.val_resp_dip_vw" +
            String sql = "select descr_sede_dip as descrizione, sede_dip cod  from val_resp_dip_vw" +
                         " where anno = :pAnno" +
                         " union select ' -- Seleziona Afferenza --' as descrizione, '0' as cod" +
                         " from dual order by 1";
            return accDB.sel_dati(sql, coll);
        }
        public DataTable PopolaDDLMatricola(int pAnno)
        {
            Dictionary<string, object> coll = new Dictionary<string, object>();
            coll.Add("anno", pAnno);
            //String sql = "select CAST(matri_dip as varchar(6)) as descrizione, matri_dip cod from dauts.val_resp_dip_vw" +
            String sql = "select CAST(matri_dip as varchar(6)) as descrizione, matri_dip cod from val_resp_dip_vw" +
                         " where anno = :pAnno" +
                         " union select ' -- Seleziona Matricola Dipendente --' as descrizione, 0 as cod" +
                         " from dual order by 1";
            return accDB.sel_dati(sql, coll);
        }
        public DataTable PopolaDDLStato()
        {
            String sql = "select descr as descrizione, stato as cod from val_stati" +
                         " where stato >= 10" +
                         " union select ' -- Seleziona Stato --' as descrizione, -2 as cod from dual" +
                         " union select 'Senza Compilazione' as descrizione, 0 as cod" +
                         " from dual order by 2";
            return accDB.sel_dati(sql, null);
        }
        public DataTable PopolaDDLCognome(int pAnno)
        {
            Dictionary<string, object> coll = new Dictionary<string, object>();
            coll.Add("anno", pAnno);
            String sql = "select cognome_dip || ' ' || nome_dip as descrizione, matri_dip as cod from val_resp_dip_vw" +
                         " where anno = :pAnno" +
                         " union select ' -- Seleziona Cognome --' as descrizione, 0 as cod" +
                         " from dual order by 1";
            return accDB.sel_dati(sql, coll);
        }
        public IList<Valutazioni> LeggiValutazioni(int pCompId, int pSelId, string pCatcod, int? pIndId, int? pIndDetId, clsTransaction p_clsTrx)
        {
            string sql = " select COMP_ID, SELEZIONE_ID, CATEGORIA_COD, IND_ID, IND_DET_ID, DESCRIZIONE, PUNTEGGIO " +
                         " from VAL_VALUTAZIONI " +
                         " where comp_id = :comp_id " +
                         " and selezione_id = :selezione_id " +
                         " and categoria_cod = :categoria_cod";
            Dictionary<String, Object> coll = new Dictionary<String, Object>();
            coll.Add("comp_id", pCompId);
            coll.Add("selezione_id", pSelId);
            coll.Add("categoria_cod", pCatcod);
            if (pIndId != null)
            {
                sql += " and ind_id = :ind_id ";
                coll.Add("ind_id", pIndId);
            }
            if (pIndDetId != null)
            {
                sql += " and ind_det_id = :ind_det_id ";
                coll.Add("ind_det_id", pIndDetId);
            }
            Valutazioni oValutazione;
            IList<Valutazioni> lValutazione = new List<Valutazioni>();
            DataTable dt;
            dt = accDB.sel_dati(sql, coll);
            foreach (DataRow riga in dt.Rows)
            {
                oValutazione = new Valutazioni();
                oValutazione.CompId = Int32.Parse(riga["comp_id"].ToString());
                oValutazione.SelezioneId = Int32.Parse(riga["selezione_id"].ToString());
                oValutazione.CategoriaCod = riga["categoria_cod"].ToString();
                oValutazione.IndId = Int32.Parse(riga["ind_id"].ToString());
                oValutazione.IndDetId = Int32.Parse(riga["ind_det_id"].ToString());
                oValutazione.Descrizione = riga["descrizione"].ToString();
                oValutazione.Punteggio = float.Parse(riga["punteggio"].ToString());
                lValutazione.Add(oValutazione);
            }

            return lValutazione;
        }
        public void ins_valutazione(Valutazioni pValutazione, clsTransaction p_clsTrx)
        {
            Dictionary<String, Object> coll;
            clsTransaction clsTrx = p_clsTrx;
            if (clsTrx == null)
            {
                clsTrx = new clsTransaction();
            }
            try
            {
                coll = new Dictionary<String, Object>();
                coll.Add("comp_id", pValutazione.CompId);
                coll.Add("selezione_id", pValutazione.SelezioneId);
                coll.Add("categoria_cod", pValutazione.CategoriaCod);
                coll.Add("ind_id", pValutazione.IndId);
                coll.Add("ind_det_id", pValutazione.IndDetId);
                if (pValutazione.Descrizione != null)
                    coll.Add("descrizione", pValutazione.Descrizione);
                coll.Add("punteggio", pValutazione.Punteggio);
                coll.Add("usr_ins", pValutazione.UsrIns);
                coll.Add("data_ins", pValutazione.DataIns);
                coll.Add("usr_mod", pValutazione.UsrMod);
                coll.Add("data_mod", pValutazione.DataMod);
                clsTrx.insert_row_trx("val_valutazioni", coll);
            }
            catch (Exception ex)
            {
                if (p_clsTrx == null)
                {
                    clsTrx.rollback_trx();
                    clsTrx = null;
                }
                throw ex;
            }
            if (p_clsTrx == null)
            {
                clsTrx.commit_trx();
                clsTrx = null;
            }
        }
        public void upd_valutazione(Valutazioni pValutazione, clsTransaction p_clsTrx)
        {
            Dictionary<String, Object> coll;
            clsTransaction clsTrx = p_clsTrx;
            if (clsTrx == null)
            {
                clsTrx = new clsTransaction();
            }
            try
            {
                try
                {
                    coll = new Dictionary<String, Object>();
                    coll.Add("comp_id", pValutazione.CompId);
                    coll.Add("selezione_id", pValutazione.SelezioneId);
                    coll.Add("categoria_cod", pValutazione.CategoriaCod);
                    coll.Add("ind_id", pValutazione.IndId);
                    coll.Add("ind_det_id", pValutazione.IndDetId);
                    clsTrx.del_row_trx("val_valutazioni", coll);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                try
                {
                    ins_valutazione(pValutazione, clsTrx);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                if (p_clsTrx == null)
                {
                    clsTrx.rollback_trx();
                    clsTrx = null;
                }
                throw ex;
            }
            if (p_clsTrx == null)
            {
                clsTrx.commit_trx();
                clsTrx = null;
            }
        }
        public void del_valutazione(Valutazioni pValutazione, clsTransaction p_clsTrx)
        {
            Dictionary<String, Object> coll;
            clsTransaction clsTrx = p_clsTrx;
            if (clsTrx == null)
            {
                clsTrx = new clsTransaction();
            }
            try
            {
                coll = new Dictionary<String, Object>();
                coll.Add("comp_id", pValutazione.CompId);
                coll.Add("selezione_id", pValutazione.SelezioneId);
                coll.Add("categoria_cod", pValutazione.CategoriaCod);
                coll.Add("ind_id", pValutazione.IndId);
                coll.Add("ind_det_id", pValutazione.IndDetId);
                clsTrx.del_row_trx("val_valutazioni", coll);
            }
            catch (Exception ex)
            {
                if (p_clsTrx == null)
                {
                    clsTrx.rollback_trx();
                    clsTrx = null;
                }
                throw ex;
            }
            if (p_clsTrx == null)
            {
                clsTrx.commit_trx();
                clsTrx = null;
            }
        }
        //Compilazioni
        public int RicercaSelezione(clsUtenteLogin pUtente, string pSelezioneCod, string pTipo, clsTransaction p_clsTrx)
        {
            Dictionary<String, Object> coll;
            coll = new Dictionary<string, object>();
            string sql = "select selezione_id from val_selezioni " +
                         " where categoria_cod = :categoria_cod " +
                         " and selezione_cod = :selezione_cod " +
                         " and stato = 1";
            //if (pTipo == "com")
            //{

            //    sql += " and to_date(to_char(sysdate,'dd/mm/yyyy'), 'dd/mm/yyyy') between to_date(to_char(data_iniz_val,'dd/mm/yyyy'), 'dd/mm/yyyy') and to_date(to_char(data_term_pres,'dd/mm/yyyy'), 'dd/mm/yyyy')";
            //}
            coll.Add("categoria_cod", pUtente.Categoria);
            coll.Add("selezione_cod", pSelezioneCod);
            int SelId;
            try
            {
                SelId = int.Parse(accDB.sel_dati(sql, coll).Rows[0][0].ToString());
            }
            catch
            {
                SelId = 0;
            }
            return SelId;

        }
        public int RicercaCompilazione(string pUtente, int pSelezioneId, clsTransaction p_clsTrx)
        {
            Dictionary<String, Object> coll;
            coll = new Dictionary<string, object>();
            string sql = "select distinct comp_id from val_risposte " +
                         " where selezione_id = :selezione_id " +
                         " and matri_dip = :matri_dip";
            coll.Add("selezione_id", pSelezioneId);
            coll.Add("matri_dip", Int32.Parse(pUtente));
            int CompId;
            try
            {
                CompId = int.Parse(accDB.sel_dati(sql, coll).Rows[0][0].ToString());
            }
            catch (Exception ex)
            {
                CompId = 0;
            }
            return CompId;

        }
        public bool ExistsCompilazioni(int pSelId, string pCategoriaCod, int? pIndId, int? pIndDetId, int? pCompId, clsTransaction p_clsTrx)
        {
            String sql = "select count(*) from val_risposte" +
                              " where selezione_id = :selezione_id ";
            Dictionary<String, Object> coll = new Dictionary<String, Object>();
            coll.Add("selezione_id", pSelId);
            if (pCategoriaCod != null)
            {
                coll.Add("categoria_cod", pCategoriaCod);
                sql += " and categoria_cod = :categoria_cod";
            }
            if (pIndId != null)
            {
                coll.Add("ind_id", pIndId);
                sql += " and ind_id = :ind_id";
            }
            if (pIndDetId != null)
            {
                coll.Add("ind_det_id", pIndDetId);
                sql += " and ind_det_id = :ind_det_id";
            }
            if (pCompId != null)
            {
                coll.Add("comp_id", pCompId);
                sql += " and comp_id = :pCompId";
            }
            if (p_clsTrx == null)
            {
                return (int.Parse(accDB.sel_dati(sql, coll).Rows[0][0].ToString()) != 0);
            }
            else
            {
                return (int.Parse(p_clsTrx.sel_dati(sql, coll).Rows[0][0].ToString()) != 0);
            }
        }
        public bool VerificaIns(int pSelId, int pCompId, string pUtente, clsTransaction p_clsTrx)
        {
            String sql = "select count(*) from val_risposte" +
                              " where selezione_id = :pSelId " +
                              " and matri_dip = :pUtente " +
                              " and comp_id <> :pCompId";
            Dictionary<String, Object> coll = new Dictionary<String, Object>();
            coll.Add("selezione_id", pSelId);
            coll.Add("matri_dip", pUtente);
            coll.Add("comp_id", pCompId);
            
            if (p_clsTrx == null)
            {
                return (int.Parse(accDB.sel_dati(sql, coll).Rows[0][0].ToString()) == 0);
            }
            else
            {
                return (int.Parse(p_clsTrx.sel_dati(sql, coll).Rows[0][0].ToString()) == 0);
            }
        }
        public bool ExistsCompilazioniUtente(int pSelId, string pCategoriaCod, int? pIndId, int? pIndDetId, int? pCompId, int? pRispId, string pUtente, clsTransaction p_clsTrx)
        {
            String sql = "select count(*) from val_risposte" +
                              " where selezione_id = :selezione_id ";
            Dictionary<String, Object> coll = new Dictionary<String, Object>();
            coll.Add("selezione_id", pSelId);
            if (pCategoriaCod != null)
            {
                coll.Add("categoria_cod", pCategoriaCod);
                sql += " and categoria_cod = :categoria_cod";
            }
            if (pIndId != null)
            {
                coll.Add("ind_id", pIndId);
                sql += " and ind_id = :ind_id";
            }
            if (pIndDetId != null)
            {
                coll.Add("ind_det_id", pIndDetId);
                sql += " and ind_det_id = :ind_det_id";
            }
            if (pCompId != null)
            {
                coll.Add("comp_id", pCompId);
                sql += " and comp_id = :pCompId";
            }
            if (pRispId != null)
            {
                coll.Add("risp_id", pRispId);
                sql += " and risp_id = :pRispId";
            }
            if (pUtente != null)
            {
                coll.Add("matri_dip", pUtente);
                sql += " and matri_dip = :pUtente";
            }
            if (p_clsTrx == null)
            {
                return (int.Parse(accDB.sel_dati(sql, coll).Rows[0][0].ToString()) != 0);
            }
            else
            {
                return (int.Parse(p_clsTrx.sel_dati(sql, coll).Rows[0][0].ToString()) != 0);
            }
        }
        public bool ExistsCompilazioniUtenteArchivio(int pSelId, string pCategoriaCod, int? pIndId, int? pIndDetId, int? pCompId, int? pRispId, string pUtente, clsTransaction p_clsTrx)
        {
            String sql = "select count(*) from val_risposte_bck" +
                              " where selezione_id = :selezione_id ";
            Dictionary<String, Object> coll = new Dictionary<String, Object>();
            coll.Add("selezione_id", pSelId);
            if (pCategoriaCod != null)
            {
                coll.Add("categoria_cod", pCategoriaCod);
                sql += " and categoria_cod = :categoria_cod";
            }
            if (pIndId != null)
            {
                coll.Add("ind_id", pIndId);
                sql += " and ind_id = :ind_id";
            }
            if (pIndDetId != null)
            {
                coll.Add("ind_det_id", pIndDetId);
                sql += " and ind_det_id = :ind_det_id";
            }
            if (pCompId != null)
            {
                coll.Add("comp_id", pCompId);
                sql += " and comp_id = :pCompId";
            }
            if (pRispId != null)
            {
                coll.Add("risp_id", pRispId);
                sql += " and risp_id = :pRispId";
            }
            if (pUtente != null)
            {
                coll.Add("matri_dip", pUtente);
                sql += " and matri_dip = :pUtente";
            }
            if (p_clsTrx == null)
            {
                return (int.Parse(accDB.sel_dati(sql, coll).Rows[0][0].ToString()) != 0);
            }
            else
            {
                return (int.Parse(p_clsTrx.sel_dati(sql, coll).Rows[0][0].ToString()) != 0);
            }
        }
        public IList<IndicatoreDetComp> LeggiIndicatoriDetComp(int pSelId, string pUtente, string pTipoSelezione)
        {
            int CompId = RicercaCompilazione(pUtente, pSelId, null);
            if (AggiornaStatoValutazione(pSelId, CompId, pTipoSelezione).RisposteVis == "N")
                return null;
            else
            {
                Dictionary<String, Object> coll;
                coll = new Dictionary<string, object>();
                String sql = "select i.SELEZIONE_ID, i.CATEGORIA_COD, i.IND_ID, " +
                             " id.IND_DET_ID, i.DESCR descr_ind, id.descr descr_ind_det, " +
                             " id.MAX_RIGHE " +
                             " from val_indicatori i, val_indicatori_det id" +
                             " where i.selezione_id = :pSelId " +
                             " and i.selezione_id = id.selezione_id " +
                             " and i.categoria_cod = id.categoria_cod " +
                             " and i.ind_id = id.ind_id ";
                if (AggiornaStatoValutazione(pSelId, CompId, pTipoSelezione).RisposteVis == "D")
                    sql += " and i.dip_flg = 1 ";
                else if (AggiornaStatoValutazione(pSelId, CompId, pTipoSelezione).RisposteVis == "R")
                    sql += " and i.dip_flg = 2 ";
                else if (AggiornaStatoValutazione(pSelId, CompId, pTipoSelezione).RisposteVis == "A")
                    sql += " and i.dip_flg = 3 ";
                sql += " ORDER BY i.ord, id.ord";
                coll.Add("selezione_id", pSelId);

                IList<IndicatoreDetComp> lIndicatoriDetComp = new List<IndicatoreDetComp>();
                IndicatoreDetComp oIndicatoreDetComp;
                DataTable dt;
                dt = accDB.sel_dati(sql, coll);
                foreach (DataRow riga in dt.Rows)
                {
                    oIndicatoreDetComp = new IndicatoreDetComp();
                    oIndicatoreDetComp.SelezioneId = Int32.Parse(riga["selezione_id"].ToString());
                    oIndicatoreDetComp.CategoriaCod = riga["categoria_cod"].ToString();
                    oIndicatoreDetComp.IndId = Int32.Parse(riga["ind_id"].ToString());
                    oIndicatoreDetComp.IndDetId = Int32.Parse(riga["ind_det_id"].ToString());
                    oIndicatoreDetComp.DescrInd = riga["descr_ind"].ToString();
                    //oIndicatoreDetComp.DescrDet = riga["descr_ind_det"].ToString();
                    oIndicatoreDetComp.DescrDet = HttpUtility.HtmlDecode(riga["descr_ind_det"].ToString()); //>>ga22072013<<
                    oIndicatoreDetComp.MaxRighe = Int32.Parse(riga["max_righe"].ToString());

                    if (pUtente != null && pUtente != "")
                    {
                        if (CompId != 0)
                        {
                            oIndicatoreDetComp.CompId = CompId;
                            oIndicatoreDetComp.FlRisposte = ExistsCompilazioni(pSelId, riga["categoria_cod"].ToString(), Int32.Parse(riga["ind_id"].ToString()), Int32.Parse(riga["ind_det_id"].ToString()), CompId, null);
                            sql = "select punteggio from val_valutazioni " +
                                  " where comp_id = :comp_id" +
                                  " and selezione_id = :selezione_id" +
                                  " and categoria_cod = :categoria_cod" +
                                  " and ind_id = :ind_id" +
                                  " and ind_det_id = :ind_det_id";
                            coll = new Dictionary<string, object>();
                            coll.Add("comp_id", CompId);
                            coll.Add("selezione_id", pSelId);
                            coll.Add("categoria_cod", riga["categoria_cod"].ToString());
                            coll.Add("ind_id", Int32.Parse(riga["ind_id"].ToString()));
                            coll.Add("ind_det_id", Int32.Parse(riga["ind_det_id"].ToString()));
                            if (accDB.sel_dati(sql, coll).Rows.Count > 0)
                                oIndicatoreDetComp.Punteggio = float.Parse(accDB.sel_dati(sql, coll).Rows[0][0].ToString());
                            else
                                oIndicatoreDetComp.Punteggio = 0;
                        }
                    }
                    if (ExistsNote(pSelId, riga["categoria_cod"].ToString(), Int32.Parse(riga["ind_id"].ToString()), Int32.Parse(riga["ind_det_id"].ToString())))
                    {
                        if (AggiornaStatoValutazione(pSelId, CompId, pTipoSelezione).NoteVis != "N")
                            lIndicatoriDetComp.Add(oIndicatoreDetComp);
                    }
                    else
                        lIndicatoriDetComp.Add(oIndicatoreDetComp);
                }
                return lIndicatoriDetComp;
            }
        }
        public clsUtenteLogin LeggiUtenteLogin(string pUser)
        {
            String sql = "select matri_dip, nome_dip, cognome_dip, e_mail_dip, " +
                         " matri_resp, nome_resp, cognome_resp, e_mail_resp, " +
                         " trim(substr(inquadr_dip,1,2)) categoria, trim(substr(inquadr_dip,3,1)) livello, anno " +
                         " from val_resp_dip_vw v" +
                //" from dauts.val_resp_dip_vw " +
                         " where matri_dip = :pUser" +
                         " AND ANNO = (SELECT MAX(ANNO) FROM VAL_DIP where matri_def = v.matri_dip)";
            Dictionary<String, Object> coll = new Dictionary<String, Object>();
            coll.Add("user_id", pUser);
            clsUtenteLogin oclsUtenteLogin = new clsUtenteLogin();
            DataTable dt = accDB.sel_dati(sql, coll);
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            else
            {
                oclsUtenteLogin.UserId = dt.Rows[0]["matri_dip"].ToString();
                oclsUtenteLogin.Nome = dt.Rows[0]["nome_dip"].ToString();
                oclsUtenteLogin.Cognome = dt.Rows[0]["cognome_dip"].ToString();
                oclsUtenteLogin.Email = dt.Rows[0]["e_mail_dip"].ToString();
                oclsUtenteLogin.UserIdResp = dt.Rows[0]["matri_resp"].ToString();
                oclsUtenteLogin.EmailResp = dt.Rows[0]["e_mail_resp"].ToString();
                oclsUtenteLogin.Categoria = dt.Rows[0]["categoria"].ToString();
                oclsUtenteLogin.Livello = dt.Rows[0]["livello"].ToString();
                oclsUtenteLogin.CognomeResp = dt.Rows[0]["cognome_resp"].ToString();  //>>ga22072013<<
                oclsUtenteLogin.NomeResp = dt.Rows[0]["nome_resp"].ToString(); //>>ga22072013<<
                oclsUtenteLogin.Anno = int.Parse(dt.Rows[0]["anno"].ToString()); //>>ga22072013<<

                sql = "select * " +
                    //" from DAUTS.PP_PERS_STU_TUTTI_VW" +
                      " from PP_PERS_STU_TUTTI_VW" +
                      " where user_id = :pUser";
                dt = accDB.sel_dati(sql, coll);
                if (dt.Rows.Count != 0)
                {
                    oclsUtenteLogin.codfis = dt.Rows[0]["cod_fisc"].ToString();
                    oclsUtenteLogin.IndirizzoDomicilio = dt.Rows[0]["indirizzo_domicilio"].ToString() + "," + dt.Rows[0]["cap_domicilio"].ToString() + " " + dt.Rows[0]["comune_domicilio"].ToString();
                    oclsUtenteLogin.IndirizzoResidenza = dt.Rows[0]["indirizzo_residenza"].ToString() + "," + dt.Rows[0]["cap_residenza"].ToString() + " " + dt.Rows[0]["comune_residenza"].ToString();
                    oclsUtenteLogin.LuogoNascita = dt.Rows[0]["loc_nascita"].ToString();
                    oclsUtenteLogin.Afferenza = dt.Rows[0]["descr_aff_cds"].ToString();
                    if (dt.Rows[0]["data_nascita"].ToString() != "")
                    {
                        oclsUtenteLogin.DataNascita = DateTime.Parse(dt.Rows[0]["data_nascita"].ToString());
                    }
                    oclsUtenteLogin.TelInterno = dt.Rows[0]["tel_interno"].ToString();
                }
                oclsUtenteLogin.Archivio = 0;
                sql = "select count(*) " +
                      " from val_risposte_bck" +
                      " where matri_dip = :pUser";
                dt = accDB.sel_dati(sql, coll);
                if (int.Parse(dt.Rows[0].ItemArray[0].ToString()) != 0)
                {
                    oclsUtenteLogin.Archivio = 1;
                }
                return oclsUtenteLogin;
            }
        }

        public int rec_compid(clsTransaction p_clsTrx)
        {
            //String sql = "select nvl(max(comp_id),0) + 1 from val_risposte" +
            //             " where selezione_id = :selezione_id " +
            //             " and categoria_cod = :categoria_cod";
            String sql = "SELECT val_comp_id.nextval  FROM dual";
            Dictionary<String, Object> coll = new Dictionary<String, Object>();
            //coll.Add("selezione_id", pSelId);
            //coll.Add("categoria_cod", pCatcod);
            if (p_clsTrx == null)
            {
                return int.Parse(accDB.sel_dati(sql, null).Rows[0][0].ToString());
            }
            else
            {
                return int.Parse(p_clsTrx.sel_dati(sql, null).Rows[0][0].ToString());
            }
        }
        
        public void ins_compilazione(CompRisposte pCompRisposte, clsTransaction p_clsTrx)
        {
            Dictionary<String, Object> coll;
            clsTransaction clsTrx = p_clsTrx;
            if (clsTrx == null)
            {
                clsTrx = new clsTransaction();
            }
            try
            {
                foreach (Risposta item in pCompRisposte.RigheRisposta)
                {
                    coll = new Dictionary<String, Object>();
                    coll.Add("comp_id", pCompRisposte.CompId);
                    coll.Add("selezione_id", item.SelezioneId);
                    coll.Add("categoria_cod", item.CategoriaCod);
                    coll.Add("ind_id", item.IndId);
                    coll.Add("ind_det_id", item.IndDetId);
                    coll.Add("risp_id", item.RispId);
                    coll.Add("tipo_riga", item.TipoRiga);
                    coll.Add("riga_id", item.RigaId);
                    coll.Add("risp", item.Risp);
                    coll.Add("matri_dip", pCompRisposte.MatriDip);
                    coll.Add("matri_rsp", pCompRisposte.MatriRsp);
                    coll.Add("stato", pCompRisposte.Stato);
                    coll.Add("usr_ins", item.UsrIns);
                    coll.Add("data_ins", item.DataIns);
                    coll.Add("usr_mod", item.UsrMod);
                    coll.Add("data_mod", item.DataMod);
                    clsTrx.insert_row_trx("val_risposte", coll);
                }
            }
            catch (Exception ex)
            {
                if (p_clsTrx == null)
                {
                    clsTrx.rollback_trx();
                    clsTrx = null;
                }
                throw ex;
            }
            if (p_clsTrx == null)
            {
                clsTrx.commit_trx();
                clsTrx = null;
            }
        }
        public CompRisposte LeggiRisposte(int pCompId, int pSelId, string pCatCod, int? pIndId, int? pIndDetId, int? pRispId, int? pTipoRiga, int? pRigaId, clsTransaction p_clsTrx)
        {
            Dictionary<String, Object> coll = new Dictionary<string, object>();
            String sql = "select r.COMP_ID, r.SELEZIONE_ID, r.CATEGORIA_COD, r.IND_ID, " +
                          " r.IND_DET_ID, r.RISP_ID, r.TIPO_RIGA, r.RIGA_ID, " +
                          " r.RISP, r.MATRI_DIP, r.MATRI_RSP, r.STATO, s.DESCR DescrStato, " +
                          " r.USR_INS, DATA_INS, r.USR_MOD, r.DATA_MOD " +
                          " from VAL_RISPOSTE r, VAL_STATI s" +
                          " where r.selezione_id = :pSelId " +
                          " and r.stato = s.stato " +
                          " and r.comp_id = :comp_id ";
            coll.Add("selezione_id", pSelId);
            coll.Add("comp_id", pCompId);
            if (pCatCod != null && pCatCod != "")
            {
                sql += " and r.categoria_cod = :categoria_cod";
                coll.Add("categoria_cod", pCatCod);
            }
            if (pIndId != null && pIndId != 0)
            {
                sql += " and r.ind_id = :ind_id";
                coll.Add("ind_id", pIndId);
            }
            if (pIndDetId != null && pIndDetId != 0)
            {
                sql += " and r.ind_det_id = :ind_det_id";
                coll.Add("ind_det_id", pIndDetId);
            }
            if (pRispId != null && pRispId != 0)
            {
                sql += " and r.risp_id = :risp_id";
                coll.Add("risp_id", pRispId);
            }
            if (pTipoRiga != null && pTipoRiga != 0)
            {
                sql += " and r.tipo_riga = :tipo_riga";
                coll.Add("tipo_riga", pTipoRiga);
            }
            if (pRigaId != null && pRigaId != 0)
            {
                sql += " and r.riga_id = :riga_id";
                coll.Add("riga_id", pRigaId);
            }
            CompRisposte oCompRisposte = new CompRisposte();
            Risposta oRisposta;
            List<Risposta> lRisposta = new List<Risposta>();
            DataTable dt;
            dt = accDB.sel_dati(sql, coll);
            foreach (DataRow riga in dt.Rows)
            {
                oCompRisposte.CompId = Int32.Parse(riga["comp_id"].ToString());
                oCompRisposte.Stato = Int32.Parse(riga["stato"].ToString());
                oCompRisposte.DescrStato = riga["descrstato"].ToString();
                oCompRisposte.MatriDip = Int32.Parse(riga["matri_dip"].ToString());
                oCompRisposte.MatriRsp = Int32.Parse(riga["matri_rsp"].ToString());
                oRisposta = new Risposta();
                oRisposta.SelezioneId = Int32.Parse(riga["selezione_id"].ToString());
                oRisposta.CategoriaCod = riga["categoria_cod"].ToString();
                oRisposta.IndId = Int32.Parse(riga["ind_id"].ToString());
                oRisposta.IndDetId = Int32.Parse(riga["ind_det_id"].ToString());
                oRisposta.RispId = Int32.Parse(riga["risp_id"].ToString());
                oRisposta.TipoRiga = Int32.Parse(riga["tipo_riga"].ToString());
                oRisposta.RigaId = Int32.Parse(riga["riga_id"].ToString());
                oRisposta.Risp = HttpUtility.HtmlDecode(riga["risp"].ToString());
                oRisposta.UsrIns = riga["usr_ins"].ToString();
                if (riga["data_ins"].ToString() != null && riga["data_ins"].ToString() == "")
                    oRisposta.DataIns = DateTime.Parse(riga["data_ins"].ToString());
                oRisposta.UsrMod = riga["usr_mod"].ToString();
                if (riga["data_mod"].ToString() != null && riga["data_mod"].ToString() != "")
                    oRisposta.DataMod = DateTime.Parse(riga["data_mod"].ToString());
                lRisposta.Add(oRisposta);
            }
            oCompRisposte.RigheRisposta = lRisposta;
            return oCompRisposte;
        }
        // estrazione di tutte le risposte per passare i dati da val_risposte in val_risposte_bck
        public IList<RispostaArchivio> LeggiRisposteArchivio(int pSelId, string pCatCod, clsTransaction p_clsTrx)
        {

            RispostaArchivio oRisposta = new RispostaArchivio();
            List<RispostaArchivio> lRisposte = new List<RispostaArchivio>();
            Dictionary<String, Object> coll = new Dictionary<string, object>();
            String sql = "select r.COMP_ID, r.SELEZIONE_ID, r.CATEGORIA_COD, r.IND_ID, " +
                          " r.IND_DET_ID, r.RISP_ID, r.TIPO_RIGA, r.RIGA_ID, " +
                          " r.RISP, r.MATRI_DIP, r.MATRI_RSP, r.STATO, s.DESCR DescrStato, " +
                          " r.USR_INS, r.DATA_INS, r.USR_MOD, r.DATA_MOD " +
                          " from VAL_RISPOSTE r, VAL_STATI s" +
                          " where r.selezione_id = :pSelId " +
                          " and r.stato = s.stato ";

            coll.Add("selezione_id", pSelId);

            if (pCatCod != null && pCatCod != "")
            {
                sql += " and r.categoria_cod = :categoria_cod";
                coll.Add("categoria_cod", pCatCod);
            }


            DataTable dt;
            dt = accDB.sel_dati(sql, coll);

            foreach (DataRow riga in dt.Rows)
            {
                oRisposta = new RispostaArchivio();
                oRisposta.CompId = Int32.Parse(riga["comp_id"].ToString());
                oRisposta.Stato = Int32.Parse(riga["stato"].ToString());
                oRisposta.MatriDip = Int32.Parse(riga["matri_dip"].ToString());
                oRisposta.MatriResp = Int32.Parse(riga["matri_rsp"].ToString());
                oRisposta.SelezioneId = Int32.Parse(riga["selezione_id"].ToString());
                oRisposta.CategoriaCod = riga["categoria_cod"].ToString();
                oRisposta.IndId = Int32.Parse(riga["ind_id"].ToString());
                oRisposta.IndDetId = Int32.Parse(riga["ind_det_id"].ToString());
                oRisposta.RispId = Int32.Parse(riga["risp_id"].ToString());
                oRisposta.TipoRiga = Int32.Parse(riga["tipo_riga"].ToString());
                oRisposta.RigaId = Int32.Parse(riga["riga_id"].ToString());
                oRisposta.Risp = HttpUtility.HtmlDecode(riga["risp"].ToString());
                oRisposta.UsrIns = riga["usr_ins"].ToString();
                if (riga["data_ins"].ToString() != null && riga["data_ins"].ToString() != "")
                    oRisposta.DataIns = DateTime.Parse(riga["data_ins"].ToString());
                oRisposta.UsrMod = riga["usr_mod"].ToString();
                if (riga["data_mod"].ToString() != null && riga["data_mod"].ToString() != "")
                    oRisposta.DataMod = DateTime.Parse(riga["data_mod"].ToString());
                lRisposte.Add(oRisposta);
            }

            return lRisposte;
        }
        // estrazione delle risposte di una compilazione nella tabella  val_risposte_bck
        public CompRisposte LeggiRisposteArchivioComp(int pCompId, int pSelId, string pCatCod, int? pIndId, int? pIndDetId, int? pRispId, int? pTipoRiga, int? pRigaId, clsTransaction p_clsTrx)
        {

            Dictionary<String, Object> coll = new Dictionary<string, object>();
            String sql = "select r.COMP_ID, r.SELEZIONE_ID, r.CATEGORIA_COD, r.IND_ID, " +
                          " r.IND_DET_ID, r.RISP_ID, r.TIPO_RIGA, r.RIGA_ID, " +
                          " r.RISP, r.MATRI_DIP, r.MATRI_RSP, r.STATO, s.DESCR DescrStato, " +
                          " r.USR_INS, r.DATA_INS, r.USR_MOD, r.DATA_MOD " +
                          " from VAL_RISPOSTE_BCK r, VAL_STATI s" +
                          " where r.selezione_id = :pSelId " +
                          " and r.stato = s.stato " +
                          " and r.COMP_ID = :pCompId ";

            coll.Add("selezione_id", pSelId);
            coll.Add("comp_id", pCompId);

            if (pCatCod != null && pCatCod != "")
            {
                sql += " and r.categoria_cod = :categoria_cod";
                coll.Add("categoria_cod", pCatCod);
            }

            if (pIndId != null && pIndId != 0)
            {
                sql += " and r.ind_id = :ind_id";
                coll.Add("ind_id", pIndId);
            }
            if (pIndDetId != null && pIndDetId != 0)
            {
                sql += " and r.ind_det_id = :ind_det_id";
                coll.Add("ind_det_id", pIndDetId);
            }
            if (pRispId != null && pRispId != 0)
            {
                sql += " and r.risp_id = :risp_id";
                coll.Add("risp_id", pRispId);
            }
            if (pTipoRiga != null && pTipoRiga != 0)
            {
                sql += " and r.tipo_riga = :tipo_riga";
                coll.Add("tipo_riga", pTipoRiga);
            }
            if (pRigaId != null && pRigaId != 0)
            {
                sql += " and r.riga_id = :riga_id";
                coll.Add("riga_id", pRigaId);
            }
            CompRisposte oCompRisposte = new CompRisposte();
            Risposta oRisposta;
            List<Risposta> lRisposta = new List<Risposta>();
            DataTable dt;
            dt = accDB.sel_dati(sql, coll);
            foreach (DataRow riga in dt.Rows)
            {
                oCompRisposte.CompId = Int32.Parse(riga["comp_id"].ToString());
                oCompRisposte.Stato = Int32.Parse(riga["stato"].ToString());
                oCompRisposte.DescrStato = riga["descrstato"].ToString();
                oCompRisposte.MatriDip = Int32.Parse(riga["matri_dip"].ToString());
                oCompRisposte.MatriRsp = Int32.Parse(riga["matri_rsp"].ToString());
                oRisposta = new Risposta();
                oRisposta.SelezioneId = Int32.Parse(riga["selezione_id"].ToString());
                oRisposta.CategoriaCod = riga["categoria_cod"].ToString();
                oRisposta.IndId = Int32.Parse(riga["ind_id"].ToString());
                oRisposta.IndDetId = Int32.Parse(riga["ind_det_id"].ToString());
                oRisposta.RispId = Int32.Parse(riga["risp_id"].ToString());
                oRisposta.TipoRiga = Int32.Parse(riga["tipo_riga"].ToString());
                oRisposta.RigaId = Int32.Parse(riga["riga_id"].ToString());
                oRisposta.Risp = HttpUtility.HtmlDecode(riga["risp"].ToString());
                oRisposta.UsrIns = riga["usr_ins"].ToString();
                if (riga["data_ins"].ToString() != null && riga["data_ins"].ToString() == "")
                    oRisposta.DataIns = DateTime.Parse(riga["data_ins"].ToString());
                oRisposta.UsrMod = riga["usr_mod"].ToString();
                if (riga["data_mod"].ToString() != null && riga["data_mod"].ToString() != "")
                    oRisposta.DataMod = DateTime.Parse(riga["data_mod"].ToString());
                lRisposta.Add(oRisposta);
            }
            oCompRisposte.RigheRisposta = lRisposta;
            return oCompRisposte;
        }
        public IList<Valutazioni> LeggiValutazioniArchivio(int pSelId, string pCatCod, clsTransaction p_clsTrx)
        {

            Valutazioni oValutazioni = new Valutazioni();
            List<Valutazioni> lValutazioni = new List<Valutazioni>();
            Dictionary<String, Object> coll = new Dictionary<string, object>();
            String sql = "select r.COMP_ID, r.SELEZIONE_ID, r.CATEGORIA_COD, r.IND_ID, " +
                          " r.IND_DET_ID, r.descrizione, r.punteggio, " +
                          " r.USR_INS, DATA_INS, r.USR_MOD, r.DATA_MOD " +
                          " from VAL_VALUTAZIONI r" +
                          " where r.selezione_id = :pSelId ";
            // " and r.COMP_ID = 91 ";  // attenzione da eliminare;

            coll.Add("selezione_id", pSelId);
            if (pCatCod != null && pCatCod != "")
            {
                sql += " and r.categoria_cod = :categoria_cod";
                coll.Add("categoria_cod", pCatCod);
            }


            DataTable dt;
            dt = accDB.sel_dati(sql, coll);

            foreach (DataRow riga in dt.Rows)
            {
                oValutazioni = new Valutazioni();
                oValutazioni.CompId = Int32.Parse(riga["comp_id"].ToString());
                oValutazioni.SelezioneId = Int32.Parse(riga["selezione_id"].ToString());
                oValutazioni.CategoriaCod = riga["categoria_cod"].ToString();
                oValutazioni.IndId = Int32.Parse(riga["ind_id"].ToString());
                oValutazioni.IndDetId = Int32.Parse(riga["ind_det_id"].ToString());
                oValutazioni.Descrizione = riga["descrizione"].ToString();
                oValutazioni.Punteggio = float.Parse(riga["punteggio"].ToString());
                oValutazioni.UsrIns = riga["usr_ins"].ToString();
                if (riga["data_ins"].ToString() != null && riga["data_ins"].ToString() != "")
                    oValutazioni.DataIns = DateTime.Parse(riga["data_ins"].ToString());
                oValutazioni.UsrMod = riga["usr_mod"].ToString();
                if (riga["data_mod"].ToString() != null && riga["data_mod"].ToString() != "")
                    oValutazioni.DataMod = DateTime.Parse(riga["data_mod"].ToString());
                lValutazioni.Add(oValutazioni);
            }

            return lValutazioni;
        }
        public IList<Valutazioni> LeggiValutazioniArchivioComp(int pCompId, int pSelId, string pCatcod, int? pIndId, int? pIndDetId, clsTransaction p_clsTrx)
        {
            string sql = " select COMP_ID, SELEZIONE_ID, CATEGORIA_COD, IND_ID, IND_DET_ID, DESCRIZIONE, PUNTEGGIO " +
                         " from VAL_VALUTAZIONI_BCK " +
                         " where comp_id = :comp_id " +
                         " and selezione_id = :selezione_id " +
                         " and categoria_cod = :categoria_cod";
            Dictionary<String, Object> coll = new Dictionary<String, Object>();
            coll.Add("comp_id", pCompId);
            coll.Add("selezione_id", pSelId);
            coll.Add("categoria_cod", pCatcod);
            if (pIndId != null)
            {
                sql += " and ind_id = :ind_id ";
                coll.Add("ind_id", pIndId);
            }
            if (pIndDetId != null)
            {
                sql += " and ind_det_id = :ind_det_id ";
                coll.Add("ind_det_id", pIndDetId);
            }
            Valutazioni oValutazione;
            IList<Valutazioni> lValutazione = new List<Valutazioni>();
            DataTable dt;
            dt = accDB.sel_dati(sql, coll);
            foreach (DataRow riga in dt.Rows)
            {
                oValutazione = new Valutazioni();
                oValutazione.CompId = Int32.Parse(riga["comp_id"].ToString());
                oValutazione.SelezioneId = Int32.Parse(riga["selezione_id"].ToString());
                oValutazione.CategoriaCod = riga["categoria_cod"].ToString();
                oValutazione.IndId = Int32.Parse(riga["ind_id"].ToString());
                oValutazione.IndDetId = Int32.Parse(riga["ind_det_id"].ToString());
                oValutazione.Descrizione = riga["descrizione"].ToString();
                oValutazione.Punteggio = float.Parse(riga["punteggio"].ToString());
                lValutazione.Add(oValutazione);
            }

            return lValutazione;
        }
        // estrazione delle risposte di una compilazione nella tabella  val_risposte_bck fine
        //public void upd_compilazione(CompRisposte pCompRisposte, clsTransaction p_clsTrx) //>>ga13092016<<
        public void upd_compilazione(CompRisposte pOldCompRisposte, CompRisposte pCompRisposte, clsTransaction p_clsTrx) //>>ga13092016<<
        {
            Dictionary<String, Object> coll;
            clsTransaction clsTrx = p_clsTrx;
            if (clsTrx == null)
            {
                clsTrx = new clsTransaction();
            }
            try
            {
                try
                /*{ //>>ga092015<< possono esistere diversi tipo_riga => con peo 2015 è stato creato il campo note in ogni domanda per inserire le valutazione sulle risposte date
                    coll = new Dictionary<String, Object>();
                    coll.Add("comp_id", pCompRisposte.CompId);
                    coll.Add("selezione_id", pCompRisposte.RigheRisposta[0].SelezioneId);
                    coll.Add("categoria_cod", pCompRisposte.RigheRisposta[0].CategoriaCod);
                    coll.Add("ind_id", pCompRisposte.RigheRisposta[0].IndId);
                    coll.Add("ind_det_id", pCompRisposte.RigheRisposta[0].IndDetId);
                    coll.Add("tipo_riga", pCompRisposte.RigheRisposta[0].TipoRiga);
                    clsTrx.del_row_trx("val_risposte", coll);
                }*/
                {

                    //foreach (Risposta oRisposta in pCompRisposte.RigheRisposta) //>>ga13092016<< elimino prima le risposte dal db e poi inserisco le nuove
                    foreach (Risposta oRisposta in pOldCompRisposte.RigheRisposta)
                    {
                        coll = new Dictionary<String, Object>();
                        coll.Add("comp_id", pCompRisposte.CompId);
                        coll.Add("selezione_id", oRisposta.SelezioneId);
                        coll.Add("categoria_cod", oRisposta.CategoriaCod);
                        coll.Add("ind_id", oRisposta.IndId);
                        coll.Add("ind_det_id", oRisposta.IndDetId);
                        coll.Add("tipo_riga", oRisposta.TipoRiga);
                        coll.Add("riga_id", oRisposta.RigaId);
                        coll.Add("risp_id", oRisposta.RispId);
                        clsTrx.del_row_trx("val_risposte", coll);
                    }
                }
                    // fine >>ga092015<<
                catch (Exception ex)
                {
                    throw ex;
                }
                try
                {
                    ins_compilazione(pCompRisposte, clsTrx);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                if (p_clsTrx == null)
                {
                    clsTrx.rollback_trx();
                    clsTrx = null;
                }
                throw ex;
            }
            if (p_clsTrx == null)
            {
                clsTrx.commit_trx();
                clsTrx = null;
            }
        }
        public void del_risp_IndicatoreDet(int pSelezioneId, string pCategoriaCod, int pCompId, int pIndId, int pIndDetId, string pUtente, clsTransaction p_clsTrx)
        {
            Dictionary<String, Object> coll;
            clsTransaction clsTrx = p_clsTrx;
            CompFile oCompFile;
            int OpId;
            if (clsTrx == null)
            {
                clsTrx = new clsTransaction();
            }
            try
            {
                coll = new Dictionary<String, Object>();
                coll.Add("comp_id", pCompId);
                coll.Add("selezione_id", pSelezioneId);
                coll.Add("categoria_cod", pCategoriaCod);
                coll.Add("ind_id", pIndId);
                coll.Add("ind_det_id", pIndDetId);

                OpId = del_risp_nota_log(pSelezioneId, pCategoriaCod, pCompId, pIndId, pIndDetId, null, pUtente, clsTrx);

                Valutazioni oValutazioni = new Valutazioni();
                oValutazioni.SelezioneId = pSelezioneId;
                oValutazioni.CompId = pCompId;
                oValutazioni.CategoriaCod = pCategoriaCod;
                oValutazioni.IndId = pIndId;
                oValutazioni.IndDetId = pIndDetId;
                del_valutazione(oValutazioni, clsTrx);

                clsTrx.del_row_trx("val_risposte", coll);

                //Lettura tabella allegati per scrittura su log
                string sql = "select COMP_ID, SELEZIONE_ID, CATEGORIA_COD, IND_ID, IND_DET_ID, RISP_ID, " +
                             " DESCRIZIONE, ALLEGATO, BYTES, EXT, USR_INS, DATA_INS, USR_MOD, DATA_MOD " +
                             " from val_allegati" +
                             " where comp_id = :comp_id" +
                             " and selezione_id = :selezione_id" +
                             " and categoria_cod = :categoria_cod" +
                             " and ind_id = :ind_id" +
                             " and ind_det_id = :ind_det_id";
                DataTable dt = accDB.sel_dati(sql, coll);
                foreach (DataRow riga in dt.Rows)
                {
                    oCompFile = new CompFile();
                    oCompFile.CompId = pCompId;
                    oCompFile.SelezioneId = pSelezioneId;
                    oCompFile.CategoriaCod = pCategoriaCod;
                    oCompFile.IndId = pIndId;
                    oCompFile.IndDetId = pIndDetId;
                    oCompFile.RispId = int.Parse(riga["risp_id"].ToString());
                    oCompFile.Descrizione = riga["descrizione"].ToString();
                    oCompFile.Blob = (byte[])riga["allegato"];
                    oCompFile.Bytes_curr = int.Parse(riga["bytes"].ToString());
                    oCompFile.Ext_curr = riga["ext"].ToString();
                    oCompFile.UsrIns = riga["usr_ins"].ToString();
                    if (riga["data_ins"].ToString() != null && riga["data_ins"].ToString() != "")
                        oCompFile.DataIns = DateTime.Parse(riga["data_ins"].ToString());
                    oCompFile.UsrMod = riga["usr_mod"].ToString();
                    if (riga["data_mod"].ToString() != null && riga["data_mod"].ToString() != "")
                        oCompFile.DataMod = (DateTime)riga["data_mod"];
                    //Scrittura su log
                    ins_compilazioneFile_log(oCompFile, OpId, "DELETE", pUtente, clsTrx);
                }

                clsTrx.del_row_trx("val_allegati", coll);
            }
            catch (Exception ex)
            {
                if (p_clsTrx == null)
                {
                    clsTrx.rollback_trx();
                    clsTrx = null;
                }
                throw ex;
            }
            if (p_clsTrx == null)
            {
                clsTrx.commit_trx();
                clsTrx = null;
            }
        }
        public int del_risp_Nota(int pSelezioneId, string pCategoriaCod, int pCompId, int pIndId, int pIndDetId, int pTipoRiga, string pUser, clsTransaction p_clsTrx)
        {
            Dictionary<String, Object> coll;
            clsTransaction clsTrx = p_clsTrx;
            int OpId;
            if (clsTrx == null)
            {
                clsTrx = new clsTransaction();
            }
            try
            {
                coll = new Dictionary<String, Object>();
                coll.Add("comp_id", pCompId);
                coll.Add("selezione_id", pSelezioneId);
                coll.Add("categoria_cod", pCategoriaCod);
                coll.Add("ind_id", pIndId);
                coll.Add("ind_det_id", pIndDetId);
                coll.Add("tipo_riga", pTipoRiga);

                OpId = del_risp_nota_log(pSelezioneId, pCategoriaCod, pCompId, pIndId, pIndDetId, pTipoRiga, pUser, clsTrx);

                clsTrx.del_row_trx("val_risposte", coll);
            }
            catch (Exception ex)
            {
                if (p_clsTrx == null)
                {
                    clsTrx.rollback_trx();
                    clsTrx = null;
                }
                throw ex;
            }
            if (p_clsTrx == null)
            {
                clsTrx.commit_trx();
                clsTrx = null;
            }
            return OpId;
        }
        public void del_compilazioneFile(CompFile pCompFile, int pOpId, string pUtente, clsTransaction p_clsTrx)
        {
            Dictionary<String, Object> coll;
            clsTransaction clsTrx = p_clsTrx;
            CompFile oCompFile = new CompFile();
            if (clsTrx == null)
            {
                clsTrx = new clsTransaction();
            }
            try
            {
                coll = new Dictionary<String, Object>();
                coll.Add("comp_id", pCompFile.CompId);
                coll.Add("selezione_id", pCompFile.SelezioneId);
                coll.Add("categoria_cod", pCompFile.CategoriaCod);
                coll.Add("ind_id", pCompFile.IndId);
                coll.Add("ind_det_id", pCompFile.IndDetId);
                coll.Add("risp_id", pCompFile.RispId);

                //Lettura tabella allegati per scrittura su log
                string sql = "select COMP_ID, SELEZIONE_ID, CATEGORIA_COD, IND_ID, IND_DET_ID, RISP_ID, " +
                             " DESCRIZIONE, ALLEGATO, BYTES, EXT, USR_INS, DATA_INS, USR_MOD, DATA_MOD " +
                             " from val_allegati" +
                             " where comp_id = :comp_id" +
                             " and selezione_id = :selezione_id" +
                             " and categoria_cod = :categoria_cod" +
                             " and ind_id = :ind_id" +
                             " and ind_det_id = :ind_det_id" +
                             " and risp_id = :risp_id";
                DataTable dt = accDB.sel_dati(sql, coll);
                if (dt.Rows.Count > 0)
                {
                    oCompFile.CompId = pCompFile.CompId;
                    oCompFile.SelezioneId = pCompFile.SelezioneId;
                    oCompFile.CategoriaCod = pCompFile.CategoriaCod;
                    oCompFile.IndId = pCompFile.IndId;
                    oCompFile.IndDetId = pCompFile.IndDetId;
                    oCompFile.RispId = pCompFile.RispId;
                    oCompFile.Descrizione = dt.Rows[0]["descrizione"].ToString();
                    oCompFile.Blob = (byte[])dt.Rows[0]["allegato"];
                    oCompFile.Bytes_curr = int.Parse(dt.Rows[0]["bytes"].ToString());
                    oCompFile.Ext_curr = dt.Rows[0]["ext"].ToString();
                    oCompFile.UsrIns = dt.Rows[0]["usr_ins"].ToString();
                    if (dt.Rows[0]["data_ins"].ToString() != null && dt.Rows[0]["data_ins"].ToString() != "")
                        oCompFile.DataIns = DateTime.Parse(dt.Rows[0]["data_ins"].ToString());
                    oCompFile.UsrMod = dt.Rows[0]["usr_mod"].ToString();
                    if (dt.Rows[0]["data_mod"].ToString() != null && dt.Rows[0]["data_mod"].ToString() != "")
                        oCompFile.DataMod = (DateTime)dt.Rows[0]["data_mod"];
                }
                //Scrittura su log
                ins_compilazioneFile_log(oCompFile, pOpId, "DELETE", pUtente, clsTrx);

                clsTrx.del_row_trx("val_allegati", coll);
            }
            catch (Exception ex)
            {
                if (p_clsTrx == null)
                {
                    clsTrx.rollback_trx();
                    clsTrx = null;
                }
                throw ex;
            }
            if (p_clsTrx == null)
            {
                clsTrx.commit_trx();
                clsTrx = null;
            }
        }
        public string ins_compilazioneFile(CompFile pCompFile, clsTransaction p_clsTrx)
        {
            Dictionary<String, Object> coll;
            coll = new Dictionary<String, Object>();
            Dictionary<String, Object> coll_w;
            coll_w = new Dictionary<String, Object>();
            clsTransaction clsTrx = p_clsTrx;
            string TipoOp;
            if (clsTrx == null)
            {
                clsTrx = new clsTransaction();
            }
            try
            {
                // verifico se già esiste un file
                DataRow dr = LeggiFile(pCompFile.CompId, pCompFile.SelezioneId, pCompFile.CategoriaCod, pCompFile.IndId, pCompFile.IndDetId, pCompFile.RispId);
                if (dr.Table.Rows.Count == 0) // nuovo inserimento file
                {

                    TipoOp = "INSERT";
                    coll.Add("comp_id", pCompFile.CompId);
                    coll.Add("selezione_id", pCompFile.SelezioneId);
                    coll.Add("categoria_cod", pCompFile.CategoriaCod);
                    coll.Add("ind_id", pCompFile.IndId);
                    coll.Add("ind_det_id", pCompFile.IndDetId);
                    coll.Add("risp_id", pCompFile.RispId);
                    coll.Add("descrizione", pCompFile.Descrizione);
                    coll.Add("allegato", pCompFile.Blob);
                    coll.Add("bytes", pCompFile.Bytes_curr);
                    coll.Add("ext", pCompFile.Ext_curr);
                    coll.Add("usr_ins", pCompFile.UsrIns);
                    coll.Add("data_ins", pCompFile.DataIns);

                    clsTrx.insert_row_trx("val_allegati", coll);
                }
                else
                {
                    TipoOp = "UPDATE";
                    coll_w.Clear();
                    coll_w.Add("comp_id", pCompFile.CompId);
                    coll_w.Add("selezione_id", pCompFile.SelezioneId);
                    coll_w.Add("categoria_cod", pCompFile.CategoriaCod);
                    coll_w.Add("ind_id", pCompFile.IndId);
                    coll_w.Add("ind_det_id", pCompFile.IndDetId);
                    coll_w.Add("risp_id", pCompFile.RispId);
                    coll.Clear();
                    coll.Add("descrizione", pCompFile.Descrizione);
                    coll.Add("allegato", pCompFile.Blob);
                    coll.Add("bytes", pCompFile.Bytes_curr);
                    coll.Add("ext", pCompFile.Ext_curr);
                    coll.Add("usr_ins", pCompFile.UsrIns);
                    coll.Add("data_ins", pCompFile.DataIns);
                    clsTrx.update_row_trx("val_allegati", coll, coll_w);
                }

            }
            catch (Exception ex)
            {
                if (p_clsTrx == null)
                {
                    clsTrx.rollback_trx();
                    clsTrx = null;
                }
                throw ex;
            }
            if (p_clsTrx == null)
            {
                clsTrx.commit_trx();
                clsTrx = null;
            }
            return TipoOp;
        }
        public void CambiaStato(int pSelezioneId, int pCompId, string pUtenteDip, clsTransaction p_clsTrx)
        {

            int codStatoAttuale = LeggiStatoCompilazioneCod(pSelezioneId, pUtenteDip);
            int codNewStato = 0;
            Dictionary<String, Object> coll;
            Dictionary<String, Object> coll_w;
            clsTransaction clsTrx = p_clsTrx;
            if (clsTrx == null)
            {
                clsTrx = new clsTransaction();
            }

            if (codStatoAttuale == 9)
            {
                codNewStato = 10;
            }
            else if (codStatoAttuale == 10)
            {
                codNewStato = 15;
            }
            else if (codStatoAttuale == 15)
            {
                codNewStato = 20;
            }
            else if (codStatoAttuale == 20)
            {
                codNewStato = 40;
            }
            else if (codStatoAttuale == 40)
            {
                codNewStato = 50;
            }
            else if (codStatoAttuale == 50)
            {
                codNewStato = 90;
            }
            if (codNewStato != 0)
            {
                try
                {

                    coll = new Dictionary<String, Object>();
                    coll_w = new Dictionary<String, Object>();
                    coll.Add("stato", codNewStato);
                    coll_w = new Dictionary<String, Object>();
                    coll_w.Add("selezione_id", pSelezioneId);
                    coll_w.Add("comp_id", pCompId);
                    clsTrx.update_row_trx("val_risposte", coll, coll_w);

                }
                catch (Exception ex)
                {
                    if (p_clsTrx == null)
                    {
                        clsTrx.rollback_trx();
                        clsTrx = null;
                    }
                    throw ex;
                }
            }
            if (p_clsTrx == null)
            {
                clsTrx.commit_trx();
                clsTrx = null;
            }
        }
        public void CambiaStatoPrev(int pSelezioneId, int pCompId, string pUtenteDip, clsTransaction p_clsTrx)
        {

            int codStatoAttuale = LeggiStatoCompilazioneCod(pSelezioneId, pUtenteDip);

            // non si può portare una compilazione in stato bozza: si parte almeno dallo stato 15: controllo ufficio
            if (codStatoAttuale > 10)
            {
                int codNewStato = 0;
                Dictionary<String, Object> coll;
                Dictionary<String, Object> coll_w;
                clsTransaction clsTrx = p_clsTrx;
                if (clsTrx == null)
                {
                    clsTrx = new clsTransaction();
                }
                if (codStatoAttuale == 15)
                {
                    codNewStato = 10;
                }
                else if (codStatoAttuale == 20)
                {
                    codNewStato = 15;
                }
                else if (codStatoAttuale == 40)
                {
                    codNewStato = 20;
                }
                else if (codStatoAttuale == 50)
                {
                    codNewStato = 40;
                }
                else if (codStatoAttuale == 90)
                {
                    codNewStato = 50;
                }
                try
                {
                    coll = new Dictionary<String, Object>();
                    coll_w = new Dictionary<String, Object>();
                    coll.Add("stato", codNewStato);
                    coll_w = new Dictionary<String, Object>();
                    coll_w.Add("selezione_id", pSelezioneId);
                    coll_w.Add("comp_id", pCompId);
                    clsTrx.update_row_trx("val_risposte", coll, coll_w);


                }
                catch (Exception ex)
                {
                    if (p_clsTrx == null)
                    {
                        clsTrx.rollback_trx();
                        clsTrx = null;
                    }
                    throw ex;
                }
                if (p_clsTrx == null)
                {
                    clsTrx.commit_trx();
                    clsTrx = null;
                }
            }
        }
        public DataRow LeggiFile(int pCompId, int pSelezioneId, string pCategoriaCod, int pIndId, int pIndDetId, int pRispId)
        {
            DataTable dt = new DataTable();
            DataRow dr = dt.NewRow();
            Dictionary<string, object> coll = new Dictionary<string, object>();
            coll.Add("comp_id", pCompId);
            coll.Add("selezione_id", pSelezioneId);
            coll.Add("categoria_cod", pCategoriaCod);
            coll.Add("ind_id", pIndId);
            coll.Add("ind_det_id", pIndDetId);
            coll.Add("risp_id", pRispId);
            String sql = "select allegato, bytes, ext, descrizione" +
                " from val_allegati " +
                " where comp_id = :comp_id " +
                " and selezione_id = :selezione_id" +
                " and categoria_cod = :categoria_cod" +
                " and ind_id = :ind_id" +
                " and ind_det_id = :ind_det_id" +
                " and risp_id = :risp_id";
            if (accDB.sel_dati(sql, coll).Rows.Count != 0)
                return accDB.sel_dati(sql, coll).Rows[0];
            else
                return dr;
        }
        public string LeggiStatoCompilazione(int pSelezioneId, string pUtente)
        {
            string sql;
            int CompId = RicercaCompilazione(pUtente, pSelezioneId, null);
            if (CompId == 0)
            {
                sql = " select descr from val_stati where stato = 9";
                return accDB.sel_dati(sql, null).Rows[0][0].ToString();
            }
            else
            {
                sql = " select distinct s.descr from val_stati s, val_risposte r" +
                                        " where r.selezione_id = :selezione_id " +
                                        " and r.comp_id = :comp_id " +
                                        " and r.stato = s.stato";
                Dictionary<string, object> coll = new Dictionary<string, object>();
                coll.Add("selezione_id", pSelezioneId);
                coll.Add("comp_id", CompId);
                return accDB.sel_dati(sql, coll).Rows[0][0].ToString();
            }
        }
        public int LeggiStatoCompilazioneCod(int pSelezioneId, string pUtente)
        {
            string sql;
            int CompId = RicercaCompilazione(pUtente, pSelezioneId, null);
            if (CompId == 0)
            {
                return 9;
            }
            else
            {
                sql = " select distinct s.stato from val_stati s, val_risposte r" +
                                        " where r.selezione_id = :selezione_id " +
                                        " and r.comp_id = :comp_id " +
                                        " and r.stato = s.stato";
                Dictionary<string, object> coll = new Dictionary<string, object>();
                coll.Add("selezione_id", pSelezioneId);
                coll.Add("comp_id", CompId);
                return int.Parse(accDB.sel_dati(sql, coll).Rows[0][0].ToString());
            }
        }
        public int LeggiStatoCompilazioneCod(int pSelezioneId, int pCompId)
        {
            string sql;
            if (pCompId == 0)
            {
                return 9;
            }
            else
            {
                sql = " select distinct s.stato from val_stati s, val_risposte r" +
                                        " where r.selezione_id = :selezione_id " +
                                        " and r.comp_id = :comp_id " +
                                        " and r.stato = s.stato";
                Dictionary<string, object> coll = new Dictionary<string, object>();
                coll.Add("selezione_id", pSelezioneId);
                coll.Add("comp_id", pCompId);
                if (accDB.sel_dati(sql, coll).Rows.Count > 0)
                    return int.Parse(accDB.sel_dati(sql, coll).Rows[0][0].ToString());
                else
                    return 9;
            }
        }
        public StatoValutazione AggiornaStatoValutazione(int pSelId, int pCompId, string pTipo)
        {
            /* 
             * RisposteVis / RisposteMod = visualizzazione / modifica campi delle risposte (val_risposte con tipo controllo != 6)
             * NoteVis / NoteMod = visualizzazione / modifica campi delle note (val_risposte con tipo controllo = 6)
             * ValutatazioniVis / ValutatazioniMod = visualizzazione / modifica campi delle valutazioni (val_valutazioni)
             * Valori possibili: D (Dipendente: TipoRiga=1) / R (Responsabile: TipoRiga=2) / A (Amministrazione: TipoRiga=3) / T (tutto) / N (niente)
            */
            string RisposteVis = "N";
            string RisposteMod = "N";
            string NoteVis = "N";
            string NoteMod = "N";
            string ValutazioniVis = "N";
            string ValutazioniMod = "N";
            bool Stampa = false;
            bool Chiusura = false;
            Selezione oSelezione = LeggiSelezione(pSelId);
            switch (oSelezione.Stato)
            {
                case 0:
                    break;
                case 1:
                    if (pCompId == 0)
                    {
                        if (pTipo == "com" && string.Compare(DateTime.Now.ToString("yyyy/MM/dd"), oSelezione.DataInizVal.ToString("yyyy/MM/dd")) != -1 && string.Compare(DateTime.Now.ToString("yyyy/MM/dd"), ((DateTime)oSelezione.DataTermPres).ToString("yyyy/MM/dd")) != 1)
                        {
                            RisposteVis = "D";
                            RisposteMod = "D";
                            Stampa = true;
                        }
                    }
                    else
                    {
                        switch (LeggiStatoCompilazioneCod(pSelId, pCompId))
                        {
                            case 10:
                                if (pTipo == "com" && string.Compare(DateTime.Now.ToString("yyyy/MM/dd"), oSelezione.DataInizVal.ToString("yyyy/MM/dd")) != -1 && string.Compare(DateTime.Now.ToString("yyyy/MM/dd"), ((DateTime)oSelezione.DataTermPres).ToString("yyyy/MM/dd")) != 1)
                                {
                                    RisposteVis = "D";
                                    RisposteMod = "D";
                                    Stampa = true;
                                    Chiusura = true;
                                }
                                break;
                            case 15:
                                if (pTipo == "amm" && string.Compare(DateTime.Now.ToString("yyyy/MM/dd"), ((DateTime)oSelezione.DataTermCtrlAmm).ToString("yyyy/MM/dd")) != 1)
                                {
                                    RisposteVis = "T";
                                    RisposteMod = "D";
                                    NoteVis = "A";
                                    NoteMod = "A";
                                    ValutazioniVis = "A";
                                    ValutazioniMod = "A";
                                    Stampa = true;
                                    Chiusura = true;
                                }
                                else if (pTipo == "com")
                                {
                                    RisposteVis = "D";
                                    Stampa = true;
                                }
                                else if (pTipo == "vis") // il dipendente ha trasmesso la domanda all'ufficio --> il direttore può vedere le mie risposte 
                                {
                                    RisposteVis = "T";
                                    Stampa = true;
                                }
                                break;
                            case 20:
                                if (pTipo == "com")
                                {
                                    RisposteVis = "D";
                                    NoteVis = "A";
                                    Stampa = true;
                                }
                                else if (pTipo == "amm")
                                {
                                    RisposteVis = "T";
                                    NoteVis = "A";
                                    Stampa = true;
                                }
                                else if (pTipo == "val" && string.Compare(DateTime.Now.ToString("yyyy/MM/dd"), ((DateTime)oSelezione.DataTermCtrlAmm).ToString("yyyy/MM/dd")) != -1 && string.Compare(DateTime.Now.ToString("yyyy/MM/dd"), ((DateTime)oSelezione.DataTermValResp).ToString("yyyy/MM/dd")) != 1)
                                {
                                    RisposteVis = "T";
                                    RisposteMod = "R";
                                    NoteVis = "T";
                                    NoteMod = "R";
                                    ValutazioniVis = "R";
                                    ValutazioniMod = "R";
                                    Stampa = true;
                                    Chiusura = true;
                                }
                                else if (pTipo == "vis") // finito il controllo ufficio, domanda trasmessa al responsabile --> 
                                // il direttore può vedere le mie risposte e le note dell'ufficio amministrativo >>ga29052012<<
                                {
                                    RisposteVis = "T";
                                    NoteVis = "A";
                                    ValutazioniVis = "A";
                                    Stampa = true;
                                }
                                break;
                            case 40:
                                if (pTipo == "com" && string.Compare(DateTime.Now.ToString("yyyy/MM/dd"), ((DateTime)oSelezione.DataTermValResp).ToString("yyyy/MM/dd")) != -1 && string.Compare(DateTime.Now.ToString("yyyy/MM/dd"), ((DateTime)oSelezione.DataTermCtrlDip).ToString("yyyy/MM/dd")) != 1)
                                {
                                    RisposteVis = "T";
                                    NoteVis = "T";
                                    NoteMod = "D";
                                    ValutazioniVis = "T";
                                    Stampa = true;
                                    Chiusura = true;
                                }
                                else if (pTipo == "amm" || pTipo == "val")
                                {
                                    RisposteVis = "T";
                                    NoteVis = "T";
                                    ValutazioniVis = "T";
                                    Stampa = true;
                                }
                                else if (pTipo == "vis") // finita la valutazione del responsabile, domanda trasmessa al dipendente --> 
                                // il direttore può vedere le mie risposte e le note dell'ufficio amministrativo, le note del responsabile 
                                {
                                    RisposteVis = "T";
                                    NoteVis = "T";
                                    ValutazioniVis = "T";
                                    Stampa = true;
                                }
                                break;
                            case 50:
                                if (pTipo == "amm" && string.Compare(DateTime.Now.ToString("yyyy/MM/dd"), ((DateTime)oSelezione.DataFineVal).ToString("yyyy/MM/dd")) != 1)
                                {
                                    RisposteVis = "T";
                                   // RisposteMod = "R"; // >>ga24102012<< per poter integrare i giudizi dei mancanti dei responsabili // revoca anno 2016 >>ga14092016<<
                                    RisposteMod = "A";   //>>ga14092016<<
                                    NoteVis = "T";
                                    NoteMod = "A";
                                    ValutazioniVis = "T";
                                    //ValutazioniMod = "A"; //>>ga16102012<<
                                    ValutazioniMod = "T";
                                    Stampa = true;
                                    Chiusura = true;
                                }
                                else if (pTipo == "com" || pTipo == "val")
                                {
                                    RisposteVis = "T";
                                    NoteVis = "T";
                                    ValutazioniVis = "T";
                                    Stampa = true;
                                }
                                else if (pTipo == "vis") // finita la valutazione del responsabile, domanda trasmessa al dipendente --> 
                                // il direttore può vedere le mie risposte e le note dell'ufficio amministrativo, le note del responsabile 
                                {
                                    RisposteVis = "T";
                                    NoteVis = "T";
                                    ValutazioniVis = "T";
                                    Stampa = true;
                                }
                                break;
                            case 90:
                                if (pTipo == "amm")
                                {
                                    RisposteVis = "T";
                                    NoteVis = "T";
                                    ValutazioniVis = "T";
                                    Stampa = true;
                                }
                                else
                                {
                                    RisposteVis = "T";
                                    NoteVis = "T";
                                    ValutazioniVis = "T";
                                    Stampa = true;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case 8:
                    break;
                case 99:  //>>ga24072013<<
                    RisposteVis = "T";
                    NoteVis = "T";
                    ValutazioniVis = "T";
                    Stampa = true;
                    break;
                default:
                    break;
            }
            StatoValutazione oStatoValutazione = new StatoValutazione();
            oStatoValutazione.RisposteVis = RisposteVis;
            oStatoValutazione.RisposteMod = RisposteMod;
            oStatoValutazione.NoteVis = NoteVis;
            oStatoValutazione.NoteMod = NoteMod;
            oStatoValutazione.ValutazioniVis = ValutazioniVis;
            oStatoValutazione.ValutazioniMod = ValutazioniMod;
            oStatoValutazione.Stampa = Stampa;
            oStatoValutazione.Chiusura = Chiusura;
            return oStatoValutazione;
        }
        public StatoArchivio AggiornaStatoArchivio(clsUtenteLogin pUtenteLogin, int? pAnno)
        {
            bool InArchivio = false;
            StatoArchivio oStatoArchivio = new StatoArchivio();

            if (pUtenteLogin.Archivio == 1)
                InArchivio = true;
            oStatoArchivio.InArchivio = InArchivio;

            return oStatoArchivio;
        }
        public IList<IndicatoreDetRiga> LeggiIndicatoriDetRigaComp(int pSelId, string sCatcod, int? pIndId, int? pIndDetId, int? pTipoRiga, int? pRigaId, StatoValutazione pStatoValutazione)
        {
            Dictionary<String, Object> coll;
            coll = new Dictionary<string, object>();
            String sql = "select SELEZIONE_ID, CATEGORIA_COD, IND_ID, " +
                         " IND_DET_ID, a.TIPO_RIGA, RIGA_ID, ORD, a.TIPO_CTRL, " +
                         " a.DESCR, USR_INS, DATA_INS, USR_MOD, DATA_MOD, " +
                         " b.DESCR DESCR_TIPO_RIGA, c.DESCR DESCR_TIPO_CTRL " +
                         " from val_indicatori_det_riga a, " +
                         " val_tipi_riga b, val_tipi_ctrl c" +
                         " where selezione_id = :pSelId " +
                         " and a.tipo_riga = b.tipo_riga " +
                         " and a.tipo_ctrl = c.tipo_ctrl ";
            coll.Add("selezione_id", pSelId);
            if (sCatcod != null)
            {
                sql += " and categoria_cod = :sCatcod";
                coll.Add("categoria_cod", sCatcod);
            }
            if (pIndId != null)
            {
                sql += " and ind_id = :pIndId";
                coll.Add("ind_id", pIndId);
            }
            if (pIndDetId != null)
            {
                sql += " and ind_det_id = :pIndDetId";
                coll.Add("ind_det_id", pIndDetId);
            }
            if (pTipoRiga != null)
            {
                sql += " and a.tipo_riga = :pTipoRiga";
                coll.Add("tipo_riga", pTipoRiga);
            }
            if (pRigaId != null)
            {
                sql += " and riga_id = :pRigaId";
                coll.Add("riga_id", pRigaId);
            }
            if (pStatoValutazione != null)
            {
                if (pStatoValutazione.RisposteVis == "D")
                    sql += " and a.tipo_riga = 1";
                else if (pStatoValutazione.RisposteVis == "R")
                    sql += " and a.tipo_riga = 2";
                else if (pStatoValutazione.RisposteVis == "A")
                    sql += " and a.tipo_riga = 3";
            }
            sql += " order by selezione_id, categoria_cod, ind_id, ind_det_id, b.descr, tipo_riga, ord, riga_id";
            IList<IndicatoreDetRiga> lIndicatoreDetRiga = new List<IndicatoreDetRiga>();
            IndicatoreDetRiga oIndicatoreDetRiga;
            DataTable dt;
            dt = accDB.sel_dati(sql, coll);
            foreach (DataRow riga in dt.Rows)
            {
                oIndicatoreDetRiga = new IndicatoreDetRiga();
                oIndicatoreDetRiga.SelezioneId = Int32.Parse(riga["selezione_id"].ToString());
                oIndicatoreDetRiga.CategoriaCod = riga["categoria_cod"].ToString();
                oIndicatoreDetRiga.IndId = Int32.Parse(riga["ind_id"].ToString());
                oIndicatoreDetRiga.IndDetId = Int32.Parse(riga["ind_det_id"].ToString());
                oIndicatoreDetRiga.TipoRiga = Int32.Parse(riga["tipo_riga"].ToString());
                oIndicatoreDetRiga.RigaId = Int32.Parse(riga["riga_id"].ToString());
                oIndicatoreDetRiga.OrdRiga = Int32.Parse(riga["ord"].ToString());

                oIndicatoreDetRiga.TipoCtrl = Int32.Parse(riga["tipo_ctrl"].ToString());
                oIndicatoreDetRiga.DescrRiga = riga["descr"].ToString();
                oIndicatoreDetRiga.DescrTipoRiga = riga["descr_tipo_riga"].ToString();
                oIndicatoreDetRiga.DescrTipoCtrl = riga["descr_tipo_ctrl"].ToString();
                if (pStatoValutazione.RisposteVis != "N")
                    lIndicatoreDetRiga.Add(oIndicatoreDetRiga);
            }
            return lIndicatoreDetRiga;
        }
        public clsUsrData LeggiUsrData(int pSelId, string pCatCod, int pIndId, int pIndDetId, int? pRispId, int? pTipoRiga, int? pRigaId, int pCompId, string pParam)
        {
            Dictionary<String, Object> coll = new Dictionary<String, Object>();
            clsUsrData oclsUsrData = new clsUsrData();
            string sql = "";
            //Cerco nelle risposte
            if (pParam == "ris")
            {
                sql = " select distinct usr_ins, data_ins from val_risposte " +
                             " where selezione_id = :selezione_id " +
                             " and categoria_cod = :categoria_cod" +
                             " and ind_id = :ind_id " +
                             " and ind_det_id = :ind_det_id " +
                             " and risp_id = :risp_id " +
                             " and tipo_riga = :tipo_riga " +
                             " and riga_id = :riga_id " +
                             " and comp_id = :comp_id";
                coll.Add("selezione_id", pSelId);
                coll.Add("categoria_cod", pCatCod);
                coll.Add("ind_id", pIndId);
                coll.Add("ind_det_id", pIndDetId);
                coll.Add("risp_id", pRispId);
                coll.Add("tipo_riga", pTipoRiga);
                coll.Add("riga_id", pRigaId);
                coll.Add("comp_id", pCompId);
            }
            else
            {
                sql = " select distinct usr_ins, data_ins from val_valutazioni " +
                             " where selezione_id = :selezione_id " +
                             " and categoria_cod = :categoria_cod" +
                             " and ind_id = :ind_id " +
                             " and ind_det_id = :ind_det_id " +
                             " and comp_id = :comp_id";
                coll.Add("selezione_id", pSelId);
                coll.Add("categoria_cod", pCatCod);
                coll.Add("ind_id", pIndId);
                coll.Add("ind_det_id", pIndDetId);
                coll.Add("comp_id", pCompId);
            }

            DataTable dt = accDB.sel_dati(sql, coll);
            if (dt.Rows.Count > 0)
            {
                oclsUsrData.UsrIns = dt.Rows[0]["usr_ins"].ToString();
                if (dt.Rows[0]["data_ins"] != null)
                    oclsUsrData.DataIns = DateTime.Parse(dt.Rows[0]["data_ins"].ToString());
            }
            return oclsUsrData;
        }
        public int rec_opid(clsTransaction p_clsTrx)
        {
            String sql = "SELECT val_op_id.nextval  FROM dual";
            Dictionary<String, Object> coll = new Dictionary<String, Object>();
            if (p_clsTrx == null)
            {
                return int.Parse(accDB.sel_dati(sql, null).Rows[0][0].ToString());
            }
            else
            {
                return int.Parse(p_clsTrx.sel_dati(sql, null).Rows[0][0].ToString());
            }
        }
        public int ins_log(CompRisposte pCompRisposte, string pTipoOp, string pUtente, clsTransaction p_clsTrx)
        {
            Dictionary<String, Object> coll;
            clsTransaction clsTrx = p_clsTrx;
            int OpId = 0;
            if (clsTrx == null)
            {
                clsTrx = new clsTransaction();
            }
            try
            {
                OpId = rec_opid(null);
                foreach (Risposta item in pCompRisposte.RigheRisposta)
                {
                    coll = new Dictionary<String, Object>();
                    coll.Add("op_id", OpId);
                    coll.Add("comp_id", pCompRisposte.CompId);
                    coll.Add("selezione_id", item.SelezioneId);
                    coll.Add("categoria_cod", item.CategoriaCod);
                    coll.Add("ind_id", item.IndId);
                    coll.Add("ind_det_id", item.IndDetId);
                    coll.Add("risp_id", item.RispId);
                    coll.Add("tipo_riga", item.TipoRiga);
                    coll.Add("riga_id", item.RigaId);
                    coll.Add("risp", item.Risp);
                    coll.Add("matri_dip", pCompRisposte.MatriDip);
                    coll.Add("matri_rsp", pCompRisposte.MatriRsp);
                    coll.Add("stato", pCompRisposte.Stato);
                    coll.Add("usr_ins", item.UsrIns);
                    coll.Add("data_ins", item.DataIns);
                    coll.Add("usr_mod", item.UsrMod);
                    coll.Add("data_mod", item.DataMod);
                    coll.Add("tipo_op", pTipoOp);
                    coll.Add("usr_log_ins", pUtente);
                    coll.Add("data_log_ins", System.DateTime.Now);
                    clsTrx.insert_row_trx("val_risposte_log", coll);
                }
            }
            catch (Exception ex)
            {
                if (p_clsTrx == null)
                {
                    clsTrx.rollback_trx();
                    clsTrx = null;
                }
                throw ex;
            }
            if (p_clsTrx == null)
            {
                clsTrx.commit_trx();
                clsTrx = null;
            }
            return OpId;
        }
        public int del_risp_nota_log(int pSelezioneId, string pCategoriaCod, int pCompId, int pIndId, int pIndDetId, int? pTipoRiga, string pUtente, clsTransaction p_clsTrx)
        {
            clsTransaction clsTrx = p_clsTrx;
            int OpId = 0;
            if (clsTrx == null)
            {
                clsTrx = new clsTransaction();
            }
            try
            {
                CompRisposte oCompRisposte;
                if (pTipoRiga != null && pTipoRiga != 0)
                    oCompRisposte = LeggiRisposte(pCompId, pSelezioneId, pCategoriaCod, pIndId, pIndDetId, null, pTipoRiga, null, p_clsTrx);
                else
                    oCompRisposte = LeggiRisposte(pCompId, pSelezioneId, pCategoriaCod, pIndId, pIndDetId, null, null, null, p_clsTrx);
                OpId = ins_log(oCompRisposte, "DELETE", pUtente, p_clsTrx);
            }
            catch (Exception ex)
            {
                if (p_clsTrx == null)
                {
                    clsTrx.rollback_trx();
                    clsTrx = null;
                }
                throw ex;
            }
            if (p_clsTrx == null)
            {
                clsTrx.commit_trx();
                clsTrx = null;
            }
            return OpId;
        }
        public void ins_compilazioneFile_log(CompFile pCompFile, int pOpId, string pTipoOp, string pUtente, clsTransaction p_clsTrx)
        {
            Dictionary<String, Object> coll;
            coll = new Dictionary<String, Object>();
            clsTransaction clsTrx = p_clsTrx;
            if (clsTrx == null)
            {
                clsTrx = new clsTransaction();
            }
            try
            {
                coll.Add("op_id", pOpId);
                coll.Add("comp_id", pCompFile.CompId);
                coll.Add("selezione_id", pCompFile.SelezioneId);
                coll.Add("categoria_cod", pCompFile.CategoriaCod);
                coll.Add("ind_id", pCompFile.IndId);
                coll.Add("ind_det_id", pCompFile.IndDetId);
                coll.Add("risp_id", pCompFile.RispId);
                coll.Add("descrizione", pCompFile.Descrizione);
                coll.Add("allegato_log", pCompFile.Blob);
                coll.Add("bytes", pCompFile.Bytes_curr);
                coll.Add("ext", pCompFile.Ext_curr);
                coll.Add("tipo_op", pTipoOp);
                coll.Add("usr_ins", pCompFile.UsrIns);
                coll.Add("data_ins", pCompFile.DataIns);
                coll.Add("usr_mod", pCompFile.UsrMod);
                coll.Add("data_mod", pCompFile.DataMod);
                coll.Add("usr_log_ins", pUtente);
                coll.Add("data_log_ins", System.DateTime.Now);
                clsTrx.insert_row_trx("val_allegati_log", coll);

            }
            catch (Exception ex)
            {
                if (p_clsTrx == null)
                {
                    clsTrx.rollback_trx();
                    clsTrx = null;
                }
                throw ex;
            }
            if (p_clsTrx == null)
            {
                clsTrx.commit_trx();
                clsTrx = null;
            }
        }
        //>>ga28082012<< inizio
        public DataTable EstraiCompilazioni(int pSelId, string pCatCod, int pStato)
        {
            //>>ga102015<<
            // errore estrazione per leggere correttamente la tabella val_dip è necessario conoscere l'anno della peo
            DataTable dt = new DataTable();
            Dictionary<string, object> coll = new Dictionary<string, object>();
            coll.Add("selezione_id", pSelId);
            coll.Add("categoria_cod", pCatCod);
            coll.Add("stato", pStato);

            String sql = "select distinct r.comp_id , r.matri_dip, d.inquadr_def, w.nome_dip , w.cognome_dip " +
                //" from val_risposte r, val_dip d, val_resp_dip_vw w " +  // >>ga102015<<
                " from val_risposte r, val_dip d, val_resp_dip_vw w , val_selezioni s" +    // >>ga102015<<
                " where r.selezione_id = :selezione_id" +
                " and r.categoria_cod = :categoria_cod" +
                " and r.stato >= :stato" +
                " and r.selezione_id = s.selezione_id " + //>>ga102015<<
                " and r.matri_dip = d.matri_def " +
                " and d.anno = s.anno " + //>>ga102015<<
                " and w.matri_dip = r.matri_dip "
                ;

            dt = accDB.sel_dati(sql, coll);
            return dt;
        }
        public float EstraiPunteggioPerIndicatore(int pSelId, string pCatCod, int pCompId, int pIndId)
        {
            DataTable dt = new DataTable();
            float punteggio = 0;
            Dictionary<string, object> coll = new Dictionary<string, object>();
            coll.Add("selezione_id", pSelId);
            coll.Add("categoria_cod", pCatCod);
            coll.Add("comp_id", pCompId);
            coll.Add("ind_id", pIndId);
            string sql = "select sum(PUNTEGGIO)" +
                  " from VAL_VALUTAZIONI " +
                  " where selezione_id = :selezione_id " +
                  " and categoria_cod = :categoria_cod " +
                  " and comp_id = :comp_id " +
                  " and ind_id = :ind_id";

            dt = accDB.sel_dati(sql, coll);
            if (dt.Rows[0][0].ToString() != "")
            {
                punteggio = float.Parse(dt.Rows[0][0].ToString());
            }
            return punteggio;
        }
        public int TipoInd(int pSelId, string pCatCod, int pIndId)
        {
            DataTable dt = new DataTable();
            int TipoIndicatore = 0;
            Dictionary<string, object> coll = new Dictionary<string, object>();
            coll.Add("selezione_id", pSelId);
            coll.Add("categoria_cod", pCatCod);
            coll.Add("ind_id", pIndId);
            string sql = "select tipo_ctrl, count(*)" +
                  " from val_indicatori_det_riga " +
                  " where selezione_id = :selezione_id " +
                  " and categoria_cod = :categoria_cod " +
                  " and ind_id = :ind_id" +
                  " group by tipo_ctrl";

            dt = accDB.sel_dati(sql, coll);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow riga in dt.Rows)
                {
                    if (int.Parse(riga["tipo_ctrl"].ToString()) == 6)
                        TipoIndicatore = 6;
                }
            }
            return TipoIndicatore;
        }

        //public DataTable EstraiElencoFinale(int pAnno, string pCategoria)
        public DataTable EstraiElencoFinale(int pSelezioneId, int pAnno)
        {
            IList<Indicatore> lIndicatore = new List<Indicatore>();

            IList<EstrazioneElencoFinale> lEstrazione = new List<EstrazioneElencoFinale>();
            EstrazioneElencoFinale oEstrazione = new EstrazioneElencoFinale();
            IList<Selezione> lSelezione = new List<Selezione>();
            if (pSelezioneId != 0 && pAnno == 0)
                lSelezione = LeggiSelezioni(null, null, null, pSelezioneId);
            else if (pSelezioneId == 0 && pAnno != 0)
                lSelezione = LeggiSelezioni(null, pAnno.ToString(), null, 0);

            DataTable dtCompilazioni = new DataTable();
            foreach (Selezione oSelezione in lSelezione)
            {
                DataTable dtTemp = new DataTable();
                lIndicatore = LeggiIndicatori(oSelezione.SelezioneId, oSelezione.CategoriaCod, null);
                // elimino gli indicatori di tipo 6- note
                //foreach (Indicatore oIndicatore in lIndicatore)
                //{
                //    if (TipoInd(oSelezione.SelezioneId, oSelezione.CategoriaCod, int.Parse(oIndicatore.IndId.ToString())) == 6)
                //    {
                //        lIndicatore.Remove(oIndicatore);
                //    }
                //}
                oEstrazione.SelezioneId = oSelezione.SelezioneId;
                oEstrazione.CategoriaCod = oSelezione.CategoriaCod;
                // estraggo solo le compilazioni che sono almeno state inoltrate all'ufficio
                //dtCompilazioni = EstraiCompilazioni(oSelezione.SelezioneId, oSelezione.CategoriaCod, 15);
                dtTemp = EstraiCompilazioni(oSelezione.SelezioneId, oSelezione.CategoriaCod, 15);
                foreach (Indicatore oIndicatore in lIndicatore)
                {
                    //dtCompilazioni.Columns.Add("ind_id-" + oIndicatore.IndId.ToString());
                    dtTemp.Columns.Add("ind_id-" + oIndicatore.IndId.ToString());
                }
                //dtCompilazioni.Columns.Add("Punteggio Totale");
                dtTemp.Columns.Add("Punteggio Totale");

                //foreach (DataRow dr in dtCompilazioni.Rows)
                foreach (DataRow dr in dtTemp.Rows)
                {

                    Decimal PuntTotale = 0;
                    int indicatore = 0;
                    //foreach (DataColumn dc in dtCompilazioni.Columns)
                    foreach (DataColumn dc in dtTemp.Columns)
                    {
                        if (dc.ColumnName.Substring(0, 7) == "ind_id-")
                        {
                            indicatore = 0;
                            indicatore = int.Parse(dc.ColumnName.Substring(7).ToString());

                            dr[dc.Ordinal] = EstraiPunteggioPerIndicatore(oSelezione.SelezioneId, oSelezione.CategoriaCod, int.Parse(dr["comp_id"].ToString()), indicatore);
                            PuntTotale = PuntTotale + Decimal.Parse(dr[dc.Ordinal].ToString());
                        }
                        if (dc.ColumnName.Substring(0) == "Punteggio Totale")
                        {
                            dr[dc.Ordinal] = PuntTotale;
                        }


                    }

                }
                dtCompilazioni.Merge(dtTemp);
            }
            // ricerca descrizione indicatore
            foreach (DataColumn dc in dtCompilazioni.Columns)
            {
                if (dc.ColumnName.Substring(0, 7) == "ind_id-")
                {
                    int tInd = int.Parse(dc.ColumnName.Substring(7).ToString());
                    foreach (Indicatore oIndicatore in lIndicatore)
                    {
                        if (oIndicatore.IndId == tInd)
                        {
                            dc.ColumnName = oIndicatore.Descr.ToString();
                        }
                    }
                }
            }
            return dtCompilazioni;
        }
        //>>ga28082012<< fine

        //>>ga082016<< inizio
        //richiesta revisione punteggi
        // il dipendente può inviare una solo email
        public void ins_ricrev(Dictionary<string, object> p_coll_s)
        {
            clsTransaction clsTrx = new clsTransaction();
            try
            {
                accDB.insert_row("val_revisionepunteggi", p_coll_s);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void del_ricrev(Dictionary<string, object> p_coll_s)
        {
            clsTransaction clsTrx = new clsTransaction();
            try
            {
                accDB.del_row("val_revisionepunteggi", p_coll_s);
            }
            catch (Exception ex)
            {
             throw ex;
            }
        }
        public DataTable EstraiRichiestaRevisione(int pSelId, string pCategoriaCod, int pCompId, string pUtente)
        {
            //try
            //{
                String sql = "select testoemail, data_ins from val_revisionepunteggi" +
                            " where selezione_id = :selezione_id" +
                            "   and categoria_cod = :categoria_cod" +
                            "   and comp_id = :comp_id" +
                            "   and usr_ins = :usr_ins";

                DataTable dt = new DataTable();
                Dictionary<string, object> coll = new Dictionary<string, object>();
                coll.Add("selezione_id", pSelId);
                coll.Add("categoria_cod", pCategoriaCod);
                coll.Add("comp_id", pCompId);
                coll.Add("usr_ins", pUtente);
                try
                {
                    dt = accDB.sel_dati(sql, coll);
                    return dt;


                }
                catch (Exception)
                {
                    throw new CustomExceptions.OperazioneFallita("Applicazione Verifica esistenza richiesta revisione: estrazione dati tabella val_revisionepunteggi FALLITA. Avvisa I.S.I.");
                }

            //}
            //catch (CustomExceptions.InserimentoRichiestaRevisione ex)
            //{
           //     throw ex;
            //}
            
        }
        public string spedisci_mailRevisione(Selezione tmpSelezione, int tmpCompId, clsUtenteLogin tmpUtenteDip, string tmpRevision, string tmpEmailStruttura)
        {
            //tmpEmailStruttura = "giovanna.aulenti@amm.units.it"; //DA USARE PER PROVE

            string sReturn;
            // mandare mail
            MailMessage mail = new MailMessage();
            String EmailDip = "";
            String EmailStruttura = tmpEmailStruttura;

            String osservazioni = "";

            String Matricola = tmpUtenteDip.UserId;
            String NomeDipendente = tmpUtenteDip.Nome + " " + tmpUtenteDip.Cognome;



            if (tmpUtenteDip.Email != "")
                EmailDip = tmpUtenteDip.Email;
            else
            {
                EmailDip = tmpEmailStruttura;
                osservazioni = " Il dipendente non ha una email associata </br> ";
            }

            
            mail.From = new MailAddress(EmailDip);
            // destinatari
            mail.To.Add(new MailAddress(EmailDip));
            mail.To.Add(new MailAddress(EmailStruttura));



            // oggetto
            mail.Subject = "Richiesta revisione punteggi. Nr. Compilazione " + tmpCompId + " - " + NomeDipendente + " " + Matricola;
            // mail.SubjectEncoding = System.Text.Encoding.GetEncoding("iso-8859-15");
            //corpo
            mail.Body = "PEO: " + tmpSelezione.SelezioneId + " Anno " + tmpSelezione.Anno + " Categoria " + tmpSelezione.CategoriaCod + "<br />" +
                        "Compilazione ID: " + tmpCompId + "<br />" +
                        "Matricola: " + Matricola + "<br />" +
                        "Cognome Nome: " + NomeDipendente + "<br />" + "<br />" +
                        tmpRevision + "<br />" + "<br />" + "<br />" + "<br />" +
                        osservazioni;

            mail.Body = mail.Body + "<br />" + "<br />" + "Cordiali saluti";
            mail.IsBodyHtml = true;

            //----> INVIO <------------------
            SmtpClient server = new SmtpClient();
            server.Host = "SMTP.UNITS.IT";
            try
            {
                server.Send(mail);
                sReturn = "E-Mail inviata con successo";
                return sReturn;
            }
            catch (Exception ex)
            {
                sReturn = "Invio E-Mail fallito";
                return sReturn;
            }

        }
        //>>ga082016<< fine
        public void InviaEmail(clsUtenteLogin pUtenteDip, Selezione pSelezione, string pDescrStato, int oCompId, int oCodStatoSel)
        {
            string sReturn = "";
            // invio  mail
            MailMessage mail = new MailMessage();
            String mailFrom = "";
            String mailTo = "";
            if (oCodStatoSel != 40)
            {
                //pUtenteDip.Email = null;

                //>>ga07102015<< controllo esistenza email dipendente e responsabile
                //if (pUtenteDip.Email != null || pUtenteDip.Email != "")
                if (!String.IsNullOrEmpty(pUtenteDip.Email))
                {
                    mailFrom = pUtenteDip.Email;
                }
                else
                {
                    mailFrom = "rel.sindacali@amm.units.it";
                }
                mail.From = new MailAddress(mailFrom);
            }
            else
            {
                if (!String.IsNullOrEmpty(pUtenteDip.EmailResp))
                {
                    mailFrom = pUtenteDip.EmailResp;
                }
                else
                {
                    mailFrom = "rel.sindacali@amm.units.it";
                }

                mail.From = new MailAddress(mailFrom);
            }
            //mailTo = pUtenteDip.Email;
            if (!String.IsNullOrEmpty(pUtenteDip.Email))
            {
                mailTo = pUtenteDip.Email;
            }
            else
            {
                mailTo = "rel.sindacali@amm.units.it";
            }
            mail.To.Add(new MailAddress(mailTo));
            // da stato 10 a stato 15 (sono nello stato 15): il dipendente ha inviato la selezione all'amministrazione --> email solo dipendente
            // da stato 15 a stato 20 (sono nello stato 20): l'amministrazione ha inviato la selezione al responsabile --> nessuna email
            // da stato 20 a stato 40 (sono nello stato 40): il responsabile  ha inviato la selezione al dipedente --> email responsabile e dipendente
            // da stato 40 a stato 50 (sono nello stato 50): il dipendente ha inviato la selezione all'amministrazione --> email solo dipendente
            switch (oCodStatoSel)
            {
                case 40:
                    mailTo = pUtenteDip.EmailResp;
                    mail.To.Add(new MailAddress(mailTo));
                    break;
                default:
                    break;
            }

            mail.Subject = "Invio compilazione PEO " + pSelezione.Anno + " - " + pSelezione.CategoriaCod;

            //corpo
            mail.Body = "<strong>" + "PEO: " + pSelezione.Titolo + " " + pSelezione.Anno + " - " + pSelezione.CategoriaCod + "<br />" + "<br />" +
                        "Selezione trasmessa con successo" +
                        "<br />" + "<br />" + "</strong>" +
                        "Compilazione ID: " + oCompId + "<br />" +
                        "User: " + pUtenteDip.UserId + "<br />" +
                        "Cognome Nome: " + pUtenteDip.Cognome + " " + pUtenteDip.Nome + "<br />" +
                        "Stato Attuale Selezione: " + pDescrStato;
            if (pUtenteDip.Email == null)
            {
                mail.Body = mail.Body + "<br />" + "Attenzione, l'utente non ha ancora comunicato la propria Email";
            }
            if (pUtenteDip.EmailResp == null)
            {
                mail.Body = mail.Body + "<br />" + "Attenzione, il responsabile non ha ancora comunicato la propria Email";
            }
            mail.Body = mail.Body + "<br />" + "<br />" + "Cordiali saluti";
            //mail.BodyEncoding = System.Text.Encoding.GetEncoding("iso-8859-15");
            mail.IsBodyHtml = true;
 

            //----> INVIO <------------------
            SmtpClient server = new SmtpClient();

            server.Host = "SMTP.UNITS.IT";
            try
            {
                server.Send(mail);
                sReturn = "E-Mail inviata con successo";
                //return sReturn;
            }
            catch (Exception ex)
            {
                sReturn = "Invio E-Mail fallito";
                //return sReturn;
            }


        }

        //>>ga2013agosto<< inizio
        // controllo punteggi / motivazioni dei responsabili

        public bool controlli_responsabile(int pSelId, int pCompId)
        {
            DataTable dt = new DataTable();
            DataTable tmpDT = new DataTable();
            DataTable dtPunt = new DataTable();
            DataTable dtPunteggioAssegnato = new DataTable();
            DataTable dtRispId = new DataTable();
            Dictionary<string, object> coll = new Dictionary<string, object>();
            bool esito = true;
           
            coll.Add("selezione_id", pSelId);
                        
            String sql =   " select distinct i.ind_id, r.ind_det_id, i.dip_flg, i.rsp_flg, i.amm_flg " +
                           " from val_indicatori i, val_indicatori_det_riga r " +
                           " where i.selezione_id = :pSelId " +
                           " and i.rsp_flg = 1 " +
                           " and i.selezione_id = r.selezione_id " +
                           " and i.categoria_cod = r.categoria_cod " +
                           " and i.ind_id = r.ind_id " +
                           " and r.tipo_riga = 2 " +
                           " and r.tipo_ctrl <> 6" 
                            ;

            dt = accDB.sel_dati(sql, coll);
            int tmpDipFlg = 0;
            int nRighe = 0;
            foreach (DataRow dr in dt.Rows)
            {
                tmpDipFlg = int.Parse(dr["dip_flg"].ToString());
                Dictionary<string, object> tmpColl = new Dictionary<string, object>();
                tmpColl.Add("selezione_id", pSelId);
                tmpColl.Add("ind_id", int.Parse(dr["ind_id"].ToString()));
                tmpColl.Add("ind_det_id", int.Parse(dr["ind_det_id"].ToString()));
                tmpColl.Add("comp_id", pCompId);
                if (tmpDipFlg == 0)
                {
                    // deve esistere la valutazione del responsabile
                    string tmpSql = "select count(*) as numrighe from val_risposte " +
                                    " where selezione_id = :pSelId " +
                                    " and ind_id = :ind_id " +
                                    " and ind_det_id = :ind_det_id" +
                                    " and comp_id = :pcompId";
                    tmpDT = accDB.sel_dati(tmpSql, tmpColl);

                    nRighe = int.Parse(tmpDT.Rows[0]["numrighe"].ToString());
                    if (nRighe == 0)
                    {
                        esito = false;
                        continue;
                    }
                    else // verifico se deve esistere anche un punteggio
                    {
                        tmpColl.Remove("comp_id");
                        string PunteggioIndSql = "select selezione_id, ind_id, ind_det_id, punt_id, punt" +
                                                 " from val_indicatori_det_punteggi " +
                                                 " where selezione_id = :pSelId " +
                                                 " and ind_id = :ind_id " +
                                                 " and ind_det_id = :ind_det_id";
                        dtPunt = accDB.sel_dati(PunteggioIndSql, tmpColl);
                        tmpColl.Add("comp_id", pCompId);
                        int nPunteggi = dtPunt.Rows.Count;
                        if (nPunteggi > 0)
                        {
                            // cerco il punteggio nella tabella val_valutazioni
                            
                            string PunteggioAssegnatoSql = "select punteggio" +
                                                 " from val_valutazioni " +
                                                 " where selezione_id = :pSelId " +
                                                 " and ind_id = :ind_id " +
                                                 " and ind_det_id = :ind_det_id" +
                                                 " and comp_id = :pcompId";
                            dtPunteggioAssegnato = accDB.sel_dati(PunteggioAssegnatoSql, tmpColl);
                            bool esitoPunteggio = false;
                            // foreach (DataRow drPA in dtPunteggioAssegnato.Rows)
                            // {
                            if (dtPunteggioAssegnato.Rows.Count > 0) //>>ga07102015<<
                            {                                        //>>ga07102015<<
                                float PuntAssegnato = float.Parse(dtPunteggioAssegnato.Rows[0]["punteggio"].ToString());
                                foreach (DataRow drPunt in dtPunt.Rows)
                                {
                                    if (float.Parse(drPunt["punt"].ToString()) == PuntAssegnato)
                                        esitoPunteggio = true;
                                }
                            }
                            
                                //}
                            if (!esitoPunteggio)
                                esito = false;

                        }
                    }

                }
                else // domanda a cui devono rispondere sia il dipendente che il responsabile
                    // verifico se il dipendente ha risposto alla domanda e quante risposte ha dato
                    // verifico se il responsabile ha risposto alla domanda e se ha dato la valutazione su tutte le risposte della domanda
                {

                    string SqlRispId = "select distinct risp_id from val_risposte " +
                                    " where selezione_id = :pSelId " +
                                    " and ind_id = :ind_id " +
                                    " and ind_det_id = :ind_det_id" +
                                    " and comp_id = :pcompId";
                    dtRispId = accDB.sel_dati(SqlRispId, tmpColl);
                    foreach (DataRow drRispId in dtRispId.Rows)
                    {
                        // lettura delle risposte di tipo 2 (del responsabile)
                        CompRisposte ocompRisposta = LeggiRisposte(pCompId, pSelId, "", int.Parse(dr["ind_id"].ToString()), int.Parse(dr["ind_det_id"].ToString()), int.Parse(drRispId["risp_id"].ToString()), 2, null, null);
                        if (ocompRisposta.RigheRisposta.Count == 0)
                            esito = false;
                    }

                }
                
            }
            return esito;
        }
        //>>ga2013agosto<< fine

        //>>ga2013dicembre --> aggiunto il 11032015
        // gestione file
        public DataTable CaricaIstruzioniInserite(string pTipoSel, int pAnno)
        {
            Dictionary<String, Object> coll;
            coll = new Dictionary<string, object>();
            coll.Add("selezione_cod", pTipoSel);
            coll.Add("anno", pAnno);
            String sql = " select  id, selezione_cod, anno, istruzioni, titolo, ext, bytes, descrizione, titolo_file " +
                         " from val_istruzioni " +
                         " where selezione_cod = :selezione_cod" +
                         " and anno = :anno";

            DataTable dt = new DataTable();

            dt = accDB.sel_dati(sql, coll);

            return dt;


        }

        public DataTable CaricaIstruzioniInserite()
        {

            String sql = " select  id, selezione_cod, anno, istruzioni, titolo, categoria_cod, ext, bytes, descrizione, titolo_file " +
                         " from val_istruzioni ";

            DataTable dt = new DataTable();

            dt = accDB.sel_dati(sql);

            return dt;


        }

        public int VisualizzaIstruzioniCompilazione(string pCatCod, int pAnno)
        {
            int IDFile;
            Dictionary<String, Object> coll;
            coll = new Dictionary<string, object>();
            coll.Add("categoria_cod", pCatCod);
            coll.Add("anno", pAnno);

            Dictionary<String, Object> collAnno;
            collAnno = new Dictionary<string, object>();
            collAnno.Add("anno", pAnno);

            String sql = " select  id " +
                         " from val_istruzioni " +
                         " where categoria_cod = :categoria_cod" +
                         " and anno = :anno";
            try
            {
                IDFile = int.Parse(accDB.sel_dati(sql, coll).Rows[0][0].ToString());
            }
            catch
            {
                sql = " select  id " +
                      " from val_istruzioni " +
                      " where anno = :anno";
                try
                {
                    IDFile = int.Parse(accDB.sel_dati(sql, collAnno).Rows[0][0].ToString());
                }
                catch
                {
                    IDFile = 0;
                }
            }
            
            return IDFile;

        }
        public DataTable CaricaDettaglioIstruzioni(int pID)
        {
            Dictionary<string, object> coll = new Dictionary<string, object>();
            coll.Add("id", pID);
            String sql = " select  id, selezione_id, categoria_cod, selezione_cod, anno, descrizione, titolo, istruzioni, ext, bytes , titolo_file" +
                        " from val_istruzioni " +
                        " where id = :id";

            DataTable dt = accDB.sel_dati(sql, coll);

            return dt;
        }
        public void ins_FileIstruzioni(Dictionary<string, object> p_coll_s)
        {
            clsTransaction clsTrx = new clsTransaction();
            try
            {
                accDB.insert_row("val_istruzioni", p_coll_s);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void del_FileIstruzioni(int pID, clsTransaction p_clsTrx)
        {
            Dictionary<String, Object> coll;
            clsTransaction clsTrx = p_clsTrx;
            if (clsTrx == null)
            {
                clsTrx = new clsTransaction();
            }
            try
            {
                coll = new Dictionary<String, Object>();
                coll.Add("id", pID);
                clsTrx.del_row_trx("val_istruzioni", coll);
            }
            catch (Exception ex)
            {
                if (p_clsTrx == null)
                {
                    clsTrx.rollback_trx();
                    clsTrx = null;
                }
                throw ex;
            }
            if (p_clsTrx == null)
            {
                clsTrx.commit_trx();
                clsTrx = null;
            }
        }
        public void upd_FileIstruzioni(Dictionary<string, object> p_coll_s, Dictionary<string, object> p_coll_w)
        {
            try
            {
                accDB.update_row("val_istruzioni", p_coll_s, p_coll_w);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataRow LeggiFileIstruzioni(int pId)
        {
            DataTable dt = new DataTable();
            DataRow dr = dt.NewRow();
            Dictionary<string, object> coll = new Dictionary<string, object>();
            coll.Add("id", pId);

            String sql = "select istruzioni, bytes, ext, descrizione, titolo_file" +
                " from val_istruzioni " +
                " where id = :id ";
            if (accDB.sel_dati(sql, coll).Rows.Count != 0)
                return accDB.sel_dati(sql, coll).Rows[0];
            else
                return dr;
        }
        //>>ga2013dicembre


    }
}
