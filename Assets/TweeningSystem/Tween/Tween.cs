// --~~~~======# Author : Lupon Dylan #======~~~~~~--- //
// --~~~~======# Date   : 03 / 05 / 2025 #======~~~~-- //
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnBocal.TweeningSystem.Interpolations;
using System.Linq;

namespace UnBocal.TweeningSystem
{
	public class Tween
	{
		public enum Properties
		{
			POSITION,
			ROTATION,
			SCALE
		}

		// -------~~~~~~~~~~================# // Events
		public UnityEvent<Transform> OnStarted = new UnityEvent<Transform>();
		public UnityEvent<Transform> OnStoped = new UnityEvent<Transform>();

		// -------~~~~~~~~~~================# // Time
		bool _isFinished = true;

		// -------~~~~~~~~~~================# // Interpolators
		Dictionary<Transform, List<IInterpolator>> _objectsAndInterpolators = new Dictionary<Transform, List<IInterpolator>>();

		// ----------------~~~~~~~~~~~~~~~~~~~==========================# // Control
		public void Start()
		{
			Reset();
			TweenExecutionHandler.AddTweenToUpdate(this);
			TweenExecutionHandler.StartUpdateTween();
		}

        public void Reset() => DoOnInterpolator(ResetInterpolator);

		// ----------------~~~~~~~~~~~~~~~~~~~==========================# // Update
		public void UpdateInterpolation()
		{
			_isFinished = true;

			DoOnInterpolator(UpdateInterpolator);

			if (!_isFinished) return;
			TweenExecutionHandler.RemoveTween(this);
		}

        // ----------------~~~~~~~~~~~~~~~~~~~==========================# // Object
		private void AddObject(Transform pObject) => _objectsAndInterpolators[pObject] = new List<IInterpolator>();

        // ----------------~~~~~~~~~~~~~~~~~~~==========================# // Interpolator
        void AddInterpolator(Transform pObject, IInterpolator pInterpolator)
        {
            if (!_objectsAndInterpolators.ContainsKey(pObject)) _objectsAndInterpolators[pObject] = new List<IInterpolator>();
            _objectsAndInterpolators[pObject].Add(pInterpolator);
        }

        private void DoOnInterpolator(Action<Transform, IInterpolator> pFunc)
        {
			int lInterpolatorCount;
            foreach (Transform lObject in _objectsAndInterpolators.Keys)
			{
                lInterpolatorCount = _objectsAndInterpolators[lObject].Count;
				for (int lIndexInterpolator = lInterpolatorCount - 1; lIndexInterpolator  >= 0; lIndexInterpolator--)
                    pFunc(lObject, _objectsAndInterpolators[lObject][lIndexInterpolator]);
			}
        }

        private void UpdateInterpolator(Transform pObject, IInterpolator pInterpolator)
        {
            pInterpolator.Interpolate(pObject);
            if (pInterpolator.IsFinished()) return;
            _isFinished = false;
        }

		private void ResetInterpolator(Transform pObject, IInterpolator pInterpolator) => pInterpolator.Reset(pObject);

		private void FindInterpolatorAndAddInterpolator<ValueType>(Transform pObject, Interpolation<ValueType> pInterpolation, Properties pProperties)
		{
			if (_objectsAndInterpolators.Keys.Contains(pObject))
			{
				foreach (IInterpolator pInterpolator in _objectsAndInterpolators[pObject])
				{
					if (!(pInterpolator is PropertyInterpolator<ValueType>)) continue;
					((PropertyInterpolator<ValueType>)pInterpolator).AddInterpolation(pInterpolation);
					return;
				}
			}
			else AddObject(pObject);

			CreateInterpolatorAndAddInterpolator(pObject, pInterpolation, pProperties);
		}

		private void CreateInterpolatorAndAddInterpolator<ValueType>(Transform pObject, Interpolation<ValueType> pInterpolation, Properties pProperties)
		{
			IInterpolator lInterpolator = null;

            switch (pProperties)
			{
				case Properties.POSITION:
					lInterpolator = new PositionInterpolator();
                    break;

				case  Properties.ROTATION:
                    lInterpolator = new RotationFastInterpolator();
                    break;

				case  Properties.SCALE:
                    lInterpolator = new ScaleInterpolator();
                    break;
            }

			if (lInterpolator == null) return;

            ((PropertyInterpolator<ValueType>)lInterpolator)?.AddInterpolation(pInterpolation);
			AddInterpolator(pObject, lInterpolator);
        }

        // ----------------~~~~~~~~~~~~~~~~~~~==========================# // Interpolations
        public void Interpolate<ValueType>(Transform pObject, Properties pProperties, ValueType pStartValue = default, ValueType pEndValue = default, float pDuration = 1, Func<float, float> pEase = default, float pDelay = 0f)
		{
			Interpolation<ValueType> lInterpolation = new Interpolation<ValueType>();
            lInterpolation.StartValue = pStartValue;
            lInterpolation.EndValue = pEndValue;
            lInterpolation.Duration = pDuration;
            lInterpolation.Delay = pDelay;
            lInterpolation.EaseFunction = pEase;

			FindInterpolatorAndAddInterpolator(pObject, lInterpolation, pProperties);
        }

        // ----------------~~~~~~~~~~~~~~~~~~~==========================# // Errors
        void ErrorWrongProperties(Transform pObject, Type pType, Properties pGiventProperty, params Properties[] pUsableProperties)
		{
			string lUsableProperitesNames = "" + pUsableProperties[0];
			int lPropertiesCount = pUsableProperties.Length;

			for (int lPropertyIndex = 1; lPropertyIndex < lPropertiesCount; lPropertyIndex++) lUsableProperitesNames += $", or {pUsableProperties[lPropertyIndex]}";

			string lMessage = $"The given property \"{pGiventProperty}\" doesn't work with the type of interpolation currently used ({pType}). The Type {pType} only works with {lUsableProperitesNames}.";

			Debug.LogError(lMessage);
		}
	}
}