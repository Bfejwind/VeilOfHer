using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class CooldownTracker : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI cmdCooldownText;
    [SerializeField] private TextMeshProUGUI cmdChargesText;

    [Header("Cooldown Image")]
    [SerializeField] private Image cmdCooldownImage;

    public void CMDCooldownTracker(string time)
    {
        cmdCooldownText.text = time;
        if (cmdCooldownText != null)
            cmdCooldownText.text = time;
    }

    public void CMDChargesTracker(string num)
    {
        cmdChargesText.text = num;
        if (cmdChargesText != null)
            cmdChargesText.text = num;
    }

    public void SetCMDCooldownFill(float value)
    {
        if (cmdCooldownImage != null)
            cmdCooldownImage.fillAmount = value;
    }
}
