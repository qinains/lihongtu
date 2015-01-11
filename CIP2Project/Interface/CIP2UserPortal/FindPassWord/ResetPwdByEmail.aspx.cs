using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using System.Text;

using Linkage.BestTone.Interface.BTException;
using Linkage.BestTone.Interface.Utility;
using Linkage.BestTone.Interface.Rule;

public partial class FindPassWord_ResetPwdByEmail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        String urlParam = Request["UrlParam"] == null ? String.Empty : HttpUtility.UrlDecode(Request["UrlParam"]);
        if (String.IsNullOrEmpty(urlParam))
        {
            this.ResetPanel.Visible = false;
            this.MsgPanel.Visible = true;
        }
        else
        {
            this.ResetPanel.Visible = true;
            this.MsgPanel.Visible = false;
            try
            {
                //解析并获取参数
                String DecryptParam = CryptographyUtil.Decrypt(Encoding.UTF8.GetString(CryptographyUtil.FromBase64String(urlParam)));
                String[] paramArray = DecryptParam.Split('$');
                String spid = paramArray[0];
                String custid = paramArray[1];
                String email = paramArray[2];
                String returnUrl = String.IsNullOrEmpty(paramArray[3]) ? ConstHelper.DefaultInstance.BesttoneLoginPage : paramArray[3];
                String authenCode = paramArray[4];
                String timeTamp = paramArray[5];
                String digest = paramArray[6];
                this.hdCustID.Value = custid;
                this.hdEmail.Value = email;
                this.hdAuthenCode.Value = authenCode;

                //对参数进行验证
                SPInfoManager spInfo = new SPInfoManager();
                Object SPData = spInfo.GetSPData(this.Context, "SPData");
                String key = spInfo.GetPropertyBySPID(spid, "SecretKey", SPData);
                String NewDigest = CryptographyUtil.GenerateAuthenticator(spid + "$" + custid + "$" + email + "$" + returnUrl + "$" + authenCode + "$" + timeTamp, key);
                //看是否过期
                DateTime sendMailTime = Convert.ToDateTime(timeTamp);
                Int32 expiredHour = ConstHelper.DefaultInstance.ResetPwdExpiredHour;

                //签名不正确
                if (String.Equals(digest, NewDigest) == false || (sendMailTime.AddHours(expiredHour) < DateTime.Now))
                {
                    this.ResetPanel.Visible = false;
                    this.MsgPanel.Visible = true;
                }
                else
                {
                    String ErrMsg = String.Empty;
                    Int32 result = SetMail.CheckEmaklSend(custid, email, authenCode, out ErrMsg);
                    if (result == 0)
                    {
                        this.hdCustID.Value = custid;
                        this.hdReturnUrl.Value = returnUrl;
                    }
                    else
                    {
                        this.ResetPanel.Visible = false;
                        this.MsgPanel.Visible = true;
                    }
                }         
            }
            catch (Exception ex)
            {
                this.ResetPanel.Visible = false;
                this.MsgPanel.Visible = true;
            }
            
        }
    }
}
