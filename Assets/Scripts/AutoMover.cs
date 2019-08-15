using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMover : MonoBehaviour
{
    public const int INFINITY = 1073741823;
    public const int QUEUE_MAX = 45;
    public readonly Vector2 NULL_VECTOR = new Vector2(-INFINITY, -INFINITY);
    public static readonly List<Vector2> cardinals = 
        new List<Vector2>(new Vector2[] { Vector2.right, Vector2.left,
            Vector2.up, Vector2.down });
    public bool verbose = false;

    public List<Vector2> route;
    public AudioClip alarm;
    public AudioClip punch;
    public GameObject cross;

    private GridMover mover;
    private GridMover player;
    private GameObject crossInstance;

    private AudioSource source;
    private int startAngle;
    private int routeProgress = 0;
    private List<Vector2> graph;
    private List<int> dist = new List<int>();
    private List<int> prev = new List<int>();
    private Vector2 destination;
    private bool chasing = false;
    private Shooter shooter;

    private float stunCounter = 0;

    private bool courtesyStun = false;
    private bool confusedStun = false;
    private Vector2 savedDestination;

   // public GameObject marker;

    // Start is called before the first frame update
    void Start()
    {
        crossInstance = Instantiate(cross, transform.position, Quaternion.identity);
        crossInstance.GetComponent<SpriteRenderer>().enabled = false;

        source = GetComponent<AudioSource>();
        shooter = GetComponent<Shooter>();
        player = ReferenceHolder.instance.player.GetComponent<GridMover>();
        startAngle = GetComponent<Rotator>().GetAngle();
        if (route.Count == 0)
            route.Add(transform.position);

        mover = GetComponent<GridMover>();
        graph = MakeGraph(transform.position);
        destination = transform.position;
        SetChasing(false);
    }

    // Update is called once per frame
    void Update()
    {

        if(stunCounter > 0)
        {
            
            stunCounter -= Time.deltaTime;
            return;
        } else if (confusedStun || courtesyStun)
        {
            if(confusedStun)
                destination = savedDestination;
            if (courtesyStun)
            {
                SetChasing(true);
            }
            confusedStun = false;
            courtesyStun = false;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.GetDiscretePosition());

        if(TouchingPlayer())
        {
            SetChasing(true);
            SetDestination(player.transform.position, true);
            
            if (!player.gameObject.GetComponent<Flasher>().IsFlashing())
            {
                //player.gameObject.GetComponent<ParticleSystem>().Play();
                Camera.main.GetComponent<Jerk>().Shake(1);
                source.PlayOneShot(punch);
                player.gameObject.GetComponent<Flasher>().Flash(1);
                player.gameObject.GetComponent<Health>().TakeDamage();
            }
                
        }

        if (!chasing)
        {
            destination = route[routeProgress % route.Count];
            if (Vector2.Distance(transform.position, destination) == 0)
            {
                ++routeProgress;
                if (route.Count == 1 && !chasing)
                    GetComponent<Rotator>().Rotate(startAngle);
            }
        }

        if (mover.GetCanTurn() && (distanceToPlayer > 1 || !chasing))
        {
            graph = MakeGraph(transform.position);
            List<int> path = FindPath(VectorToIndex(destination));
            if(path.Count > 1)
            {
                int nextTileIndex = path[1];
                Vector2 nextTile = graph[nextTileIndex];
                Vector2 hereToThere = nextTile - (Vector2)transform.position;
                mover.ChangeDirection(hereToThere);
            } else
            {
                if (chasing)
                {
                    crossInstance.GetComponent<SpriteRenderer>().enabled = false;
                    ConfuseStun(2);
                }
                SetChasing(false);
                destination = route[routeProgress % route.Count];
            }
        }
    }

    private void ConfuseStun(float duration)
    {
        savedDestination = destination;
        destination = transform.position;
        confusedStun = true;
        stunCounter = Mathf.Max(duration, stunCounter);
    }

    private void CourtesyStun(float duration)
    {
        savedDestination = destination;
        destination = transform.position;
        courtesyStun = true;
        stunCounter = Mathf.Max(duration, stunCounter);
    }

    private bool TouchingPlayer()
    {
        return Mathf.Abs(transform.position.x - player.transform.position.x) <= 1 &&
            Mathf.Abs(transform.position.y - player.transform.position.y) <= 1;
    }

    public void SetChasing(bool value)
    {
        if (value && !courtesyStun)
        {
            if (!source.isPlaying)
            {
                source.PlayOneShot(alarm);
                source.Play();
            }
            
            mover.moveSpeed = 4;
            stunCounter = 0;
            confusedStun = false;
        } else if(!value)
            mover.moveSpeed = 2;

        chasing = value;
        shooter.SetAutoFire(value);
        gameObject.GetComponent<SightVisualizer>().SetAlert(value);
    }

    public void SetDestination(Vector2 dest, bool detected=false)
    {
        destination = dest;
        if (detected)
        {
            crossInstance.transform.position = dest;
            crossInstance.GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    private int VectorToIndex(Vector2 point)
    {
        for(int i = 0; i < graph.Count; ++i)
        {
            if(Mathf.RoundToInt(graph[i].x) == Mathf.RoundToInt(point.x) &&
                Mathf.RoundToInt(graph[i].y) == Mathf.RoundToInt(point.y))
            {
                return i;
            }
        }
            
        return -1;
    }

    // Searches for a path from current position to destination
    private List<int> FindPath(int end)
    {
        List<int> path = new List<int>();
        List<int> queue = new List<int>();

        for (int i = 0; i < graph.Count; ++i)
        {
            queue.Add(i);
            dist.Add(INFINITY);
            prev.Add(-1);
        }

        int startingIndex = -1;
        for(int i = 0; i < graph.Count; ++i)
        {
            if(graph[i] == (Vector2)transform.position)
            {
                startingIndex = i;
                break;
            }
        }
        if (startingIndex == -1)
            return path;
        dist[startingIndex] = 0;

        while(queue.Count > 0)
        {

            int u = ClosestPoint(queue);
            queue.Remove(u);

            if (u == end && (prev[u] != -1 || u == startingIndex))
            {
                while (u != -1)
                {
                    path.Insert(0, u);
                    u = prev[u];
                }
                dist.Clear();
                prev.Clear();
                return path;
            }

            List<int> neighbors = GetNeighbors(queue, graph[u]);
            for(int v = 0; v < neighbors.Count; ++v)
            {
                int alt = dist[u] + 1;
                if(alt < dist[neighbors[v]])
                {
                    dist[neighbors[v]] = alt;
                    prev[neighbors[v]] = u;
                }
            }
        }

        return path;
    }

    private List<int> GetNeighbors(List<int> queue, Vector2 point)
    {
        List<int> neighbors = new List<int>();
        for(int i = 0; i < queue.Count; ++i)
            if(Vector2.Distance(graph[queue[i]], point) == 1)
                neighbors.Add(queue[i]);
        return neighbors;
    }

    // Requires that "distances" and "graph" have same length
    // REMEMBER: dist and prev are the size of the ENTIRE graph, not just queue
    private int ClosestPoint(List<int> queue)
    {
        int minimum = INFINITY;
        int closestPoint = -1;
        for(int i = 0; i < queue.Count; ++i)
            if(dist[queue[i]] < minimum) // replace with queue[i] ? ? ?
            {
                minimum = dist[queue[i]];
                closestPoint = queue[i];
            }
        return closestPoint;
    }

    private List<Vector2> MakeGraph(Vector2 start)
    {
        return GraphMaker.GetGraph();
    }

}
