namespace CTP.Net
{
    public class SrpUserMapping
    {
        public string LoginName;
        public string[] Roles;

        public SrpUserMapping(string loginName, string[] roles)
        {
            LoginName = loginName;
            Roles = roles;
        }
    }
}