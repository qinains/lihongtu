using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Newtonsoft.Json;
using Linkage.BestTone.Interface.Rule;
using Linkage.BestTone.Interface.Utility;
using Linkage.BestTone.Interface.BTException;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.Collections.Specialized;
public partial class UnifyPlatformCustomer_UnifyAccountRegister : System.Web.UI.Page
{
    public String SPID;
    public Int32 Result;
    public String ErrMsg;
    public String userName;
    public String password;
    public String captcha;

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public String register()
    {
        StringBuilder strMsg = new StringBuilder();
        StringBuilder ResponseMsg = new StringBuilder();
        Result = ErrorDefinition.IError_Result_UnknowError_Code;
        ErrMsg = ErrorDefinition.IError_Result_UnknowError_Msg;

        #region 数据校验
        if (CommonUtility.IsEmpty(SPID))
        {
            // 返回错误信息
            ResponseMsg.Length = 0;
            ResponseMsg.Append("{");
            ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "995");
            ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "SPID不能为空！");
            ResponseMsg.Append("}");
            return ResponseMsg.ToString();
        }


       

        #endregion


        return null;

    }

}
