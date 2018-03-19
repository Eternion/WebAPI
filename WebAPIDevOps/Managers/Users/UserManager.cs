using System;
using System.Collections.Generic;
using System.Linq;
using WebApiDevOps.Server.Initialization;
using WebApiDevOps.Server.Managers;
using WebAPIDevOps.Constant.Enums;
using WebAPIDevOps.Server.Databases.Users;
using WebAPIDevOps.Server.Managers.Texts;

namespace WebAPIDevOps.Server.Managers.Users
{
    public class UserManager : DataManager<UserManager>
    {
        public Dictionary<int, UserRecord> m_users = new Dictionary<int, UserRecord>();
        [Initialization(InitializationPass.First)]
        public override void Initialize()
        {
            m_users = Database.Query<UserRecord>(UserRecordRelator.FetchQueries).ToDictionary(x => x.Id);
        }
        public UserRecord GetUserById(int userId)
        {
            if (m_users.ContainsKey(userId))
                return m_users[userId];
            return null;
        }
        
        public UserRecord GetUserByName(string username)
        {
            return m_users.Values.FirstOrDefault(record => record.Name == username);
        }

        public bool AddUser(string userName, string passwordhash, string email, LangIdEnum lang, string sid, out string message)
        {
            if(m_users.Values.Any(x => x.Name == userName))
            {
                message = TextManager.Instance.GetText(TextIdEnum.NameExist, lang);
                return false;
            }
            else if(m_users.Values.Any(x => x.Name == email))
            {
                message = TextManager.Instance.GetText(TextIdEnum.EmailExist, lang);
            }

            UserRecord user = new UserRecord()
            {
                Name = userName,
                PasswordHash = passwordhash,
                Lang = (byte)lang,
                CreationDate = DateTime.Now,
                Email = email,
                Sid = sid
            };

            user.Id = (int)Database.Insert(user);

            m_users.Add(user.Id, user);

            message = TextManager.Instance.GetText(TextIdEnum.Ok, lang);
            return true;
        }
    }
}