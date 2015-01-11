//==============================================================================================================
//
// Class Name: DBUtility
// Description: �ṩ���ݿ��������
// Author: Է��
// Contact Email: yuanfeng@lianchuang.com
// Created Date: 2006-04-04
//
//==============================================================================================================
using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Linkage.BestTone.Interface.Utility
{

    public class DBUtility
    {
        /// <summary>
        /// BestToneCenter�����ַ���
        /// </summary>
        public static string BestToneCenterConStr
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["BestToneCenterConStr"];
                }
                catch
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// �÷�������ִ�и��¡���������践�����ݵĲ���
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static bool Execute(SqlCommand cmd, string connectionString)
        {
            bool isSuccess = false;
            SqlConnection conObj = null;

            try
            {
                conObj = new SqlConnection(connectionString);
                cmd.Connection = conObj;

                if (conObj.State != ConnectionState.Open)
                    conObj.Open();

                isSuccess = (cmd.ExecuteNonQuery() >= 0) ? true : false;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                if (conObj.State != ConnectionState.Closed)
                    conObj.Close();
            }

            return isSuccess;
        }

        /// <summary>
        /// �÷�������ִ�л�ȡ���ݼ��Ĳ���
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="resultTableName"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static DataSet FillData(SqlCommand cmd, string resultTableName, string connectionString)
        {
            DataSet tmpData = new DataSet();
            SqlConnection conObj = null;

            try
            {
                conObj = new SqlConnection(connectionString);
                cmd.Connection = conObj;
                if (conObj.State != ConnectionState.Open)
                    conObj.Open();
                SqlDataAdapter adapterObj = new SqlDataAdapter(cmd);
                adapterObj.Fill(tmpData, resultTableName);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                if (conObj.State != ConnectionState.Closed)
                    conObj.Close();
            }

            return tmpData;
        }

        /// <summary>
        /// �÷�������ִ�л�ȡ���ݼ��Ĳ���
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static DataSet FillData(SqlCommand cmd, string connectionString)
        {
            DataSet tmpData = new DataSet();
            SqlConnection conObj = null;

            try
            {
                conObj = new SqlConnection(connectionString);
                cmd.Connection = conObj;
                if (conObj.State != ConnectionState.Open)
                    conObj.Open();
                SqlDataAdapter adapterObj = new SqlDataAdapter(cmd);
                adapterObj.Fill(tmpData);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                if (conObj.State != ConnectionState.Closed)
                    conObj.Close();
            }

            return tmpData;
        }


     



    }// End Class
}


