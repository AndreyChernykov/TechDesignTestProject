using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class PlayerManager : MonoBehaviour
{
    [SerializeField] State state;
    [SerializeField] int health;
    [SerializeField] float speedRun;
    [SerializeField] Transform housePosition;
    [SerializeField] AudioClip sword;
    [SerializeField] AudioClip damage;
    [SerializeField] AudioClip step;

    float stoppingDist = 0.5f;
    (float, float) randomPitch = (0.8f, 1.2f );

    public bool isAlive { get; private set; }
    public bool isAttack = false;
    public int Health { get { return health; } }

    Animator animator;
    Rigidbody2D rb;
    BoxCollider2D boxCollider;
    AudioSource audioSource;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>(); 

        isAlive = true;
    }

    enum State
    {
        idle,
        run,
        attack,
        damage,
        death,
    }

    private void Update()
    {
        if(health <= 0)
        {
            Death();
        }
    }

    private void OnMouseDown()
    {
        HeroState(State.attack);
        StartCoroutine(Attack());
    }

    private void OnMouseUp()
    {
        HeroState(State.idle);
    }

    IEnumerator Attack()
    {
        float[] times = { 0.1f, 0.8f };

        for (int i = 0; i < times.Length; i++)
        {
            yield return new WaitForSeconds(times[i]);
            audioSource.pitch = UnityEngine.Random.Range(randomPitch.Item1, randomPitch.Item2);
            if (!audioSource.isPlaying) audioSource.Play();
            isAttack = !isAttack;
        }
    }

    public void Run()
    {
        HeroState(State.run);
        StartCoroutine(GoToHouse());
    }

    IEnumerator GoToHouse()
    {

        while (MathF.Abs(transform.position.x - housePosition.position.x) > stoppingDist)
        {
            transform.eulerAngles = new Vector2(0, 180);
            rb.velocity = Vector2.left * speedRun;
            audioSource.pitch = UnityEngine.Random.Range(randomPitch.Item1, randomPitch.Item2);
            audioSource.PlayOneShot(step);
            yield return new WaitForSeconds(0.3f);
        }
        gameObject.SetActive(false);
        yield return null;
    }

    void Damage()
    {
        audioSource.pitch = UnityEngine.Random.Range(randomPitch.Item1, randomPitch.Item2);
        audioSource.PlayOneShot(damage);
        HeroState(State.damage);
        if(health > 0) health--;
    }

    void Death()
    {
        isAlive = false;
        boxCollider.enabled = false;
        rb.simulated = false;
        HeroState(State.death);
    }

    void HeroState(State stateHeto)
    {
        foreach (State s in Enum.GetValues(typeof(State)))
        {
            animator.SetBool(s.ToString(), false);
        }
        state = stateHeto;
        animator.SetBool(state.ToString(), true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            if (isAttack)
            {
                collision.gameObject.GetComponent<Enemy>().Damage();
            }
            else
            {
                Damage();
            }
            
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            HeroState(State.idle);
        }
    }
}
