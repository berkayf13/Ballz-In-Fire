using System;
using System.Linq;
using F13StandardUtils.CrowdDynamics.Scripts;
using UnityEngine;

public class NotrMember : MonoBehaviour
{
    private void Awake()
    {
        transform.localScale = Vector3.one*CrowdMember.DEFAULT_SCALE;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PlayerType.Player.ToString()))
        {
            var crowdMember = other.GetComponent<CrowdMember>();
            if (crowdMember)
            {
                var player = FindObjectsOfType<CrowdManager>().First(c => c.type == PlayerType.Player);
                player.UpdateCount(player.Count+1);
                Destroy(gameObject);
            }
        }
    }
}
