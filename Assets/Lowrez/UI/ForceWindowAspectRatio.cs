using JK.Injection;
using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Lowrez.UI
{
    [DisallowMultipleComponent]
    public class ForceWindowAspectRatio : MonoBehaviour
    {
        #region Inspector



        #endregion

        //Text dbgText; 

        //private void Awake()
        //{
        //    dbgText = Context.Find(this).Get<Text>("dbg");
        //}

        private Vector2 prevSize;
        private Vector2 lastFrameSize;

        private void Start()
        {
            if (!PlatformUtils.IsDesktop || PlatformUtils.IsWebGL)
                Destroy(this);

            prevSize = new Vector2(Screen.width, Screen.height);
            lastFrameSize = prevSize;
        }

        private void Update()
        {
            if (Screen.fullScreenMode != FullScreenMode.Windowed)
                return;

            //if (dbgText)
            //    dbgText.text = $"{Screen.width} x {Screen.height}";

            if (PlatformUtils.IsEditor)
                return;

            Vector2 currentSize = new Vector2(Screen.width, Screen.height);

            if (currentSize != lastFrameSize)
            {
                lastFrameSize = currentSize;
                CancelInvoke(nameof(FixWindow));
                Invoke(nameof(FixWindow), 0.1f);
            }
        }

        private void FixWindow()
        {
            if (Screen.width != Screen.height)
            {
                int chosen;

                if (prevSize.x == Screen.width)
                    chosen = Screen.height;
                else if (prevSize.y == Screen.height)
                    chosen = Screen.width;
                else
                    chosen = Mathf.Max(Screen.height, Screen.width);

                Screen.SetResolution(chosen, chosen, FullScreenMode.Windowed);

                prevSize = new Vector2(chosen, chosen);
            }
        }
    }
}