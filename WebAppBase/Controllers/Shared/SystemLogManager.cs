namespace WebAppBase.Controllers
{
    public class SystemLogManager
    {
        public static SystemLogManager GetInstance()
        {
            return new SystemLogManager();
        }

        public void SetSystemErrorLog(string systemName, string organizationKey, long loginID, string loginUserName, string errorMessage, string stackTrace)
        {
            //var wrapper = new CommunicationWrapper();
            //wrapper.AddInParam("SystemName", systemName);
            //wrapper.AddInParam("OrganizationID", organizationKey);
            //wrapper.AddInParam("LoginID", loginID);
            //wrapper.AddInParam("LoginUserName", loginUserName);
            //wrapper.AddInParam("ErrorMessage", errorMessage);
            //wrapper.AddInParam("StackTrace", stackTrace);
            //wrapper.ExecuteAppBase(typeof(InsertErrorLog));
        }

        public void SetSystemAccessLog(string systemName, string organizationKey, long loginID, string loginUserName, string operation, string ipAddress, string userAgent)
        {
            //var wrapper = new CommunicationWrapper();
            //wrapper.AddInParam("SystemName", systemName);
            //wrapper.AddInParam("OrganizationID", organizationKey);
            //wrapper.AddInParam("LoginID", loginID);
            //wrapper.AddInParam("LoginUserName", loginUserName);
            //wrapper.AddInParam("Operation", operation);
            //wrapper.AddInParam("IPAddress", ipAddress);
            //wrapper.AddInParam("UserAgent", userAgent);
            //wrapper.ExecuteAppBase(typeof(InsertAccessLog));
        }
    }
}
