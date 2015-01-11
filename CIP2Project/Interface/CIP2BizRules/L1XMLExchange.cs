using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;

namespace Linkage.BestTone.Interface.Rule
{
    public class L1XMLExchange
    {
        public static readonly string RootPath = AppDomain.CurrentDomain.BaseDirectory;

        /// <summary>
        /// 暂时无用
        /// </summary>
        public string BuildQryCustInfoXML(string MSID, string REQ_RESULTFORMAT)
        {
            System.Xml.XmlDocument XmlDoc = new XmlDocument();
            XmlDoc.Load(RootPath + "/SampleXml/LIReq.xml");
            System.Xml.XmlNodeList nodeTemplet = XmlDoc.DocumentElement.ChildNodes;
            foreach (XmlElement element in nodeTemplet)
            {
                if (element.Name.ToLower() == "lir")
                {
                    //得到该节点的子节点
                    XmlNodeList nodelist = element.ChildNodes;

                    foreach (XmlElement childnodelist in nodelist)
                    {

                        switch (childnodelist.Name.ToLower())
                        {
                            case "msids":
                                //得到该节点的子节点
                                XmlNodeList nodelistMsids = childnodelist.ChildNodes;

                                foreach (XmlElement childnodeElement in nodelistMsids)
                                {

                                    switch (childnodeElement.Name.ToLower())
                                    {
                                        case "msid":
                                            childnodeElement.InnerText = MSID;
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            case "req_resultformat":

                                childnodelist.InnerText = REQ_RESULTFORMAT;
                                
                                break;

                            default:
                                break;
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

        public PhoenPostionQueryReturn AnalysisPhoenPostionQueryXML(string PhoenPostionQueryXML)
        {
            System.Xml.XmlDocument XmlDoc = new XmlDocument();
            XmlDoc.InnerXml = PhoenPostionQueryXML;

            XmlNodeList nodeTemplet = XmlDoc.DocumentElement.ChildNodes;
            PhoenPostionQueryReturn phoenPostionQueryReturn = new PhoenPostionQueryReturn();
            //XmlNodeList nodeTemplet = XmlDoc.DocumentElement.ChildNodes;
            foreach (XmlElement element in nodeTemplet)
            {
                if (element.Name.ToUpper() == "LIA")
                {
                    //得到该节点的子节点
                    XmlNodeList nodelist = element.ChildNodes;

                    foreach (XmlElement childnodelist in nodelist)
                    {
                        switch (childnodelist.Name.ToUpper())
                        {
                            case "RESULT":
                                phoenPostionQueryReturn.RESULT = childnodelist.InnerText;
                                break;
                            case "POSINFOS":
                                //得到该节点的子节点
                                XmlNodeList POSINFOSnodelist = childnodelist.ChildNodes;

                                foreach (XmlElement POSINFOSnodelistEle in POSINFOSnodelist)
                                {
                                    switch (POSINFOSnodelistEle.Name.ToUpper())
                                    {
                                        case "POSINFO":
                                            XmlNodeList POSINFOnodelist = POSINFOSnodelistEle.ChildNodes;
                                            foreach (XmlElement POSINFOnodelistEle in POSINFOnodelist)
                                            {
                                                switch (POSINFOnodelistEle.Name.ToUpper())
                                                {
                                                    case "ROAMCITY":
                                                        phoenPostionQueryReturn.ROAMCITY = POSINFOnodelistEle.InnerText;
                                                        break;
                                                    case "LATITUDETYPE":
                                                        phoenPostionQueryReturn.LATITUDETYPE = POSINFOnodelistEle.InnerText;
                                                        break;
                                                    case "LATITUDE":
                                                        phoenPostionQueryReturn.LATITUDE = POSINFOnodelistEle.InnerText;
                                                        break;
                                                    case "LONGITUDETYPE":
                                                        phoenPostionQueryReturn.LONGITUDETYPE = POSINFOnodelistEle.InnerText;
                                                        break;
                                                    case "LONGITUDE":
                                                        phoenPostionQueryReturn.LONGITUDE = POSINFOnodelistEle.InnerText;
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
                                break;
                            default:
                                break;
                        }
                        
                    }
                }
            }


            return phoenPostionQueryReturn;
        }

    }

    public class PhoenPostionQueryReturn
    {
        public PhoenPostionQueryReturn()
        { }
        private string rOAMCITY = "";
        /// <summary>
        /// 动作类型标识
        /// </summary>
        public string ROAMCITY
        {
            get { return rOAMCITY; }
            set { rOAMCITY = value; }
        }
        private string rESULT = "";
        /// <summary>
        /// 动作类型标识
        /// </summary>
        public string RESULT
        {
            get { return rESULT; }
            set { rESULT = value; }
        }

        private string lATITUDETYPE = "";
        /// <summary>
        /// 动作类型标识
        /// </summary>
        public string LATITUDETYPE
        {
            get { return lATITUDETYPE; }
            set { lATITUDETYPE = value; }
        }

        private string lATITUDE = "";
        /// <summary>
        /// 动作类型标识
        /// </summary>
        public string LATITUDE
        {
            get { return lATITUDE; }
            set { lATITUDE = value; }
        }

        private string lONGITUDETYPE = "";
        /// <summary>
        /// 动作类型标识
        /// </summary>
        public string LONGITUDETYPE
        {
            get { return lONGITUDETYPE; }
            set { lONGITUDETYPE = value; }
        }

        private string lONGITUDE = "";
        /// <summary>
        /// 动作类型标识
        /// </summary>
        public string LONGITUDE
        {
            get { return lONGITUDE; }
            set { lONGITUDE = value; }
        }
    }
        
}

