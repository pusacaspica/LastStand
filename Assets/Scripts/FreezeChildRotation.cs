using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeChildRotation : MonoBehaviour
{
    public Transform parent;

    // Start is called before the first frame update
    void Start()
    {
        parent = this.GetComponentInParent<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.Rotate(-parent.rotation.eulerAngles.x, 0.0f, -parent.rotation.eulerAngles.z, Space.Self);
    }
}
