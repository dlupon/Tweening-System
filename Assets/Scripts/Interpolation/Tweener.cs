// --~~~~======# Author : Lupon Dylan #======~~~~~~--- //
// --~~~~======# Date   : 03 / 05 / 2025 #======~~~~-- //
using UnityEngine;
using UnBocal.TweeningSystem.Easing;

namespace UnBocal.TweeningSystem
{
    public abstract class Tweener
    {
        // -------~~~~~~~~~~================# // Ease
        protected System.Func<float, float> m_easeFunction;

        // -------~~~~~~~~~~================# // Time
        protected bool m_delayEnded => (m_endTime - m_startTime) > m_delay;
        protected float m_ratio => (Time.time - m_startTime - m_delay) / m_duration;

        protected float m_delay;
        protected float m_duration;
        protected float m_startTime;
        protected float m_endTime;

        // ----------------~~~~~~~~~~~~~~~~~~~==========================# // Initialization
        public Tweener(float pDuration = 1f, System.Func<float, float> pEaseFunction = default, float pDelay = 0f)
        {
            m_duration = pDuration;
            m_delay = pDelay;
            m_startTime = m_delay + Time.time;
            m_endTime = m_startTime + m_duration;

            m_easeFunction = pEaseFunction == default ? Ease.Flat : pEaseFunction;
        }
        
        // ----------------~~~~~~~~~~~~~~~~~~~==========================# // Check Type
        private void CheckTypeAndGetInterpolation()
        {
            
        }

        // ----------------~~~~~~~~~~~~~~~~~~~==========================# // Reset
        public void Reset(Transform pObject)
        {

        }

        // ----------------~~~~~~~~~~~~~~~~~~~==========================# // Interpolate
        public abstract void Interpolate(Transform pObject);

        protected abstract void UpdateInterpolation(Transform pObject);
    }
}