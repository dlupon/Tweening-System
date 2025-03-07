using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.UIElements;

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
		private List<Tween> _tweens = new List<Tween>();

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
        public static void AddTweenToUpdate(Tween pTween)
		{
			CheckInstance();
			if (_instance._tweens.Contains(pTween)) return;
			_instance._tweens.Add(pTween);
        }

		public static void RemoveTween(Tween pTween)
        {
            CheckInstance();
            if (!_instance._tweens.Contains(pTween)) return;
            _instance._tweens.Remove(pTween);
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
			while (_tweens.Count > 0)
			{
				UpdateTweens();
                yield return new WaitForEndOfFrameUnit();
			}

			_coroutine = null;

        }

		private void UpdateTweens()
		{
			int lLength = _tweens.Count;
			for (int lTweenIndex = lLength - 1; lTweenIndex >= 0; lTweenIndex--)
				_tweens[lTweenIndex].UpdateInterpolation();

        }
	}
}