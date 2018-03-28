using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Vimal_DataTable.Models
{
    public static class Common
    {
        public static SqlParameter CreateParameter(this SqlParameter para, string paraName, string paraVal, int size = 50, ParameterDirection dir = ParameterDirection.Input)
        {
            para.ParameterName = paraName;
            para.Value = paraVal;
            para.Size = size;
            para.SqlDbType = System.Data.SqlDbType.NVarChar;
            para.Direction = dir;
            return para;
        }
        public static SqlParameter CreateParameter(this SqlParameter para, string paraName, int paraVal, ParameterDirection dir = ParameterDirection.Input)
        {
            para.ParameterName = paraName;
            para.Value = paraVal;
            para.SqlDbType = System.Data.SqlDbType.Int;
            para.Direction = dir;
            return para;
        }
        public static SqlParameter CreateParameter(this SqlParameter para, string paraName, decimal paraVal, ParameterDirection dir = ParameterDirection.Input)
        {
            para.ParameterName = paraName;
            para.Value = paraVal;
            para.SqlDbType = System.Data.SqlDbType.Decimal;
            para.Direction = dir;
            return para;
        }
        public static SqlParameter CreateParameter(this SqlParameter para, string paraName, float paraVal, ParameterDirection dir = ParameterDirection.Input)
        {
            para.ParameterName = paraName;
            para.Value = paraVal;
            para.SqlDbType = System.Data.SqlDbType.Float;
            para.Direction = dir;
            return para;
        }
        public static SqlParameter CreateParameter(this SqlParameter para, string paraName, DateTime paraVal, ParameterDirection dir = ParameterDirection.Input)
        {
            para.IsNullable = true;
            para.ParameterName = paraName;
            para.Value = paraVal;
            para.SqlDbType = System.Data.SqlDbType.DateTime;
            para.Direction = dir;
            return para;
        }
        public static SqlParameter CreateParameter(this SqlParameter para, string paraName, System.Data.DataTable paraVal, ParameterDirection dir = ParameterDirection.Input)
        {
            para.ParameterName = paraName;
            para.Value = paraVal;
            para.SqlDbType = System.Data.SqlDbType.Structured;
            para.Direction = dir;
            return para;
        }

        public static string ConvertToJSON(this DataTable table, Boolean IsSkipTotalRow = true)
        {
            var list = new List<Dictionary<string, object>>();

            foreach (DataRow row in table.Rows)
            {
                var dict = new Dictionary<string, object>();

                foreach (DataColumn col in table.Columns)
                {
                    if (IsSkipTotalRow && col.ColumnName.ToLower() != "totalrows")
                        dict[col.ColumnName] = row[col];
                }
                list.Add(dict);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            return serializer.Serialize(list);
        }

        public static string GetJsonForDataTableJS(this DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            string data = dt.ConvertToJSON();
            sb.AppendLine("{\"data\":" + data);
            sb.Append(",\"draw\":\"" + Convert.ToString(HttpContext.Current.Request.Form["draw"]) + "\"");
            sb.Append(",\"recordsFiltered\":\"" + (dt.Rows.Count == 0 ? "0" : dt.Rows[0]["TotalRows"].ToString()) + "\"");
            sb.Append(",\"recordsTotal\":\"" + (dt.Rows.Count == 0 ? "0" : dt.Rows[0]["TotalRows"].ToString()) + "\"}");
            return sb.ToString();
        }

        public static void ExportToExcel(this DataTable dt, string FileName)
        {
            GridView GridView1 = new GridView();
            GridView1.AllowPaging = false;
            GridView1.DataSource = dt;
            GridView1.DataBind();
            GridView1.HeaderRow.BackColor = System.Drawing.Color.Blue;
            GridView1.HeaderRow.ForeColor = System.Drawing.Color.White;
            GridView1.HeaderRow.Font.Bold = true;
            //GridView1.Style.Add("word-wrap", "break-word");
            //GridView1.Style.Add("width", "100");
            if (string.IsNullOrEmpty(FileName))
                FileName = "Excel";
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.AddHeader("content-disposition",
             "attachment;filename=" + FileName + ".xls");
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.Unicode;
            HttpContext.Current.Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
            HttpContext.Current.Response.Charset = "";
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            //TextBox txtAddress = new TextBox();
            //txtAddress.ReadOnly = false;
            //txtAddress.Style.Add("width", "99%");
            foreach (GridViewRow row in GridView1.Rows)
            {
                foreach (TableCell cell in row.Cells)
                {
                    cell.Style.Add("class", "textmode");
                    //txtAddress.Style = "width:100%;";
                }
            }
            //GridView1.Attributes.Add("style", "table-layout:fixed");
            //for (int i = 0; i < GridView1.Rows.Count; i++)
            //{
            //    //Apply text style to each Row
            //    GridView1.Rows[i].Attributes.Add("class", "textmode");
            //}
            GridView1.RenderControl(hw);
            string style = @"<style> .textmode { mso-number-format:\@; } </style>";
            HttpContext.Current.Response.Output.Write("<meta http-equiv ='Content-Type' content='text/html;charset=utf-8'>");// add this line to fix characterset in arabic
            HttpContext.Current.Response.Write(style);
            HttpContext.Current.Response.Output.Write(sw.ToString());
            //string style = @"<!--mce:2-->"; 
            //HttpContext.Current.Response.Write(style);
            //Open a memory stream that you can use to write back to the response
            //byte[] byteArray = Encoding.ASCII.GetBytes(sw.ToString());
            //MemoryStream s = new MemoryStream(byteArray);
            //StreamReader sr = new StreamReader(s, Encoding.ASCII);
            //HttpContext.Current.Response.Write(sr.ReadToEnd());
            HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
            HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        public static void ExportToWord(this DataTable dt, string FileName)
        {
            GridView GridView1 = new GridView();
            GridView1.AllowPaging = false;
            GridView1.DataSource = dt;
            GridView1.DataBind();
            GridView1.HeaderRow.BackColor = System.Drawing.Color.Blue;
            GridView1.HeaderRow.ForeColor = System.Drawing.Color.White;
            GridView1.HeaderRow.Font.Bold = true;

            if (string.IsNullOrEmpty(FileName))
                FileName = "Word";
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.AddHeader("content-disposition",
                "attachment;filename=" + FileName + ".doc");
            HttpContext.Current.Response.Charset = "";
            HttpContext.Current.Response.ContentType = "application/vnd.ms-word ";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            GridView1.RenderControl(hw);
            HttpContext.Current.Response.Output.Write(sw.ToString());
            HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
            HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        public static void ExportToPdf(this DataTable dt, string FileName)
        {
            GridView GridView1 = new GridView();
            GridView1.AllowPaging = false;
            //GridView1.HeaderStyle.BackColor = System.Drawing.Color.Black;
            //GridView1.HeaderStyle.ForeColor = System.Drawing.Color.White;
            GridView1.HeaderStyle.Font.Bold = true;
            GridView1.HeaderStyle.Font.Size = 12;
            GridView1.Font.Size = 10;
            GridView1.DataSource = dt;
            GridView1.DataBind();
            GridView1.HeaderRow.BackColor = System.Drawing.Color.Blue;
            GridView1.HeaderRow.ForeColor = System.Drawing.Color.White;
            GridView1.HeaderRow.Font.Bold = true;
            GridView1.Style.Add("width", "100");
            if (string.IsNullOrEmpty(FileName))
                FileName = "Pdf";
            HttpContext.Current.Response.ContentType = "application/pdf";
            HttpContext.Current.Response.AddHeader("content-disposition",
                "attachment;filename=" + FileName + ".pdf");
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            GridView1.RenderControl(hw);
            StringReader sr = new StringReader(sw.ToString());
            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter.GetInstance(pdfDoc, HttpContext.Current.Response.OutputStream);
            pdfDoc.Open();
            htmlparser.Parse(sr);
            pdfDoc.Close();
            HttpContext.Current.Response.Write(pdfDoc);
            HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
            HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        public static void ExportToCsv(this DataTable dt, string FileName)
        {
            if (string.IsNullOrEmpty(FileName))
                FileName = "csv";
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.AddHeader("content-disposition",
                "attachment;filename=" + FileName + ".csv");
            HttpContext.Current.Response.Charset = "";
            HttpContext.Current.Response.ContentType = "application/text";


            StringBuilder sb = new StringBuilder();
            for (int k = 0; k < dt.Columns.Count; k++)
            {
                //add separator
                sb.Append(dt.Columns[k].ColumnName + ',');
            }
            //append new line
            sb.Append("\r\n");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int k = 0; k < dt.Columns.Count; k++)
                {
                    //add separator
                    sb.Append(dt.Rows[i][k].ToString().Replace(",", ";") + ',');
                }
                //append new line
                sb.Append("\r\n");
            }
            HttpContext.Current.Response.Output.Write(sb.ToString());
            HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
            HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        public static void SetLog(this Exception ex, string msg, Boolean IsRedirect = true)
        {
            
            string FileName = DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year;
            if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/Logs")))
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Logs"));
            if (!File.Exists(HttpContext.Current.Server.MapPath("~/Logs/" + FileName + ".txt")))
            {
                using (StreamWriter sw = File.CreateText(HttpContext.Current.Server.MapPath("~/Logs/" + FileName + ".txt")))
                {
                    // StreamWriter sw = new StreamWriter(HttpContext.Current.Server.MapPath("~/Logs/" + FileName + ".txt"), true);
                    sw.WriteLine("Error on " + DateTime.Now + " ,Exception Message:" + ex.Message + ",Inner Message:" + ex.InnerException + ",Line:" + ex.StackTrace + ",Additional Msg:" + msg);
                    sw.Flush();
                    sw.Close();
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(HttpContext.Current.Server.MapPath("~/Logs/" + FileName + ".txt")))
                {
                    sw.WriteLine("Error on " + DateTime.Now + " ,Exception Message:" + ex.Message + ",Inner Message:" + ex.InnerException + ",Line:" + ex.StackTrace + ",Additional Msg: " + msg);
                    sw.Flush();
                    sw.Close();
                }
            }
            if (IsRedirect)
                throw ex;
        }

    }
}