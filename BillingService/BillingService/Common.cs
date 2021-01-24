using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace BillingService
{
    public class Common
    {
        public static void GetCurrentDateTime(out int currentDate, out int currentTime)
        {
            DateTime currentDateTime = DateTime.Now;
            int.TryParse(currentDateTime.ToString("yyyyMMdd"), out currentDate);
            int.TryParse(currentDateTime.ToString("HHmmss"), out currentTime);
        }

        public static Int32 GetInt32(IDataReader dataReader, string fieldName)
        {
            return dataReader.GetInt32(dataReader.GetOrdinal(fieldName));
        }

        public static Int64 GetInt64(IDataReader dataReader, string fieldName)
        {
            return dataReader.GetInt64(dataReader.GetOrdinal(fieldName));
        }

        public static string GetString(IDataReader dataReader, string fieldName)
        {
            if (!DBNull.Value.Equals(dataReader[fieldName]))
                return dataReader.GetString(dataReader.GetOrdinal(fieldName));
            else
                return string.Empty;
        }

        public static decimal GetDecimal(IDataReader dataReader, string fieldName)
        {
            return dataReader.GetDecimal(dataReader.GetOrdinal(fieldName));
        }

        public static double GetDouble(IDataReader dataReader, string fieldName)
        {
            return dataReader.GetDouble(dataReader.GetOrdinal(fieldName));
        }

        public static void LogException(Exception ex)
        {
            DateTime currentDateTime = DateTime.Now;
            string[] message = {
                "Time: " + currentDateTime.ToString("yyyy-MM-ddTHH:mm:ssZ")
                ,"Error: " + ex.Message + ex.StackTrace
            };
            string logFileLocation = ConfigurationManager.AppSettings["LogFileLocation"].ToString().Trim();
            System.IO.File.WriteAllLines(logFileLocation + "/" + currentDateTime.ToString("yyyyMMddHHmmssfff") + ".log", message);
        }
    }
}