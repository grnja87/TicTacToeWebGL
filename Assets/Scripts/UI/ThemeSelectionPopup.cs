using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ThemeSelectionPopup : BasePopup
{
    [SerializeField] private Transform optionContainer;
    [SerializeField] private GameObject themeOptionPrefab;
    [SerializeField] private Button startButton;

    private int _pendingIndex = -1;

    protected override void Awake() {
        base.Awake();
        startButton.onClick.AddListener(OnStartClicked);
    }

    protected override void OnShow() {
        _pendingIndex = GameData.Settings.selectedThemeIndex;

        foreach (Transform child in optionContainer)
            Destroy(child.gameObject);

        ThemeData[] themes = ThemeManager.Instance.GetAllThemes();
        for (int i = 0; i < themes.Length; i++) {
            int idx = i;
            GameObject obj = Instantiate(themeOptionPrefab, optionContainer);

            var images = obj.GetComponentsInChildren<Image>();
            var previewImg = images.Length > 1 ? images[1] : images.Length > 0 ? images[0] : null;
            if (previewImg != null && themes[i].themePreviewIcon != null)
                previewImg.sprite = themes[i].themePreviewIcon;

            var label = obj.GetComponentInChildren<TextMeshProUGUI>();
            if (label != null) label.text = themes[i].themeName;

            var btn = obj.GetComponent<Button>();
            if (btn != null)
                btn.onClick.AddListener(() => {
                    AudioManager.Instance?.PlayButtonClick();
                    _pendingIndex = idx;
                    HighlightSelected(idx);
                });
        }

        HighlightSelected(_pendingIndex);
    }

    private void HighlightSelected(int index) {
        int i = 0;
        foreach (Transform child in optionContainer) {
            var img = child.GetComponent<Image>();
            if (img != null)
                img.color = (i == index) ? Color.yellow : Color.white;
            i++;
        }
    }

    private void OnStartClicked() {
        AudioManager.Instance?.PlayButtonClick();
        if (_pendingIndex >= 0) ThemeManager.Instance.SelectTheme(_pendingIndex);
        Hide();
        SceneLoader.LoadGameScene();
    }
}
