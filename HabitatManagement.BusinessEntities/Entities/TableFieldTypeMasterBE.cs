using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitatManagement.BusinessEntities
{
    public class TableFieldTypeMasterBE
    {
        public int Id { get; set; }
        public int Field { get; set; }
        public string ColumnName { get; set; }
        public int RowCount { get; set; }
        public int ColumnType { get; set; }
    }
}