using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace UI
{
    public class VolumeSlider : MonoBehaviour
    {
        private Slider _slider;
        [SerializeField] private AudioMixer mixer;
        [SerializeField] private AudioMixerGroup mixerGroup;

        private void Start()
        {
            _slider = GetComponent<Slider>();
            _slider.onValueChanged.AddListener(OnSliderValueChanged);
            SetVolume(PlayerPrefs.GetFloat("Volume", 1f));
        }

        private void OnSliderValueChanged(float value)
        {
            SetVolume(value);
        }

        public void SetVolume(float volume)
        {
            mixer.SetFloat(mixerGroup.ToString(), Mathf.Log10(volume) * 20);
            PlayerPrefs.SetFloat("Volume", volume);
            _slider.value = volume;
        }

        private void OnDestroy()
        {
            PlayerPrefs.Save();
        }
    }
}