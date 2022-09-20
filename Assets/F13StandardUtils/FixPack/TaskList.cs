using System.Collections;
using System.Collections.Generic;
using F13StandardUtils.Scripts.Core;
using UnityEngine;
using UnityEngine.Events;

public class TaskList : MonoBehaviour
{
    [SerializeField] private UnityEvent OnProcess=new UnityEvent();

    public void Process(float delay)
    {
        this.StartWaitForSecondCoroutine(delay, OnProcess.Invoke);
    }

    public void ProcessImmediate()
    {
        OnProcess.Invoke();
    }

}
