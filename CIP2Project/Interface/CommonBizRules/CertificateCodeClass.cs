using System;

using System.Text;

using System.Collections;

using System.Text.RegularExpressions;



namespace UserLib.World.Person
{

    /// <summary>

    /// ���֤��

    /// </summary>

    public class IDCard
    {

        /// <summary>

        /// ʡ������������

        /// </summary>

        private static string[] city =

         {

              null,null,null,null,null,null,null,null,null,null,null,

              "����","���","�ӱ�","ɽ��","���ɹ�",

              null,null,null,null,null,

              "����","����","������",

              null,null, null,null,null,null,null,

              "�Ϻ�","����","�㽭","��΢","����","����","ɽ��",

              null,null, null,

              "����","����","����","�㶫","����","����",

              null,null,null,

              "����","�Ĵ�","����","����","����",

              null,null,null,null,null,null,

              "����","����","�ຣ","����","�½�",

              null,null, null,null,null,

              "̨��",

              null,null,null,null,null,null,null,null,null,

              "���","����",

              null,null,null,null,null,null,null,null,

              "����"

         };



        /// <summary>

        /// ȡ��У����

        /// </summary>

        /// <param name="cid">���֤����</param>

        /// <returns>У����</returns>

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

        /// ���֤λתλ

        /// </summary>

        /// <param name="cid">15λ���֤����</param>

        /// <returns>18λ���֤����</returns>

        public static string CID15To18(string cid)
        {

            string rs = cid.Substring(0, 6) + "19" + cid.Substring(6);

            rs += GetCheckCode(cid);

            return rs;

        }



        /// <summary>

        /// ���֤��У��

        /// </summary>

        /// <param name="cid">���֤����</param>

        /// <returns>�Ƿ�ͨ��У��</returns>

        public static bool Validate(string cid)
        {

            ArrayList msg;

            bool rs = Validate(cid, out msg);

            return rs;

        }



        /// <summary>

        /// ���֤��У��

        /// </summary>

        /// <param name="cid">���֤����</param>

        /// <param name="msg">У��ʧ����Ϣ</param>

        /// <returns>�Ƿ�ͨ��У��</returns>

        public static bool Validate(string cid, out ArrayList msg)
        {

            msg = new ArrayList();



            //�жϸ�ʽ��ȷ��

            System.Text.RegularExpressions.Regex rg = new System.Text.RegularExpressions.Regex(@"^\d{17}(\d|x)$");

            System.Text.RegularExpressions.Match mc = rg.Match(cid);

            if (!mc.Success)
            {

                msg.Add("��ʽ�Ƿ�");

            }



            //�жϵ�����ȷ��

            if (city[int.Parse(cid.Substring(0, 2))] == null)
            {

                msg.Add("�����Ƿ�");

            }



            //�жϳ���������ȷ��

            try
            {

                DateTime.Parse(cid.Substring(6, 4) + "-" + cid.Substring(10, 2) + "-" + cid.Substring(12, 2));

            }

            catch
            {

                msg.Add("�������ڷǷ�");

            }



            //�ж�У������ȷ��

            if (cid.Substring(17, 1) != GetCheckCode(cid))
            {

                msg.Add("У����Ƿ�");

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

        /// ȡ�����֤��Ϣ

        /// </summary>

        /// <param name="cid">���֤����</param>

        /// <param name="province">ʡ��</param>

        /// <param name="birDate">��������</param>

        /// <param name="sex">�Ա�</param>

        /// <returns>�Ƿ�Ϊ��Ч�����֤����</returns>

        public static bool GetInfo(string cid, out string province, out DateTime birDate, out string sex)
        {

            province = "";

            birDate = DateTime.Parse("2000-01-01");

            sex = "";

            if (Validate(cid))
            {

                province = city[int.Parse(cid.Substring(0, 2))];

                birDate = DateTime.Parse(cid.Substring(6, 4) + "-" + cid.Substring(10, 2) + "-" + cid.Substring(12, 2));

                sex = int.Parse(cid.Substring(16, 1)) % 2 == 1 ? "��" : "Ů";

                return true;

            }

            else
            {

                return false;

            }

        }



    }

}
