using UnityEngine;

/// <summary>
/// Makes an object pulse by changing its scale, color or emission over time.
/// Highly customizable and supports both UI and 3D objects.
/// </summary>
[AddComponentMenu("Effects/Pulsing Effect")]
public class PulsingEffect : MonoBehaviour
{
    [Header("Pulse Type")]
    [Tooltip("Choose what property to pulse")]
    public PulseMode pulseMode = PulseMode.Scale;

    public enum PulseMode { Scale, Color, Emission }

    [Header("General Settings")]
    [Tooltip("How fast the pulsing oscillates (cycles per second)")]
    [Range(0.1f, 10f)] public float speed = 2f;

    [Tooltip("How strong the pulse effect is")]
    [Range(0f, 1f)] public float intensity = 0.3f;

    [Tooltip("Optional: offset phase for syncing multiple objects")]
    public float phaseOffset = 0f;

    [Tooltip("If true, start pulsing immediately")]
    public bool playOnStart = true;

    [Header("Scale Settings")]
    [Tooltip("Base scale multiplier when pulsing")]
    public Vector3 baseScale = Vector3.one;

    [Header("Color Settings")]
    [Tooltip("Renderer or Graphic whose color will pulse")]
    public Renderer targetRenderer;
    public UnityEngine.UI.Graphic targetGraphic;

    [Tooltip("Base color of the object")]
    public Color baseColor = Color.white;

    [Tooltip("Color added at pulse peak")]
    public Color pulseColor = Color.cyan;

    [Header("Emission Settings")]
    [Tooltip("Material property name for emission (usually '_EmissionColor')")]
    public string emissionProperty = "_EmissionColor";

    private Vector3 originalScale;
    private Material targetMaterial;
    private bool isPlaying = false;

    private void Start()
    {
        if (pulseMode == PulseMode.Scale)
            originalScale = transform.localScale;

        if (pulseMode == PulseMode.Color || pulseMode == PulseMode.Emission)
        {
            if (targetRenderer != null)
                targetMaterial = targetRenderer.material;
        }

        if (playOnStart)
            StartPulse();
    }

    private void Update()
    {
        if (!isPlaying) return;

        float pulse = (Mathf.Sin((Time.time + phaseOffset) * speed * Mathf.PI * 2f) + 1f) / 2f; // 0–1 range
        float t = Mathf.Lerp(1f - intensity, 1f + intensity, pulse);

        switch (pulseMode)
        {
            case PulseMode.Scale:
                transform.localScale = baseScale * t;
                break;

            case PulseMode.Color:
                Color newColor = Color.Lerp(baseColor, pulseColor, pulse);
                if (targetGraphic != null)
                    targetGraphic.color = newColor;
                else if (targetMaterial != null)
                    targetMaterial.color = newColor;
                break;

            case PulseMode.Emission:
                if (targetMaterial != null)
                {
                    Color emission = Color.Lerp(baseColor, pulseColor, pulse);
                    targetMaterial.SetColor(emissionProperty, emission);
                }
                break;
        }
    }

    /// <summary> Starts the pulsing animation. </summary>
    public void StartPulse() => isPlaying = true;

    /// <summary> Stops the pulsing animation and resets to base state. </summary>
    public void StopPulse()
    {
        isPlaying = false;

        if (pulseMode == PulseMode.Scale)
            transform.localScale = baseScale;

        if (pulseMode == PulseMode.Color)
        {
            if (targetGraphic != null)
                targetGraphic.color = baseColor;
            else if (targetMaterial != null)
                targetMaterial.color = baseColor;
        }

        if (pulseMode == PulseMode.Emission && targetMaterial != null)
            targetMaterial.SetColor(emissionProperty, baseColor);
    }
}
