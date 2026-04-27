using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySceneUI : MonoBehaviour
{
    [Header("Popups")]
    [SerializeField] private ThemeSelectionPopup themePopup;
    [SerializeField] private StatsPopup statsPopup;
    [SerializeField] private SettingsPopup settingsPopup;
    [SerializeField] private ExitConfirmationPopup exitPopup;

    void Start() {
        GameData.Load();
        AudioManager.Instance?.ApplySettings();
    }

    public void OnPlayButtonClicked() {
        AudioManager.Instance?.PlayButtonClick();
        themePopup.Show();
    }

    public void OnStatsButtonClicked() {
        AudioManager.Instance?.PlayButtonClick();
        statsPopup.Show();
    }

    public void OnSettingsButtonClicked() {
        AudioManager.Instance?.PlayButtonClick();
        settingsPopup.Show();
    }

    public void OnExitButtonClick() {
        AudioManager.Instance?.PlayButtonClick();
        exitPopup.Show();
    }
}
