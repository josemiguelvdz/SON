using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundLoop : MonoBehaviour
{
    public AudioClip chatterLoop;
    public AudioClip trafficLoop;
    AudioSource trafficLoopSource, chatterLoopSource;

    [Range(0f, 1f)]
    public float minVol, maxVol, SourceVol;  // volumenes máximo y mínimo establecidos y volumen origintal del source
    [Range(0f, 30f)]
    public float minTime, maxTime;  // intervalo temporal de lanzamiento
    [Range(0, 50)]
    public int distRand, maxDist;   // 
    [Range(0f, 1.1f)]
    public float spatialBlend;

    [SerializeField, Range(0, 1)]
    float Itraffic;
    [SerializeField, Range(0, 1)]
    float Ichatter;

    private void Awake()
    {
        trafficLoopSource = new GameObject("Traffic Loop").AddComponent<AudioSource>();
        chatterLoopSource = new GameObject("Chatter Loop").AddComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    { 
        trafficLoopSource.playOnAwake = true;
        trafficLoopSource.loop = true;
        trafficLoopSource.volume = Itraffic;
        trafficLoopSource.clip = trafficLoop;
        trafficLoopSource.Play();

        chatterLoopSource.playOnAwake = true;
        chatterLoopSource.loop = true;
        chatterLoopSource.volume = Ichatter;
        chatterLoopSource.clip = chatterLoop;
        chatterLoopSource.Play();
    }
}
