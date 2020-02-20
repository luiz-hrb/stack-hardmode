using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameMode { Menu, Playing, EndGame }

public class GameManager : MonoBehaviour
{
    public GameMode gameMode;
    public Stack stack;
    public CameraLerp camera;
    public int score;
    public HighscoreView highscoreView;
    public LerpBackgroundColor lerpBackground;
    public AudioSource audioEndgame;
    public LowpassDecrease musicEffectLowpass;

    // User Interface
    public GameObject ui_menu, hud, ui_score;
    public Text text_score;

    public void StartGame()
    {
        gameMode = GameMode.Playing;
        ui_menu.SetActive(false);
        hud.SetActive(true);

        stack.StartStack();
        SetScore(0);
    }

    public void EndGame()
    {
        gameMode = GameMode.EndGame;

        hud.SetActive(false);
        ui_score.SetActive(true);

        ShowHighscore();
        camera.ZoomOut();
        audioEndgame.Play();
        musicEffectLowpass.StartDecreaseLowpass();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Stack");
    }

    public void Scored()
    {
        var card = stack.GetCard();
        SetScore(score + 1);
        camera.SetTarget(card.transform.position);
        lerpBackground.SetColor(card.GetColor());
    }

    private void SetScore(int newScore)
    {
        score = newScore;
        text_score.text = score.ToString();
    }

    private void ShowHighscore()
    {
        highscoreView.CheckHighscore(score);
    }
}
