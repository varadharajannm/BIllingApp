using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BillingService
{
    [DataContract]
    public class AppParam
    {
        private int _userID = 0;

        [DataMember]
        public int UserID { get => _userID; set => _userID = value; }
    }

    #region UserInfo

    [DataContract]
    public class UserInfo
    {
        private int _userID = 0;
        private string _userName = string.Empty;
        private string _password = string.Empty;
        private string _firstName = string.Empty;
        private string _lastName = string.Empty;
        private AccessLevel _accessLevel = AccessLevel.Standard;

        [DataMember]
        public int UserID { get => _userID; set => _userID = value; }
        [DataMember]
        public string UserName { get => _userName; set => _userName = value; }
        [DataMember]
        public string Password { get => _password; set => _password = value; }
        [DataMember]
        public string FirstName { get => _firstName; set => _firstName = value; }
        [DataMember]
        public string LastName { get => _lastName; set => _lastName = value; }
        [DataMember]
        public AccessLevel AccessLevel { get => _accessLevel; set => _accessLevel = value; }
    }

    [DataContract]
    public class UserInfoFilter : UserInfo
    {
        private int _loginUserID = 0;

        [DataMember]
        public int LoginUserID { get => _loginUserID; set => _loginUserID = value; }
    }

    [DataContract]
    public class UserFilter : AppParam
    {
        private int _selectedUserID = 0;

        [DataMember]
        public int SelectedUserID { get => _selectedUserID; set => _selectedUserID = value; }
    }

    [DataContract]
    public enum AccessLevel
    {
        [EnumMember]
        Administrator = 1,
        [EnumMember]
        Standard = 2
    }

    #endregion

    #region ItemInfo

    [DataContract]
    public class ItemInfo
    {
        private int _itemID = 0;
        private string _itemName = string.Empty;
        private string _itemDescription = string.Empty;
        private decimal _price = 0;

        [DataMember]
        public int ItemID { get => _itemID; set => _itemID = value; }
        [DataMember]
        public string ItemName { get => _itemName; set => _itemName = value; }
        [DataMember]
        public string ItemDescription { get => _itemDescription; set => _itemDescription = value; }
        [DataMember]
        public decimal Price { get => _price; set => _price = value; }
    }

    [DataContract]
    public class ItemInfoFilter : ItemInfo
    {
        private int _userID = 0;

        [DataMember]
        public int UserID { get => _userID; set => _userID = value; }
    }

    [DataContract]
    public class ItemFilter : AppParam
    {
        private int _itemID = 0;
        private string _itemName = string.Empty;

        [DataMember]
        public int ItemID { get => _itemID; set => _itemID = value; }
        [DataMember]
        public string ItemName { get => _itemName; set => _itemName = value; }
    }

    #endregion

    #region BillingInfo

    [DataContract]
    public class BillingInfo : AppParam
    {
        private long _billingID = 0;
        private decimal _totalPrice = 0;
        private List<BillingItemInfo> _billingItemList = new List<BillingItemInfo>();
        private string _comments = string.Empty;

        [DataMember]
        public long BillingID { get => _billingID; set => _billingID = value; }
        [DataMember]
        public decimal TotalPrice { get => _totalPrice; set => _totalPrice = value; }
        [DataMember]
        public List<BillingItemInfo> BillingItemList { get => _billingItemList; set => _billingItemList = value; }
        [DataMember]
        public string Comments { get => _comments; set => _comments = value; }
    }

    [DataContract]
    public class BillingItemInfo
    {
        private int _itemID = 0;
        private decimal _itemPrice = 0;
        private int _quantity = 0;

        [DataMember]
        public int ItemID { get => _itemID; set => _itemID = value; }
        [DataMember]
        public decimal ItemPrice { get => _itemPrice; set => _itemPrice = value; }
        [DataMember]
        public int Quantity { get => _quantity; set => _quantity = value; }
    }

    [DataContract]
    public class BillingExportFilter
    {
        private int _fromDate = 0;
        private int _toDate = 0;
        private bool _isDrillDown = false;

        [DataMember]
        public int FromDate { get => _fromDate; set => _fromDate = value; }
        [DataMember]
        public int ToDate { get => _toDate; set => _toDate = value; }
        [DataMember]
        public bool IsDrillDown { get => _isDrillDown; set => _isDrillDown = value; }
    }

    [DataContract]
    public class ReportInfo
    {
        private List<string> _xValues = new List<string>();
        private List<List<decimal>> _yValues = new List<List<decimal>>();
        private List<string> _seriesNames = new List<string>();

        [DataMember]
        public List<string> XValues { get => _xValues; set => _xValues = value; }
        [DataMember]
        public List<List<decimal>> YValues { get => _yValues; set => _yValues = value; }
        [DataMember]
        public List<string> SeriesNames { get => _seriesNames; set => _seriesNames = value; }
    }

    public class ReportData
    {
        private decimal _yValue = 0;
        private string _xValue = string.Empty;
        private string _seriesName = string.Empty;

        public decimal YValue { get => _yValue; set => _yValue = value; }
        public string XValue { get => _xValue; set => _xValue = value; }
        public string SeriesName { get => _seriesName; set => _seriesName = value; }
    }

    #endregion
}