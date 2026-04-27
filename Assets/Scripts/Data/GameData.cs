using System;
using UnityEngine;

[Serializable]
public class GameStats {
    public int totalGamesPlayed;
    public int player1Wins;
    public int player2Wins;
    public int draws;
    public float totalGameDuration;

    public float AverageGameDuration =>
        totalGamesPlayed > 0 ? totalGameDuration / totalGamesPlayed : 0f;
}

[Serializable]
public class GameSettings {
    public bool bgmEnabled = true;
    public bool sfxEnabled = true;
    public int selectedThemeIndex = 0;
}

public static class GameData
{
    private const string STATS_KEY = "GameStats";
    private const string SETTINGS_KEY = "GameSettings";

    private static GameStats _stats;
    private static GameSettings _settings;

    public static GameStats Stats {
        get {
            if (_stats == null) Load();
            return _stats;
        }
    }

    public static GameSettings Settings {
        get {
            if (_settings == null) Load();
            return _settings;
        }
    }

    public static void Load() {
        string statsJson = PlayerPrefs.GetString(STATS_KEY, "");
        _stats = string.IsNullOrEmpty(statsJson) ? new GameStats() : JsonUtility.FromJson<GameStats>(statsJson);

        string settingsJson = PlayerPrefs.GetString(SETTINGS_KEY, "");
        _settings = string.IsNullOrEmpty(settingsJson) ? new GameSettings() : JsonUtility.FromJson<GameSettings>(settingsJson);
    }

    public static void Save() {
        PlayerPrefs.SetString(STATS_KEY, JsonUtility.ToJson(_stats));
        PlayerPrefs.SetString(SETTINGS_KEY, JsonUtility.ToJson(_settings));
        PlayerPrefs.Save();
    }

    public static void RecordGameResult(int winner, float duration) {
        // winner: 0 = draw, 1 = player1, 2 = player2
        _stats.totalGamesPlayed++;
        _stats.totalGameDuration += duration;
        if (winner == 1) _stats.player1Wins++;
        else if (winner == 2) _stats.player2Wins++;
        else _stats.draws++;
        Save();
    }

    public static void ResetStats() {
        _stats = new GameStats();
        Save();
    }
}
