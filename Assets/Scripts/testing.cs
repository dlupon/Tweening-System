using UnityEngine;
using UnBocal.TweeningSystem;
using UnBocal.TweeningSystem.Easing;

// --~~~~======# Author : Lupon Dylan #======~~~~~~--- //
// --~~~~======# Date   : 00 / 00 / 2000 #======~~~~-- //
public class testing : MonoBehaviour
{
	Tween _tween = new Tween();
    [SerializeField] private AnimationCurve _curve;

	// ----------------~~~~~~~~~~~~~~~~~~~==========================# // Unity   
	private void Start()
	{
		Vector3 lStartPosition = Vector3.zero;
		Vector3 lEndPosition = Vector3.up * 3;
		float lDuration = 2f;

        _tween.Interpolate(transform, Tween.Properties.POSITION, lStartPosition, lEndPosition, lDuration, Ease.Flat);

        _tween.Start();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) _tween.Start();
    }
}