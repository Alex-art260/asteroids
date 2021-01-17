using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SpaceController : MonoBehaviour
{

    public Rigidbody2D rb;
    public float screenTop;
    public float screenButtom;
    public float screenRight;
    public float screenLeft;
    public float thrust;
    public float turnThrust;
    private float _thrustInput;
    private float _turnInput;

  //  public float speed; 
 //   public float rotationOffset;

    public GameObject bullet;
    public float bulletForce;
    public float deathForce;
    public Alien alien;

    public int score;
    public int lives;

    public Text scoreText;
    public Text livesText;
    public GameObject gameOverPanel;
    public GameObject newHighScorePanel;
    public InputField highScoreInput;
    public Text highScoreListText;
    public GameManager gm;

    public Color inColor;
    public Color normalColor;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;

        scoreText.text = "Score " + score;
        livesText.text = "Lives " + lives;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newPos = transform.position;
        if (transform.position.y > screenTop)
        {
            newPos.y = screenButtom;
        }
        if (transform.position.y < screenButtom)
        {
            newPos.y = screenTop;
        }
        if (transform.position.x > screenRight)
        {
            newPos.x = screenLeft;
        }
        if (transform.position.x < screenLeft)
        {
            newPos.x = screenRight;
        }
        transform.position = newPos;

        _thrustInput = Input.GetAxis("Vertical");
        _turnInput = Input.GetAxis("Horizontal");

        if(Input.GetMouseButtonDown(0))
        {
            GameObject newBullet = Instantiate(bullet, transform.position, transform.rotation);
            newBullet.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up * bulletForce);
            Destroy(newBullet, 5.0f);
        }

        transform.Rotate(Vector3.forward * _turnInput * Time.deltaTime * -turnThrust);

       /* Vector3 mousePos = Input.mousePosition;
        mousePos.z = 0;
        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);

        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + rotationOffset));

        Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetPos.z = 0;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime); управление для мышки */
    }

    private void FixedUpdate()
    {
        rb.AddRelativeForce(Vector2.up * _thrustInput);
        //rb.AddTorque(-_turnInput);
    }

    private void ScorePoints(int pointsToAdd)
    {
        score += pointsToAdd;
        scoreText.text = "Score " + score;
    }

    private void Respawn()
    {
        rb.velocity = Vector2.zero;
        transform.position = Vector2.zero;

        SpriteRenderer sr =  GetComponent<SpriteRenderer>();
        sr.enabled = true;
        sr.color = inColor;
        Invoke("Invulnereable", 3f);
    }

    private void Invulnereable()
    {
        GetComponent<Collider2D>().enabled = true;
        GetComponent<SpriteRenderer>().color = normalColor;
    }

    private void LoseLife()
    {
        lives--;
        livesText.text = "Lives " + lives;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        Invoke("Respawn", 3f);
        if (lives <= 0)
        {
            GameOver();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.relativeVelocity.magnitude > deathForce)
        {
            LoseLife();
            alien.Disable();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
       if(other.CompareTag("bean"))
        {
            LoseLife();
        }
    }
    private void GameOver()
    {
        CancelInvoke();

        if(gm.CheckForHighScore(score))
        {
            newHighScorePanel.SetActive(true);
        }
        else
        {
            gameOverPanel.SetActive(true);
            highScoreListText.text = "HIGH SCORE" + "\n" + "\n" + PlayerPrefs.GetString("highscoreName") + " " + PlayerPrefs.GetInt("highscore");
        }
    }

    public void HighScoreInput()
    {
        string newInput = highScoreInput.text;
        newHighScorePanel.SetActive(false);
        gameOverPanel.SetActive(true);
        PlayerPrefs.SetString("highscoreName", newInput);
        PlayerPrefs.SetInt("highscore", score);
        highScoreListText.text = "HIGH SCORE" + "\n" + "\n" + PlayerPrefs.GetString("highscoreName") + " " + PlayerPrefs.GetInt("highscore");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }
    public void PlayAgain()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
