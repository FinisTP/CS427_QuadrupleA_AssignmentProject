using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] float moveSpeed = 3.0f;
    [SerializeField] float height = 1.0f;
    [SerializeField] bool isGoingRight = true;
    [SerializeField] float raycastingDistance = 1f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] bool initSpriteFacingRight = false;
    private SpriteRenderer _spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        if (initSpriteFacingRight)
            _spriteRenderer.flipX = !isGoingRight;
        else _spriteRenderer.flipX = isGoingRight;
    }


    void Update()
    {
        Vector3 directionTranslation = (isGoingRight) ? transform.right : -transform.right;
        directionTranslation *= Time.deltaTime * moveSpeed;

        transform.Translate(directionTranslation);
    }

    private void FixedUpdate()
    {
        CheckForWalls();
    }

    private void CheckForWalls()
    {
        Vector3 raycastDirection = (isGoingRight) ? Vector2.right : Vector2.left;

        RaycastHit2D hitWall = Physics2D.Raycast(transform.position + raycastDirection * raycastingDistance - new Vector3(0f, 0.25f, 0f), raycastDirection, 0.075f, groundLayer);
        RaycastHit2D hitAir = Physics2D.Raycast(transform.position + Vector3.down * height + raycastDirection * raycastingDistance - new Vector3(0f, 0.25f, 0f), raycastDirection, 0.075f, groundLayer);
        // Debug.DrawRay(transform.position + Vector3.down * height + raycastDirection * raycastingDistance - new Vector3(0f, 0.25f, 0f), raycastDirection, Color.blue);

        if (hitWall || hitAir)
        {
            isGoingRight = !isGoingRight;
            if (initSpriteFacingRight)
                _spriteRenderer.flipX = !isGoingRight;
            else _spriteRenderer.flipX = isGoingRight;
        }

    }
}
