using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuceneDemo.DAL
{
    public class Content
    {
        #region 增加一条数据
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(Model.Content model)
        {
            StringBuilder sqlstr = new StringBuilder();
            sqlstr.Append("insert into MB_Broker(");
            sqlstr.Append("Title,Info,Author,CreateTime");
            sqlstr.Append(") values (");
            sqlstr.Append("@Title,@Info,@Author,@CreateTime");
            sqlstr.Append(") ");

            SqlParameter[] parameters = { 
			            new SqlParameter("@Title", model.Title),
                        new SqlParameter("@Info", model.Info),
                        new SqlParameter("@Author", model.Author),
                        new SqlParameter("@CreateTime", model.CreateTime) 
            	
            };
            int rows = SqlHelp.ExecuterNonQuery(sqlstr.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 获取所有数据
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        public List<Model.Content> GetModels()
        {
            StringBuilder sqlstr = new StringBuilder();
            sqlstr.Append("select * from Contents");
            List<Model.Content> models = new List<Model.Content>();
            DataTable table = SqlHelp.ExecuteDatatable(sqlstr.ToString());
            if (table.Rows.Count > 0)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    models.Add(DataRowToModel(table.Rows[i]));
                }
            }
            return models;
        } 
        #endregion

        #region 得到一个对象实体
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Model.Content DataRowToModel(DataRow row)
        {
            Model.Content model = new Model.Content();
            if (row != null)
            {
                if (row["ID"] != null && row["ID"].ToString() != "")
                {
                    model.ID = Convert.ToInt32(Guid.Parse(row["ID"].ToString()));
                }
                if (row["Title"] != null && row["Title"].ToString() != "")
                {
                    model.Title = row["Title"].ToString();
                }

                if (row["Info"] != null && row["Info"].ToString() != "")
                {
                    model.Info = row["Info"].ToString();
                }

                if (row["Author"] != null && row["Author"].ToString() != "")
                {
                    model.Author = row["Author"].ToString();
                }
                if (row["CreateTime"] != null && row["CreateTime"].ToString() != "")
                {
                    model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
                }
                if (row["IsDelete"] != null && row["IsDelete"].ToString() != "")
                {
                    if ((row["IsDelete"].ToString() == "1") || (row["IsDelete"].ToString().ToLower() == "true"))
                    {
                        model.IsDelete = true;
                    }
                    else
                    {
                        model.IsDelete = false;
                    }
                }

            }
            return model;
        }
        #endregion

    }
}
