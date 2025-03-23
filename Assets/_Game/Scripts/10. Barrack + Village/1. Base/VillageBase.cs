using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class VillageBase : GameUnit
{
    public Check_Enemy_Village enemyCheck;
    public Component_Health healthComponent;
    public Component_Spawner_Village spawnerComponent;
    
    
    #region spawner variables
    public GameUnit _minionMeleePrefab;
    
    //reinforcement variables
    private Coroutine _reinforcementCoroutine;
    public int reinforcementCount;
    public int reinforcementSpawned;
    
    //defense variables
    public List<Collider> enemyInRange = new List<Collider>();
    public int defenseCount;
    public int defenseSpawned;
    public float cooldownTime;
    
    #endregion
    

    void Start()
    {
        OnInit();
    }
    public void Update()
    {
        reinforcementCount = MapManager.Instance.NumberOfMinionRequired();
        
        if (DoVillageNeedMinion())
        {
            cooldownTime -= Time.deltaTime;
            if (cooldownTime <= 0)
            {
                defenseCount++;
                cooldownTime = 5f;
            }
        }
        if (defenseCount >= enemyInRange.Count && enemyInRange.Count > defenseSpawned)
        {
            defenseSpawned++;
            spawnerComponent.DefendingBase(enemyInRange[^1]);
        }
        if (DoBarrackNeedReinforcement() && _reinforcementCoroutine == null)
        {
            _reinforcementCoroutine = StartCoroutine(nameof(Reinforcement));
        }
    }
    
    public override void StateMachineConstructor()
    {
        base.StateMachineConstructor();
    }

    public override void ComponentConstructor()
    {
        base.ComponentConstructor();
        spawnerComponent = new Component_Spawner_Village(this);
        components.Add(spawnerComponent);
        healthComponent = new Component_Health(this, transform, 100f);
        components.Add(healthComponent);
    }

    public override void OnInit()
    {
        base.OnInit();
        reinforcementCount = 0;
        reinforcementSpawned = 0;
        _reinforcementCoroutine = null;

        defenseCount = 0;
        defenseSpawned = 0;
        cooldownTime = 5f;
        enemyInRange.Clear();
        enemyCheck._owner = this;
    }

    public override void InitAllComponents()
    {
        base.InitAllComponents();
        spawnerComponent.OnInit();
        healthComponent.OnInit();
    }

    public override void OnDespawn()
    {
        Destroy(gameObject);
    }

    private bool DoBarrackNeedReinforcement()
    {
        return reinforcementSpawned < reinforcementCount;
    }
    private IEnumerator Reinforcement()
    {
        while (DoBarrackNeedReinforcement())
        {
            //đợi 5s rồi spawn
            yield return new WaitForSeconds(spawnerComponent._spawnTime);
            spawnerComponent.Reinforcement();
            reinforcementSpawned++;
        }
        //nếu spawn xong thấy đủ minion rồi thì stop
        _reinforcementCoroutine = null;
    }

    private bool DoVillageNeedMinion()
    {
        return defenseCount < 20;
    }
    
}
