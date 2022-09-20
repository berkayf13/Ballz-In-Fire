using System;
using System.Collections.Generic;
using System.Linq;
using F13StandardUtils.Crowd.Scripts;
using F13StandardUtils.CrowdDynamics.Scripts;
using F13StandardUtils.Scripts.Core;
using Sirenix.OdinInspector;
using UnityEngine;


    public enum FormationType
    {
        Circular,
        Rectangular,
        SwerveRectangular,
        Triangular,
        TriangularTower,
        Hexagon,
        Star
    }
    public enum PlayerType
    {
        Player = 0,
        Enemy = 1
    }

    public static class PlayerTypeExtensions
    {
        public static PlayerType Opposite(this PlayerType playerType)
        {
            var enumCount = Enum.GetValues(typeof(PlayerType)).Length;
            var val = ((int) playerType + 1) % enumCount;
            return (PlayerType) val;
        }
    }

    public class CrowdManager : MonoBehaviour
    {
        public static float PULL_POWER = 10f;
        public static float DEFAULT_PULL_DELAY = .5f;
        public static float CIRCULAR_INTERVAL = 1.7f;
        public static float CIRCULAR_RANDOMIZATION = 0.255f;
        public static float RECTANGULAR_INTERVAL = 4f;
        public float PullDelay => (lastCount > Count) ? DEFAULT_PULL_DELAY : 0.01f;

        public PlayerType type;
        [SerializeField]private FormationType _formationType;
        [SerializeField] private GameObject prefab;
        [SerializeField] private GameObject particlePrefab;
        [HideInEditorMode] public List<CrowdMember> memberList = new List<CrowdMember>();
        public int initialCount;
        public int Count => memberList.Count;
        public FormationType FormationType => _formationType;
        public int lastIncrement;
        public bool isPulling = true;
        

        private void Awake()
        {
            SpawnInitial();
        }

        private void FixedUpdate()
        {
            PullProcess();
        }
        
        [Button]
        public void SetFormation(FormationType formationType)
        {
            _formationType = formationType;
            ForcePullCrowd();
        }
        [Button]
        public void Spawn()
        {
            var crowdMember = PoolManager.Instance.Instantiate<CrowdMember>(prefab);
            crowdMember.enabled = true;
            crowdMember.transform.SetParent(transform);
            crowdMember.owner = this;
            crowdMember.SetPlayerType(type);
            crowdMember.transform.localPosition = CrowdUtils.RandomNormalizedVector() * CIRCULAR_RANDOMIZATION;
            crowdMember.transform.localRotation = Quaternion.identity;
            memberList.Add(crowdMember);
            PullCrowd();
        }

        [Button]
        public void UpdateCount(int newValue)
        {
            var diff = newValue - Count;
            if (diff > 0)
            {
                Spawn(diff);
            }
            else
            {
                for (var i = 0; i < -diff; i++)
                {
                    if (Count > 0)
                        Kill(Count - 1);
                }
            }

            lastIncrement = diff;
        }

        public void Spawn(int count)
        {
            for (var i = 0; i < count; i++)
            {
                Spawn();
            }
        }
        
        [Button]
        public List<List<CrowdMember>> SplitCrowd(params float[] ratios)
        {
            var groups = new List<List<CrowdMember>>();
            for (var i = 0; i < ratios.Length; i++)
            {
                groups.Add(new List<CrowdMember>());
            }
            
            for (var i = 0; i < memberList.Count; i++)
            {
                var result = CrowdUtils.CalculateSplitGroupCircularPositionWithIndex(i,CIRCULAR_INTERVAL,memberList.Count,ratios);
                memberList[i].DestinationPos = result.pos;
                memberList[i].DestinationPos +=CrowdUtils.RandomNormalizedVector(randY: false) * CIRCULAR_RANDOMIZATION;
                groups[result.groupIndex].Add(memberList[i]);
            }
            return groups;
        }

        public void Kill(CrowdMember member,bool instant=true)
        {
            int index = memberList.IndexOf(member);
            if (index >= 0)
            {
                Kill(index,instant);
            }
        }
        [Button]
        public void Kill(int memberIndex,bool instant=true)
        {
            if (!memberList.Any())
            {
                Debug.LogWarning("There is no member in crowd");
                return;
            }

            memberIndex %= memberList.Count;
            PlayParticle(memberIndex);
            var member = memberList[memberIndex];
            member.Death(instant,()=>PoolManager.Instance.Destroy(member));
            memberList.RemoveAt(memberIndex);
            PullCrowd();
        }


        public float lastPullTime;
         
        public void PullCrowd()
        {
            lastPullTime = Time.time;
        }

        [SerializeField] private int lastCount;

        private void PullProcess()
        {
            if (!isPulling) return;
            if (lastCount == Count) return;
            if ((Time.time - lastPullTime) < PullDelay) return;
            lastCount = Count;
            ForcePullCrowd();

        }

        public void ForcePullCrowd()
        {
            for (var i = 0; i < memberList.Count; i++)
            {
                var pos = Vector3.zero;
                switch (_formationType)
                {
                    case FormationType.Circular:
                        pos = CrowdUtils.CalculateCircularPositionWithIndex(i, CIRCULAR_INTERVAL);
                        pos += CrowdUtils.RandomNormalizedVector(randY: false) * CIRCULAR_RANDOMIZATION;
                        break;
                    case FormationType.Rectangular:
                        pos = CrowdUtils.CalculateFinishPositionWithIndex(i,Count,CIRCULAR_INTERVAL*1.5f,groupInterval:RECTANGULAR_INTERVAL);
                        break;
                    case FormationType.SwerveRectangular:
                        var ratio = SwerveController.Instance?.NormalizedPosition??0f;
                        pos = CrowdUtils.CalculateDynamicRectangularPosition(i,ratio,interval:CIRCULAR_INTERVAL);
                        if(MoveZ.Instance && MoveZ.Instance.isMove) pos += CrowdUtils.RandomNormalizedVector(randY: false) * CIRCULAR_RANDOMIZATION;
                        break;
                    case FormationType.Triangular:
                        pos = CrowdUtils.CalculateTriangularPosition(i, CIRCULAR_INTERVAL,CIRCULAR_INTERVAL);
                        pos += CrowdUtils.RandomNormalizedVector(randY: false) * CIRCULAR_RANDOMIZATION;
                        break;
                    case FormationType.TriangularTower:
                        var temp = CrowdUtils.CalculateTriangularPosition(i, CIRCULAR_INTERVAL,CrowdMember.DEFAULT_SCALE*3);
                        pos = new Vector3(temp.x,temp.z,0);
                        break;
                    case FormationType.Hexagon:
                        pos = CrowdUtils.CalculateHexagonPosition(i, CIRCULAR_INTERVAL);
                        pos += CrowdUtils.RandomNormalizedVector(randY: false) * CIRCULAR_RANDOMIZATION;
                        break;
                    case FormationType.Star:
                        pos = CrowdUtils.CalculateStarPosition(i, CIRCULAR_INTERVAL,CIRCULAR_INTERVAL);
                        // pos += CrowdUtils.RandomNormalizedVector(randY: false) * CIRCULAR_RANDOMIZATION;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                memberList[i].DestinationPos = pos;
            }
        }

        private void PlayParticle(int memberIndex)
        {
            var particle = PoolManager.Instance.Instantiate<CrowdMemberParticle>(particlePrefab);
            particle.transform.position = memberList[memberIndex].transform.position + Vector3.up * 1.5f;
            particle.Play(type);
        }

        private void SpawnInitial()
        {
            Spawn(initialCount);
        }
    }


