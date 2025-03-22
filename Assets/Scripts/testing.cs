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
            _tween.Empty();
            transform.localScale = Vector3.one * 2f;
            _tween.localScale(transform, Vector3.one * 2f, Vector3.one, 1f, EaseType.InOutElastic, lDeplay);
            _tween.Start();
        }

        if (Input.GetMouseButtonDown(1))
        {
            Tween.RemoveAll(transform);
        }
    }
}