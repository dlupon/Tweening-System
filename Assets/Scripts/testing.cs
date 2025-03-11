using UnityEngine;
using UnBocal.TweeningSystem;
using UnBocal.TweeningSystem.Interpolations;
using UnBocal.TweeningSystem.Easing;

// --~~~~======# Author : Lupon Dylan #======~~~~~~--- //
// --~~~~======# Date   : 00 / 00 / 2000 #======~~~~-- //
public class testing : MonoBehaviour
{
	Tween _tween = new Tween();
    [SerializeField] private AnimationCurve _curve;

    [SerializeField] private Transform _otherCube;

	// ----------------~~~~~~~~~~~~~~~~~~~==========================# // Unity   
	private void Start()
	{
        _tween.Interpolate<Position>(transform).Add(Vector3.zero, Vector3.up);
        _tween.Interpolate<Position, Vector3>(transform, Vector3.up, Vector3.up + Vector3.right, pDelay : 1f, pEase : EaseFunction.OutExpo);

        _tween.Start();
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        _tween.Start();
    }
}