using UnityEngine;
using UnBocal.TweeningSystem;
using UnBocal.TweeningSystem.Easing;


public class testing : MonoBehaviour
{
	Tween _tween = new Tween();

    // ----------------~~~~~~~~~~~~~~~~~~~==========================# // Unity   
    private void Start()
	{
        _tween.Interpolate(transform, nameof(transform.localScale), Vector3.one * 2f, Vector3.one, 1, EaseType.InOutElastic, pDelay : Random.value);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) _tween.Start();
    }
}