using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flasher : MonoBehaviour
{

    private float flashTimer = 0;
    private new SpriteRenderer renderer;
    private Color startingColor;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        startingColor = renderer.color;
    }

    // Update is called once per frame
    void Update()
    {
        flashTimer -= Time.deltaTime;
        if (flashTimer > 0)
            renderer.color = Mathf.Round(flashTimer * 7) % 2 == 0 ? Color.white : startingColor;
        else
            renderer.color = startingColor;
    }

    public void Flash(float time)
    {
        if(flashTimer <= 0)
            flashTimer = time;
    }

    public bool IsFlashing()
    {
        return flashTimer > 0;
    }

}
