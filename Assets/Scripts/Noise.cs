using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise : MonoBehaviour
{

    public static float hearDistance = 3.3f;
    private GameObject enemies;
    //private CircleCollider2D collider;
    private Color color;

    // Start is called before the first frame update
    void Start()
    {
        // Make size variable? hearDistance * amt, localScale * amt?
        enemies = ReferenceHolder.instance.enemies;
        color = GetComponent<SpriteRenderer>().color;

        for(int i = 0; i < enemies.transform.childCount; ++i)
        {
            if (Vector2.Distance(enemies.transform.GetChild(i).transform.position, transform.position) <= hearDistance)
            {
                AutoMover mover = enemies.transform.GetChild(i).GetComponent<AutoMover>();
                if (mover != null)
                {
                    mover.SetDestination(transform.position, true);
                    mover.SetChasing(true);
                }
                    
            }
        }
        //collider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = transform.localScale * 1.01f;
        color.a -= 0.5f * Time.deltaTime;
        GetComponent<SpriteRenderer>().color = color;
        if (color.a <= 0)
            Destroy(gameObject);
    }
}
