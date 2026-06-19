using TMPro;
using UnityEngine;

public class CooldownTracker : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI cmdCooldownText;
    [SerializeField]private TextMeshProUGUI cmdChargesText;

    public void CMDCooldownTracker(string time)
    {
        cmdCooldownText.text = time;
    }
    public void CMDChargesTracker(string num)
    {
        cmdChargesText.text = num;
    }
}
