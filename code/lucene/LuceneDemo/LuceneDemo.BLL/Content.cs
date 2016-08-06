using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuceneDemo.BLL
{
    public class Content
    {
        private static DAL.Content content;

        public Content()
        {
            content = new DAL.Content();
        }



        #region 增加一条数据
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(Model.Content model)
        {
            return content.Add(model);
        }
        #endregion

        #region 获取所有数据
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        public List<Model.Content> GetModels()
        {
            return content.GetModels();
        }
        #endregion


    }
}
