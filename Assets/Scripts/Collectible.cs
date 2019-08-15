using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Collectible : MonoBehaviour
{

    public bool isGoal = false;
    private GridMover player;
    private AudioSource kaching;

    // Start is called before the first frame update
    void Start()
    {
        if (isGoal)
            transform.position = ReferenceHolder.instance.player.transform.position;
        kaching = GetComponent<AudioSource>();
        player = ReferenceHolder.instance.player.GetComponent<GridMover>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Mathf.Abs(transform.position.x - player.transform.position.x) <= 1 &&
            Mathf.Abs(transform.position.y - player.transform.position.y) <= 1 &&
            transform.position.z == 0)
        {

            if (!isGoal)
            {
                transform.position = transform.position + new Vector3(0, 0, 1);
                GetComponent<SpriteRenderer>().enabled = false;
                kaching.Play();
                gameObject.AddComponent<AutoVanish>().timeToLive = 1;
            }

            if(transform.parent.childCount == 1 && isGoal)
            {
                Transitioner.instance.Transition((SceneManager.GetActiveScene().buildIndex + 1) % 4);
                //SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1) % 3);
                Vector2 entryPoint = ReferenceHolder.instance.gameObject.GetComponent<GraphMaker>().entryPoint;
                ReferenceHolder.instance.gameObject.GetComponent<GraphMaker>().MakeGraph(entryPoint);
            }
                
        }
    }

}
