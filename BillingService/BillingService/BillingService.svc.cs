using BillingServiceDAL;
using Microsoft.Practices.EnterpriseLibrary.Data;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;

namespace BillingService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class BillingService : IBillingService
    {
        static BillingService()
        {
            DatabaseFactory.SetDatabaseProviderFactory(new DatabaseProviderFactory());
        }

        #region UserInfo

        public UserInfo GetUserInfo(string username, string password)
        {
            IDataReader dataReader = null;
            try
            {
                UserInfo userInfo = new UserInfo();

                Database db = DatabaseFactory.CreateDatabase("DbConnection");
                dataReader = BillingDAL.GetUserInfo(db, username, password);
                if (dataReader.Read())
                {
                    userInfo.UserID = Common.GetInt32(dataReader, "FUSERID");
                    userInfo.UserName = Common.GetString(dataReader, "FUSERNAME");
                    userInfo.FirstName = Common.GetString(dataReader, "FFIRSTNAME");
                    userInfo.LastName = Common.GetString(dataReader, "FLASTNAME");
                    userInfo.AccessLevel = (AccessLevel)Common.GetInt32(dataReader, "FACCESSLEVEL");
                }
                dataReader.Close();

                return userInfo;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                throw new WebFaultException<string>(ex.Message, HttpStatusCode.InternalServerError);
            }
            finally
            {
                if (dataReader != null && !dataReader.IsClosed)
                    dataReader.Close();
            }
        }

        public List<UserInfo> GetUsers()
        {
            IDataReader dataReader = null;
            try
            {
                List<UserInfo> userList = new List<UserInfo>();
                UserInfo userInfo = null;

                Database db = DatabaseFactory.CreateDatabase("DbConnection");
                dataReader = BillingDAL.GetUsers(db);
                while (dataReader.Read())
                {
                    userInfo = new UserInfo();
                    userInfo.UserID = Common.GetInt32(dataReader, "FUSERID");
                    userInfo.UserName = Common.GetString(dataReader, "FUSERNAME");
                    userInfo.FirstName = Common.GetString(dataReader, "FFIRSTNAME");
                    userInfo.LastName = Common.GetString(dataReader, "FLASTNAME");
                    userInfo.Password = Common.GetString(dataReader, "FPASSWORD");
                    userList.Add(userInfo);
                }
                dataReader.Close();

                return userList;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                throw new WebFaultException<string>(ex.Message, HttpStatusCode.InternalServerError);
            }
            finally
            {
                if (dataReader != null && !dataReader.IsClosed)
                    dataReader.Close();
            }
        }

        public int InsertOrUpdateUser(UserInfoFilter userInfo)
        {
            IDataReader dataReader = null;
            try
            {
                Database db = DatabaseFactory.CreateDatabase("DbConnection");

                int currentDate = 0;
                int currentTime = 0;
                Common.GetCurrentDateTime(out currentDate, out currentTime);

                dataReader = BillingDAL.CheckUserNameExists(db, userInfo.UserID, userInfo.UserName);
                if (dataReader.Read())
                {
                    dataReader.Close();
                    return -1; //username already exists
                }
                else
                {
                    dataReader.Close();
                    if (userInfo.UserID > 0)
                    {
                        BillingDAL.UpdateUser(db, userInfo.UserID, userInfo.Password, userInfo.FirstName, userInfo.LastName, userInfo.LoginUserID, currentDate, currentTime);
                        return userInfo.UserID;
                    }
                    else
                    {
                        return BillingDAL.InsertUser(db, userInfo.UserName, userInfo.Password, userInfo.FirstName, userInfo.LastName, userInfo.LoginUserID, currentDate, currentTime);
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                throw new WebFaultException<string>(ex.Message, HttpStatusCode.InternalServerError);
            }
            finally
            {
                if (dataReader != null && !dataReader.IsClosed)
                    dataReader.Close();
            }
        }

        public bool DeleteUser(UserFilter filter)
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase("DbConnection");

                int currentDate = 0;
                int currentTime = 0;
                Common.GetCurrentDateTime(out currentDate, out currentTime);

                BillingDAL.DeleteUser(db, filter.SelectedUserID, filter.UserID, currentDate, currentTime);
                return true;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                throw new WebFaultException<string>(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        #endregion

        #region ItemInfo

        public List<ItemInfo> GetItems()
        {
            IDataReader dataReader = null;
            try
            {
                List<ItemInfo> itemList = new List<ItemInfo>();
                ItemInfo itemInfo = null;

                Database db = DatabaseFactory.CreateDatabase("DbConnection");
                dataReader = BillingDAL.GetItems(db);
                while (dataReader.Read())
                {
                    itemInfo = new ItemInfo();
                    itemInfo.ItemID = Common.GetInt32(dataReader, "FITEMID");
                    itemInfo.ItemName = Common.GetString(dataReader, "FITEMNAME");
                    itemInfo.ItemDescription = Common.GetString(dataReader, "FITEMDESC");
                    itemInfo.Price = Common.GetDecimal(dataReader, "FPRICE");
                    itemList.Add(itemInfo);
                }
                dataReader.Close();

                return itemList;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                throw new WebFaultException<string>(ex.Message, HttpStatusCode.InternalServerError);
            }
            finally
            {
                if (dataReader != null && !dataReader.IsClosed)
                    dataReader.Close();
            }
        }

        public int InsertOrUpdateItem(ItemInfoFilter itemInfo)
        {
            IDataReader dataReader = null;
            try
            {
                Database db = DatabaseFactory.CreateDatabase("DbConnection");

                int currentDate = 0;
                int currentTime = 0;
                Common.GetCurrentDateTime(out currentDate, out currentTime);

                dataReader = BillingDAL.CheckItemNameExists(db, itemInfo.ItemID, itemInfo.ItemName);
                if (dataReader.Read())
                {
                    dataReader.Close();
                    return -1; //itemname already exists
                }
                else
                {
                    dataReader.Close();
                    if (itemInfo.ItemID > 0)
                    {
                        BillingDAL.UpdateItem(db, itemInfo.ItemID, itemInfo.ItemName, itemInfo.ItemDescription, itemInfo.Price, itemInfo.UserID, currentDate, currentTime);
                        return itemInfo.ItemID;
                    }
                    else
                    {
                        return BillingDAL.InsertItem(db, itemInfo.ItemName, itemInfo.ItemDescription, itemInfo.Price, itemInfo.UserID, currentDate, currentTime);
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                throw new WebFaultException<string>(ex.Message, HttpStatusCode.InternalServerError);
            }
            finally
            {
                if (dataReader != null && !dataReader.IsClosed)
                    dataReader.Close();
            }
        }

        public bool DeleteItem(ItemFilter filter)
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase("DbConnection");

                int currentDate = 0;
                int currentTime = 0;
                Common.GetCurrentDateTime(out currentDate, out currentTime);

                BillingDAL.DeleteItem(db, filter.ItemID, filter.UserID, currentDate, currentTime);
                return true;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                throw new WebFaultException<string>(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        #endregion

        #region BillingInfo

        public long InsertBillingInfo(BillingInfo billingInfo)
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase("DbConnection");

                int currentDate = 0;
                int currentTime = 0;
                Common.GetCurrentDateTime(out currentDate, out currentTime);

                using (DbConnection connection = db.CreateConnection())
                {
                    DbTransaction transaction = null;
                    try
                    {
                        connection.Open();
                        transaction = connection.BeginTransaction();

                        long billingID = BillingDAL.InsertBilling(db, transaction, billingInfo.Comments, billingInfo.TotalPrice, billingInfo.UserID, currentDate, currentTime);

                        foreach (BillingItemInfo itemInfo in billingInfo.BillingItemList)
                        {
                            BillingDAL.InsertBillingItem(db, transaction, billingID, itemInfo.ItemID, itemInfo.ItemPrice, itemInfo.Quantity);
                        }

                        transaction.Commit();
                        return billingID;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        if (connection != null && connection.State != ConnectionState.Closed)
                            connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                throw new WebFaultException<string>(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        public string DownloadBillingInfo(BillingExportFilter filter)
        {
            IDataReader dataReader = null;
            try
            {
                ExcelPackage excelPackage = new ExcelPackage();

                ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets.Add("Report Data");
                int rowIndex = 1;
                int colIndex = 1;

                workSheet.Cells[rowIndex, colIndex].Value = "Billing ID";
                colIndex++;
                workSheet.Cells[rowIndex, colIndex].Value = "Item Name";
                colIndex++;
                workSheet.Cells[rowIndex, colIndex].Value = "Item Price";
                colIndex++;
                workSheet.Cells[rowIndex, colIndex].Value = "Quantity";
                colIndex++;
                workSheet.Cells[rowIndex, colIndex].Value = "Amount";
                colIndex++;
                workSheet.Cells[rowIndex, colIndex].Value = "User";
                colIndex++;
                workSheet.Cells[rowIndex, colIndex].Value = "Created Time";
                colIndex++;
                workSheet.Cells[rowIndex, colIndex].Value = "Comments";

                rowIndex = 2;
                bool recordExists = false;
                DateTime createdDateTime;

                Database db = DatabaseFactory.CreateDatabase("DbConnection");
                dataReader = BillingDAL.GetBillingInfoForExport(db, filter.FromDate, filter.ToDate);
                while (dataReader.Read())
                {
                    colIndex = 1;
                    recordExists = true;

                    workSheet.Cells[rowIndex, colIndex].Value = Common.GetInt64(dataReader, "FBILLINGID");
                    colIndex++;
                    workSheet.Cells[rowIndex, colIndex].Value = Common.GetString(dataReader, "FITEMNAME");
                    colIndex++;

                    decimal itemPrice = Common.GetDecimal(dataReader, "FITEMPRICE");
                    int quantity = Common.GetInt32(dataReader, "FQTY");

                    workSheet.Cells[rowIndex, colIndex].Value = itemPrice;
                    colIndex++;
                    workSheet.Cells[rowIndex, colIndex].Value = quantity;
                    colIndex++;
                    workSheet.Cells[rowIndex, colIndex].Value = itemPrice * quantity;
                    colIndex++;
                    workSheet.Cells[rowIndex, colIndex].Value = Common.GetString(dataReader, "FUSERNAME");
                    colIndex++;

                    DateTime.TryParseExact(Common.GetInt32(dataReader, "FCREATEDDATE").ToString() + Common.GetInt32(dataReader, "FCREATEDTIME").ToString().PadLeft(6, '0'), "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out createdDateTime);
                    workSheet.Cells[rowIndex, colIndex].Value = createdDateTime.ToString("yyyy-MMM-dd hh:mm:ss tt");
                    colIndex++;
                    workSheet.Cells[rowIndex, colIndex].Value = Common.GetString(dataReader, "FCOMMENT");

                    rowIndex++;
                }
                dataReader.Close();

                if (recordExists)
                {
                    workSheet.Cells.AutoFitColumns();

                    string fileName = "BillingData_" + DateTime.Now.ToString("yyyyMMddHHmmss")+".xlsx";
                    string excelFolderPath = ConfigurationManager.AppSettings["LogFileLocation"].ToString();
                    FileInfo excelFile = new FileInfo(excelFolderPath + "/" + fileName);
                    excelPackage.SaveAs(excelFile);
                    return fileName;
                }

                return string.Empty;
            }
            catch(Exception ex)
            {
                Common.LogException(ex);
                throw new WebFaultException<string>(ex.Message, HttpStatusCode.InternalServerError);
            }
            finally
            {
                if (dataReader != null && !dataReader.IsClosed)
                    dataReader.Close();
            }
        }

        public ReportInfo GetReportData(BillingExportFilter filter)
        {
            IDataReader dataReader = null;
            try
            {
                ReportInfo repInfo = new ReportInfo();

                List<string> seriesNameList = new List<string>();
                List<string> xValueList = new List<string>();
                List<decimal> yValueList = new List<decimal>();

                //Get all the xaxis date values for the selected date type
                DateTime fromDateValue;
                DateTime.TryParseExact(filter.FromDate.ToString().Trim(), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out fromDateValue);
                DateTime toDateValue;
                DateTime.TryParseExact(filter.ToDate.ToString().Trim(), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out toDateValue);

                List<string> dateList = new List<string>();
                while (fromDateValue <= toDateValue)
                {
                    string formattedDate = fromDateValue.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
                    dateList.Add(formattedDate);
                    string formattedDateString = fromDateValue.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture);
                    xValueList.Add(formattedDateString);
                    fromDateValue = fromDateValue.AddDays(1);
                }

                Database db = DatabaseFactory.CreateDatabase("DbConnection");

                if(filter.IsDrillDown)
                    dataReader = BillingDAL.GetBillingItemPrice(db, filter.FromDate, filter.ToDate);
                else
                    dataReader = BillingDAL.GetTotalBillingPrice(db, filter.FromDate, filter.ToDate);

                List<ReportData> reportDataList = new List<ReportData>();
                ReportData reportData = null;

                while (dataReader.Read())
                {
                    reportData = new ReportData();
                    reportData.XValue = Common.GetInt32(dataReader, "FXVALUE").ToString();
                    reportData.YValue = Common.GetDecimal(dataReader, "FYVALUE");
                    reportData.SeriesName = Common.GetString(dataReader, "FSERIESNAME");
                    reportDataList.Add(reportData);

                    if (!seriesNameList.Contains(reportData.SeriesName))
                        seriesNameList.Add(reportData.SeriesName);
                }
                dataReader.Close();

                foreach (string seriesName in seriesNameList)
                {
                    yValueList = new List<decimal>();
                    foreach (string dateStr in dateList)
                    {
                        ReportData selReportData = reportDataList.FirstOrDefault(r => r.XValue == dateStr && r.SeriesName == seriesName);
                        if(selReportData != null)
                            yValueList.Add(selReportData.YValue);
                        else
                            yValueList.Add(0);
                    }
                    repInfo.YValues.Add(yValueList);
                }

                repInfo.XValues = xValueList;
                repInfo.SeriesNames = seriesNameList;

                return repInfo;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                throw new WebFaultException<string>(ex.Message, HttpStatusCode.InternalServerError);
            }
            finally
            {
                if (dataReader != null && !dataReader.IsClosed)
                    dataReader.Close();
            }
        }

        #endregion
    }
}
