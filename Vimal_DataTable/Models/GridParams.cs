using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vimal_DataTable.Models
{
    public class GridParams
    {
        public string TableName { get; set; }
        public string ColumnsName { get; set; }
        public string SortColumn { get; set; }
        public string SortOrder { get; set; }
        public int PageNumber { get; set; }
        public int RecordPerPage { get; set; }
        public string WhereClause { get; set; }
        public string ExportedColumns { get; set; }
        public string ExportedFileName { get; set; }

        public string GetData()
        {
            GridFunctions oGrid = new GridFunctions();
            return oGrid.GetJson(this);
        }
        public void ExportData()
        {
            GridFunctions oGrid = new GridFunctions();
            oGrid.Export(this);
        }
    }
}