using System.Collections.Generic;

namespace Sttp.Adapters
{
    public class ClientOptions
    {
        #region [ Serialization ]

        public string UserName;

        public string DisplayName
        {
            get
            {
                return UserName;
            }
        }

        public List<string> TryValidate()
        {
            var lst = new List<string>();

            if (string.IsNullOrWhiteSpace(UserName))
                lst.Add("Username cannot be empty");

            return lst;
        }

        #endregion

    }
}
