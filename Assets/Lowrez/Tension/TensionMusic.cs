using DG.Tweening;
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
    public class TensionMusic : MonoBehaviour
    {
        #region Inspector

        public float chaseThreshold = 0.8f;

        public float returnToExploreThreshold = 0.3f;

        [Header("Runtime")]

        [SerializeField]
        private bool isPlayingChase = false;

        #endregion

        private AudioSource exploreSource;
        private AudioSource chaseSource;
        private ObservableProperty<float> tension;


        private void Awake()
        {
            var context = Context.Find(this);

            exploreSource = context.Get<AudioSource>("explore");
            chaseSource = context.Get<AudioSource>("chase");
            tension = context.Get<ObservableProperty<float>>("tension");
        }

        private void Start()
        {
            tension.onChange.AddListener(OnTensionChanged);
        }

        private void OnDestroy()
        {
            tension.onChange.RemoveListener(OnTensionChanged);
        }

        private void OnTensionChanged(ObservableProperty<float>.Changed arg)
        {
            if (!isPlayingChase)
            {
                if (arg.updated >= chaseThreshold)
                {
                    exploreSource.DOFade(0, 0.5f).onComplete += () =>
                    {
                        exploreSource.Pause();
                    };

                    chaseSource.time = 0;
                    chaseSource.volume = 1;
                    chaseSource.Play();

                    isPlayingChase = true;
                }
            }
            else if (arg.updated <= returnToExploreThreshold)
            {
                chaseSource.DOFade(0, 4).onComplete += () =>
                {
                    chaseSource.Pause();
                };

                exploreSource.Play();
                exploreSource.DOFade(1, 4);

                isPlayingChase = false;
            }
        }
    }
}