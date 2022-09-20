using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace F13StandardUtils.Scripts.Core
{
    public class DoGroupAnim : MonoBehaviour
    {
        [SerializeField] private List<DoAnim> anims = new List<DoAnim>();


        [Button]
        public void StartAnim()
        {
            anims.ForEach(x => x.StartAnim());
        }
    
        [Button]
        public void RestartAnim()
        {
            anims.ForEach(x => x.RestartAnim());
        }
    
    
        [Button]
        public void KillAnim()
        {
            anims.ForEach(x => x.KillAnim());
        }
        
        [Button]
        public void CompleteAnim()
        {
            anims.ForEach(x => x.CompleteAnim());
        }
        
    
        [Button]
        public void ResetTransform()
        {
            anims.ForEach(x => x.ResetTransform());
        }
    
        

    }
}