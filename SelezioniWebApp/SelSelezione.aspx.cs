using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using clDB;
using SelezioniObjects;
using System.Text;
using System.Data;

namespace SelezioniWebApp
{
    public partial class SelSelezione : System.Web.UI.Page
    {
        ClsDB DbUtility = new ClsDB();
        ClsUtility Utility = new ClsUtility(); //>>ga29082012<<

        protected void Page_Load(object sender, EventArgs e)
        {
            //ClsDB DbUtility = new ClsDB(); 
            lblErr.Text = "";
            if (!IsPostBack)
            {

                IList<clsStato> lStato = new List<clsStato>();
                lStato = DbUtility.PopolaDDLFiltroStato();
                ddl_filtro_stato.DataSource = lStato;
                ddl_filtro_stato.DataValueField = "Stato";
                ddl_filtro_stato.DataTextField = "DescrStato";
                ddl_filtro_stato.DataBind();
                ddl_filtro_stato.SelectedIndex = 0;

                IList<clsAnno> lAnno = new List<clsAnno>();
                lAnno = DbUtility.PopolaDDLFiltroAnno();
                ddl_filtro_anno.DataSource = lAnno;
                ddl_filtro_anno.DataValueField = "Anno";
                ddl_filtro_anno.DataTextField = "Anno";
                ddl_filtro_anno.DataBind();
                ddl_filtro_anno.SelectedIndex = 0;

                IList<clsCategoria> lCategoria = new List<clsCategoria>();
                lCategoria = DbUtility.PopolaDDLFiltroCategoria();
                ddl_filtro_categoria.DataSource = lCategoria;
                ddl_filtro_categoria.DataValueField = "Categoria";
                ddl_filtro_categoria.DataTextField = "Categoria";
                ddl_filtro_categoria.DataBind();
                ddl_filtro_categoria.SelectedIndex = 0;

                //carica_selezioni(); 
            }
            carica_selezioni();

        }

        private void carica_selezioni()
        {
            string sUser = "";
            sUser = User.Identity.Name;
            ClsDB DbUtility = new ClsDB();
            IList<Selezione> lSelezione;
            lSelezione = DbUtility.LeggiSelezioni(ddl_filtro_stato.SelectedValue, ddl_filtro_anno.SelectedValue, ddl_filtro_categoria.SelectedValue, 0);
            grdSelezioni.DataSource = lSelezione;
            grdSelezioni.DataBind();
        }

