using UnityEngine;
using UnityEngine.UI;

public class OrientationLayout : MonoBehaviour
{
    [System.Serializable]
    public struct AnchorPreset {
        public Vector2 anchorMin;
        public Vector2 anchorMax;
        public Vector2 anchoredPosition;
        public Vector2 sizeDelta;
    }

    [SerializeField] private RectTransform target;
    [SerializeField] private AnchorPreset portraitLayout;
    [SerializeField] private AnchorPreset landscapeLayout;

    private bool _wasPortrait;

    private void Update() {
        bool isPortrait = Screen.height > Screen.width;
        if (isPortrait == _wasPortrait) return;
        _wasPortrait = isPortrait;
        ApplyLayout(isPortrait ? portraitLayout : landscapeLayout);
    }

    private void ApplyLayout(AnchorPreset p) {
        target.anchorMin = p.anchorMin;
        target.anchorMax = p.anchorMax;
        target.anchoredPosition = p.anchoredPosition;
        target.sizeDelta = p.sizeDelta;
    }
}
