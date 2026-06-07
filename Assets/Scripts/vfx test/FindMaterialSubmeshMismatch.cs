using UnityEngine;
using UnityEditor;

public class FindMaterialSubmeshMismatch
{
    [MenuItem("Tools/Find Material/Submesh Mismatches")]
    static void FindMismatches()
    {
        Renderer[] renderers = Object.FindObjectsByType<Renderer>(FindObjectsSortMode.None);

        int count = 0;

        foreach (Renderer r in renderers)
        {
            Mesh mesh = null;

            MeshFilter mf = r.GetComponent<MeshFilter>();
            if (mf != null)
                mesh = mf.sharedMesh;

            SkinnedMeshRenderer smr = r as SkinnedMeshRenderer;
            if (smr != null)
                mesh = smr.sharedMesh;

            if (mesh == null)
                continue;

            int matCount = r.sharedMaterials.Length;
            int subMeshCount = mesh.subMeshCount;

            if (matCount > subMeshCount)
            {
                count++;

                Debug.LogWarning(
                    $"Material/SubMesh mismatch on: {r.gameObject.name}\n" +
                    $"Materials: {matCount}, SubMeshes: {subMeshCount}\n" +
                    $"Path: {GetPath(r.transform)}",
                    r.gameObject
                );
            }
        }

        Debug.Log($"Finished. Found {count} mismatched renderer(s).");
    }

    static string GetPath(Transform t)
    {
        string path = t.name;

        while (t.parent != null)
        {
            t = t.parent;
            path = t.name + "/" + path;
        }

        return path;
    }
}