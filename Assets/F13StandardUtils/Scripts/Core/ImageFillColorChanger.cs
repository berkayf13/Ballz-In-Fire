using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace F13StandardUtils.Scripts.Core
{
    public class ImageFillColorChanger : MonoBehaviour
    {
        [SerializeField] private Image _fillImage;
        [SerializeField] private List<Color> _colors=new List<Color>();

        private float _lastFillAmount;
        private void Update()
        {
            if (_lastFillAmount!=_fillImage.fillAmount)
            {
                _lastFillAmount = _fillImage.fillAmount;
                var ratio = _lastFillAmount;
                var floatIndex = (ratio*_colors.Count);
                var index = Mathf.Clamp((int) floatIndex,0,_colors.Count-1);
                var coloRatio = floatIndex - index;
                var beforeIndex = Mathf.Clamp(index - 1, 0, int.MaxValue);
                var color = Color.Lerp(_colors[beforeIndex], _colors[index], coloRatio);
                _fillImage.color = color;
            }
        }
    
    }
}