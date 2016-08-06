using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuceneDemo.DAL
{
    public class SqlHelp
    { 
        private static readonly string connStr = ConfigurationManager.ConnectionStrings["SqlConnStr"].ConnectionString;

        #region ExecuterNonQuery、对数据库进行增、删、改操作
        /// <summary>
        /// ExecuterNonQuery、对数据库进行增、删、改操作
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static int ExecuterNonQuery(string sql, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {

                    // SqlTransaction tran = conn.BeginTransaction();//开始事务
                    try
                    {
                        conn.Open();
                        //cmd.Transaction = tran;
                        cmd.CommandText = sql;
                        cmd.Parameters.AddRange(parameters);
                        int rows = cmd.ExecuteNonQuery();
                        //tran.Commit();//提交事务
                        return rows;
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        //tran.Rollback();//事务回滚
                        conn.Close();
                        throw e;
                    }
                }
            }

        }
        #endregion

        #region ExecuterScacle查询一条数据
        /// <summary>
        /// ExecuterScacle查询一条数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static object ExecuterScacle(string sql, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    try
                    {
                        conn.Open();
                        cmd.CommandText = sql;
                        cmd.Parameters.AddRange(parameters);
                        object rows = cmd.ExecuteScalar();
                        return rows;
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        conn.Close();
                        throw e;
                    }
                }
            }
        }
        #endregion

        #region ExecuteDatatable查询数据
        /// <summary>
        /// ExecuteDatatable查询数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static DataTable ExecuteDatatable(string sql, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {

                    try
                    {
                        conn.Open();
                        cmd.CommandText = sql;
                        cmd.Parameters.AddRange(parameters);
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        adapter.Fill(ds);
                        return ds.Tables[0];
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        conn.Close();
                        throw e;
                    }
                }
            }
        }
        #endregion

        #region 返回ds
        /// <summary>
        /// 返回ds
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static DataSet ExecuteDataset(string sql, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {

                    try
                    {
                        conn.Open();
                        cmd.CommandText = sql;
                        cmd.Parameters.AddRange(parameters);
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        adapter.Fill(ds);
                        return ds;
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        conn.Close();
                        throw e;
                    }
                }
            }
        }
        #endregion
    }
}
