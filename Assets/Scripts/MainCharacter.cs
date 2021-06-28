using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : MonoBehaviour
{
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float jumpVelocity = 20f;
    [SerializeField] float moveVelocity = 10f;

    Rigidbody2D rigid;
    BoxCollider2D col;
    Animator anim;

    bool facingRight = true;
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
        currentMoveVelocity = Input.GetAxisRaw("Horizontal") * moveVelocity;
        if (currentMoveVelocity != 0)
        {
            anim.Play("Walking");
        }
        if ((currentMoveVelocity > 0 && !facingRight) || (currentMoveVelocity < 0 && facingRight))
        {
            Flip();
        }

        currentJumpVelocity = rigid.velocity.y;
        if (Input.GetButton("Jump") && IsGrounded())
        {
            currentJumpVelocity = jumpVelocity;
            anim.Play("Jump");
        }
    }

    bool IsGrounded()
    {
        RaycastHit2D raycast = Physics2D.BoxCast(col.bounds.center, col.bounds.size, 0f, Vector2.down, distanceToCheckGround, groundLayer);

        Color color = Color.green;
        //Debug.DrawRay(col.bounds.center - new Vector3(col.bounds.extents.x, 0), Vector2.down * (col.bounds.extents.y + distanceToCheckGround), color);
        //Debug.DrawRay(col.bounds.center + new Vector3(col.bounds.extents.x, 0), Vector2.down * (col.bounds.extents.y + distanceToCheckGround), color);
        //Debug.DrawRay(col.bounds.center - new Vector3(col.bounds.extents.x, col.bounds.extents.y + distanceToCheckGround), Vector2.right * col.bounds.extents.x * 2, color);

        return raycast.collider != null;
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void FixedUpdate()
    {
        rigid.velocity = new Vector2(currentMoveVelocity, currentJumpVelocity);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            foreach (ContactPoint2D i in collision.contacts)
            {
                if (i.normal.y > 0)
                {
                    Destroy(collision.gameObject);
                    return;
                }
            }
            Time.timeScale = 0;
        }
        else if (collision.gameObject.tag == "Coin")
        {
            Destroy(collision.gameObject);
        }
    }
}
