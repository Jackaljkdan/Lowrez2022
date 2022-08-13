using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace JK.Attention
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(UnityEngine.Rendering.Universal.Light2D))]
    public class FlickerLight2D : MonoBehaviour
    {
        #region Inspector

        public float flickersPerSecond = 1;
        public float offSeconds = 0.1f;

        public float variancePercentage = 0.5f;
        
        public float rescheduleThresholdPercentage = 0.2f;

        [Header("Runtime")]

        [SerializeField]
        private float nextFlickerTime = 0;

        #endregion

        private float minIntensity = 0;
        private float maxIntensity = 1;


        private void Awake()
        {
            maxIntensity = GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity;
        }

        private void Start()
        {
            ForceRescheduleNextFlicker();
        }

        private void Update()
        {
            if (flickersPerSecond <= 0)
                return;

            if (Time.time < nextFlickerTime)
                return;

            var light = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
            light.intensity = light.intensity = minIntensity;

            ForceRescheduleNextFlicker();

            IEnumerator restoreIntensityCoroutine()
            {
                float delta = offSeconds * variancePercentage;
                float waitSeconds = UnityEngine.Random.Range(offSeconds - delta, offSeconds + delta);
                yield return new WaitForSeconds(waitSeconds);
                light.intensity = maxIntensity;
            }

            StartCoroutine(restoreIntensityCoroutine());
        }

        public void ForceRescheduleNextFlicker()
        {
            float avgWaitSeconds = 1 / flickersPerSecond;
            float delta = avgWaitSeconds * variancePercentage;
            nextFlickerTime = Time.time + UnityEngine.Random.Range(avgWaitSeconds - delta, avgWaitSeconds + delta);
        }

        public void RescheduleNextFlickerIfNeeded()
        {
            float waitSeconds = nextFlickerTime - Time.time;

            if (waitSeconds < 1)
                return;

            float avgWaitSeconds = 1 / flickersPerSecond;
            float delta = Math.Abs(waitSeconds - avgWaitSeconds);

            if (delta > avgWaitSeconds * rescheduleThresholdPercentage)
                ForceRescheduleNextFlicker();
        }
    }
}