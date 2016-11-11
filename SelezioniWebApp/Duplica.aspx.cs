using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using clDB;
using SelezioniObjects;

namespace SelezioniWebApp
{
    public partial class Duplica : System.Web.UI.Page
    {
        ClsDB FilClass = new ClsDB();
        private int SelezioneId;
        string stremess = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            IList<Selezione> lSelezioni = new List<Selezione>();
            if (!Page.IsPostBack)
            {
                try
                {
                    SelezioneId = int.Parse(Request.QueryString["SelezioneId"].ToString());
                    ddl_ins_categoria_cod.DataSource = FilClass.PopolaDDLCategoria();
                    ddl_ins_categoria_cod.DataValueField = "categoria";
                    ddl_ins_categoria_cod.DataTextField = "des_categoria";
                    ddl_ins_categoria_cod.DataBind();
                    lSelezioni = FilClass.LeggiSelezioni(null, null, null, SelezioneId);
                    lbl_anno_t.Text = lSelezioni[0].Anno.ToString();
                    lbl_titolo_t.Text = lSelezioni[0].Titolo.ToString();
                    lbl_categoria_cod_t.Text = lSelezioni[0].CategoriaCod.ToString();
                }
                catch (Exception ex)
                {
                    throw (ex);
                }

            }

        }
        protected void btnConfermaSel_Click(object sender, EventArgs e)
        {
            if (((Button)sender).ID == "btnConfermaSel")
            {
                if (ckb_anno.Checked == false && ddl_ins_categoria_cod.SelectedIndex == 0)
                {
                    stremess = "Occorre selezionare una tipologia di duplicazione";
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + stremess + "');", true);
                }
                else if (ckb_anno.Checked == true && ddl_ins_categoria_cod.SelectedIndex != 0)
                {
                    stremess = "Occorre selezionare una SOLA tipologia di duplicazione";
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + stremess + "');", true);
                }
                else if (lbl_categoria_cod_t.Text == ddl_ins_categoria_cod.Text)
                {
                    stremess = "Non è possibile duplicare nello stesso anno con la stessa categoria";
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + stremess + "');", true);
                }
                else
                {
                    string pUser = User.Identity.Name.ToString();

                    string sUser = pUser.Substring(pUser.IndexOf("\\") + 1);
                    try
                    {
                        if (ckb_anno.Checked == true) // duplico la valutazione nell'anno successivo
                        {
                            FilClass.dupl_selezione(int.Parse(Request.QueryString["SelezioneId"].ToString()), int.Parse(lbl_anno_t.Text) + 1, null, sUser);

                        }
                        else // duplico la valutazione in una nuova categoria
                        {
                            FilClass.dupl_selezione(int.Parse(Request.QueryString["SelezioneId"].ToString()), 0, ddl_ins_categoria_cod.Text, sUser);

                        }
                        Response.Redirect("SelSelezione.aspx");
                    }
                    catch (CustomExceptions.SelezioneEsistente ex)
                    {
                        stremess = "Duplicazione non riuscita. Contattare I.S.I." + "" + ex.CustomMessage.ToString() +
                            " Anno = " + ex.Anno + " e categoria = " + ex.CatCod.ToString();
                        this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + stremess + "');", true);
                    }
                    catch (Exception ex)
                    {
                        stremess = "Duplicazione non riuscita. Contattare I.S.I.";
                        this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + stremess + "');", true);
                    }
                }
            }

            else if (((Button)sender).ID == "btnAnnullaSel")
            {
                Page.Response.Redirect("SelSelezione.aspx");
            }
        }

        protected void mnu_DuplicaSelezione_MenuItemClick(object sender, MenuEventArgs e)
        {
            ClsDB dbUtility = new ClsDB();
            if (e.Item.Value == "Back")
            {
                Page.Response.Redirect("SelSelezione.aspx");
            }
        }
        protected void ddl_ins_categoria_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}