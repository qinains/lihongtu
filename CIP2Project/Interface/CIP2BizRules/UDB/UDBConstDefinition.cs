using System;
using System.Collections.Generic;
using System.Text;

using System.Xml;

namespace Linkage.BestTone.Interface.Rule
{
    /// <summary>
    /// UDBһЩ����
    /// </summary>
    public class UDBConstDefinition
    {
        private XmlDocument _xmlDoc;
        public UDBConstDefinition()
        {
            _xmlDoc = new XmlDocument();
            _xmlDoc.Load(AppDomain.CurrentDomain.BaseDirectory + @"XmlModel\ConstParams.xml");
        }

        public static UDBConstDefinition DefaultInstance
        {
            get
            {
                return new UDBConstDefinition();
            }
        }




        /// <summary>
        /// UDB�ض�����֤��ҳ��
        /// </summary>
        public String UDBMiddleLoginUrl
        {
            get
            {
                XmlNode node = GetConstUrlByProperty("ID", "UDBMiddleLoginUrl");
                if (node != null)
                    return node.InnerText;
                return String.Empty;
            }
        }

        /// <summary>
        /// UDB�ض�����֤Сҳ��
        /// </summary>
        public String UDBSmallLoginUrl
        {
            get
            {
                XmlNode node = GetConstUrlByProperty("ID", "UDBSmallLoginUrl");
                return node == null ? String.Empty : node.InnerText;
            }
        }

        /// <summary>
        /// UDB�ض���ǳ��ӿڵ�ַ
        /// </summary>
        public String UDBLogoutUrl
        {
            get
            {
                XmlNode node = GetConstUrlByProperty("ID", "UDBLogoutUrl");
                return node == null ? String.Empty : node.InnerText;
            }
        }
        
        /// <summary>
        /// UDB�ض���������֤��ַ
        /// </summary>
        public String UDBTicketCheckUrl
        {
            get
            {
                XmlNode node = GetConstUrlByProperty("ID", "UDBTicketCheckUrl");
                return node == null ? String.Empty : node.InnerText;
            }
        }

        /// <summary>
        /// ����SOAP��ַ
        /// </summary>
        public String BJSOAPUrl
        {
            get
            {
                XmlNode node = GetConstUrlByProperty("ID", "BJSOAPUrl");
                return node == null ? String.Empty : node.InnerText;
            }
        }

        /// <summary>
        /// �Ϻ�SOAP��ַ
        /// </summary>
        public String SHSOAPUrl
        {
            get
            {
                XmlNode node = GetConstUrlByProperty("ID", "SHSOAPUrl");
                return node == null ? String.Empty : node.InnerText;
            }
        }

        /// <summary>
        /// ����webservice��ʽ
        /// </summary>
        public String UserWebServiceMethod
        {
            get
            {
                XmlNode node = GetConstParamByProperty("ID", "UserWebServiceMethod");
                return node == null ? String.Empty : node.InnerText;
            }
        }

        /// <summary>
        /// UDB�ںŰٿͻ���Ϣƽ̨��SPID
        /// </summary>
        public String UDBSPID
        {
            get
            {
                XmlNode node = GetConstParamByProperty("ID", "UDBSPID");
                return node == null ? String.Empty : node.InnerText;
            }
        }

        /// <summary>
        /// UDB��¼��ķ��ؽ���ҳ��
        /// </summary>
        public String UDBLoginParseUrl
        {
            get
            {
                XmlNode node = GetConstUrlByProperty("ID", "UDBLoginParseUrl");
                return node == null ? String.Empty : node.InnerText;
            }
        }

        /// <summary>
        /// UDB��¼�ɹ��Ժ���תҳ��
        /// </summary>
        public String UDBLoginSuccessRedirectUrl
        {
            get
            {
                XmlNode node = GetConstUrlByProperty("ID", "UDBLoginSuccessRedirectUrl");
                return node == null ? String.Empty : node.InnerText;
            }
        }

