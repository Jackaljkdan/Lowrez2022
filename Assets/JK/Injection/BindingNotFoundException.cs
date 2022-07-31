using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Injection
{
    public class BindingNotFoundException : ArgumentException
    {
        public BindingNotFoundException(Context context, Type type, string id) : base($"Binding of type {type} with id {id} was not found in context {context.name}") { }
    }
}