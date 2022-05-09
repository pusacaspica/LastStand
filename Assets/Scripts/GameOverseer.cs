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
    public int cooldownTime, fireCooldownTime; // in seconds
    public int maxConcurrentEnemies;
    public bool onCooldown, enemyHasFired;

    // Start is called before the first frame update
    void Start() {
        enemyHasFired = false;
        onCooldown = false;
        Player = GameObject.FindObjectOfType<PlayerManager>();
        Spawners = GameObject.FindObjectsOfType<Spawner>().ToList<Spawner>();
        foreach(Spawner spawnPoint in Spawners){
            SpawnersColliders.Add(spawnPoint.gameObject.GetComponent<Collider>());
        }
    }

    Spawner SelectSpawner(){
        int spawnerIndex = Random.Range(0, Spawners.Count-1);
        if(!Spawners[spawnerIndex].isVisible && !Spawners[spawnerIndex].loaded) return Spawners[spawnerIndex];
        else return null;
    }

    EnemyTank SelectAttacker(){
        int enemyIndex = Random.Range(0, Enemies.Count-1);
        if(!Enemies[enemyIndex].shellIsLive && !enemyHasFired) return Enemies[enemyIndex];
        else return null;
    }

    // Update is called once per frame

    void FixedUpdate() {
        Enemies = GameObject.FindObjectsOfType<EnemyTank>().ToList<EnemyTank>();

        //if(Enemies.Count == maxConcurrentEnemies && !onCooldown) StopCoroutines();
        
        if(Enemies.Count < maxConcurrentEnemies && !onCooldown){
            StartCoroutine(SpawnEnemy());
        }

        if(Enemies.Count > 1 && !enemyHasFired){
            StartCoroutine(AttackPlayer());
        }
    }

    IEnumerator AttackPlayer(){
        EnemyTank attacker = SelectAttacker();
        if(attacker == null) yield return null;
        attacker.Fire();
        enemyHasFired = true;
        attacker.shellIsLive = true;
        yield return new WaitForSeconds(fireCooldownTime);
        enemyHasFired = false;
    }

    IEnumerator SpawnEnemy(){
        Spawner enemy = SelectSpawner();
        if(enemy == null) yield return null;
        enemy.Spawn();
        onCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        onCooldown = false;
    }
}
