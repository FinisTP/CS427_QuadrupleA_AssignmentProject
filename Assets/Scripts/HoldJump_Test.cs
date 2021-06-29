using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Phat_Script
{
    public class HoldJump_Test : MonoBehaviour
    {
        [Header("Movement")]   
        [SerializeField] float moveVelocity = 10f;
        [SerializeField] Vector2 startPoint;
        [SerializeField] ParticleSystem dustParticle;
        bool facingRight = true;
        float currentMoveVelocity;
        Rigidbody2D rigid;
        Animator anim;

        [Header("Jumping")]
        [SerializeField] LayerMask groundLayer;
        [SerializeField] Transform groundCheckPos;
        [SerializeField] float jumpVelocity = 20f;
        [SerializeField] float maxJumpTime = 2f;
        [SerializeField] float cornerJumpTime = 0.5f;
        bool fallJump = false;
        bool isJumping = false;
        float jumpTimer = 0f;
        bool isGrounded = false;

        private void Awake()
        {
            rigid = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            startPoint = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            bool check = Physics2D.OverlapBox(groundCheckPos.position, new Vector2(0.5f, 0.3f), 0f, groundLayer);
            if (isGrounded && !check)
            {
                if (!fallJump)
                {
                    fallJump = true;
                    StartCoroutine(TransitionToGroundless());
                } else if (isJumping)
                {
                    StopAllCoroutines();
                    fallJump = false;
                    isGrounded = false;
                }   
            }
            else isGrounded = check;
            if (!isGrounded) anim.Play("MarioJump");
            ProcessMove();
            ProcessJump();
        }

        private void FixedUpdate()
        {
            rigid.velocity = new Vector2(currentMoveVelocity, rigid.velocity.y);
            if (isJumping) rigid.velocity = new Vector2(rigid.velocity.x, jumpVelocity);
        }

        void ProcessMove()
        {
            currentMoveVelocity = Input.GetAxis("Horizontal") * moveVelocity;
            if (currentMoveVelocity != 0 && isGrounded)
            {
                anim.Play("MarioRun");
            }
            if ((currentMoveVelocity > 0 && !facingRight) || (currentMoveVelocity < 0 && facingRight))
            {
                Flip();
            }
            if (Mathf.Abs(Input.GetAxis("Horizontal")) >= 0.8 && isGrounded)
            {
                dustParticle.Play();
            }
        }

        void ProcessJump()
        {
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                jumpTimer = maxJumpTime;
                // anim.Play("Jump");
                // dustParticle.Play();
                GameManager_.Instance.soundManager.PlayClip("Jump", 0.75f);
                isJumping = true;
                
            }

            if (Input.GetButton("Jump") && isJumping)
            {
                if (jumpTimer >= 0)
                {
                    isJumping = true;
                    jumpTimer -= Time.deltaTime;
                }
                else isJumping = false;
            }

            if (Input.GetButtonUp("Jump")) isJumping = false;
        }

        IEnumerator TransitionToGroundless()
        {
            yield return new WaitForSeconds(cornerJumpTime);
            isGrounded = false; fallJump = false;
        }


        void Flip()
        {
            facingRight = !facingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Enemy")
            {
                foreach (ContactPoint2D i in collision.contacts)
                {
                    if (i.normal.y > 0)
                    {
                        GameManager_.Instance.soundManager.PlayClip("Stomp", 1f);
                        Destroy(collision.gameObject);
                        return;
                    }
                }
                GameManager_.Instance.soundManager.PlayClip("Die", 0.75f);
                GameManager_.Instance.AddLives(-1);
                transform.position = startPoint;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Coin")
            {
                GameManager_.Instance.soundManager.PlayClip("Coin", 0.75f);
                GameManager_.Instance.AddScore(100);
                Destroy(collision.gameObject);
            } else if (collision.gameObject.tag == "Goal")
            {
                GameManager_.Instance.soundManager.GetComponent<AudioSource>().Stop();
                GameManager_.Instance.soundManager.PlayClip("Win", 0.75f);
            }
        }
        private void OnDrawGizmos()
        {
            Gizmos.DrawCube(groundCheckPos.position, new Vector3(0.5f, 0.01f, 0f));
        }
    }
}

