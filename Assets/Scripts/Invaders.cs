using UnityEngine;
using UnityEngine.SceneManagement;

public class Invaders : MonoBehaviour
{
    [Header("Invaders")]
    public Invader[] prefabs = new Invader[5];
    public AnimationCurve speed;
    public System.Action<Invader> killed;
    public Vector3 initialPosition { get; private set; }
    private Vector3 _direction = Vector2.right;
    private AudioManager audioManager;

    public int amountKilled { get; private set; }
    public int amountAlive => this.totalInvaders - this.amountKilled;
    public int totalInvaders => this.rows * this.columns;
    public float percentKilled => (float)this.amountKilled / (float)this.totalInvaders; 
    // вираховування відсотку вбитих для прискорення вражини

    [Header("Grid")]
    public int rows = 5;
    public int columns = 11;
    
    [Header("Missiles")]
    public Projectile misslePrefab;
    public float missileAttackRate = 1.0f;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
        CreateInvaders();
    }

    private void Start()
    {
        InvokeRepeating(nameof(MissileAttack), this.missileAttackRate, this.missileAttackRate);
    }

    private void CreateInvaders()
    {
        initialPosition = transform.position;
        for (int row = 0; row < this.rows; row++)
        {
            float width = 2.0f * (this.columns - 1);
            float hight = 2.0f * (this.rows - 1);

            Vector2 centering = new Vector2(-width / 2, -hight / 2);
            Vector3 rowPosition = new Vector3(centering.x, centering.y + (row * 2.0f), 0.0f);

            for (int col = 0; col < this.columns; col++)
            {
                Invader invader = Instantiate(this.prefabs[row], this.transform);
                invader.killed += OnInvaderKilled;

                Vector3 position = rowPosition;
                position.x += col * 2.0f;
                invader.transform.localPosition = position;
            }
        }
    }

    private void Update() {

        this.transform.position += _direction * this.speed.Evaluate(this.percentKilled) * Time.deltaTime;

        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

        foreach ( Transform invader in this.transform)
        {
            if (!invader.gameObject.activeInHierarchy) {
                continue;
            }
            // удари інвейдерів об стінки, що заставляють їх рухатися униз
            if (_direction == Vector3.right && invader.position.x >= (rightEdge.x - 1.0f)) {
                AdvanceRow();
            } else if (_direction == Vector3.left && invader.position.x <= (leftEdge.x + 1.0f))
            {
                AdvanceRow();
            }
        }
    }

    private void AdvanceRow()
    {
        _direction.x *= -1.0f;

        Vector3 position = this.transform.position;
        position.y -= 1.0f;
        this.transform.position = position;
    }

    public void ResetInvaders()
    {
        amountKilled = 0;
        _direction = Vector3.right;
        transform.position = initialPosition;

        foreach (Transform invader in transform) {
            invader.gameObject.SetActive(true);
        }
        audioManager.PlaySfx("Reset");
    }

    private void MissileAttack() 
    {
        foreach ( Transform invader in this.transform)
        {
            if (!invader.gameObject.activeInHierarchy) {
                continue;
            }
            if (Random.value < (1.0f / (float)this.amountAlive))
            {
                Instantiate(this.misslePrefab, invader.position, Quaternion.identity);
                break;
            }
        }
    }
    void OnInvaderKilled(Invader invader)
    {
        invader.gameObject.SetActive(false);
        this.amountKilled++;
        killed(invader);
    }
}
