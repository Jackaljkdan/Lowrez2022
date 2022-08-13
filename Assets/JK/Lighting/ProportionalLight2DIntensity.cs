using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace JK.Lighting
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(UnityEngine.Rendering.Universal.Light2D))]
    public class ProportionalLight2DIntensity : MonoBehaviour
    {
        #region Inspector

        public UnityEngine.Rendering.Universal.Light2D target;

        #endregion

        private float maxIntensity;
        private float targetMaxIntensity;

        private void Awake()
        {
            maxIntensity = GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity;
            targetMaxIntensity = target.intensity;
        }

        private void LateUpdate()
        {
            GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity = target.enabled
                ? maxIntensity * target.intensity / targetMaxIntensity
                : 0
            ;
        }
    }
}