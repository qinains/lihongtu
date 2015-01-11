using System;
using System.Collections.Generic;
using System.Text;

using System.Xml;

using Linkage.BestTone.Interface.BTException;
using Linkage.BestTone.Interface.Utility;

namespace Linkage.BestTone.Interface.Rule
{
    public class UDBBusiness
    {

        #region 用户帐号信息查询解析

        /// <summary>
        /// 拼凑UDB用户信息查询xml
        /// </summary>
        public static String BuildAccountInfoQueryXml(String SrcSsDeviceNo, String AuthSsDeviceNo, String UDBTicket)
        {
            String returnXml = String.Empty;
            String timeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            String key = String.Empty;
            String digest = CryptographyUtil.Decrypt(SrcSsDeviceNo + AuthSsDeviceNo + UDBTicket + timeStamp, key);

            //声明xml文档
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement xmlElem;
            XmlText elemText;
            //添加声明
            XmlDeclaration xmlDeclare = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDoc.AppendChild(xmlDeclare);

            XmlElement rootNode = xmlDoc.CreateElement("AccountInfoCheckRequest");
            xmlElem = xmlDoc.CreateElement("Authenticator");
            elemText = xmlDoc.CreateTextNode(digest);
            xmlElem.AppendChild(elemText);
            rootNode.AppendChild(xmlElem);

            xmlElem = xmlDoc.CreateElement("SrcSsDeviceNo");
            elemText = xmlDoc.CreateTextNode(SrcSsDeviceNo);
            xmlElem.AppendChild(elemText);
            rootNode.AppendChild(xmlElem);

            xmlElem = xmlDoc.CreateElement("AuthSsDeviceNo");
            elemText = xmlDoc.CreateTextNode(AuthSsDeviceNo);
            xmlElem.AppendChild(elemText);
            rootNode.AppendChild(xmlElem);

            xmlElem = xmlDoc.CreateElement("AuthSsDeviceNo");
            elemText = xmlDoc.CreateTextNode(AuthSsDeviceNo);
            xmlElem.AppendChild(elemText);
            rootNode.AppendChild(xmlElem);

            xmlElem = xmlDoc.CreateElement("UDBTicket");
            elemText = xmlDoc.CreateTextNode(UDBTicket);
            xmlElem.AppendChild(elemText);
            rootNode.AppendChild(xmlElem);

            xmlElem = xmlDoc.CreateElement("TimeStamp");
            elemText = xmlDoc.CreateTextNode(timeStamp);
            xmlElem.AppendChild(elemText);
            rootNode.AppendChild(xmlElem);

            xmlDoc.AppendChild(rootNode);

            returnXml = xmlDoc.OuterXml;

            return returnXml;

        }

        /// <summary>
        /// 解析从UDB查询用户信息返回的xml
        /// </summary>
        public static Int32 ParseAccountInfoQueryXml(String xmlStr, out UDBAccountInfo accountInfo, out String ErrMsg)
        {
            Int32 Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
            accountInfo = new UDBAccountInfo();
            try
            {

                //加载xml
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlStr);
                XmlNamespaceManager manager = new XmlNamespaceManager(xmlDoc.NameTable);
                manager.AddNamespace("UDB", "http://udb.chinatelecom.com");

                //解析xml数据
                Result = Convert.ToInt32(xmlDoc.SelectSingleNode("/UDB:AccountInfoCheckResult/UDB:ResultCode", manager).InnerText);
                accountInfo.UserType = xmlDoc.SelectSingleNode("/UDB:AccountInfoCheckResult/UDB:UserType", manager).InnerText;

                XmlNode node = xmlDoc.SelectSingleNode("/UDB:AccountInfoCheckResult/UDB:ReturnUserGroupList", manager);

                accountInfo.UserID = xmlDoc.SelectSingleNode("/UDB:AccountInfoCheckResult/UDB:UserID", manager).InnerText;
                accountInfo.UserIDType = xmlDoc.SelectSingleNode("/UDB:AccountInfoCheckResult/UDB:UserIDType", manager).InnerText;
                accountInfo.PUserID = xmlDoc.SelectSingleNode("/UDB:AccountInfoCheckResult/UDB:PUserID", manager).InnerText;
                accountInfo.Alias = xmlDoc.SelectSingleNode("/UDB:AccountInfoCheckResult/UDB:Alias", manager).InnerText;
                accountInfo.BindingAccessNo = xmlDoc.SelectSingleNode("/UDB:AccountInfoCheckResult/UDB:BindingAccessNo", manager).InnerText;
                accountInfo.ThirdSsUserID = xmlDoc.SelectSingleNode("/UDB:AccountInfoCheckResult/UDB:ThirdSsUserID", manager).InnerText;
                accountInfo.UserIDStatus = xmlDoc.SelectSingleNode("/UDB:AccountInfoCheckResult/UDB:UserIDStatus", manager).InnerText;
                accountInfo.UserIDSsStatus = xmlDoc.SelectSingleNode("/UDB:AccountInfoCheckResult/UDB:UserIDSsStatus", manager).InnerText;
                accountInfo.UserPayType = xmlDoc.SelectSingleNode("/UDB:AccountInfoCheckResult/UDB:UserPayType", manager).InnerText;
                accountInfo.PrePaySystemNo = xmlDoc.SelectSingleNode("/UDB:AccountInfoCheckResult/UDB:PrePaySystemNo", manager).InnerText;
                String temp_Description = xmlDoc.SelectSingleNode("/UDB:AccountInfoCheckResult/UDB:Description", manager).InnerText;
                accountInfo.Description = xmlDoc.SelectSingleNode("/UDB:AccountInfoCheckResult/UDB:Description", manager).InnerText;
                if (Result == 0 || Result == 5)
                {
                    //<Ex><PID>07</PID><NF>2</NF></Ex>
                    Int32 startIndex = temp_Description.IndexOf("<PID>");
                    Int32 endIndex = temp_Description.IndexOf("</PID>");
                    accountInfo.ProvinceID = temp_Description.Substring(startIndex + 5, endIndex - startIndex - 5);
                    startIndex = temp_Description.IndexOf("<NF>");
                    endIndex = temp_Description.IndexOf("</NF>");
                    accountInfo.NumFlag = temp_Description.Substring(startIndex + 4, endIndex - startIndex - 4);
                }
                else
                {
                    ErrMsg = xmlDoc.SelectSingleNode("/UDB:AccountInfoCheckResult/UDB:Description", manager).InnerText;
                }
            }
            catch (Exception ex)
            {
                ErrMsg += ex.Message;
            }

