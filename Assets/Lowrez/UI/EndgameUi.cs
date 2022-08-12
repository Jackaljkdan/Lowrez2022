using DG.Tweening;
using JK.Injection;
using JK.Utils;
using Lowrez.Monsters;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Lowrez.UI
{
    [DisallowMultipleComponent]
    public class EndgameUi : MonoBehaviour
    {
        #region Inspector

        public SpriteRenderer backgroundRenderer;

        public SpriteRenderer endgameRenderer;

        public Sprite winSprite;
        public Sprite loseSprite;

        public Sprite organicBgSprite;

        #endregion

        private SignalBus signalBus;

        private void Awake()
        {
            signalBus = Context.Find(this).Get<SignalBus>();
        }

        private void Start()
        {
            backgroundRenderer.gameObject.SetActive(false);
            endgameRenderer.gameObject.SetActive(false);
            signalBus.AddListener<BrainDeathSignal>(OnBrainDeathSignal);
        }

        private void OnDestroy()
        {
            signalBus.RemoveListener<BrainDeathSignal>(OnBrainDeathSignal);
        }

        private void OnBrainDeathSignal(BrainDeathSignal signal)
        {
            backgroundRenderer.gameObject.SetActive(true);

            //backgroundRenderer.color = Color.white.WithAlpha(0);
            //backgroundRenderer.DOColor(Color.white, 2).onComplete += () =>
            //{
            //    backgroundRenderer.DOColor(Color.black, 1).onComplete += () =>
            //    {
            //        endgameRenderer.gameObject.SetActive(true);
            //        endgameRenderer.sprite = winSprite;
            //        endgameRenderer.color = Color.white.WithAlpha(0);
            //        endgameRenderer.DOColor(Color.white, 1);
            //    };
            //};

            backgroundRenderer.sprite = organicBgSprite;
            backgroundRenderer.color = Color.white.WithAlpha(0);
            backgroundRenderer.DOColor(Color.white, 2).onComplete += () =>
            {
                endgameRenderer.gameObject.SetActive(true);
                endgameRenderer.sprite = winSprite;
                endgameRenderer.color = Color.white.WithAlpha(0);
                endgameRenderer.DOColor(Color.white, 1);
            };
        }
    }
}