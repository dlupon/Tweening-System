using UnityEngine;
using UnBocal.TweeningSystem;


public class testing : MonoBehaviour
{
    Tween _tween = new Tween();

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            float lDeplay = Random.value * .5f;
            _tween.Clear();
            _tween.localRotation(transform, Quaternion.identity, Quaternion.identity * Quaternion.AngleAxis(180f, Vector3.up), 1f, EaseType.Flat, lDeplay);
            _tween.localScale(transform, Vector3.one * 2f, Vector3.one, 1f, EaseType.Flat, lDeplay);
            _tween.Start();
        }
    }
}