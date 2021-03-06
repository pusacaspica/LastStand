using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Clockwatch : MonoBehaviour
{
    public Renderer renderer;
    public float elapsedTime;
    public GameOverseer overseer;
    public PlayerManager playerManager;
    public Material timerMaterial;

    void Start(){
        timerMaterial = renderer.materials[1];
        elapsedTime = 0.0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        elapsedTime += Time.deltaTime;
        timerMaterial.SetFloat("_Wheel_Slice", elapsedTime*(Mathf.PI*2)/overseer.gameTime);
        if(elapsedTime >= overseer.gameTime && SceneManager.GetActiveScene().name == "Game"){
            playerManager.GameOver();
        }
    }
}
