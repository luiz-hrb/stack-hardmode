using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Outline))]
public class BlurUI : MonoBehaviour
{
    public bool startOnAwake = true;
    public float bloomTime;
    public float distance;
    private Outline outline;
    private float cronometer;

    #region Monobehaviour Events

    void Start()
    {
        outline = GetComponent<Outline>();
        if (startOnAwake)
            StartBlur();
    }

    void Update()
    {
        if (IsBlurring())
            DecreaseBlur();
    }
    #endregion

    public void StartBlur()
    {
        cronometer = bloomTime;
        UpdateBlur();
    }

    private void UpdateBlur()
    {
        if (outline != null)
        {
            float ratio = cronometer / bloomTime;
            outline.effectDistance = Vector2.one * distance * ratio;
        }
    }

    private void DecreaseBlur()
    {
        cronometer -= Time.deltaTime;
        UpdateBlur();
    }

    private bool IsBlurring()
    {
        return cronometer > 0f;
    }
}
