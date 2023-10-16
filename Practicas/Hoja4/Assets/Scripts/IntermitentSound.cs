
// '1' empieza la reproducción del sonido intermitente
// '2' lo para

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IntermitentSound : MonoBehaviour {
    [SerializeField]
    uint trafficNumber, chatterNumber;
    private AudioSource[] _Speakers;  // audio source asosicada a la entidad
    AudioSource trafficLoopSource, chatterLoopSource;
    [Range(0f, 1f )]
    public float minVol, maxVol, SourceVol;  // volumenes máximo y mínimo establecidos y volumen origintal del source
    [Range(0f, 30f )]
    public float minTime, maxTime;  // intervalo temporal de lanzamiento
    [Range(0, 50)]
    public int distRand, maxDist;   // 
    [Range(0f, 1.1f )]
    public float spatialBlend;
    public AudioClip trafficLoop;
    public AudioClip[] trafficData;
    public AudioClip chatterLoop;
    public AudioClip[] chatterData;
    public bool enablePlayMode;

    [SerializeField, Range(0,1)]
    float Itraffic;
    [SerializeField, Range(0, 1)]
    float Ichatter;

    int count = 0;

    void Awake(){
        _Speakers = new AudioSource[trafficNumber + chatterNumber];

        for (int i = 0; i < trafficNumber + chatterNumber; i++)
        {
            _Speakers[i] = new GameObject("Altavoz " + i).AddComponent<AudioSource>();
        }

        trafficLoopSource = new GameObject("Traffic Loop").AddComponent<AudioSource>();
        chatterLoopSource = new GameObject("Chatter Loop").AddComponent<AudioSource>();
    }

    void Start() {
        for (int i = 0; i < trafficNumber + chatterNumber; i++)
        {
            _Speakers[i].playOnAwake = false;
            _Speakers[i].loop = false;
            _Speakers[i].volume = 0.1f;

            StartCoroutine(i < trafficNumber ? "WaitforitTraffic" : "WaitforitChatter", i);
        }

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

    // Update is called once per frame
    void Update() {
        if (!enablePlayMode)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) && count < trafficNumber + chatterNumber)
            {
                enablePlayMode = true;
            }
        }
        else if (enablePlayMode)
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                StopSound();
                count = 0;
            }
    }



    IEnumerator WaitforitTraffic(int audioSourceId) {
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
        if (enablePlayMode) PlaySoundTraffic(audioSourceId);      
    }

    IEnumerator WaitforitChatter(int audioSourceId)
    {
        // tiempo de espera aleatorio en el intervalo [minTime,maxTime]
        float waitTime = Random.Range(minTime, maxTime);

        // miramos si hay un clip asignado al source (sirve para la primera vez q se ejecuta)
        if (_Speakers[audioSourceId].clip == null)
            // waitfor seconds suspende la coroutine durante waitTime
            yield return new WaitForSeconds(waitTime);

        // cuando hay clip se añade la long del clip + el tiempo de espera para esperar entre lanzamientos
        else
            yield return new WaitForSeconds(_Speakers[audioSourceId].clip.length + waitTime);

        // si esta activado reproducimos sonido
        if (enablePlayMode) PlaySoundChatter(audioSourceId);
    }

    void PlaySoundTraffic(int audioSourceId) {
        Debug.Log("Play");
        AudioClip randomClip = trafficData[Random.Range(0, trafficData.Length)];
        SetSourceProperties(_Speakers[audioSourceId], randomClip, minVol, maxVol, distRand, maxDist, spatialBlend);
        _Speakers[audioSourceId].Play();
        StartCoroutine("WaitforitTraffic", audioSourceId);
    }

    void PlaySoundChatter(int audioSourceId)
    {
        AudioClip randomClip = chatterData[Random.Range(0, chatterData.Length)];
        SetSourceProperties(_Speakers[audioSourceId], randomClip, minVol, maxVol, distRand, maxDist, spatialBlend);
        _Speakers[audioSourceId].Play();
        StartCoroutine("WaitforitChatter", audioSourceId);
    }



    public void SetSourceProperties(AudioSource speaker, AudioClip audioData, float minVol, float maxVol,
                                    int minDist, int maxDist, float SpatialBlend) {
        speaker.loop = false;
        speaker.maxDistance = maxDist - Random.Range(0f, distRand);
        speaker.spatialBlend = spatialBlend;
        speaker.clip = audioData;
        speaker.volume = SourceVol + Random.Range(minVol, maxVol);
    }




    void StopSound() {
        enablePlayMode = false;
        Debug.Log("stop");
    }
}    
    