        /// <summary>
        /// UDBSPID
        /// </summary>
        public String BesttoneUDBSPID
        {
            get
            {
                XmlNode node = GetConstUrlByProperty("ID", "BesttoneUDBSPID");
                return node == null ? String.Empty : node.InnerText;
            }
        }


        public String UnifyPlatformWebRegisterUrl
        {
            get
            {
                XmlNode node = GetConstUrlByProperty("ID", "UnifyPlatformWebRegisterUrl");
                if (node != null)
                    return node.InnerText;
                return String.Empty;
            }
        }

        public String UnifyPlatformWebRegisterCallBackUrl
        {
            get
            {
                XmlNode node = GetConstUrlByProperty("ID", "UnifyPlatformWebRegisterCallBackUrl");
                if (node != null)
                    return node.InnerText;
                return String.Empty;
            }
        }


        public String UnifyPlatformWapRegisterUrl
        {
            get
            {
                XmlNode node = GetConstUrlByProperty("ID", "UnifyPlatformWapRegisterUrl");
                if (node != null)
                    return node.InnerText;
                return String.Empty;
            }
        }

        public String UnifyPlatformWapFindPwdUrl
        {
            get
            {
                XmlNode node = GetConstUrlByProperty("ID", "UnifyPlatformWapFindPwdUrl");
                if (node != null)
                    return node.InnerText;
                return String.Empty;
            }
        }

        public String UnifyPlatformWapRegisterCallBackUrl
        {
            get
            {
                XmlNode node = GetConstUrlByProperty("ID", "UnifyPlatformWapRegisterCallBackUrl");
                if (node != null)
                    return node.InnerText;
                return String.Empty;
            }
        }

        public String UnifyPlatformLogonUrl
        {
            get
            {
                XmlNode node = GetConstUrlByProperty("ID", "UnifyPlatformLogonUrl");
                if (node != null)
                    return node.InnerText;
                return String.Empty;
            }
        }

        public String UnifyPlatformLogoutUrl
        {
            get
            {
                XmlNode node = GetConstUrlByProperty("ID", "UnifyPlatformLogoutUrl");
                if (node != null)
                    return node.InnerText;
                return String.Empty;
            }
        }


        public String UnifyPlatformCallBackUrl
        {
            get
            {
                XmlNode node = GetConstUrlByProperty("ID", "UnifyPlatformCallBackUrl");
                return node == null ? String.Empty : node.InnerText;
            }
        }

              
       
        public String UnifyPlatformCallBackForClientUrl
        {
            get
            {
                XmlNode node = GetConstUrlByProperty("ID", "UnifyPlatformCallBackForClientUrl");
                return node == null ? String.Empty : node.InnerText;
            }
        }

        public String UnifyPlatformCallBackForWapUrl
        {
            get
            {
                XmlNode node = GetConstUrlByProperty("ID", "UnifyPlatformCallBackForWapUrl");
                return node == null ? String.Empty : node.InnerText;
            }
        }

      
        public String UnifyPlatformFindPwdUrl
        {
            get
            {
                XmlNode node = GetConstUrlByProperty("ID", "UnifyPlatformFindPwdUrl");
                return node == null ? String.Empty : node.InnerText;
            }
        }

        public String UnifyAccountRegisterUrl
        {
            get
            {
                XmlNode node = GetConstUrlByProperty("ID", "UnifyAccountRegisterUrl");
                return node == null ? String.Empty : node.InnerText;
            }
        }
        public String UnifyPlatformUpdatePwdUrl
        {
            get
            {
                XmlNode node = GetConstUrlByProperty("ID", "UnifyPlatformUpdatePwdUrl");
                return node == null ? String.Empty : node.InnerText;
            }
        }
        public String UnifyPlatformUpdateYingShiLoginUrl
        {
            get
            {
                XmlNode node = GetConstUrlByProperty("ID", "UnifyPlatformUpdateYingShiLoginUrl");
                return node == null ? String.Empty : node.InnerText;
            }
        }

