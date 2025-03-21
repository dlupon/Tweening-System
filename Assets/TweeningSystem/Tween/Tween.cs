// --~~~~======# Author : Lupon Dylan #======~~~~~~--- //
// --~~~~======# Date   : 03 / 05 / 2025 #======~~~~-- //

using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using UnBocal.TweeningSystem.Interpolations;

namespace UnBocal.TweeningSystem
{
	public class Tween
	{
		// -------~~~~~~~~~~================# // Events
		public UnityEvent<object> OnStarted = new UnityEvent<object>();
		public UnityEvent<object> OnStoped = new UnityEvent<object>();

		// -------~~~~~~~~~~================# // Time
		bool _isFinished = true;

        // -------~~~~~~~~~~================# // Interpolation
        private Dictionary<object, Dictionary<string, Interpolator>> _objectsAndInterpolators = new Dictionary<object, Dictionary<string, Interpolator>>();

        // ----------------~~~~~~~~~~~~~~~~~~~==========================# // Control
        /// <summary>
        /// Reset all inteprolators, start the first one of all objects and add the tween to the TweenExecutionHandler.
        /// </summary>
        public void Start()
		{
            DoOnInterpolator(StartInterpolator);
            TweenExecutionHandler.AddInterpolators(GetInterpolators());
			TweenExecutionHandler.StartUpdateTween();
		}

        public void Clear()
        {
            DoOnInterpolator(StartInterpolator);
            TweenExecutionHandler.RemoveInterpolators(GetInterpolators());
            _objectsAndInterpolators.Clear();
        }

        // ----------------~~~~~~~~~~~~~~~~~~~==========================# // Update
        /// <summary>
        /// Update allinterpolation and remove the tween when finished.
        /// </summary>
        public void UpdateInterpolation()
		{
			_isFinished = true;

			DoOnInterpolator(UpdateInterpolator);

			if (!_isFinished) return;
			TweenExecutionHandler.RemoveInterpolators(GetInterpolators());
		}

		// ----------------~~~~~~~~~~~~~~~~~~~==========================# // Object
        /// <summary>
        /// Create a new list of Interpolator for the given object.
        /// </summary>
        /// <param name="pObject">Targeted Object.</param>
		private void AddObject(object pObject) => _objectsAndInterpolators[pObject] = new Dictionary<string, Interpolator>();

        // ----------------~~~~~~~~~~~~~~~~~~~==========================# // Interpolator
        /// <summary>
        /// Store the givent interpolator in the right place in the dictionary.
        /// </summary>
        /// <param name="pObject">Targeted Object.</param>
        /// <param name="pInterpolator">Interpolator to store.</param>
        private void AddInterpolator(object pObject, string pPropertyName, Interpolator pInterpolator)
        {
			if (!_objectsAndInterpolators.ContainsKey(pObject)) AddObject(pObject);
			_objectsAndInterpolators[pObject][pPropertyName] = pInterpolator;
        }

        /// <summary>
        /// Call pFunc on every Interplator.
        /// </summary>
        private void DoOnInterpolator(Action<Interpolator> pFunc)
        {
            foreach (object lObject in _objectsAndInterpolators.Keys)
                foreach (string lPropertyName in _objectsAndInterpolators[lObject].Keys)
                    pFunc(_objectsAndInterpolators[lObject][lPropertyName]);
        }

        /// <summary>
        /// Update pInterpolator and tells the tween if it is finished.
        /// </summary>
        private void UpdateInterpolator(Interpolator pInterpolator)
        {
            pInterpolator.Update();
            if (pInterpolator.IsFinished) return;
            _isFinished = false;
        }

		private void StartInterpolator(Interpolator pInterpolator) => pInterpolator.Start();

        /// <summary>
        /// Find or create the appropriate interpolator.
        /// </summary>
        /// <typeparam name="ValueType">What type of property is interpolated.</typeparam>
        /// <param name="pObject">Targeted object.</param>
        /// <param name="pPropertyName">Object property name.</param>
        /// <returns>The right interolator based on the parameters</returns>
		private Interpolator FindInterpolator(object pObject, string pPropertyName)
        {
            // Try Find Object (key) And Interpolation (Value)
            if (_objectsAndInterpolators.Keys.Contains(pObject)
                && _objectsAndInterpolators[pObject].Keys.Contains(pPropertyName))
                return _objectsAndInterpolators[pObject][pPropertyName];

            // No Object (key) Found Then Create One
            else if (!_objectsAndInterpolators.Keys.Contains(pObject)) AddObject(pObject);

			// If No Interpolation Has Been Found, Then Create One
			return CreateInterpolator(pObject, pPropertyName);
		}

