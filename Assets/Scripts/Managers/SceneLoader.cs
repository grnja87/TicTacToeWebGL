using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public const string PLAY_SCENE = "PlayScene";
    public const string GAME_SCENE = "GameScene";

    public static void LoadPlayScene() => SceneManager.LoadScene(PLAY_SCENE);
    public static void LoadGameScene() => SceneManager.LoadScene(GAME_SCENE);
}
