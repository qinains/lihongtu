using System;
using System.Collections.Generic;
using System.Text;

namespace Linkage.BestTone.Interface.Rule
{
    /// <summary>
    /// Author Lihongtu 2013.02.26
    /// </summary>
    public class PackType
    {
        	public static  String TEST_BACK = "0000";// 客户端发送的测试包
	        public static  String LOGIN_BACK = "0001";// Login回复包
	        public static  String DATA_BACK = "0002";// 客户端发送的数据包
	        public static  String RECEIVE_BACK = "0003";// 接收情况回复包
	        public static  String LOGIN_REQUEST_BACK = "0004";// Login请求包
	        public static  String OVERTIME_BACK = "0005";// 连接超时回复包
    }
}
