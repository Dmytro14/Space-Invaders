using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    private Player player;
    private MysteryShip mysteryShip;
    private Invaders invaders;
    private AudioManager audioManager;
    public Projectile projectile;
    public int score { get; private set; }
    public int lives { get; private set; }
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    public GameObject winUI;
    public GameObject loseUI;

    private bool BonusOn;

    private void Awake()
    {
        mysteryShip = FindObjectOfType<MysteryShip>();
        invaders = FindObjectOfType<Invaders>();
        player = FindObjectOfType<Player>();
        audioManager = FindObjectOfType<AudioManager>();
        
    }
    private void Start()
    {
        Time.timeScale = 1f;
        SetScore(0);
        SetLives(3);
        winUI.SetActive(false);
        loseUI.SetActive(false);

        player.killed += OnPlayerKilled;
        mysteryShip.killed += OnMysteryShipKilled;
        invaders.killed += OnInvaderKilled;
    }
    private void Update() {
        if (Input.GetKey(KeyCode.R))
        {
            TotalScore();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (Input.GetKey(KeyCode.O))
        {
             PlayerPrefs.SetInt("HightScore", 0);
        }
    }
    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString().PadLeft(4, '0');
    }
    public void TotalScore()
    {
        if (this.score > PlayerPrefs.GetInt("HightScore"))
        {
            PlayerPrefs.SetInt("HightScore", this.score);
        }
    }
    private void SetLives(int lives)
    {
        this.lives = Mathf.Max(lives, 0);
        livesText.text = lives.ToString();
    }
    public void BonusHp()
    {
        audioManager.PlaySfx("Bonus");
        SetLives( this.lives + 2 );
    }
    private void NewRound()
    {
        invaders.ResetInvaders();
    }
    public void Lose()
    {
        TotalScore();
        loseUI.SetActive(true);
        Time.timeScale = 0f;
    }
    private void Win()
    {
        if (this.score >= 9999)
        {
            this.score = 9999;
            TotalScore();
            scoreText.text = score.ToString().PadLeft(4, '0');
            winUI.SetActive(true);
            audioManager.PlayWinMusic();
            Time.timeScale = 0f;
        }
    }
    private void OnPlayerKilled()
    {
        SetLives(lives - 1);

        if (lives <= 0)
            {
                Lose(); 
            }
    }

    private void OnMysteryShipKilled(MysteryShip mysteryShip)
    {
        SetScore(score + mysteryShip.score);
        Win();   
        
    }
    private void OnInvaderKilled(Invader invader)
    {
        audioManager.PlaySfx("Invader");
        SetScore(score + invader.score);
        Win();
        if (invaders.amountKilled == invaders.totalInvaders)
        {
            NewRound();
        }
    }

}
