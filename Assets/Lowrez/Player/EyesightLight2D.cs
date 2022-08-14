using DG.Tweening;
using JK.Lighting;
using JK.Utils.DGTweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

namespace Lowrez.Player
{
    [DisallowMultipleComponent]
    public class EyesightLight2D : MonoBehaviour
    {
        #region Inspector

        public float offIntensityMultiplier = 1.5f;

        public float tweenSeconds = 1;

        public Light2D target;

        public Light2DSwitch flashlightSwitch;

        private void Reset()
        {
            target = GetComponent<Light2D>();
        }

        #endregion

        private Tween tween;

        private float referenceIntensity;

        private void Awake()
        {
            referenceIntensity = target.intensity;
        }

        private void Start()
        {
            flashlightSwitch.onSwitchOn.AddListener(OnSwitchedOn);
            flashlightSwitch.onSwitchOff.AddListener(OnSwitchedOff);

            if (flashlightSwitch.IsOn)
                OnSwitchedOn();
            else
                OnSwitchedOff();
        }

        private void OnDestroy()
        {
            flashlightSwitch.onSwitchOn.RemoveListener(OnSwitchedOn);
            flashlightSwitch.onSwitchOff.RemoveListener(OnSwitchedOff);
        }

        private void OnSwitchedOff()
        {
            TweenLight(referenceIntensity * offIntensityMultiplier);
        }

        private void OnSwitchedOn()
        {
            TweenLight(referenceIntensity);
        }

        private void TweenLight(float intensity)
        {
            target.intensity = 0;

            tween?.Kill();
            tween = target.DOIntensity(intensity, tweenSeconds);
        }
    }
}