using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using GK.Platform;

namespace GK.Managers
{
    /// <summary>
    /// 音频管理器
    /// 处理不同平台的音频播放和管理
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        [Header("音频设置")]
        [SerializeField] private bool initializeOnStart = true;
        [SerializeField] private int maxAudioSources = 10;
        [SerializeField] private float masterVolume = 1.0f;
        [SerializeField] private float musicVolume = 0.8f;
        [SerializeField] private float sfxVolume = 1.0f;

        [Header("音频资源")]
        [SerializeField] private AudioClip[] musicClips;
        [SerializeField] private AudioClip[] sfxClips;

        private static AudioManager _instance;
        private AudioSource _musicSource;
        private List<AudioSource> _sfxSources;
        private Dictionary<string, AudioClip> _audioClips;
        private bool _isMusicMuted;
        private bool _isSfxMuted;

        public static AudioManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<AudioManager>();
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("AudioManager");
                        _instance = go.AddComponent<AudioManager>();
                        DontDestroyOnLoad(go);
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// 主音量
        /// </summary>
        public float MasterVolume
        {
            get => masterVolume;
            set
            {
                masterVolume = Mathf.Clamp01(value);
                UpdateAllVolumes();
            }
        }

        /// <summary>
        /// 音乐音量
        /// </summary>
        public float MusicVolume
        {
            get => musicVolume;
            set
            {
                musicVolume = Mathf.Clamp01(value);
                UpdateMusicVolume();
            }
        }

        /// <summary>
        /// 音效音量
        /// </summary>
        public float SfxVolume
        {
            get => sfxVolume;
            set
            {
                sfxVolume = Mathf.Clamp01(value);
                UpdateSfxVolume();
            }
        }

        /// <summary>
        /// 音乐是否静音
        /// </summary>
        public bool IsMusicMuted
        {
            get => _isMusicMuted;
            set
            {
                _isMusicMuted = value;
                UpdateMusicVolume();
            }
        }

