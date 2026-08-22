using Sirenix.OdinInspector;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using ReadOnly = Unity.Collections.ReadOnlyAttribute;

namespace com.ktgame.manager.job.math.core
{
    public class TestJob : MonoBehaviour
    {
        [Button]
        private void Test()
        {
            var input = new float[10];

            for (int i = 0; i < input.Length; i++)
            {
                input[i] = i * 1.0f;
            }

            Debug.Log($"Normal: {new Calculator().Execute(input)}");
            Debug.Log($"Job: {new JobCalculator().Execute(input)}");
        }
    }    

    public struct Calculator
    {
        public float Execute(params float[] array)
        {
            float result = 0;
            foreach (var item in array)
            {
                result += item;
            }
            return result;
        }
    }

    public struct JobCalculator
    {
        public float Execute(params float[] array)
        {
            var input = new NativeArray<float>(array.Length, Allocator.Persistent);
            var output = new NativeArray<float>(1, Allocator.Persistent);

            for (int i = 0; i < input.Length; i++)
            {
                input[i] = array[i];
            }

            var job = new MyJob()
            {
                Input = input,
                Output = output
            };

            job.Schedule().Complete();
            var result = output[0];

            input.Dispose();
            output.Dispose();

            return result;
        }
    }

    [BurstCompile(CompileSynchronously = true)]
    public struct MyJob : Unity.Jobs.IJob
    {
        [ReadOnly]
        public NativeArray<float> Input;

        [WriteOnly]
        public NativeArray<float> Output;

        public void Execute()
        {
            float result = 0;
            foreach (var item in Input)
            {
                result += item;
            }
            Output[0] = result;
        }
    }
}
