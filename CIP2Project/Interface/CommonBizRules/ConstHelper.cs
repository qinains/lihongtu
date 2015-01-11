using System;
using System.Collections.Generic;
using System.Text;

using System.Xml;
using System.Web.Caching;

namespace Linkage.BestTone.Interface.Rule
{
    /// <summary>
    /// 一些常用变量
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
        /// 初始化
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

                //http拦截配置
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
        /// 号百的SPID
        /// </summary>
        public String BesttoneSPID
        {
            get { return _besttoneSPID; }
            protected set { _besttoneSPID = value; }
        }

        private string _scoreBesttoneSPID;
        /// <summary>
        /// 积分SPID
        /// </summary>
        public String ScoreBesttoneSPID
        {
            get { return _scoreBesttoneSPID; }
            protected set { _scoreBesttoneSPID = value; }
        }

        private string _besttoneHomePage;
        /// <summary>
        /// 号百的门户首页
        /// </summary>
        public String BesttoneHomePage
        {
            get { return _besttoneHomePage; }
            protected set { _besttoneHomePage = value; }
        }

        private string _besttoneLoginPage;
        /// <summary>
        /// 号百门户的登录页面
        /// </summary>
        public String BesttoneLoginPage
        {
            get { return _besttoneLoginPage; }
            protected set { _besttoneLoginPage = value; }
        }

        private string _besttoneRegisterPage;
        /// <summary>
        /// 号百门户的注册页面
        /// </summary>
        public String BesttoneRegisterPage
        {
            get { return _besttoneRegisterPage; }
            protected set { _besttoneRegisterPage = value; }
        }

        private string _besttoneResetPwdByEmail;
        /// <summary>
        /// 号百通过邮件重置页面
        /// </summary>
        public String BesttoneResetPwdByEmail
        {
            get { return _besttoneResetPwdByEmail; }
            protected set { _besttoneResetPwdByEmail = value; }
        }

        private Int32 _resetPwdExpiredHour;
        /// <summary>
        /// 通过邮件重置密码的超时时间
        /// 默认：小时
        /// </summary>
        public Int32 ResetPwdExpiredHour
        {
            get { return _resetPwdExpiredHour; }
            protected set { _resetPwdExpiredHour = value; }
        }

        private string _unWriteDBLogInterface;
        /// <summary>
        /// 不记录数据库日志的接口
        /// </summary>
        public String UnWriteDBLogInterface
        {
            get { return _unWriteDBLogInterface; }
            protected set { _unWriteDBLogInterface = value; }
        }

        private string _unValidateSPIDInterface;
        /// <summary>
        /// 不需要进行SPID验证的接口
        /// </summary>
        public String UnValidateSPIDInterface
        {
            get { return _unValidateSPIDInterface; }
            protected set { _unValidateSPIDInterface = value; }
        }

        private string _unValidateSPID;
        /// <summary>
        /// 不需要进行SPID验证的业务系统
        /// </summary>
        public String UnValidateSPID
        {
            get { return _unValidateSPID; }
            protected set { _unValidateSPID = value; }
        }

        private Boolean _dBLogEnabled;
        /// <summary>
        /// 是否启用数据库日志功能
        /// </summary>
        public Boolean DBLogEnabled
        {
            get { return _dBLogEnabled; }
            protected set { _dBLogEnabled = value; }
        }

        private Boolean _isOpenHttpFilter;
        /// <summary>
        /// 是否启用Http拦截功能
        /// </summary>
        public Boolean IsOpenHttpFilter
        {
            get { return _isOpenHttpFilter; }
            protected set { _isOpenHttpFilter = value; }
        }

        private String _httpFilterMethods;
        /// <summary>
        /// Http拦截方式:post和get两种拦截
        /// </summary>
        public String HttpFilterMethods
        {
            get { return _httpFilterMethods; }
            protected set { _httpFilterMethods = value; }
        }

        #endregion

        #region 私有方法

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
