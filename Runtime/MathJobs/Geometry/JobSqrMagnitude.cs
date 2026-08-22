using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace com.ktgame.manager.job.math.geometry
{
    public struct JobSqrMagnitude
    {
        public Allocator Allocator;
        private const bool compile = true;

        public JobSqrMagnitude(Allocator allocator = Allocator.Persistent)
        {
            this.Allocator = allocator;
        }

        public float Execute(float disX, float disY)
        {
            NativeArray<float> result = new NativeArray<float>(1, Allocator);

            var job = new JobSqrMagnitudeModel()
            {
                DisX = disX,
                DisY = disY,
                Result = result
            };
            job.Schedule().Complete();

            var rs = result[0];
            result.Dispose();

            return rs;
        }

        [BurstCompile(CompileSynchronously = compile)]
        private struct JobSqrMagnitudeModel : Unity.Jobs.IJob
        {
            [ReadOnly]
            public float DisX;

            [ReadOnly]
            public float DisY;

            [WriteOnly]
            public NativeArray<float> Result;

            public void Init(float DisX, float DisY, NativeArray<float> Result)
            {
                this.DisX = DisX;
                this.DisY = DisY;
                this.Result = Result;
            }

            public void Execute()
            {
                float disX = DisX > 0 ? DisX : -DisX;
                float disY = DisY > 0 ? DisY : -DisY;

                Result[0] = disX * disX + disY * disY;
            }
        }
    }
}
