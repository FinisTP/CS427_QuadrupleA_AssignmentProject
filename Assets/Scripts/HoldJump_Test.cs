using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Phat_Script
{
    public class HoldJump_Test : MonoBehaviour
    {
        [SerializeField] LayerMask groundLayer;
        [SerializeField] float jumpVelocity = 20f;
        [SerializeField] float JumpForce = 1f;
        [SerializeField] float moveVelocity = 10f;
        [SerializeField] float maxJumpTime = 2f;

        Rigidbody2D rigid;
        BoxCollider2D col;
        Animator anim;

        bool facingRight = true;
        float distanceToCheckGround = 0.2f;
        float currentMoveVelocity;
        float currentJumpVelocity = -9.81f;

        [SerializeField] bool isJumping = false;
        float startJumpY = 0f;
        float jumpTimer = 0f;
        [SerializeField] bool isGrounded = false;

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

            if (isGrounded) jumpTimer = maxJumpTime;

            // currentJumpVelocity = rigid.velocity.y;
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                rigid.AddForce(Vector2.up * JumpForce * Time.deltaTime, ForceMode2D.Impulse);
                anim.Play("Jump");
                isJumping = true;
            }

            if (Input.GetButton("Jump") && isJumping)
            {
                if (jumpTimer > 0)
                {
                    rigid.AddForce(Vector2.up * JumpForce * Time.deltaTime, ForceMode2D.Impulse);
                    jumpTimer -= Time.deltaTime;
                }
                else
                {
                    isJumping = false;
                    jumpTimer = 0f;
                }
            }

            if (Input.GetButtonUp("Jump"))
            {
                isJumping = false;
                jumpTimer = 0f;
            }
        }


        bool IsGrounded()
        {
            RaycastHit2D raycast = Physics2D.BoxCast(col.bounds.center, col.bounds.size, 0f, Vector2.down, distanceToCheckGround, groundLayer);

            // Color color = Color.green;
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
            isGrounded = IsGrounded();
            rigid.velocity = new Vector2(currentMoveVelocity, rigid.velocity.y);
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
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Coin")
            {
                Destroy(collision.gameObject);
            }
        }
    }
}

