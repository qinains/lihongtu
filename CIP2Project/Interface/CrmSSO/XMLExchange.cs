using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;

namespace Linkage.BestTone.CrmSSO.Rule
{
    public class XMLExchange
    {
        public static readonly string RootPath = AppDomain.CurrentDomain.BaseDirectory;

        #region
        public string BuildQryUserInfoXML(string BusCode, string ServiceCode, string ServiceContractVer, string ActionCode, string TransactionID,
            string ServiceLevel, string SrcOrgID, string SrcSysID, string SrcSysSign, string DstOrgID, string DstSysID, string ReqTime,
            string ProdNbrType, string ProdNbr, string ProvinceCode, string ProvinceName, string CityCode, string CityName)
        {
            System.Xml.XmlDocument XmlDoc = new XmlDocument();
            XmlDoc.Load(RootPath + "/SampleXml/CustmerRelationMsgQryReq.xml");
            System.Xml.XmlNodeList nodeTemplet = XmlDoc.DocumentElement.ChildNodes;
            foreach (XmlElement element in nodeTemplet)
            {
                if (element.Name.ToLower() == "tcpcont")
                {
                    //得到该节点的子节点

                    XmlNodeList nodelist = element.ChildNodes;

                    foreach (XmlElement childnodelist in nodelist)
                    {
                        switch (childnodelist.Name.ToLower())
                        {
                            case "buscode":
                                childnodelist.InnerText = BusCode;
                                break;
                            case "servicecode":
                                childnodelist.InnerText = ServiceCode;
                                break;
                            case "servicecontractver":
                                childnodelist.InnerText = ServiceContractVer;
                                break;
                            case "actioncode":
                                childnodelist.InnerText = ActionCode;
                                break;
                            case "transactionid":
                                childnodelist.InnerText = TransactionID;
                                break;
                            case "servicelevel":
                                childnodelist.InnerText = ServiceLevel;
                                break;
                            case "srcorgid":
                                childnodelist.InnerText = SrcOrgID;
                                break;
                            case "srcsysid":
                                childnodelist.InnerText = SrcSysID;
                                break;
                            case "srcsyssign":
                                childnodelist.InnerText = SrcSysSign;
                                break;
                            case "dstorgid":
                                childnodelist.InnerText = DstOrgID;
                                break;
                            case "dstsysid":
                                childnodelist.InnerText = DstSysID;
                                break;
                            case "reqtime":
                                childnodelist.InnerText = ReqTime;
                                break;
                            default:
                                break;
                        }
                    }
                }
                if (element.Name.ToLower() == "svccont")
                {
                    //得到该节点的子节点

                    XmlNodeList nodelist = element.ChildNodes;
                    foreach (XmlElement childnodelist in nodelist)
                    {
                        if (childnodelist.Name.ToLower() == "qryuserinfo")
                        {
                            XmlNodeList nodelistqryuserinfo = childnodelist.ChildNodes;
                            foreach (XmlElement childnodelistqryuserinfo in nodelistqryuserinfo)
                            {
                                switch (childnodelistqryuserinfo.Name.ToLower())
                                {
                                    case "prodnbrtype":
                                        childnodelistqryuserinfo.InnerText = ProdNbrType;
                                        break;
                                    case "prodnbr":
                                        childnodelistqryuserinfo.InnerText = ProdNbr;
                                        break;
                                    case "belonginfo":
                                        XmlNodeList nodelistbelonginfo = childnodelistqryuserinfo.ChildNodes;
                                        foreach (XmlElement childnodeElement in nodelistbelonginfo)
                                        {
                                            switch (childnodeElement.Name.ToLower())
                                            {
                                                case "provincecode":
                                                    childnodeElement.InnerText = ProvinceCode;
                                                    break;
                                                case "provincename":
                                                    childnodeElement.InnerText = ProvinceName;
                                                    break;
                                                case "citycode":
                                                    childnodeElement.InnerText = CityCode;
                                                    break;
                                                case "cityname":
                                                    childnodeElement.InnerText = CityName;
                                                    break;
                                            }
                                        }
                                        break;
                                }
                            }
                        }
                    }

                }

            }
            List<string> InfoList = new List<string>();
            IEnumerator<string> itrading;
            using (StringReader reader = new StringReader(XmlDoc.InnerXml))
            {
                string strline;
                while ((strline = reader.ReadLine()) != null)
                {
                    InfoList.Add(strline);
                }
            }
            itrading = InfoList.GetEnumerator();
            string xmlText = string.Empty;
            if (itrading.MoveNext())
            {
                xmlText = itrading.Current;
            }
            return xmlText;
        }


        public string BuildQryUserStatusXML(string BusCode, string ServiceCode, string ServiceContractVer, string ActionCode, string TransactionID,
            string ServiceLevel, string SrcOrgID, string SrcSysID, string SrcSysSign, string DstOrgID, string DstSysID, string ReqTime,
            string InfoTypeID, string CodeType, string CodeValue, string CityCode)
        {
            System.Xml.XmlDocument XmlDoc = new XmlDocument();
            XmlDoc.Load(RootPath + "/SampleXml/ProductStatusQryReq.xml");
            System.Xml.XmlNodeList nodeTemplet = XmlDoc.DocumentElement.ChildNodes;
            foreach (XmlElement element in nodeTemplet)
            {
                if (element.Name.ToLower() == "tcpcont")
                {
                    //得到该节点的子节点

                    XmlNodeList nodelist = element.ChildNodes;

                    foreach (XmlElement childnodelist in nodelist)
                    {
                        switch (childnodelist.Name.ToLower())
                        {
                            case "buscode":
                                childnodelist.InnerText = BusCode;
                                break;
                            case "servicecode":
                                childnodelist.InnerText = ServiceCode;
                                break;
                            case "servicecontractver":
                                childnodelist.InnerText = ServiceContractVer;
                                break;
                            case "actioncode":
                                childnodelist.InnerText = ActionCode;
                                break;
                            case "transactionid":
                                childnodelist.InnerText = TransactionID;
                                break;
                            case "servicelevel":
                                childnodelist.InnerText = ServiceLevel;
                                break;
                            case "srcorgid":
                                childnodelist.InnerText = SrcOrgID;
                                break;
                            case "srcsysid":
                                childnodelist.InnerText = SrcSysID;
                                break;
                            case "srcsyssign":
                                childnodelist.InnerText = SrcSysSign;
                                break;
                            case "dstorgid":
                                childnodelist.InnerText = DstOrgID;
                                break;
                            case "dstsysid":
                                childnodelist.InnerText = DstSysID;
                                break;
                            case "reqtime":
                                childnodelist.InnerText = ReqTime;
                                break;
                            default:
                                break;
                        }
                    }
                }
                if (element.Name.ToLower() == "svccont")
                {
                    //得到该节点的子节点

                    XmlNodeList nodelist = element.ChildNodes;
                    foreach (XmlElement childnodelist in nodelist)
                    {
                        if (childnodelist.Name.ToLower() == "qryinforeq")
                        {
                            XmlNodeList nodelistqryuserinfo = childnodelist.ChildNodes;
                            foreach (XmlElement childnodelistqryuserinfo in nodelistqryuserinfo)
                            {
                                switch (childnodelistqryuserinfo.Name.ToLower())
                                {
                                    case "infotypeid":
                                        childnodelistqryuserinfo.InnerText = InfoTypeID;
                                        break;
                                    case "partycodeinfo":
                                        XmlNodeList nodelistbelonginfo = childnodelistqryuserinfo.ChildNodes;
                                        foreach (XmlElement childnodeElement in nodelistbelonginfo)
                                        {
                                            switch (childnodeElement.Name.ToLower())
                                            {
                                                case "codetype":
                                                    childnodeElement.InnerText = CodeType;
                                                    break;
                                                case "codevalue":
                                                    childnodeElement.InnerText = CodeValue;
                                                    break;
                                                case "citycode":
                                                    childnodeElement.InnerText = CityCode;
                                                    break;
                                            }
                                        }
                                        break;
                                }
                            }
                        }
                    }

                }

            }
            List<string> InfoList = new List<string>();
            IEnumerator<string> itrading;
            using (StringReader reader = new StringReader(XmlDoc.InnerXml))
            {
                string strline;
                while ((strline = reader.ReadLine()) != null)
                {
                    InfoList.Add(strline);
                }
            }
            itrading = InfoList.GetEnumerator();
            string xmlText = string.Empty;
            if (itrading.MoveNext())
            {
                xmlText = itrading.Current;
            }
            return xmlText;
        }

