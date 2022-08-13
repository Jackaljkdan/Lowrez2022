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

        public new Camera camera;

        #endregion

        private void Update()
        {
            var mousePosition = UnityEngine.Input.mousePosition;
            var normalizedMousePosition = new Vector3(
                mousePosition.x / Screen.width - 0.5f,
                mousePosition.y / Screen.height - 0.5f
            );

            // TODO: fix con classe base
            var actuator = GetComponent<RigidBody2DRotationTowardsTargetActuator>();

            Vector3 worldPoint = actuator.transform.position + normalizedMousePosition;

            //Debug.Log($"mp: {mousePosition:0.0} nmp: {normalizedMousePosition:0.0} wp: {worldPoint:0.0}");

            GetComponent<IRotationTowardsTargetActuator>().Target = worldPoint;
        }
    }
}