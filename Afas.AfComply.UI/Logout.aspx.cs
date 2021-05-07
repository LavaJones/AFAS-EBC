using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Afas.AfComply.Domain;
using Afas.Domain;

namespace Afas.AfComply.UI
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session.Abandon();
            Session["ValidLogon"] = null;
            Session["CurrentUser"] = null;
            Session["CurrentDistrict"] = null;

            // Clear out all cookies 
            foreach (var key in Request.Cookies.AllKeys) 
            {
                if (Request.Cookies[key] != null)
                {
                    //Response.Cookies[key].Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Set(new HttpCookie(key)
                    {
                        Expires = DateTime.Now.AddDays(-1) 
                    });
                }
            }

            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();

            string query = "";
            if (null != Request.QueryString && false == Request.QueryString["redirect"].IsNullOrEmpty())
            {
                query = "?redirect=" + Request.QueryString["redirect"];
            }

            Response.Redirect("~/default.aspx"+query, false);
        }
    }
}