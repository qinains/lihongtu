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
using Linkage.BestTone.Interface.Rule;
using Linkage.BestTone.Interface.Utility;
using Linkage.BestTone.Interface.BTException;
public partial class BindingAndRegister : System.Web.UI.Page
{

    string ReturnUrl = "";
    string CustID = "";
    string HeadFooter = "";
    string stamp = "";
    string phoneNum = "";


    string checkCode = "";
    string contactMail = "";
    string sex = "";
    string certnum = "";
    string realName = "";
    string TransactionID = DateTime.Now.ToString("yyyyMMddHHmmss");





    string BesttoneAccount = "";
    public string newSPTokenRequest = "";
    public string SPTokenRequest = "";
    string SPID = "35000000";

    public IList<TxnItem> txnItemList = new List<TxnItem>();
    public String BesttoneAccountBalance = "";




    string ErrMsg = "";
    int Result = 0;
    protected void Page_Load(object sender, EventArgs e)
    {

        if ("yes".Equals(HeadFooter))
        {
            this.header.Visible = true;
            this.footer.Visible = true;
        }
        else
        {
            this.header.Visible = false;
            this.footer.Visible = false;
        }


        string binding = Request.Form["binding"];
        string register = Request.Form["register"];

        if("1".Equals(binding))
        {

        }

        if("1".Equals(register))
        {
            if (!IsPostBack)
            {

                log("OpenBestToneAccount");
                realName = Request["realName"];
                checkCode = Request["checkCode"];
                contactMail = Request["contactMail"];
                sex = Request["sex"];
                certnum = Request["certnum"];

                ParseSPTokenRequest();

                if (Result == 0)
                {
                    int QueryResult = 0;
                    log(String.Format("CustID:{0},SPID{1},HeadFooter{2}", CustID, SPID, HeadFooter));
                    this.myCustID.Value = CustID;
                    this.myReturnUrl.Value = ReturnUrl;
                   

                    PhoneRecord[] phones = CustBasicInfo.getPhoneRecord(CustID, out QueryResult, out ErrMsg);
                    if (QueryResult == 0 && phones != null && phones.Length > 0)
                    {
                        log("getPhoneRecord成功!");
                        phoneNum = phones[0].Phone;
                        this.mobile.Text = phoneNum;
                        this.contactTel.Text = phoneNum;
                        log(String.Format("phoneNum:{0}", phoneNum));
                    }
                    else
                    {
                        log(String.Format("ErrMsg:{0}", ErrMsg));
                        this.mobile.Text = "";
                    }

                }
                else
                {
                    Response.Redirect("ErrorInfo.aspx?ErrorInfo=" + ErrMsg);

                }

            }
  
        }



    }
    protected void register_Click(object sender, EventArgs e)
    {


        Int32 Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
        String ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;


        //判断手机验证码
        if (checkCode != null && !"".Equals(checkCode))
        {
            Result = PhoneBO.SelSendSMSMassage("", this.mobile.Text, checkCode, out ErrMsg);
            if (Result != 0)
            {
                hintCode.InnerHtml = "手机验证码错误，请重新输入";  // 这里如何控制样式
                return;
            }
        }


        //String realName,String contactTel,String sex,String certtype,String certnum, 
        Result = BesttoneAccountHelper.RegisterBesttoneAccount(this.mobile.Text, realName, this.contactTel.Text, contactMail, sex, this.certtype.Value, certnum, TransactionID, out ErrMsg);
        if (Result == 0)
        {
            log(String.Format("开户结果:{0},{1},{2}", Result, ErrMsg, this.myCustID.Value));
            // todo 建立绑定关系，插入绑定关系表
            int ret = 0;

            ret = UserRegistry.CreateBesttoneAccount(SPID, this.myCustID.Value, this.mobile.Text, out ErrMsg);
            if (ret == 0)
            {



                log(String.Format("绑定结果:ret:{0},ErrMsg:{1},ReturnUrl:{2}", ret, ErrMsg, ReturnUrl));

     
                Response.Redirect(this.myReturnUrl.Value);
            }
            else
            {
                log(String.Format("绑定结果:{0},{1}", ret, ErrMsg));
                Response.Redirect("ErrorInfo.aspx?ErrorInfo=" + ErrMsg);
            }
        }
        else
        {
            log(String.Format("开户结果:{0},{1}", Result, ErrMsg));
            Response.Redirect("ErrorInfo.aspx?ErrorInfo=" + ErrMsg);

        }
       
    }


    protected void ParseSPTokenRequest()
    {
        if (CommonUtility.IsParameterExist("SPTokenRequest", this.Page))
        {
            SPTokenRequest = Request["SPTokenRequest"];
            //日志
            log("【SPTokenRequest参数】:" + SPTokenRequest);
            //解析请求参数
            Result = SSOClass.ParseBesttoneAccountPageRequest(SPTokenRequest, this.Context, out SPID, out CustID,
            out HeadFooter, out ReturnUrl, out ErrMsg);
            //日志
            log(String.Format("【解析参数结果】:Result:{0},ErrMsg:{1},SPID:{2},CustID:{3},HeadFooter:{4},stamp:{5},ReturnUrl:{6}", Result, ErrMsg, SPID, CustID, HeadFooter, stamp, ReturnUrl));



        }
    }


    protected void log(string str)
    {
        System.Text.StringBuilder msg = new System.Text.StringBuilder();
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        msg.Append(str);
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        BTUCenterInterfaceLog.CenterForBizTourLog("createBesttoneAccount", msg);
    }


}
