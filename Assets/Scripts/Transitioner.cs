using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transitioner : MonoBehaviour
{

    public static Transitioner instance;
    public float refreshRate = 0.025f;
    public AudioClip failed;
    public AudioClip complete;

    private AudioSource source;
    private float sizeNeeded;
    private int expanding = 0;
    private int targetScene;
    private float easing = 0.2f;
    private float counter = 0;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        if (instance != null && instance != this)
            Destroy(this);
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }

        float width = GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        float height = GetComponent<SpriteRenderer>().sprite.bounds.size.y;

        float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        sizeNeeded = Mathf.Max(worldScreenWidth / width, worldScreenHeight / height);

        transform.localScale = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {

        transform.position = Camera.main.transform.position + new Vector3(0, 0, 10);

        ++counter;
        if(counter > refreshRate)
        {
            counter = 0;

            if (expanding == 1)
            {
                if (transform.localScale.x < sizeNeeded)
                    transform.localScale = (transform.localScale + Vector3.one) * 1.05f;
                else
                {
                    SceneManager.LoadScene(targetScene);
                    expanding = -1;
                }
            }
            else if (expanding == -1)
            {
                if (transform.localScale.x > 0)
                    transform.localScale = (transform.localScale - Vector3.one) * 0.95f;
                else
                {
                    transform.localScale = Vector3.zero;
                    expanding = 0;
                }
                    
            }

        }
    }

    public void Transition(int scene)
    {
        targetScene = scene;
        if(!source.isPlaying)
            source.PlayOneShot(SceneManager.GetActiveScene().buildIndex == scene ? failed : complete);
        expanding = 1;
    }

}
