using DG.Tweening;
using JK.Injection;
using JK.Observables;
using Lowrez.Monsters;
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

        public AudioClip endgameClip;

        [Header("Runtime")]

        [SerializeField]
        private bool isPlayingChase = false;

        #endregion

        private AudioSource exploreSource;
        private AudioSource chaseSource;
        private ObservableProperty<float> tension;

        private SignalBus signalBus;

        private void Awake()
        {
            var context = Context.Find(this);

            exploreSource = context.Get<AudioSource>("explore");
            chaseSource = context.Get<AudioSource>("chase");
            tension = context.Get<ObservableProperty<float>>("tension");

            signalBus = context.Get<SignalBus>();
        }

        private void Start()
        {
            tension.onChange.AddListener(OnTensionChanged);
            signalBus.AddListener<BrainDeathSignal>(OnBrainDeathSignal);
        }

        private void OnDestroy()
        {
            tension.onChange.RemoveListener(OnTensionChanged);
            signalBus.RemoveListener<BrainDeathSignal>(OnBrainDeathSignal);
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

        private void OnBrainDeathSignal(BrainDeathSignal signal)
        {
            OnDestroy();
            chaseSource.DOFade(0, 2);

            exploreSource.clip = endgameClip;
            exploreSource.time = 0;
            exploreSource.volume = 1;
            exploreSource.loop = false;
            exploreSource.Play();
        }
    }
}