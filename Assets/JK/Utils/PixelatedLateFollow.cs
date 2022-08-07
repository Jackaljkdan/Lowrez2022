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

        public int unit = 1;

        #endregion

        private void LateUpdate()
        {
            transform.position = new Vector3(
                Pixelate(target.position.x),
                Pixelate(target.position.y),
                Pixelate(target.position.z)
            );
        }

        private float Pixelate(float value)
        {
            int floor = Mathf.FloorToInt(value);
            int mod = floor % unit;
            return floor - mod;
        }
    }
}