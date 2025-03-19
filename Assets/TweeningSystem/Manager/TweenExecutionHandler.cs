using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;
using UnBocal.TweeningSystem.Interpolations;

// --~~~~======# Author : Lupon Dylan #======~~~~~~--- //
// --~~~~======# Date   : 03 / 05 / 2025 #======~~~~-- //
namespace UnBocal.TweeningSystem
{
	public class TweenExecutionHandler : MonoBehaviour
	{
		// -------~~~~~~~~~~================# // Host Properties
		private const string HOST_NAME = nameof(TweenExecutionHandler);

		// -------~~~~~~~~~~================# // Singleton
		public static TweenExecutionHandler Instance => GetInstance();
		private static TweenExecutionHandler _instance;

		// -------~~~~~~~~~~================# // Tween
		private HashSet<Interpolator> _interpolators = new HashSet<Interpolator>();

		// -------~~~~~~~~~~================# // Coroutine
		private Coroutine _coroutine;

		// ----------------~~~~~~~~~~~~~~~~~~~==========================# // Singleton
		private static void CheckInstance()
		{
			if (_instance != null) return;
			GetInstance();
        }

		private static TweenExecutionHandler GetInstance()
		{
            if (_instance == null) InitInstance();
			return _instance;
		}

        private static void InitInstance()
        {
            GameObject lHost = new GameObject(HOST_NAME);
			_instance = lHost.AddComponent<TweenExecutionHandler>();
			DontDestroyOnLoad(lHost);
        }

        // -------~~~~~~~~~~================# // Tween Management
        public static void AddInterpolators(List<Interpolator> pInterpolators)
		{
			CheckInstance();
            _instance._interpolators.AddRange(pInterpolators);
        }

        public static void AddInterpolator(Interpolator pInterpolator)
        {
            CheckInstance();
            _instance._interpolators.Add(pInterpolator);
        }

        public static void RemoveInterpolators(List<Interpolator> pInterpolators)
        {
            CheckInstance();
			foreach (Interpolator interpolator in pInterpolators)
				RemoveInterpolator(interpolator);
        }

        public static void RemoveInterpolator(Interpolator pInterpolator)
        {
            CheckInstance();
            if (!_instance._interpolators.Contains(pInterpolator)) return;
            _instance._interpolators.Remove(pInterpolator);
        }

        // -------~~~~~~~~~~================# // Coroutine
        public static void StartUpdateTween()
		{
            CheckInstance();
			_instance.StartCoroutine();
        }

		private void StartCoroutine()
		{
            if (_coroutine != null) return;
			_coroutine = StartCoroutine(nameof(LoopThroughTweens));
        }

		private IEnumerator LoopThroughTweens()
		{
			while (_interpolators.Count > 0)
			{
				UpdateTweens();
                yield return new WaitForEndOfFrameUnit();
			}

			_coroutine = null;
        }

		private void UpdateTweens()
		{
			int lInterpolatorCount = _interpolators.Count;
			foreach (Interpolator lInterpolator in _interpolators)
			{
                lInterpolator.Update();
				if (!lInterpolator.IsFinished) continue;
				RemoveInterpolator(lInterpolator);
            }

        }
	}
}