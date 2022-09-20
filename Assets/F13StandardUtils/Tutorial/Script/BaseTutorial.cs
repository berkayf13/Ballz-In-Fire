using System.Collections;
using F13StandardUtils.Scripts.Core;
using UnityEngine;

namespace F13StandardUtils.CollectTicket.Tutorial.Script
{
    public abstract class BaseTutorial:MonoBehaviour
    {
        [SerializeField] private string _tutorialId = "tutorial_0";
        [SerializeField] private float _delay=2;
        [SerializeField] private bool _isStopTimeScale=true;
        
        private bool _isStarted = false;
        private bool _isComplete = false;

        public bool IsStarted => _isStarted;
        public bool IsComplete => _isComplete;
        public string TutorialId => _tutorialId;

        private bool IsTutorialDoneBefore
        {
            get => PlayerPrefs.GetInt(_tutorialId, 0) == 1;
            set => PlayerPrefs.SetInt(_tutorialId, value?1:0);
        }

        protected abstract bool TutorialNotStartCondition();
        
        protected abstract bool TutorialStartCondition();
        protected abstract bool TutorialEndCondition();
        protected abstract void OnTutorialStart();
        protected abstract void OnTutorialEnd();

        
        private void OnEnable()
        {
            if (IsTutorialDoneBefore || TutorialNotStartCondition())
            {
                Destroy(gameObject);
            }
            StartCoroutine(TutorialLifeCycle());
        }
        
        private IEnumerator TutorialLifeCycle()
        {
            yield return new WaitUntil(TutorialStartCondition);
            yield return new WaitForSeconds(_delay);
            _isStarted = true;
            if(_isStopTimeScale) Time.timeScale = 0;
            OnTutorialStart();
            yield return new WaitUntil(TutorialEndCondition);
            IsTutorialDoneBefore = true;
            if(_isStopTimeScale) Time.timeScale = 1;
            _isComplete = true;
            OnTutorialEnd();
            Destroy(gameObject);
        }


    }
}