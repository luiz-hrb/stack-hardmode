using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreView : MonoBehaviour
{
    public Text highscore_value;
    public Text highscore_label;
    public int highscore;

    #region Monobehaviour Events

    void Awake()
    {
        highscore = PlayerPrefs.GetInt("highscore");
    }
    #endregion

    public void CheckHighscore(int score)
    {
        if (score > highscore)
            SetHighscore(score);
        ShowHighscore();
    }

    private void ShowHighscore()
    {
        highscore_value.text = highscore.ToString();
    }

    private void SetHighscore(int newHighscore)
    {
        highscore = newHighscore;
        highscore_label.text = "New Highscore!";
        PlayerPrefs.SetInt("highscore", highscore);
    }
}
