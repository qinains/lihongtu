using System;
using System.Collections.Generic;
using System.Text;

using System.Xml;
using System.Web.Caching;

namespace Linkage.BestTone.Interface.Rule
{
    /// <summary>
    /// һЩ���ñ���
    /// </summary>
    public sealed class ConstHelper
    {
        private XmlDocument xmlDoc;
        private ConstHelper()
        {
            xmlDoc = new XmlDocument();
            String filePath = AppDomain.CurrentDomain.BaseDirectory + @"XmlModel\ConstParams.xml";
            xmlDoc.Load(filePath);
            Init();
        }

        public static ConstHelper DefaultInstance
        {
            get
            {
                ConstHelper instance = null;
                if (System.Web.HttpRuntime.Cache["ConstParameters"] == null)
                {
                    instance = new ConstHelper();
                    CacheDependency dependency = new CacheDependency(AppDomain.CurrentDomain.BaseDirectory + @"XmlModel\ConstParams.xml");

                    System.Web.HttpRuntime.Cache.Insert("ConstParameters", instance, dependency);
                }
                instance = (ConstHelper)System.Web.HttpRuntime.Cache["ConstParameters"];

                return instance;
            }
        }

        /// <summary>
        /// ��ʼ��
        /// </summary>
        private void Init()
        {
            try
            {
                BesttoneSPID = GetConstParamsNodeByProperty("ID", "BesttoneSPID") == null ? String.Empty : GetConstParamsNodeByProperty("ID", "BesttoneSPID").InnerText;
                ScoreBesttoneSPID = GetConstParamsNodeByProperty("ID", "ScoreBesttoneSPID") == null ? String.Empty : GetConstParamsNodeByProperty("ID", "ScoreBesttoneSPID").InnerText;
                BesttoneHomePage = GetConstUrlNodeByProperty("ID", "BesttoneHomePage") == null ? String.Empty : GetConstUrlNodeByProperty("ID", "BesttoneHomePage").InnerText;
                BesttoneLoginPage = GetConstUrlNodeByProperty("ID", "BesttoneLoginPage") == null ? String.Empty : GetConstUrlNodeByProperty("ID", "BesttoneLoginPage").InnerText;
                BesttoneRegisterPage = GetConstUrlNodeByProperty("ID", "BesttoneRegisterPage") == null ? String.Empty : GetConstUrlNodeByProperty("ID", "BesttoneRegisterPage").InnerText;
                BesttoneResetPwdByEmail = GetNodeByProperty("ID", "BesttoneResetPwdByEmail") == null ? String.Empty : GetNodeByProperty("ID", "BesttoneResetPwdByEmail").InnerText;
                ResetPwdExpiredHour = GetConstParamsNodeByProperty("ID", "ResetPwdExpiredHour") == null ? 0 : Convert.ToInt32(GetConstParamsNodeByProperty("ID", "ResetPwdExpiredHour").InnerText);
                UnWriteDBLogInterface = GetConstParamsNodeByProperty("ID", "UnWriteDBLogInterface") == null ? String.Empty : GetConstParamsNodeByProperty("ID", "UnWriteDBLogInterface").InnerText;
                UnValidateSPIDInterface = GetConstParamsNodeByProperty("ID", "UnValidateSPIDInterface") == null ? String.Empty : GetConstParamsNodeByProperty("ID", "UnValidateSPIDInterface").InnerText;
                UnValidateSPID = GetConstParamsNodeByProperty("ID", "UnValidateSPID") == null ? String.Empty : GetConstParamsNodeByProperty("ID", "UnValidateSPID").InnerText;

                Boolean result = true;
                XmlNode node = GetConstParamsNodeByProperty("ID", "DBLogEnabled");
                if (node != null)
                {
                    result = node.InnerText == "0" ? true : false;
                }
                DBLogEnabled = result;

                //http��������
                node = null;
                node = GetNodeByPath(@"ConstRoot/HttpFilter");
                if (node != null)
                {
                    IsOpenHttpFilter = node.Attributes["IsOpen"].Value == "1" ? true : false;
                    HttpFilterMethods = node.SelectSingleNode("FilterMethods").InnerText;
                }
            }
            catch { }
        }

        #region Public Members
     

        private string _besttoneSPID;
        /// <summary>
        /// �Űٵ�SPID
        /// </summary>
        public String BesttoneSPID
        {
            get { return _besttoneSPID; }
            protected set { _besttoneSPID = value; }
        }

