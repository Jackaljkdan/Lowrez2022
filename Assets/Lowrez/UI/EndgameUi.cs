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
    public class EndgameUi : EndgameListener
    {
        #region Inspector

        public SpriteRenderer backgroundRenderer;

        public SpriteRenderer endgameRenderer;

        public Sprite winSprite;
        public Sprite loseSprite;

        public Sprite organicBgSprite;

        #endregion

        protected override void Start()
        {
            base.Start();
            backgroundRenderer.gameObject.SetActive(false);
            endgameRenderer.gameObject.SetActive(false);
        }

        protected override void OnEndGame(bool win)
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