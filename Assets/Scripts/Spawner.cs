using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Camera mainCamera;
    public Plane[] planes;
    public bool isVisible;
    public GameObject spawnedObject;

    public Bounds boundary;
    // Start is called before the first frame update
    void Start() {
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
        Instantiate(spawnedObject, this.GetComponent<Collider>().transform.position, Quaternion.Euler(0.0f, this.transform.rotation.eulerAngles.y + 90.0f, 0.0f));
    }
}
