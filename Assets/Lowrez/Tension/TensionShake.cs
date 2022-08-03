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
    [RequireComponent(typeof(Shake))]
    public class TensionShake : MonoBehaviour
    {
        #region Inspector

        public float maxIntensityMultiplier = 0.08f;
        public float maxFrequencyMultiplier = 5f;

        #endregion

        private ObservableProperty<float> tension;

        private float minIntensityMultiplier;
        private float minFrequencyMultiplier;

        private void Awake()
        {
            var shake = GetComponent<Shake>();
            minIntensityMultiplier = shake.intensityMultiplier;
            minFrequencyMultiplier = shake.frequencyMultiplier;

            tension = Context.Find(this).Get<ObservableProperty<float>>("tension");
            tension.onChange.AddListener(OnTensionChanged);
        }

        private void Start()
        {
            OnTensionChanged(new ObservableProperty<float>.Changed() { updated = tension.Value });
        }

        private void OnTensionChanged(ObservableProperty<float>.Changed arg)
        {
            var shake = GetComponent<Shake>();
            shake.intensityMultiplier = Mathf.Lerp(minIntensityMultiplier, maxIntensityMultiplier, arg.updated);
            shake.frequencyMultiplier = Mathf.Lerp(minFrequencyMultiplier, maxFrequencyMultiplier, arg.updated);
        }
    }
}