        public String UnifyPlatformLoginByIMSIUrl
        {
            get
            {
                XmlNode node = GetConstUrlByProperty("ID", "UnifyPlatformLoginByIMSIUrl");
                return node == null ? String.Empty : node.InnerText;
            }
        }

        public String UnifyPlatformGetUserInfoUrl
        {
            get
            {
                XmlNode node = GetConstUrlByProperty("ID", "UnifyPlatformGetUserInfoUrl");
                return node == null ? String.Empty : node.InnerText;
            }
        }

        public String UnifyPlatformUpdateUserInfoUrl
        {
            get
            {
                XmlNode node = GetConstUrlByProperty("ID", "UnifyPlatformUpdateUserInfoUrl");
                return node == null ? String.Empty : node.InnerText;
            }
        }

        public String UnifyPlatformUpdateUserNameUrl
        {
            get
            {
                XmlNode node = GetConstUrlByProperty("ID", "UnifyPlatformUpdateUserNameUrl");
                return node == null ? String.Empty : node.InnerText;
            }
        }



        public String UnifyPlatformIsUserNameExistUrl
        {
            get
            {
                XmlNode node = GetConstUrlByProperty("ID", "UnifyPlatformIsUserNameExistUrl");
                return node == null ? String.Empty : node.InnerText;
            }
        }
        public String UnifyPlatformGetUserInfoByNameUrl
        {
            get
            {
                XmlNode node = GetConstUrlByProperty("ID", "UnifyPlatformGetUserInfoByNameUrl");
                return node == null ? String.Empty : node.InnerText;
            }
        }

        public String UnifyPlatformRegisterAccountUrl
        {
            get
            {
                XmlNode node = GetConstUrlByProperty("ID", "UnifyPlatformRegisterAccountUrl");
                return node == null ? String.Empty : node.InnerText;
            }
        }




        public String UnifyPlatformRegisterUrl
        {
            get
            {
                XmlNode node = GetConstUrlByProperty("ID", "UnifyPlatformRegisterUrl");
                return node == null ? String.Empty : node.InnerText;
            }
        }


        public String UnifyPlatformLoginUrl
        {
            get
            {
                XmlNode node = GetConstUrlByProperty("ID", "UnifyPlatformLoginUrl");
                return node == null ? String.Empty : node.InnerText;
            }
        }

        public String UnifyPlatformCaptchaUrl
        {
            get
            {
                XmlNode node = GetConstUrlByProperty("ID", "UnifyPlatformCaptchaUrl");
                return node == null ? String.Empty : node.InnerText;
            }
        }

        public String UnifyAccountCheckUrl
        {
            get
            {
                XmlNode node = GetConstUrlByProperty("ID", "UnifyPlatformAccountCheckUrl");
                return node == null ? String.Empty : node.InnerText;
            }
        }

        public String UnifyAccountCheckCallBackUrl
        {
            get
            {
                XmlNode node = GetConstUrlByProperty("ID", "UnifyAccountCheckCallBackUrl");
                return node == null ? String.Empty : node.InnerText;
            }
        }

        public String UnifyPlatformWebRegisterCallbackOnAccountCheckCallackUrl
        {
            get
            {
                XmlNode node = GetConstUrlByProperty("ID", "UnifyPlatformWebRegisterCallbackOnAccountCheckCallackUrl");
                return node == null ? String.Empty : node.InnerText;
            }
        }

        public String UnifyPlatformWapRegisterCallbackOnAccountCheckCallackUrl
        {
            get
            {
                XmlNode node = GetConstUrlByProperty("ID", "UnifyPlatformWapRegisterCallbackOnAccountCheckCallackUrl");
                return node == null ? String.Empty : node.InnerText;
            }
        }

        public String UnifyAccountCheckCallBackUrlYY
        {
            get
            {
                XmlNode node = GetConstUrlByProperty("ID", "UnifyAccountCheckCallBackUrlYY");
                return node == null ? String.Empty : node.InnerText;
            }
        }

