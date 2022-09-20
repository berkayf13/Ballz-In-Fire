using DG.Tweening;
using F13StandardUtils.Scripts.Core;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;


public class SimpleCursor : BaseObjectUpdater<bool>
{

    [SerializeField] private bool useUnityCursor = false;
    [SerializeField,ShowIf(nameof(useUnityCursor))] private Texture2D _notPressedTexture;
    [SerializeField,ShowIf(nameof(useUnityCursor))] private Texture2D _pressedTexture;

    [SerializeField,HideIf(nameof(useUnityCursor))] private Image _cursor;
    [SerializeField,HideIf(nameof(useUnityCursor))] private Image _shadowCursor;
    [SerializeField,HideIf(nameof(useUnityCursor))] private Sprite _notPressed;
    [SerializeField,HideIf(nameof(useUnityCursor))] private Sprite _pressed;
    
    [SerializeField,HideIf(nameof(detailedCursor))] private Vector3 offset=Vector3.zero;
    [SerializeField] private Vector3 _notPressedScale=Vector3.one*1.1f;
    [SerializeField] private Vector3 _pressedScale=Vector3.one;

    [SerializeField] private bool detailedCursor=false;
    [SerializeField,ShowIf(nameof(detailedCursor))] private Vector3 _shadowOffset=Vector3.zero;
    [SerializeField,ShowIf(nameof(detailedCursor))] private Vector3 _cursorNotPressedOffset=Vector3.one*50;
    [SerializeField,ShowIf(nameof(detailedCursor))] private Vector3 _cursorPressedOffset=Vector3.zero;
    [SerializeField,ShowIf(nameof(detailedCursor))] private float _pressAnimDuration = 0.25f;

    protected override bool Value => Input.GetMouseButton(0);

    public Sprite NotPressed
    {
        get => _notPressed;
        set
        {
            _notPressed = value;
            OnValueUpdate();
        }
    }

    public Sprite Pressed
    {
        get => _pressed;
        set
        {
            _pressed = value;
            OnValueUpdate();
        }
    }


    private Vector3 _cursorCurrentOffset;
    protected override void Update()
    {
        base.Update();
        
        if (_cursor)
        {
            _cursor.transform.position = Input.mousePosition+ (detailedCursor? _cursorCurrentOffset : offset);
        }
        if (_shadowCursor)
        {
            _shadowCursor.transform.position = Input.mousePosition+ (detailedCursor? _shadowOffset : offset);
        }
        
    }

    protected override void OnValueUpdate()
    {
        var isPressed = Value;
        if (_cursor)
        {
            _cursor.enabled = !useUnityCursor;
            if (isPressed)
            {
                _cursor.sprite = Pressed;
                _cursor.transform.DOScale(_pressedScale, _pressAnimDuration);
                DOTween.To(()=>_cursorCurrentOffset, x => _cursorCurrentOffset = x, _cursorPressedOffset, _pressAnimDuration);

            }
            else
            {
                _cursor.sprite = NotPressed;
                _cursor.transform.DOScale(_notPressedScale, _pressAnimDuration);
                DOTween.To(()=>_cursorCurrentOffset, x => _cursorCurrentOffset = x, _cursorNotPressedOffset, _pressAnimDuration);
            }
        }

        if (_shadowCursor)
        {
            _shadowCursor.enabled = !useUnityCursor;
            _shadowCursor.sprite = isPressed? Pressed : NotPressed;
            _shadowCursor.transform.DOScale(isPressed? _pressedScale:_notPressedScale , _pressAnimDuration);

        }
        if(useUnityCursor) Cursor.SetCursor(isPressed?_pressedTexture:_notPressedTexture,offset, CursorMode.Auto);
    }


    
}
