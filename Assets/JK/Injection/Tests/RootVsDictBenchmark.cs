using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Injection.Tests
{
    [DisallowMultipleComponent]
    public class RootVsDictBenchmark : MonoBehaviour
    {
        #region Inspector

        

        #endregion

        [ContextMenu("Run")]
        private void Run()
        {
            Dictionary<Type, Transform> dict = new Dictionary<Type, Transform>();

            dict.Add(typeof(int), transform.root);
            dict.Add(typeof(float), transform.root);
            dict.Add(typeof(double), transform.root);
            dict.Add(typeof(uint), transform.root);

            dict.Add(typeof(char), transform.root);
            dict.Add(typeof(string), transform.root);
            dict.Add(typeof(List<char>), transform.root);
            dict.Add(typeof(List<string>), transform.root);

            dict.Add(typeof(List<int>), transform.root);
            dict.Add(typeof(List<float>), transform.root);
            dict.Add(typeof(List<double>), transform.root);
            dict.Add(typeof(List<uint>), transform.root);

            dict.Add(typeof(GameObject), transform.root);
            dict.Add(typeof(Component), transform.root);
            dict.Add(typeof(MonoBehaviour), transform.root);
            dict.Add(typeof(RootVsDictBenchmark), transform.root);

            Benchmark.Time("dict", () => dict[typeof(int)]);
            Benchmark.Time("root", () => transform.root);
        }
    }
}