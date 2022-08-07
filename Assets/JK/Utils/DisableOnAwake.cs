using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    public class DisableOnAwake : MonoBehaviour
    {
        #region Inspector

        public bool onlyOnBuild = false;

        #endregion

        private void Awake()
        {
            if (!PlatformUtils.IsEditor || !onlyOnBuild)
                gameObject.SetActive(false);
        }
    }
}