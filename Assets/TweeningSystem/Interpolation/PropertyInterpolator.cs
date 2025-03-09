// --~~~~======# Author : Lupon Dylan #======~~~~~~--- //
// --~~~~======# Date   : 03 / 05 / 2025 #======~~~~-- //
using System.Collections.Generic;
using UnityEngine;

namespace UnBocal.TweeningSystem.Interpolations
{
    public abstract class PropertyInterpolator<ValueType> : IInterpolator
    {
        // -------~~~~~~~~~~================# // Interpolation
        private List<Interpolation<ValueType>> _interpolations = new List<Interpolation<ValueType>>();
        private Interpolation<ValueType> _nextInterpolation;
        private int _currentInterpolationIndex = 0;
        private bool _isOnLastInterpolation = false;
        
        protected Interpolation<ValueType> m_currentInterpolation;
        protected float m_startTime;

        // -------~~~~~~~~~~================# // Time
        protected bool m_delayEnded => (Time.time - m_startTime) >= 0;
        protected float m_ratio => Mathf.Clamp01((Time.time - m_startTime) / m_currentInterpolation.Duration);
        private float _timeSinceInterpolationStart => Time.time - _overAllStartTime;

        private float _overAllStartTime;

        // ----------------~~~~~~~~~~~~~~~~~~~==========================# // Interpolations
        private bool SetInterpolation(int pIndex)
        {
            if (_interpolations.Count <= 0) return false;
            // Store Current Interpolation
            _currentInterpolationIndex = Mathf.Clamp(pIndex, 0, _interpolations.Count - 1);
            m_currentInterpolation = _interpolations[_currentInterpolationIndex];

            // Try to Store Next Interpolation
            _isOnLastInterpolation = _currentInterpolationIndex >= _interpolations.Count - 1;
            _nextInterpolation = _isOnLastInterpolation ? m_currentInterpolation : _interpolations[_currentInterpolationIndex + 1];

            UpdateInterpolationTime();
            return true;
        }

        private bool NextInterpolation() => SetInterpolation(_currentInterpolationIndex + 1);

        private void CheckNextInterpolation()
        {
            if (_isOnLastInterpolation) return;
            if (_timeSinceInterpolationStart <= _nextInterpolation.Delay) return;
            NextInterpolation();
        }

        private void UpdateInterpolationTime()
        {
            m_startTime = Time.time;
        }

        public void AddInterpolation(Interpolation<ValueType> pInterpolation)
        {
            int lInterpolationCount = _interpolations.Count;
            for (int lInterpolationIndex = lInterpolationCount -1; lInterpolationIndex >= 0; lInterpolationCount--)
            {
                // Placing the New Interpolation Based On The Delay Length
                if (_interpolations[lInterpolationIndex].Delay > pInterpolation.Delay) continue;
                _interpolations.Insert(lInterpolationIndex + 1, pInterpolation);
                return;
            }

            _interpolations.Add(pInterpolation);
        }

        private void ChackAndSwitchInterpolation()
        {
            int lInterpolationCount = _interpolations.Count;
            int lInterpolationIndex = 0;
            float lTimeSinceStart = _timeSinceInterpolationStart;

            // Find The Interpolation That Should Be Played
            for (lInterpolationIndex = lInterpolationCount - 1; lInterpolationIndex >= 0; lInterpolationIndex--)
                if (_interpolations[lInterpolationIndex].Delay <= lTimeSinceStart) break;

            // Start Playing
            if (_currentInterpolationIndex == lInterpolationIndex) return;
            SetInterpolation(lInterpolationIndex);
        }

        // ----------------~~~~~~~~~~~~~~~~~~~==========================# // Time
        public bool IsFinished() => _isOnLastInterpolation && m_ratio >= 1;

        // ----------------~~~~~~~~~~~~~~~~~~~==========================# // Interpolate
        public void Reset(Transform pObject)
        {
            if (_interpolations.Count <= 0) return;
            SetInterpolation(0);
            m_startTime += _interpolations[0].Delay;
            _overAllStartTime = Time.time;
        }

        public void Interpolate(Transform pObject)
        {
            if (IsFinished()) return;
            CheckNextInterpolation();
            UpdateInterpolation(pObject);
        }

        protected abstract void UpdateInterpolation(Transform pObject);
    }
}