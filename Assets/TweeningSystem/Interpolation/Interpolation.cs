// --~~~~======# Author : Lupon Dylan #======~~~~~~--- //
// --~~~~======# Date   : 03 / 15 / 2025 #======~~~~-- //

using System;
using UnityEngine;

namespace UnBocal.TweeningSystem.Interpolations
{
    public class Interpolation
    {
        // -------~~~~~~~~~~================# // Time
        public float Ratio => Mathf.Clamp01((Time.time - StartTime) / EndTimeRatio);

        private float StartTime;
        private float EndTimeRatio;
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
            EndTimeRatio = Duration + Delay;
        }

        public void Interpolate()
        {

        }
    }
}