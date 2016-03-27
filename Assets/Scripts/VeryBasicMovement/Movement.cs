using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{

    Rigidbody2D rb;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        rb.position = rb.position + Vector2.right * h * Time.deltaTime;
        rb.position = rb.position + Vector2.up * v * Time.deltaTime * 10;
    }
}
