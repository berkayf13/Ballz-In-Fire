using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using Assets.F13SDK.Scripts;
using F13StandardUtils.Scripts.Core;
using TMPro;
using UnityEngine.Events;


public class StarUnlock : Singleton<StarUnlock>
{
    public static int SCORE = 1;
    public static int MAXSCORE = 3;
    [SerializeField] private GameObject[] stars;
    [SerializeField] private GameObject[] borderStars;
    [SerializeField] private TextMeshProUGUI _msgText;
    [SerializeField] private List<Color> _msgColors;
    [SerializeField] private List<int> _msgRatios;
    public UnityEvent OnVisible=new UnityEvent();
    public UnityEvent OnUnlocked=new UnityEvent();
    public UnityEvent OnDissapear=new UnityEvent();



    private void OnEnable()
    {
        OnVisible.Invoke();
        LockStar();
        this.StartWaitForSecondCoroutine(0.5f, UnlockStar);
    }

    private void OnDisable()
    {
        _msgText.DOFade(0, 0f);
        OnDissapear.Invoke();
    }

    private void UnlockStar()
    {
        for (int i = 0; i < MAXSCORE; i++)
        {
            borderStars[i].SetActive(true);
        }
        for (int i = 0; i < SCORE; i++)
        {
            var a = i;
            var star = stars[a];
            star.SetActive(true);
            star.transform.DOScale(Vector3.one, 0.5f).SetDelay(0.25f*a).OnComplete(() =>
            {
                if (a == SCORE - 1)
                {
                    if (MAXSCORE > 1)
                    {
                        _msgText.text = _msgRatios[SCORE-1]+"% of people can fix it this way!";
                        _msgText.DOColor(_msgColors[SCORE-1], 0.33f);
                    }
                    OnUnlocked.Invoke();
                }
            });
        }
    }
    private void LockStar()
    {
        for (int i = 0; i < borderStars.Length; i++)
        {
            borderStars[i].SetActive(false);
        }
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].SetActive(false);
            stars[i].transform.localScale = Vector3.zero;
        }
    }
}
