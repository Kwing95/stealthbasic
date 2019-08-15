using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightVisualizer : MonoBehaviour
{

    public bool isMask = false;
    public float sightDistance = 10;
    public int peripheral = 30;
    public int wedgeCount = 10;
    public float refreshRate = 0.1f;

    private bool alert = false;
    private LayerMask mask = ~(1 << 9) & ~(1 << 10) & ~(1 << 11) & ~(1 << 12);
    private GameObject sight;
    private float counter = 0;

    private Texture2D textureIdle;
    private Texture2D textureAlert;
    private Sprite spriteIdle;
    private Sprite spriteAlert;

    // Start is called before the first frame update
    void Start()
    {
        // create an array and fill the texture with your color
        textureIdle = new Texture2D(1025, 1025);
        textureAlert = new Texture2D(1025, 1025);

        List<Color> colsIdle = new List<Color>();
        List<Color> colsAlert = new List<Color>();
        for (int i = 0; i < (textureIdle.width * textureIdle.height); i++)
        {
            colsIdle.Add(new Color(1, 1, 0, 0.5f));
            colsAlert.Add(new Color(1, 0, 0, 0.5f));
        }
        textureIdle.SetPixels(colsIdle.ToArray());
        textureIdle.Apply();

        textureAlert.SetPixels(colsAlert.ToArray());
        textureAlert.Apply();

        spriteIdle = Sprite.Create(textureIdle, new Rect(0, 0, 1024, 1024), Vector2.zero, 1);
        spriteAlert = Sprite.Create(textureAlert, new Rect(0, 0, 1024, 1024), Vector2.zero, 1);
    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;
        if (counter > refreshRate)
            UpdateVisualizer();
    }

    public void UpdateVisualizer()
    {
        DrawSight(peripheral, sightDistance, wedgeCount);
        counter = 0;
    }

    private void DrawSight(int peripheral, float distance, int numWedges)
    {
        Destroy(sight);

        List<Vector2> vertices = new List<Vector2>(new Vector2[] { transform.position });
        List<ushort> triangles = new List<ushort>();

        vertices.Add(transform.position);

        for (int angle = -peripheral; angle <= peripheral; angle += (peripheral * 2) / numWedges)
        {
            Vector2 origin = transform.position;
            float angleInRadians = (angle + 90 + transform.eulerAngles.z) * Mathf.PI / 180;
            Vector2 direction = new Vector2(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians));

            RaycastHit2D hit = Physics2D.Raycast(origin, direction, sightDistance, mask);

            if (hit.collider != null)
                vertices.Add(hit.point);
            else
                vertices.Add(origin + ((Vector2)Vector3.Normalize(direction) * sightDistance));

            if (vertices.Count >= 3)
                triangles.AddRange(new ushort[] { 0, (ushort)(vertices.Count - 1), (ushort)(vertices.Count - 2) });
        }

        sight = DrawPolygon2D(vertices, triangles, new Color(1, alert ? 0 : 1, 0, 0.5f));
        sight.GetComponent<SpriteRenderer>().sortingLayerName = "Effects";
        sight.transform.parent = transform;
        if (isMask)
        {
            sight.GetComponent<SpriteRenderer>().enabled = false;
            SpriteMask blindfold = sight.AddComponent<SpriteMask>();
            blindfold.sprite = sight.GetComponent<SpriteRenderer>().sprite;
            //sight.GetComponent<SpriteRenderer>().sortingLayerName = "Blindfold";
        }

    }

    private GameObject DrawPolygon2D(List<Vector2> vertices, List<ushort> triangles, Color color)
    {

        GameObject polygon = new GameObject(); //create a new game object
        SpriteRenderer sr = polygon.AddComponent<SpriteRenderer>(); // add a sprite renderer
       
        sr.color = color; //you can also add that color to the sprite renderer

        sr.sprite = alert ? spriteAlert : spriteIdle;

        //convert coordinates to local space
        float lx = Mathf.Infinity, ly = Mathf.Infinity;
        foreach (Vector2 vi in vertices)
        {
            if (vi.x < lx)
                lx = vi.x;
            if (vi.y < ly)
                ly = vi.y;
        }

        Vector2[] localv = new Vector2[vertices.Count];
        for (int i = 0; i < vertices.Count; i++)
        {
            localv[i] = vertices[i] - new Vector2(lx, ly);
        }

        sr.sprite.OverrideGeometry(localv, triangles.ToArray()); // set the vertices and triangles

        polygon.transform.position = transform.InverseTransformPoint(transform.position) + new Vector3(lx, ly, -1); // return to world space

        return polygon;
    }

    public void SetAlert(bool value)
    {
        alert = value;
    }

}
