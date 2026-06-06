using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

[ExecuteAlways]
[RequireComponent(typeof(Camera))]
public class HDRPCameraGlitchEffect : MonoBehaviour
{
    [Range(0f, 1f)]
    public float intensity = 0f;

    [Header("Full Effect Settings")]
    public float maxChromaticAberration = 0.45f;
    public float maxFilmGrain = 0.55f;
    public float maxVignette = 0.35f;
    public float maxLensDistortion = -0.18f;
    public float maxBloom = 0.35f;

    [Header("Optional Flicker")]
    public bool useFlicker = true;
    public float flickerSpeed = 12f;
    public float flickerAmount = 0.15f;

    [Header("Horizontal Flickering Lines")]
    public bool enableLines = true;

    [Range(0f, 2f)]
    public float lineIntensityMultiplier = 1f;

    public Color lineColor = new Color(0.2f, 1f, 0.9f, 0.45f);

    [Tooltip("How many horizontal lines can appear")]
    public int lineCount = 45;

    [Tooltip("Height of each line in pixels")]
    public float lineThickness = 2f;

    [Tooltip("How fast the lines flicker")]
    public float lineFlickerSpeed = 24f;

    [Tooltip("How fast the lines move vertically")]
    public float lineMoveSpeed = 35f;

    [Tooltip("Chance for each line to appear")]
    [Range(0f, 1f)]
    public float lineAppearChance = 0.35f;

    private Volume volume;
    private VolumeProfile profile;

    private ChromaticAberration chromatic;
    private FilmGrain filmGrain;
    private Vignette vignette;
    private LensDistortion lensDistortion;
    private Bloom bloom;

    private Texture2D whiteTexture;

    void OnEnable()
    {
        SetupVolume();
        SetupLineTexture();
    }

    void Update()
    {
        if (volume == null || profile == null)
            SetupVolume();

        ApplyEffect();
    }

    void SetupVolume()
    {
        volume = GetComponent<Volume>();

        if (volume == null)
            volume = gameObject.AddComponent<Volume>();

        volume.isGlobal = true;
        volume.priority = 999f;

        if (profile == null)
        {
            profile = ScriptableObject.CreateInstance<VolumeProfile>();
            profile.name = "Runtime Camera Glitch Profile";
        }

        volume.sharedProfile = profile;

        if (!profile.TryGet(out chromatic))
            chromatic = profile.Add<ChromaticAberration>(true);

        if (!profile.TryGet(out filmGrain))
            filmGrain = profile.Add<FilmGrain>(true);

        if (!profile.TryGet(out vignette))
            vignette = profile.Add<Vignette>(true);

        if (!profile.TryGet(out lensDistortion))
            lensDistortion = profile.Add<LensDistortion>(true);

        if (!profile.TryGet(out bloom))
            bloom = profile.Add<Bloom>(true);
    }

    void SetupLineTexture()
    {
        if (whiteTexture != null)
            return;

        whiteTexture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
        whiteTexture.SetPixel(0, 0, Color.white);
        whiteTexture.Apply();
    }

    void ApplyEffect()
    {
        float finalIntensity = intensity;

        if (useFlicker && intensity > 0f)
        {
            float flicker = Mathf.PerlinNoise(Time.realtimeSinceStartup * flickerSpeed, 0f);
            flicker = Mathf.Lerp(1f - flickerAmount, 1f + flickerAmount, flicker);
            finalIntensity *= flicker;
        }

        finalIntensity = Mathf.Clamp01(finalIntensity);

        chromatic.active = true;
        chromatic.intensity.overrideState = true;
        chromatic.intensity.value = maxChromaticAberration * finalIntensity;

        filmGrain.active = true;
        filmGrain.type.overrideState = true;
        filmGrain.type.value = FilmGrainLookup.Thin1;
        filmGrain.intensity.overrideState = true;
        filmGrain.intensity.value = maxFilmGrain * finalIntensity;
        filmGrain.response.overrideState = true;
        filmGrain.response.value = 0.8f;

        vignette.active = true;
        vignette.intensity.overrideState = true;
        vignette.intensity.value = maxVignette * finalIntensity;
        vignette.smoothness.overrideState = true;
        vignette.smoothness.value = 0.45f;

        lensDistortion.active = true;
        lensDistortion.intensity.overrideState = true;
        lensDistortion.intensity.value = maxLensDistortion * finalIntensity;
        lensDistortion.scale.overrideState = true;
        lensDistortion.scale.value = Mathf.Lerp(1f, 1.08f, finalIntensity);

        bloom.active = true;
        bloom.intensity.overrideState = true;
        bloom.intensity.value = maxBloom * finalIntensity;
    }

    void OnGUI()
    {
        if (!enableLines || intensity <= 0.001f)
            return;

        if (whiteTexture == null)
            SetupLineTexture();

        float lineIntensity = Mathf.Clamp01(intensity * lineIntensityMultiplier);
        float time = Time.realtimeSinceStartup;

        for (int i = 0; i < lineCount; i++)
        {
            float randomBase = Hash(i * 13.17f + Mathf.Floor(time * lineFlickerSpeed));

            if (randomBase > lineAppearChance)
                continue;

            float ySeed = Hash(i * 8.23f);
            float y = (ySeed * Screen.height + time * lineMoveSpeed * (0.5f + ySeed)) % Screen.height;

            float alphaNoise = Hash(i * 4.91f + Mathf.Floor(time * lineFlickerSpeed * 0.7f));
            float alpha = lineColor.a * lineIntensity * Mathf.Lerp(0.25f, 1f, alphaNoise);

            Color oldColor = GUI.color;
            GUI.color = new Color(lineColor.r, lineColor.g, lineColor.b, alpha);

            float widthNoise = Hash(i * 2.77f);
            float lineWidth = Mathf.Lerp(Screen.width * 0.35f, Screen.width, widthNoise);
            float x = Hash(i * 5.31f + Mathf.Floor(time * 6f)) * (Screen.width - lineWidth);

            GUI.DrawTexture(
                new Rect(x, y, lineWidth, lineThickness),
                whiteTexture
            );

            GUI.color = oldColor;
        }
    }

    float Hash(float n)
    {
        return Mathf.Repeat(Mathf.Sin(n) * 43758.5453f, 1f);
    }

    public void SetIntensity(float value)
    {
        intensity = Mathf.Clamp01(value);
    }
}