        private string _scoreBesttoneSPID;
        /// <summary>
        /// ����SPID
        /// </summary>
        public String ScoreBesttoneSPID
        {
            get { return _scoreBesttoneSPID; }
            protected set { _scoreBesttoneSPID = value; }
        }

        private string _besttoneHomePage;
        /// <summary>
        /// �Űٵ��Ż���ҳ
        /// </summary>
        public String BesttoneHomePage
        {
            get { return _besttoneHomePage; }
            protected set { _besttoneHomePage = value; }
        }

        private string _besttoneLoginPage;
        /// <summary>
        /// �Ű��Ż��ĵ�¼ҳ��
        /// </summary>
        public String BesttoneLoginPage
        {
            get { return _besttoneLoginPage; }
            protected set { _besttoneLoginPage = value; }
        }

        private string _besttoneRegisterPage;
        /// <summary>
        /// �Ű��Ż���ע��ҳ��
        /// </summary>
        public String BesttoneRegisterPage
        {
            get { return _besttoneRegisterPage; }
            protected set { _besttoneRegisterPage = value; }
        }

        private string _besttoneResetPwdByEmail;
        /// <summary>
        /// �Ű�ͨ���ʼ�����ҳ��
        /// </summary>
        public String BesttoneResetPwdByEmail
        {
            get { return _besttoneResetPwdByEmail; }
            protected set { _besttoneResetPwdByEmail = value; }
        }

        private Int32 _resetPwdExpiredHour;
        /// <summary>
        /// ͨ���ʼ���������ĳ�ʱʱ��
        /// Ĭ�ϣ�Сʱ
        /// </summary>
        public Int32 ResetPwdExpiredHour
        {
            get { return _resetPwdExpiredHour; }
            protected set { _resetPwdExpiredHour = value; }
        }

        private string _unWriteDBLogInterface;
        /// <summary>
        /// ����¼���ݿ���־�Ľӿ�
        /// </summary>
        public String UnWriteDBLogInterface
        {
            get { return _unWriteDBLogInterface; }
            protected set { _unWriteDBLogInterface = value; }
        }

        private string _unValidateSPIDInterface;
        /// <summary>
        /// ����Ҫ����SPID��֤�Ľӿ�
        /// </summary>
        public String UnValidateSPIDInterface
        {
            get { return _unValidateSPIDInterface; }
            protected set { _unValidateSPIDInterface = value; }
        }

        private string _unValidateSPID;
        /// <summary>
        /// ����Ҫ����SPID��֤��ҵ��ϵͳ
        /// </summary>
        public String UnValidateSPID
        {
            get { return _unValidateSPID; }
            protected set { _unValidateSPID = value; }
        }

        private Boolean _dBLogEnabled;
        /// <summary>
        /// �Ƿ��������ݿ���־����
        /// </summary>
        public Boolean DBLogEnabled
        {
            get { return _dBLogEnabled; }
            protected set { _dBLogEnabled = value; }
        }

        private Boolean _isOpenHttpFilter;
        /// <summary>
        /// �Ƿ�����Http���ع���
        /// </summary>
        public Boolean IsOpenHttpFilter
        {
            get { return _isOpenHttpFilter; }
            protected set { _isOpenHttpFilter = value; }
        }

        private String _httpFilterMethods;
        /// <summary>
        /// Http���ط�ʽ:post��get��������
        /// </summary>
        public String HttpFilterMethods
        {
            get { return _httpFilterMethods; }
            protected set { _httpFilterMethods = value; }
        }

        #endregion

        #region ˽�з���

        private XmlNode GetNodeByName(String nodeName)
        {
            XmlNodeList list = xmlDoc.GetElementsByTagName(nodeName);
            if (list != null)
                return list[0];
            return null;
        }

        private XmlNode GetNodeByPath(String path)
        {
            return xmlDoc.SelectSingleNode(path);
        }

        private XmlNode GetNodeByProperty(String property, String propertyValue)
        {
            XmlNode node = xmlDoc.SelectSingleNode("//*[@" + property + "='" + propertyValue + "']");
            return node;
        }

        private XmlNode GetConstUrlNodeByProperty(String property, String propertyValue)
        {
            XmlNode node = xmlDoc.SelectSingleNode("ConstRoot/ConstUrls/Url[@" + property + "='" + propertyValue + "']");
            return node;
        }

        private XmlNode GetConstParamsNodeByProperty(String property, String propertyValue)
        {
            XmlNode node = xmlDoc.SelectSingleNode("ConstRoot/ConstParams/Parameter[@" + property + "='" + propertyValue + "']");
            return node;
        }

        #endregion
    }

}
