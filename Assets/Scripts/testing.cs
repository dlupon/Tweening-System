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
		Vector3 lStartPosition = Vector3.zero;
		Vector3 lEndPosition = Vector3.up;
        
        _tween.Interpolate<Position>(transform, Vector3.up, Vector3.up * 2f, 1.5f, _curve.Evaluate);
        _tween.Interpolate<RotationFast>(transform, Quaternion.identity, Quaternion.identity * Quaternion.AngleAxis(180f, Vector3.up), 1.5f, EaseFunction.EaseOutExpo);
        _tween.Interpolate<RotationFast>(transform, Quaternion.identity, Quaternion.identity * Quaternion.AngleAxis(180f, Vector3.forward), 1.5f, _curve.Evaluate, 1.5f);

        _tween.Interpolate<Scale>(_otherCube, new Vector3(1.3f, .7f, 1.3f), _otherCube.localScale, 1.5f, EaseFunction.EaseOutBack);

        _tween.Start();
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        _tween.Start();
    }
}