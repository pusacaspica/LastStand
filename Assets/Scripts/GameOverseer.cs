using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverseer : MonoBehaviour
{

    public PlayerManager Player;
    public List<EnemyTank> Enemies;
    public List<SpawnerManager> Spawners;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindObjectOfType<PlayerManager>();
    }

    // Update is called once per frame
    void FixedUpdate() {
        Enemies = GameObject.FindObjectsOfType<EnemyTank>().ToList<EnemyTank>();
    }
}
