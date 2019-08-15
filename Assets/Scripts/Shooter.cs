using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{

    public Material white;
    public AudioClip gunshot;

    private AudioSource source;
    private bool firing = false;
    private GridMover target;
    private float counter = 0;
    private float countMax = 1;
    private float marginOfError = 15;
    private LayerMask mask = ~(1 << 9);
    private Rotator rotator;
    private float peripheralVision = 30;

    // Start is called before the first frame update
    void Start()
    {
        rotator = GetComponent<Rotator>();
        source = GetComponent<AudioSource>();
        target = ReferenceHolder.instance.player.GetComponent<GridMover>();
    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;
        if(counter > countMax && firing)
        {
            FireShot();
            counter = 0;
        }
    }

    public void FireShot()
    {
        Vector2 direction = target.transform.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 9, mask);
        float angle = Vector2.Angle(rotator.FrontOffset(), direction);
        
        if (hit.collider == null || !hit.collider.CompareTag("Player") || angle > peripheralVision)
            return;

        angle += Random.Range(0, marginOfError) * (Random.Range(0, 2) == 0 ? 1 : -1);
        direction = Quaternion.AngleAxis(angle, Vector3.forward) * direction;
        
        hit = Physics2D.Raycast(transform.position, direction, 9, mask);

        if(hit.collider != null && Vector3.Distance(transform.position, target.transform.position) > 1)
        {
            source.PlayOneShot(gunshot);
            Camera.main.GetComponent<Jerk>().Shake(1);
            if (hit.collider.CompareTag("Player"))
            {
                //target.gameObject.GetComponent<ParticleSystem>().Play();
                target.gameObject.GetComponent<Flasher>().Flash(1);
                target.gameObject.GetComponent<Health>().TakeDamage();
            }
            GameObject shotObject = new GameObject();
            AutoVanish vanisher = shotObject.AddComponent<AutoVanish>();
            vanisher.timeToLive = 0.05f;
            LineRenderer shotLine = shotObject.AddComponent<LineRenderer>();
            shotLine.SetPositions(new Vector3[] { transform.position + new Vector3(0, 0, -1), (Vector3)hit.point + new Vector3(0, 0, -1) });
            shotLine.sortingLayerName = "ForegroundFX";
            shotLine.material = white;
            shotLine.startWidth = shotLine.endWidth = 0.03f;
        } 
    }

    public void SetAutoFire(bool value)
    {
        firing = value;
    }

}
