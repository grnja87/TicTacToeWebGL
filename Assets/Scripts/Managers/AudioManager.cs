using UnityEngine;

public class AudioManager : MonoBehaviour {
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    public AudioClip bgmClip;

    public AudioClip buttonClickSFX;
    public AudioClip placementSFX;
    public AudioClip winSFX;
    public AudioClip drawSFX;
    public AudioClip popupOpenSFX;
    public AudioClip popupCloseSFX;

    private void Awake() {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        ApplySettings();
    }

    public void ApplySettings() {
        bgmSource.mute = !GameData.Settings.bgmEnabled;
        sfxSource.mute = !GameData.Settings.sfxEnabled;

        if (bgmClip != null && !bgmSource.isPlaying) {
            bgmSource.clip = bgmClip;
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }

    public void SetBGM(bool enabled) {
        GameData.Settings.bgmEnabled = enabled;
        bgmSource.mute = !enabled;
        GameData.Save();
    }

    public void SetSFX(bool enabled) {
        GameData.Settings.sfxEnabled = enabled;
        sfxSource.mute = !enabled;
        GameData.Save();
    }

    public void PlayButtonClick() => PlaySFX(buttonClickSFX);
    public void PlayPlacement() => PlaySFX(placementSFX);
    public void PlayWin() => PlaySFX(winSFX);
    public void PlayDraw() => PlaySFX(drawSFX);
    public void PlayPopupOpen() => PlaySFX(popupOpenSFX);
    public void PlayPopupClose() => PlaySFX(popupCloseSFX);

    private void PlaySFX(AudioClip clip) {
        if (clip != null && !sfxSource.mute)
            sfxSource.PlayOneShot(clip);
    }
}
