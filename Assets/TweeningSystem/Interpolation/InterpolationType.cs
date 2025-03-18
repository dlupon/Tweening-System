// --~~~~======# Author : Lupon Dylan #======~~~~~~--- //
// --~~~~======# Date   : 03 / 11 / 2025 #======~~~~-- //

using UnityEngine;

namespace UnBocal.TweeningSystem.Interpolations
{
    public static class InterpolationType
    {
        public static Vector3 InterpolateVector3(Vector3 pStart, Vector3 pEnd, float pRatio)
            => Vector3.LerpUnclamped(pStart, pEnd, pRatio);

        public static float InterpolateSingle(float pStart, float pEnd, float pRatio)
            => Mathf.LerpUnclamped(pStart, pEnd, pRatio);

        public static int InterpolateInt32(int pStart, int pEnd, float pRatio)
            => Mathf.RoundToInt(Mathf.LerpUnclamped(pStart, pEnd, pRatio));

        public static Quaternion InterpolateQuaternion(Quaternion pStart, Quaternion pEnd, float pRatio)
            => Quaternion.LerpUnclamped(pStart, pEnd, pRatio);
    }
}