        public String UnifyPlatformAppId
        {
            get
            {
                XmlNode node = GetConstParamByProperty("ID", "UnifyPlatformAppId");
                return node == null ? String.Empty : node.InnerText;
            }
        }
        
        public String UnifyPlatformAppSecret
        {
            get
            {
                XmlNode node = GetConstParamByProperty("ID", "UnifyPlatformAppSecret");
                return node == null ? String.Empty : node.InnerText;
            }
        }

        public String UnifyPlatformVersion
        {
            get
            {
                XmlNode node = GetConstParamByProperty("ID", "UnifyPlatformVersion");
                return node == null ? String.Empty : node.InnerText;
            }
        }

        public String UnifyPlatformClientType
        {
            get
            {
                XmlNode node = GetConstParamByProperty("ID", "UnifyPlatformClientType");
                return node == null ? String.Empty : node.InnerText;
            }
        }


        public String UnifyPlatformAccountType
        {
            get
            {
                XmlNode node = GetConstParamByProperty("ID", "UnifyPlatformAccountType");
                return node == null ? String.Empty : node.InnerText;
            }
        }

        public String UnifyPlatformPageKey
        {
            get
            {
                XmlNode node = GetConstParamByProperty("ID", "UnifyPlatformPageKey");
                return node == null ? String.Empty : node.InnerText;
            }
        
        }

        public String UnifyPlatformBusinessPage
        {
            get
            {
                XmlNode node = GetConstParamByProperty("ID", "UnifyPlatformBusinessPage");
                return node == null ? String.Empty : node.InnerText;
            }
        }

        public String UnifyPlatformThirdAccount
        {
            get
            {
                XmlNode node = GetConstParamByProperty("ID", "UnifyPlatformThirdAccount");
                return node == null ? String.Empty : node.InnerText;
            }
        }

        public String UnifyPlatformRegisterAccountSendSms
        {
            get
            {
                XmlNode node = GetConstParamByProperty("ID", "UnifyPlatformRegisterAccountSendSms");
                return node == null ? String.Empty : node.InnerText;
            }
        }
        public String UnifyPlatformMustBind
        {
            get
            {
                XmlNode node = GetConstParamByProperty("ID", "UnifyPlatformMustBind");
                return node == null ? String.Empty : node.InnerText;
            }
        
        }

        public String UnifyPlatformQuicklogin
        {
            get
            {
                XmlNode node = GetConstParamByProperty("ID", "UnifyPlatformQuicklogin");
                return node == null ? String.Empty : node.InnerText;
            }
        }

 
        /// <summary>
        /// UDBKey
        /// </summary>
        public String BesttoneUDBKey
        {
            get
            {
                XmlNode node = GetConstUrlByProperty("ID", "BesttoneUDBKey");
                return node == null ? String.Empty : node.InnerText;
            }
        }

        #region ˽�к���

        private XmlNode GetNodeByName(String nodeName)
        {
            XmlNodeList nodeList = _xmlDoc.GetElementsByTagName(nodeName);
            if (nodeList != null && nodeList.Count > 0)
            {
                return nodeList[0];
            }

            return null;
        }

        private XmlNode GetNodeByProperty(String propertyName,String propertyValue)
        {
            XmlNode node = _xmlDoc.SelectSingleNode("//*[@" + propertyName + "='" + propertyValue + "']");
            return node;
        }

        private XmlNode GetConstUrlByProperty(String propertyName, String propertyValue)
        {
            XmlNode node = _xmlDoc.SelectSingleNode("ConstRoot/ConstUrls/Url[@" + propertyName + "='" + propertyValue + "']");
            return node;
        }

        private XmlNode GetConstParamByProperty(String propertyName, String propertyValue)
        {
            XmlNode node = _xmlDoc.SelectSingleNode("ConstRoot/ConstParams/Parameter[@" + propertyName + "='" + propertyValue + "']");
            return node;
        }

        #endregion
    }
}
