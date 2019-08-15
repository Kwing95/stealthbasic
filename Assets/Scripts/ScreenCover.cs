using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenCover : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float width = GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        float height = GetComponent<SpriteRenderer>().sprite.bounds.size.y;

        float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        transform.localScale = new Vector2(worldScreenWidth / width, worldScreenHeight / height);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
