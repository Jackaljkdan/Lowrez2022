using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Observables
{
    [Serializable]
    public class ObservableProperty<T>
    {
        [SerializeField]
        private T _value;

        public T Value
        {
            get => _value;
            set
            {
                if (ReferenceEquals(_value, value) || _value.Equals(value))
                    return;

                T old = _value;
                _value = value;

                onChange.Invoke(new Changed()
                {
                    old = old,
                    updated = value,
                });
            }
        }

        public struct Changed
        {
            public T old;
            public T updated;
        }

        public UnityEvent<Changed> onChange = new UnityEvent<Changed>();

        public void SetSilently(T value)
        {
            _value = value;
        }

        public override string ToString()
        {
            return _value?.ToString();
        }
    }
}