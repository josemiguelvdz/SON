using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundIntermitent : MonoBehaviour
{
    private AudioSource[] _Speakers;

    [Range(0f, 1f)]
    public float minVol, maxVol, SourceVol;  // volumenes máximo y mínimo establecidos y volumen origintal del source
    [Range(0f, 30f)]
    public float minTime, maxTime;  // intervalo temporal de lanzamiento
    [Range(0, 50)]
    public int distRand, maxDist;   // 
    [Range(0f, 1.1f)]
    public float spatialBlend;

    public AudioClip[] soundClips;
    public bool enablePlayMode;

    void Awake()
    {
        _Speakers = new AudioSource[soundClips.Length];

        for (int i = 0; i < soundClips.Length ; i++)
        {
            _Speakers[i] = new GameObject("Altavoz Tren " + i).AddComponent<AudioSource>();
        }
    }

    void Start()
    {
        for (int i = 0; i < soundClips.Length; i++)
        {
            _Speakers[i].playOnAwake = false;
            _Speakers[i].loop = false;
            _Speakers[i].volume = 0.1f;

            StartCoroutine("WaitforitTrain", i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!enablePlayMode && Input.GetKeyDown(KeyCode.Alpha1))
            enablePlayMode = true;
        else if (enablePlayMode && Input.GetKeyDown(KeyCode.Alpha2))
             StopSound();
    }

    IEnumerator WaitforitTrain(int audioSourceId)
    {
        // tiempo de espera aleatorio en el intervalo [minTime,maxTime]
        float waitTime = Random.Range(minTime, maxTime);
        Debug.Log(audioSourceId);

        // miramos si hay un clip asignado al source (sirve para la primera vez q se ejecuta)
        if (_Speakers[audioSourceId].clip == null)
            // waitfor seconds suspende la coroutine durante waitTime
            yield return new WaitForSeconds(waitTime);

        // cuando hay clip se añade la long del clip + el tiempo de espera para esperar entre lanzamientos
        else
            yield return new WaitForSeconds(_Speakers[audioSourceId].clip.length + waitTime);

        // si esta activado reproducimos sonido
        if (enablePlayMode) PlaySoundTrain(audioSourceId);
    }

    void PlaySoundTrain(int audioSourceId)
    {
        Debug.Log("Play Train Audio");
        AudioClip randomClip = soundClips[Random.Range(0, soundClips.Length)];
        SetSourceProperties(_Speakers[audioSourceId], randomClip, minVol, maxVol, distRand, maxDist, spatialBlend);
        _Speakers[audioSourceId].Play();
        StartCoroutine("WaitforitTrain", audioSourceId);
    }

    public void SetSourceProperties(AudioSource speaker, AudioClip audioData, float minVol, float maxVol,
                                    int minDist, int maxDist, float SpatialBlend)
    {
        speaker.loop = false;
        speaker.maxDistance = maxDist - Random.Range(0f, distRand);
        speaker.spatialBlend = spatialBlend;
        speaker.clip = audioData;
        speaker.volume = SourceVol + Random.Range(minVol, maxVol);
    }

    void StopSound()
    {
        enablePlayMode = false;
        Debug.Log("stop train sounds");
    }
}
