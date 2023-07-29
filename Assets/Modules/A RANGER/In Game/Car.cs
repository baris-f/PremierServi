using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class Car : MonoBehaviour
{
    public float refSpeed = 0.02f;
    public SpriteRenderer sprite;
    public List<Color> colors = new();
    private float speed = 0.02f;

    private void Start()
    {
        randomize();
    }

    void randomize()
    {
        speed = refSpeed + Random.Range(-0.01f, 0.01f);
        sprite.color = colors[Random.Range(0, colors.Count)];
    }

    void FixedUpdate()
    {
        transform.Translate(Vector2.up * -speed);

        if ((transform.position.y < -7f && transform.rotation.z % 360 == 0) ||
            (transform.position.y > 6f && transform.rotation.z == 1))
            changeDir();
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col || !col.CompareTag("Player"))
            return;
        col.GetComponent<Character>().Die();
        //Destroy(col.gameObject);
        //Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        Debug.Log("invi");
    }

    void changeDir()
    {
        randomize();
        transform.Rotate(Vector3.forward, 180);
    }
}
