using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Rendering.Universal;

namespace JK.Attention
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Light2D))]
    public class FlickerLight2D : MonoBehaviour
    {
        #region Inspector

        public float flickersPerSecond = 1;
        public float offSeconds = 0.1f;

        public float variancePercentage = 0.5f;

        #endregion

        private float minIntensity = 0;
        private float maxIntensity = 1;

        private float nextFlickerTime = 0;

        private void Awake()
        {
            maxIntensity = GetComponent<Light2D>().intensity;
        }

        private void Start()
        {
            RescheduleNextFlicker();
        }

        private void Update()
        {
            if (flickersPerSecond <= 0)
                return;

            if (Time.time < nextFlickerTime)
                return;

            var light = GetComponent<Light2D>();
            light.intensity = light.intensity = minIntensity;

            RescheduleNextFlicker();

            IEnumerator restoreIntensityCoroutine()
            {
                float delta = offSeconds * variancePercentage;
                float waitSeconds = UnityEngine.Random.Range(offSeconds - delta, offSeconds + delta);
                yield return new WaitForSeconds(waitSeconds);
                light.intensity = maxIntensity;
            }

            StartCoroutine(restoreIntensityCoroutine());
        }

        public void RescheduleNextFlicker()
        {
            float avgWaitSeconds = 1 / flickersPerSecond;
            float delta = avgWaitSeconds * variancePercentage;
            nextFlickerTime = Time.time + UnityEngine.Random.Range(avgWaitSeconds - delta, avgWaitSeconds + delta);
        }
    }
}