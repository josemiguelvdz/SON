using System.Collections;
using UnityEngine;

public class SoundIntermitent : MonoBehaviour
{
    private AudioSource[] speakers;

    [Min(1)]
    public int numSpeakers = 1;

    public string soundCategory = "";

    [Range(0f, 1f)]
    public float minVol, maxVol;  // volumenes máximo y mínimo establecidos y volumen origintal del source
	[Range(0f, 1f)]
	public float volumeLimit;
	[Range(0f, 30f)]
    public float minTime, maxTime;  // intervalo temporal de lanzamiento
    [Range(0, 50)]
    public int distRand, maxDist;
    [Range(0f, 1.1f)]
    public float spatialBlend;
	[Range(0f, .5f)]
	public float pitchVariation;
    public bool randomPosition = false;
    [Range(0f, 50f)]
    public float maxPositionVariation;

	public AudioClip[] soundClips;
    public bool enablePlayMode;

    void Awake()
    {
        speakers = new AudioSource[numSpeakers];

        for (int i = 0; i < numSpeakers; i++)
        {
            speakers[i] = new GameObject("Altavoz " + soundCategory + " " + i).AddComponent<AudioSource>();
        }
    }

    void Start()
    {
        for (int i = 0; i < numSpeakers; i++)
        {
            speakers[i].playOnAwake = false;
            speakers[i].loop = false;
            speakers[i].volume = 0.1f;

            StartCoroutine("Waitforit", i);
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

    IEnumerator Waitforit(int audioSourceId)
    {
        // tiempo de espera aleatorio en el intervalo [minTime,maxTime]
        float waitTime = Random.Range(minTime, maxTime);
        Debug.Log(audioSourceId);

        // miramos si hay un clip asignado al source (sirve para la primera vez q se ejecuta)
        if (speakers[audioSourceId].clip == null)
            // waitfor seconds suspende la coroutine durante waitTime
            yield return new WaitForSeconds(waitTime);

        // cuando hay clip se añade la long del clip + el tiempo de espera para esperar entre lanzamientos
        else
            yield return new WaitForSeconds(speakers[audioSourceId].clip.length + waitTime);

        // si esta activado reproducimos sonido
        if (enablePlayMode) PlaySound(audioSourceId);
    }

    void PlaySound(int audioSourceId)
    {
        Debug.Log("Play Audio");
        AudioClip randomClip = soundClips[Random.Range(0, soundClips.Length)];
        SetSourceProperties(speakers[audioSourceId], randomClip);

        if (randomPosition) {
            speakers[audioSourceId].transform.position = new Vector3(Random.Range(-maxPositionVariation, maxPositionVariation),
				0,
				Random.Range(-maxPositionVariation, maxPositionVariation));
        }

        speakers[audioSourceId].Play();
        StartCoroutine("Waitforit", audioSourceId);
    }

    public void SetSourceProperties(AudioSource speaker, AudioClip audioData)
    {
        float volume = Random.Range(minVol, maxVol);

		if (volume < volumeLimit) {
			speaker.volume = 0;
            return;
        }

		speaker.volume = Mathf.InverseLerp(volumeLimit, maxVol, volume);
		speaker.loop = false;
        speaker.maxDistance = maxDist - Random.Range(0f, distRand);
        speaker.spatialBlend = spatialBlend;
        speaker.clip = audioData;
        speaker.pitch = 1 + Random.Range(-pitchVariation, pitchVariation);
    }

    void StopSound()
    {
        enablePlayMode = false;
        Debug.Log("stop sounds");
    }
}
