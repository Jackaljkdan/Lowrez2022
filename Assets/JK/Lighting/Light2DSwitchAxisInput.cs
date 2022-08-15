using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Lighting
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Light2DSwitch))]
    public class Light2DSwitchAxisInput : MonoBehaviour
    {
        #region Inspector

        public string axis = "Fire1";

        #endregion

        private float lastFrameValue;

        private void Update()
        {
            float value = UnityEngine.Input.GetAxisRaw(axis);

            if (value == 1 && value != lastFrameValue)
                GetComponent<Light2DSwitch>().Switch();

            lastFrameValue = value;
        }
    }
}