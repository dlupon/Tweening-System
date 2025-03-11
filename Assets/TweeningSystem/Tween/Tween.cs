// --~~~~======# Author : Lupon Dylan #======~~~~~~--- //
// --~~~~======# Date   : 03 / 05 / 2025 #======~~~~-- //
using System;
using System.Linq;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnBocal.TweeningSystem.Interfaces;
using UnBocal.TweeningSystem.Interpolations;

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
            IInterpolator lInterpolator = InterpolationFactory.GetNew<InterpolatorType>();
			AddInterpolator(pObject, lInterpolator);

            return lInterpolator;
        }

        // ----------------~~~~~~~~~~~~~~~~~~~==========================# // Interpolations
		public InterpolatorType Interpolate<InterpolatorType>(Transform pObject) where InterpolatorType : IInterpolator
		 	=> (InterpolatorType)FindInterpolator<InterpolatorType>(pObject);

        public InterpolatorType Interpolate<InterpolatorType, ValueType>(Transform pObject, ValueType pStartValue = default, ValueType pEndValue = default, float pDuration = 1, Func<float, float> pEase = default, float pDelay = 0f) where InterpolatorType : PropertyInterpolator<ValueType> where ValueType : struct
        {
			InterpolatorType lInterpolator = Interpolate<InterpolatorType>(pObject);
			lInterpolator.Add(pStartValue, pEndValue, pDuration, pEase, pDelay);

            return lInterpolator;
        }

    }
}