using System;

namespace WowkoolMini
{
    public partial class PaymentResult : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string res = Request["res"];
            ltrRes.Text = res;
        }
    }
}