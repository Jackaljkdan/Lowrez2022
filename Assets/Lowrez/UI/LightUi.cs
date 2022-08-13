using DG.Tweening;
using JK.Injection;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Lowrez.UI
{
    [DisallowMultipleComponent]
    public class LightUi : MonoBehaviour
    {
        #region Inspector

        public SpriteRenderer spriteRenderer;

        private void Reset()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        #endregion

        private void Update()
        {
            if (Input.GetAxisRaw("Fire1") > 0)
            {
                enabled = false;
                spriteRenderer.DOFade(0, 0.25f).onComplete += () =>
                {
                    spriteRenderer.gameObject.SetActive(false);
                    Context.Find(this).Get<SignalBus>().Invoke(new ClosedLightUiSignal());
                };
            }
        }
    }
}