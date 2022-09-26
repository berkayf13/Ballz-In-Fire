using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Door : MonoBehaviour
{
    private enum DoorType { Range,FireRate,Bullet,Bouncy }

    [SerializeField] private float _value;
    [SerializeField] private TextMeshProUGUI _tmpui;
    [SerializeField] private DoorType _type;

    private void Awake()
    {
        UpdateText();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerFire player))
        {
            switch (_type)
            {
                case DoorType.Range:
                    player.bulletRange += _value;
                    break;
                case DoorType.FireRate:
                    player.bulletFireRate += _value;
                    break;
                case DoorType.Bullet:
                    player.bulletCount += _value;
                    break;
                case DoorType.Bouncy:
                    player.bulletBouncy += _value;
                    break;
            }
            Destroy(gameObject);
        }
    }

    private void UpdateText()
    {
        switch (_type)
        {
            case DoorType.Range:
                _tmpui.text = " RANGE";
                break;
            case DoorType.FireRate:
                _tmpui.text = " FIRERATE";
                break;
            case DoorType.Bullet:
                _tmpui.text = " BULLET";
                break;
            case DoorType.Bouncy:
                _tmpui.text = " BOUNCY";
                break;
        }

    }
}