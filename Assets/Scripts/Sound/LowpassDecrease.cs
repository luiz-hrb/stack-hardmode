using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioLowPassFilter))]
public class LowpassDecrease : MonoBehaviour
{
    [Range(0f, 1f)]
    public float lerp = 0.1f;
    public float cutoffMin = 100f;
    AudioLowPassFilter lowpass;

    #region Monobehaviour Events

    void Start()
    {
        lowpass = GetComponent<AudioLowPassFilter>();
    }
    #endregion

    public void StartDecreaseLowpass()
    {
        InvokeRepeating("DecreaseLowPass", 0.1f, 0.1f);
    }

    private void DecreaseLowPass()
    {
        float frequency = lowpass.cutoffFrequency;
        lowpass.cutoffFrequency = Mathf.Lerp(frequency, cutoffMin, lerp);
    }
}
