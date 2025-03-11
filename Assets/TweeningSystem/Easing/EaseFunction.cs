// --~~~~======# Author : Gallot Valentin #======~~~~~~--- //
// --~~~~======# Date   : 03 / 05 / 2025  #======~~~~~~--- //

using UnityEngine;

namespace UnBocal.TweeningSystem.Easing
{
    public static class EaseFunction
    {
        private const float c1 = 1.70158f;
        private const float c2 = c1 * 1.525f;
        private const float c3 = c1 + 1;
        private const float c4 = 2 * Mathf.PI / 3;
        private const float c5 = 2 * Mathf.PI / 4.5f;
        private const float n1 = 7.5625f;
        private const float d1 = 2.75f;

        public static float Flat(float pRatio) => pRatio;

        #region easeIns
        public static float InSin(float pX)
        {
            return 1 - Mathf.Cos(pX * Mathf.PI / 2);
        }

        public static float InCubic(float pX)
        {
            return pX * pX * pX;
        }

        public static float InCirc(float pX)
        {
            return 1 - Mathf.Sqrt(1 - Mathf.Pow(pX, 2));
        }

        public static float InQuint(float pX)
        {
            return pX * pX * pX * pX * pX;
        }

        /// <summary>
        /// values returned can be lower than 0
        /// </summary>
        /// <param name="pX"></param>
        /// <returns></returns>
        public static float InElastic(float pX)
        {
            return pX == 0 ? 0 : pX == 1 ? 1 : -Mathf.Pow(2, 10 * pX - 10) * Mathf.Sin((pX * 10f - 10.75f) * c4);
        }

        public static float InQuad(float pX)
        {
            return pX * pX;
        }

        public static float InQuart(float pX)
        {
            return pX * pX * pX * pX;
        }

        public static float InExpo(float pX)
        {
            return Mathf.Exp(pX);
        }

        /// <summary>
        /// values returned can be lower than 0
        /// </summary>
        /// <param name="pX"></param>
        /// <returns></returns>
        public static float InBack(float pX)
        {
            return c3 * pX * pX * pX - c1 * pX * pX;
        }

        public static float InBounce(float pX)
        {
            return 1 - OutBounce(1 - pX);
        }
        #endregion

        #region EaseOuts
        public static float OutSin(float pX)
        {
            return Mathf.Sin(pX * Mathf.PI / 2);
        }

        public static float OutCubic(float pX)
        {
            return 1 - Mathf.Pow(1 - pX, 3);
        }

        public static float OutCirc(float pX)
        {
            return Mathf.Sqrt(1 - Mathf.Pow(pX - 1, 2));
        }

        public static float OutQuint(float pX)
        {
            return 1 - Mathf.Pow(1 - pX, 5);
        }

        /// <summary>
        /// values returned can be higher than 1
        /// </summary>
        /// <param name="pX"></param>
        /// <returns></returns>
        public static float OutElastic(float pX)
        {
            return pX == 0 ? 0 : pX == 1 ? 1 : Mathf.Pow(2, -10 * pX) * Mathf.Sin((pX * 10 - 0.75f) * c4) + 1;
        }

        public static float OutQuad(float pX)
        {
            return 1 - Mathf.Pow(1 - pX, 2);
        }

        public static float OutQuart(float pX)
        {
            return 1 - Mathf.Pow(1 - pX, 4);
        }

        public static float OutExpo(float pX)
        {
            return pX == 1 ? 1 : 1 - Mathf.Pow(2, -10 * pX);
        }

        /// <summary>
        /// values returned can be higher than 1
        /// </summary>
        /// <param name="pX"></param>
        /// <returns></returns>
        public static float OutBack(float pX)
        {
            return 1 + c3 * Mathf.Pow(pX - 1, 3) + c1 * Mathf.Pow(pX - 1, 2);
        }

        public static float OutBounce(float pX)
        {
            if (pX < 1 / d1)
            {
                return n1 * pX * pX;
            }
            else if (pX < 2 / d1)
            {
                return n1 * (pX -= 1.5f / d1) * pX + 0.75f;
            }
            else if (pX < 2.5f / d1)
            {
                return n1 * (pX -= 2.25f / d1) * pX + 0.9375f;
            }
            else
            {
                return n1 * (pX -= 2.625f / d1) * pX + 0.984375f;
            }
        }
        #endregion

        #region EaseInOuts
        public static float InOutSin(float pX)
        {
            return -(Mathf.Cos(Mathf.PI * pX) - 1) / 2;
        }

        public static float InOutCubic(float pX)
        {
            return pX < 0.5 ? 4 * pX * pX * pX : 1 - Mathf.Pow(-2 * pX + 2, 3) / 2;
        }

        public static float InOutCirc(float pX)
        {
            return pX < 0.5f ? (1 - Mathf.Sqrt(1 - Mathf.Pow(2 * pX, 2))) / 2 : (Mathf.Sqrt(1 - Mathf.Pow(-2 * pX + 2, 2)) + 1) / 2;
        }

        public static float InOutQuint(float pX)
        {
            return pX < 0.5f ? 16 * pX * pX * pX * pX * pX : 1 - Mathf.Pow(-2 * pX + 2, 5) / 2;
        }

        /// <summary>
        /// values returned can be lower than 0 or higher than 1
        /// </summary>
        /// <param name="pX"></param>
        /// <returns></returns>
        public static float InOutElastic(float pX)
        {
            return pX == 0 ? 0 : pX == 1 ? 1 : pX < 0.5f ? -(Mathf.Pow(2, 20 * pX - 10) * Mathf.Sin((20 * pX - 11.125f) * c5)) / 2 : Mathf.Pow(2, -20 * pX + 10) * Mathf.Sin((20 * pX - 11.125f) * c5) / 2 + 1;
        }

        public static float InOutQuad(float pX)
        {
            return pX < 0.5f ? 2 * pX * pX : 1 - Mathf.Pow(-2 * pX + 2, 2) / 2;
        }

        public static float InOutQuart(float pX)
        {
            return pX < 0.5f ? 8 * pX * pX * pX * pX : 1 - Mathf.Pow(-2 * pX + 2, 4) / 2;
        }

        public static float InOutExpo(float pX)
        {
            return pX == 0 ? 0 : pX == 1 ? 1 : pX < 0.5f ? Mathf.Pow(2, 20 * pX - 10) / 2 : (2 - Mathf.Pow(2, -20 * pX + 10)) / 2;
        }

        /// <summary>
        /// values returned can be lower than 0 or higher than 1
        /// </summary>
        /// <param name="pX"></param>
        /// <returns></returns>
        public static float InOutBack(float pX)
        {
            return pX < 0.5f ? Mathf.Pow(2 * pX, 2) * ((c2 + 1) * 2 * pX - c2) / 2 : (Mathf.Pow(2 * pX - 2, 2) * ((c2 + 1) * (pX * 2 - 2) + c2) + 2) / 2;
        }

        public static float InOutBounce(float pX)
        {
            return pX < 0.5f ? (1 - OutBounce(1 - 2 * pX)) / 2 : (1 + OutBounce(2 * pX - 1)) / 2;
        }
        #endregion

    }
}