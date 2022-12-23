using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MovementController : MonoBehaviour
{
    public float movementSpeed = 5.0f;
    Vector2 movement = new Vector2();

    Animator animator;

    Rigidbody2D rb2D;

    PhotonView view;

    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCoolDown = 1.0f;

    [SerializeField] private TrailRenderer trailRenderer;


    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();

        view = gameObject.GetPhotonView();

        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();
    }

    void FixedUpdate()
    {
        if (view.IsMine)
        {
            MoveCharacter();
        }
    }

    private void MoveCharacter()
    {
        if (isDashing)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
            return;
        }

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        movement.Normalize();
        rb2D.velocity = movement * movementSpeed;
    }



    private void UpdateState()
    {

        if (Mathf.Approximately(movement.x, 0.0f) && Mathf.Approximately(movement.y, 0.0f) && isDashing == false)
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

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        rb2D.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        trailRenderer.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        trailRenderer.emitting = false;
        isDashing = false;
        yield return new WaitForSeconds(dashingCoolDown);
        canDash = true;

    }
}
