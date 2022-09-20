using System;
using System.Collections.Generic;
using F13StandardUtils.Scripts.Core;
using UnityEngine;

namespace F13StandardUtils.Crowd.Scripts
{
    public class CrowdMemberAnimController : UnitAnimController
    {
        public PlayerType playerType;
        private Material _defaultMaterial;
        [SerializeField] private Material _deathMaterial;
        [SerializeField] private Material _shineMaterial;
        [SerializeField] private Renderer _renderer;
        

        public bool isMove = false;
        private bool lastIsMoving = true;
        private bool isDeath = false;
        public List<AnimatorOverrideController> animatorControllerList = new List<AnimatorOverrideController>();
        
        
        private void Awake()
        {
            // RandomMirror();
            RandomSpeed();
            _defaultMaterial = _renderer.sharedMaterial;

        }
        
        private void OnEnable()
        {
            curentAnimState=string.Empty;
            ResetStates();
            _renderer.sharedMaterial = _defaultMaterial;
            UpdateAnim();
        }

        private void ResetStates()
        {
            isMove = false;
            lastIsMoving = true;
            isDeath = false;
        }
        
        private void LateUpdate()
        {
            if (MoveZ.Instance && playerType == PlayerType.Player)
            {
                isMove = MoveZ.Instance.isMove;
            }
            if (lastIsMoving != isMove)
            {
                UpdateAnim();
            }
        }

        private void UpdateAnim()
        {
            lastIsMoving = isMove;
            if (isMove)
                Run();
            else
                Idle();
        }


        private void RandomMirror()
        {
            var randomBool = Utils.RandomBool();
            SetMirror(randomBool);
        }
        private void RandomSpeed()
        {
            var randomSpeed = UnityEngine.Random.Range(0.85f, 1f);
            SetSpeed(randomSpeed);
        }
        
        public override void Death(Action onMidAction = null, Action onEndAction = null)
        {
            base.Death(onMidAction, onEndAction);
            isDeath = true;
            _renderer.sharedMaterial = _deathMaterial;
        }
        
        public void Shine(float sec=0.05f)
        {
            _renderer.sharedMaterial = _shineMaterial;
            this.StartWaitForSecondCoroutine(sec,() =>
            {
                _renderer.sharedMaterial = isDeath?_deathMaterial : _defaultMaterial;
            });
        }
    
    }
}