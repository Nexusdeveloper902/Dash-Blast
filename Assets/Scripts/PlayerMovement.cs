using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 _movement;
    [SerializeField] private float speed;

    void Update()
    {
        _movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
    }

    void FixedUpdate()
    {
        transform.position += new Vector3(speed * Time.fixedDeltaTime * _movement.x, speed * Time.fixedDeltaTime * _movement.y, 0);
    }
}
