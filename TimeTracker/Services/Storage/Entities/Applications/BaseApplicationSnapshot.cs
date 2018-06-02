using System;
using System.Collections.Generic;

namespace TimeTracker.Services.Storage
{
    public class BaseApplicationSnapshot<TSnapshotItemModel>
        where TSnapshotItemModel : class
    {
        public IEnumerable<TSnapshotItemModel> Items { get; set; }
    }

    public class BaseApplicationSnapshotItem<TApplicationModel>
        where TApplicationModel : class
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CheckDate { get; set; } = DateTime.UtcNow;
        public List<TApplicationModel> Applications { get; set; } = new List<TApplicationModel>();
    }
}