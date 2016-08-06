using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class BooksService
    {
        List<Books> blist = null;
        public List<Books> GetModelList()
        {
            blist = new List<Books>();
            Books b = null;
            IDataReader dr = SqlHelper.ExecuteReader("select top 20 Id, Title,ContentDescription from Books");
            while (dr.Read())
            {
                b = new Books();
                b.Id = Convert.ToInt32(dr["Id"]);
                b.Title = dr["Title"].ToString();
                b.ContentDescription = dr["ContentDescription"].ToString();
                blist.Add(b);
            }
            return blist;
        }
    }
}
