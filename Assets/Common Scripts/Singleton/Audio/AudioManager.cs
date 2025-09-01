using System.Collections.Generic;
using UnityEngine;

namespace Common2D.Singleton
{
    public class AudioManager : Singleton<AudioManager>
    {
        [System.Serializable]
        public class Sound
        {
            public string name;
            public AudioClip clip;
            [Range(0f, 1f)]
            public float volume = 1f;
            [Range(0.1f, 3f)]
            public float pitch = 1f;
            public bool loop = false;

            [HideInInspector]
            public AudioSource source;
        }

        public Sound[] sounds;
        public float masterVolume = 1f;
        public float musicVolume = 1f;
        public float sfxVolume = 1f;

        private Dictionary<string, Sound> soundDictionary = new Dictionary<string, Sound>();

        protected override void Awake()
        {
            base.Awake();
            // Todo: Add initialization code here if needed

            foreach (Sound s in sounds)
            {
                AudioSource source = gameObject.AddComponent<AudioSource>();
                s.source = source;

                source.clip = s.clip;
                source.volume = s.volume;
                source.pitch = s.pitch;
                source.loop = s.loop;

                soundDictionary[s.name] = s;
            }
        }

        public void Play(string name)
        {
            if (soundDictionary.TryGetValue(name, out Sound sound))
            {
                sound.source.Play();
            }
            else
            {
                Debug.LogWarning("Sound: " + name + " not found in AudioManager");
            }
        }

        public void Stop(string name)
        {
            if (soundDictionary.TryGetValue(name, out Sound sound))
            {
                sound.source.Stop();
            }
        }

        public void SetVolume(string name, float volume)
        {
            if (soundDictionary.TryGetValue(name, out Sound sound))
            {
                sound.volume = volume;
                sound.source.volume = volume * masterVolume;
            }
        }
    }
}
