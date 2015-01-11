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

        #region �û��ʺ���Ϣ��ѯ����

        /// <summary>
        /// ƴ��UDB�û���Ϣ��ѯxml
        /// </summary>
        public static String BuildAccountInfoQueryXml(String SrcSsDeviceNo, String AuthSsDeviceNo, String UDBTicket)
        {
            String returnXml = String.Empty;
            String timeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            String key = String.Empty;
            String digest = CryptographyUtil.Decrypt(SrcSsDeviceNo + AuthSsDeviceNo + UDBTicket + timeStamp, key);

            //����xml�ĵ�
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement xmlElem;
            XmlText elemText;
            //�������
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
        /// ������UDB��ѯ�û���Ϣ���ص�xml
        /// </summary>
        public static Int32 ParseAccountInfoQueryXml(String xmlStr, out UDBAccountInfo accountInfo, out String ErrMsg)
        {
            Int32 Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
            accountInfo = new UDBAccountInfo();
            try
            {

                //����xml
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlStr);
                XmlNamespaceManager manager = new XmlNamespaceManager(xmlDoc.NameTable);
                manager.AddNamespace("UDB", "http://udb.chinatelecom.com");

                //����xml����
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
        /// UDB�û�������ͻ���Ϣƽ̨��Ӧ����ת��
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
        /// д��־����
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
            public String pUserId;//  String 12  UDB �˺ŵ�ID
            public String productUid ;// String 30  �����˺�ԭuserId  ����Ϊ�� ��appId�й�
            public int userType;//  Integer 4   �˺�����  ��δʹ��
            public int status;// Integer 4 �˺�״̬
            public String userName;// String 30
            public String zhUserName;// String 30 �ۺ�ƽ̨UserName 
            public String mobileName;// String 11 ���ֻ���
            public String emailName;// String60 ��������
            public String mobileList;// String 300 �󶨵��ֻ����б�  181000001,181....
            public String emailList;// String 300 �󶨵������б�  
            public String aliasName;// String 30  ���ֻ��ű���
            public String nickName;// String 30 �ǳ�
            public int gender;// Integer  1 �Ա�
            public String province;//  String 30 ����ʡ 
            public String city;//  String 60 ������
            public String birthday;//  Striing 8   yyyy-MM-dd
            public String address;//  String 300 ��ϵ��ַ
            public String mail;// String 60  ��ϵ����
            public String qq;//  String 50 qq�˺�
            public String position;//  String ְλ
            public String intro;// String 300 ���˽���
            public String userIconUrl1;//  String 256 �û�ͷ������  150*150
            public String userIconUrl2;//  String 256 �û�ͷ������  50*50

    
    }

    /// <summary>
    /// UDB�����û��ʺ���Ϣ
    /// </summary>
    public class UDBAccountInfo
    {
        public String SourceSPID;
        /// <summary>
        /// ͳһ�ʺ�
        /// </summary>
        public String UserID;
        /// <summary>
        /// �ʺ�����
        /// </summary>
        public String UserIDType;
        /// <summary>
        /// �û�����
        /// </summary>
        public String UserType;
        /// <summary>
        /// �û�Ψһ��ʶλ
        /// </summary>
        public String PUserID;
        /// <summary>
        /// �û�������Ϣ
        /// </summary>
        public IList<UserAccount> ReturnUserGroupList;
        /// <summary>
        /// �û�����
        /// </summary>
        public String Alias;
        /// <summary>
        /// �󶨵Ŀ�������ʺ�
        /// </summary>
        public String BindingAccessNo;
        /// <summary>
        /// �󶨵ĵ�����Ӧ�õ�ԭ�ʺ�
        /// </summary>
        public String ThirdSsUserID;
        /// <summary>
        /// ͳһ�ʺ�״̬
        /// </summary>
        public String UserIDStatus;
        /// <summary>
        /// �û��ڸ�Ӧ��ϵͳע��״̬
        /// </summary>
        public String UserIDSsStatus;
        /// <summary>
        /// �û���������
        /// </summary>
        public String UserPayType;
        /// <summary>
        /// �û���������
        /// </summary>
        public String PrePaySystemNo;
        /// <summary>
        /// ������Ϣ
        /// </summary>
        public String Description;

        /// <summary>
        /// ʡID
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
