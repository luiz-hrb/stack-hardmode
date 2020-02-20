using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    public AudioClip[] comboClips;
    public GameObject[] comboEffects;
    public AudioSource comboBaseSound;
    public Stack stack;
    private int comboID = -1;
    private AudioSource audioSource;

    #region Monobehaviour Events

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    #endregion

    public void Hit()
    {
        IncrementComboID();
        PlayCombo();
    }
      
    public void Miss()
    {
        comboID = -1;
    }

    public void PlayCombo()
    {
        if (comboID >= 0)
        {
            PlayComboAudio();
            PlayComboEffect();
        }
    }

    private void IncrementComboID()
    {
        int maxID = Mathf.Max(comboClips.Length - 1, comboEffects.Length - 1);
        comboID = Mathf.Clamp(comboID + 1, 0, maxID);
    }

    private void PlayComboAudio()
    {
        audioSource.clip = comboClips[comboID];
        audioSource.Play();
        comboBaseSound.Play();
    }

    private void PlayComboEffect()
    {
        var lastCard = stack.GetCard();
        var effect = Instantiate(comboEffects[comboID], null);
        float effectMargin = 0.1f;
        effect.transform.position = lastCard.transform.position;
        effect.transform.localScale = new Vector3(lastCard.transform.localScale.x + effectMargin,
                                                effect.transform.localScale.y,
                                                lastCard.transform.localScale.z + effectMargin);
    }
}
