using JK.Attention;
using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Rendering.Universal;

namespace JK.Lighting
{
    [DisallowMultipleComponent]
    public class Light2DSwitch : MonoBehaviour
    {
        #region Inspector

        public Light2D target;

        public AudioSource audioSource;
        public AudioClip onClip;
        public AudioClip offClip;

        private void Reset()
        {
            target = GetComponent<Light2D>();
            audioSource = GetComponent<AudioSource>();
        }

        #endregion

        public bool IsOn => target.enabled;

        public void Switch()
        {
            Switch(!IsOn);
        }

        private void Switch(bool on)
        {
            target.enabled = on;
            audioSource.PlayOneShotSafely(on ? onClip : offClip);

            if (target.TryGetComponent(out FlickerLight2D flicker))
                flicker.enabled = on;
        }
    }
}