using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Octrees;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Graph 
{
    public readonly Dictionary<OctreeNode, Node> nodes = new Dictionary<OctreeNode, Node>();
    public readonly HashSet<Edge> edges = new HashSet<Edge>();
    
    List<Node> pathList = new List<Node>();
    
    public int GetPathLength() => pathList.Count;
    public OctreeNode GetPathNode(int index)
    {
        if (pathList == null) return null;

        if (index < 0 || index >= pathList.Count)
        {
            Debug.LogError($"Index out of bounds. Path length: {pathList.Count}, Index: {index}");
            return null;
        }
        return pathList[index].octreeNode;
        
    }
    
    int maxIterations = 1000;

    public bool AStar(OctreeNode start, OctreeNode end)
    {
        pathList.Clear();
        Node startNode = FindNode(start);
        Node endNode = FindNode(end);

        if (startNode == null || endNode == null)
        {
            Debug.LogError("Start or end node is null");
            return false;
        }

        SortedSet<Node> openSet = new(new NodeComparer());
        HashSet<Node> closedSet = new();
        int iterationCount = 0;

        startNode.g = 0;
        startNode.h = Heuristic(startNode, endNode);
        startNode.f = startNode.g + startNode.h;
        startNode.from = null;
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            if (++iterationCount >= maxIterations)
            {
                Debug.LogError("A* exceeded max iterations");
                return false;
            }

            Node currentNode = openSet.First();
            openSet.Remove(currentNode);

            if (currentNode.Equals(endNode))
            {
                ReconstructPath(currentNode);
                return true;
            }
            
            closedSet.Add(currentNode);

            foreach (Edge edge in currentNode.edges)
            {
                Node neighborNode = Equals(edge.a, currentNode) ? edge.b : edge.a;
                if (closedSet.Contains(neighborNode))
                    continue;
                
                float tentative_gScore = currentNode.g + (currentNode.octreeNode.bounds.center - neighborNode.octreeNode.bounds.center).sqrMagnitude;

                if (tentative_gScore < neighborNode.g || !openSet.Contains(neighborNode))
                {
                    neighborNode.g = tentative_gScore;
                    neighborNode.h = Heuristic(neighborNode, endNode); 
                    neighborNode.f = neighborNode.g + neighborNode.h;
                    neighborNode.from = currentNode;
                    openSet.Add(neighborNode);
                }
            }
        }
        Debug.Log("No path found");
        return false;
    }

    private void ReconstructPath(Node currentNode)
    {
        while (currentNode != null)
        {
            pathList.Add(currentNode);
            currentNode = currentNode.from;
        }
        pathList.Reverse();
    }

    private float Heuristic(Node a, Node b)
    {
        return (a.octreeNode.bounds.center - b.octreeNode.bounds.center).sqrMagnitude;
    }

    public class NodeComparer: IComparer<Node>
    {
        public int Compare(Node x, Node y)
        {
            if (x == null && y == null) return 0;
            
            int compare = x.f.CompareTo(y.f);
            if (compare == 0)
            {
                return x.id.CompareTo(y.id);
            }

            return compare;
        }
    }
    
    public void AddNode(OctreeNode octreeNode)
    {
        if (!nodes.ContainsKey(octreeNode))
        {
            nodes.Add(octreeNode, new Node(octreeNode));
        }
    }

    public void AddEdge(OctreeNode a, OctreeNode b)
    {
        Node nodeA = FindNode(a);
        Node nodeB = FindNode(b);

        if (nodeA == null || nodeB == null)
            return;
        
        var edge = new Edge(nodeA, nodeB);

        if (edges.Add(edge))
        {
            nodeA.edges.Add(edge);
            nodeB.edges.Add(edge);
        }
    }

    public void DrawaGraph()
    {
        Gizmos.color = Color.blue;
        foreach (var edge in edges)
        {
            Gizmos.DrawLine(edge.a.octreeNode.bounds.center, edge.b.octreeNode.bounds.center);
        }
        foreach (var node in nodes.Values)
        {
            Gizmos.DrawWireSphere(node.octreeNode.bounds.center, 0.2f);
        }
    }
    
    Node FindNode(OctreeNode octreeNode)
    {
        nodes.TryGetValue(octreeNode, out Node node);
        return node;
    }
    
}

public class Node
{
    private static int nextID;
    public readonly int id;

    public float f, g, h;
    public Node from;
    
    public List<Edge> edges = new List<Edge>();
    public OctreeNode octreeNode;

    public Node(OctreeNode octreeNode)
    {
        this.id = nextID++;
        this.octreeNode = octreeNode;
    }
    public override bool Equals(object obj) => obj is Node other && id == other.id;
    public override int GetHashCode() => id.GetHashCode();
}

public class Edge
{
    public readonly Node a , b;
    public Edge(Node a, Node b)
    {
        this.a = a;
        this.b = b;
    }

    public override bool Equals(object obj)
    {
        return obj is Edge other && ((a == other.a && b == other.b) || (a == other.b && b == other.a));
    }
    
    public override int GetHashCode() => a.GetHashCode() ^ b.GetHashCode();
}