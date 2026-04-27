using UnityEngine;

public class ExitConfirmationPopup : BasePopup
{
    public void OnYesClicked() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OnNoClicked() {
        AudioManager.Instance?.PlayButtonClick();
        Hide();
    }
}
