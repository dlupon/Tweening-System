using System;
using System.Text;
using UnityEngine;

// --~~~~======# Author : Lupon Dylan #======~~~~~~--- //
// --~~~~======# Date   : 03 / 05 / 2025 #======~~~~-- //
namespace UnBocal.TweeningSystem
{
    public class TweenManager : MonoBehaviour
    {   
        Tween _tween = new Tween();

        private void Start()
        {
            _tween.Interpolate(transform, Tween.Properties.POSITION, Vector3.left * 2, Vector3.right * 2, 3f);
        }

    
        private void Update()
        {
            _tween.UpdateInterpolation();
        }
    }
}