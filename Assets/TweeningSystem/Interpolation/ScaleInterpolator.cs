// --~~~~======# Author : Lupon Dylan #======~~~~~~--- //
// --~~~~======# Date   : 03 / 05 / 2025 #======~~~~-- //
using UnityEngine;

namespace UnBocal.TweeningSystem.Interpolations
{
    public class ScaleInterpolator : PropertyInterpolator<Vector3>
    {
        // ----------------~~~~~~~~~~~~~~~~~~~==========================# // Interpolate
        protected override void UpdateInterpolation(Transform pObject)
        {
            Vector3 lScaleDifference = m_currentInterpolation.EndValue - m_currentInterpolation.StartValue;
            pObject.localScale = m_currentInterpolation.StartValue + lScaleDifference * m_currentInterpolation.EaseFunction(m_ratio);
        }
    }
}