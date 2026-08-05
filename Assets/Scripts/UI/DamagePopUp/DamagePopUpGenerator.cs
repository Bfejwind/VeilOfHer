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
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            DamagePopup(Vector3.one, Random.Range(0,100).ToString());
        }
    }
    public void DamagePopup(Vector3 position, string text)
    {
        var popUp = Instantiate(damagePopupPrefab, position, Quaternion.identity);
        var tempTMP = popUp.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        tempTMP.text = text;
        Destroy(popUp, 1f);
    }
}
