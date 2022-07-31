using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Injection.Tests
{
    [DisallowMultipleComponent]
    public class InjectionBenchmark : MonoBehaviour
    {
        #region Inspector



        #endregion

        private void Start()
        {

        }

        struct Vanilla
        {
            public Type type;
            public string id;
        }

        struct Eq
        {
            public Type type;
            public string id;

            public bool Equals(Eq other)
            {
                return other.type == type && other.id == id;
            }
        }

        struct IEq : IEquatable<IEq>
        {
            public Type type;
            public string id;

            public bool Equals(IEq other)
            {
                return other.type == type && other.id == id;
            }
        }

        [ContextMenu("Run")]
        private void Run()
        {
            Benchmark.Time<object>("baseline", () => null);

            Benchmark.Time("typeof(Test)", () => typeof(string));

            Benchmark.Time("typeof(Test).ToString()", () => typeof(string).ToString());

            Benchmark.Time("vanilla struct", () =>
            {
                Vanilla v1 = new Vanilla { type = typeof(Vanilla), id = "ciao" };
                Vanilla v2 = new Vanilla { type = typeof(Vanilla), id = "ciao" };
                return v1.Equals(v2);
            });

            Benchmark.Time("eq struct", () =>
            {
                Eq v1 = new Eq { type = typeof(Vanilla), id = "ciao" };
                Eq v2 = new Eq { type = typeof(Vanilla), id = "ciao" };
                return v1.Equals(v2);
            });

            Benchmark.Time("ieq struct", () =>
            {
                IEq v1 = new IEq { type = typeof(Vanilla), id = "ciao" };
                IEq v2 = new IEq { type = typeof(Vanilla), id = "ciao" };
                return v1.Equals(v2);
            });

            List<Vanilla> vanillaList = new List<Vanilla>();
            List<Eq> eqList = new List<Eq>();
            List<IEq> ieqList = new List<IEq>();

            for (int i = 0; i < 100; i++)
            {
                vanillaList.Add(new Vanilla { type = typeof(Vanilla), id = i.ToString() });
                eqList.Add(new Eq { type = typeof(Vanilla), id = i.ToString() });
                ieqList.Add(new IEq { type = typeof(Vanilla), id = i.ToString() });
            }

            Benchmark.Time("list contains vanilla", () => vanillaList.Contains(new Vanilla { type = typeof(Vanilla), id = "1" }));
            Benchmark.Time("list contains eq", () => eqList.Contains(new Eq { type = typeof(Vanilla), id = "1" }));
            Benchmark.Time("list contains ieq", () => ieqList.Contains(new IEq { type = typeof(Vanilla), id = "1" }));

            Dictionary<Vanilla, object> vanillaDict = new Dictionary<Vanilla, object>();
            Dictionary<Eq, object> eqDict = new Dictionary<Eq, object>();
            Dictionary<IEq, object> ieqDict = new Dictionary<IEq, object>();

            for (int i = 0; i < 100; i++)
            {
                vanillaDict.Add(new Vanilla { type = typeof(Vanilla), id = i.ToString() }, null);
                eqDict.Add(new Eq { type = typeof(Vanilla), id = i.ToString() }, null);
                ieqDict.Add(new IEq { type = typeof(Vanilla), id = i.ToString() }, null);
            }

            Benchmark.Time("dict contains vanilla", () => vanillaDict.ContainsKey(new Vanilla { type = typeof(Vanilla), id = "1" }));
            Benchmark.Time("dict contains eq", () => eqDict.ContainsKey(new Eq { type = typeof(Vanilla), id = "1" }));
            Benchmark.Time("dict contains ieq", () => ieqDict.ContainsKey(new IEq { type = typeof(Vanilla), id = "1" }));
        }
    }
}