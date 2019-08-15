using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphMaker : MonoBehaviour
{

    public const int QUEUE_MAX = 45;
    public static readonly List<Vector2> cardinals =
        new List<Vector2>(new Vector2[] { Vector2.right, Vector2.left,
            Vector2.up, Vector2.down });
    public static List<Vector2> graph;

    public Vector2 entryPoint;

    // Start is called before the first frame update
    void Start()
    {
        graph = MakeGraph(entryPoint);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static List<Vector2> GetGraph()
    {
        return graph;
    }

    public List<Vector2> MakeGraph(Vector2 start)
    {
        List<Vector2> visited = new List<Vector2>(new Vector2[] { start });
        Queue<Vector2> queue = new Queue<Vector2>(new Vector2[] { start });

        //int graphSize = 0;
        while (queue.Count > 0 && queue.Count <= QUEUE_MAX)
        {
            //++graphSize;
            AddAdjacent(queue, visited, queue.Dequeue());
        }
        //Debug.Log(graphSize);
        return visited;
    }

    private void AddAdjacent(Queue<Vector2> queue, List<Vector2> visited, Vector2 point)
    {
        for (int i = 0; i < cardinals.Count; ++i)
        {
            if (!visited.Contains(point + cardinals[i]) && PointIsClear(point + cardinals[i]))
            {
                queue.Enqueue(point + cardinals[i]);
                visited.Add(point + cardinals[i]);
            }
        }
    }

    private bool PointIsClear(Vector2 point)
    {
        RaycastHit2D hit = Physics2D.Raycast(point, Vector2.zero, 0/*, mask*/);
        if (hit.collider != null)
        {
            return !hit.collider.CompareTag("Wall");
        }
        return true;
    }

}
