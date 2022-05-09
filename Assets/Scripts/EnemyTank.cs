using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTank : Tank
{
    public GameOverseer overseer;
    public GameObject tankTip, projectile;
    public Spawner spawner;
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void FixedUpdate() {
    }

    public void Fire(){
        GameObject shell = Instantiate(projectile, tankTip.transform.position, tankTip.transform.rotation);
        shell.GetComponent<ProjectileBehaviour>().TravelSpeed = 10.0f;
        shell.GetComponent<ProjectileBehaviour>().Owner = this.gameObject;
    }
}
