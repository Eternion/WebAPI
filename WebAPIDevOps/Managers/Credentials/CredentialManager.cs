using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPIDevOps.Server.Databases.Users;
using WebAPIDevOps.Server.Managers.Users;

namespace WebAPIDevOps.Server.Managers.Credentials
{
    public static class CredentialManager
    {
        public static bool CheckCredentials(out UserRecord record, string username, string pwdHashed)
        {
            record = UserManager.Instance.GetUserByName(username);
            if (record == null)
                return false;
            if (record.PasswordHash != pwdHashed)
                return false;
            return true;
        }
    }
}
