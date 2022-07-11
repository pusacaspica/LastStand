using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using OVRTouchSample;

[Serializable]
public class PlayerManager : Tank
{
    public Transform ForwardDirection;
    public GameOverseer overseer;
    public GameObject TankTip, Projectile, turnable;
    public Collider vulnerability;
    public ParticleSystem smoke;
    public AudioSource source;
    public float RotateVelocity;
    public int currentLives, score, highscore; 
    private float deltaY, lastY;
    private string hiscore, ppath, _highscore;
    private PlayerManager json;

    void Start(){
        highscoreEntry = new HighscoreEntry();
        hiscore = "/highscore.bin";
        ppath = Application.persistentDataPath + hiscore;
        /*if(File.Exists(ppath)){
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(ppath, FileMode.Open);
            highscoreEntry = bf.Deserialize(fs) as HighscoreEntry;
            if(highscoreEntry != null) highscore = highscoreEntry.highscore;
            Debug.Log("Read highscore, it's "+highscore.ToString());
            fs.Close();
        } else {
            highscore = 0;
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(ppath, FileMode.Create);
            highscoreEntry.highscore = highscore;
            bf.Serialize(fs, highscoreEntry);
            fs.Close();
        }*/
        lastY = 0.0f;
        score = 0;
        shellIsLive = false;
        currentLives = maxLives;
    }

    void Update(){
        if( (Input.GetKeyDown(KeyCode.Space)||
            OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger)!=0)||
            OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger)!=0
            &&!shellIsLive){
            source.Play();
            smoke.Emit(1250);
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

        if(currentLives == 0 && SceneManager.GetActiveScene().name == "Game"){
            GameOver();
        }
    }

    public void takeHit(){
        Debug.Log("got him!");
        currentLives--;
    }

    public void GameOver(){
        if (score > highscore){
            /*BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(ppath, FileMode.OpenOrCreate);
            highscoreEntry.highscore = score;
            bf.Serialize(fs, highscoreEntry.highscore);
            fs.Close();*/
            SceneManager.LoadScene("NewLeaderboard");
        }
        else{
            Debug.Log("git gut with your puny "+score.ToString());
            SceneManager.LoadScene("MainMenu");
        }
    }
}
