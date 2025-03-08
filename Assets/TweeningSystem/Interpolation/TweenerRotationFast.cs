// --~~~~======# Author : Lupon Dylan #======~~~~~~--- //
// --~~~~======# Date   : 03 / 08 / 2025 #======~~~~-- //
using UnityEngine;

namespace UnBocal.TweeningSystem
{
    public class TweenerRotationFast : Tweener
    {
        // -------~~~~~~~~~~================# // Rotation
        Quaternion _startRotation;
        Quaternion _endRotation;

        // ----------------~~~~~~~~~~~~~~~~~~~==========================# // Initialization
        public TweenerRotationFast(Quaternion pStartRotation, Quaternion pEndRotation, float pDuration = 1f, System.Func<float, float> pEaseFunction = default, float pDelay = 0f) : base(pDuration, pEaseFunction, pDelay)
        {
            _startRotation = pStartRotation;
            _endRotation = pEndRotation;
        }

        // ----------------~~~~~~~~~~~~~~~~~~~==========================# // Interpolate
        public override void Interpolate(Transform pObject)
        {
            if (!m_delayEnded) return;
            UpdateInterpolation(pObject);
        }

        protected override void UpdateInterpolation(Transform pObject)
        {
            pObject.rotation = Quaternion.LerpUnclamped(_startRotation, _endRotation, m_easeFunction(m_ratio));
        }
    }

    public class TweenerRotationAround : Tweener
    {
        // -------~~~~~~~~~~================# // Rotation
        Quaternion _startRotation;
        Quaternion _endRotation;

        // ----------------~~~~~~~~~~~~~~~~~~~==========================# // Initialization
        public TweenerRotationAround(Quaternion pStartRotation, Quaternion pEndRotation, float pDuration = 1f, System.Func<float, float> pEaseFunction = default, float pDelay = 0f) : base(pDuration, pEaseFunction, pDelay)
        {
            _startRotation = pStartRotation;
            _endRotation = pEndRotation;
        }

        // ----------------~~~~~~~~~~~~~~~~~~~==========================# // Interpolate
        public override void Interpolate(Transform pObject)
        {
            if (!m_delayEnded) return;
            UpdateInterpolation(pObject);
        }

        protected override void UpdateInterpolation(Transform pObject)
        {
            pObject.rotation = Quaternion.LerpUnclamped(_startRotation, _endRotation, m_easeFunction(m_ratio));
        }
    }
}