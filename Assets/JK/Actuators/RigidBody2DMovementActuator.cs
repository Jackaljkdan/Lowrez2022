using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Actuators
{
    [DisallowMultipleComponent]
    public class RigidBody2DMovementActuator : MonoBehaviour, IMovementActuator
    {
        #region Inspector

        [SerializeField]
        private float _speed = 3;

        public Transform _directionReference;

        public bool clampInput = true;

        [field: SerializeField, Header("Runtime")]
        public Vector3 Input { get; set; }

        [SerializeField]
        private UnityEvent<Vector3> _onMovement = new UnityEvent<Vector3>();

        private void Reset()
        {
            DirectionReference = transform;
        }

        #endregion

        public float Speed
        {
            get => _speed;
            set => _speed = value;
        }

        public Transform DirectionReference
        {
            get => _directionReference;
            set => _directionReference = value;
        }

        public UnityEvent<Vector3> onMovement => _onMovement;

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