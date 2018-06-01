using System.Collections.Generic;

namespace TimeTracker.Services.Storage
{
    public class BaseApplicationSnapshot<TApplicationModel>
        where TApplicationModel : class
    {
        public List<TApplicationModel> Checks { get; set; }
    }
}