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

    float stoppingDist = 0.5f;
    public bool isAlive { get; private set; }

    Animator animator;
    Rigidbody2D rb;
    BoxCollider2D boxCollider;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();

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
    }

    private void OnMouseUp()
    {
        HeroState(State.idle);
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
            yield return null;
        }
        gameObject.SetActive(false);
        yield return null;
    }

    void Damage()
    {
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
            Damage();
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
