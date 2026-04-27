using UnityEngine;
using TMPro;

public class StatsPopup : BasePopup
{
    [SerializeField] private TextMeshProUGUI totalGamesText;
    [SerializeField] private TextMeshProUGUI player1WinsText;
    [SerializeField] private TextMeshProUGUI player2WinsText;
    [SerializeField] private TextMeshProUGUI drawsText;
    [SerializeField] private TextMeshProUGUI avgDurationText;

    protected override void OnShow() {
        var s = GameData.Stats;
        totalGamesText.text = $"Total Games: {s.totalGamesPlayed}";
        player1WinsText.text = $"Player 1 Wins: {s.player1Wins}";
        player2WinsText.text = $"Player 2 Wins: {s.player2Wins}";
        drawsText.text = $"Draws: {s.draws}";

        avgDurationText.text = $"Avg Duration: {s.AverageGameDuration:F1}s";
    }
}
