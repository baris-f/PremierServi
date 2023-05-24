using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
using Vector3 = System.Numerics.Vector3;

public class CharacterController2D : MonoBehaviour
{
    public Animator animator;
    [FormerlySerializedAs("renderer")] public SpriteRenderer render;
    public float speed = 0.05f;
    public KeyCode key;
    public bool isIA;
    public bool isMoving = false;

    private void Start()
    {
        render.sortingOrder = (int) (transform.position.y * 10);
        if (isIA)
            StartCoroutine(IA());
    }

    private void FixedUpdate()
    {
        if (!isIA)
            Move(Input.GetKey(key));
        else
            Move(isMoving);
    }

    IEnumerator IA()
    {
        yield return new WaitForSeconds(Random.Range(0.5f, 2f));
        while (true)
        {
            isMoving = true;
            yield return new WaitForSeconds(Random.Range(0.5f, 2f));
            isMoving = false;
            yield return new WaitForSeconds(Random.Range(1f, 4f));
        }
    }

    private void Move(bool moving)
    {
        if (moving)
        {
            transform.Translate(Vector2.right * speed);
            animator.SetBool("Running", true);
        }
        else
            animator.SetBool("Running", false);
    }
}