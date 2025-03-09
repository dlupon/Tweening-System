using UnityEngine;
using UnBocal.TweeningSystem;
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
		float lDuration = 2f;

        _tween.Interpolate(transform, Tween.Properties.POSITION, lStartPosition, lEndPosition, lDuration, EaseFunction.EaseOutBack);

        _tween.Interpolate(transform, Tween.Properties.POSITION, lEndPosition, lEndPosition + Vector3.right, lDuration, EaseFunction.EaseInOutBounce, lDuration);

        _tween.Interpolate(transform, Tween.Properties.ROTATION, transform.rotation, transform.rotation * Quaternion.AngleAxis(180f, Vector3.up), lDuration, EaseFunction.EaseOutExpo, lDuration);

        // _tween.Interpolate(transform, Tween.Properties.SCALE, new Vector3(1.3f, .7f, 1.3f), transform.localScale, .5f, EaseFunction.EaseOutElastic, lDuration + 1);

        _tween.Interpolate(_otherCube, Tween.Properties.POSITION, default, lStartPosition - Vector3.right, lDuration, EaseFunction.EaseInOutBack, lDuration);

        // _tween.Interpolate(_otherCube, Tween.Properties.SCALE, new Vector3(1.3f, .7f, 1.3f), _otherCube.localScale, .5f, _curve.Evaluate, lDuration + 1);

        _tween.Start();
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        _tween.Start();
    }
}