// --~~~~======# Author : Lupon Dylan #======~~~~~~--- //
// --~~~~======# Date   : 03 / 08 / 2025 #======~~~~-- //
using UnityEngine;

namespace UnBocal.TweeningSystem.Interpolations
{
    public class RotationFast : PropertyInterpolator<Quaternion>
    {
        // ----------------~~~~~~~~~~~~~~~~~~~==========================# // Interpolate
        protected override void UpdateInterpolation(Transform pObject)
        {
            pObject.rotation = Quaternion.LerpUnclamped(m_currentInterpolation.StartValue, m_currentInterpolation.EndValue, m_currentInterpolation.EaseFunction(m_ratio));
        }
    }
}