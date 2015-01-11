using System;
using System.Collections.Generic;
using System.Text;

namespace Linkage.BestTone.Interface.Rule
{
    /// <summary>
    /// Author 2013.02.26
    /// </summary>
    public class ReceiveCode
    {
	    public static  String LOGIN_SUCCESS = "000";// 登陆成功
	    public static  String LAWLESS_USER_NAME = "002";// 非法用户名
	    public static  String LAWLESS_USER_PASS = "003";// 密码错误
	    public static  String RECEIVE_SUCCESS = "001";// 数据包正确，接收成功
	    public static  String LAWLESS_FROMAT = "004";// 非法格式数据包
	    public static  String LAWLESS_AIM_PHONE = "005";// 非法目标号码
	    public static  String LINK_OVERTIME = "006";// 连接超时
	    public static  String INFO_TOO_LONG = "007";// 信息内容长度太长
	    public static  String LAWLESS_SEND_PHONE = "008";// 非法发送号码
	    public static  String LAWLESS_SEND_TIME = "009";// 非法申请发送时间
	    public static  String LAWLESS_MSG_TYPE = "010";// 非法短信业务类型
	    public static  String USER_NOT_OPEN = "011";// 用户没有开通
	    public static  String USER_NOT_LOGIN = "012";// 你还未登录或者会话已过期，请先登录，再发送数据包
	    public static  String LAWLESS_LOGIN_FROMAT = "013";// Login数据包格式错误
	    public static  String TOO_MANY_REQUESTS = "014";// 请求次数太多
	    public static  String SEND_LIMIT = "015";// 短信发送量超过服务器限定的最大值
	    public static  String LAWLESS_LOGIN_IP = "016"; // 非法的登录ip
	    public static  String TOO_MANY_SAME_MESSAGE = "017"; // 短时间内大量的相同信息和相同目标
	    public static  String TOO_MANY_SMS = "018"; // 流量溢出
	    public static  String NO_SUPOORT_OPER = "019"; // 不支持此类运行商
    	
	    public static  String NO_MORE_LESS = "099"; // 未定义异常,客户端太旧，异常码不足
    }
}
