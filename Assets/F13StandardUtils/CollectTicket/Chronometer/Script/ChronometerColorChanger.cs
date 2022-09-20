using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChronometerColorChanger : MonoBehaviour
{
    [SerializeField] private Image _fillImage;
    [SerializeField] private Chronometer _chronometer;
    [SerializeField] private List<Color> _colors=new List<Color>();

    private void Update()
    {
        if (_chronometer.IsPlaying)
        {
            var ratio = _chronometer.CompleteRatio;
            var floatIndex = (ratio*_colors.Count);
            var index = Mathf.Clamp((int) floatIndex,0,_colors.Count-1);
            var coloRatio = floatIndex - index;
            var beforeIndex = Mathf.Clamp(index - 1, 0, int.MaxValue);
            var color = Color.Lerp(_colors[beforeIndex], _colors[index], coloRatio);
            _fillImage.color = color;
        }
    }
    
}
