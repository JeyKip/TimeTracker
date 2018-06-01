﻿namespace TimeTracker.Services.Tracking
{
    public interface ITakeSnapshot<TSnapshotModel>
        where TSnapshotModel: class
    {
        /// <summary>
        /// Takes a snapshot of tracking service activity and resets any internal counters.
        /// </summary>
        /// <returns>An instance </returns>
        TSnapshotModel TakeSnapshot();
    }
}