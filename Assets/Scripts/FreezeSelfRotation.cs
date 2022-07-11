using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeSelfRotation : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(-this.transform.rotation.eulerAngles.x, 0f, -this.transform.rotation.eulerAngles.z, Space.Self);
    }
}
