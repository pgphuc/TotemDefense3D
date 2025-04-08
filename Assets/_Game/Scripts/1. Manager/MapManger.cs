using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class MapManager : Singleton<MapManager>
{
    #region Global variables used by other classes
    public Dictionary<Vector3, bool> surroundBasePoints = new Dictionary<Vector3, bool>();//Enemy move target
    //Territory Management --> CanvasGameplay
    public static Dictionary<int, List<TerritoryGrid>> gridDictionary = new Dictionary<int, List<TerritoryGrid>>();
    public List<BarrackBase> BarrackList = new List<BarrackBase>();//Component_Check_Defender + Component_Move_Minion
    [HideInInspector] public GameObject village;//Component_Check_Totem + Component_Move_Enemy + Component_Move_Minion
    [HideInInspector] public Collider villageCollider;
    #endregion

    #region Prefab References
    [SerializeField] private GameObject groundPrefab;
    [SerializeField] private Transform groundParent;

    [SerializeField] private List<GameObject> wallPrefabsList; //Reworked
    [SerializeField] private Transform wallParent;

    [SerializeField] private GameObject enemyPortalPrefab;
    [SerializeField] private Transform enemyPortalParent;

    [SerializeField] private GameObject territoryGridPrefab;
    [SerializeField] public Material greenerTrueMaterial; //Reworked
    [SerializeField] public Material greenerFalseMaterial; //Reworked
    [SerializeField] private Transform territoryParent;

    [SerializeField] private GameObject villagePrefab;
    [SerializeField] private Transform villageParent;

    [SerializeField] private NavMeshSurface surface;
    #endregion

    #region Resources variables
    [SerializeField] private TextAsset mapText;
    #endregion

    #region Map Data, used to generate map
    private GameObject ground;
    private Vector3 startPosition;
    private int cellSize = 10;
    private float distance = 1f; //surroundBasePoint

    private Vector3 generatePosition; //FIXME
    private int gridValue; //FIXME
    private int territoryID;  //FIXME
    private int territoryType; //FIXME

    #endregion

    #region Start functions
    public void OnInit() //GameManager
    {
        GenerateGround();
        startPosition = FindStartPosition();
        GenerateObstacle();
        StartCoroutine(nameof(BakeNavMesh));
        FindSurroundBasePoints();
    }

    private IEnumerator BakeNavMesh()
    {
        yield return null;
        surface.BuildNavMesh();
    }
    #endregion

    #region restart functions
    private List<GameObject> GeneratedObjects = new List<GameObject>();

    public void OnDestroy() //GameManager
    {
        surroundBasePoints = new Dictionary<Vector3, bool>();
        gridDictionary = new Dictionary<int, List<TerritoryGrid>>();
        BarrackList = new List<BarrackBase>();
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
        return ground.transform.position - new Vector3(10 * ground.transform.localScale.x / 2 - cellSize / 2, -0.3f, 10 * ground.transform.localScale.z / 2 - cellSize / 2);
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
                generatePosition = FindGeneratePosition(column, row);
                switch (gridValue)
                {
                    case 0: //Do nothing
                        break;
                    case 1:
                        GenerateVillage();
                        break;
                    case 2:
                        GenerateEnemyPortal();
                        break;
                    case 3:
                        GenerateWall();
                        break;
                    default:
                        GenerateTerritoryGrid(column, row);
                        break;
                }
            }
        }
    }

    private Vector3 FindGeneratePosition(int x, int z)
    {
        Vector3 pos = startPosition + new Vector3(x * 10, 0, z * 10);
        if (gridValue == 2)
            pos += new Vector3(0, 3f, 0); // enemy portal
        return pos;
    }

    private void GenerateVillage()
    {
        village = Instantiate(villagePrefab, generatePosition, Quaternion.identity, villageParent);
        villageCollider = village.GetComponent<Collider>();
        GeneratedObjects.Add(village);
    }

    private void GenerateEnemyPortal()
    {
        GameObject portal = Instantiate(enemyPortalPrefab, generatePosition, Quaternion.identity, enemyPortalParent);
        GeneratedObjects.Add(portal);
        EnemyWaveManager.Instance.spawnerPosition.Add(portal.transform.position);
    }
    private void GenerateWall()
    {
        GameObject obj = Instantiate(RandomWallPrefab(), generatePosition, RandomWallRotation(), wallParent);
        GeneratedObjects.Add(obj);
    }
    private void GenerateTerritoryGrid(int column, int row)
    {
        territoryID = gridValue % 100;
        territoryType = gridValue / 100;
        GameObject territoryGrid = Instantiate(territoryGridPrefab, generatePosition, Quaternion.identity, territoryParent);
        GeneratedObjects.Add(territoryGrid);

        TerritoryGrid grid = territoryGrid.GetComponent<TerritoryGrid>();
        grid.territoryID = territoryID;
        grid.isGreener = column % 2 == row % 2; //FIXME

        SetTerritoryState(grid);
        SetGridColour(grid);
        AddToTerritoryGridList(grid);
    }
    #endregion

    #region Generation Set-up functions
    private void SetTerritoryState(TerritoryGrid grid)
    {
        grid.state = territoryType == 1 ? TerritoryState.Unlocked : TerritoryState.Locked;
    }
    private void SetGridColour(TerritoryGrid grid)
    {
        Color gridColor;
        if (grid.isGreener)
        {
            gridColor = greenerTrueMaterial.color;
            grid.territoryMaterials = greenerTrueMaterial;
        }
        else
        {
            gridColor = greenerFalseMaterial.color;
            grid.territoryMaterials = greenerFalseMaterial;
        }

        if (territoryType == 3) //Locked
            gridColor = ToGrayscale(gridColor);

        grid.gridRenderer.material.color = gridColor;
    }

    private Color ToGrayscale(Color color)
    {
        float gray = color.r * 0.299f + color.g * 0.587f + color.b * 0.114f;
        return new Color(gray, gray, gray, color.a);
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
        return BarrackList.Count > 0;
    }
    public int NumberOfMinionRequired()//Component_Spawner_Village
    {
        int numberOfMinions = 0;
        foreach (BarrackBase barrack in BarrackList)
        {
            numberOfMinions += barrack._spawnerComponent.NumberOfMinionsNeeded();
        }
        return numberOfMinions;
    }
    //Random Wall Prefab & Rotation
    private GameObject RandomWallPrefab()
    {
        int randomIndex = Random.Range(0, wallPrefabsList.Count);
        GameObject randomPrefab = wallPrefabsList[randomIndex];
        return randomPrefab;
    }
    private Quaternion RandomWallRotation()
    {
        int[] angles = { 0, 90, 180, 270 };
        int randomIndex = Random.Range(0, angles.Length);
        return Quaternion.Euler(0, angles[randomIndex], 0);
    }
    #endregion

    #region Set surround base points functions (for enemy movement)
    private void FindSurroundBasePoints()
    {
        List<Vector3> points = new List<Vector3>();
        for (float x = village.transform.position.x - village.transform.localScale.x / 2 - distance; x < village.transform.position.x + village.transform.localScale.x / 2 + distance; x += distance)
        {
            points.Add(new Vector3(x, village.transform.position.y, village.transform.position.z - village.transform.localScale.z / 2 - distance));//below
            points.Add(new Vector3(x, village.transform.position.y, village.transform.position.z + village.transform.localScale.z / 2 + distance));//above
        }
        for (float z = village.transform.position.z - village.transform.localScale.z / 2 - distance; z < village.transform.position.z + village.transform.localScale.z / 2 + distance; z += distance)
        {
            points.Add(new Vector3(village.transform.position.x - village.transform.localScale.x / 2 - distance, village.transform.position.y, z));//left
            points.Add(new Vector3(village.transform.position.x + village.transform.localScale.x / 2 + distance, village.transform.position.y, z));//right
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
    
}
