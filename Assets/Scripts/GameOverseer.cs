using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverseer : MonoBehaviour
{

    public PlayerManager Player;
    public List<EnemyTank> Enemies;
    public List<Spawner> Spawners;
    public int maxConcurrentEnemies;
    public bool waiting;

    // Start is called before the first frame update
    void Start()
    {
        waiting = false;
        Player = GameObject.FindObjectOfType<PlayerManager>();
    }

    Spawner ElectSpawner(){
        int spawnerIndex = Random.Range(0, 7);
        if(!Spawners[spawnerIndex].isVisible) return Spawners[spawnerIndex];
        else{
            while(true){
                if(!Spawners[spawnerIndex].isVisible) return Spawners[spawnerIndex];
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate() {
        Enemies = GameObject.FindObjectsOfType<EnemyTank>().ToList<EnemyTank>();
        Spawners = GameObject.FindObjectsOfType<Spawner>().ToList<Spawner>();

        if(Enemies.Count < maxConcurrentEnemies && waiting){
            StartCoroutine(SpawnerActivation());
        }
    }

    IEnumerator SpawnerActivation(){
        Spawner elected = ElectSpawner();
        elected.Spawn();
        waiting = true;
        yield return new WaitForSecondsRealtime(1);
        waiting = false;
    }
}
