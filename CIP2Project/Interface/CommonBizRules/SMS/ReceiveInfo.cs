using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
namespace Linkage.BestTone.Interface.Rule
{
    /// <summary>
    /// Author 2013.02.26
    /// </summary>
    public class ReceiveInfo
    {
        public static Dictionary<String,String> map = new Dictionary<string,string>();
        
        public ReceiveInfo()
        {
            map.Add(ReceiveCode.LOGIN_SUCCESS, "登录成功");
            map.Add(ReceiveCode.LAWLESS_USER_NAME, "非法用户名");
            map.Add(ReceiveCode.LAWLESS_USER_PASS, "密码错误");
            map.Add(ReceiveCode.RECEIVE_SUCCESS, "数据包正确，接收成功");
            map.Add(ReceiveCode.LAWLESS_FROMAT, "非法格式数据包");
            map.Add(ReceiveCode.LAWLESS_AIM_PHONE, "非法目标号码");
            map.Add(ReceiveCode.LINK_OVERTIME, "连接超时");
            map.Add(ReceiveCode.INFO_TOO_LONG, "信息内容长度太长");
            map.Add(ReceiveCode.LAWLESS_SEND_PHONE, "非法发送号码");
            map.Add(ReceiveCode.LAWLESS_SEND_TIME, "非法申请发送时间");
            map.Add(ReceiveCode.LAWLESS_MSG_TYPE, "非法短信业务类型");
            map.Add(ReceiveCode.USER_NOT_OPEN, "用户没有开通");
            map.Add(ReceiveCode.USER_NOT_LOGIN, "你还未登录或者会话已过期，请先登录，再发送数据包");
            map.Add(ReceiveCode.LAWLESS_LOGIN_FROMAT, "Login数据包格式错误");
            map.Add(ReceiveCode.TOO_MANY_REQUESTS, "请求次数太多");
            map.Add(ReceiveCode.SEND_LIMIT, "短信发送量超过服务器限定的最大值");
            map.Add(ReceiveCode.LAWLESS_LOGIN_IP, "非法的登录ip");
            map.Add(ReceiveCode.TOO_MANY_SAME_MESSAGE, "短时间内大量的相同信息和相同目标");
            map.Add(ReceiveCode.TOO_MANY_SMS, "流量溢出");

            map.Add(ReceiveCode.NO_SUPOORT_OPER, "不支持此类运行商");
            map.Add(ReceiveCode.NO_MORE_LESS, "未定义异常");


        }
    }
}
