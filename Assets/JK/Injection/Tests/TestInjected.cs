using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

namespace JK.Injection.Tests
{
    [DisallowMultipleComponent]
    public class TestInjected : MonoBehaviour
    {
        #region Inspector



        #endregion

        [ContextMenu("Test")]
        private void Test()
        {
            Context context = Context.Find(this);
            string injected = context.Get<string>(id: "test");

            Debug.Log($"injected: {injected} {injected == "test"}");
        }
    }
}