using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{

    private int angle = 0;

    // Start is called before the first frame update
    void Awake()
    {
        angle = Mathf.RoundToInt(transform.rotation.eulerAngles.z);
        Rotate(angle);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Maybe put FaceDirection inside its own Rotator class?
    public void Rotate(Vector2 direction)
    {
        // 45 degrees means error
        angle = 45;

        if (direction.x > 0)
            angle = 270;
        else if (direction.x < 0)
            angle = 90;
        else if (direction.y > 0)
            angle = 0;
        else if (direction.y < 0)
            angle = 180;
        Rotate(angle);
    }

    public void Rotate(int ang)
    {
        angle = ang;
        transform.rotation = Quaternion.Euler(Vector3.forward * ang);
    }

    public Vector2 FrontOffset()
    {
        switch (angle)
        {
            case 0:
                return Vector2.up / 2;
            case 180:
                return Vector2.down / 2;
            case 90:
                return Vector2.left / 2;
            case 270:
                return Vector2.right / 2;
        }
        return Vector2.zero;
    }

    public int GetAngle()
    {
        return angle;
    }

}
