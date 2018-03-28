using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


namespace Vimal_DataTable.Models
{

    public class GridFunctions
    {
        public string GetColumns()
        {
            string columns = "";
            for (int i = 0; ; i++)
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["columns[" + i + "][data]"]))
                {
                    string c = Convert.ToString((HttpContext.Current.Request.Form["columns[" + i + "][data]"]));
                    columns += columns == "" ? c : "," + c;
                }
                else
                    break;
            }
            if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["Columns"]))
                columns = Convert.ToString(HttpContext.Current.Request.Form["Columns"]);
            return columns;
        }

        public string GetSortColumn(string defaultColName)
        {
            if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["order[0][column]"]))
            {
                string index = Convert.ToString(HttpContext.Current.Request.Form["order[0][column]"]);
                string ColName = Convert.ToString(HttpContext.Current.Request.Form["columns[" + index + "][data]"]);
                if (string.IsNullOrEmpty(ColName))
                    ColName = defaultColName;
                return ColName;
            }
            else
                return "";
        }

        public string GetSortOrder()
        {
            if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["order[0][dir]"]))
            {
                string order = Convert.ToString(HttpContext.Current.Request.Form["order[0][dir]"]);
                if (string.IsNullOrEmpty(order))
                    order = "asc";
                return order;
            }
            else
                return "asc";
        }

        public string GetWhereClause(string w = "")
        {
            string where = w;
            if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["FixClause"]))
            {
                string fix = Convert.ToString(HttpContext.Current.Request.Form["FixClause"]);
                if (fix != "")
                    where += where == "" ? fix : " AND " + fix;
            }
            if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["search[value]"]))
            {
                string val = Convert.ToString(HttpContext.Current.Request.Form["search[value]"]);
                if (val != "")
                {
                    string whereforall = "";
                    // where = where == "" ? " 1 = 1 " : where;
                    string[] columns = GetColumns().Split(',');
                    foreach (string col in columns)
                    {
                        if (col.ToLower() != "rownumber")
                            whereforall += whereforall == "" ? col + " LIKE N'%" + val + "%'" : " OR " + col + " LIKE N'%" + val + "%'";
                    }
                    where += where == "" ? "(" + whereforall + ")" : " AND (" + whereforall + ")";
                }
            }
            return where;
        }

        public int GetPageNumber()
        {
            if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["start"]))
                return Convert.ToInt32(Convert.ToString(HttpContext.Current.Request.Form["start"]));
            else
                return 1;
        }

        public int GetRecordPerPage()
        {
            if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["length"]))
                return Convert.ToInt32(Convert.ToString(HttpContext.Current.Request.Form["length"]));
            else
                return 10;
        }

        public DataTable GetDataTable(GridData oGrid)
        {
            //GridData og = new GridData(mode);
            SqlParameter[] para = new SqlParameter[7];
            para[0] = new SqlParameter().CreateParameter("@TableName", oGrid.TableName, -1);
            para[1] = new SqlParameter().CreateParameter("@ColumnsName", oGrid.ColumnsName, -1);
            para[2] = new SqlParameter().CreateParameter("@SortOrder", GetSortOrder());
            para[3] = new SqlParameter().CreateParameter("@SortColumn", GetSortColumn(oGrid.SortColumn));
            para[4] = new SqlParameter().CreateParameter("@PageNumber", GetPageNumber());
            para[5] = new SqlParameter().CreateParameter("@RecordPerPage", GetRecordPerPage());
            para[6] = new SqlParameter().CreateParameter("@WhereClause", GetWhereClause(oGrid.WhereClause), -1);
            DataTable dt = new dalc().GetDataTable("GetDataForGridWeb", para);
            return dt;
        }

        public string GetJson(GridData oGrid)
        {
            DataTable dt = GetDataTable(oGrid);
            return dt.GetJsonForDataTableJS();
        }

        public void Export(GridData oGrid)
        {
            DataTable dt = GetDataTable(oGrid);
            oGrid.ExportedColumns = Convert.ToString(HttpContext.Current.Request.Form["Columns"]).Replace("null,", "");
            if (oGrid != null)
            {
                string[] c = oGrid.ExportedColumns.Split(',');
                string[] s = oGrid.ExportedColumns.Split(',');
                for (int i = 0; i < c.Length; i++)
                {
                    c[i] = c[i].Split(' ')[0].ToString();
                }

                DataTable dtTemp = dt.Copy();
                int j = 0;
                foreach (DataColumn dc in dtTemp.Columns)
                {
                    if (!c.Contains(dc.ColumnName))
                        dt.Columns.Remove(dc.ColumnName);
                    else
                    {
                        dt.Columns[s[j].Split(' ')[0].ToString()].SetOrdinal(j);
                        dt.Columns[s[j].Split(' ')[0].ToString()].ColumnName = s[j].Split('[').Length > 1 ? s[j].Split('[')[1].Replace("]", "").ToString() : s[j];
                        j++;
                    }
                }

            }
            if (dt.Columns.Contains("TotalRows"))
                dt.Columns.Remove("TotalRows");
            // if (dt.Columns.Contains("RowNumber"))
            //dt.TableName = dt.TableName +  "SR#";
            //dt.Columns["RowNumber"].ColumnName = oGrid.TableName +  "SR#";
            //dt.Columns.Remove("RowNumber");
            string type = Convert.ToString(HttpContext.Current.Request.Form["type"]);
            if (type.ToLower() == "excel")
                dt.ExportToExcel(oGrid.ExportedFileName);
            else if (type.ToLower() == "pdf")
                dt.ExportToPdf(oGrid.ExportedFileName);
            else if (type.ToLower() == "word")
                dt.ExportToWord(oGrid.ExportedFileName);
            else
                dt.ExportToExcel(oGrid.ExportedFileName);
        }
    }

    public class Gdata<T>
    {
        public List<T> data { get; set; }
        public string draw { get; set; }
        public string recordsTotal { get; set; }
        public string recordsFiltered { get; set; }
    }
}