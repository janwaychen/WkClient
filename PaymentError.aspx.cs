using System;

namespace WowkoolMini
{
    public partial class PaymentError : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Write(Request["res"]);
        }
    }
}