using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuceneDemo.Model
{
    public class Content
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Info { get; set; }
        public string Author { get; set; }
        public DateTime CreateTime { get; set; }
        public bool IsDelete { get; set; }
    }
}
