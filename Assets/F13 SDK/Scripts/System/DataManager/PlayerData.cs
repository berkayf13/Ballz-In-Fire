// using ElephantSDK;

using System.Collections.Generic;
using F13StandardUtils.Scripts.Core;
using UnityEngine;
using Utils = F13StandardUtils.Scripts.Core.Utils;

namespace Assets.F13SDK.Scripts
{
    public class PlayerData
    {
        private bool _isAudioPlaying = true;
        private float _audioMasterVolume = 1f;
        private bool _isMusicPlaying = true;
        private float _musicMasterVolume = 1f;
        private bool _isHapticActive = true;
        private int _level = 1;
        private float _queueLenght = 0;
        private int _money = 0;
        private int _levelMoney = 0;
        private List<float> _seatUnlockedRatios = new List<float>();
        private int _checkSum;
        private bool _updateCheckSum = true;
        private bool _cheatingStatus = false;
        private int _fireRate = 0;
        private int _damage = 0;
        private int _income = 0;

        public int CheckSum()
        {
            var checkSum = 0;
            checkSum += _level.GetHashCode();
            checkSum += _queueLenght.GetHashCode();
            checkSum += _money.GetHashCode();
            checkSum += _levelMoney.GetHashCode();
            checkSum += _fireRate.GetHashCode();
            checkSum += _damage.GetHashCode();
            checkSum += _income.GetHashCode();
            for (var index = 0; index < _seatUnlockedRatios.Count; index++)
            {
                checkSum += _seatUnlockedRatios[index].GetHashCode();

            }
            return checkSum;
        }

        public void UpdateCheckSum()
        {
            if (!_updateCheckSum) return;
            var checkSum = CheckSum();
            var encrypted = Utils.Encrypt(GameController.ENCRYPT, checkSum);
            PlayerPrefs.SetString("checkSum", encrypted);
        }

        private void CheckCheatStatus()
        {
            if (!PlayerPrefs.HasKey("checkSum"))
                UpdateCheckSum();
            var encrypt = PlayerPrefs.GetString("checkSum");
            _checkSum = Utils.Decrypt<int>(GameController.ENCRYPT, encrypt);
            if (_checkSum != CheckSum())
            {
                Debug.LogError("CHEATER!");
                _cheatingStatus = true;
                // GameController.Instance.SendEvent(GameEvents.gen_cheater, _level,
                //     Params.New()
                //         .Set("income",(double)_income)
                //         .Set("customerFreq",(double)_customerFreq)
                //         .Set("queueLenght",(double)_queueLenght)
                //         .Set("money",_money)
                //         .Set("_levelMoney",_levelMoney)
                //         .Set("seatUnlockedRatios", Utils.ToJson("seatUnlockedRatios", _seatUnlockedRatios)));

                _updateCheckSum = false;
                Level = 1;
                QueueLenght = 0;
                Money = 0;
                LevelMoney = 0;
                FireRate = 0;
                Damage = 0;
                Income = 0;
                ResetSeatRatios();
                _updateCheckSum = true;
                UpdateCheckSum();
            }
        }

        public PlayerData()
        {
            Load();
        }

        private void Load()
        {
            _isAudioPlaying = PlayerPrefs.GetInt("isAudioPlaying", _isAudioPlaying ? 1 : 0) == 1;
            _audioMasterVolume = PlayerPrefs.GetFloat("audioMasterVolume", _audioMasterVolume);
            _isMusicPlaying = PlayerPrefs.GetInt("isMusicPlaying", _isMusicPlaying ? 1 : 0) == 1;
            _musicMasterVolume = PlayerPrefs.GetFloat("musicMasterVolume", _musicMasterVolume);
            _isHapticActive = PlayerPrefs.GetInt("isHapticActive", _isHapticActive ? 1 : 0) == 1;
            _level = PlayerPrefs.GetInt("level", _level);
            _queueLenght = PlayerPrefs.GetFloat("queueLenght", _queueLenght);
            _money = PlayerPrefs.GetInt("money", _money);
            _levelMoney = PlayerPrefs.GetInt("levelMoney", _levelMoney);
            _fireRate = PlayerPrefs.GetInt("fireRate", _fireRate);
            _damage = PlayerPrefs.GetInt("damage", _damage);
            _income = PlayerPrefs.GetInt("income", _income);
            LoadSeatRatios();
            CheckCheatStatus();
        }


