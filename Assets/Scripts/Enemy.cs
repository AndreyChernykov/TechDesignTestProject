using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int points;
    [SerializeField] int health;
    [SerializeField] float speed;
    [SerializeField] AudioClip damage;
    
    Rigidbody2D rb;
    AudioSource audioSource;
    bool isDash = false;
    (float, float) randomPitch = (0.6f, 1.5f);

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        audioSource.pitch = Random.Range(randomPitch.Item1, randomPitch.Item2);
    }

    
    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        if(!isDash)rb.velocity = new Vector2(-speed, -speed * 2);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            StartCoroutine(Dash());
        }
    }

    IEnumerator Dash()
    {
        isDash = true;
        rb.velocity = new Vector2(speed, speed);
        yield return new WaitForSeconds(1);
        isDash = false;
    }

    public void Damage()
    {
        health--;
        if (health > 0)
        {
            
            StartCoroutine(Dash());
        }
        else
        {
            StartCoroutine(Death());
        }
    }

    IEnumerator Death()
    {

        isDash = true;
        rb.velocity = new Vector2(speed/2, speed/2);
        audioSource.Stop();
        audioSource.PlayOneShot(damage);
        yield return new WaitForSeconds(1f);
        GameManager.points += points;
        Destroy(gameObject);
    }
}
