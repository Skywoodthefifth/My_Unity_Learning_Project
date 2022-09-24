using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public float movementSpeed = 5.0f;
    Vector2 movement = new Vector2();

    Animator animator;
    string animationState = "AnimationState";

    enum CharStates {
        Idle,
        Walk,
        Attack,
        Die,
    }

    Rigidbody2D rb2D;

    private SpriteRenderer mySpriteRenderer;

    bool isFacingRight;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();

        mySpriteRenderer = GetComponent<SpriteRenderer>();

        isFacingRight = true;

        animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();
        MovementFlip();
    }

    void FixedUpdate() {
        
        MoveCharacter();

    }

    private void MoveCharacter() {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        movement.Normalize();
        rb2D.velocity = movement * movementSpeed;
    }

    private void MovementFlip() {
        //Flip the sprite of when moving left or right
        if(movement.x < 0f) {

            isFacingRight = true;

            mySpriteRenderer.flipX = isFacingRight;

        } else if(movement.x > 0f) { 

            isFacingRight = false;

            mySpriteRenderer.flipX = isFacingRight;
        }
    }

    private void UpdateState() {
        
        if(movement.x != 0 || movement.y != 0) 
        {
            animator.SetInteger(animationState, (int) CharStates.Walk);
        } 
        else if (movement.x == 0 && movement.y == 0)
        {
            animator.SetInteger(animationState, (int) CharStates.Idle);
        }
    }
    
}
