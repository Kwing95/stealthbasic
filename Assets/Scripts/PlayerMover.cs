using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{

    private GridMover mover;
    public GameObject noise;
    public GameObject enemies;
    private AudioSource source;
    public int runSpeed = 6;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        ReferenceHolder.instance.player = gameObject;
        mover = GetComponent<GridMover>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = GetInput();
        if (input != Vector2.zero)
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                mover.moveSpeed = 6;
            else
                mover.moveSpeed = 3;

            if (mover.ChangeDirection(input) && mover.moveSpeed == 6)
            {
                source.pitch = Random.Range(1f, 1.2f);
                source.Play();
                Instantiate(noise, transform.position, Quaternion.identity);
            }
                
        }
            
    }

    Vector2 GetInput()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = horizontalInput == 0 ? Input.GetAxisRaw("Vertical") : 0;
        return new Vector2(horizontalInput, verticalInput);
    }

}