        /// <summary>
        /// Create and store an interpolator based on the given property.
        /// </summary>
        /// <typeparam name="ValueType">What type of property is interpolated.</typeparam>
        /// <param name="pObject">Targeted object.</param>
        /// <param name="pPropertyName">Object property name.</param>
        /// <returns></returns>
		private Interpolator CreateInterpolator(object pObject, string pPropertyName)
		{
            // Create Interpolation And Store Property
            Interpolator lInterpolator = new Interpolator();
			AddInterpolator(pObject, pPropertyName, lInterpolator);

            return lInterpolator;
        }

        private List<Interpolator> GetInterpolators()
        {
            List<Interpolator> lInterpolators = new List<Interpolator>();
            foreach (object lObject in _objectsAndInterpolators.Keys)
                foreach (string pPropertyName in _objectsAndInterpolators[lObject].Keys)
                    lInterpolators.Add(_objectsAndInterpolators[lObject][pPropertyName]);

            return lInterpolators;
        }

        // ----------------~~~~~~~~~~~~~~~~~~~==========================# // Interpolations
        private void AddInterpolation(object pObject, string pPropertyName, Action<float> pInterpolationMethod, float pDuration, float pDelay)
        {
            // Find Interpolation
            Interpolator lInterpolator = FindInterpolator(pObject, pPropertyName);

            // Add New Intrpolation To The Rigt Interpolator
            lInterpolator.Add(pInterpolationMethod, pDuration, pDelay);
        }

        #region// -------~~~~~~~~~~================# // Transform Position
        public void Position(Transform pTransform, Vector3 pStartValue, Vector3 pEndValue, float pDuration, EaseType pEasing = EaseType.Flat, float pDelay = 0f)
         => Position(pTransform, pStartValue, pEndValue, pDuration, EaseFunction.GetFunction(pEasing), pDelay);

        public void Position(Transform pTransform, Vector3 pStartValue, Vector3 pEndValue, float pDuration, AnimationCurve pCurve, float pDelay = 0f)
         => Position(pTransform, pStartValue, pEndValue, pDuration, pCurve.Evaluate, pDelay);

        private void Position(Transform pTransform, Vector3 pStartValue, Vector3 pEndValue, float pDuration, Func<float, float> pEasing, float pDelay = 0f)
		{
            // Get Interpolation Method Based On ValueType
            Action<float> lInterpolationMethod = (float pRatio) => pTransform.position = Vector3.LerpUnclamped(pStartValue, pEndValue, pEasing(pRatio));

            AddInterpolation(pTransform, nameof(pTransform.position), lInterpolationMethod, pDuration, pDelay);
        }
        #endregion

        #region// -------~~~~~~~~~~================# // Transform Local Scale
        public void localScale(Transform pTransform, Vector3 pStartScale, Vector3 pEndScale, float pDuration, EaseType pEasing = EaseType.Flat, float pDelay = 0f)
            => localScale(pTransform, pStartScale, pEndScale, pDuration, EaseFunction.GetFunction(pEasing), pDelay);

        public void localScale(Transform pTransform, Vector3 pStartScale, Vector3 pEndScale, float pDuration, AnimationCurve pCurve, float pDelay = 0f)
            => localScale(pTransform, pStartScale, pEndScale, pDuration, pCurve.Evaluate, pDelay);

        private void localScale(Transform pTransform, Vector3 pStartScale, Vector3 pEndScale, float pDuration, Func<float, float> pEasing, float pDelay = 0f)
        {
            Action<float> lInterpolationMethod = (float pRatio) => pTransform.localScale = Vector3.LerpUnclamped(pStartScale, pEndScale, pEasing(pRatio));

            AddInterpolation(pTransform, nameof(pTransform.localScale), lInterpolationMethod, pDuration, pDelay);
        }
        #endregion

        #region// -------~~~~~~~~~~================# // Transform Local Rotation
        public void localRotation(Transform pTransform, Quaternion pStartRotation, Quaternion pEndRotation, float pDuration, EaseType pEasing, float pDelay = 0f)
            => localRotation(pTransform, pStartRotation, pEndRotation, pDuration, EaseFunction.GetFunction(pEasing), pDelay);

