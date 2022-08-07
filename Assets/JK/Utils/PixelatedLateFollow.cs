using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    [DisallowMultipleComponent]
    public class PixelatedLateFollow : LateFollow
    {
        #region Inspector

        public float unit = 1;

        #endregion

        private void LateUpdate()
        {
            Vector3 pixelated = new Vector3(
                Pixelate(target.position.x),
                Pixelate(target.position.y),
                Pixelate(target.position.z)
            );

            transform.position = pixelated + offset;
        }

        private float Pixelate(float value)
        {
            float mult = value / unit;
            return unit * Mathf.Floor(mult);
        }
    }
}