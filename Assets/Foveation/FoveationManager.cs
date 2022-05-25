using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoveationManager : MonoBehaviour
{
    public bool isItOn;
    public Rigidbody car;
    public CarControls carControls;
    public RFR_SingleEye leftEye, rightEye;
    public Material leftEyeMaterial, rightEyeMaterial;

    // although the name shown is deltaSigma and maxSigma, one should aim at changing sigma0 on both eyes
    // I mean, if your goal is to change sigma. You can change whatever you want, I'm not your parent.
    public float deltaSigma, maxSigma, 
                deltaFx, maxFx, 
                deltaFy, maxFy, 
                deltaEyeX, maxEyeX, 
                deltaEyeY, maxEyeY, 
                maxRadius, deltaRadius;

    // the "vanilla" foveation factors' values are meant to be default values
    // although they are used sometimes as minimal values, THIS IS NOT THEIR MAIN FUNCTION
    private float lastDeltaSigma, sigma,
        lastDeltaFx, fx, 
        lastDeltaFy, fy, 
        lastDeltaEyeX, eyex, 
        lastDeltaEyeY, eyey,
        lastDeltaRadius, radius3, radius5, radius11;

    private Material leftEyeDenoiser, rightEyeDenoiser;
    // Start is called before the first frame update
    void Start()
    {
        carControls = car.GetComponent<CarControls>();
        leftEyeMaterial = leftEye.DenoiseMaterial;
        rightEyeMaterial = rightEye.DenoiseMaterial;

        isItOn = false;
        sigma = leftEye.sigma;
        fx = leftEye.fx;
        fy = leftEye.fy;
        eyex = leftEye.eyeX;
        eyey = leftEye.eyeY;

        lastDeltaSigma = deltaSigma;
        lastDeltaFx = deltaFx;
        lastDeltaFy = deltaFy;
        lastDeltaEyeX = deltaEyeX;
        lastDeltaEyeY = deltaEyeY;

        radius3 = leftEyeMaterial.GetFloat("_denoiseRadius3");
        radius5 = leftEyeMaterial.GetFloat("_denoiseRadius5");
        radius11 = leftEyeMaterial.GetFloat("_denoiseRadius11");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float leftRadius3 = leftEyeMaterial.GetFloat("_denoiseRadius3");
        float leftRadius5 = leftEyeMaterial.GetFloat("_denoiseRadius5");
        float leftRadius11 = leftEyeMaterial.GetFloat("_denoiseRadius11");

        deltaSigma = (carControls.AccelerationMagnitude > 0.0f ? 1 : -1) * car.velocity.magnitude/100;
        deltaFx = (carControls.AccelerationMagnitude > 0.0f ? 1 : -1) * car.velocity.magnitude/100;
        deltaFy = (carControls.AccelerationMagnitude > 0.0f ? 1 : -1) * car.velocity.magnitude/100;
        deltaRadius = (carControls.AccelerationMagnitude > 0.0f ? 1 : -1) * car.velocity.magnitude/100;
        //if(deltaSigma == 0.0f) deltaSigma = defaultDeltaSigma;
        //if(deltaFx == 0.0f) deltaFx = defaultDeltaFx;
        //if(deltaFy == 0.0f) deltaFy = defaultDeltaFy;
        //if(deltaRadius == 0.0f) deltaRadius = defaultDeltaRadius;

        if(Input.GetKeyDown(KeyCode.Tab)){
            isItOn = !isItOn;
            if(!isItOn){
                leftEye.sigma0 = sigma;
                rightEye.sigma0 = sigma;

                leftEye.fx = fx;
                rightEye.fx = fx;

                leftEye.fy = fy;
                rightEye.fy = fy;

                leftEye.DenoiseMaterial.SetFloat("_denoiseRadius3", radius3);
                leftEye.DenoiseMaterial.SetFloat("_denoiseRadius5", radius5);
                leftEye.DenoiseMaterial.SetFloat("_denoiseRadius11", radius11);

                rightEye.DenoiseMaterial.SetFloat("_denoiseRadius3", radius3);
                rightEye.DenoiseMaterial.SetFloat("_denoiseRadius5", radius5);
                rightEye.DenoiseMaterial.SetFloat("_denoiseRadius11", radius11);
            }
        }

        if(isItOn){
            // VELOCITY IS ZERO AND HAVEN'T RETURNED TO NORMAL? NEVER FEAR
            if(Mathf.Abs(car.velocity.magnitude) <= 0.0025f){
                if(leftEye.sigma0 > sigma) deltaSigma = lastDeltaSigma*10;
                if(leftEye.fx < fx) deltaFx = lastDeltaFx*10;
                if(leftEye.fy < fy) deltaFy = lastDeltaFy*10;
                if(leftEyeMaterial.GetFloat("_denoiseRadius3") < radius3 
                    || leftEyeMaterial.GetFloat("_denoiseRadius5") < radius5
                    || leftEyeMaterial.GetFloat("_denoiseRadius11") < radius11) deltaRadius = lastDeltaRadius*10;
            }

            leftEye.sigma0 += deltaSigma * Time.deltaTime;
            rightEye.sigma0 += deltaSigma * Time.deltaTime;

            leftEye.fx -= deltaFx * Time.deltaTime;
            rightEye.fx -= deltaFx * Time.deltaTime;

            leftEye.fy -= deltaFy * Time.deltaTime;
            rightEye.fy -= deltaFy * Time.deltaTime;

            leftEye.DenoiseMaterial.SetFloat("_denoiseRadius3", leftRadius3 - deltaRadius * Time.deltaTime);
            leftEye.DenoiseMaterial.SetFloat("_denoiseRadius5", leftRadius5 - deltaRadius * Time.deltaTime);
            leftEye.DenoiseMaterial.SetFloat("_denoiseRadius11", leftRadius11 - deltaRadius * Time.deltaTime);

            rightEye.DenoiseMaterial.SetFloat("_denoiseRadius3", leftRadius3 - deltaRadius * Time.deltaTime);
            rightEye.DenoiseMaterial.SetFloat("_denoiseRadius5", leftRadius5 - deltaRadius * Time.deltaTime);
            rightEye.DenoiseMaterial.SetFloat("_denoiseRadius11", leftRadius11 - deltaRadius * Time.deltaTime);

            
            if(leftEye.sigma0 > maxSigma) leftEye.sigma0 = maxSigma;
            if(rightEye.sigma0 > maxSigma) rightEye.sigma0 = maxSigma;

            if(leftEye.fx < maxFx) leftEye.fx = maxFx;
            if(rightEye.fx < maxFx) rightEye.fx = maxFx;

            if(leftEye.fy < maxFy) leftEye.fy = maxFy;
            if(rightEye.fy < maxFy) rightEye.fy = maxFy;

            if(leftEyeMaterial.GetFloat("_denoiseRadius3") < maxRadius /*&& car.velocity.magnitude > 0.0025f*/) leftEye.DenoiseMaterial.SetFloat("_denoiseRadius3", maxRadius);
            if(rightEyeMaterial.GetFloat("_denoiseRadius3") < maxRadius /*&& car.velocity.magnitude > 0.0025f*/) rightEye.DenoiseMaterial.SetFloat("_denoiseRadius3", maxRadius);
            
            if(leftEyeMaterial.GetFloat("_denoiseRadius5") < maxRadius*1.05 /*&& car.velocity.magnitude > 0.0025f*/) leftEye.DenoiseMaterial.SetFloat("_denoiseRadius5", maxRadius*1.05f);
            if(rightEyeMaterial.GetFloat("_denoiseRadius5") < maxRadius*1.05 /*&& car.velocity.magnitude > 0.0025f*/) rightEye.DenoiseMaterial.SetFloat("_denoiseRadius5", maxRadius*1.05f);

            if(leftEyeMaterial.GetFloat("_denoiseRadius11") < maxRadius*1.15 /*&& car.velocity.magnitude > 0.0025f*/) leftEye.DenoiseMaterial.SetFloat("_denoiseRadius11", maxRadius*1.15f);
            if(rightEyeMaterial.GetFloat("_denoiseRadius11") < maxRadius*1.15 /*&& car.velocity.magnitude > 0.0025f*/) rightEye.DenoiseMaterial.SetFloat("_denoiseRadius11", maxRadius*1.15f);

            // RETURN TO NORMAL
            if(leftEye.sigma0 < sigma) leftEye.sigma0 = sigma;
            if(rightEye.sigma0 < sigma) rightEye.sigma0 = sigma;

            if(leftEye.fx > fx) leftEye.fx = fx;
            if(rightEye.fx > fx) rightEye.fx = fx;

            if(leftEye.fy > fy) leftEye.fy = fy;
            if(rightEye.fy > fy) rightEye.fy = fy;

            if(leftEyeMaterial.GetFloat("_denoiseRadius3") > radius3) leftEye.DenoiseMaterial.SetFloat("_denoiseRadius3", radius3);
            if(rightEyeMaterial.GetFloat("_denoiseRadius3") > radius3) rightEye.DenoiseMaterial.SetFloat("_denoiseRadius3", radius3);

            if(leftEyeMaterial.GetFloat("_denoiseRadius5") > radius5) leftEye.DenoiseMaterial.SetFloat("_denoiseRadius5", radius5);
            if(rightEyeMaterial.GetFloat("_denoiseRadius5") > radius5) rightEye.DenoiseMaterial.SetFloat("_denoiseRadius5", radius5);

            if(leftEyeMaterial.GetFloat("_denoiseRadius11") > radius11) leftEye.DenoiseMaterial.SetFloat("_denoiseRadius11", radius11);
            if(rightEyeMaterial.GetFloat("_denoiseRadius11") > radius11) rightEye.DenoiseMaterial.SetFloat("_denoiseRadius11", radius11);

            // UPDATE LAST DELTAS IN CASE THE VALUES NEED TO BE CHANGED AFTER CAR HAVE STOPPED
            if(car.velocity.magnitude != 0.0f){
                lastDeltaSigma = (carControls.AccelerationMagnitude > 0.0f ? 1 : -1) *deltaSigma;
                lastDeltaFx = (carControls.AccelerationMagnitude > 0.0f ? 1 : -1) *deltaFx;
                lastDeltaFy = (carControls.AccelerationMagnitude > 0.0f ? 1 : -1) *deltaFy;
                lastDeltaEyeX = (carControls.AccelerationMagnitude > 0.0f ? 1 : -1) *deltaEyeX;
                lastDeltaEyeY = (carControls.AccelerationMagnitude > 0.0f ? 1 : -1) *deltaEyeY;
                lastDeltaRadius = (carControls.AccelerationMagnitude > 0.0f ? 1 : -1) *deltaRadius;
            }
        }
    }
}
