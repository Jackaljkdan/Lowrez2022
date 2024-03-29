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
    [RequireComponent(typeof(FlickerLight2D))]
    public class TensionFlickerLight2D : MonoBehaviour
    {
        #region Inspector

        public float maxFlickersPerSecond = 5;

        public AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);

        private void OnValidate()
        {
            if (Application.isPlaying && tension != null)
                OnTensionChanged(new ObservableProperty<float>.Changed() { updated = tension.Value });
        }

        #endregion

        private ObservableProperty<float> tension;

        private float minFlickersPerSecond;

        private void Awake()
        {
            var flicker = GetComponent<FlickerLight2D>();
            minFlickersPerSecond = flicker.flickersPerSecond;

            tension = Context.Find(this).Get<ObservableProperty<float>>("tension");
            tension.onChange.AddListener(OnTensionChanged);
        }

        private void Start()
        {
            OnTensionChanged(new ObservableProperty<float>.Changed() { updated = tension.Value });
        }

        private void OnTensionChanged(ObservableProperty<float>.Changed arg)
        {
            var flicker = GetComponent<FlickerLight2D>();
            flicker.flickersPerSecond = Mathf.Lerp(minFlickersPerSecond, maxFlickersPerSecond, curve.Evaluate(arg.updated));
            flicker.RescheduleNextFlickerIfNeeded();
        }
    }
}