using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Actuators
{
    [DisallowMultipleComponent]
    public class MovementActuatorBehaviour : MonoBehaviour, IMovementActuator
    {
        #region Inspector

        [SerializeField]
        private float _speed = 3;

        public Transform _directionReference;

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
    }
}