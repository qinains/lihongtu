using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using System.Web.Caching;
using System.Web;

namespace Linkage.BestTone.Interface.Rule
{
    /// <summary>
    /// 号码百事通账户一些配置数据
    /// </summary>
    public class BesttoneAccountConstDefinition
    {
        private XmlDocument xmlDoc;
        private static string fileName = AppDomain.CurrentDomain.BaseDirectory + @"XmlModel\BesttoneAccountConfig.xml";
        private static BesttoneAccountConstDefinition _default;
        public static BesttoneAccountConstDefinition DefaultInstance
        {
            get 
            {
                BesttoneAccountConstDefinition instance = null;
                if (HttpRuntime.Cache["BesttoneAccountConfigCache"] == null)
                {
                    instance = new BesttoneAccountConstDefinition();
                    CacheDependency dependency = new CacheDependency(fileName);

                    HttpRuntime.Cache.Insert("BesttoneAccountConfigCache", instance, dependency);
                }
                instance = (BesttoneAccountConstDefinition)HttpRuntime.Cache["BesttoneAccountConfigCache"];
                return instance;
            }
        }

        private BesttoneAccountConstDefinition()
        {
            xmlDoc = new XmlDocument();
            xmlDoc.Load(fileName);
        }

        #region Properties

        /// <summary>
        /// 【网关，有密支付】有密支付http请求页面
        /// </summary>
        public string CardPayHttpsPage
        {
            get
            {
                XmlNode node = GetConstUrlNodeByProperty("ID", "CardPayHttpsPage");
                if (node != null)
                    return node.InnerText;
                return string.Empty;
            }
        }

        /// <summary>
        /// 【网关，网银支付】网银支付跳转页面
        /// </summary>
        public string BankPayWebPage
        {
            get
            {
                XmlNode node = GetConstUrlNodeByProperty("ID", "BankPayWebPage");
                if (node != null)
                    return node.InnerText;
                return string.Empty;
            }
        }

        /// <summary>
        /// 【网关，网银支付】网银支付前台返回地址
        /// </summary>
        public string BesttoneMerchantURL
        {
            get
            {
                XmlNode node = GetConstUrlNodeByProperty("ID", "BesttoneMerchantURL");
                if (node != null)
                    return node.InnerText;
                return string.Empty;
            }
        }

        /// <summary>
        /// 【网关，网银支付】网银支付后台返回地址
        /// </summary>
        public string BesttoneBackMerchantURL
        {
            get
            {
                XmlNode node = GetConstUrlNodeByProperty("ID", "BesttoneBackMerchantURL");
                if (node != null)
                    return node.InnerText;
                return string.Empty;
            }
        }

        /// <summary>
        /// 【融合支付】机构种类（1位）+机构类型（2位）+地区代码（4位）+区县（2）+000000
        /// </summary>
        public string APANAGE
        {
            get
            {
                XmlNode node = GetConstParamsNodeByProperty("ID", "APANAGE");
                if (node != null)
                    return node.InnerText;
                return string.Empty;
            }
        }

        /// <summary>
        /// 【融合支付】受理机构代码
        /// </summary>
        public string ACCEPTORGCODE
        {
            get
            {
                XmlNode node = GetConstParamsNodeByProperty("ID", "ACCEPTORGCODE");
                if (node != null)
                    return node.InnerText;
                return string.Empty;
            }
        }

