using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable] public class ChronometerEvent : UnityEvent<Chronometer> { }

public class Chronometer : MonoBehaviour
{
    
    [SerializeField] private Image _fill;
    
    public float duration=2;
    
    [SerializeField,ReadOnly] private float _passedTime;
    [SerializeField,ReadOnly] private bool _isPlaying = false;

    public ChronometerEvent OnStart=new ChronometerEvent();
    public ChronometerEvent OnResume=new ChronometerEvent();
    public ChronometerEvent OnPlay=new ChronometerEvent();
    public ChronometerEvent OnPause=new ChronometerEvent();
    public ChronometerEvent OnUpdate=new ChronometerEvent();
    public ChronometerEvent OnPauseUpdate=new ChronometerEvent();
    public ChronometerEvent OnComplete=new ChronometerEvent();
    public ChronometerEvent OnReset=new ChronometerEvent();


    public bool IsPlaying => _isPlaying;
    public float PassedTime => _passedTime;
    public float RemainingTime => Mathf.Clamp(duration - _passedTime,0,float.MaxValue);
    public float CompleteRatio => Mathf.Clamp(PassedTime/duration, 0, 1);
    public bool IsCompleted => CompleteRatio == 1f;


    [Button]
    public void ResetAndPlay()
    {
        ResetTime();
        _isPlaying = true;
        OnPlay.Invoke(this);
        OnStart.Invoke(this);
    }
    
    [Button]
    public void Resume()
    {
        _isPlaying = true;
        OnPlay.Invoke(this);
        OnResume.Invoke(this);
    }
    [Button]
    public void Pause()
    {
        _isPlaying = false;
        OnPause.Invoke(this);
    }
    [Button]
    public void Complete()
    {
        _isPlaying = false;
        _passedTime = duration;
        UpdateUI();
        OnComplete.Invoke(this);
    }
    [Button]
    public void ResetTime()
    {
        _isPlaying = false;
        _passedTime = 0;
        UpdateUI();
        OnReset.Invoke(this);
    }
    
    private void Update()
    {
        if (_isPlaying)
        {
            _passedTime += Time.deltaTime;
            UpdateUI();
            OnUpdate.Invoke(this);
            if(IsCompleted)
                Complete();
        }
        else
        {
            OnPauseUpdate.Invoke(this);
        }
    }

    private void UpdateUI()
    {
        _fill.fillAmount = CompleteRatio;
    }
}


