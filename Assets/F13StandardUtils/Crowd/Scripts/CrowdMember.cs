using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using F13StandardUtils.Crowd.Scripts;
using F13StandardUtils.Scripts.Core;
using UnityEngine;

namespace F13StandardUtils.CrowdDynamics.Scripts
{
    public class CrowdMember : MonoBehaviour
    {
        public static float DEFAULT_SCALE = 1.25f;
        public static float ENEMY_IN_GAME_SPEED = 20f;
        public static float ENEMY_FINISH_SPEED = 23f;
        
        [SerializeField] private PlayerType _type;
        [SerializeField] private CapsuleCollider _triggerCollider;
        [SerializeField] private CapsuleCollider _collisionCollider;
        [SerializeField] private Rigidbody _rigid;
        [SerializeField] private List<CrowdMemberAnimController> _playerTypeObjects;
        public bool isDeath = false;

        public bool isPulling;
        public float PullTime => 0.75f;
        
        public CrowdMemberAnimController CrowdMemberAnimController => _playerTypeObjects[(int)PlayerType];


        [HideInInspector] public CrowdManager owner;
        private float lastPullTime;
        private Vector3 _destinationPos;


        public PlayerType PlayerType => _type;
        

        private void OnEnable()
        {
            SpawnEffect();
            if(_collisionCollider) _collisionCollider.radius = CrowdManager.CIRCULAR_INTERVAL *0.5f * 0.65f;
            if(_collisionCollider) _triggerCollider.radius = CrowdManager.CIRCULAR_INTERVAL *0.5f * .7f; //Dont touch this is not wrong
            Invoke(nameof(EnableTriggerCollider),CrowdManager.DEFAULT_PULL_DELAY);
            DestinationPos=Vector3.zero;
            isDeath = false;

        }

        private void OnDisable()
        {
            DisableTriggerCollider();
            owner = null;
        }

        public Vector3 DestinationPos
        {
            get => _destinationPos;
            set
            {
                _destinationPos = value;
                Pull();
            }
        }

        private void FixedUpdate()
        {
            PullProcess();
        }

        private void PullProcess()
        {
            if (Time.time - lastPullTime > PullTime || _destinationPos==transform.localPosition)
            {
                isPulling = false;
                if(_collisionCollider) _collisionCollider.enabled = true;

            }
            if (!isDeath && isPulling)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, DestinationPos, Time.deltaTime * CrowdManager.PULL_POWER);
                if(_collisionCollider) _collisionCollider.enabled = false;
            }

        }


        public void Pull()
        {
            isPulling = true;
            lastPullTime = Time.time;
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other && other.CompareTag(_type.Opposite().ToString()))
            {
                owner?.Kill(this);
            }
        }


        
        
        public void SetPlayerType(PlayerType playerType)
        {
            _type = playerType;
            UpdatePlayerType();
        }
        

        

        
        private void SpawnEffect()
        {
            transform.DOScale(DEFAULT_SCALE * 0.5f, 0);
            transform.DOScale(DEFAULT_SCALE, 0.5f);
        }
        
        private void UpdatePlayerType()
        {
            gameObject.tag = _type.ToString();
            for (var i = 0; i < _playerTypeObjects.Count; i++)
            {
                _playerTypeObjects[i].gameObject.SetActive(i==(int)_type);
            }
        }
        
        private void EnableTriggerCollider()
        {
            _triggerCollider.enabled = true;
            if(_collisionCollider) _collisionCollider.enabled = true;
        }
        
        private void DisableTriggerCollider()
        {
            _triggerCollider.enabled = false;
            if(_collisionCollider) _collisionCollider.enabled = false;
        }


        public void Death(bool isInstant,Action onEndAction)
        {
            isDeath = true;
            transform.SetParent(owner.transform.parent);
            _triggerCollider.enabled = false;
            if (isInstant)
            {
                onEndAction?.Invoke();
            }
            else
            {
                CrowdMemberAnimController.Death(() =>
                {
                    transform.DOScale(0, 0.3f).OnComplete(() =>
                    {
                        onEndAction?.Invoke();
                    });
                });
            }

        }
        
        private bool isJumping = false;

        public void Jump(float jumpPower)
        {
            if(isJumping) return;
            StartCoroutine(JumpCoroutine(jumpPower));
        }

        private IEnumerator JumpCoroutine(float jumpPower)
        {
            isJumping = true;
            isPulling = false;
            _rigid.isKinematic = false;
            _rigid.useGravity = true;
            _rigid.AddForce(Vector3.up * jumpPower);
            _triggerCollider.enabled = false;
            yield return new WaitForSeconds(0.1f);
            yield return new WaitUntil(()=>transform.localPosition.y<0.2f);
            isPulling = true;
            _rigid.isKinematic = true;
            _rigid.useGravity = false;
            _triggerCollider.enabled = true;
            isJumping = false;
        }
    }
}

