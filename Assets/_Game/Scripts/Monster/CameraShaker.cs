using System.Collections;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    public static CameraShaker Instance;

    [SerializeField] private float duration, magnitude;
    private Vector3 originalPos;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        originalPos = transform.localPosition;
    }

    [ContextMenu("Shake Camera")]
    public void ShakeScreen()
    {
        StartCoroutine(Shake());
    }
    
    private IEnumerator Shake()
    {
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3 (x, y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        Vector3 current = transform.localPosition;
        Vector3 target = originalPos;
        Vector3 velocity = Vector3.zero; // Bunu class-level yap!
        float smoothTime = 0.9f;

        transform.localPosition = Vector3.SmoothDamp(current, target, ref velocity, smoothTime);

    }
}
