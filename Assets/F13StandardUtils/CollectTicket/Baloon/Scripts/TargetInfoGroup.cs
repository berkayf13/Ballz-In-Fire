using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TargetInfoGroup : MonoBehaviour
{
    [SerializeField] private GameObject _singleBalloon, _bigBalloon;
    [SerializeField] private TextMeshProUGUI _singleMoneyText, _bigMoneyText;
    [SerializeField] private Image _singleSpecialOrder,_bigSpecialOrder;
    [SerializeField] private List<TextMeshProUGUI> _idModifiers;
    [SerializeField] private List<Image> _imgModifiers;

    public void SetAppear(List<BaseSeatModifier> ml, bool o, int money)
    {
        if (ml.Count > 0)
        {
            _bigBalloon.SetActive(true);
            _bigMoneyText.text = "$" + money;
            if (o) _bigSpecialOrder.transform.parent.gameObject.SetActive(true);
            if (SeatModifierSpriteService.Instance.TryGetSprite(out Sprite[] sprites, ml.ToArray()))
            {
                for (int i = 0; i < sprites.Length; i++)
                {
                    _imgModifiers[i].gameObject.SetActive(true);
                    _imgModifiers[i].sprite = sprites[i];
                }
            }
            else
            {
                for (var i = 0; i < ml.Count; i++)
                {
                    _idModifiers[i].gameObject.SetActive(true);
                    _idModifiers[i].text = ml[i].id;
                }
            }
        }
        else
        {
            _singleBalloon.SetActive(true);
            _singleMoneyText.text = "$" + money;
            if (o) _singleSpecialOrder.transform.parent.gameObject.SetActive(true);

        }

    }
}