using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using Linkage.BestTone.Interface.Utility;
namespace Linkage.BestTone.Interface.Rule
{
    /// <summary>
    /// Author Lihongtu 2013.02.26
    /// </summary>
    public class SmsClient
    {
        private String _userId;
        private String _passWord;
        private String sequenceID = "0000";

        private String _ip;
        private int _port;
        private Socket socket;
       

        private Boolean loginFlag;
        public static String COMPART_TAG = "&"; // 数据包不同字段的分隔符

        public SmsClient(String ip, int port)
        {
            _ip = ip;
            _port = port;
        }

      
    
        public void setUserId(String UserId)
        {
            _userId = UserId;
        }

        public void setPassWord(String PassWord)
        {
            _passWord = PassWord;
        }

        public Socket getSocket()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            return socket;
        }

        public Boolean connection()
        {
            IPEndPoint ie = new IPEndPoint(IPAddress.Parse(_ip), _port);
            StringBuilder msg = new StringBuilder();
            try
            {
                socket.Connect(ie);
            }
            catch (SocketException e)
            {
                msg.AppendFormat(e.ToString());
                log(msg);
                return false;
            }
            return true;
        }

        public void destroy()
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }

        public Boolean login()
        {
            Boolean flag = connection();
            StringBuilder msg = new StringBuilder();
            if (flag)
            {
                try
                {
                    sendLoginData();
                    receiveLoginData();
                }
                catch (Exception e)
                {
                    log(msg.AppendFormat(e.ToString()));
                }
            }
            else
            { 
                // 连不上服务器。。。
                msg.AppendFormat("连不上服务器。。。");
                log(msg);
            }
            return loginFlag;

        }

        private void sendLoginData()
        {
            byte[] tagByte = Encoding.UTF8.GetBytes(COMPART_TAG);

            byte[] packHeadByte = Encoding.UTF8.GetBytes(PackType.LOGIN_REQUEST_BACK);

            byte[] userIdByte = Encoding.UTF8.GetBytes(_userId);
            byte[] passwordByte = Encoding.UTF8.GetBytes(_passWord);
            byte[] sequenceIdByte = Encoding.UTF8.GetBytes(sequenceID);
            int length = packHeadByte.Length;
            length += userIdByte.Length;
            length += passwordByte.Length;
            length += sequenceIdByte.Length;
            length += 4;
            int size = length + Encoding.UTF8.GetBytes(Convert.ToString(length)).Length;
            length = length + Encoding.UTF8.GetBytes(Convert.ToString(size)).Length;
            
            List<byte> bb = new List<byte>();
            bb.AddRange(Encoding.UTF8.GetBytes(Convert.ToString(length)));
            bb.AddRange(tagByte);
            bb.AddRange(packHeadByte);
            bb.AddRange(tagByte);
            bb.AddRange(sequenceIdByte);
            bb.AddRange(tagByte);
            bb.AddRange(userIdByte);
            bb.AddRange(tagByte);
            bb.AddRange(passwordByte);
            socket.Send(bb.ToArray());
 
        }


        private void receiveLoginData()
        {
            byte[] readData = new byte[100];
            socket.Receive(readData);
            String receiveStr = Encoding.UTF8.GetString(readData);
            String[] datas = receiveStr.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder msg = new StringBuilder();
            if (datas.Length < 2)
            {
                msg.AppendFormat("接收数据包为空或格式错误！");
                return;  //接收数据包为空或格式错误！
            }
            String pageType = datas[1]; //包类型
            // 判断数据包的类型 登录返回包
            if (PackType.LOGIN_BACK.Equals(pageType))
            {   //ReceiveCode.LOGIN_SUCCESS.Equals(datas[3])
                if (datas[3].StartsWith(ReceiveCode.LOGIN_SUCCESS))
                {
                    loginFlag = true;
                }
                else
                {
                    msg.AppendFormat("登录失败代码");
                    log(msg);
                    // 登录失败代码
                }
            }
        }


        public String sendSms(SmsMessage sms)
        {
            StringBuilder msg = new StringBuilder();
            if (loginFlag == false)
            {
                login();
            }
            if (loginFlag)
            {
                String smsType = sms.SmsType;
                if (smsType == null)
                {
                    smsType = PackType.DATA_BACK;
                }

                byte[] tagByte = Encoding.Default.GetBytes(COMPART_TAG);
                byte[] packHeadByte = Encoding.Default.GetBytes(smsType);
                byte[] sequenceIdByte = Encoding.Default.GetBytes(sms.SequenceId);
                byte[] targetNumberByte = Encoding.Default.GetBytes(sms.TargetNumber);
                byte[] sendNumberByte = Encoding.Default.GetBytes(sms.SendNumber);

                byte[] contentByte = null;
                try
                {
                    contentByte = Encoding.Default.GetBytes(sms.Content);

                }
                catch (Exception e)
                {
                    msg.AppendFormat(e.ToString());
                    log(msg);
                }

                byte[] createTimeByte = Encoding.Default.GetBytes(sms.CreateTime);
                int length = packHeadByte.Length;
                length += sequenceIdByte.Length;
                length += targetNumberByte.Length;
                length += sendNumberByte.Length;
                length += contentByte.Length;
                length += createTimeByte.Length;
                length += 6;

                int size = length + Encoding.Default.GetBytes(Convert.ToString(length)).Length;
                length = length + Encoding.Default.GetBytes(Convert.ToString(size)).Length;
                try
                {
                    List<byte> bb = new List<byte>();
                    bb.AddRange(Encoding.Default.GetBytes(Convert.ToString(length)));
                    bb.AddRange(tagByte);
                    bb.AddRange(packHeadByte);
                    bb.AddRange(tagByte);
                    bb.AddRange(sequenceIdByte);
                    bb.AddRange(tagByte);

                    bb.AddRange(targetNumberByte);
                    bb.AddRange(tagByte);
                    bb.AddRange(sendNumberByte);
                    bb.AddRange(tagByte);
                    bb.AddRange(contentByte);
                    bb.AddRange(tagByte);
                    bb.AddRange(createTimeByte);

                    socket.Send(bb.ToArray());

                }
                catch (Exception e)
                {
                    msg.AppendFormat(e.ToString());
                    log(msg);
                }

                byte[] readData = new byte[100];
                try
                {
                    socket.Receive(readData);
                }
                catch (Exception e)
                {
                    msg.AppendFormat(e.ToString());
                    log(msg);
                }

                String receiveStr = Encoding.Default.GetString(readData);
                String[] datas = receiveStr.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
                if (datas.Length < 2)
                {
                    return null;
                }

                String pageType = datas[1];
                String receiveCode = datas[3];

                if (PackType.RECEIVE_BACK.Equals(pageType))
                {
                    sms.ReceiveCode = receiveCode;
                    if (datas[3].StartsWith(ReceiveCode.RECEIVE_SUCCESS))
                    {
                        sms.ReceiveId = datas[4];
                    }
                    else
                    {
                        string _receiveInfo = "";
                        ReceiveInfo.map.TryGetValue(receiveCode, out _receiveInfo);
                        sms.ReceiveInfo = _receiveInfo;
                    }
                }
                return receiveStr;

            }
            else
            { 
                // 请先登录
                msg.AppendFormat("请先登录");
                log(msg);
            }

            return null;
        }

        public SmsClient()
        {
            _ip = System.Configuration.ConfigurationManager.AppSettings["SMS_SOCKET_IP"];
            if (String.IsNullOrEmpty(_ip))
            {
                _ip = "116.228.55.215";
            }
            string str_port = System.Configuration.ConfigurationManager.AppSettings["SMS_SOCKET_PORT"];
            if (String.IsNullOrEmpty(str_port))
            {
                _port = 3000;
            }
            else
            {
                _port = int.Parse(str_port);
            }

            string str_userid = System.Configuration.ConfigurationManager.AppSettings["SMS_SOCKET_USERID"];
            if (String.IsNullOrEmpty(str_userid))
            {
                setUserId("TYJK");    //  TYJK
            }
            else
            {
                setUserId(str_userid);
            }

            string str_password = System.Configuration.ConfigurationManager.AppSettings["SMS_SOCKET_PASSWORD"];
            if (String.IsNullOrEmpty(str_password))
            {
                setPassWord("OMk8vLj4");  // TYJK
            }
            else
            {
                setPassWord(str_password);
            }
        }


        public void sendSingleSms(String sourceSPID, String sourcePhone, String targetPhone, String message)
        {
            getSocket();
            setUserId(_userId);
            setPassWord(_passWord);
            login();
            SmsMessage sms = new SmsMessage();
            sms.SequenceId = "1214";
            sms.Content = message;
            sms.TargetNumber = targetPhone;
            sms.SendNumber = sourcePhone;
            sms.CreateTime = sourceSPID;
            sms.SmsType = PackType.DATA_BACK;
            sendSingleSms(sms);
        }

        public void sendSingleSms(String sourcePhone, String targetPhone, String message) {

            getSocket();
            setUserId(_userId);
            setPassWord(_passWord);
            login();
            SmsMessage sms = new SmsMessage();
            sms.SequenceId = "1214";
            sms.Content = message;
            sms.TargetNumber = targetPhone;
            sms.SendNumber = sourcePhone;
            sms.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            sms.SmsType = PackType.DATA_BACK;
            sendSingleSms(sms);
        }


        public void sendSingleSms(SmsMessage sms)
        {
            StringBuilder msg = new StringBuilder();
            if (loginFlag)
            {
                String receiveStr = sendSms(sms);
                if (!String.IsNullOrEmpty(receiveStr) && !String.IsNullOrEmpty(sms.ReceiveId))
                {
                    // send ok 
                    msg.AppendFormat("msg send ok!\r\n");
                    msg.AppendFormat("receivecode:"+sms.ReceiveCode+"\r\n");
                    msg.AppendFormat("targetnumber:"+sms.TargetNumber+"\r\n");
                    msg.AppendFormat("time:"+sms.CreateTime+"\r\n");
                    msg.AppendFormat("content:"+sms.Content);
                    log(msg);
                }
                else
                { 
                    //对于发送失败额状态码异常处理
                    dealError(sms.ReceiveCode);
                }
            }
            destroy();
        }

        public void sendMultiSms(List<SmsMessage> messages)
        {
            if (loginFlag)
            { 
                foreach(SmsMessage sms in messages)
                {
                    String receiveStr = sendSms(sms);
                    if (!String.IsNullOrEmpty(receiveStr) && !String.IsNullOrEmpty(sms.ReceiveId))
                    {

                    }
                    else
                    {
                        dealError(sms.ReceiveCode);
                    }
                }
            }
            destroy();
        }


        private void dealError(String receiveCode)
        {
            StringBuilder msg = new StringBuilder();

            if (ReceiveInfo.map.ContainsKey(receiveCode))
            {
                msg.AppendFormat(receiveCode);
                log(msg);
                // 请处理异常
            }
            else
            {
                msg.AppendFormat("无法识别的错误。");
                log(msg);
                //无法识别的错误。
            }
            
        }

        public void log(StringBuilder content)
        {
            StringBuilder msg = new StringBuilder();
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
            msg.Append("短信发送" + DateTime.Now.ToString("u") + "\r\n");
            msg.Append(content);
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
            BTUCenterInterfaceLog.CenterForBizTourLog("SmsLog", msg);
        }

    }
}
