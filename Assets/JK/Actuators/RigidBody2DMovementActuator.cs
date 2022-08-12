using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Actuators
{
    [DisallowMultipleComponent]
    public class RigidBody2DMovementActuator : MovementActuatorBehaviour
    {
        #region Inspector

        public bool clampInput = true;

        #endregion

        private void Start()
        {
            
        }

        private void FixedUpdate()
        {
            var body = GetComponent<Rigidbody2D>();

            Vector2 properInput = new Vector2(Input.x, Input.z);

            Vector2 processedInput = clampInput
                ? Vector2.ClampMagnitude(properInput, 1)
                : properInput
            ;

            body.velocity = DirectionReference.TransformDirection(processedInput) * Speed;

            if (Input.sqrMagnitude > 0)
                onMovement.Invoke(processedInput);
        }
    }
}