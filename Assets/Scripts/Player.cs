using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class Player : MonoBehaviour
{
    public AudioManager audioManager;
    public Projectile laserPrefab;
    public DeathRay deathRay;
    public TextMeshProUGUI textLive;
    public System.Action killed;
    public float speed = 5.0f;
    private bool _laserActive;
    public bool bonus = false;
    public int lives = 3;

    public SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.green;
    }

    private void Update()
    {
        Vector3 position = transform.position;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            position.x -= speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            position.x += speed * Time.deltaTime;
        }

        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

        // Clamp the position of the character so they do not go out of bounds
        position.x = Mathf.Clamp(position.x, leftEdge.x, rightEdge.x);
        transform.position = position;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            if (!bonus)
            {
                Shoot();
            }else
            {
                Debug.Log("Bonus Shoot");
                BonusShoot();
            }
        }
    }

    private void Shoot()
    {
        if (!_laserActive)
        {
            audioManager.PlaySfx("Shoot");
            Projectile projectile = Instantiate(this.laserPrefab, this.transform.position, Quaternion.identity);
            projectile.destroyed += LaserDestroyed;
            _laserActive = true;
        }

    }
    private void BonusShoot()
    {
        audioManager.PlaySfx("Explosion");
        DeathRay deathRay = Instantiate(this.deathRay, this.transform.position + new Vector3(.0f, 14.0f, .0f), Quaternion.identity);
        bonus = false;
        spriteRenderer.color = Color.green;
    }

    private void LaserDestroyed()
    {
        _laserActive = false;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Missile") || 
        other.gameObject.layer == LayerMask.NameToLayer("Invader"))
            {
                audioManager.PlaySfx("Player");
                if (killed != null) {
                killed.Invoke();
                }
            }
    }
    public void BonusSpeed()
    {
        audioManager.PlaySfx("Bonus");
        StartCoroutine(Timer());
    }
    IEnumerator Timer()
    {
        this.speed = 10.0f;
        Debug.Log("Швидкість тепер: " + speed);
        yield return new WaitForSeconds(20);
        this.speed = 5.0f;
        spriteRenderer.color = Color.green;
        Debug.Log("Швидкість тепер: " + speed);
    }
}