            return Result;
        }

        #endregion

        /// <summary>
        /// UDB用户类型与客户信息平台对应类型转换
        /// </summary>
        public static String ConvertAuthenType(String UDBType)
        {
            String type = String.Empty;
            switch (UDBType)
            { 
                case "1":
                    type = "9";
                    break;
                case "2":
                    type = "2";
                    break;
                case "3":
                    type = "11";
                    break;
                default:
                    type = "";
                    break;
            }
            return type;

        }

        /// <summary>
        /// 写日志功能
        /// </summary>
        protected static void WriteLog(String str)
        {
            StringBuilder msg = new StringBuilder();
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
            msg.Append(str);
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
            BTUCenterInterfaceLog.CenterForBizTourLog("UDBBusiness", msg);
        }

    }


    public class UnifyAccountInfo
    { 
            public long userId;//    Long 18 
            public String pUserId;//  String 12  UDB 账号的ID
            public String productUid ;// String 30  基地账号原userId  可以为空 和appId有关
            public int userType;//  Integer 4   账号类型  暂未使用
            public int status;// Integer 4 账号状态
            public String userName;// String 30
            public String zhUserName;// String 30 综合平台UserName 
            public String mobileName;// String 11 主手机号
            public String emailName;// String60 主油箱名
            public String mobileList;// String 300 绑定的手机号列表  181000001,181....
            public String emailList;// String 300 绑定的邮箱列表  
            public String aliasName;// String 30  主手机号别名
            public String nickName;// String 30 昵称
            public int gender;// Integer  1 性别
            public String province;//  String 30 所属省 
            public String city;//  String 60 所属市
            public String birthday;//  Striing 8   yyyy-MM-dd
            public String address;//  String 300 联系地址
            public String mail;// String 60  联系邮箱
            public String qq;//  String 50 qq账号
            public String position;//  String 职位
            public String intro;// String 300 个人介绍
            public String userIconUrl1;//  String 256 用户头像链接  150*150
            public String userIconUrl2;//  String 256 用户头像链接  50*50

    
    }

    /// <summary>
    /// UDB返回用户帐号信息
    /// </summary>
    public class UDBAccountInfo
    {
        public String SourceSPID;
        /// <summary>
        /// 统一帐号
        /// </summary>
        public String UserID;
        /// <summary>
        /// 帐号类型
        /// </summary>
        public String UserIDType;
        /// <summary>
        /// 用户类型
        /// </summary>
        public String UserType;
        /// <summary>
        /// 用户唯一标识位
        /// </summary>
        public String PUserID;
        /// <summary>
        /// 用户数组信息
        /// </summary>
        public IList<UserAccount> ReturnUserGroupList;
        /// <summary>
        /// 用户别名
        /// </summary>
        public String Alias;
        /// <summary>
        /// 绑定的宽带接入帐号
        /// </summary>
        public String BindingAccessNo;
        /// <summary>
        /// 绑定的第三方应用的原帐号
        /// </summary>
        public String ThirdSsUserID;
        /// <summary>
        /// 统一帐号状态
        /// </summary>
        public String UserIDStatus;
        /// <summary>
        /// 用户在该应用系统注册状态
        /// </summary>
        public String UserIDSsStatus;
        /// <summary>
        /// 用户付费类型
        /// </summary>
        public String UserPayType;
        /// <summary>
        /// 用户付费类型
        /// </summary>
        public String PrePaySystemNo;
        /// <summary>
        /// 其他信息
        /// </summary>
        public String Description;

        /// <summary>
        /// 省ID
        /// </summary>
        public String ProvinceID;

        public String NumFlag;

        public UDBAccountInfo()
        {
            ReturnUserGroupList = new List<UserAccount>();
        }
    }

    public class UserAccount
    {
        private string _userID;
        private string _userIDType;

        public String UserID
        {
            get { return _userID; }
            set { _userID = value; }
        }

        public String UserIDType
        {
            get { return _userIDType; }
            set { _userIDType = value; }
        }
    }

}
