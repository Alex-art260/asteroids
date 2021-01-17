using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
    public Rigidbody2D rb;
    public Vector2 direction;
    public float speed;
    public float shootingDelay;
    public float lastTimeShoot = 0f;
    public float bulletSpeed;

    public Transform player;
    public GameObject bullet;
    public SpriteRenderer spriteRenderer;
    public Collider2D collider;
    public bool disabled;
    public int points;
    public float timeBeforeSpawning;
    public int currentLevel = 0;

    public Transform startPosition;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        NewLevel();
    }

    // Update is called once per frame
    void Update()
    {
        if(disabled)
        {
            return;
        }
        if(Time.time > lastTimeShoot + shootingDelay)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);

            GameObject newBullet = Instantiate(bullet, transform.position, q);

            newBullet.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(0f, bulletSpeed));
            lastTimeShoot = Time.time;
        }
    }
    
    void FixedUpdate()
    {
        if (disabled)
        {
            return;
        }
        direction = (player.position - transform.position).normalized;
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
    }

    public void NewLevel()
    {
        Disable();
        currentLevel++;
        timeBeforeSpawning = Random.Range(5f, 20f);
        Invoke("Enable", timeBeforeSpawning);
        speed = currentLevel;
        bulletSpeed = 250 * currentLevel;
        points = 200 * currentLevel;
    }

    private void Enable()
    {
        transform.position = startPosition.position;
        collider.enabled = true;
        spriteRenderer.enabled = true;
        disabled = false;
    }

    public void Disable()
    {
        collider.enabled = false;
        spriteRenderer.enabled = false;
        disabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {        
        if(other.CompareTag("bullet"))
        {
            player.SendMessage("ScorePoints", points);
            Disable();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.CompareTag("Player"))
        {
            //Disable();
        }
    }
}
