// --~~~~======# Author : Lupon Dylan #======~~~~~~--- //
// --~~~~======# Date   : 03 / 05 / 2025 #======~~~~-- //

using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.Events;
using UnBocal.TweeningSystem.Interfaces;
using UnBocal.TweeningSystem.Interpolations;
using UnBocal.TweeningSystem.Easing;
using UnityEngine;
using System.Linq.Expressions;

namespace UnBocal.TweeningSystem
{
	public class Tween
	{
		// -------~~~~~~~~~~================# // Events
		public UnityEvent<object> OnStarted = new UnityEvent<object>();
		public UnityEvent<object> OnStoped = new UnityEvent<object>();

		// -------~~~~~~~~~~================# // Time
		bool _isFinished = true;

        // -------~~~~~~~~~~================# // Interpolators
        private Dictionary<object, List<IInterpolator>> _objectsAndInterpolators = new Dictionary<object, List<IInterpolator>>();

        // ----------------~~~~~~~~~~~~~~~~~~~==========================# // Control

        /// <summary>
        /// Reset all inteprolators, start the first one of all objects and add the tween to the TweenExecutionHandler.
        /// </summary>
        public void Start()
		{
            DoOnInterpolator(StartInterpolator);
            TweenExecutionHandler.AddTweenToUpdate(this);
			TweenExecutionHandler.StartUpdateTween();
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
			TweenExecutionHandler.RemoveTween(this);
		}

		// ----------------~~~~~~~~~~~~~~~~~~~==========================# // Object
        /// <summary>
        /// Create a new list of Interpolator for the given object.
        /// </summary>
        /// <param name="pObject">Targeted Object.</param>
		private void AddObject(object pObject) => _objectsAndInterpolators[pObject] = new List<IInterpolator>();

        // ----------------~~~~~~~~~~~~~~~~~~~==========================# // Interpolator
        /// <summary>
        /// Store the givent interpolator in the right place in the dictionary.
        /// </summary>
        /// <param name="pObject">Targeted Object.</param>
        /// <param name="pInterpolator">Interpolator to store.</param>
        private void AddInterpolator(object pObject, IInterpolator pInterpolator)
        {
			if (!_objectsAndInterpolators.ContainsKey(pObject)) AddObject(pObject);
			_objectsAndInterpolators[pObject].Add(pInterpolator);
        }

        /// <summary>
        /// Call pFunc on every Interplator.
        /// </summary>
        private void DoOnInterpolator(Action<IInterpolator> pFunc)
        {
            List<IInterpolator> lInterpolators;
            foreach (object lObject in _objectsAndInterpolators.Keys)
            {
                lInterpolators = _objectsAndInterpolators[lObject];
				foreach (IInterpolator lInterpolation in lInterpolators)
					pFunc(lInterpolation);
            }
        }

        /// <summary>
        /// Update pInterpolator and tells the tween if it is finished.
        /// </summary>
        private void UpdateInterpolator(IInterpolator pInterpolator)
        {
            pInterpolator.Update();
            if (pInterpolator.IsFinished()) return;
            _isFinished = false;
        }

		private void StartInterpolator(IInterpolator pInterpolator) => pInterpolator.Start();

        /// <summary>
        /// Find or create the appropriate interpolator.
        /// </summary>
        /// <typeparam name="ValueType">What type of property is interpolated.</typeparam>
        /// <param name="pObject">Targeted object.</param>
        /// <param name="pPropertyName">Object property name.</param>
        /// <returns>The right interolator based on the parameters</returns>
		private Interpolator<ValueType> FindInterpolator<ValueType>(object pObject, string pPropertyName)
        {
			// Try Find Object (key) And Interpolation (Value)
			if (_objectsAndInterpolators.Keys.Contains(pObject))
			{
				foreach (IInterpolator pInterpolator in _objectsAndInterpolators[pObject])
				{
					if (pInterpolator.PropertyName != pPropertyName) continue;
					return (Interpolator<ValueType>)pInterpolator;
				}
			}

            // No Object (key) Found Then Create One
            else AddObject(pObject);

			// If No Interpolation Has Been Found, Then Create One
			return CreateInterpolator<ValueType>(pObject, pPropertyName);
		}

        /// <summary>
        /// Create and store an interpolator based on the given property.
        /// </summary>
        /// <typeparam name="ValueType">What type of property is interpolated.</typeparam>
        /// <param name="pObject">Targeted object.</param>
        /// <param name="pPropertyName">Object property name.</param>
        /// <returns></returns>
		private Interpolator<ValueType> CreateInterpolator<ValueType>(object pObject, string pPropertyName)
		{
			// Create Interpolation And Store Property
            Interpolator<ValueType> lInterpolator = Interpolator<ValueType>.GetNew(pObject, pPropertyName);
			AddInterpolator(pObject, lInterpolator);

            return lInterpolator;
        }

