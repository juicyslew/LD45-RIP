using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    public float shakeDuration = 0f;
    public float shakefactor = 1.5f; //
    public float maxshakefactor = 1.2f;
    public float maxshakeduration = 1f;
    public float decreasefactor = 1.0f;
    Vector3 originalPos;

    // Use this for initialization
    void Start()
    {
        originalPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (shakeDuration > 0)
        {
            shakeDuration = Mathf.Min(shakeDuration, maxshakeduration);
            transform.localPosition = originalPos + ((Vector3)Random.insideUnitCircle) * Mathf.Min(shakefactor * shakeDuration, maxshakefactor) / 2.5f;

            shakeDuration -= Time.deltaTime * decreasefactor;
        }
        else
        {
            shakeDuration = 0f;
            transform.localPosition = originalPos;

        }
    }

    public void CamShake(float duradd)
    {
        shakeDuration = Mathf.Max(duradd, shakeDuration);
    }
}
