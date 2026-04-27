using UnityEngine;

public class ThemeManager : MonoBehaviour
{
    public static ThemeManager Instance { get; private set; }

    [SerializeField] private ThemeData[] themes;

    public ThemeData CurrentTheme { get; private set; }

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        int idx = Mathf.Clamp(GameData.Settings.selectedThemeIndex, 0, themes.Length - 1);
        CurrentTheme = themes[idx];
    }

    public ThemeData[] GetAllThemes() => themes;

    public void SelectTheme(int index) {
        if (index < 0 || index >= themes.Length) return;
        CurrentTheme = themes[index];
        GameData.Settings.selectedThemeIndex = index;
        GameData.Save();
    }
}
