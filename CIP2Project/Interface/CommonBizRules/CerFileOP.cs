/*********************************************************************************************************
 *     ����: ֤���ļ�����
 * ����ƽ̨: Windows XP + Microsoft SQL Server 2005
 * ��������: C#
 * ��������: Microsoft Visual Studio.Net 2005
 *     ����: Է��
 * ��ϵ��ʽ: 
 *     ��˾: �����Ƽ�(�Ͼ�)�ɷ����޹�˾
 * ��������: 2009-07-31
 **********/

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using System.Data.SqlClient;

using Linkage.BestTone.Interface.Utility;
using Linkage.BestTone.Interface.BTException;

namespace Linkage.BestTone.Interface.Rule
{
    public class CerFileOP
    {
        /// <summary>
        /// ���ļ�����Byte����
        /// ���ߣ�Է��      ʱ�䣺2009-7-31
        /// �޸ģ�          ʱ�䣺
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public byte[] ReadFile(string fileName)
        {

            FileStream pFileStream = null;

            byte[] pReadByte = new byte[0];

            try
            {

                pFileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);

                BinaryReader r = new BinaryReader(pFileStream);

                r.BaseStream.Seek(0, SeekOrigin.Begin);    //���ļ�ָ�����õ��ļ���

                pReadByte = r.ReadBytes((int)r.BaseStream.Length);

                return pReadByte;

            }

            catch
            {

                return pReadByte;

            }

            finally
            {

                if (pFileStream != null)

                    pFileStream.Close();

            }

        }

        /// <summary>
        /// ��֤���ļ��������ݿ�
        /// ���ߣ�Է��      ʱ�䣺2009-7-31
        /// �޸ģ�          ʱ�䣺
        /// </summary>
        public int InsertCerFileByte(string SPID, string CerType, string FilePath, string UserName, string CerPassword, byte[] btCer, out string ErrMsg)
        {
            int Result = 0;
            ErrMsg = "";

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "dbo.up_BT_V3_Interface_InsertCerFile";

            try
            {
                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parUserName = new SqlParameter("@UserName", SqlDbType.VarChar, 100);
                parUserName.Value = UserName;
                cmd.Parameters.Add(parUserName);

                SqlParameter parCerPassword = new SqlParameter("@CerPassword", SqlDbType.VarChar, 100);
                parCerPassword.Value = CerPassword;
                cmd.Parameters.Add(parCerPassword);

                SqlParameter parbtCer = new SqlParameter("@btCer", SqlDbType.Image);
                parbtCer.Value = btCer;
                cmd.Parameters.Add(parbtCer);

                SqlParameter parFilePath = new SqlParameter("@FilePath", SqlDbType.VarChar,512);
                parFilePath.Value = FilePath;
                cmd.Parameters.Add(parFilePath);

                SqlParameter parCerType = new SqlParameter("@CerType", SqlDbType.Int);
                parCerType.Value = CerType;
                cmd.Parameters.Add(parCerType);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);
            }
            catch (Exception e)
            {
                Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = e.Message;
            }

            return Result;

        }
    }
}
