using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RFR_SingleEye : MonoBehaviour
{
    public RenderTexture TexturePass0;
    RenderTexture TexturePass1 = null;
    RenderTexture TexturePass2;
    RenderTexture TextureDenoise;
    public Material Pass1Material;
    public Material Pass2Material;
    public Material DenoiseMaterial;

    public float sigma = 1.8f;
    [Range(0.01f, 0.99f)]
    public float fx;
    [Range(0.01f, 0.99f)]
    public float fy;
    [Range(0.01f, 0.99f)]
    public float eyeX;
    [Range(0.01f, 0.99f)]
    public float eyeY;
    int iApplyRFRMap1;
    int iApplyRFRMap2;

    public string savePath;

    public float sigma0;

    bool b_save = false;

    // Start is called before the first frame update
    void Start()
    {
        sigma0 = sigma;
        iApplyRFRMap1 = 1;
        iApplyRFRMap2 = 1;

        TexturePass1 = new RenderTexture(Mathf.RoundToInt((Screen.width/2) / sigma), Mathf.RoundToInt(Screen.height / sigma), 24, RenderTextureFormat.Default);
        TexturePass1.Create();

        TexturePass2 = new RenderTexture(Mathf.RoundToInt((Screen.width/2)), Mathf.RoundToInt(Screen.height), 24, RenderTextureFormat.Default);
        TexturePass2.Create();

        TextureDenoise = new RenderTexture(Mathf.RoundToInt((Screen.width/2)), Mathf.RoundToInt(Screen.height), 24, RenderTextureFormat.Default);
        TextureDenoise.Create();
    }

    // Update is called once per frame
    void Update()
    {
        keyControl();
        saveImages();
        if (sigma0 != sigma)
        {
            updateTextureSize();
            sigma = sigma0;
        }
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        Pass1MainL();
        Pass2MainL();
        Pass3DenoiseL();
        Graphics.Blit(TextureDenoise, dst);
    }

    void Pass1MainL()
    {
        Pass1Material.SetFloat("_eyeX", eyeX);
        Pass1Material.SetFloat("_eyeY", eyeY);
        Pass1Material.SetFloat("_scaleRatio", sigma);
        Pass1Material.SetFloat("_fx", fx);
        Pass1Material.SetFloat("_fy", fy);
        Pass1Material.SetInt("_iApplyRFRMap1", iApplyRFRMap1);
        Graphics.Blit(TexturePass0, TexturePass1, Pass1Material);
    }
    void Pass2MainL()
    {
        Pass2Material.SetFloat("_eyeX", eyeX);
        Pass2Material.SetFloat("_eyeY", eyeY);
        Pass2Material.SetFloat("_scaleRatio", sigma);
        Pass2Material.SetFloat("_fx", fx);
        Pass2Material.SetFloat("_fy", fy);
        Pass2Material.SetInt("_iApplyRFRMap2", iApplyRFRMap2);
        Pass2Material.SetTexture("_MidTex", TexturePass1);
        Graphics.Blit(null, TexturePass2, Pass2Material);
    }

    void Pass3DenoiseL()
    {
        DenoiseMaterial.SetFloat("_iResolutionX", Screen.width);
        DenoiseMaterial.SetFloat("_iResolutionY", Screen.height);
        DenoiseMaterial.SetFloat("_eyeX", eyeX);
        DenoiseMaterial.SetFloat("_eyeY", eyeY);
        DenoiseMaterial.SetTexture("_Pass2Tex", TexturePass2);
        Graphics.Blit(null, TextureDenoise, DenoiseMaterial);
    }

    void keyControl()
    {
        if (Input.GetKey(KeyCode.Keypad7) || Input.GetKey(KeyCode.O))
            sigma0 = sigma0 <= 0.05f ? 0.05f : sigma0 - 0.05f; //originally, capped at 1.2
        if (Input.GetKey(KeyCode.Keypad9) || Input.GetKey(KeyCode.U))
            sigma0 = sigma0 >= 5.0f ? 5.0f : sigma0 + 0.05f; // capped at 2.8
        
        if (Input.GetKey(KeyCode.Keypad4) || Input.GetKey(KeyCode.J))
            eyeX = eyeX <= 0.05f ? 0.05f : eyeX - 0.05f;
        if (Input.GetKey(KeyCode.Keypad6) || Input.GetKey(KeyCode.L))
            eyeX = eyeX >= 0.95f ? 0.95f : eyeX + 0.05f;

        if (Input.GetKey(KeyCode.Keypad8) || Input.GetKey(KeyCode.I))
            eyeY = eyeY >= 0.95f ? 0.95f : eyeY + 0.05f;
        if (Input.GetKey(KeyCode.Keypad5) || Input.GetKey(KeyCode.K))
            eyeY = eyeY <= 0.05f ? 0.05f : eyeY - 0.05f;

        if (Input.GetKey(KeyCode.LeftArrow))
            fx = fx <= 0.05f ? 0.05f : fx - 0.05f; // capped at 0.1
        if (Input.GetKey(KeyCode.RightArrow))
            fx = fx >= 0.95f ? 1.0f : fx + 0.05f; // capped at 0.9

        if (Input.GetKey(KeyCode.UpArrow))
            fy = fy >= 0.95f ? 1.0f : fy + 0.05f; // capped at 0.9
        if (Input.GetKey(KeyCode.DownArrow))
            fy = fy <= 0.05f ? 0.05f : fy - 0.05f; // capped at 0.1

        if (Input.GetKeyDown(KeyCode.F9))
            b_save = !b_save;
    }

    void updateTextureSize()
    {
        RenderTexture tempTexture = new RenderTexture(Mathf.RoundToInt((Screen.width/2) / sigma0), Mathf.RoundToInt(Screen.height / sigma0), 24, RenderTextureFormat.Default);
        tempTexture.Create();

        TexturePass1.Release();
        TexturePass1 = tempTexture;
    }

    void saveImages()
    {
        if (b_save)
        {
            Debug.Log("eyeXL:" + eyeX.ToString() + "\teyeYL:" + eyeY.ToString() + 
            "sigma:" + sigma + "\n" + "fx:" + fx.ToString() + "\tfy:" + fy.ToString());

            SaveToFile(TexturePass0, savePath + "/original_left_n.png");

            SaveToFile(TexturePass1, savePath +"/sigma_" +
               sigma.ToString() + "_fx_" + fx.ToString() + "_fy_" + fy.ToString() +
               "_eX_" + eyeX.ToString() + "_eY_" + eyeY.ToString() + "_p1.png");

            SaveToFile(TexturePass2, savePath + "/sigma_" +
               sigma.ToString() + "_fx_" + fx.ToString() + "_fy_" + fy.ToString() +
               "_eX_" + eyeX.ToString() + "_eY_" + eyeY.ToString() + "_p2.png");

            SaveToFile(TextureDenoise, savePath + "/sigma_" +
               sigma.ToString() + "_fx_" + fx.ToString() + "_fy_" + fy.ToString() +
               "_eX_" + eyeX.ToString() + "_eY_" + eyeY.ToString() + "_dn.png");

            b_save = false;
        }

    }

    public void SaveToFile(RenderTexture renderTexture, string name)
    {
        RenderTexture currentActiveRT = RenderTexture.active;
        RenderTexture.active = renderTexture;
        Texture2D tex = new Texture2D(renderTexture.width, renderTexture.height);
        tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
        var bytes = tex.EncodeToPNG();
        System.IO.File.WriteAllBytes(name, bytes);
        UnityEngine.Object.Destroy(tex);
        RenderTexture.active = currentActiveRT;
    }

    void DispText(int idx, float variable, string name)
    {
        GUIStyle guiStyle = new GUIStyle();
        guiStyle.fontSize = 50;
        string text = string.Format(name + " = {0}", variable);
        GUI.Label(new Rect(0, idx * 50, (Screen.width/2), Screen.height), text, guiStyle);
    }

    void OnGUI()
    {
        int idx = 0;
        DispText(idx++, sigma, "sigma");
        DispText(idx++, fx, "fx");
        DispText(idx++, fy, "fy");
        DispText(idx++, eyeX, "eyeX");
        DispText(idx++, eyeY, "eyeY");
    }
}
