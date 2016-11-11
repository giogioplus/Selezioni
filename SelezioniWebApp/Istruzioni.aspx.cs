using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using DB_ODP;
using clDB;
using SelezioniObjects;
using System.IO;
using System.Reflection;
using MyControls;

namespace SelezioniWebApp
{
    public partial class Istruzioni : System.Web.UI.Page
    {
        ClsDB DBUtility = new ClsDB();
        ClsUtility Utility = new ClsUtility();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                dtv_dettaglio.Visible = true;
                dtv_dettaglio.DataSource = DBUtility.CaricaIstruzioniInserite();
                dtv_dettaglio.DataBind();
            }
            RichiestaConferma(true, "NoOp", "");
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            // elimino il controllo del messaggio di avvertimento/scelta
            if (Session["EliminaCtrl"] != null && (bool)Session["EliminaCtrl"] == true)
            {
                MessaggioConf.Controls.Clear();
                Session["EliminaCtrl"] = null;
            }
        }

        protected void dtv_dettaglio_Sorting(object sender, GridViewSortEventArgs e)
        {
        }

        protected void dtv_dettaglio_SelectedIndexChanged(object sender, GridViewSortEventArgs e)
        {
        }

        protected void dtv_dettaglio_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Canc")
            {
                Session["FileID"] = e.CommandArgument.ToString();
                RichiestaConferma(false, "Del", "Sicuro di voler cancellare il file?");
            }

        }

        private void RichiestaConferma(bool pLoad, string pTipoOp, string pMsg)
        {
            if ((pLoad == false && Session["ctrl_agg_conf"] == null) || (pLoad == true && ((Session["ctrl_agg_conf"] != null))))
            {
                ModPopUpYesNo myMsgBox = new ModPopUpYesNo();
                myMsgBox.Scegli += SceltaEffettuata;
                myMsgBox.OkButtonText = "Sì";
                myMsgBox.CancButtonText = "No";
                ModPopUpYesNo.TipoOp = pTipoOp;
                if (pMsg != null && pMsg != "")
                {
                    myMsgBox.ConfirmText = pMsg;
                    myMsgBox.Show(ModPopUpYesNo.MessageType.Warning, "Attenzione!", myMsgBox.ConfirmText, 300, 300, 250, 60);

                }
                //myMsgBox.Show(ModPopUpYesNo.MessageType.Warning, "Attenzione!", myMsgBox.ConfirmText, 300, 300, 250, 60);
                MessaggioConf.Controls.Add(myMsgBox);
                Session["TipoOp"] = "Del";
                Session["ctrl_agg_conf"] = true;

            }
            else
            {
                Session["ctrl_agg_conf"] = null;
                Session["TipoOp"] = null;
            }

        }
        public void SceltaEffettuata(bool bOperazione, string sTipoOp)
        {
            if (bOperazione == true)
            {
                try
                {
                    if (sTipoOp == "Del")
                    {
                        CancellaRecord();
                    }
                }
                catch (Exception ex)
                {
                    LblErrDefault.Text = "Operazione FALLITA Avvisa I.S.I." + ex.Message;
                }
                finally
                {
                    Session["EliminaCtrl"] = true;
                    Session["ctrl_agg_conf"] = null;
                    Session["TipoOp"] = null;
                }
            }
            else
            {
                Session["EliminaCtrl"] = true;
                Session["ctrl_agg_conf"] = null;
                Session["TipoOp"] = null;
            }
        }

        private void CancellaRecord()
        {
            int pId = int.Parse(Session["FileID"].ToString());

            DBUtility.del_FileIstruzioni(pId, null);
            dtv_dettaglio.DataSource = DBUtility.CaricaIstruzioniInserite();
            dtv_dettaglio.DataBind();

        }
        protected string RichiamaDettaglio(Int32 pID)
        {
            Session["dtv_dettaglio"] = dtv_dettaglio;
            return "IstruzioniDettaglio.aspx?id=" + pID.ToString();


        }

        protected void mnu_main_MenuItemClick(object sender, MenuEventArgs e)
        {

            if (e.Item.Value == "New")
            {
                Page.Response.Redirect("IstruzioniDettaglio.aspx?ope=new");

            }
            else if (e.Item.Value == "Esc")
            {
                Page.Response.Redirect("Gestione.aspx");
            }


        }
    }
}