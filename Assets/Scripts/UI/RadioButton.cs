using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public class RadioButton : MonoBehaviour
    {

        private Toggle _toggle;

        public bool IsOn
        {
            get => _toggle.isOn; 
            set => _toggle.isOn = value;
        }

        public RadioButtonEvent ValueChanged = new();

        private void Awake()
        {
            _toggle = GetComponent<Toggle>();
            _toggle.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnValueChanged(bool isOn) => ValueChanged.Invoke(this);

        public class RadioButtonEvent : UnityEvent<RadioButton> {}
    }
}
