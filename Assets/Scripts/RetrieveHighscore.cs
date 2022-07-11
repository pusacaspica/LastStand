using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using TMPro;

public class RetrieveHighscore : MonoBehaviour
{
    public string ppath, hiscore;
    public string highscore;
    public HighscoreEntry entry;
    public TMP_Text display;
    public PlayerManager playerManager;
    // Start is called before the first frame update
    void Start() {
        display.text = playerManager.highscore.ToString();
        /*ppath = Application.persistentDataPath+"/highscore.bin";
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = new FileStream(ppath, FileMode.Create);
        entry = bf.Deserialize(fs) as HighscoreEntry;
        highscore = entry.highscore.ToString();
        Debug.Log("Read highscore, it's "+highscore);
        fs.Close();
        display.text = highscore.ToString();*/
    }
}
