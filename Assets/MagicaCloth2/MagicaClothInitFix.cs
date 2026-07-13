using UnityEngine;
using MagicaCloth2;

public class MagicaClothInitFix : MonoBehaviour
{
    void Start()
    {
        var cloths = GetComponentsInChildren<MagicaCloth>();
        foreach (var cloth in cloths)
        {
            cloth.Initialize();
        }
    }
}
