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
using Linkage.BestTone.Interface.Rule;
using Linkage.BestTone.Interface.Utility;
using Linkage.BestTone.Interface.BTException;
using System.Text;

public partial class ChangePayPWD :AccountBasePage
{
    Int32 Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
    String ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;
    public string _ReturnUrl = "";
    public string success = "1";

    protected void Page_Load(object sender, EventArgs e)
    {
        this.hdHeadFoot.Value = base.IsNeedHeadFoot == "yes" ? "1" : "0";

    }

    protected void Modify_Click(object sender, EventArgs e)
    {
        StringBuilder strLog = new StringBuilder();
        try
        {
            String oldPassWord = Request.Form["oldPassWord"];
            String newPassWord = Request.Form["newPassWord"];
            String confirmPassWord = Request.Form["confirmPassWord"];
            string BesttoneAccount = base.BestPayAccount;
            strLog.AppendFormat("【开始修改密码,事件：{0}】", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            strLog.AppendFormat("参数：oldPassWord:{0},newPassWord:{1},confirmPassWord:{2},BesttonePayAccount:{3}", oldPassWord, newPassWord, confirmPassWord, BesttoneAccount);
            
            BestPayEncryptService bpes = new BestPayEncryptService();
            string e_oldPassWord = "";
            string e_newPassWord = "";
            string e_confirmPassWord = "";

            AccountItem ai = new AccountItem();
            String ResCode = "";
            int QueryBesttoneAccountResult = BesttoneAccountHelper.BesttoneAccountInfoQuery(BesttoneAccount, out ai,out ResCode, out ErrMsg);
            if (QueryBesttoneAccountResult == 0)
            {
                if (ai != null)
                {
                    e_oldPassWord = bpes.encryptNoKey(oldPassWord, ai.AccountNo);
                    e_newPassWord = bpes.encryptNoKey(newPassWord, ai.AccountNo);
                    e_confirmPassWord = bpes.encryptNoKey(confirmPassWord, ai.AccountNo);

                    strLog.AppendFormat("e_oldPassWord{0},e_newPassWord{1},e_confirmPassWord{2}", e_oldPassWord, e_newPassWord, e_confirmPassWord);

                    int ModifyBestPayPasswordResult = BesttoneAccountHelper.ModifyBestPayPassword(ai.AccountNo, e_oldPassWord, e_newPassWord, e_confirmPassWord, out ErrMsg);

                    if (ModifyBestPayPasswordResult == 0)
                    {
                        success = "0";
                        _ReturnUrl = base.ReturnUrl;
                    }
                    else
                    {
                        strLog.Append(",失败3");
                        Response.Redirect("ErrorInfo.aspx?ErrorInfo=" + ErrMsg);
                    }
                }
                else
                {
                    strLog.Append(",失败2");
                    Response.Redirect("ErrorInfo.aspx?ErrorInfo=账户信息未获取");
                }
            }
            else
            {
                strLog.Append(",失败1");
                Response.Redirect("ErrorInfo.aspx?ErrorInfo=" + ErrMsg);
            }
        }
        catch (System.Exception ex)
        {
            log(ex.ToString());
        }
        finally
        {
            log(strLog.ToString());
        
        }
    }

    protected void log(string str)
    {
        System.Text.StringBuilder msg = new System.Text.StringBuilder();
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        msg.Append(str);
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        BTUCenterInterfaceLog.CenterForBizTourLog("ChangePayPassword", msg);
    }

}
