using UnityEngine;
using UnityEngine.UI;

public class StrikeAnimator : MonoBehaviour {
    [SerializeField] private StrikeVFX strikeVFX;
    [SerializeField] private RectTransform boardRect;

    private RectTransform[,] _cellRects;

    public void Init(BoardCell[,] cells, RectTransform board) {
        boardRect = board;
        _cellRects = new RectTransform[3, 3];
        for (int r = 0; r < 3; r++)
            for (int c = 0; c < 3; c++)
                _cellRects[r, c] = cells[r, c].GetComponent<RectTransform>();
        Hide();
    }

    public void AnimateStrike(int r0, int c0, int r1, int c1, Color tint) {
        Vector2 start = GetBoardLocalPos(r0, c0);
        Vector2 end = GetBoardLocalPos(r1, c1);
        strikeVFX.Animate(start, end, tint);
    }

    public void Hide() => strikeVFX?.Hide();

    private Vector2 GetBoardLocalPos(int r, int c) {
        RectTransform cellRT = _cellRects[r, c];
        Vector3 worldPos = cellRT.position;

        Camera cam = GetCanvasCamera();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            boardRect,
            RectTransformUtility.WorldToScreenPoint(cam, worldPos),
            cam,
            out Vector2 localPos);
        return localPos;
    }

    private Camera GetCanvasCamera() {
        var canvas = boardRect.GetComponentInParent<Canvas>();
        return canvas != null ? canvas.worldCamera : null;
    }
}
