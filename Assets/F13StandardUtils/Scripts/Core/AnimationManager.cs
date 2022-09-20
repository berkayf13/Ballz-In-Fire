using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using F13StandardUtils.Scripts.Core;
using Sirenix.OdinInspector;
using UnityEngine;

public class AnimationManager : Singleton<AnimationManager>
{
    public List<GameObject> uiPrefabs=new List<GameObject>();
    
    [Button]
    public void UICollectAnimation(GameObject particlePrefab, int particleCount, Vector3 from, Vector3 to,
        Transform parent, float maxAreaWidth = 200, float maxRotate = 15f, Action onParticleCollect = null,
        Action onComplete = null)
    {
        for (int index = 0; index < particleCount; index++)
        {
            var i = index;
            var createdParticle = Instantiate(particlePrefab, parent, true);
            var screenPos = from;
            var random = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), 0f) *
                         maxAreaWidth;
            createdParticle.transform.position = screenPos + random;
            createdParticle.transform.Rotate(Vector3.forward * UnityEngine.Random.Range(-maxRotate, maxRotate));

            createdParticle.transform.localScale = Vector3.zero;
            createdParticle.transform.DOScale(Vector3.one * 1.5f, 0.2f).SetDelay(i * 0.01f).OnComplete(() =>
            {
                createdParticle.transform.DOScale(Vector3.one, 0.1f).OnComplete(() =>
                {
                    createdParticle.transform.DOScale(Vector3.one * 0.4f, 0.5f).SetDelay(0.5f + i * 0.01f);
                    createdParticle.transform.DOMove(to, 0.5f).OnComplete((() =>
                    {
                        onParticleCollect?.Invoke();
                        if (i + 1 >= particleCount)
                        {
                            onComplete?.Invoke();
                        }

                        Destroy(createdParticle);
                    })).SetDelay(0.5f + i * 0.01f);
                });
            });

        }
    }
}
