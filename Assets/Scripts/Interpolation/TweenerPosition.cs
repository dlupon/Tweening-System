// --~~~~======# Author : Lupon Dylan #======~~~~~~--- //
// --~~~~======# Date   : 03 / 05 / 2025 #======~~~~-- //
using UnityEngine;

namespace UnBocal.TweeningSystem
{
    public class TweenerPosition : Tweener
    {
        // -------~~~~~~~~~~================# // Position
        Vector3 _startPosition;
        Vector3 _endPosition;

        // ----------------~~~~~~~~~~~~~~~~~~~==========================# // Initialization
        public TweenerPosition(Vector3 pStartPosition, Vector3 pEndPosition, float pDuration = 1f, System.Func<float, float> pEaseFunction = default, float pDelay = 0f) : base(pDuration, pEaseFunction, pDelay)
        {
            _startPosition = pStartPosition;
            _endPosition = pEndPosition;
        }
        

        // ----------------~~~~~~~~~~~~~~~~~~~==========================# // Interpolate
        public override void Interpolate(Transform pObject)
        {
            if (!m_delayEnded) return;
            UpdateInterpolation(pObject);
        }

        protected override void UpdateInterpolation(Transform pObject)
        {
            Debug.Log(m_ratio);

            Vector3 lDirection = _endPosition - _startPosition;
            pObject.position = _startPosition + lDirection * m_easeFunction(m_ratio);
        }
    }
}