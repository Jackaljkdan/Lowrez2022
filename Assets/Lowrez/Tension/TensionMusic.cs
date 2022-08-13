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
    public class TensionMusic : EndgameListener
    {
        #region Inspector

        public float chaseThreshold = 0.8f;

        public float returnToExploreThreshold = 0.3f;

        public AudioClip winClip;
        public AudioClip loseClip;

        [Header("Runtime")]

        [SerializeField]
        private bool isPlayingChase = false;

        #endregion

        private AudioSource exploreSource;
        private AudioSource chaseSource;
        private ObservableProperty<float> tension;

        protected override void Awake()
        {
            base.Awake();

            var context = Context.Find(this);

            exploreSource = context.Get<AudioSource>("explore");
            chaseSource = context.Get<AudioSource>("chase");
            tension = context.Get<ObservableProperty<float>>("tension");
        }

        protected override void Start()
        {
            base.Start();
            tension.onChange.AddListener(OnTensionChanged);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
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

        protected override void OnEndGame(bool win)
        {
            OnDestroy();
            chaseSource.DOFade(0, 2);

            exploreSource.clip = win ? winClip : loseClip;
            exploreSource.time = 0;
            exploreSource.volume = 1;
            exploreSource.loop = false;
            exploreSource.Play();
        }
    }
}