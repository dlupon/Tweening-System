// --~~~~======# Author : Lupon Dylan #======~~~~~~--- //
// --~~~~======# Date   : 03 / 15 / 2025 #======~~~~-- //

using System;
using TMPro;
using UnityEngine;

namespace UnBocal.TweeningSystem.Interpolations
{
    public class Interpolation
    {
        // -------~~~~~~~~~~================# // Time
        public bool IsFinished => Update == null;
        public float Ratio => Mathf.Clamp01((Time.time - StartTime) / (EndTime - StartTime));

        private float StartTime;
        private float EndTime;
        public float Duration;
        public float Delay;

        // -------~~~~~~~~~~================# // Value
        public Action<float> InterpolationMethod;

        // -------~~~~~~~~~~================# // Interpolation
        public Action Update;

        // -------~~~~~~~~~~================# // Initialization
        public Interpolation() => Update = UpdateWait;

        // ----------------~~~~~~~~~~~~~~~~~~~==========================# // Time
        /// <summary>
        /// Reset time so the Ratio is at the right place.
        /// </summary>
        public void Start()
        {
            StartTime = Time.time + Delay;
            EndTime = StartTime + Duration;

            if (Delay > 0) Update = UpdateWait;
            else Update = UpdateInterpolation;
        }

        // ----------------~~~~~~~~~~~~~~~~~~~==========================# // Interpolation
        private void UpdateWait()
        {
            if (Time.time < StartTime) return;
            Update = UpdateInterpolation;
        }

        private void UpdateInterpolation()
        {
            InterpolationMethod(Ratio);
            if (Ratio >= 1) Update = null; ;
        }
    }
}