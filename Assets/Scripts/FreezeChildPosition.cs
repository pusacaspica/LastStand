using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeChildPosition : MonoBehaviour
{
    public Transform parent;
    public float targetY, targetZ;

    private float offsetY, offsetZ;

    // Start is called before the first frame update
    void Start()
    {
        parent = this.GetComponentInParent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        offsetY = parent.position.y - targetY;
        offsetZ = parent.position.z - targetZ;
        this.transform.position += new Vector3(0.0f, offsetZ, offsetZ);
    }
}
