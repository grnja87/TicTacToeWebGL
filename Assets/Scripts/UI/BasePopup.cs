using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public abstract class BasePopup : MonoBehaviour {
    [SerializeField] protected RectTransform panel;

    private CanvasGroup _cg;
    private Coroutine _anim;

    protected virtual void Awake() {
        _cg = GetComponent<CanvasGroup>();
        gameObject.SetActive(false);
    }

    public virtual void Show() {
        if (_anim != null) StopCoroutine(_anim);
        gameObject.SetActive(true);
        _cg.blocksRaycasts = true;
        _cg.interactable = false;
        SetChildGraphicsVisible(false);
        _anim = StartCoroutine(AnimateScale(Vector3.zero, Vector3.one, 0.28f, easeOutBack: true,
            onDone: () => {
                SetChildGraphicsVisible(true);
                _cg.interactable = true;
            }));
        AudioManager.Instance?.PlayPopupOpen();
        OnShow();
    }

    public virtual void Hide() {
        if (_anim != null) StopCoroutine(_anim);
        _cg.interactable = false;
        _cg.blocksRaycasts = false;
        SetChildGraphicsVisible(false);
        _anim = StartCoroutine(AnimateScale(Vector3.one, Vector3.zero, 0.18f, easeOutBack: false,
            onDone: () => gameObject.SetActive(false)));
        AudioManager.Instance?.PlayPopupClose();
        OnHide();
    }

    protected virtual void OnShow() { }
    protected virtual void OnHide() { }

    public void OnCloseButtonClicked() {
        AudioManager.Instance?.PlayButtonClick();
        Hide();
    }

    private void SetChildGraphicsVisible(bool visible) {
        foreach (var graphic in panel.GetComponentsInChildren<UnityEngine.UI.Graphic>())
            if (graphic.transform != panel) graphic.enabled = visible;
    }

    private IEnumerator AnimateScale(Vector3 from, Vector3 to, float duration, bool easeOutBack, System.Action onDone) {
        float elapsed = 0f;
        panel.localScale = from;

        while (elapsed < duration) {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float s = easeOutBack ? EaseOutBack(t) : EaseInBack(t);
            panel.localScale = Vector3.LerpUnclamped(from, to, s);
            yield return null;
        }

        panel.localScale = to;
        onDone?.Invoke();
    }

    private static float EaseOutBack(float t) {
        const float c1 = 1.70158f, c3 = c1 + 1f;
        return 1f + c3 * Mathf.Pow(t - 1f, 3f) + c1 * Mathf.Pow(t - 1f, 2f);
    }

    private static float EaseInBack(float t) {
        const float c1 = 1.70158f, c3 = c1 + 1f;
        return c3 * t * t * t - c1 * t * t;
    }
}
