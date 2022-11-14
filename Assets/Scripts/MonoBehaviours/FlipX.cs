﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipX : MonoBehaviour
{
    //SpriteRenderer SpriteRenderer;

    //bool isFacingRight;

    Vector3 lastPosition;

    // Start is called before the first frame update
    void Start()
    {
        //SpriteRenderer = GetComponent<SpriteRenderer>();

        //isFacingRight = true;

        lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Delta: " + (transform.position - lastPosition));
        MovementFlip((transform.position - lastPosition));
        lastPosition = transform.position;
    }





    private void MovementFlip(Vector3 deltaPosition)
    {
        //Flip the sprite when moving left or right
        if (deltaPosition.x > 0.0f)
        {


            //isFacingRight = true;

            //SpriteRenderer.flipX = !isFacingRight;
            transform.localScale = new Vector3(1, 1, 1);


        }
        else if (deltaPosition.x < 0.0f)
        {

            //isFacingRight = false;

            //SpriteRenderer.flipX = !isFacingRight;
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}

