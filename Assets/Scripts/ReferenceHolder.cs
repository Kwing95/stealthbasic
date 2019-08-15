using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReferenceHolder : MonoBehaviour
{

    public static ReferenceHolder instance;
    public GameObject player;
    public GameObject enemies;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        /*if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
