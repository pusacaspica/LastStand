using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMortality : MonoBehaviour
{
    public GameObject self;
    public Collider vulnerability;

    void Start(){
        //vulnerability = this.GetComponent<Collider>();
    }

    void OnTriggerEnter(Collider collision){
        ProjectileBehaviour projectile = collision.gameObject.GetComponent<ProjectileBehaviour>();
        if(projectile.Owner.name == "Player") {        
            projectile.Owner.GetComponent<Tank>().shellIsLive = false;
            projectile.Owner.GetComponent<PlayerManager>().score += 1;
            self.GetComponent<EnemyTank>().spawner.loaded = false;
            Destroy(collision.gameObject);
            Destroy(self.gameObject);
        }
    }
}
