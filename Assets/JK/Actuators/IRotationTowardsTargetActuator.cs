using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Actuators
{
    public interface IRotationTowardsTargetActuator
    {
        float Speed { get; set; }

        Vector3 Target { get; set; }
    }
}