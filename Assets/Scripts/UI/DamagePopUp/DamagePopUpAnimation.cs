using TMPro;
using UnityEngine;

public class DamagePopUpAnimation : MonoBehaviour
{
    public AnimationCurve opacityCurve;
    public AnimationCurve scaleCurve;
    public AnimationCurve heightCurve;
    private Vector3 origin;
    private TextMeshProUGUI damageTMP;
    private float timer = 0;
    private void Awake()
    {
        damageTMP = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        origin = transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        damageTMP.color = new Color(1,1,1, opacityCurve.Evaluate(timer));
        transform.localScale = Vector3.one * scaleCurve.Evaluate(timer);
        transform.position = origin + new Vector3(0,1 + heightCurve.Evaluate(timer), 0);
        timer += Time.deltaTime;
    }
}
