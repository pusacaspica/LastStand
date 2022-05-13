using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RFR_DebugControls : MonoBehaviour
{

    public RFR_SingleEye Left, Right;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1)){
            Left.enabled = !Left.enabled;
            Right.enabled = !Right.enabled;
        }
        if(Input.GetKeyDown(KeyCode.F2)){
            Left.enabled = !Left.enabled;
        }
        if(Input.GetKeyDown(KeyCode.F3)){
            Right.enabled = !Right.enabled;
        }
    }
}
