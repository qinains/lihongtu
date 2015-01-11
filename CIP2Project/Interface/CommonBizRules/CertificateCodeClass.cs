using System;

using System.Text;

using System.Collections;

using System.Text.RegularExpressions;



namespace UserLib.World.Person
{

    /// <summary>

    /// 身份证类

    /// </summary>

    public class IDCard
    {

        /// <summary>

        /// 省市自治区数组

        /// </summary>

        private static string[] city =

         {

              null,null,null,null,null,null,null,null,null,null,null,

              "北京","天津","河北","山西","内蒙古",

              null,null,null,null,null,

              "辽宁","吉林","黑龙江",

              null,null, null,null,null,null,null,

              "上海","江苏","浙江","安微","福建","江西","山东",

              null,null, null,

              "河南","湖北","湖南","广东","广西","海南",

              null,null,null,

              "重庆","四川","贵州","云南","西藏",

              null,null,null,null,null,null,

              "陕西","甘肃","青海","宁夏","新疆",

              null,null, null,null,null,

              "台湾",

              null,null,null,null,null,null,null,null,null,

              "香港","澳门",

              null,null,null,null,null,null,null,null,

              "国外"

         };



        /// <summary>

        /// 取得校验码

        /// </summary>

        /// <param name="cid">身份证号码</param>

        /// <returns>校验码</returns>

        private static string GetCheckCode(string cid)
        {

            string[] check = { "1", "0", "X", "9", "8", "7", "6", "5", "4", "3", "2" };

            int[] weight = { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2, 1 };

            int rs = 0;

            for (int i = 0; i <= cid.Length - 1; i++)
            {

                rs += int.Parse(cid.Substring(i, 1)) * weight[i];

            }

            rs = rs % 11;

            return check[rs];

        }



        /// <summary>

        /// 身份证位转位

        /// </summary>

        /// <param name="cid">15位身份证号码</param>

        /// <returns>18位身份证号码</returns>

        public static string CID15To18(string cid)
        {

            string rs = cid.Substring(0, 6) + "19" + cid.Substring(6);

            rs += GetCheckCode(cid);

            return rs;

        }



        /// <summary>

        /// 身份证的校验

        /// </summary>

        /// <param name="cid">身份证号码</param>

        /// <returns>是否通过校验</returns>

        public static bool Validate(string cid)
        {

            ArrayList msg;

            bool rs = Validate(cid, out msg);

            return rs;

        }



        /// <summary>

        /// 身份证的校验

        /// </summary>

        /// <param name="cid">身份证号码</param>

        /// <param name="msg">校验失败信息</param>

        /// <returns>是否通过校验</returns>

        public static bool Validate(string cid, out ArrayList msg)
        {

            msg = new ArrayList();



            //判断格式正确性

            System.Text.RegularExpressions.Regex rg = new System.Text.RegularExpressions.Regex(@"^\d{17}(\d|x)$");

            System.Text.RegularExpressions.Match mc = rg.Match(cid);

            if (!mc.Success)
            {

                msg.Add("格式非法");

            }



            //判断地区正确性

            if (city[int.Parse(cid.Substring(0, 2))] == null)
            {

                msg.Add("地区非法");

            }



            //判断出生日期正确性

            try
            {

                DateTime.Parse(cid.Substring(6, 4) + "-" + cid.Substring(10, 2) + "-" + cid.Substring(12, 2));

            }

            catch
            {

                msg.Add("出生日期非法");

            }



            //判断校验码正确性

            if (cid.Substring(17, 1) != GetCheckCode(cid))
            {

                msg.Add("校验码非法");

            }



            if (msg.Count == 0)
            {

                return true;

            }

            else
            {

                return false;

            }

        }



        /// <summary>

        /// 取得身份证信息

        /// </summary>

        /// <param name="cid">身份证号码</param>

        /// <param name="province">省份</param>

        /// <param name="birDate">出生年月</param>

        /// <param name="sex">性别</param>

        /// <returns>是否为有效的身份证号码</returns>

        public static bool GetInfo(string cid, out string province, out DateTime birDate, out string sex)
        {

            province = "";

            birDate = DateTime.Parse("2000-01-01");

            sex = "";

            if (Validate(cid))
            {

                province = city[int.Parse(cid.Substring(0, 2))];

                birDate = DateTime.Parse(cid.Substring(6, 4) + "-" + cid.Substring(10, 2) + "-" + cid.Substring(12, 2));

                sex = int.Parse(cid.Substring(16, 1)) % 2 == 1 ? "男" : "女";

                return true;

            }

            else
            {

                return false;

            }

        }



    }

}
