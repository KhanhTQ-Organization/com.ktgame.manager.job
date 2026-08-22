using com.ktgame.core.manager;

namespace com.ktgame.manager.job
{
    public interface IJobManager : IManager
    {
        int MaxConcurrentJobs { get; set; }
        
        /// <summary>
        /// Adds a job to the execution queue.
        /// </summary>
        void Enqueue(IJob job);
        
        /// <summary>
        /// Cancels a specific job by its ID.
        /// </summary>
        void CancelJob(string jobId);
        
        /// <summary>
        /// Cancels all pending and currently running jobs.
        /// </summary>
        void CancelAll();
    }
}
