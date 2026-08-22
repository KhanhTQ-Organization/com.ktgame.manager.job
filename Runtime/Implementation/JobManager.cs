using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace com.ktgame.manager.job
{
    public class JobManager : IJobManager
    {
        public int Priority => 0; // IManager interface property
        public bool IsInitialized { get; private set; }
        
        public int MaxConcurrentJobs { get; set; } = 3;

        private readonly List<IJob> _pendingJobs = new List<IJob>();
        private readonly Dictionary<string, IJob> _runningJobs = new Dictionary<string, IJob>();
        private CancellationTokenSource _globalCts;
        private bool _isProcessingQueue = false;

        public UniTask Initialize()
        {
            if (IsInitialized) return UniTask.CompletedTask;

            _globalCts = new CancellationTokenSource();
            IsInitialized = true;
            return UniTask.CompletedTask;
        }

        public void Enqueue(IJob job)
        {
            if (job == null) return;
            
            _pendingJobs.Add(job);
            // Sort by priority (Highest first). If priorities are equal, you could maintain insertion order, 
            // but for simplicity, we just sort by enum value descending.
            _pendingJobs.Sort((a, b) => b.Priority.CompareTo(a.Priority));
            
            ProcessQueueAsync().Forget();
        }

        private async UniTaskVoid ProcessQueueAsync()
        {
            if (_isProcessingQueue) return;
            _isProcessingQueue = true;

            try
            {
                while (_pendingJobs.Count > 0 || _runningJobs.Count > 0)
                {
                    _globalCts.Token.ThrowIfCancellationRequested();

                    // Start jobs up to MaxConcurrentJobs
                    while (_runningJobs.Count < MaxConcurrentJobs && _pendingJobs.Count > 0)
                    {
                        var job = _pendingJobs[0];
                        _pendingJobs.RemoveAt(0);

                        if (job.State == JobState.Canceled) continue;

                        _runningJobs[job.Id] = job;
                        RunJobFireAndForget(job).Forget();
                    }

                    // Wait until at least one job finishes before trying to start more
                    // A small yield prevents locking the main thread if everything finishes instantly
                    await UniTask.Yield(PlayerLoopTiming.Update, _globalCts.Token);
                }
            }
            catch (System.OperationCanceledException)
            {
                // System was shut down
            }
            finally
            {
                _isProcessingQueue = false;
            }
        }

        private async UniTaskVoid RunJobFireAndForget(IJob job)
        {
            try
            {
                await job.ExecuteAsync(_globalCts.Token);
            }
            finally
            {
                _runningJobs.Remove(job.Id);
            }
        }

        public void CancelJob(string jobId)
        {
            // Check running jobs
            if (_runningJobs.TryGetValue(jobId, out var runningJob))
            {
                runningJob.Cancel();
            }

            // Check pending jobs
            var pendingJob = _pendingJobs.FirstOrDefault(j => j.Id == jobId);
            if (pendingJob != null)
            {
                pendingJob.Cancel();
                _pendingJobs.Remove(pendingJob);
            }
        }

        public void CancelAll()
        {
            foreach (var job in _pendingJobs)
            {
                job.Cancel();
            }
            _pendingJobs.Clear();

            foreach (var job in _runningJobs.Values.ToList())
            {
                job.Cancel();
            }
            // Cannot clear runningJobs here immediately as the tasks will remove themselves in their finally blocks.
        }

        public void Dispose()
        {
            CancelAll();
            _globalCts?.Cancel();
            _globalCts?.Dispose();
            IsInitialized = false;
        }
    }
}
