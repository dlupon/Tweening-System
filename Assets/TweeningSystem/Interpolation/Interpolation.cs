// --~~~~======# Author : Lupon Dylan #======~~~~~~--- //
// --~~~~======# Date   : 03 / 15 / 2025 #======~~~~-- //

using System;
using UnityEngine;

namespace UnBocal.TweeningSystem.Interpolations
{
    public class Interpolation<ValueType>
    {
        // -------~~~~~~~~~~================# // Time
        public float Ratio => Mathf.Clamp01((Time.time - StartTime) / EndTimeRatio);

        private float StartTime;
        private float EndTimeRatio;
        public float Duration;
        public float Delay;

        // -------~~~~~~~~~~================# // Value
        public Func<float, ValueType> UpdatePropertyMethod;
        public ValueType CurrentValueOnRatio => UpdatePropertyMethod(Ratio);

        /// <summary>
        /// Reset time so the Ratio is at the right place.
        /// </summary>
        public void ResetTime()
        {   
            StartTime = Time.time + Delay;
            EndTimeRatio = Duration + Delay;
        }
    }
}