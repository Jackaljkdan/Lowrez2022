using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Rendering.Universal;

namespace JK.Lighting
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Light2D))]
    public class ProportionalLight2DIntensity : MonoBehaviour
    {
        #region Inspector

        public Light2D target;

        #endregion

        private float maxIntensity;
        private float targetMaxIntensity;

        private void Awake()
        {
            maxIntensity = GetComponent<Light2D>().intensity;
            targetMaxIntensity = target.intensity;
        }

        private void LateUpdate()
        {
            GetComponent<Light2D>().intensity = target.enabled
                ? maxIntensity * target.intensity / targetMaxIntensity
                : 0
            ;
        }
    }
}