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
    public partial class CreaSelezione : System.Web.UI.Page
    {
        ClsDB FilClass = new ClsDB();
        static int SelezioneId;
        IList<Selezione> lSelezioni = new List<Selezione>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.Count == 1)  // modifica testata
            {
                if (!Page.IsPostBack)
                {
                    SelezioneId = int.Parse(Request.QueryString["SelezioneId"].ToString());
                    lSelezioni = FilClass.LeggiSelezioni(null, null, null, SelezioneId);
                    txt_ins_titolo.Text = lSelezioni[0].Titolo.ToString();
                    ddl_ins_categoria_cod.DataSource = FilClass.PopolaDDLCategoria();
                    ddl_ins_categoria_cod.DataValueField = "categoria";
                    ddl_ins_categoria_cod.DataTextField = "des_categoria";
                    ddl_ins_categoria_cod.DataBind();
                    ddl_ins_categoria_cod.SelectedValue = lSelezioni[0].CategoriaCod.ToString();
                    ddl_ins_selezione_cod.DataSource = FilClass.PopolaDDLSelezioneCod();
                    ddl_ins_selezione_cod.DataValueField = "cod_selezione";
                    ddl_ins_selezione_cod.DataTextField = "des_selezione";
                    ddl_ins_selezione_cod.DataBind();
                    ddl_ins_selezione_cod.SelectedValue = lSelezioni[0].SelezioneCod.ToString();
                    txt_ins_anno.Text = lSelezioni[0].Anno.ToString();
                    txt_ins_descrizione.Text = lSelezioni[0].Descrizione.ToString();
                    txt_ins_data_iniz_val.Text = lSelezioni[0].DataInizVal.ToString();
                    txt_ins_data_fine_val.Text = lSelezioni[0].DataFineVal.ToString();
                    txt_ins_data_term_pres.Text = lSelezioni[0].DataTermPres.ToString();
                    txt_ins_data_term_controllo_amm.Text = lSelezioni[0].DataTermCtrlAmm.ToString();
                    txt_ins_data_term_val_resp.Text = lSelezioni[0].DataTermValResp.ToString();
                    txt_ins_data_term_controllo_dip.Text = lSelezioni[0].DataTermCtrlDip.ToString();
                    txt_ins_data_term_val_amm.Text = lSelezioni[0].DataTermValAmm.ToString();
                    btnConfermaSel.Text = "Modifica";
                }
            }
            else // creazione testata
            {
                if (!Page.IsPostBack)
                {
                    ddl_ins_categoria_cod.DataSource = FilClass.PopolaDDLCategoria();
                    ddl_ins_categoria_cod.DataValueField = "categoria";
                    ddl_ins_categoria_cod.DataTextField = "des_categoria";
                    ddl_ins_categoria_cod.DataBind();

                    ddl_ins_selezione_cod.DataSource = FilClass.PopolaDDLSelezioneCod();
                    ddl_ins_selezione_cod.DataValueField = "cod_selezione";
                    ddl_ins_selezione_cod.DataTextField = "des_selezione";
                    ddl_ins_selezione_cod.DataBind();
                }
            }
        }
        protected void mnu_CreaSelezione_MenuItemClick(object sender, MenuEventArgs e)
        {
            ClsDB dbUtility = new ClsDB();
            if (e.Item.Value == "Back")
            {
                Page.Response.Redirect("Gestione.aspx");
            }
        }
        protected void btnConfermaSel_Click(object sender, EventArgs e)
        {
            if (((Button)sender).ID == "btnConfermaSel")
            {
                if (txt_ins_titolo.Text != "")
                {
                    String stremess = "";
                    ClsDB FilClass = new ClsDB();
                    Dictionary<String, Object> coll = new Dictionary<String, Object>();

                    if (btnConfermaSel.Text == "Inserisci")
                    {
                        coll.Add("stato", "0");
                    }
                    foreach (Control c in testata_creamod.Controls)
                    {
                        if (c.GetType().Name != "LiteralControl")
                        {
                            if (c.ID.IndexOf("_ins_", 0) == 3)
                            {
                                if (object.Equals("TextBox", c.GetType().Name))
                                {
                                    if (c.ID.IndexOf("_data_") > 0 )
                                    {
                                        if (((TextBox)c).Text != "")
                                            coll.Add((c.ID).Substring(8, (c.ID).Length - 8), DateTime.Parse(((TextBox)c).Text));
                                        else
                                            coll.Add((c.ID).Substring(8, (c.ID).Length - 8), null);
                                    }
                                    else
                                    {
                                        coll.Add((c.ID).Substring(8, (c.ID).Length - 8), ((TextBox)c).Text);
                                    }
                                }
                                else if (object.Equals("DropDownList", c.GetType().Name))
                                {
                                    //TODO: Controllare che sia stata fatta una scelta nelle 2 DropDownList
                                    coll.Add((c.ID).Substring(8, (c.ID).Length - 8), ((DropDownList)c).SelectedValue);
                                }
                                else if (object.Equals("CheckBox", c.GetType().Name))
                                {
                                    coll.Add((c.ID).Substring(8, (c.ID).Length - 8), (((CheckBox)c).Checked == true) ? 1 : 0);
                                }
                            }
                        }
                    }
                    try
                    {
                        //string sUser = User.Identity.Name;
                        //sUser = sUser.Substring(sUser.IndexOf("\\") + 1);
                        //sUser = sUser.PadLeft(6, '0');
                        string sUser = ((clsUtenteLogin)Session["Utente"]).UserId;

                        if (btnConfermaSel.Text == "Inserisci")
                        {
                            coll.Add("usr_ins", sUser);
                            coll.Add("data_ins", System.DateTime.Today.Date);
                            FilClass.ins_selezione(coll);
                            coll.Clear();
                            Page.Response.Redirect("SelSelezione.aspx");
                        }
                        else if (btnConfermaSel.Text == "Modifica")
                        {
                            coll.Add("usr_mod", sUser);
                            coll.Add("data_mod", System.DateTime.Today.Date);
                            Dictionary<String, Object> coll_w = new Dictionary<String, Object>();
                            coll_w.Add("selezione_id", SelezioneId);
                            FilClass.upd_selezione(coll, coll_w);
                            Page.Response.Redirect("Costruzione.aspx?SelezioneId=" + SelezioneId);

                        }
                    }
                    catch (Exception ex)
                    {
                        stremess = "Operazione FALLITA Avvisa I.S.I." + ex.Message;
                    }
                    if (string.IsNullOrEmpty(stremess))
                    {
                    }
                    else
                    {
                        this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + stremess + "');", true);
                    }

                    //LblErr.Text = stremess;

                }
            }
            else if (((Button)sender).ID == "btnAnnullaSel")
            {
                if (Request.QueryString.Count == 1)  // modifica testata
                {
                    Page.Response.Redirect("Costruzione.aspx?SelezioneId=" + SelezioneId);
                }
                else
                {
                    Page.Response.Redirect("Gestione.aspx");
                }
            }
        }
        protected void ddl_ins_categoria_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void ddl_ins_selezione_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}