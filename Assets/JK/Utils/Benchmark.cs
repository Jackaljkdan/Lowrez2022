using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    public static class Benchmark
    {
        const int Iterations = 1000000;

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int Call(object obj)
        {
            return 1;
        }

        public static void Time<T>(string name, Func<T> func)
        {
            Stopwatch sw = Stopwatch.StartNew();

            int count = 0;
            for (int i = 0; i < Iterations; i++)
                count += Call(func());

            sw.Stop();
            UnityEngine.Debug.Log($"{name}: {sw.ElapsedTicks} ticks {sw.ElapsedMilliseconds} ms");
        }
    }
}