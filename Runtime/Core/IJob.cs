using Cysharp.Threading.Tasks;
using System.Threading;

namespace com.ktgame.manager.job
{
    public interface IJob
    {
        string Id { get; }
        JobPriority Priority { get; }
        JobState State { get; }
        
        /// <summary>
        /// Executes the job asynchronously.
        /// </summary>
        UniTask ExecuteAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Attempts to cancel the job gracefully.
        /// </summary>
        void Cancel();
    }
}
