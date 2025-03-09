// --~~~~======# Author : Lupon Dylan #======~~~~~~--- //
// --~~~~======# Date   : 03 / 08 / 2025 #======~~~~-- //
using UnityEngine;

namespace UnBocal.TweeningSystem.Interpolations
{
    public interface IInterpolator
    {
        // ----------------~~~~~~~~~~~~~~~~~~~==========================# // Time
        public bool IsFinished();

        // ----------------~~~~~~~~~~~~~~~~~~~==========================# // Interpolation
        public void Reset(Transform pObject);

        public void Interpolate(Transform pObject);
    }
}