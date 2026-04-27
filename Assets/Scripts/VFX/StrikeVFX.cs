using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StrikeVFX : MonoBehaviour
{
    [Header("Strike Images")]
    [SerializeField] private Image strikeBar;
    [SerializeField] private Image strikeGlow;

    [Header("Animation")]
    [SerializeField] private float growDuration = 0.35f;
    [SerializeField] private float holdDuration = 0.15f;
    [SerializeField] private float glowPulse = 0.4f;

    private RectTransform _barRT;
    private RectTransform _glowRT;
    private Coroutine _pulseRoutine;

    private void Awake() {
        _barRT = strikeBar?.GetComponent<RectTransform>();
        _glowRT = strikeGlow?.GetComponent<RectTransform>();
        Hide();
    }

    public void Animate(Vector2 startPos, Vector2 endPos, Color tint) {
        Hide();
        StartCoroutine(AnimateRoutine(startPos, endPos, tint));
    }

    public void Hide() {
        if (_pulseRoutine != null) StopCoroutine(_pulseRoutine);
        if (strikeBar != null) strikeBar.gameObject.SetActive(false);
        if (strikeGlow != null) strikeGlow.gameObject.SetActive(false);
    }

    private IEnumerator AnimateRoutine(Vector2 startPos, Vector2 endPos, Color tint) {
        Vector2 delta = endPos - startPos;
        float length = delta.magnitude;
        float angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;
        Vector2 center = (startPos + endPos) * 0.5f;

        SetupImage(_barRT, center, angle, tint, 1.0f);
        SetupImage(_glowRT, center, angle, tint, 0.7f);

        strikeBar.gameObject.SetActive(true);
        strikeGlow.gameObject.SetActive(true);

        float elapsed = 0f;
        while (elapsed < growDuration) {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / growDuration);
            float eased = EaseOutBack(t);

            float w = Mathf.Lerp(0f, length, eased);
            _barRT.sizeDelta = new Vector2(w, _barRT.sizeDelta.y);
            _glowRT.sizeDelta = new Vector2(w * 1.2f, _glowRT.sizeDelta.y);

            SetAlpha(strikeGlow, Mathf.Lerp(0f, 0.6f, eased));
            SetAlpha(strikeBar, Mathf.Lerp(0f, 1.0f, eased));

            yield return null;
        }

        _barRT.sizeDelta = new Vector2(length, _barRT.sizeDelta.y);
        _glowRT.sizeDelta = new Vector2(length * 1.2f, _glowRT.sizeDelta.y);

        yield return new WaitForSeconds(holdDuration);

        _pulseRoutine = StartCoroutine(PulseGlow());
    }

    private IEnumerator PulseGlow() {
        while (true) {
            float elapsed = 0f;
            while (elapsed < glowPulse) {
                elapsed += Time.deltaTime;
                float t = elapsed / glowPulse;
                float alpha = 0.4f + 0.3f * Mathf.Sin(t * Mathf.PI);
                SetAlpha(strikeGlow, alpha);
                yield return null;
            }
        }
    }

    private void SetupImage(RectTransform rt, Vector2 pos, float angle, Color tint, float alpha) {
        if (rt == null) return;
        rt.anchoredPosition = pos;
        rt.localEulerAngles = new Vector3(0f, 0f, angle);
        rt.sizeDelta = new Vector2(0f, rt.sizeDelta.y);

        var img = rt.GetComponent<Image>();
        img.color = new Color(tint.r, tint.g, tint.b, alpha);
    }

    private static void SetAlpha(Image img, float a) {
        if (img == null) return;
        var c = img.color; c.a = a; img.color = c;
    }

    private static float EaseOutBack(float t) {
        const float c1 = 1.70158f;
        const float c3 = c1 + 1f;
        return 1f + c3 * Mathf.Pow(t - 1f, 3f) + c1 * Mathf.Pow(t - 1f, 2f);
    }
}
