using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI bestScoreText;
    private void Start() {
        BestScore();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void BestScore()
    {
        Debug.Log("Best Score");
        bestScoreText.text = "Best Score: " + PlayerPrefs.GetInt("HightScore").ToString().PadLeft(4, '0');
    }

}
