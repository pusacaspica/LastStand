using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : Tank
{
    public Transform ForwardDirection;
    public GameOverseer overseer;
    public GameObject TankTip, Projectile, turnable;
    public Collider vulnerability;
    public float RotateVelocity;
    public int currentLives, score; 

    private float deltaY, lastY;


    void Start(){
        lastY = 0.0f;
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

        deltaY = turnable.transform.rotation.eulerAngles.y - lastY;
        lastY = turnable.transform.rotation.eulerAngles.y;

        if(Input.GetKey(KeyCode.D)){
            turnable.transform.Rotate(0.0f, RotateVelocity * 1.0f * Time.deltaTime * 10.0f, 0.0f, Space.Self);
        }
        if(Input.GetKey(KeyCode.A)){
            turnable.transform.Rotate(0.0f, RotateVelocity * -1.0f * Time.deltaTime * 10.0f, 0.0f, Space.Self);
        }

        //turnable.transform.eulerAngles = new Vector3(0.0f, -ForwardDirection.transform.eulerAngles.y + turnable.transform.eulerAngles.y, 0.0f);
        //turnable.transform.Rotate(0.0f, (ForwardDirection.transform.rotation.eulerAngles.y - turnable.transform.rotation.eulerAngles.y) * Time.deltaTime, 0.0f);

        if(currentLives == 0){
            GameOver();
        }
    }

    public void takeHit(){
        Debug.Log("got him!");
        currentLives--;
    }

    void GameOver(){
        /*string jsonString = PlayerPrefs.GetString("Leaderboard");
        Leaderboard leaderboard = JsonUtility.FromJson<Leaderboard>(jsonString);
        if(){

        }
        else */SceneManager.LoadScene("NewLeaderboard");
    }
}
