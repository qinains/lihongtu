<%@ WebHandler Language="C#" Class="ContactInfoHandler" %>

using System;
using System.Web;
using Linkage.BestTone.Interface.BTException;
using Linkage.BestTone.Interface.Rule;
using Linkage.BestTone.Interface.Utility;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Data;

    public class ContactInfoHandler : IHttpHandler
    {

        public String SPID = "35433334";
        
        public void ProcessRequest(HttpContext context)
        {
            context.Response.Clear();
            context.Response.ContentType = "json/plain";
            context.Response.ContentEncoding = Encoding.UTF8;

            StringBuilder returnMsg = new StringBuilder();
            try
            {
                String type = context.Request["Type"];
                SPID = context.Request["SPID"];
                if (String.IsNullOrEmpty(SPID))
                {
                    SPID = "35433334";
                }
                switch (type)
                {
                    case "GetAddressInfoByID":
                        returnMsg.Append(GetAddressInfoByID(context));
                        break;
                    case "SaveAddressInfo":
                        returnMsg.Append(SaveAddressInfo(context));
                        break;
                    case "DeleteAddressInfo":
                        returnMsg.Append(DeleteAddressInfo(context));
                        break;
                    case "GetProvinces":
                        returnMsg.Append(GetProvinces(context));
                        break;
                    case "GetCities":
                        returnMsg.Append(GetCities(context));
                        break;
                    case "GetDistircts":
                        returnMsg.Append(GetDistircts(context));
                        break;  
                    default:
                        returnMsg.Append("[{result:\"false\",info:\"" + "找不到方法" + "\"}]");
                        break;
                }

            }
            catch (Exception e)
            {
                returnMsg.Append("[{result:\"false\",info:\"" + e.Message + "\"}]");
            }
            finally
            { }
            context.Response.Write(returnMsg.ToString());
            context.Response.Flush();
            context.Response.End();
        }

        protected String GetDictName(HttpContext context)
        {
            StringBuilder returnMsg = new StringBuilder();
            Int32 Result;
            String ErrMsg = "";
            string AreaCode = context.Request["AreaCode"];
            string Grade = context.Request["Grade"];
            String Code = String.Empty; 
            try
            {
                String DictName = AddressInfoBO.GetDictCityByCode(AreaCode, Convert.ToInt16(Grade), out  Code,out Result, out ErrMsg);
                if (Result == 0)
                {
                    returnMsg.Append("{\"result\":\"true\", \"msg\":\"成功\",");
                    returnMsg.AppendFormat("\"Code\":\"{0}\",",Code); 
                    returnMsg.AppendFormat("\"Name\":\"{0}\"", DictName);
                    returnMsg.Append("}");  
                }
                else
                {
                    returnMsg.Append("{result:\"false\",\"msg:\":\"失败\"}");
                }
            }
            catch (Exception e)
            {
                returnMsg.Append("{result:\"false\",info:\"" + e.Message + "\"}");
            }  
            return returnMsg.ToString();
        }
        
        protected String GetProvinces(HttpContext context)
        {
            StringBuilder returnMsg = new StringBuilder();
            Int32 Result;
            String ErrMsg = "";
            try
            {
                DictCity[] provinces = AddressInfoBO.GetProvinces(out Result, out ErrMsg);
                if (provinces != null && provinces.Length > 0)
                {
                    returnMsg.Append("{\"result\":\"true\",\"info\":[");
                    int index = 0;
                    foreach (DictCity dc in provinces)
                    {
                        index++;
                        returnMsg.Append("{");
                        returnMsg.AppendFormat("\"ID\":\"{0}\",", dc.ID);
                        returnMsg.AppendFormat("\"Name\":\"{0}\",", dc.Name);
                        returnMsg.AppendFormat("\"Code\":\"{0}\",", dc.Code);
                        returnMsg.AppendFormat("\"Grade\":\"{0}\"", dc.Grade);
                        returnMsg.Append("}");
                        if (index < provinces.Length)
                        {
                            returnMsg.Append(",");
                        }
                    }
                }
                returnMsg.Append("]}");
            }
            catch (Exception e)
            {
                returnMsg.Append("{\"result\":\"false\",\"msg\":\"" + e.Message + "\"}");
            }
            return returnMsg.ToString();
        }


        protected String GetCities(HttpContext context)
        {
            StringBuilder returnMsg = new StringBuilder();
            Int32 Result;
            String ErrMsg = "";
            string ProvinceID = context.Request["ProvinceID"]; 
            try
            {
                DictCity[] cities = AddressInfoBO.GetCities(ProvinceID, out Result, out ErrMsg);
                if (cities != null && cities.Length > 0)
                {
                    returnMsg.Append("{\"result\":\"true\",\"info\":[");
                    int index = 0;
                    foreach (DictCity dc in cities)
                    {
                        index++;
                        returnMsg.Append("{");
                        returnMsg.AppendFormat("\"ID\":\"{0}\",", dc.ID);
                        returnMsg.AppendFormat("\"Name\":\"{0}\",", dc.Name);
                        returnMsg.AppendFormat("\"Code\":\"{0}\",", dc.Code);
                        returnMsg.AppendFormat("\"Grade\":\"{0}\"", dc.Grade);
                        returnMsg.Append("}");
                        if (index < cities.Length)
                        {
                            returnMsg.Append(",");
                        }
                    }
                }
                returnMsg.Append("]}");
            }
            catch (Exception e)
            {
                returnMsg.Append("{\"result\":\"false\",\"msg\":\"" + e.Message + "\"}");
            }
            return returnMsg.ToString();
        }

        protected String GetDistircts(HttpContext context)
        {
            StringBuilder returnMsg = new StringBuilder();
            Int32 Result;
            String ErrMsg = "";
            string CityID = context.Request["CityID"];
            try
            {
                DictCity[] distircts = AddressInfoBO.GetDistircts(CityID, out Result, out ErrMsg);
                if (distircts != null && distircts.Length > 0)
                {
                    returnMsg.Append("{\"result\":\"true\",\"info\":[");
                    int index = 0;
                    foreach (DictCity dc in distircts)
                    {
                        index++;
                        returnMsg.Append("{");
                        returnMsg.AppendFormat("\"ID\":\"{0}\",", dc.ID);
                        returnMsg.AppendFormat("\"Name\":\"{0}\",", dc.Name);
                        returnMsg.AppendFormat("\"Code\":\"{0}\",", dc.Code);
                        returnMsg.AppendFormat("\"Grade\":\"{0}\"", dc.Grade);
                        returnMsg.Append("}");
                        if (index < distircts.Length)
                        {
                            returnMsg.Append(",");
                        }
                    }
                }
                returnMsg.Append("]}");
            }
            catch (Exception e)
            {
                returnMsg.Append("{\"result\":\"false\",\"msg\":\"" + e.Message + "\"}");
            }
            return returnMsg.ToString();
        }
        
        
        protected String DeleteAddressInfo(HttpContext context)
        {
            StringBuilder returnMsg = new StringBuilder();
            Int32 Result;
            String ErrMsg = "";
            String CustID = context.Request["CustID"];
            String AddressID = context.Request["AddressID"];
            try
            {
                //Result = AddressInfoBO.delete(SPID, CustID, Convert.ToInt32(AddressID), out ErrMsg);
                Result =  AddressInfoBO.AddressInfoDelete(SPID, Convert.ToInt32(AddressID), out ErrMsg);
                
                if (Result == 0)
                {
                    returnMsg.Append("{\"result\":\"true\",\"msg\":\"删除成功!\"}");
                }
                else
                {
                    returnMsg.Append("{\"result\":\"false\",\"msg\":\""+ErrMsg+"\"}");
                }
            }
            catch (Exception e)
            {
                returnMsg.Append("{\"result\":\"false\",\"msg\":\"" + e.Message + "\"}");
            }  
            return returnMsg.ToString();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected String SaveAddressInfo(HttpContext context)
        {
            StringBuilder returnMsg = new StringBuilder();
            Int32 Result;
            String ErrMsg = "";
            
            String CustID = context.Request["CustID"];
            String AreaCode = context.Request["AreaCode"]; 
            String AddressID = context.Request["AddressID"];
            String Address = context.Request["Address"];
            String ZipCode = context.Request["ZipCode"]; 
            String Type = context.Request["AddRessType"];
            String RelationPerson = context.Request["RelationPerson"];
            String FixedPhone = context.Request["FixedPhone"];
            String OtherType = context.Request["OtherType"];
            String ContactPhone = context.Request["ContactPhone"];
            AddressInfo air = new AddressInfo();
             
            air.Address = Address;
            air.AreaCode = AreaCode;
            air.Zipcode = ZipCode; 
            air.OtherType = OtherType;
            air.Type = Type;
            air.RelationPerson = RelationPerson;
            air.FixedPhone = FixedPhone;   
            air.Mobile = ContactPhone;
            //air.CustID = CustID;
            
            try
            {
                if (String.IsNullOrEmpty(AddressID))
                {
                    //Result = AddressInfoBO.Add(SPID, CustID, air, out air.AddressID, out ErrMsg);
                    Result = AddressInfoBO.AddressInfoAdd(SPID, CustID, air, out air.AddressID, out ErrMsg);
                }
                else
                {
                    air.AddressID = Convert.ToInt32(AddressID);
                    //Result = AddressInfoBO.save(SPID, CustID, air, out ErrMsg);
                    Result = AddressInfoBO.AddressInfoSave(SPID, CustID, air, out ErrMsg);
                }

                String province = String.Empty;
                String provinceCode = String.Empty;
                String city = String.Empty;
                String cityCode = String.Empty;
                String district = String.Empty;
                String districtCode = String.Empty;
                int _Result = -1;
                String _ErrMsg = String.Empty; 
                if (Result == 0)
                {
                    returnMsg.Append("{\"result\":\"true\",\"msg\":\"保存成功！\",\"info\":[{");
                    returnMsg.AppendFormat("\"ID\":\"{0}\",", air.AddressID);
                    returnMsg.AppendFormat("\"Address\":\"{0}\",", air.Address);
                    province = AddressInfoBO.GetDictCityByCode(air.AreaCode, 1, out provinceCode, out _Result, out _ErrMsg);
                    city = AddressInfoBO.GetDictCityByCode(air.AreaCode, 2, out cityCode, out _Result, out _ErrMsg);
                    district = AddressInfoBO.GetDictCityByCode(air.AreaCode, 3, out districtCode, out _Result, out _ErrMsg);
                    returnMsg.AppendFormat("\"Address\":\"{0}\",", air.Address);
                    returnMsg.AppendFormat("\"ZipCode\":\"{0}\",", air.Zipcode);
                    returnMsg.AppendFormat("\"Province\":\"{0}\",", province);
                    returnMsg.AppendFormat("\"ProvinceCode\":\"{0}\",", provinceCode);
                    returnMsg.AppendFormat("\"City\":\"{0}\",", city);
                    returnMsg.AppendFormat("\"CityCode\":\"{0}\",", cityCode);
                    returnMsg.AppendFormat("\"District\":\"{0}\",", district);
                    returnMsg.AppendFormat("\"DistrictCode\":\"{0}\",", districtCode);                     
                      
                    returnMsg.AppendFormat("\"Phone\":\"{0}\",", air.Mobile);
                    returnMsg.AppendFormat("\"Type\":\"{0}\",", air.Type);
                    returnMsg.AppendFormat("\"RelationPerson\":\"{0}\",", air.RelationPerson);
                    returnMsg.AppendFormat("\"FixedPhone\":\"{0}\",", air.FixedPhone);
                    returnMsg.AppendFormat("\"OtherType\":\"{0}\"", air.OtherType);
                    returnMsg.Append("}]");
                    returnMsg.Append("}");
                }
                else
                {
                    returnMsg.Append("{\"result\":\"false\",\"msg\":\"" + ErrMsg + "\"}");
                }
            }
            catch (Exception e)
            {
                returnMsg.Append("{\"result\":\"false\",\"msg\":\"" + e.Message + "\"}");
            }   
            return returnMsg.ToString();
        }

     
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected String GetAddressInfoByID(HttpContext context)
        {
            StringBuilder returnMsg = new StringBuilder();
            String CustID = context.Request["CustID"];
            Int32 Result = 0;
            String ErrMsg = "";
            try
            {
                AddressInfoRecord[] addresses = AddressInfoBO.GetAddressInfo(SPID, CustID, out Result, out ErrMsg);
                if (Result == 0)
                {
                    returnMsg.Append("{\"result\":\"true\",\"info\":[");
                    if (addresses != null && addresses.Length > 0)
                    {
                        int index = 0;
                        String province = String.Empty;
                        String provinceCode = String.Empty;
                        String city = String.Empty;
                        String cityCode = String.Empty;
                        String district = String.Empty;
                        String districtCode = String.Empty;
                        int _Result = -1;
                        String _ErrMsg = String.Empty;  
                        foreach (AddressInfoRecord air in addresses)
                        {
                            index++;
                            returnMsg.Append("{");
                            returnMsg.AppendFormat("\"ID\":\"{0}\",", air.AddressID);
                            returnMsg.AppendFormat("\"AreaCode\":\"{0}\",", air.AreaCode);
                            province = AddressInfoBO.GetDictCityByCode(air.AreaCode, 1,out provinceCode, out _Result, out _ErrMsg);
                            city = AddressInfoBO.GetDictCityByCode(air.AreaCode, 2, out cityCode,out _Result, out _ErrMsg);
                            district = AddressInfoBO.GetDictCityByCode(air.AreaCode, 3, out districtCode, out _Result, out _ErrMsg); 
                            returnMsg.AppendFormat("\"Address\":\"{0}\",", air.Address);
                            returnMsg.AppendFormat("\"ZipCode\":\"{0}\",", air.Zipcode); 
                            returnMsg.AppendFormat("\"Province\":\"{0}\",", province);
                            returnMsg.AppendFormat("\"ProvinceCode\":\"{0}\",", provinceCode); 
                            returnMsg.AppendFormat("\"City\":\"{0}\",", city);
                            returnMsg.AppendFormat("\"CityCode\":\"{0}\",", cityCode); 
                            returnMsg.AppendFormat("\"District\":\"{0}\",", district);
                            returnMsg.AppendFormat("\"DistrictCode\":\"{0}\",", districtCode); 
                            returnMsg.AppendFormat("\"Phone\":\"{0}\",", air.Mobile);
                            returnMsg.AppendFormat("\"Type\":\"{0}\",", air.Type); 
                            returnMsg.AppendFormat("\"RelationPerson\":\"{0}\",", air.RelationPerson);
                            returnMsg.AppendFormat("\"FixedPhone\":\"{0}\",", air.FixedPhone);
                            returnMsg.AppendFormat("\"OtherType\":\"{0}\"", air.OtherType);
                            returnMsg.Append("}");
                            if (index < addresses.Length)
                            {
                                returnMsg.Append(",");
                            }
                        }
                    }
                    returnMsg.Append("]}");
                }
                else
                {
                    returnMsg.Append("{\"result\":\"false\",\"msg\":\"" + ErrMsg + "\"}");
                }
            }
            catch (Exception e)
            {
                returnMsg.Append("{\"result\":\"false\",\"msg\":\"" + e.Message + "\"}");
            }  
            return returnMsg.ToString();
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

    }
