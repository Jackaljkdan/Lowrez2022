using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Injection.Tests
{
    [DisallowMultipleComponent]
    public class Test : MonoBehaviour
    {
        #region Inspector

        public GameObject reference;
        public new Light light;

        #endregion

        private string test;

        private Context localCtx;
        private int x;
        private int pi;
        private string s;
        private GameObject boundReference;
        private Light boundLight;
        private InjectionDictionary dict;

        private void Start()
        {
            localCtx = new Context(null, "test-specific");

            localCtx.Bind(42);
            localCtx.Bind(314, "pi");
            localCtx.Bind("hmm");

            localCtx.Bind(reference);
            localCtx.Bind(light);
        }

        [ContextMenu("Keys")]
        private void Keys()
        {
            InjectionDictionary dict = new InjectionDictionary();

            var keyCtx = dict.GetKey(typeof(Context), string.Empty);
            var keyInt = dict.GetKey(typeof(int), string.Empty);
            var keyInt2 = dict.GetKey(typeof(int), string.Empty);
            var keyIntId = dict.GetKey(typeof(int), "id");

            Debug.Log($"keyInt == keyCtx : {keyInt.Equals(keyCtx)}");
            Debug.Log($"keyInt == keyInt2 : {keyInt.Equals(keyInt2)}");
            Debug.Log($"keyInt == keyIntId : {keyInt.Equals(keyIntId)}");
        }

        [ContextMenu("Set")]
        private void Set()
        {
            Context context = Context.Find(this);
            test = context.Get<string>(id: "test");

            x = localCtx.Get<int>();
            pi = localCtx.Get<int>("pi");
            s = localCtx.Get<string>();
            boundReference = localCtx.Get<GameObject>();
            boundLight = localCtx.Get<Light>();
            dict = new InjectionDictionary();
            dict.Add(dict.GetKey(typeof(string), "mmffpp"), "bhueue");

            Show();
        }

        [ContextMenu("Show")]
        private void Show()
        {
            string d = "\"null\"";
            if (dict != null && dict.TryGetValue(dict.GetKey(typeof(string), "mmffpp"), out object val))
                d = (string)val;

            Debug.Log($"test {test ?? "\"null\""} x {x} pi {pi} s {s ?? "\"null\""} br {boundReference.GetName()} light {boundLight.GetName()} dict {d}");
        }
    }
}