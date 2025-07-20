using UnityEngine;
using System.Collections;

public class MetallicController : MonoBehaviour
{
    private Material mat;
    private Coroutine currentEffect;

    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    public void StartMetallicEffect(float duration = 2f)
    {
        if (currentEffect != null)
            StopCoroutine(currentEffect);

        currentEffect = StartCoroutine(MetallicLerpEffect(duration));
    }

    private IEnumerator MetallicLerpEffect(float duration)
    {
        float t = 0f;
        bool forward = true;

        while (true)
        {
            t += Time.deltaTime / duration;
            float metallicValue = forward ? Mathf.Lerp(0f, 1f, t) : Mathf.Lerp(1f, 0f, t);

            mat.SetFloat("_Metallic", metallicValue);

            if (t >= 1f)
            {
                t = 0f;
                forward = !forward;
            }

            yield return null;
        }
    }
}
