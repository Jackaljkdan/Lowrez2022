using JK.Attention;
using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace JK.Lighting
{
    [DisallowMultipleComponent]
    public class Light2DSwitch : MonoBehaviour
    {
        #region Inspector

        public UnityEngine.Rendering.Universal.Light2D target;

        public AudioSource audioSource;
        public AudioClip onClip;
        public AudioClip offClip;

        public UnityEvent onSwitchOn = new UnityEvent();
        public UnityEvent onSwitchOff = new UnityEvent();

        private void Reset()
        {
            target = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
            audioSource = GetComponent<AudioSource>();
        }

        #endregion

        public bool IsOn => target.enabled;

        private void Start()
        {
            Switch(IsOn, playSound: false);
        }

        public void Switch()
        {
            Switch(!IsOn, playSound: true);
        }

        private void Switch(bool on, bool playSound)
        {
            target.enabled = on;

            if (playSound)
                audioSource.PlayOneShotSafely(on ? onClip : offClip);

            if (target.TryGetComponent(out FlickerLight2D flicker))
                flicker.enabled = on;

            if (on)
                onSwitchOn.Invoke();
            else
                onSwitchOff.Invoke();
        }
    }
}