        public string BuildQryCustInfoXML(string BusCode, string ServiceCode, string ServiceContractVer, string ActionCode, string TransactionID,
          string ServiceLevel, string SrcOrgID, string SrcSysID, string SrcSysSign, string DstOrgID, string DstSysID, string ReqTime,
          string InfoTypeID, string CodeType, string CodeValue, string CityCode, string PasswdFlag, string CCPasswd)
        {
            System.Xml.XmlDocument XmlDoc = new XmlDocument();
            XmlDoc.Load(RootPath + "/SampleXml/CustInfoQryReq.xml");
            System.Xml.XmlNodeList nodeTemplet = XmlDoc.DocumentElement.ChildNodes;
            foreach (XmlElement element in nodeTemplet)
            {
                if (element.Name.ToLower() == "tcpcont")
                {
                    //得到该节点的子节点

                    XmlNodeList nodelist = element.ChildNodes;

                    foreach (XmlElement childnodelist in nodelist)
                    {
                        switch (childnodelist.Name.ToLower())
                        {
                            case "buscode":
                                childnodelist.InnerText = BusCode;
                                break;
                            case "servicecode":
                                childnodelist.InnerText = ServiceCode;
                                break;
                            case "servicecontractver":
                                childnodelist.InnerText = ServiceContractVer;
                                break;
                            case "actioncode":
                                childnodelist.InnerText = ActionCode;
                                break;
                            case "transactionid":
                                childnodelist.InnerText = TransactionID;
                                break;
                            case "servicelevel":
                                childnodelist.InnerText = ServiceLevel;
                                break;
                            case "srcorgid":
                                childnodelist.InnerText = SrcOrgID;
                                break;
                            case "srcsysid":
                                childnodelist.InnerText = SrcSysID;
                                break;
                            case "srcsyssign":
                                childnodelist.InnerText = SrcSysSign;
                                break;
                            case "dstorgid":
                                childnodelist.InnerText = DstOrgID;
                                break;
                            case "dstsysid":
                                childnodelist.InnerText = DstSysID;
                                break;
                            case "reqtime":
                                childnodelist.InnerText = ReqTime;
                                break;
                            default:
                                break;
                        }
                    }
                }
                if (element.Name.ToLower() == "svccont")
                {
                    //得到该节点的子节点

                    XmlNodeList nodelist = element.ChildNodes;
                    foreach (XmlElement childnodelist in nodelist)
                    {
                        if (childnodelist.Name.ToLower() == "qryinforeq")
                        {
                            XmlNodeList nodelistqryuserinfo = childnodelist.ChildNodes;
                            foreach (XmlElement childnodelistqryuserinfo in nodelistqryuserinfo)
                            {
                                switch (childnodelistqryuserinfo.Name.ToLower())
                                {
                                    case "infotypeid":
                                        childnodelistqryuserinfo.InnerText = InfoTypeID;
                                        break;
                                    case "partycodeinfo":
                                        XmlNodeList nodelistbelonginfo = childnodelistqryuserinfo.ChildNodes;
                                        foreach (XmlElement childnodeElement in nodelistbelonginfo)
                                        {
                                            switch (childnodeElement.Name.ToLower())
                                            {
                                                case "codetype":
                                                    childnodeElement.InnerText = CodeType;
                                                    break;
                                                case "codevalue":
                                                    childnodeElement.InnerText = CodeValue;
                                                    break;
                                            }
                                        }
                                        break;
                                    case "passwdflag":
                                        childnodelistqryuserinfo.InnerText = PasswdFlag;
                                        break;
                                    case "ccpasswd":
                                        childnodelistqryuserinfo.InnerText = CCPasswd;
                                        break;
                                }
                            }
                        }
                    }

                }

            }
            List<string> InfoList = new List<string>();
            IEnumerator<string> itrading;
            using (StringReader reader = new StringReader(XmlDoc.InnerXml))
            {
                string strline;
                while ((strline = reader.ReadLine()) != null)
                {
                    InfoList.Add(strline);
                }
            }
            itrading = InfoList.GetEnumerator();
            string xmlText = string.Empty;
            if (itrading.MoveNext())
            {
                xmlText = itrading.Current;
            }
            return xmlText;
        }

