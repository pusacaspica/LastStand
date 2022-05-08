using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    public float TravelSpeed;
    public GameObject Owner;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate() {
        this.transform.position += TravelSpeed * this.transform.up * Time.deltaTime;
        this.transform.Rotate(0.25f, 0.0f, 0.0f, Space.Self);

        if(this.transform.position.y <= 0.0f){
            Owner.GetComponent<Tank>().shellIsLive = false;
            if(Owner.GetComponent<EnemyTank>()){
                Owner.GetComponent<EnemyTank>().overseer.enemyHasFired = false;
            }
            Destroy(this.gameObject);
        }
    }
}
