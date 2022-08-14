using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.UI
{
    [DisallowMultipleComponent]
    public class SetCursorTexture : MonoBehaviour
    {
        #region Inspector

        public Texture2D cursorTexture;

        public CursorMode cursorMode = CursorMode.Auto;

        public Vector2 hotSpot = Vector2.zero;

        #endregion

        private void Start()
        {
            Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
        }
    }
}