        /// <summary>
        /// 音效是否静音
        /// </summary>
        public bool IsSfxMuted
        {
            get => _isSfxMuted;
            set
            {
                _isSfxMuted = value;
                UpdateSfxVolume();
            }
        }

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);

                if (initializeOnStart)
                {
                    Initialize();
                }
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// 初始化音频管理器
        /// </summary>
        public void Initialize()
        {
            InitializeAudioSources();
            LoadAudioClips();
            AdaptToPlatform();
        }

        /// <summary>
        /// 初始化音频源
        /// </summary>
        private void InitializeAudioSources()
        {
            // 创建音乐音频源
            _musicSource = gameObject.AddComponent<AudioSource>();
            _musicSource.loop = true;
            _musicSource.playOnAwake = false;

            // 创建音效音频源池
            _sfxSources = new List<AudioSource>();
            for (int i = 0; i < maxAudioSources; i++)
            {
                AudioSource sfxSource = gameObject.AddComponent<AudioSource>();
                sfxSource.loop = false;
                sfxSource.playOnAwake = false;
                _sfxSources.Add(sfxSource);
            }
        }

        /// <summary>
        /// 加载音频剪辑
        /// </summary>
        private void LoadAudioClips()
        {
            _audioClips = new Dictionary<string, AudioClip>();

            // 加载音乐
            if (musicClips != null)
            {
                foreach (var clip in musicClips)
                {
                    if (clip != null)
                    {
                        _audioClips[clip.name] = clip;
                    }
                }
            }

            // 加载音效
            if (sfxClips != null)
            {
                foreach (var clip in sfxClips)
                {
                    if (clip != null)
                    {
                        _audioClips[clip.name] = clip;
                    }
                }
            }
        }

        /// <summary>
        /// 适配不同平台
        /// </summary>
        private void AdaptToPlatform()
        {
            if (PlatformDetector.IsMiniGame)
            {
                // 小游戏平台音频限制
                maxAudioSources = Mathf.Min(maxAudioSources, 5);

                // 微信小游戏需要用户交互后才能播放音频
                if (PlatformDetector.CurrentPlatform == PlatformDetector.PlatformType.WeixinMiniGame)
                {
                    StartCoroutine(WaitForUserInteraction());
                }
            }
            else if (PlatformDetector.IsMobile)
            {
                // 移动设备音频优化
                AudioSettings.outputSampleRate = 22050;
            }
        }

        /// <summary>
        /// 等待用户交互（小游戏平台需要）
        /// </summary>
        private IEnumerator WaitForUserInteraction()
        {
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0) || Input.touchCount > 0);

            // 播放一个静音音频以激活音频系统
            PlaySfx("", 0f);
        }

        /// <summary>
        /// 播放音乐
        /// </summary>
        public void PlayMusic(string clipName, bool loop = true, float fadeInTime = 0f)
        {
            if (_audioClips.TryGetValue(clipName, out AudioClip clip))
            {
                if (fadeInTime > 0f)
                {
                    StartCoroutine(FadeInMusic(clip, loop, fadeInTime));
                }
                else
                {
                    _musicSource.clip = clip;
                    _musicSource.loop = loop;
                    _musicSource.Play();
                }
            }
            else
            {
                Debug.LogWarning($"Music clip '{clipName}' not found!");
            }
        }

        /// <summary>
        /// 停止音乐
        /// </summary>
        public void StopMusic(float fadeOutTime = 0f)
        {
            if (fadeOutTime > 0f)
            {
                StartCoroutine(FadeOutMusic(fadeOutTime));
            }
            else
            {
                _musicSource.Stop();
            }
        }

        /// <summary>
        /// 播放音效
        /// </summary>
        public void PlaySfx(string clipName, float volume = 1f)
        {
            if (_audioClips.TryGetValue(clipName, out AudioClip clip))
            {
                AudioSource availableSource = GetAvailableSfxSource();
                if (availableSource != null)
                {
                    availableSource.clip = clip;
                    availableSource.volume = volume * sfxVolume * masterVolume * (_isSfxMuted ? 0f : 1f);
                    availableSource.Play();
                }
            }
            else if (!string.IsNullOrEmpty(clipName))
            {
                Debug.LogWarning($"SFX clip '{clipName}' not found!");
            }
        }

        /// <summary>
        /// 获取可用的音效音频源
        /// </summary>
        private AudioSource GetAvailableSfxSource()
        {
            foreach (var source in _sfxSources)
            {
                if (!source.isPlaying)
                {
                    return source;
                }
            }
            return _sfxSources[0]; // 如果都在播放，使用第一个
        }

        /// <summary>
        /// 淡入音乐
        /// </summary>
        private IEnumerator FadeInMusic(AudioClip clip, bool loop, float fadeTime)
        {
            _musicSource.clip = clip;
            _musicSource.loop = loop;
            _musicSource.volume = 0f;
            _musicSource.Play();

            float timer = 0f;
            float targetVolume = musicVolume * masterVolume * (_isMusicMuted ? 0f : 1f);

            while (timer < fadeTime)
            {
                timer += Time.deltaTime;
                _musicSource.volume = Mathf.Lerp(0f, targetVolume, timer / fadeTime);
                yield return null;
            }

            _musicSource.volume = targetVolume;
        }

        /// <summary>
        /// 淡出音乐
        /// </summary>
        private IEnumerator FadeOutMusic(float fadeTime)
        {
            float startVolume = _musicSource.volume;
            float timer = 0f;

            while (timer < fadeTime)
            {
                timer += Time.deltaTime;
                _musicSource.volume = Mathf.Lerp(startVolume, 0f, timer / fadeTime);
                yield return null;
            }

            _musicSource.Stop();
            _musicSource.volume = startVolume;
        }

        /// <summary>
        /// 更新所有音量
        /// </summary>
        private void UpdateAllVolumes()
        {
            UpdateMusicVolume();
            UpdateSfxVolume();
        }

        /// <summary>
        /// 更新音乐音量
        /// </summary>
        private void UpdateMusicVolume()
        {
            if (_musicSource != null)
            {
                _musicSource.volume = musicVolume * masterVolume * (_isMusicMuted ? 0f : 1f);
            }
        }

        /// <summary>
        /// 更新音效音量
        /// </summary>
        private void UpdateSfxVolume()
        {
            float volume = sfxVolume * masterVolume * (_isSfxMuted ? 0f : 1f);
            foreach (var source in _sfxSources)
            {
                if (source.isPlaying)
                {
                    source.volume = volume;
                }
            }
        }

        /// <summary>
        /// 添加音频剪辑
        /// </summary>
        public void AddAudioClip(string name, AudioClip clip)
        {
            if (clip != null)
            {
                _audioClips[name] = clip;
            }
        }

        /// <summary>
        /// 暂停所有音频
        /// </summary>
        public void PauseAll()
        {
            _musicSource.Pause();
            foreach (var source in _sfxSources)
            {
                source.Pause();
            }
        }

        /// <summary>
        /// 恢复所有音频
        /// </summary>
        public void ResumeAll()
        {
            _musicSource.UnPause();
            foreach (var source in _sfxSources)
            {
                source.UnPause();
            }
        }
    }
}
