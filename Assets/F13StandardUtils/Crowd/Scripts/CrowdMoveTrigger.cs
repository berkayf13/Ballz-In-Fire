using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using F13StandardUtils.CrowdDynamics.Scripts;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _GAME.Scripts.Player
{
    [System.Serializable]
    public class CrowdMove
    {
        public Transform target;
        public bool randX = false;
        public bool randZ = false;

    }
    
    public class CrowdMoveTrigger : MonoBehaviour
    {
        public static float MOVE_RANDOMIZATION = 2.5f;
        [SerializeField] private Collider _triggerCollider;
        [SerializeField] private CrowdManager _moveCrowd;
        [SerializeField] private List<CrowdMove> _destinations=new List<CrowdMove>();
        [SerializeField] private PlayerType _triggerPlayer;
        [SerializeField] private bool isMove = false;

        private float Speed => FinishManager.Instance.IsFinishStart ? CrowdMember.ENEMY_FINISH_SPEED : CrowdMember.ENEMY_IN_GAME_SPEED;
        

        public CrowdManager MoveCrowd => _moveCrowd;
        private int defaultCount;
        private List<Vector3> defaultLocalPosList = new List<Vector3>();

        private void Start()
        {
            defaultCount = _moveCrowd.Count;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals(_triggerPlayer.ToString()))
            {
                _triggerCollider.enabled = false;
                Move();
            }
        }

        private void StoreStartPositions()
        {
            foreach (var crowdMember in _moveCrowd.memberList)
            {
                defaultLocalPosList.Add(crowdMember.transform.localPosition);
            }
        }

        [Button]
        private void ResetPositions()
        {
            StopAllCoroutines();
            _moveCrowd.UpdateCount(defaultCount);
            for (var index = 0; index < _moveCrowd.memberList.Count; index++)
            {
                var crowdMember = _moveCrowd.memberList[index];
                crowdMember.CrowdMemberAnimController.isMove = false;
                crowdMember.transform.localPosition = defaultLocalPosList[index];
            }
        }


        [Button]
        public void Move()
        {
            StoreStartPositions();
            StopAllCoroutines();
            isMove = true;
            _moveCrowd.isPulling = false;
            foreach (var crowdMember in _moveCrowd.memberList)
            {
                var randList = new List<Vector3>();
                _destinations.ForEach(d=>
                {
                    randList.Add(CrowdUtils.RandomNormalizedVector(randY: false) *
                                 Random.Range(0f, MOVE_RANDOMIZATION) + crowdMember.transform.localPosition);
                });
                StartCoroutine(MoveCoroutine(crowdMember,randList));
            }
        }

        private IEnumerator MoveCoroutine(CrowdMember enemy,List<Vector3> randList)
        {
            var index = 0;
            enemy.CrowdMemberAnimController.isMove = true;
            var lastLookRotation = Quaternion.identity;
            while (index<_destinations.Count && !enemy.isDeath)
            {
                if (isMove)
                {
                    var localDest = enemy.transform.parent.InverseTransformPoint(_destinations[index].target.position)+randList[index];
                    var localPos = enemy.transform.localPosition;
                    var diff = localDest-localPos;
                    var rotateDuration = 10f / (Speed==0f?0.00000001f:Speed);
                    var translateAmount = Vector3.forward * Time.deltaTime * Speed;
                    if (diff.magnitude > 2* Mathf.PI * translateAmount.magnitude*rotateDuration*2)
                    {
                        var direction = diff.normalized;
                        var lookRotation = Quaternion.LookRotation(direction);
                        if (lookRotation != lastLookRotation)
                        {
                            enemy.transform.DOLocalRotateQuaternion(lookRotation,rotateDuration);
                            lastLookRotation = lookRotation;
                        }
                        enemy.transform.Translate(translateAmount);
                    }
                    else
                    {
                        index++;
                    }
                }
                yield return null;
            }

            if (!enemy.isDeath)
            {
                enemy.CrowdMemberAnimController.isMove = false;
                if(!FinishManager.Instance.IsFinishStart) enemy.owner.Kill(enemy);
            }

        }
    }
}
