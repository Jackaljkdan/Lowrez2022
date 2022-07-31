using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Actuators
{
    public interface IMovementActuator
    {
        float Speed { get; set; }
        Vector3 Input { get; set; }
        Transform DirectionReference { get; set; }
        UnityEvent<Vector3> onMovement { get; }
    }
}