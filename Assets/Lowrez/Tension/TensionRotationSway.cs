using JK.Attention;
using JK.Injection;
using JK.Observables;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Lowrez.Tension
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RotationSway))]
    public class TensionRotationSway : MonoBehaviour
    {
        #region Inspector

        public float maxIntensityMultiplier = 20f;
        public float maxFrequencyMultiplier = 1f;

        #endregion

        private ObservableProperty<float> tension;

        private float minIntensityMultiplier;
        private float minFrequencyMultiplier;

        private void Awake()
        {
            var rotationSway = GetComponent<RotationSway>();
            minIntensityMultiplier = rotationSway.intensityMultiplier;
            minFrequencyMultiplier = rotationSway.frequencyMultiplier;

            tension = Context.Find(this).Get<ObservableProperty<float>>("tension");
            tension.onChange.AddListener(OnTensionChanged);
        }

        private void Start()
        {
            OnTensionChanged(new ObservableProperty<float>.Changed() { updated = tension.Value });
        }

        private void OnTensionChanged(ObservableProperty<float>.Changed arg)
        {
            var rotationSway = GetComponent<RotationSway>();
            rotationSway.intensityMultiplier= Mathf.Lerp(minIntensityMultiplier, maxIntensityMultiplier, arg.updated);
            rotationSway.frequencyMultiplier = Mathf.Lerp(minFrequencyMultiplier, maxFrequencyMultiplier, arg.updated);
        }
    }
}