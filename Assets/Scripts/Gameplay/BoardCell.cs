using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BoardCell : MonoBehaviour {
    [Header("Cell Identity")]
    public int Row;
    public int Col;

    [Header("References")]
    [SerializeField] private Image markImage;
    [SerializeField] private Button button;
    [SerializeField] private CellVFX cellVFX;

    private GameController _controller;

    public void Init(GameController controller) {
        _controller = controller;
        button.onClick.AddListener(OnCellClicked);
        ClearMark();
    }

    public void SetMark(TicTacToeGame.CellState state) {
        if (state == TicTacToeGame.CellState.Empty) { ClearMark(); return; }

        ThemeData theme = ThemeManager.Instance.CurrentTheme;
        button.interactable = false;

        Color markColor;
        if (state == TicTacToeGame.CellState.X) {
            markImage.sprite = theme.xSprite;
            markImage.color = theme.xColor;
            markColor = theme.xColor;
        } else {
            markImage.sprite = theme.oSprite;
            markImage.color = theme.oColor;
            markColor = theme.oColor;
        }

        markImage.gameObject.SetActive(true);

        StartCoroutine(PopIn(markImage.transform));

        cellVFX?.PlayPlacementBurst(markColor);
    }

    public void ClearMark() {
        markImage.gameObject.SetActive(false);
        button.interactable = true;
    }

    private void OnCellClicked() {
        AudioManager.Instance?.PlayPlacement();
        _controller.OnCellSelected(Row, Col);
    }

    private static IEnumerator PopIn(Transform t) {
        float duration = 0.2f;
        float elapsed = 0f;
        t.localScale = Vector3.zero;

        while (elapsed < duration) {
            elapsed += Time.deltaTime;
            float prog = elapsed / duration;

            const float c1 = 1.70158f, c3 = c1 + 1f;
            float s = 1f + c3 * Mathf.Pow(prog - 1f, 3f) + c1 * Mathf.Pow(prog - 1f, 2f);
            t.localScale = Vector3.one * Mathf.Max(0f, s);
            yield return null;
        }
        t.localScale = Vector3.one;
    }
}
