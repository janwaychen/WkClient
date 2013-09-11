using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace WowkoolMini
{
    public partial class PaymentTalk : System.Web.UI.Page
    {
        private StringBuilder postData =new StringBuilder();
        private Dictionary<string,string> _map =new Dictionary<string, string>();
        protected void Page_Load(object sender, EventArgs e)
        {
            string _res = prepareData();
            // send data
            string url = "http://192.168.8.105:8080/PutToPT.do";
            //string url = "http://pay.wowkool.com/PutToPT.jsp";
            _res = sendData(url);
            _res = _res.Replace("\r\n", string.Empty);
            _map = GetDictionary(_res);
            if (_map.ContainsKey("ack"))
            {
                if (_map["ack"] == "success")
                {
                    //Response.Redirect("https://pay.wowkool.com/payAPI/PTCommit.jsp?token=" + _map["token"]);
                    Response.Redirect("http://192.168.8.105:8080/payAPI/PTCommit.jsp?token=" + _map["token"]);
                }
                else if (_map["ack"] == "fail")
                {
                    Response.Write(_map["reason"]);
                }
            }
        }
        private string prepareData()
        {
            string _res = string.Empty;
            string apiKey = "V1QpQ87gCx94Mpxz3ARk23VaSRaNIWE6kI40HEUxF";
            string _userId = "janway@global-opto.com";
            string _orderNumber = "100000266";
            string[] _itemNames = {"刷子","洗衣精","運費","生日蛋糕","運費"};
            string[] _itemId = { "PROD_020202", "PROD_020201", "FEE_000002", "PROD_030302", "FEE_000002" };
            string[] _vendorId = { "sky000", "sky000", "sky000", "66shopping", "66shopping" };
            string[] _vendorName = { "天天小站", "天天小站", "天天小站", "66小舖", "66小舖" };
            string[] _flagShippingFee = { "N", "N", "Y", "N", "Y" };
            string[] _trackingCode = { "", "", "SH0001", "", "SH0002" };
            int[] _qtys = {1,1,1,1,1};
            double[] _amts = {60,199,10,670,0};

            double _taxSum = 0, _amountSum = 0, _taxClearence = 0;

            string _buyerId =  "janway@global-opto.com";
            string _buyerName =  "周杰倫";
            string _payTo = "service@wowkool.com";
            string _orderDt = DateTime.Now.ToString("yyyyMMddHHmmss");
            string _monetaryUnit = "TWD";
            String _rcvrName = "周杰倫";
            String _rcvrPhoneNo = "093934242";
            String _rcvrAddress = "台北市大安區敦化南路二段65號10樓";
            String _billAddress = _rcvrAddress;//Server.UrlEncode("")
            
            // set post data
            //postData = "userId=" + _userId + "&orderNumber=" + _orderNumber + "&apiKey=" + apiKey+"&vendor="+_vendor+"&itemName="+_itemName+"&amt="+_amt
            //     +"&orderDt="+_orderDt;
            postData.Append("userId=").Append(_userId)
                    .Append("&action=").Append("Checkout")
                    .Append("&apiKey=").Append(apiKey)
                    .Append("&orderNumber=").Append(_orderNumber)
                    .Append("&orderDt=").Append(_orderDt)
                    .Append("&buyerId=").Append(Server.UrlEncode(_buyerId))
                    .Append("&buyerName=").Append(Server.UrlEncode(_buyerName))
                    .Append("&payTo=").Append(Server.UrlEncode(_payTo))
                    .Append("&monetaryUnit=").Append(_monetaryUnit);
            for (int i = 0; i < _itemNames.Length; i++)
            {
                postData.Append("&vendorId=").Append(Server.UrlEncode(_vendorId[i]))
                        .Append("&vendorName=").Append(Server.UrlEncode(_vendorName[i]))
                        .Append("&itemId=").Append(Server.UrlEncode(_itemId[i]))
                        .Append("&itemName=").Append(Server.UrlEncode(_itemNames[i]))
                        .Append("&flagShippingFee=").Append(Server.UrlEncode(_flagShippingFee[i]))
                        .Append("&trackingCode=").Append(Server.UrlEncode(_trackingCode[i]))
                        .Append("&qty=").Append(_qtys[i])
                        .Append("&amt=").Append(_amts[i]);
                _amountSum += _qtys[i] * _amts[i];
            }
            postData.Append("&grandTotal=").Append(_amountSum+_taxSum+_taxClearence)
                    .Append("&taxSum=").Append(_taxSum)
                    .Append("&taxClearance=").Append(_taxClearence)
                    .Append("&rcvrName=").Append(Server.UrlEncode(_rcvrName))
                    .Append("&rcvrPhoneNo=").Append(Server.UrlEncode(_rcvrPhoneNo))
                    .Append("&rcvrAddress=").Append(Server.UrlEncode(_rcvrAddress))
                    .Append("&billAddress=").Append(Server.UrlEncode(_billAddress));
            //
            postData.Append("&successUrl=").Append("http://127.0.0.1:2672/PaymentResult.aspx")
                    .Append("&failedUrl=").Append("http://127.0.0.1:2672/PaymentError.aspx");
            return _res;
        }
        private string sendData(string url)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.Method = WebRequestMethods.Http.Post;
            request.ContentLength = postData.Length;
            request.ContentType = "application/x-www-form-urlencoded";
            StreamWriter writer = new StreamWriter(request.GetRequestStream());
            writer.Write(postData.ToString());
            writer.Close();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string _tmp = reader.ReadToEnd();
            response.Close();
            return _tmp;
        }
        private Dictionary<string, string> GetDictionary(string res)
        {
            Dictionary<string,string> _dic = new Dictionary<string, string>();
            string[] itms = res.Split('&');
            for (int i = 0; i < itms.Length; i++)
            {
                string[] nvp = itms[i].Split('=');
                if (nvp.Length == 2)
                {
                    _dic.Add(nvp[0],nvp[1]);
                }
            }
            return _dic;
        }
        
    }
}