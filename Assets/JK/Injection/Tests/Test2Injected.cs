using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace JK.Injection.Tests
{
    [DisallowMultipleComponent]
    public class Test2Injected : MonoBehaviour
    {
        #region Inspector

        public Text text1;
        public Text text2;

        #endregion

        private void Start()
        {
            text1.text = Context.Find(this).Get<string>("test");
            text2.text = Context.Find(this).Get<string>("test2");
        }
    }
}