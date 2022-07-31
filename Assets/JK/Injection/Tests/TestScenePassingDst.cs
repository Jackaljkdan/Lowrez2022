using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace JK.Injection.Tests
{
    [DisallowMultipleComponent]
    public class TestScenePassingDst : MonoBehaviour
    {
        #region Inspector

        public Text target;

        private void Reset()
        {
            target = GetComponent<Text>();
        }

        #endregion

        private void Awake()
        {
            Context context = Context.Find(this);
            target.text = context.Get<SceneParametersBus>().Get(defaultValue: "unsuccessful");
        }
    }
}