// --~~~~======# Author : Lupon Dylan #======~~~~~~--- //
// --~~~~======# Date   : 03 / 08 / 2025 #======~~~~-- //

using System;
using UnBocal.TweeningSystem.Interpolations;

namespace UnBocal.TweeningSystem.Interfaces
{
    public interface IInterpolator
    {
        public string PropertyName { get; set; }

        // ----------------~~~~~~~~~~~~~~~~~~~==========================# // Interpolation
        public bool IsFinished();

        public void Reset();

        public void Start();

        public void Update();
    }
}