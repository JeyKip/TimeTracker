using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracker.Services.Storage.Entities.SignIn
{
    public class SigninStore
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string UserDisplayName { get; set; }
    }
}
