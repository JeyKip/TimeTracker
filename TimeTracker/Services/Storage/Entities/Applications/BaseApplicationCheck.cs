using System;
using System.Collections.Generic;

namespace TimeTracker.Services.Storage
{
    public class BaseApplicationCheck<TApplicationModel>
        where TApplicationModel : class
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CheckDate { get; set; } = DateTime.UtcNow;
        public List<TApplicationModel> Applications { get; set; } = new List<TApplicationModel>();
    }
}