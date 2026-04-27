using UnityEngine;
using UnityEngine.UI;

public class SettingsPopup : BasePopup
{
    [SerializeField] private Toggle bgmToggle;
    [SerializeField] private Toggle sfxToggle;

    protected override void Awake() {
        base.Awake();

        bgmToggle.onValueChanged.AddListener(OnBGMChanged);
        sfxToggle.onValueChanged.AddListener(OnSFXChanged);
    }

    protected override void OnShow() {
        bgmToggle.SetIsOnWithoutNotify(GameData.Settings.bgmEnabled);
        sfxToggle.SetIsOnWithoutNotify(GameData.Settings.sfxEnabled);
    }

    private void OnBGMChanged(bool value) {
        AudioManager.Instance?.SetBGM(value);
    }

    private void OnSFXChanged(bool value) {
        AudioManager.Instance?.SetSFX(value);
    }
}
