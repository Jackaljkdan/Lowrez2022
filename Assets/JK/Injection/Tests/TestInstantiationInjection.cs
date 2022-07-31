using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Injection.Tests
{
    [DisallowMultipleComponent]
    public class TestInstantiationInjection : MonoBehaviour
    {
        #region Inspector

        public TestInstantiationInjection prefab;

        #endregion

        public string test;

        private void Awake()
        {
            Context context = Context.Find(this);
            test = context.Get<string>("test");
        }

        [ContextMenu("Test")]
        private void Test()
        {
            TestInstantiationInjection instance = Instantiate(prefab, transform.parent);
            Debug.Log($"instance string: {instance.test} {instance.test == test}");
        }
    }
}