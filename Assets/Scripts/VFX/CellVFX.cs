using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CellVFX : MonoBehaviour {
    [Header("Flare Images")]
    [SerializeField] private Image flareGlowImage;
    [SerializeField] private Image flareSharpImage;

    [Header("Particle Images")]
    [SerializeField] private Image[] particles;

    [Header("Timing")]
    [SerializeField] private float flareDuration = 0.45f;
    [SerializeField] private float burstRadius = 60f;
    [SerializeField] private float particleSpeed = 0.4f;

    private RectTransform _rt;

    private void Awake() {
        _rt = GetComponent<RectTransform>();
        SetFlareActive(false);
        foreach (var p in particles)
            if (p != null) p.gameObject.SetActive(false);
    }

    public void PlayPlacementBurst(Color markColor) {
        StartCoroutine(FlareRoutine(markColor));
        StartCoroutine(ParticleBurstRoutine(markColor));
    }

    private IEnumerator FlareRoutine(Color tint) {
        if (flareGlowImage != null) SetFlareColor(flareGlowImage, tint);
        if (flareSharpImage != null) SetFlareColor(flareSharpImage, tint);

        SetFlareActive(true);

        float elapsed = 0f;
        while (elapsed < flareDuration) {
            elapsed += Time.deltaTime;
            float t = elapsed / flareDuration;

            float scale = Mathf.Lerp(0.2f, 1.4f, EaseOutCubic(t));
            float alpha = Mathf.Lerp(1f, 0f, EaseInCubic(t));

            if (flareGlowImage != null) {
                flareGlowImage.transform.localScale = Vector3.one * scale;
                SetAlpha(flareGlowImage, alpha);
            }
            if (flareSharpImage != null) {
                float st = Mathf.Clamp01(t * 1.3f);
                flareSharpImage.transform.localScale = Vector3.one * Mathf.Lerp(0.1f, 1.2f, EaseOutCubic(st));
                SetAlpha(flareSharpImage, Mathf.Lerp(0.8f, 0f, EaseInCubic(st)));
            }

            yield return null;
        }

        SetFlareActive(false);
    }

    private IEnumerator ParticleBurstRoutine(Color tint) {
        if (particles == null || particles.Length == 0) yield break;

        var rects = new RectTransform[particles.Length];
        var dirs = new Vector2[particles.Length];

        for (int i = 0; i < particles.Length; i++) {
            if (particles[i] == null) continue;
            rects[i] = particles[i].GetComponent<RectTransform>();
            particles[i].color = tint;
            particles[i].gameObject.SetActive(true);
            rects[i].anchoredPosition = Vector2.zero;
            rects[i].localScale = Vector3.one * 0.3f;
            SetAlpha(particles[i], 1f);

            float baseAngle = i * 90f + Random.Range(-30f, 30f);
            float rad = baseAngle * Mathf.Deg2Rad;
            dirs[i] = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
        }

        float elapsed = 0f;
        while (elapsed < particleSpeed) {
            elapsed += Time.deltaTime;
            float t = elapsed / particleSpeed;

            for (int i = 0; i < particles.Length; i++) {
                if (particles[i] == null || rects[i] == null) continue;

                float dist = Mathf.Lerp(0f, burstRadius, EaseOutCubic(t));
                float scale = Mathf.Lerp(0.3f, 1.1f, EaseOutCubic(Mathf.Min(t * 2f, 1f)));
                float alpha = Mathf.Lerp(1f, 0f, EaseInCubic(Mathf.Max(0f, t * 2f - 1f)));

                rects[i].anchoredPosition = dirs[i] * dist;
                rects[i].localScale = Vector3.one * scale;
                SetAlpha(particles[i], alpha);
            }

            yield return null;
        }

        foreach (var p in particles)
            if (p != null) p.gameObject.SetActive(false);
    }

    private void SetFlareActive(bool active) {
        if (flareGlowImage != null) flareGlowImage.gameObject.SetActive(active);
        if (flareSharpImage != null) flareSharpImage.gameObject.SetActive(active);
    }

    private static void SetFlareColor(Image img, Color tint) {
        img.color = Color.Lerp(Color.white, tint, 0.5f);
    }

    private static void SetAlpha(Image img, float a) {
        var c = img.color;
        c.a = a;
        img.color = c;
    }

    private static float EaseOutCubic(float t) => 1f - Mathf.Pow(1f - t, 3f);
    private static float EaseInCubic(float t) => t * t * t;
}
