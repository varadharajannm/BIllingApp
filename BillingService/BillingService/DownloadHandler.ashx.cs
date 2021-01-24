using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace BillingService
{
    /// <summary>
    /// Summary description for DownloadHandler
    /// </summary>
    public class DownloadHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string reportExcel = context.Request.QueryString["file"];
                if (reportExcel != null && reportExcel.Trim().Length > 0)
                {
                    string logFilePath = System.Configuration.ConfigurationManager.AppSettings["LogFileLocation"] + "/" + reportExcel.Trim();
                    if (logFilePath != null && logFilePath.Length > 4)
                    {
                        if (logFilePath.Contains(@"\"))
                        {
                            if (File.Exists(logFilePath))
                            {
                                FileInfo fileInfo = new FileInfo(logFilePath);
                                context.Response.Clear();
                                context.Response.ContentType = "application/octet-stream";
                                context.Response.AddHeader("Content-Disposition", "attachment; filename=" + reportExcel.Trim());
                                context.Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                                context.Response.TransmitFile(fileInfo.FullName);
                                context.Response.Flush();
                            }
                        }
                    }
                }

            }
            catch(Exception ex)
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write(ex.Message);
            }
            finally
            {
                context.Response.End();
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}