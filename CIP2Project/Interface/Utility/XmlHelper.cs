using System;
using System.Collections.Generic;
using System.Text;

using System.Xml;

namespace Microsoft.DVAP.Interface.Utility
{
    public class XmlHelper
    {
        private XmlDocument xmlDoc;

        public XmlHelper()
        {
            String path = AppDomain.CurrentDomain + @"XmlModel\ConstParams.xml";
            xmlDoc.Load(path);
        }
        public XmlHelper(String fileName)
        {
            xmlDoc.Load(fileName);
        }


    }
}
