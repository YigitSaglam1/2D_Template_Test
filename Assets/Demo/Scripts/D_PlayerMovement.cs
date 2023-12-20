using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_PlayerMovement : MonoBehaviour
{
    private Rigidbody2D playerRb;

    private float horizontal;
    private float vertical;

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");// -1 is left
        vertical = Input.GetAxisRaw("Vertical");// -1 is down
    }
}
