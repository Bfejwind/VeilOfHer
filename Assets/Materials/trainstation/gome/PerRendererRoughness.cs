using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class PerRendererRoughness : MonoBehaviour
{
    [Range(0f, 2f)]
    public float roughnessScale = 1f;   // 1 = default

    // cache references so we don't allocate every frame
    Renderer  rend;
    MaterialPropertyBlock mpb;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        mpb  = new MaterialPropertyBlock();
    }

    void OnValidate()          // runs in Editor when you tweak the slider
    {
        if (!rend) rend = GetComponent<Renderer>();
        if (mpb == null) mpb = new MaterialPropertyBlock();

        Apply();
    }

    void Start()               // runs once at runtime
    {
        Apply();
    }

    void Apply()
    {
        rend.GetPropertyBlock(mpb);
        mpb.SetFloat("_RoughnessScale", roughnessScale);
        rend.SetPropertyBlock(mpb);
    }
}