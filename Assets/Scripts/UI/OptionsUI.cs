using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class OptionsUI : MonoBehaviour {
        private SoundManager soundManager;
        private MusicManager musicManager;
        
        [SerializeField] private Slider musicSlider;
        [SerializeField] private TextMeshProUGUI musicValueText;
    
        [SerializeField] private Slider sfxSlider;
        [SerializeField] private TextMeshProUGUI sfxValueText;


        [SerializeField] private Button closeButton;
        
        private void Start() {
            musicManager = MusicManager.Instance;
            soundManager = SoundManager.Instance;
            
            musicManager.OnMusicVolumeChanged += OnMusicVolumeChanged;
            soundManager.OnSfxVolumeChanged += OnSfxVolumeChanged;
            
            musicSlider.value = musicManager.MusicVolume;
            musicValueText.text = musicManager.MusicVolume.ToString("P0");

            sfxSlider.value = soundManager.SfxVolume;
            sfxValueText.text = soundManager.SfxVolume.ToString("P0");
            
            musicSlider.onValueChanged.AddListener(OnMusicSliderValueChanged);
            sfxSlider.onValueChanged.AddListener(OnSfxSliderValueChanged);
            
            closeButton.onClick.AddListener(Hide);
            
            Hide();
        }
        
        private void OnDestroy() {
            musicManager.OnMusicVolumeChanged -= OnMusicVolumeChanged;
            soundManager.OnSfxVolumeChanged -= OnSfxVolumeChanged;
            
            musicSlider.onValueChanged.RemoveListener(OnMusicSliderValueChanged);
            sfxSlider.onValueChanged.RemoveListener(OnSfxSliderValueChanged);
        }

        
        private void OnMusicSliderValueChanged(float value) => musicManager.ChangeMusicVolume(value);
        private void OnSfxSliderValueChanged(float value) => soundManager.ChangeSfxVolume(value);


        private void OnMusicVolumeChanged(object sender, ValueChangedEvent<float> e) {
            musicSlider.SetValueWithoutNotify(e.NewValue);
            musicValueText.text = e.NewValue.ToString("P0");
        }
        
        private void OnSfxVolumeChanged(object sender, ValueChangedEvent<float> e) {
            sfxSlider.SetValueWithoutNotify(e.NewValue);
            sfxValueText.text = e.NewValue.ToString("P0");
        }
        

        public void Show() {
            gameObject.SetActive(true);
        }
        
        public void Hide() {
            gameObject.SetActive(false);
        }
    }
}
