using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverseer : MonoBehaviour
{

    public PlayerManager Player;
    public List<EnemyTank> Enemies;
    public List<Spawner> Spawners;
    public List<Collider> SpawnersColliders;
    public int cooldownTime; // in seconds
    public int maxConcurrentEnemies;
    public bool onCooldown;

    // Start is called before the first frame update
    void Start() {
        onCooldown = false;
        Player = GameObject.FindObjectOfType<PlayerManager>();
        Spawners = GameObject.FindObjectsOfType<Spawner>().ToList<Spawner>();
        foreach(Spawner spawnPoint in Spawners){
            SpawnersColliders.Add(spawnPoint.gameObject.GetComponent<Collider>());
        }
    }

    Spawner ElectSpawner(){
        int spawnerIndex = Random.Range(0, Spawners.Count-1);
        if(!Spawners[spawnerIndex].isVisible && !Spawners[spawnerIndex].loaded) return Spawners[spawnerIndex];
        else return null;
    }

    // Update is called once per frame

    void FixedUpdate() {
        Enemies = GameObject.FindObjectsOfType<EnemyTank>().ToList<EnemyTank>();

        if(Enemies.Count == maxConcurrentEnemies && !onCooldown) StopCoroutines();
        
        if(Enemies.Count < maxConcurrentEnemies && !onCooldown){
            StartCoroutine(SpawnerActivation());
        }
    }
    void StopCoroutines(){
        StopAllCoroutines();
    }

    IEnumerator SpawnerActivation(){
        Spawner elected = ElectSpawner();
        while(elected == null) elected = ElectSpawner();
        elected.Spawn();
        onCooldown = true;
        yield return new WaitForSecondsRealtime(cooldownTime);
        onCooldown = false;
    }
}
