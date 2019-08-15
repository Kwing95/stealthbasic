using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jerk : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shake(float intensity)
    {
        transform.position = transform.position + new Vector3(Random.Range(0, intensity), Random.Range(0, intensity), 0);
    }
}
