using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace F13StandardUtils.Scripts.Core
{
    public enum DoAnimType
    {
        Position,
        LocalPosition,
        Rotation,
        LocalRotation,
        Scale,
        Wait,
    }

    public enum DoAnimProcess
    {
        NewTarget,
        Sum
    }

    [System.Serializable]
    public class DoAnimStep
    {
        public bool isWait = true;
        public DoAnimType type = DoAnimType.Position;
        public float duration = 1;

        [HideIf(nameof(type), DoAnimType.Wait)]
        public DoAnimProcess process = DoAnimProcess.NewTarget;

        [HideIf(nameof(type), DoAnimType.Wait)]
        public Vector3 vector;

        [HideIf(nameof(type), DoAnimType.Wait)]
        public Ease ease = Ease.Linear;
    }

    [System.Serializable]
    public class DoAnimInfo
    {

        public bool onStart;
        public bool isLoop;
        [ShowIf(nameof(isLoop))] public LoopType loopType = LoopType.Restart;
        public List<DoAnimStep> steps = new List<DoAnimStep>();
    }


    public class DoAnim : MonoBehaviour
    {
        public DoAnimInfo animInfo;
        private Sequence _sequence;

        private Vector3 deflocalPosition;
        private Vector3 deflocalRotation;
        private Vector3 deflocalScale;

        private void Awake()
        {
            deflocalPosition = transform.localPosition;
            deflocalRotation = transform.eulerAngles;
            deflocalPosition = transform.localScale;

        }

        private void OnEnable()
        {
            if(_sequence!=null) ResetTransform();
            if (animInfo.onStart) StartAnim();
        }


        [Button]
        public void StartAnim()
        {
            _sequence = DOTween.Sequence();
            foreach (var step in animInfo.steps)
            {
                //Waiting
                if (step.type == DoAnimType.Wait) _sequence.AppendInterval(step.duration);

                else
                {
                    //Append
                    if (step.isWait)
                    {
                        switch (step.type)
                        {
                            case DoAnimType.Position:
                                if (step.process == DoAnimProcess.NewTarget)
                                    _sequence.Append(transform.DOMove(step.vector, step.duration)
                                        .SetEase(step.ease));
                                else if (step.process == DoAnimProcess.Sum)
                                    _sequence.Append(transform
                                        .DOMove(transform.position + step.vector, step.duration)
                                        .SetEase(step.ease));
                                break;
                            case DoAnimType.LocalPosition:
                                if (step.process == DoAnimProcess.NewTarget)
                                    _sequence.Append(transform.DOLocalMove(step.vector, step.duration)
                                        .SetEase(step.ease));
                                else if (step.process == DoAnimProcess.Sum)
                                    _sequence.Append(transform
                                        .DOLocalMove(transform.localPosition + step.vector, step.duration)
                                        .SetEase(step.ease));
                                break;
                            case DoAnimType.Rotation:
                                if (step.process == DoAnimProcess.NewTarget)
                                    _sequence.Append(transform
                                        .DORotate(step.vector, step.duration, RotateMode.FastBeyond360)
                                        .SetEase(step.ease));
                                else if (step.process == DoAnimProcess.Sum)
                                    _sequence.Append(transform
                                        .DORotate(transform.eulerAngles + step.vector, step.duration,
                                            RotateMode.FastBeyond360)
                                        .SetEase(step.ease));
                                break;
                            case DoAnimType.LocalRotation:
                                if (step.process == DoAnimProcess.NewTarget)
                                    _sequence.Append(transform
                                        .DOLocalRotate(step.vector, step.duration, RotateMode.FastBeyond360)
                                        .SetEase(step.ease));
                                else if (step.process == DoAnimProcess.Sum)
                                    _sequence.Append(transform
                                        .DOLocalRotate(transform.localEulerAngles + step.vector, step.duration,
                                            RotateMode.FastBeyond360)
                                        .SetEase(step.ease));
                                print(transform.localEulerAngles + step.vector);
                                break;
                            case DoAnimType.Scale:
                                if (step.process == DoAnimProcess.NewTarget)
                                    _sequence.Append(transform.DOScale(step.vector, step.duration)
                                        .SetEase(step.ease));
                                else if (step.process == DoAnimProcess.Sum)
                                    _sequence.Append(transform
                                        .DOScale(transform.localScale + step.vector, step.duration)
                                        .SetEase(step.ease));
                                break;
                        }
                    }
                    //Join
                    else
                    {
                        switch (step.type)
                        {
                            case DoAnimType.Position:
                                if (step.process == DoAnimProcess.NewTarget)
                                    _sequence.Join(
                                        transform.DOMove(step.vector, step.duration).SetEase(step.ease));
                                else if (step.process == DoAnimProcess.Sum)
                                    _sequence.Join(transform
                                        .DOMove(transform.position + step.vector, step.duration)
                                        .SetEase(step.ease));
                                break;
                            case DoAnimType.LocalPosition:
                                if (step.process == DoAnimProcess.NewTarget)
                                    _sequence.Join(transform.DOLocalMove(step.vector, step.duration)
                                        .SetEase(step.ease));
                                else if (step.process == DoAnimProcess.Sum)
                                    _sequence.Join(transform
                                        .DOLocalMove(transform.localPosition + step.vector, step.duration)
                                        .SetEase(step.ease));
                                break;
                            case DoAnimType.Rotation:
                                if (step.process == DoAnimProcess.NewTarget)
                                    _sequence.Join(transform
                                        .DORotate(step.vector, step.duration, RotateMode.FastBeyond360)
                                        .SetEase(step.ease));
                                else if (step.process == DoAnimProcess.Sum)
                                    _sequence.Join(transform
                                        .DORotate(transform.eulerAngles + step.vector, step.duration,
                                            RotateMode.FastBeyond360)
                                        .SetEase(step.ease));
                                break;
                            case DoAnimType.LocalRotation:
                                if (step.process == DoAnimProcess.NewTarget)
                                    _sequence.Join(transform
                                        .DOLocalRotate(step.vector, step.duration, RotateMode.FastBeyond360)
                                        .SetEase(step.ease));
                                else if (step.process == DoAnimProcess.Sum)
                                    _sequence.Join(transform
                                        .DOLocalRotate(transform.localEulerAngles + step.vector, step.duration,
                                            RotateMode.FastBeyond360)
                                        .SetEase(step.ease));
                                break;
                            case DoAnimType.Scale:
                                if (step.process == DoAnimProcess.NewTarget)
                                    _sequence.Join(transform.DOScale(step.vector, step.duration)
                                        .SetEase(step.ease));
                                else if (step.process == DoAnimProcess.Sum)
                                    _sequence.Join(transform
                                        .DOScale(transform.localScale + step.vector, step.duration)
                                        .SetEase(step.ease));
                                break;
                        }
                    }
                }
            }

            if (animInfo.isLoop) _sequence.SetLoops(-1, animInfo.loopType);
        }

        [Button]
        public void RestartAnim()
        {
            ResetTransform();
            StartAnim();
        }


        [Button]
        public void KillAnim()
        {
            _sequence.Kill();
        }
        
        [Button]
        public void CompleteAnim()
        {
            _sequence.Complete();
        }
        

        [Button]
        public void ResetTransform()
        {
            transform.localPosition = deflocalPosition;
            transform.eulerAngles = deflocalRotation;
            transform.localScale = deflocalPosition;
        }



    }
}