using UnityEngine;
using TMPro;

public class GameResultPopup : BasePopup
{
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private TextMeshProUGUI durationText;

    private GameController _controller;

    public void SetController(GameController controller)
    {
        _controller = controller;
    }

    public void Show(int winner, float duration)
    {
        if (winner == 1) resultText.text = "Player X Wins!";
        else if (winner == 2) resultText.text = "Player O Wins!";
        else resultText.text = "It's a Draw!";

        int min = (int)(duration / 60), sec = (int)(duration % 60);
        durationText.text = $"Match Duration: {min:00}:{sec:00}";

        Show();
    }

    public void OnRetryClicked()
    {
        AudioManager.Instance?.PlayButtonClick();
        Hide();
        _controller.StartNewGame();
    }

    public void OnExitClicked()
    {
        AudioManager.Instance?.PlayButtonClick();
        Hide();
        SceneLoader.LoadPlayScene();
    }
}
