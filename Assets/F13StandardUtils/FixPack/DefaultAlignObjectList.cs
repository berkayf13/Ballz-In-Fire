using UnityEngine;

namespace _GAME.Scripts.level5.NewScripts
{
    public class DefaultAlignObjectList : BaseAlignObjects<LocalMove>
    {
        [SerializeField] private Vector3 alignSizeWeight=new Vector3(1,0,0);

        protected override Vector3 PositionOf(Transform obj)
        {
            return obj.localPosition;
        }

        protected override void Move(LocalMove obj, Vector3 pos)
        {
            obj.SetDestination(pos);
        }

        protected override float SizeOf(LocalMove obj)
        {
            if(obj.TryGetComponent(out BoxCollider boxCollider))
            {
                return Vector3.Scale(boxCollider.size,alignSizeWeight).magnitude;
            }
            return 1f;
        }
    }
}