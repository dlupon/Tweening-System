// --~~~~======# Author : Lupon Dylan #======~~~~~~--- //
// --~~~~======# Date   : 03 / 05 / 2025 #======~~~~-- //

using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using UnBocal.TweeningSystem.Interpolations;
using Unity.VisualScripting;

namespace UnBocal.TweeningSystem
{
	public class Tween
	{
		// -------~~~~~~~~~~================# // Events
		public UnityEvent<object> OnStarted = new UnityEvent<object>();
		public UnityEvent<object> OnStoped = new UnityEvent<object>();

		// -------~~~~~~~~~~================# // Time
		bool _isFinished = true;

        // -------~~~~~~~~~~================# // Tween
        private static Dictionary<object, List<Tween>> _objectAndTweens = new Dictionary<object, List<Tween>>();

        // -------~~~~~~~~~~================# // Interpolation
        private List<object> Objects => _objectsAndInterpolators.Keys.ToList();
        private Dictionary<object, Dictionary<string, List<Interpolation>>> _objectsAndInterpolators = new Dictionary<object, Dictionary<string, List<Interpolation>>>();

        // ----------------~~~~~~~~~~~~~~~~~~~==========================# //  Tween Management 
        public static void RemoveAll(object pObject)
        {
            if (!_objectAndTweens.ContainsKey(pObject)) return;
            int lTweenCount = _objectAndTweens[pObject].Count;

            Tween lCurrentTween;
            for (int lTweenIndex = lTweenCount - 1; lTweenIndex >= 0; lTweenIndex--)
            {
                lCurrentTween = _objectAndTweens[pObject][lTweenIndex];
                lCurrentTween.Remove(pObject);
            }

            _objectAndTweens.Remove(pObject);
        }

        public static void RemoveTweenFromObject(Tween pTween, object pObject)
        {
            /*Init();
            if (!_instance._objectAndTweens.ContainsKey(pObject)) return;

            if (_instance._objectAndTweens[pObject].Contains(pTween))
                _instance._objectAndTweens[pObject].Remove(pTween);

            RemoveInterpolators(pTween.GetInterpolators(pObject));

            _instance._objectAndTweens.Remove(pObject);*/
        }

        private void StoreTween()
        {
            foreach (object lObject in Objects)
            {
                if (!_objectAndTweens.ContainsKey(lObject))
                   _objectAndTweens[lObject] = new List<Tween>();

                if (_objectAndTweens[lObject].Contains(this)) continue;
                _objectAndTweens[lObject].Add(this);
            }
        }

        // ----------------~~~~~~~~~~~~~~~~~~~==========================# // Control
        /// <summary>
        /// Reset all inteprolators, start the first one of all objects and add the tween to the TweenExecutionHandler.
        /// </summary>
        public void Start()
		{
            StoreTween();
            DoOnInterpolations(StartInterpolation);
            TweenExecutionHandler.AddInterpolations(GetInterpolations());
			TweenExecutionHandler.StartUpdateTween();
		}

        public void Empty()
        {
            TweenExecutionHandler.RemoveInterpolations(GetInterpolations());
            _objectsAndInterpolators.Clear();
        }

        public void Remove(object pObject)
        {
            if (!_objectsAndInterpolators.ContainsKey(pObject)) return;
            TweenExecutionHandler.RemoveInterpolations(GetInterpolations(pObject));
            _objectsAndInterpolators[pObject].Clear();
        }

        // ----------------~~~~~~~~~~~~~~~~~~~==========================# // Update
        /// <summary>
        /// Update allinterpolation and remove the tween when finished.
        /// </summary>
        public void UpdateInterpolation()
		{
			_isFinished = true;

			DoOnInterpolations(UpdateInterpolator);

			if (!_isFinished) return;
			TweenExecutionHandler.RemoveInterpolations(GetInterpolations());
		}

        // ----------------~~~~~~~~~~~~~~~~~~~==========================# // Interpolator
        /// <summary>
        /// Call pFunc on every Interplator.
        /// </summary>
        private void DoOnInterpolations(Action<Interpolation> pFunc)
        {
            foreach (object lObject in _objectsAndInterpolators.Keys)
                foreach (string lPropertyName in _objectsAndInterpolators[lObject].Keys)
                    foreach (Interpolation lInterpolation in _objectsAndInterpolators[lObject][lPropertyName])
                        pFunc(lInterpolation);
        }

        /// <summary>
        /// Update pInterpolator and tells the tween if it is finished.
        /// </summary>
        private void UpdateInterpolator(Interpolation pInterpolator)
        {
            pInterpolator.Update();
            if (pInterpolator.IsFinished) return;
            _isFinished = false;
        }

		private void StartInterpolation(Interpolation pInterpolator) => pInterpolator.Start();

        /// <summary>
        /// Find or create the appropriate interpolator.
        /// </summary>
        /// <typeparam name="ValueType">What type of property is interpolated.</typeparam>
        /// <param name="pObject">Targeted object.</param>
        /// <param name="pPropertyName">Object property name.</param>
        /// <returns>The right interolator based on the parameters</returns>
		private List<Interpolation> GetInterpolationList(object pObject, string pPropertyName)
        {
            // No Object (key) Found Then Create One
            if (!_objectsAndInterpolators.Keys.Contains(pObject))
                _objectsAndInterpolators[pObject] = new Dictionary<string, List<Interpolation>>();

            if (!_objectsAndInterpolators[pObject].ContainsKey(pPropertyName))
                _objectsAndInterpolators[pObject][pPropertyName] = new List<Interpolation>();

            return _objectsAndInterpolators[pObject][pPropertyName];
        }

        public List<Interpolation> GetInterpolations()
        {
            List<Interpolation> lInterpolators = new List<Interpolation>();
            DoOnInterpolations(lInterpolators.Add);

            return lInterpolators;
        }

        public List<Interpolation> GetInterpolations(object pObject)
        {
            if (!_objectsAndInterpolators.Keys.Contains(pObject)) return null;

            List<Interpolation> lInterpolators = new List<Interpolation>();
            foreach (string lPropertyName in _objectsAndInterpolators[pObject].Keys)
                foreach (Interpolation lInterpolation in _objectsAndInterpolators[pObject][lPropertyName])
                    lInterpolators.Add(lInterpolation);

            return lInterpolators;
        }

        // ----------------~~~~~~~~~~~~~~~~~~~==========================# // Interpolations
        private void AddInterpolation(object pObject, string pPropertyName, Action<float> pInterpolationMethod, float pDuration, float pDelay)
        {
            Interpolation lInterpolation = new Interpolation();
            lInterpolation.InterpolationMethod = pInterpolationMethod;
            lInterpolation.Duration = pDuration;
            lInterpolation.Delay = pDelay;

            GetInterpolationList(pObject, pPropertyName).Add(lInterpolation);
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