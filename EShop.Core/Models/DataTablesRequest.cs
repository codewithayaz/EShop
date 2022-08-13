using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Core.Models
{
    public class DataTablesRequest
    {
        public int Draw { get; set; }

        public List<Column> Columns { get; set; }

        public List<Order> Order { get; set; }

        public int Start { get; set; }

        public int Length { get; set; }

        public Search Search { get; set; }
    }

    public class Column
    {
        public string Data { get; set; }

        public string Name { get; set; }

        public bool Searchable { get; set; }

        public bool Orderable { get; set; }

        public Search Search { get; set; }
    }

    public class Order
    {
        public int Column { get; set; }

        public string Dir { get; set; }
    }

    public class Search
    {
        public string Value { get; set; }

        public bool IsRegex { get; set; }
    }
}
