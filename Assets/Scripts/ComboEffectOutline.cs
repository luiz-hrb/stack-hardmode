using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboEffectOutline : MonoBehaviour
{
    public float lifetime;
    public Vector3 finalSize;
    public Color color;
    private float cronometer;
    private Material material;

    #region Monobehaviour Events

    void Start()
    {
        material = GetComponentInChildren<Renderer>().material;
        StartCoroutine(Growing());
    }
    #endregion

    private void SetSize(Vector3 size)
    {
        transform.localScale = size;
    }

    private void SetColorAlpha(float alpha)
    {
        var newColor = material.color;
        newColor.a = Mathf.Clamp01(alpha);
        material.color = newColor;
    }

    IEnumerator Growing()
    {
        cronometer = lifetime;
        material.color = color;
        var originalSize = transform.localScale;
        float originalAlpha = color.a;
        while (cronometer > 0f)
        {
            float ratio = Mathf.Sin((Mathf.PI / 2f) * (cronometer / lifetime));
            SetColorAlpha(ratio * originalAlpha);
            SetSize(Vector3.Lerp(finalSize, originalSize, ratio));
            yield return new WaitForEndOfFrame();
            cronometer -= Time.deltaTime;
        }
        Destroy(gameObject);
    }
}
