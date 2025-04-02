using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MapManager: Singleton<MapManager>
{
    #region Global variables used by other classes
    public Dictionary<Vector3, bool> surroundBasePoints = new Dictionary<Vector3, bool>();//Enemy move target
    //Territory Management --> CanvasGameplay
    public static Dictionary<int, List<TerritoryGrid>> gridDictionary = new Dictionary<int, List<TerritoryGrid>>();
    public List<BarrackBase> BarrackNotFullList = new List<BarrackBase>();//Component_Check_Defender + Component_Move_Minion
    [HideInInspector] public GameObject village;//Component_Check_Totem + Component_Move_Enemy + Component_Move_Minion
    [HideInInspector] public Collider villageCollider;
    #endregion
    
    #region Prefab References
    [SerializeField] private GameObject groundPrefab;
    [SerializeField] private Transform groundParent;
    
    [SerializeField] private GameObject wallGridPrefab;
    [SerializeField] private Transform wallParent;
    
    [SerializeField] private GameObject enemyPortalPrefab;
    [SerializeField] private Transform enemyPortalParent;
    
    [SerializeField] private GameObject territoryGridPrefab;
    [SerializeField] private Transform territoryParent;
    
    [SerializeField] private GameObject villageGridPrefab;
    [SerializeField] private Transform villageParent;
    
    
    
    [SerializeField] private NavMeshSurface surface;
    #endregion
    
    #region Resources variables
    [SerializeField] private TextAsset mapText;
    #endregion
    
    #region Map Data, used to generate map
    private GameObject ground;
    private Vector3 startPosition;
    private Vector3 generatePosition;
    private int cellSize = 10;
    private int gridValue;
    private int territoryID;
    private int territoryType;
    private float distance = 1f;//Khoảng cách giữa các điểm
    #endregion
    
    #region Start functions

    public void OnInit()//GameManager
    {
        GenerateGround();
        startPosition = FindStartPosition();
        GenerateObstacle();
        
        surface.BuildNavMesh();
        FindSurroundBasePoints();
        //octree = new Octree(objects, minNodeSize, wayPoints);//Khởi tạo khi bắt đầu game
        
    }
    
    #endregion
    
    #region restart functions
    private List<GameObject> GeneratedObjects = new List<GameObject>();

    public void  OnDestroy()//GameManager
    {
        surroundBasePoints = new Dictionary<Vector3, bool>();
        gridDictionary = new Dictionary<int, List<TerritoryGrid>>();
        BarrackNotFullList = new  List<BarrackBase>();
        foreach (GameObject obj in GeneratedObjects)
        {
            Destroy(obj);
        }
    }
    
    #endregion
    

    #region Map Generation functions
    private void GenerateGround()
    {
        ground = Instantiate(groundPrefab, transform.position, Quaternion.identity, transform);
        GeneratedObjects.Add(ground);
    }
    private Vector3 FindStartPosition()
    {
        return ground.transform.position - new Vector3(10*ground.transform.localScale.x/2 - cellSize/2, - 0.3f, 10*ground.transform.localScale.z/2 - cellSize/2);
    }
    private void GenerateObstacle()
    {
        string[] textSplit = mapText.text.Split('\n');
        for (int row = 0; row < textSplit.Length; row++)
        {
            string[] lineSplit = textSplit[row].Split('-');
            for (int column = 0; column < lineSplit.Length; column++)
            {
                gridValue = int.Parse(lineSplit[column]);
                if (FindGrid() == (null, null))
                    continue;
                var (prefab, parent) = FindGrid(); 
                generatePosition = FindGeneratePosition(column, row);
                GenerateGrid(prefab, parent);
            }
        }
    }
    private (GameObject, Transform) FindGrid()
    {
        switch (gridValue)
        {
            case 0:
                return (null, null);
            case 1:
                return (villageGridPrefab, villageParent);
            case 2:
                return (enemyPortalPrefab, enemyPortalParent);
            case 3:
                return (wallGridPrefab, wallParent); 
            default:
                return (territoryGridPrefab,territoryParent);
        }
    }
    private Vector3 FindGeneratePosition(int x, int z)
    {
        if (gridValue == 2)// (enemyPortal)
        {
            return startPosition + new Vector3(x*10 ,3f , z*10);
        }
        return startPosition + new Vector3(x*10 ,0 , z*10);
    }
    private void GenerateGrid(GameObject prefab, Transform tf)
    {
        
        if (prefab == territoryGridPrefab)
        {
            GameObject territory = GenerateTerritoryGrid(prefab, tf);
            GeneratedObjects.Add(territory);
            return;
        }
        if (prefab == villageGridPrefab)
        {
            village = Instantiate(prefab, generatePosition, Quaternion.identity, tf);
            villageCollider = village.GetComponent<Collider>();
            GeneratedObjects.Add(village);
            return;
        }
        if (prefab == enemyPortalPrefab)
        {
            GameObject portal = Instantiate(prefab, generatePosition, Quaternion.identity, tf);
            GeneratedObjects.Add(portal);
            EnemyWaveManager.Instance.spawnerPosition.Add(portal.transform.position);
        }
        GameObject obj = Instantiate(prefab, generatePosition, Quaternion.identity, tf);
        GeneratedObjects.Add(obj);
    }
    #endregion
    
    #region Territory functions
    private GameObject GenerateTerritoryGrid(GameObject prefab, Transform tf)
    {
        territoryID = gridValue % 100;
        territoryType = gridValue / 100;
        GameObject territoryGrid = Instantiate(prefab, generatePosition, Quaternion.identity, tf);
        
        TerritoryGrid grid = territoryGrid.GetComponent<TerritoryGrid>();
        
        grid.territoryID = territoryID;
        SetTerritoryState(grid);
        SetTerritoryColour(grid);
        AddToTerritoryGridList(grid);
        return territoryGrid;
    }
    
    
    private void SetTerritoryState(TerritoryGrid grid)
    {
        grid.state = territoryType == 1 ? TerritoryState.Unlocked : TerritoryState.Locked;
    }
    private void SetTerritoryColour(TerritoryGrid grid)
    {
        grid.GetComponent<MeshRenderer>().material = grid.territoryMaterials[territoryType - 1];//change colour
    }
    private void AddToTerritoryGridList(TerritoryGrid grid)
    {
        if (!gridDictionary.ContainsKey(grid.territoryID))
        {
            gridDictionary[territoryID] = new List<TerritoryGrid>();
        }
        gridDictionary[territoryID].Add(grid);
    }
    public bool CheckBarrackAvailability()//Component_Move_Minion
    {
        
        return BarrackNotFullList.Count > 0;
    }
    public int NumberOfMinionRequired()//Component_Spawner_Village
    {
        int numberOfMinions = 0;
        foreach (BarrackBase barrack in BarrackNotFullList)
        {
            numberOfMinions += barrack._spawnerComponent.NumberOfMinionsNeeded();
        }
        return numberOfMinions;
    }
    
    #endregion
    
    #region Set surround base points functions (for enemy movement)
    private void FindSurroundBasePoints()
    {
        List<Vector3> points = new List<Vector3>();
        for (float x = village.transform.position.x - village.transform.localScale.x / 2 - distance; x < village.transform.position.x + village.transform.localScale.x / 2 + distance; x += distance)
        {
            points.Add(new Vector3(x, village.transform.position.y, village.transform.position.z - village.transform.localScale.z/2 - distance));//cạnh dưới
            points.Add(new Vector3(x, village.transform.position.y, village.transform.position.z + village.transform.localScale.z/2 + distance));//cạnh trên
        }
        for (float z = village.transform.position.z - village.transform.localScale.z / 2 - distance; z < village.transform.position.z + village.transform.localScale.z / 2 + distance; z += distance)
        {
            points.Add(new Vector3(village.transform.position.x - village.transform.localScale.x/2 - distance, village.transform.position.y, z));//cạnh trái
            points.Add(new Vector3(village.transform.position.x + village.transform.localScale.x/2 + distance, village.transform.position.y, z));//cạnh phải
        }
        foreach (Vector3 point in points)
        {
            if (NavMesh.SamplePosition(point, out NavMeshHit hit, Mathf.Infinity, NavMesh.AllAreas))
            {
                surroundBasePoints.TryAdd(hit.position, false);
            }
        }
    }
    #endregion

    
   
    
    // #region Octree Implementation
    // [HideInInspector] public List<GameObject> objects = new List<GameObject>();//Các object đưa vào Octree
    // private float minNodeSize = 1f;//Kích thước của 1 node nhỏ nhất trong octree
    // private Octree octree;//Octree chính
    //
    // public readonly Graph wayPoints = new Graph();
    //
    // void OnDrawGizmos()
    // {
    //     if (octree == null)//Chỉ vẽ khi đã có octree
    //         return;
    //     //Gizmos.color = Color.yellow;
    //     //Gizmos.DrawWireCube(octree.bounds.center, octree.bounds.size);//Vẽ vùng bao ngoài cùng
    //     //octree.root.DrawNode();//Vẽ các node bên trong
    //     octree.graph.DrawaGraph();
    // }
    //
    // #endregion
}
