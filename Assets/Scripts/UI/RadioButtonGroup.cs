using System;
using System.Linq;
using UnityEngine;

namespace UI
{
    public class RadioButtonGroup : MonoBehaviour
    {
        public RadioButton[] RadioButtons;
        public bool FallBack;

#pragma warning disable CS0414 // Field is assigned but its value is never used
        private int _currentOnRadioButtonIndex = -1;
#pragma warning restore CS0414 // Field is assigned but its value is never used

        private void Start()
        {
            if (RadioButtons.Length != 0)
            {
                var radioButtonsOnCount = 0;
                foreach (var radioButton in RadioButtons)
                    if (radioButton.IsOn)
                        radioButtonsOnCount++;

                if (radioButtonsOnCount == 1)
                    return;

                if (!FallBack)
                    throw new Exception("More than one RadioButton on the same RadioButtonGroup is on.");

                if (radioButtonsOnCount > 1)
                {
                    foreach (var radioButton in RadioButtons)
                    {
                        radioButton.IsOn = false;
                    }
                }

                RadioButtons[0].IsOn = true;
                _currentOnRadioButtonIndex = 0;
            }

            foreach (var radioButton in RadioButtons)
            {
                radioButton.ValueChanged.AddListener(OnRadioButtonValueChanged);
            }
        }

        private void OnRadioButtonValueChanged(RadioButton radioButton)
        {
            //TODO
        }
        
    }
}
