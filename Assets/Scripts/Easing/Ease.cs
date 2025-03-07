// --~~~~======# Author : Lupon Dylan #======~~~~~~--- //
// --~~~~======# Date   : 03 / 05 / 2025 #======~~~~-- //
using UnityEngine;

namespace UnBocal.TweeningSystem.Easing
{
    public static class Ease
    {
        public static float Flat(float pRatio) => Mathf.Clamp01(pRatio);
    }
}