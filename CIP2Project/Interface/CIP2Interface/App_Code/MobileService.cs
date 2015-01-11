using System;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using Linkage.BestTone.Interface.Rule;
using Linkage.BestTone.Interface.BTException;
using Linkage.BestTone.Interface.Utility;
using System.Text;
/// <summary>
/// MobileService 的摘要说明
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class MobileService : System.Web.Services.WebService
{

    public MobileService()
    {

        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 
    }




    public class MobileServiceResult
    {
        public int Result;
        public string ORG_ID;
        public string Flag;
        public string ErrorDescription;

    }




    [WebMethod(Description = "软终端名片认证服务")]
    public MobileServiceResult MobileServiceUserAuth(string Account,string Password)
    {
        MobileServiceResult result = new MobileServiceResult();


        try{
            string[] MobileServiceReturn = new string[3];

            MobileServiceReturn = CustBasicInfo.MobileServiceUserAuthv2(Account, Password);
     
            result.Result = int.Parse(MobileServiceReturn[0]);
            result.ErrorDescription = MobileServiceReturn[1];
            result.ORG_ID = MobileServiceReturn[2];
            result.Flag = MobileServiceReturn[3];

        }
        catch (System.Exception ex)
        {
            result.Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
            result.ErrorDescription = ErrorDefinition.IError_Result_System_UnknowError_Msg + ex.Message;
        }
        finally
        {
            #region WriteLog
            StringBuilder msg = new StringBuilder();
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
            msg.Append("软终端名片认证服务 " + DateTime.Now.ToString("u") + "\r\n");

            msg.Append(";IP - " + HttpContext.Current.Request.UserHostAddress);
            msg.Append("\r\n");

            msg.Append("处理结果 - " + result.Result);
            msg.Append("; 错误描述 - " + result.ErrorDescription);
            msg.Append("; ORGID - " + result.ORG_ID);
          
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

            BTUCenterInterfaceLog.CenterForBizTourLog("MobileServiceUserAuth", msg);
            #endregion

        }

        return result;
    
    
    }

}

