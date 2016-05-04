/// <summary>
/// Created by Daniel Weston
/// Joint effort of AstroDillos and Armatillery! 
/// </summary>
using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using System.Collections.Generic;
using DG.Tweening;

namespace ArmadHeroes
{
    public class SoundManager : MonoBehaviour
    {
        #region Singleton
        private static SoundManager m_instance;
        public static SoundManager instance { get { return m_instance; } }
        #endregion

        #region Public Members
        public AudioSource m_bgmSource;//audio source for bgm
        public AudioMixer m_audioMixer;//game audio mixer
        public AudioMixerGroup sfx,//mixer channels
        bgm;
        #endregion

        #region Private Members
        private List<AudioSource> m_audioSourcesPool = new List<AudioSource>();//pool of audio sources
        private int m_audioPoolAmount = 64;
        private int m_audioHead = 0;
        #endregion

        #region Unity callbacks
        void Awake()
        {
            m_instance = this;
            PoolAudioSources();
        }
        #endregion

        #region SoundManager Behaviours
        /// <summary>
        /// Creates a pool of 
        /// audio sources
        /// </summary>
        void PoolAudioSources()
        {
            #region Audio Pool
            for (int i = 0; i < m_audioPoolAmount; i++)
            {
                //create a new source
                AudioSource _source = this.gameObject.AddComponent<AudioSource>();
                _source.outputAudioMixerGroup = sfx;
                _source.playOnAwake = false;
                //add to pool
                m_audioSourcesPool.Add(_source);
            }
            #endregion
        }

        /// <summary>
        /// Play passed AudioClip
        /// through the m_bgmSource AudioSource
        /// </summary>
        public void PlayMusic(AudioClip clip, bool loop = true, float volume = 1.0f)
        {
            m_bgmSource.clip = clip;
            m_bgmSource.loop = loop;
            m_bgmSource.volume = volume;
            m_bgmSource.Play();
        }

        /// <summary>
        /// Play passed AudioClip
        /// through the SFX AudioSource 
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="loop"></param>
        /// <param name="volume"></param>
        public AudioSource PlayClip(AudioClip clip, bool loop = false, float volume = 1.0f)
        {
            m_audioSourcesPool[m_audioHead].clip = clip;
            m_audioSourcesPool[m_audioHead].loop = loop;
            m_audioSourcesPool[m_audioHead].volume = volume;
            m_audioSourcesPool[m_audioHead].Play();

            //increment pool head for audio
            int prev = m_audioHead;
            m_audioHead = m_audioSourcesPool.Count - 1 == m_audioHead ? 0 : ++m_audioHead;
            return m_audioSourcesPool[prev];
        }

        /// <summary>
        /// Plays the clip with pan.
        /// </summary>
        /// <returns>The clip.</returns>
        /// <param name="clip">Clip.</param>
        /// <param name="pos">Position.</param>
        public AudioSource PlayClip(AudioClip clip, Vector3 pos, bool loop = false, float volume = 1.0f)
        {
            AudioSource source = PlayClip(clip, loop, volume);
            SetPan(pos, source);
            SetPitchVariation(source);
            return source;
        }

        /// <summary>
        /// Check whether an audiosource with a clipname is playing
        /// </summary>
        /// <param name="name">Name.</param>
        public bool IsClipnamePlaying(string name)
        {
            for (int i = 0; i < m_audioSourcesPool.Count; i++)
            {
                if (m_audioSourcesPool[i].isPlaying && m_audioSourcesPool[i].clip.name == name)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Stops all game Audio
        /// </summary>
        public void StopAllAudio()
        {
            StopAllClips();
            StopMusic();
        }

        /// <summary>
        /// Iterates through all AudioSources
        /// within m_audioSourcesPool and stops them
        /// </summary>
        public void StopAllClips()
        {
            for (int i = 0; i < m_audioSourcesPool.Count; i++)
            {
                m_audioSourcesPool[i].Stop();
            }
        }

        /// <summary>
        /// Stops m_bgmAudioSource
        /// </summary>
        public void StopMusic()
        {
            m_bgmSource.Stop();
        }

        /// <summary>
        /// Will kill passed AudioSource 
        /// within given float _time
        /// </summary>
        /// <param name="_source"></param>
        /// <param name="_time"></param>
        /// <returns></returns>
        public void FadeAndKillAudio(AudioSource source, float time = 0.5f)
        {
            Tweener _clip = source.DOFade(0.0f, time);
            _clip.OnComplete(() =>
            {
                source.Stop();
            });


        }

        /// <summary>
        /// Attenuates master 
        /// audio channel
        /// </summary>
        /// <param name="vol"></param>
        public void SetMasterChannelVol(float vol)
        {
            m_audioMixer.SetFloat("MasterVol", vol);
        }

        /// <summary>
        /// Attenuates SFX 
        /// audio channel
        /// </summary>
        /// <param name="vol"></param>
        public void SetSFXChannelVol(float vol)
        {
            sfx.audioMixer.SetFloat("SFXVol", vol);
        }

        /// <summary>
        /// Attenuates bgm 
        /// audio channel
        /// </summary>
        /// <param name="vol"></param>
        public void SetMusicChannelVol(float vol)
        {
            sfx.audioMixer.SetFloat("MusicVol", vol);
        }

        /// <summary>
        /// Sets the pan.
        /// </summary>
        public void SetPan(Vector3 pos, AudioSource sound)
        {
            sound.panStereo = WorldToPan(pos);
        }

        //Translates viewport range to pan range
        float WorldToPan(Vector3 world)
        {
            float panning = 0;
            if (Camera.current != null)
            {
                panning = (Camera.current.WorldToViewportPoint(world).x * 2) - 1;
            }
            return panning;
        }
        void SetPitchVariation(AudioSource source)
        {
            float pitch = Random.Range(0.95f, 1.05f);
            source.pitch = pitch;
        }
        #endregion
    }
}