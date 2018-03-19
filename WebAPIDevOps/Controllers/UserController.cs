using System;
using System.Threading;
using System.Web.Http;
using WebAPIDevOps.Server.Databases.Users;
using WebAPIDevOps.Server.Managers.Credentials;
using WebAPIDevOps.Server.Managers.Users;

namespace WebAPIDevOps.Server.Controllers
{
    [CustomAuthorize]
    [Route("User/{userId:int}")]
    public class UserController : ApiController
    {
        public IHttpActionResult Get(int userId)
        {
            UserRecord user = UserManager.Instance.GetUserById(userId);
            if(user == null)
            {
                return NotFound();
            }
            return Json(user);
        }

        [HttpGet]
        [Route("User/Connect/{username}/{passwordHash}/")]
        public IHttpActionResult ConnectUser(string username, string passwordHash)
        {
            UserRecord user = null;
            if (!CredentialManager.CheckCredentials(out user, username, passwordHash))
                return NotFound();
            return Json(user);
        }

        [HttpPut]
        [Route("User/{userName}/Password/{passwordhash}/Email/{email}")]
        public IHttpActionResult AddNewUser(string userName, string passwordhash, string email)
        {
            IHttpActionResult result = NotFound();
            bool timeout = false;
            var resetEvent = new ManualResetEventSlim();

            if (!resetEvent.Wait(15 * 1000))
            {
                timeout = true;
                return InternalServerError(new TimeoutException());
            }

            return result;
        }
    }
}