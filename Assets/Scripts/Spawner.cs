using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameOverseer overseer;
    public Camera mainCamera;
    public Plane[] planes;
    public bool isVisible, loaded;
    public GameObject spawnedObject;
    public Bounds boundary;
    public Vector3 spawnLocation;

    // Start is called before the first frame update
    void Start() {
        loaded = false;
        boundary = this.gameObject.GetComponentInChildren<SphereCollider>().bounds;
        mainCamera = Camera.main;
        planes = GeometryUtility.CalculateFrustumPlanes(mainCamera);
        isVisible = false;
    }

    // Update is called once per frame
    void Update() {
        planes = GeometryUtility.CalculateFrustumPlanes(mainCamera);
        if(GeometryUtility.TestPlanesAABB(planes, boundary)){
            isVisible = true;
        } else{
            isVisible = false;
        }
    }

    public void Spawn() {
        GameObject enemy;
        if(this.transform.rotation.eulerAngles.y == 0.0f || this.transform.rotation.eulerAngles.y == 180.0f){
            enemy = Instantiate(spawnedObject, spawnLocation, Quaternion.Euler(0.0f, 90.0f, 0.0f));
        } else {
            enemy = Instantiate(spawnedObject, spawnLocation, Quaternion.Euler(0.0f, this.transform.rotation.eulerAngles.y+90.0f, 0.0f));
        }
        loaded=true;
        enemy.GetComponent<EnemyTank>().spawner = this;
        enemy.GetComponent<EnemyTank>().overseer = overseer;
    }
}
