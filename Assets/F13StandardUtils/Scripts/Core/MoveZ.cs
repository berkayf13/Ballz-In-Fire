using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace F13StandardUtils.Scripts.Core
{
    public class MoveZ : Singleton<MoveZ>
    {
        public static float VELOCITY = -17;
        public static float DOWNHILL_VELOCITY_MULTIPLIER = 2f;
        public static float ROTATE_DURATION = 0.7f;

        [HideInInspector] public bool isDownHill = false;
        public bool IsMovingOrRotating => isMove || isRotating;
        public bool isMove = true;
        [SerializeField] private List<Transform> moveList=new List<Transform>();
        [SerializeField] private float _multiplier = 1;
        public float TrueSpeed => VELOCITY * _multiplier;

        public float Multiplier => _multiplier;
        public bool IsRotating => isRotating;

        private bool isRotating = false;
        

        void Update()
        {
            if(isMove) moveList.ForEach(t=>t.position= t.position+(Vector3.forward*TrueSpeed*Time.deltaTime));
        }

        public void SetMultiplier(float multiplier)
        {
            _multiplier = multiplier;
        }

        public void RestoreMultiplierDefault()
        {
            SetMultiplier(1);
        }

        [Button]
        public void RotateAround(Vector3 axis, Transform rotatePivot,float rotate=90f,bool stopMoveWhenRotate=true,Action onStart=null,Action onComplete=null)
        {
            foreach (var move in moveList)
            {
                var t = move;
                var angle = 0f;
                DOTween.To(() => angle, x =>
                {
                    var rotateAmount = Time.deltaTime * rotate/ROTATE_DURATION ;
                    if (rotate - angle < rotateAmount)
                        rotateAmount = rotate - angle;
                    angle += rotateAmount;
                    t.RotateAround(rotatePivot.position,axis, rotateAmount);
                }, rotate, ROTATE_DURATION).OnStart(() =>
                {
                    isRotating = true;
                    if(stopMoveWhenRotate) isMove = false;
                    onStart?.Invoke();
                }).OnComplete(() =>
                {
                    isRotating = false;
                    if(stopMoveWhenRotate) isMove = true;
                    onComplete?.Invoke();
                });
            }
        }
        
    }
}