        // ----------------~~~~~~~~~~~~~~~~~~~==========================# // Interpolations
        /// <summary>
        /// Determines the needed method in InterpolationType based on the pMethodType and pValueType.
        /// </summary>
        /// <param name="pMethodType">What kind of method is required.</param>
        /// <param name="pValueType">What Kind of values are accepted.</param>
        /// <returns></returns>
        private MethodInfo GetMethod(string pMethodType, Type pValueType)
		{
            string pMethodName = $"{pMethodType}{pValueType.Name}";
            return typeof(InterpolationType).GetMethod(pMethodName);
        }

        /// <summary>
        /// Interpolate a property over time.
        /// </summary>
        /// <typeparam name="ValueType">What type of property is interpolated.</typeparam>
        /// <param name="pObject">Targeted object.</param>
        /// <param name="pPropertyName">Object property name.</param>
        /// <param name="pStartValue">Where the property should begin.</param>
        /// <param name="pEndValue">Where the property should end.</param>
        /// <param name="pDuration">Overall duration of the inteprolation (in seconds).</param>
        /// <param name="pEasingFunction">Easing method used to interpolate.</param>
        /// <param name="pDelay">How much the interpolation is delayed (in seconds).</param>
        public void Interpolate<ValueType>(object pObject, string pPropertyName, ValueType pStartValue, ValueType pEndValue, float pDuration, Func<float, float> pEasingFunction, float pDelay = 0f)
		{
			// Create Interpolation Base On The ValueType
			Interpolator<ValueType> lInterpolator = FindInterpolator<ValueType>(pObject, pPropertyName);

			// Get Interpolation Method Based On ValueType
            MethodInfo lMethod = GetMethod(nameof(Interpolate), lInterpolator.Property.GetType());
            Func<float, ValueType> lInterpolationMethod = (pRatio) => (ValueType)lMethod.InvokeOptimized(null, pStartValue, pEndValue, pEasingFunction(pRatio));

            // Expression.Lambda<TDelegate>(Expression.Call(methodinfo), parameters).Compile()
            // lInterpolationMethod = (pRatio) => Expression.Lambda<ValueType>(Expression.Call(lMethod));

            // Add New Intrpolation To The Rigt Interpolator
            lInterpolator.Add(lInterpolationMethod, pObject, pPropertyName, pDuration, pDelay);
        }

        /// <summary>
        /// Interpolate a property over time.
        /// </summary>
        /// <typeparam name="ValueType">What type of property is interpolated.</typeparam>
        /// <param name="pObject">Targeted object.</param>
        /// <param name="pPropertyName">Object property name.</param>
        /// <param name="pStartValue">Where the property should begin.</param>
        /// <param name="pEndValue">Where the property should end.</param>
        /// <param name="pDuration">Overall duration of the inteprolation (in seconds).</param>
        /// <param name="pEase">Easing method used to interpolate.</param>
        /// <param name="pDelay">How much the interpolation is delayed (in seconds).</param>
        public void Interpolate<ValueType>(object pObject, string pPropertyName, ValueType pStartValue, ValueType pEndValue, float pDuration, EaseType pEase = EaseType.Flat, float pDelay = 0f)
        {
            Func<float, float> lEaseFunction = EaseFunction.GetFunction(pEase);

            Interpolate(pObject, pPropertyName, pStartValue, pEndValue, pDuration, lEaseFunction, pDelay);
        }

        /// <summary>
        /// Interpolate a property over time.
        /// </summary>
        /// <typeparam name="ValueType">What type of property is interpolated.</typeparam>
        /// <param name="pObject">Targeted object.</param>
        /// <param name="pPropertyName">Object property name.</param>
        /// <param name="pStartValue">Where the property should begin.</param>
        /// <param name="pEndValue">Where the property should end.</param>
        /// <param name="pDuration">Overall duration of the inteprolation (in seconds).</param>
        /// <param name="pAnimationCurve">Curve used to interpolate.</param>
        /// <param name="pDelay">How much the interpolation is delayed (in seconds).</param>
        public void Interpolate<ValueType>(object pObject, string pPropertyName, ValueType pStartValue, ValueType pEndValue, float pDuration, AnimationCurve pAnimationCurve, float pDelay = 0f)
        {
            // Store Animation Curve As A Ease Function
            Func<float, float> lEaseFunction = pAnimationCurve.Evaluate;

            Interpolate(pObject, pPropertyName, pStartValue, pEndValue, pDuration, lEaseFunction, pDelay);
        }
    }
}