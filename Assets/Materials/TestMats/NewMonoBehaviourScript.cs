using UnityEngine;

public class ScrollBaseAndEmissionMap : MonoBehaviour
{
    [Header("Target")]
    public Renderer targetRenderer;
    public int materialIndex = 0;

    [Header("Scroll Speed")]
    public Vector2 scrollSpeed = new Vector2(0.1f, 0f);

    private Material mat;
    private Vector2 currentOffset;

    // HDRP Lit shader property names
    private readonly string baseColorMap = "_BaseColorMap";
    private readonly string emissiveColorMap = "_EmissiveColorMap";

    void Start()
    {
        if (targetRenderer == null)
            targetRenderer = GetComponent<Renderer>();

        if (targetRenderer == null)
        {
            Debug.LogError("No Renderer found.");
            enabled = false;
            return;
        }

        mat = targetRenderer.materials[materialIndex];

        currentOffset = mat.GetTextureOffset(baseColorMap);
    }

    void Update()
    {
        currentOffset += scrollSpeed * Time.deltaTime;

        mat.SetTextureOffset(baseColorMap, currentOffset);
        mat.SetTextureOffset(emissiveColorMap, currentOffset);
    }
}