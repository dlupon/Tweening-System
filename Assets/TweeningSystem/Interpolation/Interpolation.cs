// --~~~~======# Author : Lupon Dylan #======~~~~~~--- //
// --~~~~======# Date   : 03 / 15 / 2025 #======~~~~-- //

using System;
using UnityEngine;

namespace UnBocal.TweeningSystem.Interpolations
{
    public class Interpolation
    {
        // -------~~~~~~~~~~================# // Time
        public float Ratio => Mathf.Clamp01((Time.time - StartTime) / (EndTime - StartTime));

        private float StartTime;
        private float EndTime;
        public float Duration;
        public float Delay;

        // -------~~~~~~~~~~================# // Value
        public Action<float> InterpolationMethod;

        /// <summary>
        /// Reset time so the Ratio is at the right place.
        /// </summary>
        public void ResetTime()
        {
            StartTime = Time.time + Delay;
            EndTime = StartTime + Duration;
        }
    }
}