using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using F13StandardUtils.CrowdDynamics.Scripts;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _GAME.Scripts.Player
{
    public abstract class ObjectMoveTrigger<T> : MonoBehaviour where T: MonoBehaviour
    {
        public static float MOVE_RANDOMIZATION = 5f;
        [SerializeField] private Collider _triggerCollider;

        [SerializeField] private List<Transform> _destinations=new List<Transform>();
        [SerializeField] private PlayerType _triggerPlayer;
        public Collider TriggerCollider => _triggerCollider;
        public List<Transform> Destinations => _destinations;
        public PlayerType TriggerPlayer=>_triggerPlayer;
        public bool IsMove => _isMove;
        
        private List<Vector3> defaultLocalPosList = new List<Vector3>();
        private bool _isMove = false;
        private void Start()
        {
            StoreStartPositions();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals(_triggerPlayer.ToString()))
            {
                Move();
            }
        }

        private void StoreStartPositions()
        {
            var count = Count();
            for (var index = 0; index < count; index++)
            {
                var obj = GetObj(index);
                defaultLocalPosList.Add(obj.transform.localPosition);
            }
        }

        [Button]
        private void ResetPositions()
        {
            StopAllCoroutines();
            _isMove = false;
            _triggerCollider.enabled = true;
            var count = Count();
            for (var index = 0; index < count; index++)
            {
                GetObj(index).transform.localPosition = defaultLocalPosList[index];
            }
        }


        [Button]
        public void Move()
        {
            StopAllCoroutines();
            _isMove = true;
            _triggerCollider.enabled = false;
            var count = Count();
            for (var index = 0; index < count; index++)
            {
                var obj = GetObj(index);
                var randList = new List<Vector3>();
                _destinations.ForEach(d =>
                {
                    randList.Add(CrowdUtils.RandomNormalizedVector(randY: false) *
                                 Random.Range(0f, MOVE_RANDOMIZATION));
                });
                StartCoroutine(MoveCoroutine(obj, randList));
            }
        }

        private IEnumerator MoveCoroutine(T obj,List<Vector3> randList)
        {
            OnObjectAtStart(obj);
            var index = 0;
            OnObjectMoveDestinationIndexChanged(obj, index);
            var lastLookRotation = Quaternion.identity;
            while (index<_destinations.Count && IsMoveContinue(obj))
            {
                if (_isMove)
                {
                    var localDest = obj.transform.parent.InverseTransformPoint(_destinations[index].position+randList[index]);
                    var localPos = obj.transform.localPosition;
                    var diff = localDest-localPos;
                    var speed=Speed(obj);
                    var rotateDuration = 10f / (speed==0f?0.00000001f:speed);
                    var translateAmount = Vector3.forward * Time.deltaTime * speed;
                    if (diff.magnitude > 2* Mathf.PI * translateAmount.magnitude*rotateDuration)
                    {
                        var direction = diff.normalized;
                        if (IsLookContinue(obj))
                        {
                            var lookRotation = Quaternion.LookRotation(direction);
                            if (lookRotation != lastLookRotation)
                            {
                                obj.transform.DOLocalRotateQuaternion(lookRotation,rotateDuration);
                                lastLookRotation = lookRotation;
                            }
                        }

                        obj.transform.Translate(translateAmount);
                    }
                    else
                    {
                        index++;
                        OnObjectMoveDestinationIndexChanged(obj, index);
                    }
                }
                OnObjectUpdate(obj);
                yield return null;
            }

            if (IsMoveContinue(obj))
                OnObjectAtFinish(obj);

        }

        protected abstract List<T> ObjectList();
        protected abstract T GetObj(int index);
        protected abstract int Count();
        protected abstract float Speed(T obj);
        protected abstract void OnObjectAtStart(T obj);
        protected abstract void OnObjectUpdate(T obj);
        protected abstract void OnObjectAtFinish(T obj);
        protected abstract void OnObjectMoveDestinationIndexChanged(T obj,int destinationIndex);
        protected abstract bool IsMoveContinue(T obj);
        protected abstract bool IsLookContinue(T obj);

    }
}
