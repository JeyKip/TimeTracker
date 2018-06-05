using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracker.Services.Storage.Entities.SignIn;

namespace TimeTracker.Services.Storage
{
    public interface IStorageService
    {
        #region SignInInfo methods
        SigninStore SaveSignInInfo(SigninStore obj);
        SigninStore LoadSignInInfo();
        #endregion
    }
}
