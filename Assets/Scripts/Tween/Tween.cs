// --~~~~======# Author : Lupon Dylan #======~~~~~~--- //
// --~~~~======# Date   : 03 / 05 / 2025 #======~~~~-- //
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace UnBocal.TweeningSystem
{
    public class Tween
    {
        public enum Properties
        {
            POSITION,
            SCALE,
            ROTATION
        }

        // -------~~~~~~~~~~================# // Events
        public UnityEvent<Transform> OnStarted = new UnityEvent<Transform>();
        public UnityEvent<Transform> OnStoped = new UnityEvent<Transform>();

        // -------~~~~~~~~~~================# // Time
        float _delay;
        float _duration;
        float _timeStart;
        float _timeEnd;

        // -------~~~~~~~~~~================# // Tweeners
        private Transform _object;
        public List<Tweener> _tweeners = new List<Tweener>();

        // ----------------~~~~~~~~~~~~~~~~~~~==========================# // Interpolation
        public void Interpolate(Transform pObject, Properties pProperties, Vector3 pStartPosition = default, Vector3 pEndPosition = default, float pDuration = 1, Func<float, float> pEase = default, float pDelay = 0f)
        {
            if (!(pProperties == Properties.POSITION || pProperties == Properties.SCALE))
            {
                ErrorWrongProperties(pObject, pStartPosition.GetType(), pProperties, Properties.POSITION, Properties.SCALE);
                return;
            }

            _object = pObject;

            Tweener lTweener = new TweenerPosition(pStartPosition, pEndPosition, pDuration, pEase, pDelay);
            _tweeners.Add(lTweener);
        }

        // ----------------~~~~~~~~~~~~~~~~~~~==========================# // Update
        public void UpdateInterpolation()
        {
            foreach (var tweener in _tweeners)
                tweener.Interpolate(_object);
        }

        // ----------------~~~~~~~~~~~~~~~~~~~==========================# // Errors
        private void ErrorWrongProperties(Transform pObject, Type pType, Properties pGiventProperty, params Properties[] pUsableProperties)
        {
            string lUsableProperitesNames = "" + pUsableProperties[0];
            int lPropertiesCount = pUsableProperties.Length;

            for (int lPropertyIndex = 1; lPropertyIndex < lPropertiesCount; lPropertyIndex++) lUsableProperitesNames += $", or {pUsableProperties[lPropertyIndex]}";

            string lMessage = $"The given property \"{pGiventProperty}\" doesn't work with the type of interpolation currently used ({pType}). The Type {pType} only works with {lUsableProperitesNames}.";

            Debug.LogError(lMessage);
        }
    }
}