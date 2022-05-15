using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : Tank
{
    public GameOverseer overseer;
    public GameObject TankTip, Projectile, turnable;
    public Collider vulnerability;
    public float RotateVelocity;
    public int currentLives, score; 


    void Start(){
        score = 0;
        shellIsLive = false;
        currentLives = maxLives;
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.Space)&&!shellIsLive){
            shellIsLive = true;
            GameObject shell = Instantiate(Projectile, TankTip.transform.position, TankTip.transform.rotation);
            shell.GetComponent<ProjectileBehaviour>().TravelSpeed = 10.0f;
            shell.GetComponent<ProjectileBehaviour>().Owner = this.gameObject;
        }
    }

    // Update is called once per frame
    void FixedUpdate() {
        if(Input.GetKey(KeyCode.D)){
            turnable.transform.Rotate(0.0f, RotateVelocity * 1.0f * Time.deltaTime * 10.0f, 0.0f, Space.Self);
        }
        if(Input.GetKey(KeyCode.A)){
            turnable.transform.Rotate(0.0f, RotateVelocity * -1.0f * Time.deltaTime * 10.0f, 0.0f, Space.Self);
        }
    }

    public void takeHit(){
        currentLives--;
        if(currentLives == 0) Invoke("GameOver", overseer.gameOverDelay);
    }

    void GameOver(){
        SceneManager.LoadScene("MainMenu");
    }
}
