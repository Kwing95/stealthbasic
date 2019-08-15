using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoVanish : MonoBehaviour
{

    private float counter = 0;
    public float timeToLive = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;
        if(counter > timeToLive)
            Destroy(gameObject);
    }
}
