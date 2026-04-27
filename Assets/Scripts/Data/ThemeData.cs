using UnityEngine;

[CreateAssetMenu(menuName = "TicTacToe/Theme", fileName = "NewTheme")]
public class ThemeData : ScriptableObject
{
    [Header("Theme Identity")]
    public string themeName = "Classic";
    public Sprite themePreviewIcon;

    [Header("X and O Sprites")]
    public Sprite xSprite;
    public Sprite oSprite;

    [Header("Tint Colors (Color.white = use sprite as-is)")]
    public Color xColor = Color.white;
    public Color oColor = Color.white;
}
