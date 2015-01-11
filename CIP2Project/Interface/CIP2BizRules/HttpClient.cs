using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;

namespace Linkage.BestTone.Interface.Rule
{
    /// <summary> 
    /// 继承WebClient类 
    /// 提供向 URI 标识的资源发送数据和从 URI 标识的资源接收数据的公共方法 
    /// 支持以 http:、https:、ftp:、和 file: 方案标识符开头的 URI 
    /// </summary> 
    public class HttpClient : WebClient
    {
        #region 远程POST数据并返回数据
        /// <summary> 
        /// 利用WebClient 远程POST数据并返回数据 
        /// </summary> 
        /// <param name="strUrl">远程URL地址</param> 
        /// <param name="strParams">参数</param> 
        /// <param name="RespEncode">POST数据的编码</param> 
        /// <param name="ReqEncode">获取数据的编码</param> 
        /// <returns></returns> 
        public static string PostData(string strUrl, string strParams, Encoding RespEncode, Encoding ReqEncode)
        {
            HttpClient httpclient = new HttpClient();
            try
            {
                //打开页面 
                httpclient.Credentials = CredentialCache.DefaultCredentials;
                
                //从指定的URI下载资源 
                byte[] responseData = httpclient.DownloadData(strUrl);

                string srcString = RespEncode.GetString(responseData);

                httpclient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                string postString = strParams;
                // 将字符串转换成字节数组 
                byte[] postData = Encoding.ASCII.GetBytes(postString);
                // 上传数据，返回页面的字节数组 
                responseData = httpclient.UploadData(strUrl, "POST", postData);
                srcString = ReqEncode.GetString(responseData);

                return srcString;
            }
            catch (Exception ex)
            {
                //记录异常日志 
                //释放资源 
                httpclient.Dispose();
                return string.Empty;
            }
        }

        #endregion



        #region 远程POST数据并返回数据
        /// <summary> 
        /// 利用WebClient 远程POST数据并返回数据 
        /// </summary> 
        /// <param name="strUrl">远程URL地址</param> 
        /// <param name="strParams">参数</param> 
        /// <param name="RespEncode">POST数据的编码</param> 
        /// <param name="ReqEncode">获取数据的编码</param> 
        /// <returns></returns> 
        public static string PostMyData(string strUrl, string strParams,int timeout)
        {

            try
            {
                string str="";
                
                byte[] bs = Encoding.ASCII.GetBytes(strParams);

                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(strUrl);
                req.Method = "POST";
                //req.ContentType = "application/x-www-form-urlencoded";
                req.Timeout = timeout;
                req.ContentLength = bs.Length;

                using (Stream reqStream = req.GetRequestStream())
                {
                    reqStream.Write(bs, 0, bs.Length);
                }
                using (WebResponse wr = req.GetResponse())
                {
                    Stream strm = wr.GetResponseStream();

                    StreamReader sr = new StreamReader(strm);

                    string line;

                    while ((line = sr.ReadLine()) != null)
                    {

                        str = str + line;

                    }

                    strm.Close();

                }
                return str;
            }
            catch (Exception ex)
            {
               
                return string.Empty;
            }
        }

        #endregion

        /// <summary> 
        /// 利用WebClient 远程POST XML数据并返回数据 
        /// </summary> 
        /// <param name="strUrl">远程URL地址</param> 
        /// <param name="strParams">参数</param> 
        /// <param name="RespEncode">POST数据的编码</param> 
        /// <param name="ReqEncode">获取数据的编码</param> 
        /// <returns></returns> 
        public static string PostXmlData(string strUrl, string strParams, Encoding RespEncode, Encoding ReqEncode)
        {
            HttpClient httpclient = new HttpClient();
            try
            {
                //打开页面 
                httpclient.Credentials = CredentialCache.DefaultCredentials;
                //从指定的URI下载资源 
                byte[] responseData = httpclient.DownloadData(strUrl);
                string srcString = RespEncode.GetString(responseData);

                httpclient.Headers.Add("Content-Type", "text/xml");
                string postString = strParams;
                // 将字符串转换成字节数组 
                byte[] postData = Encoding.ASCII.GetBytes(postString);
                // 上传数据，返回页面的字节数组 
                responseData = httpclient.UploadData(strUrl, "GET", postData);
                srcString = ReqEncode.GetString(responseData);

                return srcString;
            }
            catch (Exception ex)
            {
                //记录异常日志 
                //释放资源 
                httpclient.Dispose();
                return string.Empty;
            }
        }
    }




}


