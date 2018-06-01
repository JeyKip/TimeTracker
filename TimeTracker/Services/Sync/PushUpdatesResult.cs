﻿using System;
using System.Collections.Generic;
using TimeTracker.Common;

namespace TimeTracker.Services.Sync
{
    public class PushUpdatesResult : ResultBase
    {
        public IEnumerable<Guid> MouseIdList { get; set; }
        public IEnumerable<Guid> KeyboardIdList { get; set; }
    }
}