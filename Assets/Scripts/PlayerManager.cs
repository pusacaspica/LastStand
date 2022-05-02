using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Tank
{

    public GameObject TankTip, Projectile;
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
            this.transform.Rotate(0.0f, RotateVelocity * 1.0f * Time.deltaTime * 10.0f, 0.0f, Space.Self);
        }
        if(Input.GetKey(KeyCode.A)){
            this.transform.Rotate(0.0f, RotateVelocity * -1.0f * Time.deltaTime * 10.0f, 0.0f, Space.Self);
        }
    }

    void OnCollisionEnter(Collision collision){
        if(collision.collider == vulnerability){
            maxLives--;
            Destroy(collision.collider);
            if(maxLives == 0) GameOver();
        } else {
            Destroy(collision.collider);
        }
    }

    void GameOver(){
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
