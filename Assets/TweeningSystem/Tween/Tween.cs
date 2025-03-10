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

        void AddInterpolator(Transform pObject, IInterpolator pInterpolator)
        {
			if (!_objectsAndInterpolators.ContainsKey(pObject)) AddObject(pObject);
            _objectsAndInterpolators[pObject].Add(pInterpolator);
        }

        private void UpdateInterpolator(Transform pObject, IInterpolator pInterpolator)
        {
            pInterpolator.Interpolate(pObject);
            if (pInterpolator.IsFinished()) return;
            _isFinished = false;
        }

		private void ResetInterpolator(Transform pObject, IInterpolator pInterpolator) => pInterpolator.Reset(pObject);

		private IInterpolator FindInterpolator<InterpolatorType>(Transform pObject) where InterpolatorType : IInterpolator
        {
			if (_objectsAndInterpolators.Keys.Contains(pObject))
			{
				foreach (IInterpolator pInterpolator in _objectsAndInterpolators[pObject])
				{
					if (!(pInterpolator is InterpolatorType)) continue;
					return pInterpolator;
				}
			}
			else AddObject(pObject);

			return CreateInterpolator<InterpolatorType>(pObject);
		}

		private IInterpolator CreateInterpolator<InterpolatorType>(Transform pObject) where InterpolatorType : IInterpolator
		{
            IInterpolator lInterpolator = null;

            switch (typeof(InterpolatorType).Name)
			{
				case nameof(Position): lInterpolator = new Position(); break;
				case nameof(Scale): lInterpolator = new Scale(); break;
				case nameof(RotationFast): lInterpolator = new RotationFast(); break;
			}

			AddInterpolator(pObject, lInterpolator);

            return lInterpolator;
        }

        // ----------------~~~~~~~~~~~~~~~~~~~==========================# // Interpolations
        public void Interpolate<InterolatorType>(Transform pObject, object pStartValue = default, object pEndValue = default, float pDuration = 1, Func<float, float> pEase = default, float pDelay = 0f) where InterolatorType : IInterpolator
        {
			IInterpolator pInterpolator = FindInterpolator<InterolatorType>(pObject);

			switch (pStartValue.GetType().Name)
			{
				case nameof(Vector3):
                    CreateAndAddInterpolation(pInterpolator, (Vector3)pStartValue, (Vector3)pEndValue, pDuration, pEase, pDelay);
                    return;

				case nameof(Quaternion):
                    CreateAndAddInterpolation(pInterpolator, (Quaternion)pStartValue, (Quaternion)pEndValue, pDuration, pEase, pDelay);
					return;
			}			
        }

		private void CreateAndAddInterpolation<ValueType>(IInterpolator pInterpolator, ValueType pStartValue = default, ValueType pEndValue = default, float pDuration = 1, Func<float, float> pEase = default, float pDelay = 0f)
		{
			Interpolation<ValueType> lInterpolation = CreateInterpolation(pStartValue, pEndValue, pDuration, pEase, pDelay);
            ((PropertyInterpolator<ValueType>)pInterpolator).AddInterpolation(lInterpolation);
        }

		private Interpolation<T> CreateInterpolation<T>(T pStartValue = default, T pEndValue = default, float pDuration = 1, Func<float, float> pEase = default, float pDelay = 0f)
        {
            Interpolation<T> lInterpolation = new Interpolation<T>();
            lInterpolation.StartValue = pStartValue;
            lInterpolation.EndValue = pEndValue;
            lInterpolation.Duration = pDuration;
            lInterpolation.Delay = pDelay;
            lInterpolation.EaseFunction = pEase;

			return lInterpolation;
        }

        // ----------------~~~~~~~~~~~~~~~~~~~==========================# // Errors
        /*void ErrorWrongProperties(Transform pObject, Type pType, params Properties[] pUsableProperties)
		{
			string lUsableProperitesNames = "" + pUsableProperties[0];
			int lPropertiesCount = pUsableProperties.Length;

			for (int lPropertyIndex = 1; lPropertyIndex < lPropertiesCount; lPropertyIndex++) lUsableProperitesNames += $", or {pUsableProperties[lPropertyIndex]}";

			string lMessage = $"The given property \"{pGiventProperty}\" doesn't work with the type of interpolation currently used ({pType}). The Type {pType} only works with {lUsableProperitesNames}.";

			Debug.LogError(lMessage);
		}*/
    }
}