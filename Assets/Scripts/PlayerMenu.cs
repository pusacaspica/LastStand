using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMenu : Tank
{

    public GameObject TankTip, Projectile;
    public Transform Turnable;
    public float RotateVelocity;

    // Start is called before the first frame update
    void Start()
    {
        shellIsLive = false;
    }
    
    void Update(){
        if(Input.GetKeyDown(KeyCode.Space)&&!shellIsLive){
            shellIsLive = true;
            GameObject shell = Instantiate(Projectile, TankTip.transform.position, TankTip.transform.rotation);
            shell.GetComponent<ProjectileBehaviour>().TravelSpeed = 10.0f;
            shell.GetComponent<ProjectileBehaviour>().Owner = this.gameObject;
        }
    }
    void FixedUpdate() {
        if(Input.GetKey(KeyCode.D)){
            Turnable.Rotate(0.0f, RotateVelocity * 1.0f * Time.deltaTime * 10.0f, 0.0f, Space.Self);
        }
        if(Input.GetKey(KeyCode.A)){
            Turnable.Rotate(0.0f, RotateVelocity * -1.0f * Time.deltaTime * 10.0f, 0.0f, Space.Self);
        }
    }
}
