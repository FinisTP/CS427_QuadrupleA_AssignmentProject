using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float jumpVelocity = 10f;
    [SerializeField] float moveVelocity = 10f;
    [SerializeField] bool flipSprite = false;

    Rigidbody2D rigid;
    BoxCollider2D col;
    Animator anim;

    float distanceToCheckGround = 0.2f;
    float currentMoveVelocity;
    float currentJumpVelocity;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        currentJumpVelocity = rigid.velocity.y;
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            currentJumpVelocity = jumpVelocity;
            //anim.Play("Jump");
        }
        currentMoveVelocity = Input.GetAxisRaw("Horizontal") * moveVelocity;
    }

    bool IsGrounded()
    {
        RaycastHit2D raycast = Physics2D.BoxCast(col.bounds.center, col.bounds.size, 0f, Vector2.down, distanceToCheckGround, groundLayer);
        return raycast.collider.name == "Tilemap";
    }

    private void FixedUpdate()
    {
        rigid.velocity = new Vector2(currentMoveVelocity, currentJumpVelocity);
    }
}
