using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seeker : MonoBehaviour
{

    private GridMover player;
    public float sightDistance = 10;
    public int peripheralVision = 30;

    private Vector2 currentDirection;
    private Rotator rotator;
    private AutoMover mover;
    private LayerMask mask = ~(1 << 9) & ~(1 << 10) & ~(1 << 11);

    // Start is called before the first frame update
    void Start()
    {
        player = ReferenceHolder.instance.player.GetComponent<GridMover>();
        mover = GetComponent<AutoMover>();
        rotator = GetComponent<Rotator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 origin = transform.position;// + rotator.FrontOffset();
        Vector2 direction = (Vector2)player.transform.position - origin;

        float angle = Vector2.Angle(rotator.FrontOffset(), direction);
        if(angle < peripheralVision)
        {
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, sightDistance, mask);
            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {                    
                mover.SetDestination(player.GetDiscretePosition(), true);
                mover.SetChasing(true);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.parent.transform.position, player.transform.position);
            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                Debug.Log("Player detected!");
            }
        }
    }

}
