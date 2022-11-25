using AchromaticDev.Util;
using AchromaticDev.Util.Pooling;
using UnityEngine;

namespace Script.Manager
{
    public class SoundManager : MonoSingleton<SoundManager>
    {
        public AudioClip buttonClick;
        public GameObject soundPrefab;

        public void PlayEffect(AudioClip clip, Vector3 position = default)
        {
            position = position == default ? transform.position : position;

            var sound = PoolManager.Instantiate(soundPrefab, position, Quaternion.identity);
            sound.transform.SetParent(transform);

            var cachedComponent = sound.GetCachedComponent<AudioSource>();
            cachedComponent.clip = clip;
            cachedComponent.Play();

            PoolManager.Destroy(sound, clip.length);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                PlayEffect(buttonClick, Vector3.zero);
            }
        }
    }
}