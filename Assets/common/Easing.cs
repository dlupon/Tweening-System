using System.Diagnostics.Tracing;
using UnityEngine;

/// <summary>
/// <para>Functions found on easings.net .</para>
/// 
/// Static class of easing functions, returning values between 0 and 1 (exept for elastic and back easings);
/// </summary>
public static class Easing
{
    public enum EaseType
    {
        None,
        EaseInSin,
        EaseInCubic,
        EaseInQuad,
        EaseInQuart,
        EaseInQuint,
        EaseInCirc,
        EaseInElastic,
        EaseInBack,
        EaseInBounce,
        EaseInExpo,
        EaseOutSin,
        EaseOutCubic,
        EaseOutQuad,
        EaseOutQuart,
        EaseOutQuint,
        EaseOutCirc,
        EaseOutElastic,
        EaseOutBack,
        EaseOutBounce,
        EaseOutExpo,
        EaseInOutSin,
        EaseInOutCubic,
        EaseInOutQuad,
        EaseInOutQuart,
        EaseInOutQuint,
        EaseInOutCirc,
        EaseInOutElastic,
        EaseInOutBack,
        EaseInOutBounce,
        EaseInOutExpo,
    }

    private const float c1 = 1.70158f;
    private const float c2 = c1 * 1.525f;
    private const float c3 = c1 + 1;
    private const float c4 = 2 * Mathf.PI / 3;
    private const float c5 = 2 * Mathf.PI / 4.5f;
    private const float n1 = 7.5625f;
    private const float d1 = 2.75f;

    #region easeIns
    public static float EaseInSin(float pX)
    {
        return 1 - Mathf.Cos(pX * Mathf.PI / 2) ;
    }

    public static float EaseInCubic(float pX)
    {
        return pX*pX*pX ;
    }

    public static float EaseInCirc(float pX)
    {
        return 1 - Mathf.Sqrt(1 - Mathf.Pow(pX, 2)) ;
    }

    public static float EaseInQuint(float pX)
    {
        return pX*pX*pX*pX*pX ;
    }

    /// <summary>
    /// values returned can be lower than 0
    /// </summary>
    /// <param name="pX"></param>
    /// <returns></returns>
    public static float EaseInElastic(float pX)
    {
        return pX == 0 ? 0 : pX==1 ? 1 : -Mathf.Pow(2, 10 * pX - 10) * Mathf.Sin((pX * 10f - 10.75f) * c4);
    }

    public static float EaseInQuad(float pX)
    {
        return pX*pX ;
    }

    public static float EaseInQuart(float pX)
    {
        return pX*pX*pX*pX ;
    }

    public static float EaseInExpo(float pX)
    {
        return Mathf.Exp(pX) ;
    }

    /// <summary>
    /// values returned can be lower than 0
    /// </summary>
    /// <param name="pX"></param>
    /// <returns></returns>
    public static float EaseInBack(float pX)
    {
        return c3 * pX * pX * pX - c1 * pX * pX;
    }

    public static float EaseInBounce(float pX)
    {
        return 1 - EaseOutBounce(1 - pX) ;
    }
    #endregion

    #region EaseOuts
    public static float EaseOutSin(float pX)
    {
        return Mathf.Sin(pX * Mathf.PI / 2) ;
    }

    public static float EaseOutCubic(float pX)
    {
        return 1 - Mathf.Pow(1 - pX, 3) ;
    }

    public static float EaseOutCirc(float pX)
    {
        return Mathf.Sqrt(1 - Mathf.Pow(pX - 1, 2)) ;
    }

    public static float EaseOutQuint(float pX)
    {
        return 1 - Mathf.Pow(1 - pX, 5) ;
    }

    /// <summary>
    /// values returned can be higher than 1
    /// </summary>
    /// <param name="pX"></param>
    /// <returns></returns>
    public static float EaseOutElastic(float pX)
    {
        return pX == 0 ? 0 : pX==1 ? 1 : Mathf.Pow(2, -10 * pX) * Mathf.Sin((pX * 10 - 0.75f) * c4) + 1 ;
    }

    public static float EaseOutQuad(float pX)
    {
        return 1 - Mathf.Pow(1 - pX, 2) ;
    }

    public static float EaseOutQuart(float pX)
    {
        return 1 - Mathf.Pow(1 - pX, 4) ;
    }

    public static float EaseOutExpo(float pX)
    {
        return pX == 1 ? 1 : 1 - Mathf.Pow(2, -10 * pX) ;
    }

    /// <summary>
    /// values returned can be higher than 1
    /// </summary>
    /// <param name="pX"></param>
    /// <returns></returns>
    public static float EaseOutBack(float pX)
    {
        return 1 + c3 * Mathf.Pow(pX - 1, 3) + c1 * Mathf.Pow(pX - 1, 2);
    }

    public static float EaseOutBounce(float pX)
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
    public static float EaseInOutSin(float pX)
    {
        return -(Mathf.Cos(Mathf.PI * pX) - 1) / 2 ;
    }

    public static float EaseInOutCubic(float pX)
    {
        return pX < 0.5 ? 4 * pX * pX * pX : 1 - Mathf.Pow(-2 * pX + 2, 3) / 2;
    }