        /// <summary>
        /// 【融合支付】出单机构
        /// </summary>
        public string SUPPLYORGCODE
        {
            get
            {
                XmlNode node = GetConstParamsNodeByProperty("ID", "SUPPLYORGCODE");
                if (node != null)
                    return node.InnerText;
                return string.Empty;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string AREACODE
        {
            get
            {
                XmlNode node = GetConstParamsNodeByProperty("ID", "AREACODE");
                if (node != null)
                    return node.InnerText;
                return string.Empty;
            }
        }

        /// <summary>
        /// 【融合支付】受理地区代码
        /// </summary>
        public string ACCEPTAREACODE
        {
            get
            {
                XmlNode node = GetConstParamsNodeByProperty("ID", "ACCEPTAREACODE");
                if (node != null)
                    return node.InnerText;
                return string.Empty;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string CITYCODE
        {
            get
            {
                XmlNode node = GetConstParamsNodeByProperty("ID", "CITYCODE");
                if (node != null)
                    return node.InnerText;
                return string.Empty;
            }
        }

        /// <summary>
        /// 【融合支付】受理城市代码
        /// </summary>
        public string ACCEPTCITYCODE
        {
            get
            {
                XmlNode node = GetConstParamsNodeByProperty("ID", "ACCEPTCITYCODE");
                if (node != null)
                    return node.InnerText;
                return string.Empty;
            }
        }

        /// <summary>
        /// 【融合支付】密码修改密钥
        /// </summary>
        public string SymmetryPassWord
        {
            get
            {
                XmlNode node = GetConstParamsNodeByProperty("ID", "SymmetryPassWord");
                if (node != null)
                    return node.InnerText;
                return string.Empty;
            }
        }

        /// <summary>
        /// 【网关，有密支付】商户代码
        /// </summary>
        public string COMMCODE
        {
            get
            {
                XmlNode node = GetConstParamsNodeByProperty("ID", "COMMCODE");
                if (node != null)
                    return node.InnerText;
                return string.Empty;
            }
        }

        /// <summary>
        /// 【网关，有密支付】商户密码
        /// </summary>
        public string COMMPWD
        {
            get
            {
                XmlNode node = GetConstParamsNodeByProperty("ID", "COMMPWD");
                if (node != null)
                    return node.InnerText;
                return string.Empty;
            }
        }

        /// <summary>
        /// 【网关，有密支付】3DES加密key
        /// </summary>
        public string COMMKEY
        {
            get
            {
                XmlNode node = GetConstParamsNodeByProperty("ID", "COMMKEY");
                if (node != null)
                    return node.InnerText;
                return string.Empty;
            }
        }

        /// <summary>
        /// 【网关，有密支付】3DES加密偏移向量
        /// </summary>
        public byte[] COMMIV
        {
            get
            {
                try
                {
                    XmlNode node = GetConstParamsNodeByProperty("ID", "COMMIV");
                    string[] tempArray = node.InnerText.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    byte[] iv = new byte[tempArray.Length];
                    for (Int32 i = 0; i < tempArray.Length; i++)
                    {
                        iv[i] = Convert.ToByte(tempArray[i]);
                    }

                    return iv;
                }
                catch (Exception ex)
                {
                    return new byte[] { 50, 51, 52, 53, 54, 55, 56, 57 };
                }
            }
        }

        /// <summary>
        /// 【网关，网银支付】支付平台分配的商户号
        /// </summary>
        public string MERCHANTID
        {
            get
            {
                XmlNode node = GetConstParamsNodeByProperty("ID", "MERCHANTID");
                if (node != null)
                    return node.InnerText;
                return string.Empty;
            }
        }

        /// <summary>
        /// 【网关，网银支付】支付平台分配的商户号
        /// </summary>
        public string MERCHANTID_KEY
        {
            get
            {
                XmlNode node = GetConstParamsNodeByProperty("ID", "MERCHANTID_KEY");
                if (node != null)
                    return node.InnerText;
                return string.Empty;
            }
        }

        public string SUBMERCHANTID
        {
            get
            {
                XmlNode node = GetConstParamsNodeByProperty("ID", "SUBMERCHANTID");
                if (node != null)
                    return node.InnerText;
                return string.Empty;
            }
        }

        /// <summary>
        /// 个人账户单日充值上限
        /// </summary>
        public long AccountRechargeLimitedOne
        {
            get
            {
                long amount = 0;
                try
                {
                    XmlNode node = GetConstParamsNodeByProperty("ID", "AccountRechargeLimitedOne");
                    amount = Convert.ToInt64(node.InnerText);
                }
                catch { amount = 0; }

                return amount;
            }
        }

        /// <summary>
        /// 个人账户单日充值上限
        /// </summary>
        public long AccountRechargeLimitedDay
        {
            get
            {
                long amount = 0;
                try
                {
                    XmlNode node = GetConstParamsNodeByProperty("ID", "AccountRechargeLimitedDay");
                    amount = Convert.ToInt64(node.InnerText);
                }
                catch { amount = 0; }

                return amount;
            }
        }

        /// <summary>
        /// 个人账户余额上限
        /// </summary>
        public long AccountBalanceLimited
        {
            get
            {
                long amount = 0;
                try
                {
                    XmlNode node = GetConstParamsNodeByProperty("ID", "AccountBalanceLimited");
                    amount = Convert.ToInt64(node.InnerText);
                }
                catch { amount = 0; }
                return amount;
            }
        }

        #endregion

        #region private methods

        private XmlNode GetConstUrlNodeByProperty(string propertyName,string propertyValue)
        {
            return xmlDoc.SelectSingleNode("BesttoneAccount/ConstUrls/Url[@" + propertyName + "='" + propertyValue + "']");
        }

        private XmlNode GetConstParamsNodeByProperty(string propertyName, string propertyValue)
        {
            return xmlDoc.SelectSingleNode("BesttoneAccount/ConstParams/Parameter[@" + propertyName + "='" + propertyValue + "']");
        }

        #endregion
    }
}
