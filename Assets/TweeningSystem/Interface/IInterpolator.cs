// --~~~~======# Author : Lupon Dylan #======~~~~~~--- //
// --~~~~======# Date   : 03 / 08 / 2025 #======~~~~-- //

using UnityEngine;

namespace UnBocal.TweeningSystem.Interfaces
{
    public interface IInterpolator
    {
        // ----------------~~~~~~~~~~~~~~~~~~~==========================# // Initialization

        // ----------------~~~~~~~~~~~~~~~~~~~==========================# // Time
        public bool IsFinished();

        // ----------------~~~~~~~~~~~~~~~~~~~==========================# // Interpolation
        public void Reset(Transform pObject);

        public void Interpolate(Transform pObject);

        // public void Add<ValueType>(ValueType pStartValue = default, ValueType pEndValue = default, float pDuration = 1, Func<float, float> pEase = default, float pDelay = 0f);
    }
}