using System.Collections;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour {
    [Header("Board")]
    [SerializeField] private BoardCell[] cellObjects;
    [SerializeField] private RectTransform boardRect;
    [SerializeField] private StrikeAnimator strikeAnimator;

    [Header("HUD")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI player1MovesText;
    [SerializeField] private TextMeshProUGUI player2MovesText;
    [SerializeField] private TextMeshProUGUI currentPlayerText;

    [Header("Popups")]
    [SerializeField] private GameResultPopup resultPopup;
    [SerializeField] private SettingsPopup settingsPopup;

    private BoardCell[,] _cells;
    private TicTacToeGame _game = new TicTacToeGame();
    private float _elapsed;
    private bool _gameActive;

    private void Start() {
        _cells = new BoardCell[3, 3];
        for (int i = 0; i < 9; i++) {
            int r = i / 3, c = i % 3;
            _cells[r, c] = cellObjects[i];
            cellObjects[i].Row = r;
            cellObjects[i].Col = c;
            cellObjects[i].Init(this);
        }

        strikeAnimator.Init(_cells, boardRect);
        resultPopup.SetController(this);
        StartNewGame();
    }

    public void StartNewGame() {
        _game.Reset();
        strikeAnimator.Hide();

        for (int r = 0; r < 3; r++)
            for (int c = 0; c < 3; c++)
                _cells[r, c].ClearMark();

        _elapsed = 0f;
        _gameActive = true;
        UpdateHUD();
    }

    private void Update() {
        if (!_gameActive) return;
        _elapsed += Time.deltaTime;
        UpdateTimerText();
    }

    public void OnCellSelected(int row, int col) {
        if (!_gameActive) return;
        if (!_game.PlaceMark(row, col)) return;

        _cells[row, col].SetMark(_game.Board[row, col]);
        UpdateHUD();

        if (_game.State != TicTacToeGame.GameState.Playing) {
            _gameActive = false;
            StartCoroutine(ShowResultDelayed());
        }
    }

    private IEnumerator ShowResultDelayed() {
        if (_game.State != TicTacToeGame.GameState.Draw) {
            ThemeData theme = ThemeManager.Instance.CurrentTheme;
            Color tint = _game.State == TicTacToeGame.GameState.Player1Wins ? theme.xColor : theme.oColor;

            AudioManager.Instance?.PlayWin();

            var wl = _game.WinLine;
            strikeAnimator.AnimateStrike(wl.r0, wl.c0, wl.r1, wl.c1, tint);
            yield return new WaitForSeconds(0.7f);
        } else {
            AudioManager.Instance?.PlayDraw();
            yield return new WaitForSeconds(0.4f);
        }

        int winner = _game.State == TicTacToeGame.GameState.Player1Wins ? 1 :
                     _game.State == TicTacToeGame.GameState.Player2Wins ? 2 : 0;

        GameData.RecordGameResult(winner, _elapsed);
        resultPopup.Show(winner, _elapsed);
    }

    public void OnSettingsButtonClicked() {
        AudioManager.Instance?.PlayButtonClick();
        settingsPopup.Show();
    }

    private void UpdateHUD() {
        player1MovesText.text = $"X: {_game.Player1Moves} moves";
        player2MovesText.text = $"O: {_game.Player2Moves} moves";
        currentPlayerText.text = _game.State == TicTacToeGame.GameState.Playing ? (_game.CurrentPlayer == 1 ? $"X's Turn" : $"O's Turn") : "";
        UpdateTimerText();
    }

    private void UpdateTimerText() {
        int min = (int)(_elapsed / 60);
        int sec = (int)(_elapsed % 60);
        timerText.text = $"{min:00}:{sec:00}";
    }
}
