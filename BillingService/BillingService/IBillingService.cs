using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace BillingService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IBillingService
    {

        #region UserInfo

        [OperationContract]
        [WebInvoke(UriTemplate = "GetUserInfo", Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        UserInfo GetUserInfo(string username, string password);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetUsers", Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<UserInfo> GetUsers();

        [OperationContract]
        [WebInvoke(UriTemplate = "InsertOrUpdateUser", Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        int InsertOrUpdateUser(UserInfoFilter userInfo);

        [OperationContract]
        [WebInvoke(UriTemplate = "DeleteUser", Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        bool DeleteUser(UserFilter filter);

        #endregion

        #region ItemInfo

        [OperationContract]
        [WebInvoke(UriTemplate = "GetItems", Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<ItemInfo> GetItems();

        [OperationContract]
        [WebInvoke(UriTemplate = "InsertOrUpdateItem", Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        int InsertOrUpdateItem(ItemInfoFilter itemInfo);

        [OperationContract]
        [WebInvoke(UriTemplate = "DeleteItem", Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        bool DeleteItem(ItemFilter filter);

        #endregion

        #region BillingInfo

        [OperationContract]
        [WebInvoke(UriTemplate = "InsertBillingInfo", Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        long InsertBillingInfo(BillingInfo billingInfo);

        [OperationContract]
        [WebInvoke(UriTemplate = "DownloadBillingInfo", Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string DownloadBillingInfo(BillingExportFilter filter);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetReportData", Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ReportInfo GetReportData(BillingExportFilter filter);

        #endregion
    }
}
