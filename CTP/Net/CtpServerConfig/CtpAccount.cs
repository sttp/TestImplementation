using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CTP.Net
{
    [CommandName("CtpAccount")]
    public class CtpAccount
        : CommandObject<CtpAccount>
    {
        [CommandField()]
        public bool IsEnabled { get; set; }

        [CommandField()]
        public string Name { get; set; }

        [CommandField()]
        public string Description { get; set; }

        /// <summary>
        /// All of the roles granted with this account.
        /// </summary>
        [CommandField()]
        public List<string> Roles { get; set; }

        public CtpAccount()
        {
            Roles = new List<string>();
        }

        public static explicit operator CtpAccount(CtpCommand obj)
        {
            return FromCommand(obj);
        }

        public string DisplayMember
        {
            get
            {
                var sb = new StringBuilder();
                if (!IsEnabled)
                    sb.Append("(Disabled) ");
                if (!string.IsNullOrWhiteSpace(Name))
                    sb.Append("Name: " + Name + "; ");
                if (Roles != null)
                {
                    sb.Append("Roles: " + string.Join(", ", Roles) + "; ");
                }

                return sb.ToString();
            }
        }
    }
}