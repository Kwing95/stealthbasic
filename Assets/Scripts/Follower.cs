using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{

    public GridMover subject;
    public Vector3 offset = new Vector3(0, 0, -10);
    public float panSpeed = 20;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = transform.position + ((subject.transform.position + offset - transform.position) / panSpeed);
    }
}
