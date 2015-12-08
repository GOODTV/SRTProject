using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

/// <summary>
/// DBProvider 的摘要描述
/// </summary>
namespace Core.Utility
{
    public class DBProvider
    {
        SqlConnection conn = new SqlConnection();
        SqlCommand cmd = new SqlCommand();

        public DBProvider()
        {
            //
            // TODO: 在此加入建構函式的程式碼
            //
        }

        /// <summary>
        /// 開啟資料庫連線
        /// </summary>
        /// <history>
        /// 1.Tanya Wu, 2012/9/4, Create
        /// </history>
        public void OpenConnection()
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            conn.Open();
        }

        /// <summary>
        /// 關閉資料庫連線
        /// </summary>
        /// <history>
        /// 1.Tanya Wu, 2012/9/4, Create
        /// </history>
        public void CloseConnection()
        {
            conn.Close();
            conn.Dispose();
        }

        /// <summary>
        /// OpenDataTable
        /// </summary>
        /// <param name="strSQL">The SQL Command String.</param>
        /// <param name="cmd">SqlCommand Object.</param>
        /// <history>
        /// 1.Tanya Wu, 2012/9/6, Create
        /// </history>
        public DataTable OpenDataTable(string strSQL, SqlCommand cmd)
        {
            DataTable dt = new DataTable();

            //開啟資料庫連線
            this.OpenConnection();

            cmd.Connection = conn;
            cmd.CommandText = strSQL;
            
            SqlDataReader dr = cmd.ExecuteReader();
            dt.Load(dr);

            //關閉資料庫連線
            dr.Close();
            this.CloseConnection();

            return dt;
        }

        /// <summary>
        /// OpenDataTableNonParameter
        /// </summary>
        /// <param name="strSQL">The SQL Command String.</param>
        /// <history>
        /// 1.Tanya Wu, 2012/10/9, Create
        /// </history>
        public DataTable OpenDataTableNonParameter(string strSQL)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();

            //開啟資料庫連線
            this.OpenConnection();

            cmd.Connection = conn;
            cmd.CommandText = strSQL;

            SqlDataReader dr = cmd.ExecuteReader();
            dt.Load(dr);

            //關閉資料庫連線
            dr.Close();
            this.CloseConnection();

            return dt;
        }

        /// <summary>
        /// ExecuteNonQuery
        /// </summary>
        /// <param name="strSQL">The SQL Command String.</param>
        /// <param name="cmd">SqlCommand Object.</param>
        /// <history>
        /// 1.Tanya Wu, 2012/9/6, Create
        /// </history>
        public void ExecuteNonQuery(string strSQL, SqlCommand cmd)
        {
            //開啟資料庫連線
            this.OpenConnection();

            cmd.Connection = conn;
            cmd.CommandText = strSQL;

            cmd.ExecuteNonQuery();

            //關閉資料庫連線
            this.CloseConnection();
        }
    }
}