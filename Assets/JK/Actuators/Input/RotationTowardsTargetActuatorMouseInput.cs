using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Actuators.Input
{
    [DisallowMultipleComponent]
    public class RotationTowardsTargetActuatorMouseInput : RotationTowardsTargetActuatorInputBehaviour
    {
        #region Inspector

        public Camera camera;

        #endregion

        private void Update()
        {
            GetComponent<IRotationTowardsTargetActuator>().Target = camera.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
        }
    }
}