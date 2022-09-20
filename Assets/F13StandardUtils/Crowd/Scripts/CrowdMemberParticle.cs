using System.Collections.Generic;
using F13StandardUtils.CrowdDynamics.Scripts;
using F13StandardUtils.Scripts.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace F13StandardUtils.Crowd.Scripts
{
    public class CrowdMemberParticle : BaseTypeParticle<PlayerType>
    {
        protected override int ToInt(PlayerType e)
        {
            return (int) e;
        }
    }
}
