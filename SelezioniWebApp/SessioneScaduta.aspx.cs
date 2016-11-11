using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SelezioniWebApp
{
    public partial class SessioneScaduta : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                throw new CustomExceptions.SessioneScaduta();
            }
            catch (CustomExceptions.SessioneScaduta ex)
            {
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + ex.CustomMessage + "');", true);
            }

        }
        protected void mnu_CompilaSelezione_MenuItemClick(object sender, MenuEventArgs e)
        {
            if (e.Item.Value == "Rit")
            {
                //Page.Response.Redirect("http://www.units.it/intra/modulistica/peo2011/");
                //Page.Response.Redirect("http://www.units.it/intra/modulistica/peo2015/");
                Page.Response.Redirect("https://www.units.it/intra/modulistica/peo/");
            }
        }
    }
}