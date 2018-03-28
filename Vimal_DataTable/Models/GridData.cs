using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vimal_DataTable.Models
{
    public class GridData
    {
        public string TableName { get; set; }
        public string ColumnsName { get; set; }
        public string SortColumn { get; set; }
        public string SortOrder { get; set; }
        public int PageNumber { get; set; }
        public int RecordPerPage { get; set; }
        public string WhereClause { get; set; }
        public string JsonData { get; set; }
        public string ExportedColumns { get; set; }
        public string ExportedFileName { get; set; }
        public GridData(string type, string whereClause = "", Boolean IsExport = false)
        {
            if (type == "CountryMaster")
            {
                this.ColumnsName = "CountryId,CountryName,IsActive";
                this.PageNumber = 1;
                this.RecordPerPage = 10;
                this.SortColumn = "CountryId";
                this.SortOrder = "desc";
                this.TableName = "CountryMaster";
                this.WhereClause =" IsActive=1 "+ whereClause;
                this.ExportedFileName = "Country Information";
                this.ExportedColumns = "CountryId[Hidden],CountryName,IsActive[Hidden]";
                GridFunctions oGrid = new GridFunctions();
                if (!IsExport)
                    this.JsonData = oGrid.GetJson(this);
                else
                    oGrid.Export(this);
            }
            else if (type == "CurrencyMaster")
            {
                this.ColumnsName = "CurrencyId,CurrencyName,IsActive";
                this.PageNumber = 1;
                this.RecordPerPage = 10;
                this.SortColumn = "CurrencyId";
                this.SortOrder = "desc";
                this.TableName = "CurrencyMaster";
                this.WhereClause = whereClause;
                this.ExportedFileName = "CurrencyList";
                this.ExportedColumns = "CurrencyId[Hidden],CurrencyName,IsActive[Hidden]";
                GridFunctions oGrid = new GridFunctions();
                if (!IsExport)
                    this.JsonData = oGrid.GetJson(this);
                else
                    oGrid.Export(this);
            }         
        }
    }
}