        #region Properties

        public bool IsAudioPlaying
        {
            get { return _isAudioPlaying; }
            set
            {
                _isAudioPlaying = value;
                PlayerPrefs.SetInt("isAudioPlaying", _isAudioPlaying ? 1 : 0);
            }
        }

        public float AudioMasterVolume
        {
            get { return _audioMasterVolume; }
            set
            {
                _audioMasterVolume = value;
                PlayerPrefs.SetFloat("audioMasterVolume", _audioMasterVolume);
            }
        }

        public bool IsMusicPlaying
        {
            get { return _isMusicPlaying; }
            set
            {
                _isMusicPlaying = value;
                PlayerPrefs.SetInt("isMusicPlaying", _isMusicPlaying ? 1 : 0);
            }
        }

        public float MusicMasterVolume
        {
            get { return _musicMasterVolume; }
            set
            {
                _musicMasterVolume = value;
                PlayerPrefs.SetFloat("musicMasterVolume", _musicMasterVolume);
            }
        }

        public bool isHapticActive
        {
            get { return _isHapticActive; }
            set
            {
                _isHapticActive = value;
                PlayerPrefs.SetInt("isHapticActive", _isHapticActive ? 1 : 0);
            }
        }

        public int Level
        {
            get { return _level; }
            set
            {
                _level = value;
                PlayerPrefs.SetInt("level", _level);
                UpdateCheckSum();
            }
        }


        public float QueueLenght
        {
            get { return _queueLenght; }
            set
            {
                _queueLenght = value;
                PlayerPrefs.SetFloat("queueLenght", _queueLenght);
                UpdateCheckSum();
            }
        }


        public int Money
        {
            get { return _money; }
            set
            {
                _money = value;
                PlayerPrefs.SetInt("money", _money);
                UpdateCheckSum();
            }
        }

        public int LevelMoney
        {
            get { return _levelMoney; }
            set
            {
                _levelMoney = value;
                PlayerPrefs.SetInt("levelMoney", _levelMoney);
                UpdateCheckSum();
            }
        }

        public int FireRate
        {
            get { return _fireRate; }
            set
            {
                _fireRate = value;
                PlayerPrefs.SetInt("fireRate", _fireRate);
                UpdateCheckSum();
            }
        }

        public int Income
        {
            get { return _income; }
            set
            {
                _income = value;
                PlayerPrefs.SetInt("income", _income);
                UpdateCheckSum();
            }
        }

        public int Damage
        {
            get { return _damage; }
            set
            {
                _damage = value;
                PlayerPrefs.SetInt("damage", _damage);
                UpdateCheckSum();
            }
        }

        public float GetSeatRatio(int index)
        {
            return _seatUnlockedRatios[index];
        }

        public void SetSeatRatio(int index, float ratio)
        {
            ratio = Mathf.Clamp(ratio, 0f, 1f);
            _seatUnlockedRatios[index] = ratio;
            PlayerPrefs.SetFloat("seat_" + index, ratio);
            UpdateCheckSum();

        }

        public void LoadSeatRatios()
        {
            _seatUnlockedRatios.Clear();
            for (var index = 0; index < 100; index++)
            {
                _seatUnlockedRatios.Add(PlayerPrefs.GetFloat("seat_" + index, 0f));
            }
        }

        public void ResetSeatRatios()
        {
            for (var index = 0; index < 100; index++)
            {
                SetSeatRatio(index, 0f);
            }
        }

        public bool CheatingStatus
        {
            get => _cheatingStatus;
            set => _cheatingStatus = value;
        }

        #endregion
    }
}