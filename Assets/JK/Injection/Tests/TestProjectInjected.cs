using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Injection.Tests
{
    [DisallowMultipleComponent]
    public class TestProjectInjected : MonoBehaviour
    {
        #region Inspector

        

        #endregion

        [ContextMenu("Test")]
        private void Test()
        {
            Context context = Context.Find(this);
            string injected = context.Get<string>(id: "project");

            Debug.Log($"injected: {injected} {injected == "project"}");
        }
    }
}