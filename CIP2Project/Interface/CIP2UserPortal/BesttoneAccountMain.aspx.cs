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
using System.Text;
public partial class BesttoneAccountMain : System.Web.UI.Page
{
    string ReturnUrl = "";
    string CustID = "";
    string HeadFooter = "";
    string stamp = "";
    string phoneNum = "";
    public string CreateTime = "";

    public string newSPTokenRequest = "";
    public string SPTokenRequest = "";
    string SPID = "35000000";

    public IList<TxnItem> txnItemList = new List<TxnItem>();
    public IList<TxnItem> ConsumerList = new List<TxnItem>();
    public String BesttoneAccountBalance = "";
    public String BesttoneAccount = "";
    public String BesttoneAccountStatus = "";
    string ErrMsg = "";
    int Result = 0;

    public int IsBesttoneAccountBindV5Result = -1;

    protected void Page_Load(object sender, EventArgs e)
    {
        StringBuilder strLog = new StringBuilder();
        try
        {
            if (!IsPostBack)
            {
                strLog.AppendFormat("BesttoneAccountMain");
                ParseSPTokenRequest();

                if (Result == 0)
                {
                    CreateSPTokenRequest();

                    //int QueryResult = 0;
                    strLog.AppendFormat("CustID:{0},SPID{1},HeadFooter{2}", CustID, SPID, HeadFooter);
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

                    //PhoneRecord[] phones = CustBasicInfo.getPhoneRecord(CustID, out QueryResult, out ErrMsg);
                    String qryPhone = "";
                    //if (QueryResult == 0 && phones != null && phones.Length > 0)                    {
                    //    strLog.AppendFormat("getPhoneRecord成功!");
                    //    phoneNum = phones[0].Phone;

                    //    strLog.AppendFormat("根据CustID获得手机号:CustID:{0},phoneNum:{1}", CustID, phoneNum);

                        string BindedBestpayAccount = "";
                        IsBesttoneAccountBindV5Result = CIP2BizRules.IsBesttoneAccountBindV5(CustID, out BindedBestpayAccount, out CreateTime, out ErrMsg);
                        strLog.AppendFormat("IsBesttoneAccountBindV5Result:{0},ErrMsg:{1},CustID:{2},CreateTime:{3},BindedBestpayAccount:{4}", IsBesttoneAccountBindV5Result, ErrMsg, CustID, CreateTime, BindedBestpayAccount);

                        // 0 代表 绑定
                        if (IsBesttoneAccountBindV5Result == 0)
                        {


                            AccountItem ai = new AccountItem();
                            BesttoneAccount = phoneNum;
                            BesttoneAccount = ai.AccountName;

                            
                            if (!String.IsNullOrEmpty(BindedBestpayAccount))
                            {
                                qryPhone = BindedBestpayAccount;
                            }
                            else
                            {
                                qryPhone = phoneNum;
                            }


                            String ResponseCode = "";
                            int QueryBesttoneAccountResult = BesttoneAccountHelper.BesttoneAccountInfoQuery(qryPhone, out ai, out ResponseCode, out ErrMsg);

                            Linkage.BestTone.Interface.Rule.CustInfo custInfo = new Linkage.BestTone.Interface.Rule.CustInfo();
                            int resultQuerycustInfo = BesttoneAccountHelper.QueryCustInfo(qryPhone, out custInfo, out ErrMsg);
                            if (resultQuerycustInfo == 0)
                            {
                                BesttoneAccount = custInfo.CustomerName;
                            }

                            if (QueryBesttoneAccountResult == 0)
                            {
                                if (ai != null)
                                {
                                    //页面赋值

                                    if ("0".Equals(ai.AccountStatus))
                                    {
                                        //this.AccountStatus.InnerText = "未激活";
                                        BesttoneAccountStatus = "未激活";

                                    }
                                    else if ("1".Equals(ai.AccountStatus))
                                    {
                                        //this.AccountStatus.InnerText = "正常";
                                        BesttoneAccountStatus = "正常";


                                    }
                                    else if ("2".Equals(ai.AccountStatus))
                                    {
                                        //this.AccountStatus.InnerText = "挂失";
                                        BesttoneAccountStatus = "挂失";

                                    }
                                    else if ("3".Equals(ai.AccountStatus))
                                    {
                                        //this.AccountStatus.InnerText = "冻结";
                                        BesttoneAccountStatus = "冻结";

                                    }
                                    else if ("4".Equals(ai.AccountStatus))
                                    {
                                        //this.AccountStatus.InnerText = "锁定";
                                        BesttoneAccountStatus = "锁定";

                                    }
                                    else if ("9".Equals(ai.AccountStatus))
                                    {
                                        //this.AccountStatus.InnerText = "已销户";
                                        BesttoneAccountStatus = "已销户";

                                    }
                                    else
                                    {
                                        //this.AccountStatus.InnerText = "";
                                        BesttoneAccountStatus = "";

                                    }

                                    BesttoneAccountBalance = BesttoneAccountHelper.ConvertAmountToYuan(ai.AccountBalance);

                                }
                                else
                                {

                                }
                            }
                            else
                            {

                                if ("200010".Equals(ResponseCode))
                                {
                                    BesttoneAccountStatus = "账户未开通";
                                    BesttoneAccountBalance = "0.00";
                                }
                            }
                        }


                        Int64 balance = 0;
 
                        // 最近交易查询

                        String txnType = "";   // 121020充值
                        //131010消费
                        //131030退费

                        String txnChannel = "02";  //WEB

                        int maxReturnRecord = 30;
                        int startRecord = 1;
                        //int QueryAllTypeTxnResult = BesttoneAccountHelper.QueryAllTypeTxn(phoneNum, txnType, txnChannel, out txnItemList, out ErrMsg);

                        int QueryChargeResult = CIP2BizRules.QueryBestPayAllTxn(qryPhone, txnType, txnChannel, maxReturnRecord, startRecord, out txnItemList, out ErrMsg);

                        //txnType = "131090";

                    //}  // custphone query end
                    //else   // custphone query end
                    //{
                    //    strLog.AppendFormat("ErrMsg:{0}", ErrMsg);

                    //}  // custphone query end

                }
                else
                {
                    Response.Redirect("ErrorInfo.aspx?ErrorInfo=" + ErrMsg);

                }
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

    protected void CreateSPTokenRequest()
    {
        
        SPInfoManager spInfo = new SPInfoManager();
        Object SPData = spInfo.GetSPData(this.Context, "SPData");
        string ScoreSystemSecret = spInfo.GetPropertyBySPID(SPID, "SecretKey", SPData);
     
        //string RequestStr = CryptographyUtil.Decrypt(EncryptSourceStr.ToString(), ScoreSystemSecret);
        String _HeadFooter = "yes";
        String TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); ;

        UserToken UT = new UserToken();
        newSPTokenRequest = UT.GenerateBestAccountMainUserToken(CustID, ReturnUrl, _HeadFooter, TimeStamp, ScoreSystemSecret, out ErrMsg);
        newSPTokenRequest = HttpUtility.UrlEncode(SPID + "$" + newSPTokenRequest);
    }

    protected void ParseSPTokenRequest()
    {
        StringBuilder strLog = new StringBuilder();
        if (CommonUtility.IsParameterExist("SPTokenRequest", this.Page))
        {
            SPTokenRequest = Request["SPTokenRequest"];
            //日志
            strLog.AppendFormat("【SPTokenRequest参数】:" + SPTokenRequest);
            //解析请求参数
            Result = SSOClass.ParseBesttoneAccountPageRequest(SPTokenRequest, this.Context, out SPID, out CustID,
            out HeadFooter, out ReturnUrl, out ErrMsg);
            //日志
            strLog.AppendFormat(String.Format("【解析参数结果】:Result:{0},ErrMsg:{1},SPID:{2},CustID:{3},HeadFooter:{4},stamp:{5},ReturnUrl:{6}", Result, ErrMsg, SPID, CustID, HeadFooter, stamp, ReturnUrl));

            

        }
        log(strLog.ToString());
    }

    protected void log(string str)
    {
        System.Text.StringBuilder msg = new System.Text.StringBuilder();
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        msg.Append(str);
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        BTUCenterInterfaceLog.CenterForBizTourLog("BesttoneAccountMain", msg);
    }


}
