
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vimal_DataTable.Models
{
    public class ColumnConfig
    {
        public GridParams gridParams = new GridParams();
        public ColumnConfig(string mode)
        {
            if (mode == "CountryMaster")
            {
                gridParams.ColumnsName = "CountryId,CountryName,IsActive";
                gridParams.PageNumber = 1;
                gridParams.RecordPerPage = 10;
                gridParams.SortColumn = "CountryId";
                gridParams.SortOrder = "desc";
                gridParams.TableName = "CountryMaster";
                gridParams.WhereClause = " IsActive=1 ";
                gridParams.ExportedFileName = "Country Information";
                gridParams.ExportedColumns = "CountryId[Hidden],CountryName,IsActive[Hidden]";

            }
            else if (mode == "CurrencyMaster")
            {
                gridParams.ColumnsName = "CurrencyId,CurrencyName,IsActive";
                gridParams.PageNumber = 1;
                gridParams.RecordPerPage = 10;
                gridParams.SortColumn = "CurrencyId";
                gridParams.SortOrder = "desc";
                gridParams.TableName = "CurrencyMaster";
                gridParams.WhereClause = "";
                gridParams.ExportedFileName = "CurrencyList";
                gridParams.ExportedColumns = "CurrencyId[Hidden],CurrencyName,IsActive[Hidden]";
            }
        }
    }
}