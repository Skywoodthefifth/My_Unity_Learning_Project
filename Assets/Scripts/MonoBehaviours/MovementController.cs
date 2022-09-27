using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public float movementSpeed = 5.0f;
    Vector2 movement = new Vector2();

    Animator animator;

    Rigidbody2D rb2D;





    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();



        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();

    }

    void FixedUpdate()
    {

        MoveCharacter();

    }

    private void MoveCharacter()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        movement.Normalize();
        rb2D.velocity = movement * movementSpeed;
    }



    private void UpdateState()
    {

        if (Mathf.Approximately(movement.x, 0.0f) && Mathf.Approximately(movement.y, 0.0f))
        {
            animator.SetBool("isWalking", false);
        }
        else
        {
            animator.SetBool("isWalking", true);
        }
        animator.SetFloat("xDir", movement.x);
        animator.SetFloat("yDir", movement.y);
    }
}
