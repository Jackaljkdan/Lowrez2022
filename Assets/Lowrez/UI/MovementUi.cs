using DG.Tweening;
using JK.Actuators;
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
    public class MovementUi : EndgameListener
    {
        #region Inspector

        public Animator target;

        private void Reset()
        {
            target = GetComponentInChildren<Animator>();
        }

        #endregion

        private Tween tween;
        private int grabbingCount = 0;

        protected override void Start()
        {
            base.Start();

            signalBus.AddListener<ClosedLightUiSignal>(OnClosedLightUiSignal);

            signalBus.AddListener<GrabbedSignal>(OnGrabbedSignal);
            signalBus.AddListener<UngrabbedSignal>(OnUngrabbedSignal);

            target.gameObject.SetActive(false);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            RemoveListeners();
        }

        private void RemoveListeners()
        {
            signalBus.RemoveListener<GrabbedSignal>(OnGrabbedSignal);
            signalBus.RemoveListener<UngrabbedSignal>(OnUngrabbedSignal);
        }

        private void OnClosedLightUiSignal(ClosedLightUiSignal arg)
        {
            ShowAsTutorial();
        }

        protected override void OnEndGame(bool win)
        {
            RemoveListeners();
            StopAllCoroutines();
            ForceDismiss();
        }

        private void OnGrabbedSignal(GrabbedSignal arg)
        {
            grabbingCount++;

            if (grabbingCount != 1)
                return;

            Show(speed: 2);
        }

        private void OnUngrabbedSignal(UngrabbedSignal arg)
        {
            grabbingCount = Mathf.Max(0, grabbingCount - 1);
            DismissIfAppropriate();
        }

        private void ShowAsTutorial()
        {
            Show(speed: 0.25f);

            var playerMovement = Context.Find(this).Get<MovementActuatorBehaviour>("player");

            IEnumerator DismissOnInputCoroutine()
            {
                yield return new WaitWhile(() => playerMovement.Input.sqrMagnitude == 0);
                DismissIfAppropriate();
            }

            StartCoroutine(DismissOnInputCoroutine());
        }

        private void Show(float speed)
        {
            tween?.Kill();

            target.gameObject.SetActive(true);

            var spriteRenderer = target.GetComponent<SpriteRenderer>();
            spriteRenderer.color = Color.white.WithAlpha(0);
            tween = spriteRenderer.DOFade(1, 0.25f);
            target.SetFloat("Speed", speed);
        }

        private void DismissIfAppropriate()
        {
            if (grabbingCount > 0)
                return;

            ForceDismiss();
        }

        private void ForceDismiss()
        {
            tween?.Kill();

            var spriteRenderer = target.GetComponent<SpriteRenderer>();
            spriteRenderer.DOFade(0, 0.25f).onComplete += () =>
            {
                target.gameObject.SetActive(false);
            };
        }
    }
}