using UnityEngine;
using UnBocal.TweeningSystem;
using UnBocal.TweeningSystem.Easing;


public class testing : MonoBehaviour
{
    Tween _tween = new Tween();
    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            float lDeplay = Random.value * 1.5f;
            _tween.Clear();
            _tween.localRotation(transform, Quaternion.identity, Quaternion.identity * Quaternion.AngleAxis(180f, Vector3.up), 1f, EaseFunction.Flat, lDeplay);
            _tween.localScale(transform, Vector3.one * 2f, Vector3.one, 1f, EaseFunction.Flat, lDeplay);
            _tween.Start();
        }
    }
}