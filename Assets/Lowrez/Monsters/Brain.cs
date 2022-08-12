using DG.Tweening;
using JK.Utils;
using JK.World;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Lowrez.Monsters
{
    [DisallowMultipleComponent]
    public class Brain : MonoBehaviour
    {
        #region Inspector

        public Trigger2DEvents painTrigger;

        public AudioSource audioSource;

        public float healAmount = 0.1f;

        [Header("Runtime")]
        public float pain;

        [field: SerializeField]
        public bool IsInPain { get; private set; }

        private void Reset()
        {
            painTrigger = GetComponentInChildren<Trigger2DEvents>();
            audioSource = GetComponent<AudioSource>();
        }

        #endregion

        private Tween tween;

        private void Start()
        {
            painTrigger.onEnter.AddListener(OnPainTriggerEnter);
            painTrigger.onExit.AddListener(OnPainTriggerExit);
        }

        private void OnDestroy()
        {
            painTrigger.onEnter.RemoveListener(OnPainTriggerEnter);
            painTrigger.onExit.RemoveListener(OnPainTriggerExit);
        }

        private void OnPainTriggerEnter(Collider2D _)
        {
            if (!IsInPain)
            {
                IsInPain = true;
                audioSource.time = pain;
                audioSource.Play();
            }

            tween?.Kill();
            tween = audioSource.DOFade(1, 0.5f);
        }

        private void OnPainTriggerExit(Collider2D _)
        {
            tween?.Kill();
            tween = audioSource.DOFade(0, 0.5f);
            tween.onComplete += () =>
            {
                IsInPain = false;
                audioSource.Pause();
            };
        }

        private void Update()
        {
            if (IsInPain)
            {
                pain = audioSource.time;
            }
            else
            {
                pain = Mathf.Max(0, pain - TimeUtils.AdjustToFrameRate(healAmount));
            }
        }
    }
}