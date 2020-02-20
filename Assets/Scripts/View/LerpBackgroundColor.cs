using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LerpBackgroundColor : MonoBehaviour
{
    [SerializeField] private Color targetColor;
    [SerializeField] private float lerp = 0.1f;
    private SpriteRenderer sprite;

    #region Monobehaviour Events

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        InvokeRepeating("UpdateColor", 0.1f, 0.1f);
    }
    #endregion

    public void SetColor(Color newColor)
    {
        targetColor = newColor;
    }

    private void UpdateColor()
    {
        sprite.color = Color.Lerp(sprite.color, targetColor, lerp);
    }
}
