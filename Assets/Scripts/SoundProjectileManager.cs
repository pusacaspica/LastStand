using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundProjectileManager : MonoBehaviour
{
    public AudioSource _source;
    public AudioClip[] _impacts;
    public int clipIndex = 0;

    public enum Target{
        Ground,
        Enemy,
        Invulnerability,
        Vulnerability
    }
}
