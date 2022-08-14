using DG.Tweening;
using JK.Attention;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Sounds
{
    [DisallowMultipleComponent]
    public class BuzzOnFlickerLight2D : MonoBehaviour
    {
        #region Inspector

        public float threshold = 1.5f;

        public FlickerLight2D flicker;

        public AudioSource audioSource;

        [Header("Runtime")]

        public bool wasFlickering;

        private void Reset()
        {
            flicker = GetComponentInParent<FlickerLight2D>();
            audioSource = GetComponentInChildren<AudioSource>();
        }

        #endregion

        private float referenceVolume;

        private Tween tween;

        private bool IsFlickeringEnough => flicker.enabled && flicker.flickersPerSecond > threshold;

        private void Start()
        {
            referenceVolume = audioSource.volume;
            audioSource.Stop();

            wasFlickering = false;
        }

        private void LateUpdate()
        {
            if (IsFlickeringEnough && !wasFlickering)
            {
                wasFlickering = true;
                tween?.Kill();
                tween = audioSource.DOFade(referenceVolume, 0.5f);
                audioSource.Play();
            }
            else if (!IsFlickeringEnough && wasFlickering)
            {
                wasFlickering = false;
                tween?.Kill();
                tween = audioSource.DOFade(0, 0.5f);
                tween.onComplete += () => audioSource.Pause();
            }
        }
    }
}