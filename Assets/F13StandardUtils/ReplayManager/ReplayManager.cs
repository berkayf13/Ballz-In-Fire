using System;
using System.Collections.Generic;
using System.Linq;
using _GAME.Scripts.Core;
using F13StandardUtils.Scripts.Core;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

public class ReplayManager : Singleton<ReplayManager>
{
    public float delay = 1f;
    public ReplayData replayData;
    [SerializeField] private bool recordOnAwake = false;
    [SerializeField] private bool playOnAwake = false;
    [SerializeField,ShowIf(nameof(playOnAwake))] private string playOnAwakeReplay;
    [SerializeField,ReadOnly] private bool isRecording = false;
    [SerializeField,ReadOnly] private bool isPlaying = false;
    [SerializeField,ReadOnly] private float startTime;
    [SerializeField,ReadOnly] private Replay _current;

    private void Start()
    {
        PlayOrRecordOnAwakeProcess();
    }
    
    private void Update()
    {
        KeyboardShortcuts();
        RecordProcess();
        PlayProcess();
    }
    
    private void PlayOrRecordOnAwakeProcess()
    {
        this.StartWaitForSecondCoroutine(delay, () =>
        {
            if (recordOnAwake) BeginRecording();
            else if (playOnAwake)
            {
                if (!playOnAwakeReplay.IsNullOrWhitespace())
                {
                    SetReplay(playOnAwakeReplay);
                    Play();
                }
            }
        });
    }
    
    private void KeyboardShortcuts()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (isPlaying) Stop();
            else Play();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isRecording) EndRecoring();
            else BeginRecording();
        }
    }
    
    private void RecordProcess()
    {
        if (isRecording)
        {
            if(_current.record==null ) _current.record=new List<ReplayInputRecord>();
            if (Input.GetMouseButtonDown(0))
            {
                var input = new ReplayInputRecord();
                input.time = Time.time - startTime;
                input.inputState = InputState.Down;
                input.mousePosition = Input.mousePosition;
                _current.record.Add(input);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                var input = new ReplayInputRecord();
                input.time = Time.time - startTime;
                input.inputState = InputState.Up;
                input.mousePosition = Input.mousePosition;
                _current.record.Add(input);
            }

            if (Input.GetMouseButton(0))
            {
                var input = new ReplayInputRecord();
                input.time = Time.time - startTime;
                input.inputState = InputState.Stay;
                input.mousePosition = Input.mousePosition;
                _current.record.Add(input);
            }
        }
    }
    
    private void PlayProcess()
    {
        if(!GameController.Instance.IsPlaying) Stop();
        if (isPlaying)
        {
            var time = Time.time-startTime;
            if (_playing.record.Any())
            {
                var record = _playing.record.First();
                if (record.time-time<= 0.01f)
                {
                    _playing.record.RemoveAt(0);
                    Debug.Log("Remove");
                    SimulateInput(record);
                }
            }
            else
            {
                Debug.Log("Replay: "+_playing.id+" finished");
                isPlaying = false;
            }

        }
    }

    [Button]
    public void BeginRecording()
    {
        isPlaying = false;
        isRecording = true;
        startTime = Time.time;
        _current=new Replay();
        _current.id = "record" + Time.time;
        if(!GameController.Instance.IsPlaying) GameController.Instance.EnterGamePlayState();
    }
    [Button]
    public void EndRecoring()
    {
        isRecording = false;
        // replayData.replays.RemoveAt(replayData.replays.Count-1);
        replayData.replays.Add(_current);
    }

    [Button]
    public void SetReplay(string id)
    {
        _current = replayData.replays.Find(r=>r.id.Equals(id));
    }

    private Replay _playing;
    [Button]
    public void Play()
    {
        if (!_current.record.Any())
        {
            Debug.LogWarning("There is no replay record");
            return;
        }
        if(!GameController.Instance.IsPlaying) GameController.Instance.EnterGamePlayState();

        _playing = _current.Clone();
        isPlaying = true;
        isRecording = false;
        startTime = Time.time;
        Debug.Log("Replay: "+_playing.id+" started");
        
    }

    [Button]
    public void Stop()
    {
        isPlaying = false;
    }
    

    [Button]
    public void SimulateInput(ReplayInputRecord record)
    {
        switch (record.inputState)
        {
            case InputState.Down:
                SwerveController.Instance?.Simulate(true,true,record.mousePosition);
                break;
            case InputState.Up:
                SwerveController.Instance?.Simulate(false,false,record.mousePosition);
                break;
            case InputState.Stay:
                SwerveController.Instance?.Simulate(false,true,record.mousePosition);
                break;
            case InputState.None:
                SwerveController.Instance?.Simulate(false,false,record.mousePosition);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    [Button]
    private void Restart()
    {
        StopAllCoroutines();
        Stop();
        GameController.Instance.SetLevel(GameController.Instance.Level);
        PlayOrRecordOnAwakeProcess();
    }

    public void SetPlayOnAwake(string id)
    {
        playOnAwake = true;
        recordOnAwake = false;
        playOnAwakeReplay = id;
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }
    
}
