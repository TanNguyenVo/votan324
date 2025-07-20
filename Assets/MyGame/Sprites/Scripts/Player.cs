using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 30f;
    public Projectile laserPrefab;
    private UIManager uiManager;
    public int lives = 3;
    public Image[] hearts;
    //làm nhấp nháy khi bị trúng đạn
    private bool isInvincible = false;
    public float invincibilityDuration = 1.5f; // thời gian bất tử sau khi trúng đòn
    private SpriteRenderer spriteRenderer;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
        if (uiManager == null)
        {
            Debug.LogError("UIManager not found in scene!");
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float changeSteerH = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float changeSteerV = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        transform.Translate(changeSteerH, changeSteerV, 0);
        // transform.Rotate(0, -changeSteerV * 2, 0);



        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            FireLaser();
        }
    }

    private void FireLaser()
    {
        Projectile projectile = Instantiate(this.laserPrefab, this.transform.position, this.laserPrefab.transform.rotation);
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("ShootAudio");  
        }

    }

    // Kiểm tra va cham với các đối tượng khác
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Invader") ||
        other.gameObject.layer == LayerMask.NameToLayer("Missile"))
        {
            TakeDamage();
        }
    }

    //Hàm xử lí khi nhân vật bị trúng đẠN

    public void TakeDamage()
    {
        if (isInvincible || lives <= 0) return;

        lives--;
        //câp nhật giao diện hiển thị mạng sống của nhân vật
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < lives)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }

        if (lives <= 0)
        {
            // Game Over
            uiManager.ShowGameOver();
        }
        else
        {
            //Bật chế đọ bất tử
            StartCoroutine(InvincibilityCoroutine());
        }
    }

    //Coroutine để xử lí chế độ bất tử
    IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;

        float elapsed = 0f;
        bool visible = true;

        //Bất đầu nhấp nháy
        while (elapsed < invincibilityDuration)
        {
            visible = !visible;
            spriteRenderer.enabled = visible;

            yield return new WaitForSeconds(0.2f);
            elapsed += 0.2f;
        }

        spriteRenderer.enabled = true; // bật lại nếu đang tắt
        isInvincible = false;

        

    }

}
