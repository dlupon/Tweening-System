// --~~~~======# Author : Lupon Dylan #======~~~~~~--- //
// --~~~~======# Date   : 03 / 08 / 2025 #======~~~~-- //

using System;
using UnityEngine;
using System.Collections.Generic;

namespace UnBocal.TweeningSystem.Interpolations
{
    public class Interpolator
    {
        // -------~~~~~~~~~~================# // Time
        public bool IsFinished => _updateState == null;
        private float _timeSinceStart;

        // -------~~~~~~~~~~================# // State
        private Action _updateState = null;

        // -------~~~~~~~~~~================# // Interpolation
        private List<Interpolation> _interpolations = new List<Interpolation>();
        private Interpolation _currentInterpolation;
        private Interpolation _nextInterpolation;
        private int _currentInterpolationIndex = 0;
        private bool _isOnLastInterpolation = false;

        // ----------------~~~~~~~~~~~~~~~~~~~==========================# // Interpolation
        /// <summary>
        /// Add a new inteprolation the interpolation list.
        /// </summary>
        /// <param name="pInterpolation">Method used to update the property.</param>
        /// <param name="pObject">Targeted object.</param>
        /// <param name="pPropertyName">Object property name.</param>
        /// <param name="pDuration">Overall duration of the inteprolation (in seconds).</param>
        /// <param name="pDelay">How much the interpolation is delayed (in seconds).</param>
        public void Add(Action<float> pInterpolation, float pDuration, float pDelay)
        {
            Interpolation lInterpolation = new Interpolation();
            lInterpolation.InterpolationMethod = pInterpolation;
            lInterpolation.Duration = pDuration;
            lInterpolation.Delay = pDelay;

            int lInterpolationCount = _interpolations.Count;

            for (int lInterpolationIndex = lInterpolationCount - 1; lInterpolationIndex >= 0; lInterpolationCount--)
            {
                // Placing the New Interpolation Based On The Delay Length
                if (_interpolations[lInterpolationIndex].Delay > lInterpolation.Delay) continue;
                _interpolations.Insert(lInterpolationIndex + 1, lInterpolation);
                return;
            }

            _interpolations.Add(lInterpolation);
        }

        /// <summary>
        /// Update the current interpolation to be the one at the given index and update it.
        /// </summary>
        /// <param name="pIndex">Interpolation's index.</param>
        /// <returns>Is the inteprolation has been changed.</returns>
        private bool SetInterpolation(int pIndex)
        {
            if (_interpolations.Count <= 0) return false;
            // Store Current Interpolation
            _currentInterpolationIndex = Mathf.Clamp(pIndex, 0, _interpolations.Count - 1);
            _currentInterpolation = _interpolations[_currentInterpolationIndex];
            _currentInterpolation.ResetTime();

            // Try to Store Next Interpolation
            _isOnLastInterpolation = _currentInterpolationIndex >= _interpolations.Count - 1;
            _nextInterpolation = _isOnLastInterpolation ? _currentInterpolation : _interpolations[_currentInterpolationIndex + 1];

            return true;
        }

        /// <summary>
        /// Determine if the interpolation should be changed and if so change it.
        /// </summary>
        private void CheckAndSwitchInterpolation()
        {
            float lTimeSinceStart = Time.time - _timeSinceStart;
            int lInterpolationCount = _interpolations.Count;
            int lInterpolationIndex;

            // Find The Interpolation That Should Be Played Based On The Delay
            for (lInterpolationIndex = lInterpolationCount - 1; lInterpolationIndex >= 0; lInterpolationIndex--)
                if (_interpolations[lInterpolationIndex].Delay <= lTimeSinceStart) break;

            // Is The Founded Inteprolation Is The Same As The One That Is Playing Right Now ?
            if (_currentInterpolationIndex == lInterpolationIndex) return;

            // Is The Current Interpolation Is Not The Last One ?
            else if (_currentInterpolationIndex < lInterpolationCount - 1) SetInterpolation(lInterpolationIndex);
        }

        // ----------------~~~~~~~~~~~~~~~~~~~==========================# // 
        public void Reset() { }

        /// <summary>
        /// Set the property to the default value and start updating
        /// </summary>
        public void Start()
        {
            _timeSinceStart = Time.time;
            SetInterpolation(0);
            SetState(UpdateProperty);
        }

        /// <summary>
        /// Update the current state (it could be <c>UpdateProperty</c>, or <c>null</c>).
        /// </summary>
        public void Update() => _updateState?.Invoke();
        // -------~~~~~~~~~~================# // States

        private void SetState(Action pState) => _updateState = pState;

        /// <summary>
        /// Update the property based on the current inpolation and check if it needs to change the interpolation.
        /// </summary>
        public void UpdateProperty()
        {
            _currentInterpolation.InterpolationMethod(_currentInterpolation.Ratio);
            CheckAndSwitchInterpolation();
        }
    }
}