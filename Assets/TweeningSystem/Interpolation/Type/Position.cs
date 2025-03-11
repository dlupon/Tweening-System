// --~~~~======# Author : Lupon Dylan #======~~~~~~--- //
// --~~~~======# Date   : 03 / 05 / 2025 #======~~~~-- //
using UnityEngine;

namespace UnBocal.TweeningSystem.Interpolations
{
    public class Position : PropertyInterpolator<Vector3>
    {
        // -------~~~~~~~~~~================# // Interpolation
        protected override void UpdateInterpolation(Transform pObject)
        {
            Vector3 lDirection = m_currentInterpolation.EndValue - m_currentInterpolation.StartValue;
            pObject.position = m_currentInterpolation.StartValue + lDirection * m_currentInterpolation.EaseFunction(m_ratio);
        }
    }
}