        protected void mnu_main_MenuItemClick(object sender, MenuEventArgs e)
        {
            string stremess = "";

            if (e.Item.Value == "Rit")
            {
                Page.Response.Redirect("Gestione.aspx");
            }
            else if (e.Item.Value == "Stm")
            {
                if (hdnSelezioneId.Value == "0")
                {
                    string Message = "Devi selezionare una selezione per stamparla";
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + Message + "');", true);
                }
                else
                {
                    Page.Response.Redirect("StampaSelezione.aspx?SelezioneId=" + hdnSelezioneId.Value);
                }
            }
            else if (e.Item.Value == "Mod")
            {
                if (hdnSelezioneId.Value == "0")
                {
                    string Message = "Devi selezionare una riga per modificarla";
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + Message + "');", true);
                }
                else
                {
                    try
                    {
                        Page.Response.Redirect("Costruzione.aspx?SelezioneId=" + hdnSelezioneId.Value);
                    }
                    catch (Exception ex)
                    {
                        string Message = ex.Message;
                        this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + Message + "');", true);
                    }
                }
            }
            else if (e.Item.Value == "Del")
            {
                if (hdnSelezioneId.Value == "0")
                {
                    string Message = "Devi selezionare una riga ";
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + Message + "');", true);
                }
                else
                {
                    try
                    {
                        DbUtility.del_selezione(int.Parse(hdnSelezioneId.Value.ToString()));
                        Page.Response.Redirect("SelSelezione.aspx");
                    }
                    catch
                    {
                        string Message = "Cancellazione non riuscita. Contattare I.S.I.";
                        this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + Message + "');", true);
                    }
                }
            }
            else if (e.Item.Value == "Dup")
            {
                if (hdnSelezioneId.Value == "0")
                {
                    string Message = "Devi selezionare una riga ";
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + Message + "');", true);
                }
                else
                {
                    try
                    {
                        Page.Response.Redirect("Duplica.aspx?SelezioneId=" + int.Parse(hdnSelezioneId.Value.ToString()));
                    }
                    catch (Exception ex)
                    {
                        stremess = "Duplicazione FALLITA Avvisa I.S.I." + ex.Message;
                        this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + stremess + "');", true);

                    }
                }
            }
            else if (e.Item.Value == "Pub" || e.Item.Value == "Chd")
            {
                if (hdnSelezioneId.Value == "0")
                {
                    string Message = "Devi selezionare una riga ";
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + Message + "');", true);
                }
                else
                {
                    try
                    {
                        DbUtility.ModificaStatoSelezione(int.Parse(hdnSelezioneId.Value.ToString()), e.Item.Value, null, ((clsUtenteLogin)Session["Utente"]).UserId);
                        Page.Response.Redirect("SelSelezione.aspx");
                        carica_selezioni();
                    }
                    catch (CustomExceptions.SelezioneConValutazioni ex)
                    {
                        stremess = "Cambiamento stato FALLITO Avvisa I.S.I." + " " + ex.CustomMessage.ToString();
                        this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + stremess + "');", true);
                    }
                    catch (CustomExceptions.SelezioniAncoraAttive ex)
                    {
                        stremess = "Cambiamento stato FALLITO." + " " + ex.CustomMessage.ToString();
                        this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + stremess + "');", true);
                    }
                    catch (Exception ex)
                    {
                        stremess = "Cambiamento stato FALLITO Avvisa I.S.I.: " + ex.Message.ToString();
                        this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + stremess + "');", true);
                    }
                }
            }
            else if (e.Item.Value == "Exc")
            {
                if (hdnSelezioneId.Value == "0")
                {
                    string Message = "Devi selezionare una riga ";
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + Message + "');", true);
                }
                else
                {
                    try
                    {
                        DataTable dtCompilazioni = new DataTable();
                        dtCompilazioni = DbUtility.EstraiElencoFinale(int.Parse(hdnSelezioneId.Value.ToString()), 0);
                        Response.ClearContent();
                        Response.ClearHeaders();
                        Response.Buffer = true;
                        Response.Charset = "";
                        Response.ContentType = "application/vnd.ms-excel";
                        Response.AppendHeader("Content-Disposition", "inline; filename=ExportExcel.xls");
                        Utility.crea_excel(dtCompilazioni, int.Parse(hdnSelezioneId.Value.ToString()), Response.OutputStream);
                        Response.Flush();
                        Response.ClearContent();
                        Response.End();
                    }
                    catch (Exception ex)
                    {
                        stremess = "Estrazione FALLITA Avvisa I.S.I." + ex.Message;
                        this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + stremess + "');", true);

                    }
                }
            }
            else if (e.Item.Value == "Arc")
            {
                if (hdnSelezioneId.Value == "0")
                {
                    string Message = "Devi selezionare una riga ";
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + Message + "');", true);
                }
                else
                {
                    try
                    {
                          int tmpSelezioneId = int.Parse(hdnSelezioneId.Value.ToString());
                          Selezione Selezione;
                          Selezione = DbUtility.LeggiSelezione(tmpSelezioneId);
                          DbUtility.ArchiviaSelezione(tmpSelezioneId, Selezione.Anno, Selezione.CategoriaCod, ((clsUtenteLogin)Session["Utente"]).UserId);
                          try
                          {
                              DbUtility.ModificaStatoSelezione(tmpSelezioneId, "Arc", null, ((clsUtenteLogin)Session["Utente"]).UserId);
                          }
                          catch (Exception ex)
                          {
                              throw ex;
                          }

                          stremess = "Archiviazione avvenuta con successo";
                          this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + stremess + "');", true);  
                          Page.Response.Redirect("SelSelezione.aspx");
                          carica_selezioni();
                    }
                    catch (Exception ex)
                    {
                        stremess = "Archiviazione FALLITA Avvisa I.S.I." + ex.Message;
                        this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + stremess + "');", true);

                    }
                   
                }
            }
        }

        protected void ddl_filtro_stato_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddl_filtro_anno_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddl_filtro_categoria_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void grdSelezioni_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Selezione selSelezione = e.Row.DataItem as Selezione;
                e.Row.Attributes.Add("onclick", "GridView_selectRow(this,'" + selSelezione.SelezioneId + "')");
                e.Row.Attributes.Add("onmouseover", "GridView_mouseHover(this)");
            }
        }

        protected string ConvertiData(DateTime pData)
        {
            return (pData.ToString()).Substring(0, 10);
        }

        protected void btnfiltro_Click(object sender, EventArgs e)
        {
            carica_selezioni();
        }

        protected void btnAttivaDisattivaMenu_Click(object sender, EventArgs e)
        {
            int intIndex = Convert.ToInt16(Request.Form["__EVENTARGUMENT"].ToString());
            try
            {
                Selezione oSelezione = DbUtility.LeggiSelezione(intIndex);
                if (oSelezione.Stato == 0)
                {
                    abilita_disabilita_menu(true, true, true, true, true, true, false, true, false);
                }
                else if (oSelezione.Stato == 1)
                {
                    abilita_disabilita_menu(true, true, false, true, true, true, true, true, false);
                }
                else if (oSelezione.Stato == 8)
                {
                    abilita_disabilita_menu(false, true, false, false, false, false, true, true, true);
                }
                else if (oSelezione.Stato == 99)
                {
                    abilita_disabilita_menu(false, true, false, false, true, false, false, true, false);
                }
            }
            catch (Exception ex)
            {
                string Message = ex.Message;
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + Message + "');", true);
            }

        }
        private void abilita_disabilita_menu(bool pMod, bool pDup, bool pDel, bool pAut, bool pStm, bool pPub, bool pChd, bool pRit, bool pArc)
        {
            foreach (MenuItem i in mnu_main.Items)
            {
                if (i.Value == "Mod")
                {
                    i.Selectable = pMod;
                }
                else if (i.Value == "Dup")
                {
                    i.Selectable = pDup;
                }
                else if (i.Value == "Del")
                {
                    i.Selectable = pDel;
                }
                else if (i.Value == "Aut")
                {
                    i.Selectable = pAut;
                }
                else if (i.Value == "Stm")
                {
                    i.Selectable = pStm;
                }
                else if (i.Value == "Pub")
                {
                    i.Selectable = pPub;
                }
                else if (i.Value == "Chd")
                {
                    i.Selectable = pChd;
                }
                else if (i.Value == "Rit")
                {
                    i.Selectable = pRit;
                }
                else if (i.Value == "Arc")
                {
                    i.Selectable = pArc;
                }
            }
        }

        private void carica_selezioni_arc(int pSelezioneStato, int pSelezioneCod)
        {
            string sUser = "";
            sUser = User.Identity.Name;
            ClsDB DbUtility = new ClsDB();
            IList<Selezione> lSelezione;
            lSelezione = DbUtility.LeggiSelezioni(pSelezioneStato.ToString(), ddl_filtro_anno.SelectedValue, ddl_filtro_categoria.SelectedValue, pSelezioneCod);
            grdSelezioni.DataSource = lSelezione;
            grdSelezioni.DataBind();
        }
    
    }
}