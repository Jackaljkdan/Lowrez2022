using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Injection.Tests
{
    public class TestBinding : MonoBehaviour
    {
        #region Inspector

        

        #endregion

        [ContextMenu("Test")]
        private void Test()
        {
            Context context = Context.Find(this);
            TestBinding test = context.Get<TestBinding>();

            Debug.Log($"injected: {test.name} {test == this}");
        }
    }
}