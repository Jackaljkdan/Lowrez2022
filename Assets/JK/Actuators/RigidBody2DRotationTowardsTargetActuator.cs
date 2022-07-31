using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;

namespace JK.Actuators
{
    [DisallowMultipleComponent]
    public class RigidBody2DRotationTowardsTargetActuator : MonoBehaviour, IRotationTowardsTargetActuator
    {
        #region Inspector

        [SerializeField]
        private float _speed = 3;

        public float dbgAngle;

        #endregion

        public float Speed
        {
            get => _speed;
            set => _speed = value;
        }

        [field: SerializeField]
        public Vector3 Target { get; set; }

        private void Start()
        {
            
        }

        private void FixedUpdate()
        {
            var body = GetComponent<Rigidbody2D>();

            Vector2 properTarget = new Vector2(Target.x, Target.y);

            Vector2 direction = properTarget - body.position;
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            dbgAngle = targetAngle;

            body.rotation = Mathf.LerpAngle(body.rotation, targetAngle, Speed);
        }
    }
}