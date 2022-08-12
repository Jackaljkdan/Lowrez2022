using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Actuators.Input
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(IMovementActuator))]
    public class MovementActuatorAxisInput : MovementActuatorInputBehaviour
    {
        #region Inspector

        #endregion

        private void Awake()
        {
            if (!PlatformUtils.IsDesktop)
                Destroy(this);
        }

        private void Update()
        {
            Vector3 input = new Vector3(
                UnityEngine.Input.GetAxis("Horizontal"),
                0,
                UnityEngine.Input.GetAxis("Vertical")
            );

            GetComponent<IMovementActuator>().Input = input;
        }

        private void OnDisable()
        {
            GetComponent<IMovementActuator>().Input = Vector3.zero;
        }
    }
}