using TMPro;
using UnityEngine;

public class DamagePopUpGenerator : MonoBehaviour
{
    public static DamagePopUpGenerator current;
    public GameObject damagePopupPrefab;
    private void Awake()
    {
        current = this;
    }
    // Update is called once per frame
    // void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.G))
    //     {
    //         DamagePopup(Vector3.one, Random.Range(0,100).ToString(), Color.yellow);
    //     }
    // }
    public void DamagePopup(Vector3 position, string text, Color color)
    {
        var popUp = Instantiate(damagePopupPrefab, position, Quaternion.identity);
        var tempTMP = popUp.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        tempTMP.text = text;
        tempTMP.faceColor = color;
        Destroy(popUp, 1f);
    }
}
