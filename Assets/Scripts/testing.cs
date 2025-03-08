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

        _tween.Interpolate(transform, Tween.Properties.POSITION, lStartPosition, lEndPosition, lDuration, Ease.Flat);
        _tween.Interpolate(transform, Tween.Properties.POSITION, lEndPosition, lEndPosition + Vector3.right, lDuration, Ease.Flat, lDuration);
        _tween.Interpolate(transform, Tween.Properties.ROTATION, transform.rotation, transform.rotation * Quaternion.AngleAxis(180f, Vector3.up), lDuration, Ease.Flat, lDuration);

        _tween.Interpolate(_otherCube, Tween.Properties.POSITION, lStartPosition, lStartPosition - Vector3.right, lDuration, Ease.Flat, lDuration);

        _tween.Start();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) _tween.Start();
    }
}