        public void localRotation(Transform pTransform, Quaternion pStartRotation, Quaternion pEndRotation, float pDuration, AnimationCurve pCurve, float pDelay = 0f)
            => localRotation(pTransform, pStartRotation, pEndRotation, pDuration, pCurve.Evaluate, pDelay);

        private void localRotation(Transform pTransform, Quaternion pStartRotation, Quaternion pEndRotation, float pDuration, Func<float, float> pEasing, float pDelay = 0f)
        {
            Action<float> lInterpolationMethod = (float pRatio) => pTransform.localRotation = Quaternion.LerpUnclamped(pStartRotation, pEndRotation, pEasing(pRatio));

            AddInterpolation(pTransform, nameof(pTransform.localRotation), lInterpolationMethod, pDuration, pDelay);
        }
        #endregion

        #region// -------~~~~~~~~~~================# // RectTransform Position
        public void UIPosition(RectTransform pTransform, Vector3 pStartValue, Vector3 pEndValue, float pDuration, EaseType pEasing = EaseType.Flat, float pDelay = 0f)
         => UIPosition(pTransform, pStartValue, pEndValue, pDuration, EaseFunction.GetFunction(pEasing), pDelay);

        public void UIPosition(RectTransform pTransform, Vector3 pStartValue, Vector3 pEndValue, float pDuration, AnimationCurve pCurve, float pDelay = 0f)
         => UIPosition(pTransform, pStartValue, pEndValue, pDuration, pCurve.Evaluate, pDelay);

        private void UIPosition(RectTransform pRectTransform, Vector3 pStartValue, Vector3 pEndValue, float pDuration, Func<float, float> pEasing, float pDelay = 0f)
        {
            Action<float> lInterpolationMethod = (float pRatio) => pRectTransform.position = Vector3.LerpUnclamped(pStartValue, pEndValue, pEasing(pRatio));

            AddInterpolation(pRectTransform, nameof(pRectTransform.position), lInterpolationMethod, pDuration, pDelay);
        }
        #endregion

        #region// -------~~~~~~~~~~================# // RectTransform Local Scale
        public void UIScale(RectTransform pRectTransform, Vector3 pStartScale, Vector3 pEndScale, float pDuration, EaseType pEasing = EaseType.Flat, float pDelay = 0f)
            => UIScale(pRectTransform, pStartScale, pEndScale, pDuration, EaseFunction.GetFunction(pEasing), pDelay);

        public void UIScale(RectTransform pRectTransform, Vector3 pStartScale, Vector3 pEndScale, float pDuration, AnimationCurve pCurve, float pDelay = 0f)
            => UIScale(pRectTransform, pStartScale, pEndScale, pDuration, pCurve.Evaluate, pDelay);

        private void UIScale(RectTransform pRectTransform, Vector3 pStartScale, Vector3 pEndScale, float pDuration, Func<float, float> pEasing, float pDelay = 0f)
        {
            Action<float> lInterpolationMethod = (float pRatio) => pRectTransform.localScale = Vector3.LerpUnclamped(pStartScale, pEndScale, pEasing(pRatio));

            AddInterpolation(pRectTransform, nameof(pRectTransform.localScale), lInterpolationMethod, pDuration, pDelay);
        }
        #endregion

        #region// -------~~~~~~~~~~================# // RectTransform Rotation
        public void UIRotation(RectTransform pRectTransform, Quaternion pStartRotation, Quaternion pEndRotation, float pDuration, EaseType pEasing, float pDelay = 0f)
            => UIRotation(pRectTransform, pStartRotation, pEndRotation, pDuration, EaseFunction.GetFunction(pEasing), pDelay);

        public void UIRotation(RectTransform pRectTransform, Quaternion pStartRotation, Quaternion pEndRotation, float pDuration, AnimationCurve pCurve, float pDelay = 0f)
            => UIRotation(pRectTransform, pStartRotation, pEndRotation, pDuration, pCurve.Evaluate, pDelay);

        private void UIRotation(RectTransform pRectTransform, Quaternion pStartRotation, Quaternion pEndRotation, float pDuration, Func<float, float> pEasing, float pDelay = 0f)
        {
            Action<float> lInterpolationMethod = (float pRatio) => pRectTransform.rotation = Quaternion.LerpUnclamped(pStartRotation, pEndRotation, pEasing(pRatio));

            AddInterpolation(pRectTransform, nameof(pRectTransform.rotation), lInterpolationMethod, pDuration, pDelay);
        }
        #endregion
    }
}