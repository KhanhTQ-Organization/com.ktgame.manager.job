using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace com.ktgame.manager.job
{
    public abstract class BaseJob : IJob
    {
        public string Id { get; }
        public JobPriority Priority { get; protected set; }
        public JobState State { get; private set; }

        private CancellationTokenSource _cts;

        protected BaseJob(string id = null, JobPriority priority = JobPriority.Normal)
        {
            Id = string.IsNullOrEmpty(id) ? Guid.NewGuid().ToString() : id;
            Priority = priority;
            State = JobState.Pending;
        }

        public async UniTask ExecuteAsync(CancellationToken managerToken)
        {
            if (State == JobState.Canceled) return;

            State = JobState.Running;
            
            // Create a linked token so the job can be cancelled either by the manager or by itself
            _cts = CancellationTokenSource.CreateLinkedTokenSource(managerToken);

            try
            {
                await OnExecuteAsync(_cts.Token);
                
                if (State != JobState.Canceled)
                {
                    State = JobState.Completed;
                }
            }
            catch (OperationCanceledException)
            {
                State = JobState.Canceled;
            }
            catch (Exception ex)
            {
                State = JobState.Failed;
                UnityEngine.Debug.LogError($"[JobSystem] Job {Id} failed: {ex.Message}\n{ex.StackTrace}");
            }
            finally
            {
                _cts?.Dispose();
                _cts = null;
            }
        }

        public void Cancel()
        {
            if (State == JobState.Pending || State == JobState.Running)
            {
                State = JobState.Canceled;
                _cts?.Cancel();
            }
        }

        /// <summary>
        /// Derived classes must implement their specific logic here.
        /// </summary>
        protected abstract UniTask OnExecuteAsync(CancellationToken cancellationToken);
    }
}
