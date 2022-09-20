using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.F13SDK.Scripts
{
    public class PlayerPrefsManager: OmegaSingletonManager<PlayerPrefsManager>
    {
        private PlayerData _playerData;

        public PlayerData PlayerData
        {
            get
            {
                if(_playerData==null)
                    _playerData=new PlayerData();
                return _playerData;
            }
        }

        
        public void AwakePlayerPrefsManager()
        {
            IntiliazePlayerPrefs();
        }

        public void IntiliazePlayerPrefs()
        {
            OmegaAudioManager.Instance.isAudioPlaying = PlayerData.IsAudioPlaying;
            OmegaAudioManager.Instance.audioMasterVolume = PlayerData.AudioMasterVolume;
            OmegaAudioManager.Instance.isMusicPlaying = PlayerData.IsMusicPlaying;
            OmegaAudioManager.Instance.musicMasterVolume = PlayerData.MusicMasterVolume;
            OmegaHapticManager.Instance.isActive = PlayerData.isHapticActive;
            OmegaDebugManager.Instance.PrintDebug("isAudioPlaying awake: " + PlayerData.IsAudioPlaying, DebugType.AudioManager);
            OmegaDebugManager.Instance.PrintDebug("audioMasterVolume awake: " + PlayerData.AudioMasterVolume, DebugType.AudioManager);
            OmegaDebugManager.Instance.PrintDebug("isMusicPlaying awake: " + PlayerData.IsMusicPlaying, DebugType.AudioManager);
            OmegaDebugManager.Instance.PrintDebug("musicMasterVolume awake: " + PlayerData.MusicMasterVolume, DebugType.AudioManager);
            OmegaDebugManager.Instance.PrintDebug("isHapticActive awake: " + PlayerData.isHapticActive, DebugType.HapticManager);

        }

        public void SaveAudioPlayerPrefs(bool isAudioPlaying, float volume)
        {
            PlayerData.IsAudioPlaying = isAudioPlaying;
            PlayerData.AudioMasterVolume = volume;
            OmegaDebugManager.Instance.PrintDebug("audioMasterVolume save: " + PlayerData.AudioMasterVolume, DebugType.AudioManager);
            OmegaDebugManager.Instance.PrintDebug("isAudioPlaying save: " + PlayerData.IsAudioPlaying, DebugType.AudioManager);
        }

        public void SaveMusicPlayerPrefs(bool isMusicPlaying, float volume)
        {
            PlayerData.IsMusicPlaying = isMusicPlaying;
            PlayerData.MusicMasterVolume = volume;
            OmegaDebugManager.Instance.PrintDebug("musicMasterVolume save: " + PlayerData.MusicMasterVolume, DebugType.AudioManager);
            OmegaDebugManager.Instance.PrintDebug("isMusicPlaying save: " + PlayerData.IsMusicPlaying, DebugType.AudioManager);
        }
        
        public void SaveHapticManagerPrefs(bool isActive)
        {
            PlayerData.isHapticActive = isActive;
            OmegaDebugManager.Instance.PrintDebug("isHapticActive save: " + PlayerData.isHapticActive, DebugType.HapticManager);
        }

        public bool HasKey(string key)
        {
            return PlayerPrefs.HasKey(key);
        }
        
        [Button]
        private void CheatTest()
        {
            PlayerPrefs.SetInt("money", 10);
        }
    }
}
