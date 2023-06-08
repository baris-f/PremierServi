using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
using Vector3 = System.Numerics.Vector3;

public class Character : MonoBehaviour
{
    public Animator animator;
    [FormerlySerializedAs("renderer")] public SpriteRenderer render;
    public float speed = 0.02f;
    public KeyCode key;
    public bool isIA;
    public bool isRunning = false;
    public bool isWalking = false;
    public Player player = null;
    public bool isInDanger = false;
    private bool isDead = false;

    private void Start()
    {
        render.sortingOrder = (int) (transform.position.y * 10);
        if (isIA)
            StartCoroutine(IA());
    }

    private void FixedUpdate()
    {
        if (isDead)
            return;
        Move();
        if (transform.position.x > 7f)
        {
            GameObject.Find("GameManager").GetComponent<GameManagerServi>().Win(player);
        }
    }

    IEnumerator IA()
    {
        if (Random.Range(0, 100) > 30)
            yield return new WaitForSeconds(Random.Range(0.5f, 2f));
        while (true)
        {
            isWalking = true;
            yield return new WaitForSeconds(Random.Range(0.5f, 2.5f));
            isWalking = false;
            if (!isInDanger) 
                yield return new WaitForSeconds(Random.Range(1f, 4f));
            if (Random.Range(0, 100) > 60 || isInDanger)
            {
                isRunning = true;
                yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
                isRunning = false;
                if (!isInDanger)
                    yield return new WaitForSeconds(Random.Range(1f, 4f));
            }
        }
    }

    public void Die()
    {
        isDead = true;
        animator.SetBool("Walking", false);
        animator.SetBool("Running", false);
        animator.SetTrigger("Death");
        if (player && player.weapon)
            Destroy(player.weapon);
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }

    private void Move()
    {
        Walk();
        Run();
    }
    
    private void Walk()
    {
        if (isWalking && !isRunning)
        {
            transform.Translate(Vector2.right * (speed / 2));
            animator.SetBool("Walking", true);
        }
        else
            animator.SetBool("Walking", false);
    }
    
    private void Run()
    {
        if (isRunning && !isWalking)
        {
            transform.Translate(Vector2.right * speed);
            animator.SetBool("Running", true);
        }
        else
            animator.SetBool("Running", false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Road"))
        {
            isInDanger = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Road"))
            isInDanger = false;
    }
}