        public string BuildPlatExceptioFeedbackXML(string BusCode, string ServiceCode, string ServiceContractVer, string ActionCode, string TransactionID,
            string ServiceLevel, string SrcOrgID, string SrcSysID, string SrcSysSign, string DstOrgID, string DstSysID, string ReqTime, string OrderSum,
            string BPMOrder, string BizEventNbr, string OrderEventTime, string RspType, string RspCode, string RspDesc)
        {
            System.Xml.XmlDocument XmlDoc = new XmlDocument();
            XmlDoc.Load(RootPath + "/SampleXml/PlatExceptioFeedbackReq.xml");

            System.Xml.XmlNodeList nodeTemplet = XmlDoc.DocumentElement.ChildNodes;
            foreach (XmlElement element in nodeTemplet)
            {
                if (element.Name.ToLower() == "tcpcont")
                {
                    //得到该节点的子节点

                    XmlNodeList nodelist = element.ChildNodes;

                    foreach (XmlElement childnodelist in nodelist)
                    {
                        switch (childnodelist.Name.ToLower())
                        {
                            case "buscode":
                                childnodelist.InnerText = BusCode;
                                break;
                            case "servicecode":
                                childnodelist.InnerText = ServiceCode;
                                break;
                            case "servicecontractver":
                                childnodelist.InnerText = ServiceContractVer;
                                break;
                            case "actioncode":
                                childnodelist.InnerText = ActionCode;
                                break;
                            case "transactionid":
                                childnodelist.InnerText = TransactionID;
                                break;
                            case "servicelevel":
                                childnodelist.InnerText = ServiceLevel;
                                break;
                            case "srcorgid":
                                childnodelist.InnerText = SrcOrgID;
                                break;
                            case "srcsysid":
                                childnodelist.InnerText = SrcSysID;
                                break;
                            case "srcsyssign":
                                childnodelist.InnerText = SrcSysSign;
                                break;
                            case "dstorgid":
                                childnodelist.InnerText = DstOrgID;
                                break;
                            case "dstsysid":
                                childnodelist.InnerText = DstSysID;
                                break;
                            case "reqtime":
                                childnodelist.InnerText = ReqTime;
                                break;
                            default:
                                break;
                        }
                    }
                }
                if (element.Name.ToLower() == "svccont")
                {
                    //得到该节点的子节点

                    XmlNodeList nodelist = element.ChildNodes;
                    foreach (XmlElement childnodelist in nodelist)
                    {
                        if (childnodelist.Name.ToLower() == "bpmorderevent")
                        {
                            XmlNodeList BPMOrderEventNodeList = childnodelist.ChildNodes;
                            foreach (XmlElement BPMOrderEventEle in BPMOrderEventNodeList)
                            {
                                switch (BPMOrderEventEle.Name.ToLower())
                                {
                                    case "ordersum":
                                        BPMOrderEventEle.InnerText = OrderSum;
                                        break;
                                    case "bpmorder":
                                        XmlNodeList BPMOrderNodeList = BPMOrderEventEle.ChildNodes;

                                        foreach (XmlElement BPMOrderEle in BPMOrderNodeList)
                                        {
                                            switch (BPMOrderEle.Name.ToLower())
                                            {
                                                case "bizeventnbr":
                                                    BPMOrderEle.InnerText = BizEventNbr;
                                                    break;
                                                case "ordereventtime":
                                                    BPMOrderEle.InnerText = OrderEventTime;
                                                    break;
                                                case "rsptype":
                                                    BPMOrderEle.InnerText = RspType;
                                                    break;
                                                case "rspcode":
                                                    BPMOrderEle.InnerText = RspCode;
                                                    break;
                                                case "rspdesc":
                                                    BPMOrderEle.InnerText = RspDesc;
                                                    break;
                                                default:
                                                    break;
                                            }
                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            List<string> InfoList = new List<string>();
            IEnumerator<string> itrading;
            using (StringReader reader = new StringReader(XmlDoc.InnerXml))
            {
                string strline;
                while ((strline = reader.ReadLine()) != null)
                {
                    InfoList.Add(strline);
                }
            }
            itrading = InfoList.GetEnumerator();
            string xmlText = string.Empty;
            if (itrading.MoveNext())
            {
                xmlText = itrading.Current;
            }
            string returnText = System.Text.Encoding.UTF8.GetString(System.Text.Encoding.UTF8.GetBytes(xmlText));
            return returnText;
        }

        /// <summary>
        /// 解析用户信息查询接口返回XML
        /// </summary>
        /// <param name="QryUserInfoXMLText"></param>
        /// <returns></returns>
        public QryUserInfoReturn AnalysisQryUserInfoXml(string QryUserInfoXMLText)
        {
            QryUserInfoReturn qyrReturn = new QryUserInfoReturn();
            XmlDocument XmlDoc = new XmlDocument();
            XmlDoc.InnerXml = QryUserInfoXMLText;

            XmlNodeList nodeTemplet = XmlDoc.DocumentElement.ChildNodes;
            foreach (XmlElement element in nodeTemplet)
            {
                if (element.Name.ToLower() == "tcpcont")
                {
                    //得到该节点的子节点

                    XmlNodeList nodelist = element.ChildNodes;

                    foreach (XmlElement childnodelist in nodelist)
                    {
                        switch (childnodelist.Name.ToLower())
                        {
                            case "actioncode":
                                qyrReturn.TcpCont.ActionCode = childnodelist.InnerText;
                                break;
                            case "transactionid":
                                qyrReturn.TcpCont.TransactionID = childnodelist.InnerText;
                                break;
                            case "rsptime":
                                qyrReturn.TcpCont.RspTime = childnodelist.InnerText;
                                break;
                            case "response":
                                //得到该节点的子节点

                                XmlNodeList nodelistResponse = childnodelist.ChildNodes;

                                foreach (XmlElement nodelistResponseEle in nodelistResponse)
                                {
                                    switch (nodelistResponseEle.Name.ToLower())
                                    {
                                        case "rsptype":
                                            qyrReturn.TcpCont.Response.RspType = nodelistResponseEle.InnerText;
                                            break;
                                        case "rspcode":
                                            qyrReturn.TcpCont.Response.RspCode = nodelistResponseEle.InnerText;
                                            break;
                                        case "rspdesc":
                                            qyrReturn.TcpCont.Response.RspDesc = nodelistResponseEle.InnerText;
                                            break;
                                    }
                                }
                                break;

                        }
                    }
                }
                if (element.Name.ToLower() == "svccont")
                {
                    //得到该节点的子节点

                    XmlNodeList nodelist = element.ChildNodes;

                    foreach (XmlElement childnodelist in nodelist)
                    {
                        switch (childnodelist.Name.ToLower())
                        {
                            case "userinforsp":
                                //得到该节点的子节点

                                XmlNodeList UserInfoRspNodeList = childnodelist.ChildNodes;

                                foreach (XmlElement UserInfoRspEle in UserInfoRspNodeList)
                                {
                                    switch (UserInfoRspEle.Name.ToLower())
                                    {
                                        case "prodnbrtype":
                                            qyrReturn.SvcCont.UserInfoRsp.ProdNbrType = UserInfoRspEle.InnerText;
                                            break;
                                        case "prodnbr":
                                            qyrReturn.SvcCont.UserInfoRsp.ProdNbr = UserInfoRspEle.InnerText;
                                            break;
                                        case "belonginfo":
                                            XmlNodeList BelongInfoNodeList = UserInfoRspEle.ChildNodes;

                                            foreach (XmlElement BelongInfoEle in BelongInfoNodeList)
                                            {
                                                switch (BelongInfoEle.Name.ToLower())
                                                {
                                                    case "provincecode":
                                                        qyrReturn.SvcCont.UserInfoRsp.BelongInfo.ProvinceCode = BelongInfoEle.InnerText;

                                                        break;
                                                    case "provincename":
                                                        qyrReturn.SvcCont.UserInfoRsp.BelongInfo.ProvinceName = BelongInfoEle.InnerText;
                                                        break;
                                                    case "citycode":
                                                        qyrReturn.SvcCont.UserInfoRsp.BelongInfo.CityCode = BelongInfoEle.InnerText;
                                                        break;
                                                    case "cityname":
                                                        qyrReturn.SvcCont.UserInfoRsp.BelongInfo.CityName = BelongInfoEle.InnerText;
                                                        break;
                                                    default:
                                                        break;
                                                }
                                            }
                                            break;
                                        case "custid":
                                            qyrReturn.SvcCont.UserInfoRsp.CustID = UserInfoRspEle.InnerText;
                                            break;
                                        case "priceplaninfo":
                                            XmlNodeList PricePlanInfoNodeList = UserInfoRspEle.ChildNodes;

                                            foreach (XmlElement PricePlanInfoEle in PricePlanInfoNodeList)
                                            {
                                                switch (PricePlanInfoEle.Name.ToLower())
                                                {
                                                    case "lpriceplancode":
                                                        qyrReturn.SvcCont.UserInfoRsp.PricePlanInfo.LPricePlanCode = PricePlanInfoEle.InnerText;
                                                        break;
                                                    case "cpriceplancode":
                                                        qyrReturn.SvcCont.UserInfoRsp.PricePlanInfo.CPricePlanCode = PricePlanInfoEle.InnerText;
                                                        break;
                                                    case "priceplantype":
                                                        qyrReturn.SvcCont.UserInfoRsp.PricePlanInfo.PricePlanType = PricePlanInfoEle.InnerText;
                                                        break;
                                                    case "priceplanname":
                                                        qyrReturn.SvcCont.UserInfoRsp.PricePlanInfo.PricePlanName = PricePlanInfoEle.InnerText;
                                                        break;
                                                    case "priceplandesc":
                                                        qyrReturn.SvcCont.UserInfoRsp.PricePlanInfo.PricePlanDesc = PricePlanInfoEle.InnerText;
                                                        break;
                                                    case "dealtime":
                                                        qyrReturn.SvcCont.UserInfoRsp.PricePlanInfo.DealTime = PricePlanInfoEle.InnerText;
                                                        break;
                                                    case "preferencesendtime":
                                                        qyrReturn.SvcCont.UserInfoRsp.PricePlanInfo.PreferencesEndTime = PricePlanInfoEle.InnerText;
                                                        break;
                                                    default:
                                                        break;
                                                }
                                            }
                                            break;

                                        case "username":
                                            qyrReturn.SvcCont.UserInfoRsp.UserName = UserInfoRspEle.InnerText;
                                            break;
                                        case "custgroup":
                                            XmlNodeList CustGroupNodeList = UserInfoRspEle.ChildNodes;
                                            foreach (XmlElement CustGroupEle in CustGroupNodeList)
                                            {
                                                switch (CustGroupEle.Name.ToLower())
                                                {
                                                    case "group":
                                                        qyrReturn.SvcCont.UserInfoRsp.CustGroup.Group = CustGroupEle.InnerText;
                                                        break;
                                                    case "groupname":
                                                        qyrReturn.SvcCont.UserInfoRsp.CustGroup.GroupName = CustGroupEle.InnerText;
                                                        break;
                                                    default:
                                                        break;
                                                }
                                            }
                                            break;

                                        case "identityinfo":
                                            XmlNodeList IdentityInfoNodeList = UserInfoRspEle.ChildNodes;
                                            foreach (XmlElement IdentityInfoEle in IdentityInfoNodeList)
                                            {
                                                switch (IdentityInfoEle.Name.ToLower())
                                                {
                                                    case "identtype":
                                                        qyrReturn.SvcCont.UserInfoRsp.IdentityInfo.IdentType = IdentityInfoEle.InnerText;
                                                        break;
                                                    case "identnum":
                                                        qyrReturn.SvcCont.UserInfoRsp.IdentityInfo.IdentNum = IdentityInfoEle.InnerText;
                                                        break;

                                                    default:
                                                        break;
                                                }
                                            }
                                            break;
                                        case "userpaytype":
                                            qyrReturn.SvcCont.UserInfoRsp.UserPayType = UserInfoRspEle.InnerText;
                                            break;
                                        case "telephone":
                                            qyrReturn.SvcCont.UserInfoRsp.Telephone = UserInfoRspEle.InnerText;
                                            break;
                                        case "bandwidth":
                                            qyrReturn.SvcCont.UserInfoRsp.Bandwidth = UserInfoRspEle.InnerText;
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                        }
                    }

                }
            }
            return qyrReturn;
        }


      

        /// <summary>
        /// 解析用户状态查询接口返回XML
        /// </summary>
        /// <param name="QryUserStatusXMLText"></param>
        /// <returns></returns>
        public QryUserStatusReturn AnalysisQryUserStatusXML(string QryUserStatusXMLText)
        {
            QryUserStatusReturn qryReturn = new QryUserStatusReturn();
            System.Xml.XmlDocument XmlDoc = new XmlDocument();
            XmlDoc.InnerXml = QryUserStatusXMLText;

            XmlNodeList nodeTemplet = XmlDoc.DocumentElement.ChildNodes;
            foreach (XmlElement element in nodeTemplet)
            {
                if (element.Name.ToLower() == "tcpcont")
                {
                    //得到该节点的子节点

                    XmlNodeList nodelist = element.ChildNodes;

                    foreach (XmlElement childnodelist in nodelist)
                    {
                        switch (childnodelist.Name.ToLower())
                        {
                            case "actioncode":
                                qryReturn.TcpCont.ActionCode = childnodelist.InnerText;
                                break;
                            case "transactionid":
                                qryReturn.TcpCont.TransactionID = childnodelist.InnerText;
                                break;
                            case "rsptime":
                                qryReturn.TcpCont.RspTime = childnodelist.InnerText;
                                break;
                            case "response":
                                //得到该节点的子节点

                                XmlNodeList nodelistResponse = childnodelist.ChildNodes;

                                foreach (XmlElement nodelistResponseEle in nodelistResponse)
                                {
                                    switch (nodelistResponseEle.Name.ToLower())
                                    {
                                        case "rsptype":
                                            qryReturn.TcpCont.Response.RspType = nodelistResponseEle.InnerText;
                                            break;
                                        case "rspcode":
                                            qryReturn.TcpCont.Response.RspCode = nodelistResponseEle.InnerText;
                                            break;
                                        case "rspdesc":
                                            qryReturn.TcpCont.Response.RspDesc = nodelistResponseEle.InnerText;
                                            break;
                                    }
                                }
                                break;

                        }
                    }
                }
                if (element.Name.ToLower() == "svccont")
                {
                    //得到该节点的子节点

                    XmlNodeList nodelist = element.ChildNodes;

                    foreach (XmlElement childnodelist in nodelist)
                    {
                        switch (childnodelist.Name.ToLower())
                        {
                            case "qryinforsp":
                                //得到该节点的子节点

                                XmlNodeList nodelistQryInfoRsp = childnodelist.ChildNodes;

                                foreach (XmlElement nodelistQryInfoRspEle in nodelistQryInfoRsp)
                                {
                                    switch (nodelistQryInfoRspEle.Name.ToLower())
                                    {
                                        case "infotypeid":
                                            qryReturn.SvcCont.QryInfoRsp.InfoTypeID = nodelistQryInfoRspEle.InnerText;
                                            break;
                                        case "infocont":
                                            //得到该节点的子节点

                                            XmlNodeList nodelistInfoCont = nodelistQryInfoRspEle.ChildNodes;

                                            foreach (XmlElement nodelistInfoContEle in nodelistInfoCont)
                                            {
                                                switch (nodelistInfoContEle.Name.ToLower())
                                                {
                                                    case "statusinfo":
                                                        //得到该节点的子节点

                                                        XmlNodeList nodelistStatusInfo = nodelistInfoContEle.ChildNodes;
                                                        foreach (XmlElement nodelistStatusInfoEle in nodelistStatusInfo)
                                                        {
                                                            switch (nodelistStatusInfoEle.Name.ToLower())
                                                            {
                                                                case "prodnbrtype":
                                                                    qryReturn.SvcCont.QryInfoRsp.InfoCont.StatusInfo.ProdNbrType = nodelistStatusInfoEle.InnerText;
                                                                    break;
                                                                case "prodnbr":
                                                                    qryReturn.SvcCont.QryInfoRsp.InfoCont.StatusInfo.ProdNbr = nodelistStatusInfoEle.InnerText;
                                                                    break;
                                                                case "status":
                                                                    qryReturn.SvcCont.QryInfoRsp.InfoCont.StatusInfo.Status = nodelistStatusInfoEle.InnerText;
                                                                    break;
                                                                case "belonginfo":
                                                                    XmlNodeList nodelistBelongInfo = nodelistStatusInfoEle.ChildNodes;
                                                                    foreach (XmlElement nodelistBelongInfoEle in nodelistBelongInfo)
                                                                    {
                                                                        switch (nodelistBelongInfoEle.Name.ToLower())
                                                                        {
                                                                            case "provincecode":
                                                                                qryReturn.SvcCont.QryInfoRsp.InfoCont.StatusInfo.BelongInfo.ProvinceCode = nodelistBelongInfoEle.InnerText;
                                                                                break;
                                                                            case "provincename":
                                                                                qryReturn.SvcCont.QryInfoRsp.InfoCont.StatusInfo.BelongInfo.ProvinceName = nodelistBelongInfoEle.InnerText;
                                                                                break;
                                                                            case "citycode":
                                                                                qryReturn.SvcCont.QryInfoRsp.InfoCont.StatusInfo.BelongInfo.CityCode = nodelistBelongInfoEle.InnerText;
                                                                                break;
                                                                            case "cityname":
                                                                                qryReturn.SvcCont.QryInfoRsp.InfoCont.StatusInfo.BelongInfo.CityName = nodelistBelongInfoEle.InnerText;
                                                                                break;
                                                                            default:
                                                                                break;
                                                                        }
                                                                    }
                                                                    break;
                                                            }
                                                        }
                                                        break;
                                                }
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                        }
                    }
                }
            }
            return qryReturn;
        }




        /// <summary>
        /// 解析客户查询接口返回XML
        /// </summary>
        /// <param name="QryUserStatusXMLText"></param>
        /// <returns></returns>
        public QryCustInfoReturn AnalysisQryCustInfoXML(string QryCustInfoXMLText)
        {
            QryCustInfoReturn qryReturn = new QryCustInfoReturn();
            System.Xml.XmlDocument XmlDoc = new XmlDocument();
            XmlDoc.InnerXml = QryCustInfoXMLText;

            XmlNodeList nodeTemplet = XmlDoc.DocumentElement.ChildNodes;
            foreach (XmlElement element in nodeTemplet)
            {
                if (element.Name.ToLower() == "tcpcont")
                {
                    //得到该节点的子节点

                    XmlNodeList nodelist = element.ChildNodes;

                    foreach (XmlElement childnodelist in nodelist)
                    {
                        switch (childnodelist.Name.ToLower())
                        {
                            case "actioncode":
                                qryReturn.TcpCont.ActionCode = childnodelist.InnerText;
                                break;
                            case "transactionid":
                                qryReturn.TcpCont.TransactionID = childnodelist.InnerText;
                                break;
                            case "rsptime":
                                qryReturn.TcpCont.RspTime = childnodelist.InnerText;
                                break;
                            case "response":
                                //得到该节点的子节点

                                XmlNodeList nodelistResponse = childnodelist.ChildNodes;

                                foreach (XmlElement nodelistResponseEle in nodelistResponse)
                                {
                                    switch (nodelistResponseEle.Name.ToLower())
                                    {
                                        case "rsptype":
                                            qryReturn.TcpCont.Response.RspType = nodelistResponseEle.InnerText;
                                            break;
                                        case "rspcode":
                                            qryReturn.TcpCont.Response.RspCode = nodelistResponseEle.InnerText;
                                            break;
                                        case "rspdesc":
                                            qryReturn.TcpCont.Response.RspDesc = nodelistResponseEle.InnerText;
                                            break;
                                    }
                                }
                                break;

                        }
                    }
                }
                if (element.Name.ToLower() == "svccont")
                {
                    //得到该节点的子节点

                    XmlNodeList nodelist = element.ChildNodes;

                    foreach (XmlElement childnodelist in nodelist)
                    {
                        switch (childnodelist.Name.ToLower())
                        {
                            case "qryinforsp":
                                //得到该节点的子节点

                                XmlNodeList nodelistQryInfoRsp = childnodelist.ChildNodes;

                                foreach (XmlElement nodelistQryInfoRspEle in nodelistQryInfoRsp)
                                {
                                    switch (nodelistQryInfoRspEle.Name.ToLower())
                                    {
                                        case "infotypeid":
                                            qryReturn.SvcCont.QryInfoRsp.InfoTypeID = nodelistQryInfoRspEle.InnerText;
                                            break;
                                        case "infocont":
                                            //得到该节点的子节点

                                            XmlNodeList nodelistInfoCont = nodelistQryInfoRspEle.ChildNodes;

                                            foreach (XmlElement nodelistInfoContEle in nodelistInfoCont)
                                            {
                                                switch (nodelistInfoContEle.Name.ToLower())
                                                {
                                                    case "custinfo":
                                                        //得到该节点的子节点

                                                        XmlNodeList nodelistCustInfo = nodelistInfoContEle.ChildNodes;
                                                        foreach (XmlElement nodelistCustInfoEle in nodelistCustInfo)
                                                        {
                                                            switch (nodelistCustInfoEle.Name.ToLower())
                                                            {
      
                                                                case "belonginfo":
                                                                    XmlNodeList nodelistBelongInfo = nodelistCustInfoEle.ChildNodes;
                                                                    foreach (XmlElement nodelistBelongInfoEle in nodelistBelongInfo)
                                                                    {
                                                                        switch (nodelistBelongInfoEle.Name.ToLower())
                                                                        {
                                                                            case "provincecode":
                                                                                qryReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.BelongInfo.ProvinceCode = nodelistBelongInfoEle.InnerText;
                                                                                break;
                                                                            case "provincename":
                                                                                qryReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.BelongInfo.ProvinceName = nodelistBelongInfoEle.InnerText;
                                                                                break;
                                                                            case "citycode":
                                                                                qryReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.BelongInfo.CityCode = nodelistBelongInfoEle.InnerText;
                                                                                break;
                                                                            case "cityname":
                                                                                qryReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.BelongInfo.CityName = nodelistBelongInfoEle.InnerText;
                                                                                break;
                                                                            default:
                                                                                break;
                                                                        }
                                                                    }
                                                                    break;

                                                                case "partycodeinfo":
                                                                    XmlNodeList nodelistPartyCodeInfo = nodelistCustInfoEle.ChildNodes;
                                                                    foreach (XmlElement nodelistPartyCodeInfoEle in nodelistPartyCodeInfo)
                                                                    {
                                                                        switch (nodelistPartyCodeInfoEle.Name.ToLower())
                                                                        {
                                                                            case "codetype":
                                                                                qryReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.PartyCodeInfo.CodeType = nodelistPartyCodeInfoEle.InnerText;
                                                                                break;
                                                                            case "codevalue":
                                                                                qryReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.PartyCodeInfo.CodeValue = nodelistPartyCodeInfoEle.InnerText;
                                                                                break;
                                                                            case "citycode":
                                                                                qryReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.PartyCodeInfo.CityCode = nodelistPartyCodeInfoEle.InnerText;
                                                                                break;
                                                                            default:
                                                                                break;
                                                                        }
                                                                    }
                                                                    break;
                                                                case "identityinfo":
                                                                    XmlNodeList IdentityInfoNodeList = nodelistCustInfoEle.ChildNodes;
                                                                    foreach (XmlElement IdentityInfoEle in IdentityInfoNodeList)
                                                                    {
                                                                        switch (IdentityInfoEle.Name.ToLower())
                                                                        {
                                                                            case "identtype":
                                                                                qryReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.IdentityInfo.IdentType = IdentityInfoEle.InnerText;
                                                                                break;
                                                                            case "identnum":
                                                                                qryReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.IdentityInfo.IdentNum = IdentityInfoEle.InnerText;
                                                                                break;

                                                                            default:
                                                                                break;
                                                                        }
                                                                    }
                                                                    break;
                                                                case "custname":
                                                                    qryReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustName=nodelistCustInfoEle.InnerText;
                                                                    break;
                                                                case "custbrand":
                                                                    qryReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustBrand = nodelistCustInfoEle.InnerText;
                                                                    break;
                                                                case "custgroup":
                                                                    qryReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustGroup = nodelistCustInfoEle.InnerText;
                                                                    break;
                                                                case "custservicelevel":
                                                                    qryReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustServiceLevel = nodelistCustInfoEle.InnerText;
                                                                    break;
                                                                case "custaddress":
                                                                    qryReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustAddress = nodelistCustInfoEle.InnerText;
                                                                    break;

                                                            }
                                                        }
                                                        break;
                                                }
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                        }
                    }
                }
            }
            return qryReturn;
        }

        public PlatExceptioFeedbackReturn AnalysisPlatExceptioFeedbackXML(string PlatExceptioFeedbackXMLText)
        {
            PlatExceptioFeedbackReturn Return = new PlatExceptioFeedbackReturn();
            System.Xml.XmlDocument XmlDoc = new XmlDocument();
            XmlDoc.InnerXml = PlatExceptioFeedbackXMLText;

            XmlNodeList nodeTemplet = XmlDoc.DocumentElement.ChildNodes;
            foreach (XmlElement element in nodeTemplet)
            {
                if (element.Name.ToLower() == "tcpcont")
                {
                    //得到该节点的子节点

                    XmlNodeList nodelist = element.ChildNodes;

                    foreach (XmlElement childnodelist in nodelist)
                    {
                        switch (childnodelist.Name.ToLower())
                        {
                            case "actioncode":
                                Return.TcpCont.ActionCode = childnodelist.InnerText;
                                break;
                            case "transactionid":
                                Return.TcpCont.TransactionID = childnodelist.InnerText;
                                break;
                            case "rsptime":
                                Return.TcpCont.RspTime = childnodelist.InnerText;
                                break;
                            case "response":
                                //得到该节点的子节点

                                XmlNodeList nodelistResponse = childnodelist.ChildNodes;

                                foreach (XmlElement nodelistResponseEle in nodelistResponse)
                                {
                                    switch (nodelistResponseEle.Name.ToLower())
                                    {
                                        case "rsptype":
                                            Return.TcpCont.Response.RspType = nodelistResponseEle.InnerText;
                                            break;
                                        case "rspcode":
                                            Return.TcpCont.Response.RspCode = nodelistResponseEle.InnerText;
                                            break;
                                        case "rspdesc":
                                            Return.TcpCont.Response.RspDesc = nodelistResponseEle.InnerText;
                                            break;
                                    }
                                }
                                break;

                        }
                    }
                }
                if (element.Name.ToLower() == "svccont")
                {
                    XmlNodeList nodelist = element.ChildNodes;

                    foreach (XmlElement childnodelist in nodelist)
                    {
                        switch (childnodelist.Name.ToLower())
                        {
                            case "actioncode":
                                Return.SvcCont.ActionCode = childnodelist.InnerText;
                                break;
                            case "transactionid":
                                Return.SvcCont.TransactionID = childnodelist.InnerText;
                                break;
                            case "rsptime":
                                Return.SvcCont.RspTime = childnodelist.InnerText;
                                break;
                            case "response":
                                //得到该节点的子节点

                                XmlNodeList nodelistResponse = childnodelist.ChildNodes;

                                foreach (XmlElement nodelistResponseEle in nodelistResponse)
                                {
                                    switch (nodelistResponseEle.Name.ToLower())
                                    {
                                        case "rsptype":
                                            Return.SvcCont.Response.RspType = nodelistResponseEle.InnerText;
                                            break;
                                        case "rspcode":
                                            Return.SvcCont.Response.RspCode = nodelistResponseEle.InnerText;
                                            break;
                                        case "rspdesc":
                                            Return.SvcCont.Response.RspDesc = nodelistResponseEle.InnerText;
                                            break;
                                    }
                                }
                                break;
                        }
                    }
                }
            }
            return Return;
        }

        #endregion

        public static string BuildStatusChangedNotifyResXml(ResTcpCont TcpCont)
        {
            System.Xml.XmlDocument XmlDoc = new XmlDocument();

            try
            {
                XmlDoc.Load(RootPath + "/SampleXml/StatusChangedNotifyRes.xml");


                System.Xml.XmlNodeList nodeTemplet = XmlDoc.DocumentElement.ChildNodes;

                #region 对Xml模板的值进行替换
                foreach (XmlElement element in nodeTemplet)
                {
                    if (element.Name.ToLower() == "tcpcont")
                    {
                        //得到该节点的子节点

                        XmlNodeList nodelist = element.ChildNodes;

                        foreach (XmlElement Element in nodelist)
                        {
                            switch (Element.Name.ToLower())
                            {
                                case "actioncode":
                                    Element.InnerText = TcpCont.ActionCode;
                                    break;
                                case "transactionid":
                                    Element.InnerText = TcpCont.TransactionID;
                                    break;
                                case "rsptime":
                                    Element.InnerText = TcpCont.RspTime;
                                    break;
                                case "response":
                                    {
                                        foreach (XmlElement rspElement in Element.ChildNodes)
                                        {
                                            switch (rspElement.Name.ToLower())
                                            {
                                                case "rsptype":
                                                    rspElement.InnerText = TcpCont.Response.RspType;
                                                    break;

                                                case "rspcode":
                                                    rspElement.InnerText = TcpCont.Response.RspCode;
                                                    break;

                                                case "rspdesc":
                                                    rspElement.InnerText = TcpCont.Response.RspDesc;
                                                    break;

                                                default:
                                                    break;
                                            }
                                        }
                                        break;
                                    }
                                default:
                                    break;
                            }


                        }

                #endregion

                    }
                }

                XmlDoc.PreserveWhitespace = false;
            }
            catch
            {

                ;
            }

            return XmlDoc.InnerXml;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlPath"></param>
        /// <param name="RootElement"></param>
        /// <returns></returns>
        public static XmlNodeList GetNodeListByXmlPath(string xmlPath, XmlElement RootElement)
        {
            XmlNodeList NodeList = null;


            XmlNode tempNode = RootElement.SelectSingleNode(xmlPath);

            if (tempNode.HasChildNodes)
            {
                NodeList = tempNode.ChildNodes;
            }

            return NodeList;
        }

        public static StatusChangedReqBPMOrderEvent PraseStatusChangedNotifyReqXml(string Xml, ref ReqTcpCont TcpCont)
        {
            StatusChangedReqBPMOrderEvent Event = new StatusChangedReqBPMOrderEvent();
            TcpCont = new ReqTcpCont();

            StringReader textReader = new StringReader(Xml);
            XmlDocument doc = new XmlDocument();
            doc.Load(textReader);

            XmlNodeList nodeTemplet = doc.DocumentElement.ChildNodes;

            foreach (XmlElement element in nodeTemplet)
            {
                if (element.Name.ToLower() == "tcpcont")
                {
                    foreach (XmlNode Node in element.ChildNodes)
                    {
                        switch (Node.Name.ToLower())
                        {
                            case "buscode":
                                {
                                    TcpCont.BusCode = Node.InnerText;
                                    break;
                                }
                            case "servicecode":
                                {
                                    TcpCont.ServiceCode = Node.InnerText;
                                    break;
                                }
                            case "servicecontractver":
                                {
                                    TcpCont.ServiceContractVer = Node.InnerText;
                                    break;
                                }
                            case "actioncode":
                                {
                                    TcpCont.ActionCode = Node.InnerText;
                                    break;
                                }
                            case "transactionid":
                                {
                                    TcpCont.TransactionID = Node.InnerText;
                                    break;
                                }
                            case "servicelevel":
                                {
                                    TcpCont.ServiceLevel = Node.InnerText;
                                    break;
                                }
                            case "srcorgid":
                                {
                                    TcpCont.SrcOrgID = Node.InnerText;
                                    break;
                                }
                            case "srcsysid":
                                {
                                    TcpCont.SrcSysID = Node.InnerText;
                                    break;
                                }
                            case "srcsyssign":
                                {
                                    TcpCont.SrcSysSign = Node.InnerText;
                                    break;
                                }
                            case "dstorgid":
                                {
                                    TcpCont.DstOrgID = Node.InnerText;
                                    break;
                                }
                            case "dstsysid":
                                {
                                    TcpCont.DstSysID = Node.InnerText;
                                    break;
                                }
                            case "reqtime":
                                {
                                    TcpCont.ReqTime = Node.InnerText;
                                    break;
                                }
                            default:
                                break;
                        }
                    }

                }

                if (element.Name.ToLower() == "svccont")
                {
                    foreach (XmlNode var in element.FirstChild.ChildNodes)
                    {
                        if (var.Name.ToLower() == "ordersum")
                        {
                            Event.OrderSum = Int32.Parse(var.InnerText);
                        }

                        if (var.Name.ToLower() == "bpmorder")
                        {
                            #region
                            BPMOrder Order = new BPMOrder();

                            foreach (XmlNode BPMOrederElementNode in var.ChildNodes)
                            {
                                switch (BPMOrederElementNode.Name)
                                {

                                    case "BizEventNbr":
                                        {
                                            Order.BizEventNbr = BPMOrederElementNode.InnerText;
                                            break;
                                        }
                                    case "BizEventTime":
                                        {
                                            Order.BizEventTime = BPMOrederElementNode.InnerText;
                                            break;
                                        }
                                    case "OrderTypeCd":
                                        {
                                            Order.OrderTypeCd = BPMOrederElementNode.InnerText;
                                            break;
                                        }
                                    case "ProdNbrType":
                                        {
                                            Order.ProdNbrType = BPMOrederElementNode.InnerText;
                                            break;
                                        }
                                    case "ProdNbr":
                                        {
                                            Order.ProdNbr = BPMOrederElementNode.InnerText;
                                            break;
                                        }
                                    case "OldProdNbr":
                                        {
                                            Order.OldProdNbr = BPMOrederElementNode.InnerText;
                                            break;
                                        }
                                    case "IMSI":
                                        {
                                            Order.IMSI = BPMOrederElementNode.InnerText;
                                            break;
                                        }
                                    case "OldIMSI":
                                        {
                                            Order.OldIMSI = BPMOrederElementNode.InnerText;
                                            break;
                                        }
                                    case "CustID":
                                        {
                                            Order.CustID = BPMOrederElementNode.InnerText;
                                            break;
                                        }
                                    case "OldCustID":
                                        {
                                            Order.OldCustID = BPMOrederElementNode.InnerText;
                                            break;
                                        }
                                    case "BelongInfo":
                                        {
                                            if (BPMOrederElementNode.HasChildNodes)
                                            {
                                                foreach (XmlNode BelongInfoNode in BPMOrederElementNode.ChildNodes)
                                                {
                                                    switch (BelongInfoNode.Name)
                                                    {
                                                        case "ProvinceCode":
                                                            {
                                                                Order.BelongInfo.ProvinceCode = BelongInfoNode.InnerText;
                                                                break;
                                                            }
                                                        case "ProvinceName":
                                                            {
                                                                Order.BelongInfo.ProvinceName = BelongInfoNode.InnerText;
                                                                break;
                                                            }
                                                        case "CityCode":
                                                            {
                                                                Order.BelongInfo.CityCode = BelongInfoNode.InnerText;
                                                                break;
                                                            }
                                                        case "CityName":
                                                            {
                                                                Order.BelongInfo.CityName = BelongInfoNode.InnerText;
                                                                break;
                                                            }
                                                        default:
                                                            break;
                                                    }
                                                }
                                            };
                                            break;
                                        }
                                    default:
                                        break;
                                }
                            }

                            Event.BMPOrderList.Add(Order);

                            #endregion
                        }
                    }
                }
            }

            return Event;
        }


    }

    /// <summary>
    /// 
    /// </summary>
    public class PlatExceptioFeedbackReturn
    {
        public PlatExceptioFeedbackReturn()
        { }

        private TcpContReturn tcpCont = new TcpContReturn();
        /// <summary>
        /// 会话控制
        /// </summary>
        public TcpContReturn TcpCont
        {
            get { return tcpCont; }
            set { tcpCont = value; }
        }

        private SvcContResult svcCont = new SvcContResult();
        /// <summary>
        /// 业务信息
        /// </summary>
        public SvcContResult SvcCont
        {
            get { return svcCont; }
            set { svcCont = value; }
        }

        public class TcpContReturn
        {
            private string actionCode = "";
            /// <summary>
            /// 动作类型标识
            /// </summary>
            public string ActionCode
            {
                get { return actionCode; }
                set { actionCode = value; }
            }

            private string transactionID = "";
            /// <summary>
            /// 请求流水号

            /// </summary>
            public string TransactionID
            {
                get { return transactionID; }
                set { transactionID = value; }
            }

            private string rspTime = "";
            /// <summary>
            /// 答时间，时间格式字符串：YYYYMMDDHHMMSS
            /// </summary>
            public string RspTime
            {
                get { return rspTime; }
                set { rspTime = value; }
            }

            private ResponseResult response = new ResponseResult();
            /// <summary>
            /// 应答信息
            /// </summary>
            public ResponseResult Response
            {
                get { return response; }
                set { response = value; }
            }

            public class ResponseResult
            {
                private string rspType = "";
                /// <summary>
                /// 会话应答类型
                /// </summary>
                public string RspType
                {
                    get { return rspType; }
                    set { rspType = value; }
                }

                private string rspCode = "";
                /// <summary>
                /// 会话应答代码
                /// </summary>
                public string RspCode
                {
                    get { return rspCode; }
                    set { rspCode = value; }
                }

                private string rspDesc = "";
                /// <summary>
                /// 会话应答描述
                /// </summary>
                public string RspDesc
                {
                    get { return rspDesc; }
                    set { rspDesc = value; }
                }

            }
        }
        public class SvcContResult
        {
            private string actionCode = "";
            /// <summary>
            /// 动作类型标识
            /// </summary>
            public string ActionCode
            {
                get { return actionCode; }
                set { actionCode = value; }
            }

            private string transactionID = "";
            /// <summary>
            /// 请求流水号
            /// </summary>
            public string TransactionID
            {
                get { return transactionID; }
                set { transactionID = value; }
            }

            private string rspTime = "";
            /// <summary>
            /// 应答时间，时间格式字符串：YYYYMMDDHHMMSS
            /// </summary>
            public string RspTime
            {
                get { return rspTime; }
                set { rspTime = value; }
            }

            private ResponseResult response = new ResponseResult();
            /// <summary>
            /// 应答信息
            /// </summary>
            public ResponseResult Response
            {
                get { return response; }
                set { response = value; }
            }

            public class ResponseResult
            {
                private string rspType = "";
                /// <summary>
                /// 会话应答类型
                /// </summary>
                public string RspType
                {
                    get { return rspType; }
                    set { rspType = value; }
                }

                private string rspCode = "";
                /// <summary>
                /// 会话应答代码
                /// </summary>
                public string RspCode
                {
                    get { return rspCode; }
                    set { rspCode = value; }
                }

                private string rspDesc = "";
                /// <summary>
                /// 会话应答描述
                /// </summary>
                public string RspDesc
                {
                    get { return rspDesc; }
                    set { rspDesc = value; }
                }

            }

        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class QryUserInfoReturn
    {
        public QryUserInfoReturn()
        { }

        private TcpContReturn tcpCont = new TcpContReturn();
        /// <summary>
        /// 会话控制
        /// </summary>
        public TcpContReturn TcpCont
        {
            get { return tcpCont; }
            set { tcpCont = value; }
        }

        private SvcContResult svcCont = new SvcContResult();
        /// <summary>
        /// 业务信息
        /// </summary>
        public SvcContResult SvcCont
        {
            get { return svcCont; }
            set { svcCont = value; }
        }

        public class TcpContReturn
        {
            private string actionCode = "";
            /// <summary>
            /// 动作类型标识
            /// </summary>
            public string ActionCode
            {
                get { return actionCode; }
                set { actionCode = value; }
            }

            private string transactionID = "";
            /// <summary>
            /// 请求流水号

            /// </summary>
            public string TransactionID
            {
                get { return transactionID; }
                set { transactionID = value; }
            }

            private string rspTime = "";
            /// <summary>
            /// 答时间，时间格式字符串：YYYYMMDDHHMMSS
            /// </summary>
            public string RspTime
            {
                get { return rspTime; }
                set { rspTime = value; }
            }

            private ResponseResult response = new ResponseResult();
            /// <summary>
            /// 应答信息
            /// </summary>
            public ResponseResult Response
            {
                get { return response; }
                set { response = value; }
            }

            public class ResponseResult
            {
                private string rspType = "";
                /// <summary>
                /// 会话应答类型
                /// </summary>
                public string RspType
                {
                    get { return rspType; }
                    set { rspType = value; }
                }

                private string rspCode = "";
                /// <summary>
                /// 会话应答代码
                /// </summary>
                public string RspCode
                {
                    get { return rspCode; }
                    set { rspCode = value; }
                }

                private string rspDesc = "";
                /// <summary>
                /// 会话应答描述
                /// </summary>
                public string RspDesc
                {
                    get { return rspDesc; }
                    set { rspDesc = value; }
                }

            }
        }

        public class SvcContResult
        {
            private UserInfoRspResult userInfoRsp = new UserInfoRspResult();
            /// <summary>
            /// 用户信息查询
            /// </summary>
            public UserInfoRspResult UserInfoRsp
            {
                get { return userInfoRsp; }
                set { userInfoRsp = value; }
            }

            public class UserInfoRspResult
            {
                private string prodNbrType = "";
                /// <summary>
                /// 接入号号类型
                /// </summary>
                public string ProdNbrType
                {
                    get { return prodNbrType; }
                    set { prodNbrType = value; }
                }

                private string prodNbr = "";
                /// <summary>
                /// 接入号

                /// </summary>
                public string ProdNbr
                {
                    get { return prodNbr; }
                    set { prodNbr = value; }
                }

                private BelongInfoResult belongInfo = new BelongInfoResult();
                /// <summary>
                /// 所属信息

                /// </summary>
                public BelongInfoResult BelongInfo
                {
                    get { return belongInfo; }
                    set { belongInfo = value; }
                }


                public class BelongInfoResult
                {
                    private string provinceCode = "";
                    /// <summary>
                    /// 所属省代码
                    /// </summary>
                    public string ProvinceCode
                    {
                        get { return provinceCode; }
                        set { provinceCode = value; }
                    }

                    private string provinceName = "";
                    /// <summary>
                    /// 所属省名称
                    /// </summary>
                    public string ProvinceName
                    {
                        get { return provinceName; }
                        set { provinceName = value; }
                    }
                    private string cityCode = "";
                    /// <summary>
                    /// 区号
                    /// </summary>
                    public string CityCode
                    {
                        get { return cityCode; }
                        set { cityCode = value; }
                    }

                    private string cityName = "";
                    /// <summary>
                    /// 归属名称地市名称
                    /// </summary>
                    public string CityName
                    {
                        get { return cityName; }
                        set { cityName = value; }
                    }

                }

                private string custID = "";
                /// <summary>
                /// CRM客户标识ID
                /// </summary>
                public string CustID
                {
                    get { return custID; }
                    set { custID = value; }
                }

                private PricePlanInfoResult pricePlanInfo = new PricePlanInfoResult();

                public PricePlanInfoResult PricePlanInfo
                {
                    get { return pricePlanInfo; }
                    set { pricePlanInfo = value; }
                }


                public class PricePlanInfoResult
                {
                    private string lPricePlanCode = "";
                    /// <summary>
                    /// 省套餐ID
                    /// </summary>
                    public string LPricePlanCode
                    {
                        get { return lPricePlanCode; }
                        set { lPricePlanCode = value; }
                    }

                    private string cPricePlanCode = "";
                    /// <summary>
                    /// 集团统一套餐ID
                    /// </summary>
                    public string CPricePlanCode
                    {
                        get { return cPricePlanCode; }
                        set { cPricePlanCode = value; }
                    }
                    private string pricePlanType = "";
                    /// <summary>
                    /// 开通套餐业务类型
                    /// </summary>
                    public string PricePlanType
                    {
                        get { return pricePlanType; }
                        set { pricePlanType = value; }
                    }

                    private string pricePlanName = "";
                    /// <summary>
                    /// 套餐名称
                    /// </summary>
                    public string PricePlanName
                    {
                        get { return pricePlanName; }
                        set { pricePlanName = value; }
                    }

                    private string pricePlanDesc = "";
                    /// <summary>
                    /// 套餐描述
                    /// </summary>
                    public string PricePlanDesc
                    {
                        get { return pricePlanDesc; }
                        set { pricePlanDesc = value; }
                    }

                    private string dealTime = "";
                    /// <summary>
                    /// 用户套餐开通时间

                    /// </summary>
                    public string DealTime
                    {
                        get { return dealTime; }
                        set { dealTime = value; }
                    }

                    private string preferencesEndTime = "";
                    /// <summary>
                    /// 用户套餐结束时间
                    /// </summary>
                    public string PreferencesEndTime
                    {
                        get { return preferencesEndTime; }
                        set { preferencesEndTime = value; }
                    }
                }

                private string userName = "";
                /// <summary>
                /// 用户姓名
                /// </summary>
                public string UserName
                {
                    get { return userName; }
                    set { userName = value; }
                }

                private CustGroupResult custGroup = new CustGroupResult();
                /// <summary>
                /// 客户战略分群
                /// </summary>
                public CustGroupResult CustGroup
                {
                    get { return custGroup; }
                    set { custGroup = value; }
                }
                public class CustGroupResult
                {
                    private string group = "";
                    /// <summary>
                    /// 客户战略分群编码
                    /// </summary>
                    public string Group
                    {
                        get { return group; }
                        set { group = value; }
                    }

                    private string groupName = "";
                    /// <summary>
                    /// 客户战略分群名称
                    /// </summary>
                    public string GroupName
                    {
                        get { return groupName; }
                        set { groupName = value; }
                    }

                }

                private IdentityInfoResult identityInfo = new IdentityInfoResult();
                /// <summary>
                /// 证件标识信息
                /// </summary>
                public IdentityInfoResult IdentityInfo
                {
                    get { return identityInfo; }
                    set { identityInfo = value; }
                }
                public class IdentityInfoResult
                {
                    private string identType = "";
                    /// <summary>
                    /// 证件类型
                    /// </summary>
                    public string IdentType
                    {
                        get { return identType; }
                        set { identType = value; }
                    }

                    private string identNum = "";
                    /// <summary>
                    /// 证件号码
                    /// </summary>
                    public string IdentNum
                    {
                        get { return identNum; }
                        set { identNum = value; }
                    }

                }

                private string userPayType = "";
                /// <summary>
                /// 用户付费类型
                /// </summary>
                public string UserPayType
                {
                    get { return userPayType; }
                    set { userPayType = value; }
                }

                private string telephone = "";
                /// <summary>
                /// 关联号码/宽带关联固话号码（带区号），对于手机号码，必须填写IMSI
                /// </summary>
                public string Telephone
                {
                    get { return telephone; }
                    set { telephone = value; }
                }

                private string bandwidth = "";
                /// <summary>
                /// 下行宽带带宽
                /// </summary>
                public string Bandwidth
                {
                    get { return bandwidth; }
                    set { bandwidth = value; }
                }

            }
        }
    }

    public class QryUserStatusReturn
    {
        public QryUserStatusReturn()
        { }

        private TcpContReturn tcpCont = new TcpContReturn();
        /// <summary>
        /// 会话控制
        /// </summary>
        public TcpContReturn TcpCont
        {
            get { return tcpCont; }
            set { tcpCont = value; }
        }

        private SvcContResult svcCont = new SvcContResult();
        /// <summary>
        /// 业务信息
        /// </summary>
        public SvcContResult SvcCont
        {
            get { return svcCont; }
            set { svcCont = value; }
        }

        public class TcpContReturn
        {
            private string actionCode = "";
            /// <summary>
            /// 动作类型标识
            /// </summary>
            public string ActionCode
            {
                get { return actionCode; }
                set { actionCode = value; }
            }

            private string transactionID = "";
            /// <summary>
            /// 请求流水号

            /// </summary>
            public string TransactionID
            {
                get { return transactionID; }
                set { transactionID = value; }
            }

            private string rspTime = "";
            /// <summary>
            /// 答时间，时间格式字符串：YYYYMMDDHHMMSS
            /// </summary>
            public string RspTime
            {
                get { return rspTime; }
                set { rspTime = value; }
            }

            private ResponseResult response = new ResponseResult();
            /// <summary>
            /// 应答信息
            /// </summary>
            public ResponseResult Response
            {
                get { return response; }
                set { response = value; }
            }

            public class ResponseResult
            {
                private string rspType = "";
                /// <summary>
                /// 会话应答类型
                /// </summary>
                public string RspType
                {
                    get { return rspType; }
                    set { rspType = value; }
                }

                private string rspCode = "";
                /// <summary>
                /// 会话应答代码
                /// </summary>
                public string RspCode
                {
                    get { return rspCode; }
                    set { rspCode = value; }
                }

                private string rspDesc = "";
                /// <summary>
                /// 会话应答描述
                /// </summary>
                public string RspDesc
                {
                    get { return rspDesc; }
                    set { rspDesc = value; }
                }

            }
        }

        public class SvcContResult
        {
            private QryInfoRspResult qryInfoRsp = new QryInfoRspResult();
            /// <summary>
            /// 用户信息查询
            /// </summary>
            public QryInfoRspResult QryInfoRsp
            {
                get { return qryInfoRsp; }
                set { qryInfoRsp = value; }
            }

            public class QryInfoRspResult
            {
                private string infoTypeID = "";
                /// <summary>
                /// 资料类型
                /// </summary>
                public string InfoTypeID
                {
                    get { return infoTypeID; }
                    set { infoTypeID = value; }
                }

                private InfoContResult infoCont = new InfoContResult();
                /// <summary>
                /// 资料内容
                /// </summary>
                public InfoContResult InfoCont
                {
                    get { return infoCont; }
                    set { infoCont = value; }
                }


                public class InfoContResult
                {
                    private StatusInfoResult statusInfo = new StatusInfoResult();
                    /// <summary>
                    /// 产品状态信息

                    /// </summary>
                    public StatusInfoResult StatusInfo
                    {
                        get { return statusInfo; }
                        set { statusInfo = value; }
                    }

                    public class StatusInfoResult
                    {
                        private string prodNbrType = "";
                        /// <summary>
                        /// 接入号类型

                        /// </summary>
                        public string ProdNbrType
                        {
                            get { return prodNbrType; }
                            set { prodNbrType = value; }
                        }

                        private string prodNbr = "";
                        /// <summary>
                        /// 接入号

                        /// </summary>
                        public string ProdNbr
                        {
                            get { return prodNbr; }
                            set { prodNbr = value; }
                        }

                        private string status = "";
                        /// <summary>
                        /// 户状态

                        /// </summary>
                        public string Status
                        {
                            get { return status; }
                            set { status = value; }
                        }

                        private BelongInfoResult belongInfo = new BelongInfoResult();
                        /// <summary>
                        /// 所属信息

                        /// </summary>
                        public BelongInfoResult BelongInfo
                        {
                            get { return belongInfo; }
                            set { belongInfo = value; }
                        }


                        public class BelongInfoResult
                        {
                            private string provinceCode = "";
                            /// <summary>
                            /// 所属省代码
                            /// </summary>
                            public string ProvinceCode
                            {
                                get { return provinceCode; }
                                set { provinceCode = value; }
                            }

                            private string provinceName = "";
                            /// <summary>
                            /// 所属省名称
                            /// </summary>
                            public string ProvinceName
                            {
                                get { return provinceName; }
                                set { provinceName = value; }
                            }

                            private string cityCode = "";
                            /// <summary>
                            /// 区号
                            /// </summary>
                            public string CityCode
                            {
                                get { return cityCode; }
                                set { cityCode = value; }
                            }

                            private string cityName = "";
                            /// <summary>
                            /// 属名称地市名称

                            /// </summary>
                            public string CityName
                            {
                                get { return cityName; }
                                set { cityName = value; }
                            }


                        }
                    }
                }
            }
        }
    }


    public class QryCustInfoReturn
    {
        public QryCustInfoReturn()
        { }

        private TcpContReturn tcpCont = new TcpContReturn();
        /// <summary>
        /// 会话控制
        /// </summary>
        public TcpContReturn TcpCont
        {
            get { return tcpCont; }
            set { tcpCont = value; }
        }

        private SvcContResult svcCont = new SvcContResult();
        /// <summary>
        /// 业务信息
        /// </summary>
        public SvcContResult SvcCont
        {
            get { return svcCont; }
            set { svcCont = value; }
        }

        public class TcpContReturn
        {
            private string actionCode = "";
            /// <summary>
            /// 动作类型标识
            /// </summary>
            public string ActionCode
            {
                get { return actionCode; }
                set { actionCode = value; }
            }

            private string transactionID = "";
            /// <summary>
            /// 请求流水号

            /// </summary>
            public string TransactionID
            {
                get { return transactionID; }
                set { transactionID = value; }
            }

            private string rspTime = "";
            /// <summary>
            /// 答时间，时间格式字符串：YYYYMMDDHHMMSS
            /// </summary>
            public string RspTime
            {
                get { return rspTime; }
                set { rspTime = value; }
            }

            private ResponseResult response = new ResponseResult();
            /// <summary>
            /// 应答信息
            /// </summary>
            public ResponseResult Response
            {
                get { return response; }
                set { response = value; }
            }

            public class ResponseResult
            {
                private string rspType = "";
                /// <summary>
                /// 会话应答类型
                /// </summary>
                public string RspType
                {
                    get { return rspType; }
                    set { rspType = value; }
                }

                private string rspCode = "";
                /// <summary>
                /// 会话应答代码
                /// </summary>
                public string RspCode
                {
                    get { return rspCode; }
                    set { rspCode = value; }
                }

                private string rspDesc = "";
                /// <summary>
                /// 会话应答描述
                /// </summary>
                public string RspDesc
                {
                    get { return rspDesc; }
                    set { rspDesc = value; }
                }

            }
        }

        public class SvcContResult
        {
            private QryInfoRspResult qryInfoRsp = new QryInfoRspResult();
            /// <summary>
            /// 用户信息查询
            /// </summary>
            public QryInfoRspResult QryInfoRsp
            {
                get { return qryInfoRsp; }
                set { qryInfoRsp = value; }
            }

            public class QryInfoRspResult
            {
                private string infoTypeID = "";
                /// <summary>
                /// 资料类型
                /// </summary>
                public string InfoTypeID
                {
                    get { return infoTypeID; }
                    set { infoTypeID = value; }
                }

                private InfoContResult infoCont = new InfoContResult();
                /// <summary>
                /// 资料内容
                /// </summary>
                public InfoContResult InfoCont
                {
                    get { return infoCont; }
                    set { infoCont = value; }
                }


                public class InfoContResult
                {
                    private CustInfoResult custInfo = new CustInfoResult();
                    /// <summary>
                    /// 客户常用信息
                    /// </summary>
                    public CustInfoResult CustInfo
                    {
                        get { return custInfo; }
                        set { custInfo = value; }
                    }

                    public class CustInfoResult
                    {
                        private BelongInfoResult belongInfo = new BelongInfoResult();

                        public BelongInfoResult BelongInfo
                        {
                            get { return belongInfo; }
                            set { belongInfo = value; }
                        }


                        public class BelongInfoResult
                        {
                            private string provinceCode = "";
                            /// <summary>
                            /// 所属省代码
                            /// </summary>
                            public string ProvinceCode
                            {
                                get { return provinceCode; }
                                set { provinceCode = value; }
                            }

                            private string provinceName = "";
                            /// <summary>
                            /// 所属省名称
                            /// </summary>
                            public string ProvinceName
                            {
                                get { return provinceName; }
                                set { provinceName = value; }
                            }

                            private string cityCode = "";
                            /// <summary>
                            /// 区号
                            /// </summary>
                            public string CityCode
                            {
                                get { return cityCode; }
                                set { cityCode = value; }
                            }

                            private string cityName = "";
                            /// <summary>
                            /// 属名称地市名称

                            /// </summary>
                            public string CityName
                            {
                                get { return cityName; }
                                set { cityName = value; }
                            }


                        }

                        private PartyCodeInfoResult partyCodeInfo = new PartyCodeInfoResult();

                        public PartyCodeInfoResult PartyCodeInfo
                        {
                            get { return partyCodeInfo; }
                            set { partyCodeInfo = value; }
                        }


                        public class PartyCodeInfoResult
                        {
                            private string codeType = "";
                            /// <summary>
                            /// 标识类型
                            /// </summary>
                            public string CodeType
                            {
                                get { return codeType; }
                                set { codeType = value; }
                            }

                            private string codeValue = "";
                            /// <summary>
                            /// 标识号码
                            /// </summary>
                            public string CodeValue
                            {
                                get { return codeValue; }
                                set { codeValue = value; }
                            }

                            private string cityCode = "";
                            /// <summary>
                            /// 区号
                            /// </summary>
                            public string CityCode
                            {
                                get { return cityCode; }
                                set { cityCode = value; }
                            }

                           

                        }

                        private IdentityInfoResult identityInfo = new IdentityInfoResult();
                        /// <summary>
                        /// 证件标识信息
                        /// </summary>
                        public IdentityInfoResult IdentityInfo
                        {
                            get { return identityInfo; }
                            set { identityInfo = value; }
                        }
                        public class IdentityInfoResult
                        {
                            private string identType = "";
                            /// <summary>
                            /// 证件类型
                            /// </summary>
                            public string IdentType
                            {
                                get { return identType; }
                                set { identType = value; }
                            }

                            private string identNum = "";
                            /// <summary>
                            /// 证件号码
                            /// </summary>
                            public string IdentNum
                            {
                                get { return identNum; }
                                set { identNum = value; }
                            }

                        }

                        private string custName = "";
                        /// <summary>
                        /// 客户名称
                        /// </summary>
                        public string CustName
                        {
                            get { return custName; }
                            set { custName = value; }
                        }

                        private string custBrand = "";
                        /// <summary>
                        /// 客户品牌
                        /// </summary>
                        public string CustBrand
                        {
                            get { return custBrand; }
                            set { custBrand = value; }
                        }

                        private string custGroup = "";
                        /// <summary>
                        /// 客户战略分群
                        /// </summary>
                        public string CustGroup
                        {
                            get { return custGroup; }
                            set { custGroup = value; }
                        }

                        private string custServiceLevel = "";
                        /// <summary>
                        /// 客户等级
                        /// </summary>
                        public string CustServiceLevel
                        {
                            get { return custServiceLevel; }
                            set { custServiceLevel = value; }
                        }

                        private string custAddress = "";
                        /// <summary>
                        /// 通信地址
                        /// </summary>
                        public string CustAddress
                        {
                            get { return custAddress; }
                            set { custAddress = value; }
                        }
                        

                    }
                }
            }
        }
    }

    #region 状态变更实时同步

    public class ReqTcpCont
    {
        public string BusCode = "";
        public string ServiceCode = "";
        public string ServiceContractVer = "";
        public string ActionCode = "";
        public string TransactionID = "";
        public string ServiceLevel = "";
        public string SrcOrgID = "";
        public string SrcSysID = "";
        public string SrcSysSign = "";
        public string DstOrgID = "";
        public string DstSysID = "";
        public string ReqTime = "";

    }

    public class BPMOrder
    {
        public string BizEventNbr = "";
        public string BizEventTime = "";
        public string OrderTypeCd = "";
        public string ProdNbrType = "";
        public string ProdNbr = "";
        public string OldProdNbr = "";
        public string IMSI = "";
        public string OldIMSI = "";
        public string CustID = "";
        public string OldCustID = "";
        public BelongInfo BelongInfo = new BelongInfo();
    }

    public class BelongInfo
    {
        public string ProvinceCode = "";
        public string ProvinceName = "";
        public string CityCode = "";
        public string CityName = "";
    }

    public class StatusChangedReqBPMOrderEvent
    {
        public List<BPMOrder> BMPOrderList = new List<BPMOrder>();

        public int OrderSum = 0;
    }

    public class Response
    {
        public string RspType = "";
        public string RspCode = "";
        public string RspDesc = "";
    }

    public class ResTcpCont
    {
        public string ActionCode = "";
        public string TransactionID = "";
        public string RspTime = "";
        public Response Response = new Response();

        public ResTcpCont(string PActionCode, string PTrasactionCode, string PRspTime, string PRspType, string PRspCode, string PRspDesc)
        {
            this.ActionCode = PActionCode;
            this.TransactionID = PTrasactionCode;
            this.RspTime = PRspTime;
            this.Response.RspType = PRspType;
            this.Response.RspCode = PRspCode;
            this.Response.RspDesc = PRspDesc;
        }
    }


    #endregion
}

