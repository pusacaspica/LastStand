using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class GameOverseer : MonoBehaviour
{
    public PlayerManager Player;
    public TMP_Text scoreText, livesText;
    public List<EnemyTank> Enemies;
    public List<Spawner> Spawners;
    public List<Collider> SpawnersColliders;
    public int cooldownTime, fireCooldownTime, gameOverDelay, gameTime; // in seconds
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
        scoreText.text = Player.score.ToString();
        livesText.text = Player.currentLives.ToString();
        Enemies = GameObject.FindObjectsOfType<EnemyTank>().ToList<EnemyTank>();

        if(Enemies.Count < maxConcurrentEnemies && !onCooldown){
            try {
                StartCoroutine(SpawnEnemy());
            } catch (System.NullReferenceException){
                StopAllCoroutines();
            }
        }

        if(Enemies.Count > 1 && !enemyHasFired){
            try{
                StartCoroutine(AttackPlayer());
            } catch(System.NullReferenceException) {
                StopAllCoroutines();
            }
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