    public static float EaseInOutCirc(float pX)
    {
        return pX < 0.5f ? (1 - Mathf.Sqrt(1 - Mathf.Pow(2 * pX, 2))) / 2 : (Mathf.Sqrt(1 - Mathf.Pow(-2 * pX + 2, 2)) + 1) / 2 ;
    }

    public static float EaseInOutQuint(float pX)
    {
        return pX < 0.5f ? 16 * pX * pX * pX * pX * pX : 1 - Mathf.Pow(-2 * pX + 2, 5) / 2 ;
    }

    /// <summary>
    /// values returned can be lower than 0 or higher than 1
    /// </summary>
    /// <param name="pX"></param>
    /// <returns></returns>
    public static float EaseInOutElastic(float pX)
    {
        return pX == 0 ? 0 : pX == 1 ? 1 : pX < 0.5f ? -(Mathf.Pow(2, 20 * pX - 10) * Mathf.Sin((20 * pX - 11.125f) * c5)) / 2 : Mathf.Pow(2, -20 *pX + 10) * Mathf.Sin((20 * pX - 11.125f) * c5) / 2 + 1;
    }

    public static float EaseInOutQuad(float pX)
    {
        return pX < 0.5f ? 2 * pX * pX : 1 - Mathf.Pow(-2 * pX + 2, 2) / 2 ;
    }

    public static float EaseInOutQuart(float pX)
    {
        return pX < 0.5f ? 8 * pX * pX * pX * pX : 1 - Mathf.Pow(-2 * pX + 2, 4) / 2 ;
    }

    public static float EaseInOutExpo(float pX)
    {
        return pX == 0 ? 0 : pX == 1 ? 1 : pX < 0.5f ? Mathf.Pow(2 , 20 * pX - 10) / 2 : (2 - Mathf.Pow(2, -20 * pX + 10)) / 2;
    }

    /// <summary>
    /// values returned can be lower than 0 or higher than 1
    /// </summary>
    /// <param name="pX"></param>
    /// <returns></returns>
    public static float EaseInOutBack(float pX)
    {
        return pX < 0.5f ? Mathf.Pow(2 * pX, 2) * ((c2 + 1) * 2 * pX - c2) / 2 : (Mathf.Pow(2 * pX - 2, 2) * ((c2 + 1) * (pX * 2 - 2) + c2) + 2) / 2;
    }

    public static float EaseInOutBounce(float pX)
    {
        return pX < 0.5f ? (1 - EaseOutBounce(1 - 2 * pX)) / 2 : (1 + EaseOutBounce(2 * pX - 1)) / 2;
    }
    #endregion

    /// <summary>
    /// Eases <paramref name="pLi"/> according to <paramref name="pEaseType"/>'s easing function.
    /// Returns <paramref name="pLi"/> by default.
    /// </summary>
    /// <param name="pLi">Linear interpolation value, supposed between 0 and 1</param>
    /// <param name="pEaseType"></param>
    /// <returns>Eased value</returns>
    public static float Ease(float pLi, EaseType pEaseType = EaseType.None)
    {
        if (pLi<0 || pLi>1) Debug.LogWarning("interpolation value parameter outside [0;1] range");

        return pEaseType switch
        {
            EaseType.None => pLi,
            EaseType.EaseInSin => EaseInSin(pLi),
            EaseType.EaseInCubic => EaseInCubic(pLi),
            EaseType.EaseInQuad => EaseInQuad(pLi),
            EaseType.EaseInQuart => EaseInQuart(pLi),
            EaseType.EaseInQuint => EaseInQuint(pLi),
            EaseType.EaseInCirc => EaseInCirc(pLi),
            EaseType.EaseInElastic => EaseInElastic(pLi),
            EaseType.EaseInBack => EaseInBack(pLi),
            EaseType.EaseInBounce => EaseInBounce(pLi),
            EaseType.EaseInExpo => EaseInExpo(pLi),
            EaseType.EaseOutSin => EaseOutSin(pLi),
            EaseType.EaseOutCubic => EaseOutCubic(pLi),
            EaseType.EaseOutQuad => EaseOutQuad(pLi),
            EaseType.EaseOutQuart => EaseOutQuart(pLi),
            EaseType.EaseOutQuint => EaseOutQuint(pLi),
            EaseType.EaseOutCirc => EaseOutCirc(pLi),
            EaseType.EaseOutElastic => EaseOutElastic(pLi),
            EaseType.EaseOutBack => EaseOutBack(pLi),
            EaseType.EaseOutBounce => EaseOutBounce(pLi),
            EaseType.EaseOutExpo => EaseOutExpo(pLi),
            EaseType.EaseInOutSin => EaseInOutSin(pLi),
            EaseType.EaseInOutCubic => EaseInOutCubic(pLi),
            EaseType.EaseInOutQuad => EaseInOutQuad(pLi),
            EaseType.EaseInOutQuart => EaseInOutQuart(pLi),
            EaseType.EaseInOutQuint => EaseInOutQuint(pLi),
            EaseType.EaseInOutCirc => EaseInOutCirc(pLi),
            EaseType.EaseInOutElastic => EaseInOutElastic(pLi),
            EaseType.EaseInOutBack => EaseInOutBack(pLi),
            EaseType.EaseInOutBounce => EaseInOutBounce(pLi),
            EaseType.EaseInOutExpo => EaseInOutExpo(pLi),
            _ => pLi,
        };
    }
}