using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMover : MonoBehaviour
{

    public float moveSpeed = 1f;

    private bool canTurn = true;
    private char boundDirection;
    private float bound;
    private Rigidbody2D rb;
    private Vector2 nextDiscretePosition;
    private Rotator rotator;

    // Start is called before the first frame update
    void Start()
    {
        rotator = GetComponent<Rotator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!canTurn)
        {
            switch (boundDirection)
            {
                case 'U':
                    if (transform.position.y >= bound)
                    {
                        transform.position = new Vector2(transform.position.x, bound);
                        FinishMove();
                    }
                    break;
                case 'D':
                    if (transform.position.y <= bound)
                    {
                        transform.position = new Vector2(transform.position.x, bound);
                        FinishMove();
                    }
                    break;
                case 'R':
                    if (transform.position.x >= bound)
                    {
                        transform.position = new Vector2(bound, transform.position.y);
                        FinishMove();
                    }
                    break;
                case 'L':
                    if (transform.position.x <= bound)
                    {
                        transform.position = new Vector2(bound, transform.position.y);
                        FinishMove();
                    }
                    break;
            }
        }
    }

    private void FinishMove()
    {
        canTurn = true;
        rb.velocity = Vector2.zero;
        if (GetComponent<SightVisualizer>() != null)
            GetComponent<SightVisualizer>().UpdateVisualizer();
    }

    // Returns true if movement was produced
    public bool ChangeDirection(Vector2 direction)
    {
        if (canTurn && RaycastClear(direction))
        {
            if(direction.y > 0)
            {
                rb.velocity = Vector2.up * moveSpeed;
                nextDiscretePosition = (Vector2)transform.position + Vector2.up;
                bound = transform.position.y + 1;
                boundDirection = 'U';
                rotator.Rotate(0);
            }
            else if(direction.y < 0)
            {
                rb.velocity = Vector2.down * moveSpeed;
                nextDiscretePosition = (Vector2)transform.position + Vector2.down;
                bound = transform.position.y - 1;
                boundDirection = 'D';
                rotator.Rotate(180);
            }
            else if(direction.x > 0)
            {
                rb.velocity = Vector2.right * moveSpeed;
                nextDiscretePosition = (Vector2)transform.position + Vector2.right;
                bound = transform.position.x + 1;
                boundDirection = 'R';
                rotator.Rotate(270);
            }
            else if(direction.x < 0)
            {
                rb.velocity = Vector2.left * moveSpeed;
                nextDiscretePosition = (Vector2)transform.position + Vector2.left;
                bound = transform.position.x - 1;
                boundDirection = 'L';
                rotator.Rotate(90);
            }
            else
            {
                Debug.Log(gameObject.name + " did nothing");
                return false;
            }
            if (GetComponent<SightVisualizer>() != null)
                GetComponent<SightVisualizer>().UpdateVisualizer();

            canTurn = false;
            return true;
        }
        return false;
    }

    private bool RaycastClear(Vector2 direction)
    {
        LayerMask mask = ~(1 << 11);
        if (gameObject.CompareTag("Enemy"))
            mask = mask & ~(1 << 9);
        return Physics2D.Raycast((Vector2)transform.position + (direction * 0.5f), direction, 0.5f, mask).collider == null;
    }

    public bool GetCanTurn()
    {
        return canTurn;
    }

    public Vector2 GetDiscretePosition()
    {
        return nextDiscretePosition;
    }

}
