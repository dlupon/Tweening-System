using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;
using UnBocal.TweeningSystem.Interfaces;
using System.Linq;

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
		private List<IInterpolator> _interpolators = new List<IInterpolator>();

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
        public static void AddInterpolators(List<IInterpolator> pInterpolators)
		{
			CheckInstance();
			foreach (IInterpolator interpolator in pInterpolators)
				AddInterpolator(interpolator);
        }

        public static void AddInterpolator(IInterpolator pInterpolator)
        {
            CheckInstance();
            if (_instance._interpolators.Contains(pInterpolator)) return;
            _instance._interpolators.Add(pInterpolator);
        }

        public static void RemoveInterpolators(List<IInterpolator> pInterpolators)
        {
            CheckInstance();
			foreach (IInterpolator interpolator in pInterpolators)
				RemoveInterpolator(interpolator);
        }

        public static void RemoveInterpolator(IInterpolator pInterpolator)
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
			for (int lInterplatorIndex = lInterpolatorCount - 1; lInterplatorIndex >= 0; lInterplatorIndex--)
				_interpolators[lInterplatorIndex].Update();

        }
	}
}