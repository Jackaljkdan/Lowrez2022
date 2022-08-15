using JK.Injection;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace JK.Actuators.Input
{
    [DisallowMultipleComponent]
    public class RotationTowardsTargetActuatorMouseInput : RotationTowardsTargetActuatorInputBehaviour
    {
        #region Inspector

        public new Camera camera;

        #endregion

        //Text dbgText;

        //private void Awake()
        //{
        //    dbgText = Context.Find(this).Get<Text>("dbg");
        //}

        private void Update()
        {
            var mousePosition = UnityEngine.Input.mousePosition;
            var normalizedMousePosition = new Vector3(
                mousePosition.x / Screen.width - 0.5f,
                mousePosition.y / Screen.height - 0.5f
            );

            // TODO: fix con classe base
            var actuator = GetComponent<RigidBody2DRotationTowardsTargetActuator>();

            Vector3 worldPoint = actuator.transform.position + normalizedMousePosition * 10;

            //Debug.Log($"mp: {mousePosition:0.0} nmp: {normalizedMousePosition:0.0} wp: {worldPoint:0.0}");

            //if (dbgText)
            //    dbgText.text = $"m: {mousePosition:0.0}\nN: {normalizedMousePosition:0.0}\nw: {worldPoint:0.0}";

            GetComponent<IRotationTowardsTargetActuator>().Target = worldPoint;
        }
    }
}