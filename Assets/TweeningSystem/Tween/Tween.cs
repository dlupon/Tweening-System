// --~~~~======# Author : Lupon Dylan #======~~~~~~--- //
// --~~~~======# Date   : 03 / 05 / 2025 #======~~~~-- //
using System;
using System.Collections.Generic;
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
		private Dictionary<Transform, List<Tweener>> _objectsAndTweeners = new Dictionary<Transform, List<Tweener>>();

		// ----------------~~~~~~~~~~~~~~~~~~~==========================# // Control
		public void Start()
		{
			Reset();
            TweenExecutionHandler.AddTweenToUpdate(this);
            TweenExecutionHandler.StartUpdateTween();
        }

		public void Pause()
		{

        }

		public void Revert()
		{

		}

		public void Reset()
		{
			foreach (Transform lObject in _objectsAndTweeners.Keys) foreach (var lTweener in _objectsAndTweeners[lObject])
				lTweener.ResetTime();
        }

        // ----------------~~~~~~~~~~~~~~~~~~~==========================# // Update
        public void UpdateInterpolation()
        {
			bool lIsFinished = true;

            foreach (Transform lObject in _objectsAndTweeners.Keys) foreach (var lTweener in _objectsAndTweeners[lObject])
			{
				lTweener.Interpolate(lObject);
				if (lTweener.Finished) continue;
				lIsFinished = false;
			}

			if (!lIsFinished) return;
			TweenExecutionHandler.RemoveTween(this);
        }

        // ----------------~~~~~~~~~~~~~~~~~~~==========================# // Interpolation
		// -------~~~~~~~~~~================# // Position Or Scale
		public void Interpolate(Transform pObject, Properties pProperties, Vector3 pStartPosition = default, Vector3 pEndPosition = default, float pDuration = 1, Func<float, float> pEase = default, float pDelay = 0f)
		{
			if (!(pProperties == Properties.POSITION || pProperties == Properties.SCALE))
			{
				ErrorWrongProperties(pObject, pStartPosition.GetType(), pProperties, Properties.POSITION, Properties.SCALE);
				return;
			}

			Tweener lTweener;

            switch (pProperties)
			{
				case Properties.POSITION:
                    lTweener = new TweenerPosition(pStartPosition, pEndPosition, pDuration, pEase, pDelay);
					break;
				
				case Properties.SCALE:
                    lTweener = new TweenerPosition(pStartPosition, pEndPosition, pDuration, pEase, pDelay);
					break;

				default: return;
            }

			AddTweener(pObject, lTweener);
        }

        // -------~~~~~~~~~~================# // Rotation
        public void Interpolate(Transform pObject, Properties pProperties, Quaternion pStartRotation = default, Quaternion pEndRotation = default, float pDuration = 1, Func<float, float> pEase = default, float pDelay = 0f)
        {
            if (!(pProperties == Properties.ROTATION))
            {
                ErrorWrongProperties(pObject, pStartRotation.GetType(), pProperties, Properties.ROTATION);
                return;
            }

            Tweener lTweener = new TweenerRotationFast(pStartRotation, pEndRotation, pDuration, pEase, pDelay);

            AddTweener(pObject, lTweener);
        }

        // ----------------~~~~~~~~~~~~~~~~~~~==========================# // Store Tweeners
		private void AddTweener(Transform pObject, Tweener pTweener)
		{
			if (!_objectsAndTweeners.ContainsKey(pObject))  _objectsAndTweeners[pObject] = new List<Tweener>();
            _objectsAndTweeners[pObject].Add(pTweener);
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