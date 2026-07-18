using System.Collections;
using UnityEngine;

public class ScreenShakeEffects : MonoBehaviour
{
    [SerializeField] private float duration;
    public AnimationCurve curve;
    private Coroutine shakeCoroutine;

    public void ScreenShake()
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
        }
        shakeCoroutine = StartCoroutine(ShakeRoutine());
    }
    private IEnumerator ShakeRoutine()
    {
        Debug.Log("ScreenShAKING");
        float currentTime = 0;
        Vector3 originalPos = transform.localPosition;

        while (currentTime < duration)
        {
            transform.localPosition = originalPos + Random.insideUnitSphere * curve.Evaluate(currentTime/duration);
            currentTime += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = originalPos;
        shakeCoroutine = null;
    }

}
