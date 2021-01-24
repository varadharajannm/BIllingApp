using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace BillingServiceDAL
{
    public class BillingDAL
    {
        #region UserInfo

        public static IDataReader GetUserInfo(Database db, string username, string password)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" SELECT FUSERID,FUSERNAME,FFIRSTNAME,FLASTNAME,FACCESSLEVEL FROM USERINFO WHERE FSTATUS='A' AND UPPER(FUSERNAME)=UPPER(@USERNAME) AND FPASSWORD=@PASSWORD ");

            DbCommand dbCmd = db.GetSqlStringCommand(sqlBuilder.ToString());
            dbCmd.CommandType = CommandType.Text;

            db.AddInParameter(dbCmd, "@USERNAME", DbType.String, username);
            db.AddInParameter(dbCmd, "@PASSWORD", DbType.String, password);

            return db.ExecuteReader(dbCmd);
        }

        public static IDataReader GetUsers(Database db)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" SELECT FUSERID,FUSERNAME,FPASSWORD,FFIRSTNAME,FLASTNAME FROM USERINFO WHERE FSTATUS='A' AND FACCESSLEVEL=2 ");

            DbCommand dbCmd = db.GetSqlStringCommand(sqlBuilder.ToString());
            dbCmd.CommandType = CommandType.Text;

            return db.ExecuteReader(dbCmd);
        }

        public static int InsertUser(Database db, string username, string password, string firstname, string lastname, int createdBy, int createdDate, int createdTime)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" INSERT INTO USERINFO(FUSERNAME,FPASSWORD,FFIRSTNAME,FLASTNAME,FCREATEDBY,FCREATEDDATE,FCREATEDTIME) ");
            sqlBuilder.Append(" VALUES(@USERNAME,@PASSWORD,@FIRSTNAME,@LASTNAME,@CREATEDBY,@CREATEDDATE,@CREATEDTIME); ");
            sqlBuilder.Append(" SELECT LAST_INSERT_ID() ");

            DbCommand dbCmd = db.GetSqlStringCommand(sqlBuilder.ToString());
            dbCmd.CommandType = CommandType.Text;

            db.AddInParameter(dbCmd, "@USERNAME", DbType.String, username);
            db.AddInParameter(dbCmd, "@PASSWORD", DbType.String, password);
            db.AddInParameter(dbCmd, "@FIRSTNAME", DbType.String, firstname);
            db.AddInParameter(dbCmd, "@LASTNAME", DbType.String, lastname);
            db.AddInParameter(dbCmd, "@CREATEDBY", DbType.Int32, createdBy);
            db.AddInParameter(dbCmd, "@CREATEDDATE", DbType.Int32, createdDate);
            db.AddInParameter(dbCmd, "@CREATEDTIME", DbType.Int32, createdTime);

            return Convert.ToInt32(db.ExecuteScalar(dbCmd));
        }

        public static int UpdateUser(Database db, int userID, string password, string firstname, string lastname, int updatedBy, int updatedDate, int updatedTime)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" UPDATE USERINFO SET FPASSWORD=@PASSWORD,FFIRSTNAME=@FIRSTNAME,FLASTNAME=@LASTNAME,FUPDATEDBY=@UPDATEDBY,FUPDATEDDATE=@UPDATEDDATE,FUPDATEDTIME=@UPDATEDTIME WHERE FUSERID=@USERID ");

            DbCommand dbCmd = db.GetSqlStringCommand(sqlBuilder.ToString());
            dbCmd.CommandType = CommandType.Text;

            db.AddInParameter(dbCmd, "@USERID", DbType.Int32, userID);
            db.AddInParameter(dbCmd, "@PASSWORD", DbType.String, password);
            db.AddInParameter(dbCmd, "@FIRSTNAME", DbType.String, firstname);
            db.AddInParameter(dbCmd, "@LASTNAME", DbType.String, lastname);
            db.AddInParameter(dbCmd, "@UPDATEDBY", DbType.Int32, updatedBy);
            db.AddInParameter(dbCmd, "@UPDATEDDATE", DbType.Int32, updatedDate);
            db.AddInParameter(dbCmd, "@UPDATEDTIME", DbType.Int32, updatedTime);

            return db.ExecuteNonQuery(dbCmd);
        }

        public static IDataReader CheckUserNameExists(Database db, int userID, string username)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" SELECT FUSERID FROM USERINFO WHERE FUSERID<>@USERID AND UPPER(FUSERNAME)=UPPER(@USERNAME) AND FSTATUS='A' ");

            DbCommand dbCmd = db.GetSqlStringCommand(sqlBuilder.ToString());
            dbCmd.CommandType = CommandType.Text;

            db.AddInParameter(dbCmd, "@USERID", DbType.Int32, userID);
            db.AddInParameter(dbCmd, "@USERNAME", DbType.String, username);

            return db.ExecuteReader(dbCmd);
        }

        public static bool DeleteUser(Database db, int userID, int updatedBy, int updatedDate, int updatedTime)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" UPDATE USERINFO SET FSTATUS='I',FUPDATEDBY=@UPDATEDBY,FUPDATEDDATE=@UPDATEDDATE,FUPDATEDTIME=@UPDATEDTIME WHERE FUSERID=@USERID ");

            DbCommand dbCmd = db.GetSqlStringCommand(sqlBuilder.ToString());
            dbCmd.CommandType = CommandType.Text;

            db.AddInParameter(dbCmd, "@USERID", DbType.Int32, userID);
            db.AddInParameter(dbCmd, "@UPDATEDBY", DbType.Int32, updatedBy);
            db.AddInParameter(dbCmd, "@UPDATEDDATE", DbType.Int32, updatedDate);
            db.AddInParameter(dbCmd, "@UPDATEDTIME", DbType.Int32, updatedTime);

            db.ExecuteNonQuery(dbCmd);
            return true;
        }

        #endregion

        #region ItemInfo

        public static IDataReader GetItems(Database db)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" SELECT FITEMID,FITEMNAME,FITEMDESC,FPRICE FROM ITEMINFO WHERE FSTATUS='A' ");

            DbCommand dbCmd = db.GetSqlStringCommand(sqlBuilder.ToString());
            dbCmd.CommandType = CommandType.Text;

            return db.ExecuteReader(dbCmd);
        }

        public static int InsertItem(Database db, string name, string desc, decimal price, int createdBy, int createdDate, int createdTime)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" INSERT INTO ITEMINFO(FITEMNAME,FITEMDESC,FPRICE,FCREATEDBY,FCREATEDDATE,FCREATEDTIME) ");
            sqlBuilder.Append(" VALUES(@ITEMNAME,@ITEMDESC,@PRICE,@CREATEDBY,@CREATEDDATE,@CREATEDTIME); ");
            sqlBuilder.Append(" SELECT LAST_INSERT_ID() ");

            DbCommand dbCmd = db.GetSqlStringCommand(sqlBuilder.ToString());
            dbCmd.CommandType = CommandType.Text;

            db.AddInParameter(dbCmd, "@ITEMNAME", DbType.String, name);
            db.AddInParameter(dbCmd, "@ITEMDESC", DbType.String, desc);
            db.AddInParameter(dbCmd, "@PRICE", DbType.Decimal, price);
            db.AddInParameter(dbCmd, "@CREATEDBY", DbType.Int32, createdBy);
            db.AddInParameter(dbCmd, "@CREATEDDATE", DbType.Int32, createdDate);
            db.AddInParameter(dbCmd, "@CREATEDTIME", DbType.Int32, createdTime);

            return Convert.ToInt32(db.ExecuteScalar(dbCmd));
        }

        public static int UpdateItem(Database db, int itemID, string name, string desc, decimal price, int updatedBy, int updatedDate, int updatedTime)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" UPDATE ITEMINFO SET FITEMNAME=@ITEMNAME,FITEMDESC=@ITEMDESC,FPRICE=@PRICE,FUPDATEDBY=@UPDATEDBY,FUPDATEDDATE=@UPDATEDDATE,FUPDATEDTIME=@UPDATEDTIME WHERE FITEMID=@ITEMID ");

            DbCommand dbCmd = db.GetSqlStringCommand(sqlBuilder.ToString());
            dbCmd.CommandType = CommandType.Text;

            db.AddInParameter(dbCmd, "@ITEMID", DbType.Int32, itemID);
            db.AddInParameter(dbCmd, "@ITEMNAME", DbType.String, name);
            db.AddInParameter(dbCmd, "@ITEMDESC", DbType.String, desc);
            db.AddInParameter(dbCmd, "@PRICE", DbType.Decimal, price);
            db.AddInParameter(dbCmd, "@UPDATEDBY", DbType.Int32, updatedBy);
            db.AddInParameter(dbCmd, "@UPDATEDDATE", DbType.Int32, updatedDate);
            db.AddInParameter(dbCmd, "@UPDATEDTIME", DbType.Int32, updatedTime);

            return db.ExecuteNonQuery(dbCmd);
        }

        public static IDataReader CheckItemNameExists(Database db, int itemID, string name)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" SELECT FITEMID FROM ITEMINFO WHERE FITEMID<>@ITEMID AND UPPER(FITEMNAME)=UPPER(@ITEMNAME) AND FSTATUS='A' ");

            DbCommand dbCmd = db.GetSqlStringCommand(sqlBuilder.ToString());
            dbCmd.CommandType = CommandType.Text;

            db.AddInParameter(dbCmd, "@ITEMID", DbType.Int32, itemID);
            db.AddInParameter(dbCmd, "@ITEMNAME", DbType.String, name);

            return db.ExecuteReader(dbCmd);
        }

        public static bool DeleteItem(Database db, int itemID, int updatedBy, int updatedDate, int updatedTime)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" UPDATE ITEMINFO SET FSTATUS='I',FUPDATEDBY=@UPDATEDBY,FUPDATEDDATE=@UPDATEDDATE,FUPDATEDTIME=@UPDATEDTIME WHERE FITEMID=@ITEMID ");

            DbCommand dbCmd = db.GetSqlStringCommand(sqlBuilder.ToString());
            dbCmd.CommandType = CommandType.Text;

            db.AddInParameter(dbCmd, "@ITEMID", DbType.Int32, itemID);
            db.AddInParameter(dbCmd, "@UPDATEDBY", DbType.Int32, updatedBy);
            db.AddInParameter(dbCmd, "@UPDATEDDATE", DbType.Int32, updatedDate);
            db.AddInParameter(dbCmd, "@UPDATEDTIME", DbType.Int32, updatedTime);

            db.ExecuteNonQuery(dbCmd);
            return true;
        }

        #endregion

        #region BillingInfo

        public static long InsertBilling(Database db, DbTransaction transaction, string comments, decimal totalPrice, int createdBy, int createdDate, int createdTime)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" INSERT INTO BILLINGINFO(FCOMMENT,FTOTALPRICE,FCREATEDBY,FCREATEDDATE,FCREATEDTIME) VALUES (@COMMENT,@TOTALPRICE,@CREATEDBY,@CREATEDDATE,@CREATEDTIME); ");
            sqlBuilder.Append(" SELECT LAST_INSERT_ID() ");

            DbCommand dbCmd = db.GetSqlStringCommand(sqlBuilder.ToString());
            dbCmd.CommandType = CommandType.Text;

            db.AddInParameter(dbCmd, "@COMMENT", DbType.String, comments.Trim());
            db.AddInParameter(dbCmd, "@TOTALPRICE", DbType.Decimal, totalPrice);
            db.AddInParameter(dbCmd, "@CREATEDBY", DbType.Int32, createdBy);
            db.AddInParameter(dbCmd, "@CREATEDDATE", DbType.Int32, createdDate);
            db.AddInParameter(dbCmd, "@CREATEDTIME", DbType.Int32, createdTime);

            return Convert.ToInt64(db.ExecuteScalar(dbCmd, transaction));
        }

        public static int InsertBillingItem(Database db, DbTransaction transaction, long billingID, int itemID, decimal itemPrice, int quantity)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" INSERT INTO BILLING_ITEMINFO(FBILLINGID,FITEMID,FITEMPRICE,FQTY) VALUES (@BILLINGID,@ITEMID,@ITEMPRICE,@QTY) ");

            DbCommand dbCmd = db.GetSqlStringCommand(sqlBuilder.ToString());
            dbCmd.CommandType = CommandType.Text;

            db.AddInParameter(dbCmd, "@BILLINGID", DbType.Int64, billingID);
            db.AddInParameter(dbCmd, "@ITEMID", DbType.Int32, itemID);
            db.AddInParameter(dbCmd, "@ITEMPRICE", DbType.Decimal, itemPrice);
            db.AddInParameter(dbCmd, "@QTY", DbType.Int32, quantity);

            return db.ExecuteNonQuery(dbCmd, transaction);
        }

        public static IDataReader GetBillingInfoForExport(Database db, int fromDate, int toDate)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" SELECT B.FBILLINGID,B.FCOMMENT,I.FITEMNAME,BI.FITEMPRICE,BI.FQTY,CONCAT(U.FFIRSTNAME,' ',U.FLASTNAME) AS FUSERNAME ");
            sqlBuilder.Append(" ,B.FCREATEDDATE,B.FCREATEDTIME ");
            sqlBuilder.Append(" FROM BILLINGINFO B ");
            sqlBuilder.Append(" INNER JOIN BILLING_ITEMINFO BI ON BI.FBILLINGID=B.FBILLINGID ");
            sqlBuilder.Append(" INNER JOIN ITEMINFO I ON I.FITEMID=BI.FITEMID ");
            sqlBuilder.Append(" INNER JOIN USERINFO U ON U.FUSERID=B.FCREATEDBY ");
            sqlBuilder.Append(" WHERE B.FCREATEDDATE>=@FROMDATE AND B.FCREATEDDATE<=@TODATE ");

            DbCommand dbCmd = db.GetSqlStringCommand(sqlBuilder.ToString());
            dbCmd.CommandType = CommandType.Text;

            db.AddInParameter(dbCmd, "@FROMDATE", DbType.Int32, fromDate);
            db.AddInParameter(dbCmd, "@TODATE", DbType.Int32, toDate);

            return db.ExecuteReader(dbCmd);
        }

        public static IDataReader GetTotalBillingPrice(Database db, int fromDate, int toDate)
        {
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.Append(" SELECT SUM(B.FTOTALPRICE) AS FYVALUE,B.FCREATEDDATE AS FXVALUE,'Revenue' AS FSERIESNAME ");
            //sqlBuilder.Append(" ,DATE_FORMAT(STR_TO_DATE(CONVERT(FCREATEDDATE,CHAR), '%Y%m%d'),'%Y-%b-%d') AS FXVALUE ");
            sqlBuilder.Append(" FROM BILLINGINFO B ");
            sqlBuilder.Append(" WHERE B.FCREATEDDATE>=@FROMDATE AND B.FCREATEDDATE<=@TODATE ");
            sqlBuilder.Append(" GROUP BY B.FCREATEDDATE ");
            sqlBuilder.Append(" ORDER BY FXVALUE ASC ");

            DbCommand dbCmd = db.GetSqlStringCommand(sqlBuilder.ToString());
            dbCmd.CommandType = CommandType.Text;

            db.AddInParameter(dbCmd, "@FROMDATE", DbType.Int32, fromDate);
            db.AddInParameter(dbCmd, "@TODATE", DbType.Int32, toDate);

            return db.ExecuteReader(dbCmd);
        }

        public static IDataReader GetBillingItemPrice(Database db, int fromDate, int toDate)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" SELECT SUM(BI.FITEMPRICE*BI.FQTY) AS FYVALUE,B.FCREATEDDATE AS FXVALUE ");
            sqlBuilder.Append(" ,I.FITEMNAME AS FSERIESNAME ");
            sqlBuilder.Append(" FROM BILLINGINFO B ");
            sqlBuilder.Append(" INNER JOIN BILLING_ITEMINFO BI ON BI.FBILLINGID=B.FBILLINGID ");
            sqlBuilder.Append(" INNER JOIN ITEMINFO I ON I.FITEMID=BI.FITEMID ");
            sqlBuilder.Append(" WHERE B.FCREATEDDATE>=@FROMDATE AND B.FCREATEDDATE<=@TODATE ");
            sqlBuilder.Append(" GROUP BY B.FCREATEDDATE,I.FITEMNAME ");
            sqlBuilder.Append(" ORDER BY FXVALUE ASC ");

            DbCommand dbCmd = db.GetSqlStringCommand(sqlBuilder.ToString());
            dbCmd.CommandType = CommandType.Text;

            db.AddInParameter(dbCmd, "@FROMDATE", DbType.Int32, fromDate);
            db.AddInParameter(dbCmd, "@TODATE", DbType.Int32, toDate);

            return db.ExecuteReader(dbCmd);
        }

        #endregion
    }
}
