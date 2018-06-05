using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracker.Properties;
using TimeTracker.Services.Storage.Entities.SignIn;

namespace TimeTracker.Services.Storage
{
    public class StorageService : IStorageService
    {
        private object _lockObject = new object();

        public SigninStore LoadSignInInfo()
        {
            var filePath = getFilePath(typeof(SigninStore));
            SigninStore data = new SigninStore();
            if (!File.Exists(filePath))
                return data;

            lock (_lockObject) {
                var file = File.ReadAllText(filePath);
                data = JsonConvert.DeserializeObject<SigninStore>(file);
            }
            return data;
        }

        public SigninStore SaveSignInInfo(SigninStore obj)
        {
            var filePath = getFilePath(obj.GetType());
            var data = JsonConvert.SerializeObject(obj);

            lock (_lockObject)
            {
                File.WriteAllText(filePath, data);
            }
            return obj;
        }

        private string getFilePath(Type type) {
            var dir = Path.Combine($"{AppDomain.CurrentDomain.BaseDirectory}", Settings.Default.StoragePath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            var filePath = Path.Combine(dir, $"{type.Name}.data");
            return filePath;
        }
    }
}
