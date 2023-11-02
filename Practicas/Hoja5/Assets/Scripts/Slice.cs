using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slice : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] _headData;
    [SerializeField]
    private AudioClip[] _tailData;
    [SerializeField]
    private AudioClip[] _casingData;
    [SerializeField]
    private AudioClip[] _loopData;

    [SerializeField]
    private AudioSource _headSpeaker;
    [SerializeField]
    private AudioSource _tailSpeaker;
    [SerializeField]
    private AudioSource _casingSpeaker;
    [SerializeField]
    private AudioSource _loopSpeaker;

    [SerializeField, Range(0f, .5f)]
    private float overlapTime;

    [SerializeField, Range(0f, .5f)]
    private float volumeVariation;

    [SerializeField, Range(0f, 1f)]
    private float pitchVariation;

    [SerializeField, Range(0f, 0.3f)]
    private float timeVariation;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            AudioClip headChosen = _headData[Random.Range(0, _headData.Length)];
            AudioClip tailChosen = _tailData[Random.Range(0, _tailData.Length)];
            AudioClip casingChosen = _casingData[Random.Range(0, _casingData.Length)];
            _headSpeaker.clip = headChosen;
            _tailSpeaker.clip = tailChosen;

            // duración de la primera muestra, teniendo en cuenta el pitch
            // medimos en samples para mejorar precisión
            double clipLength = (double)headChosen.samples / _headSpeaker.pitch;
            int sRATE = AudioSettings.outputSampleRate;

            float[] headData = new float[(int) (overlapTime * sRATE)];
            int headOffset = (int)(sRATE * (clipLength / sRATE - overlapTime));
            headChosen.GetData(headData, headOffset);

            for (int i= 0; i< headData.Length; i++)
            {
                headData[i] = Mathf.Sqrt((overlapTime - (float)i / sRATE) / overlapTime);
            }
            headChosen.SetData(headData, headOffset);

            float[] tailData = new float[(int)(overlapTime * sRATE)];
            int tailOffset = (int)(sRATE * (clipLength / sRATE - overlapTime));
            tailChosen.GetData(tailData, tailOffset);

            for (int i = 0; i < tailData.Length; i++)
            {
                tailData[i] = Mathf.Sqrt((float)i / sRATE / overlapTime);
            }
            tailChosen.SetData(tailData, tailOffset);


            _headSpeaker.Play();

            double shootLength = clipLength + casingChosen.samples / _tailSpeaker.pitch - overlapTime * sRATE;

            // reproducción diferida de la segunda muestra
            // clipLength/SRATE devuelve la duración dela primera muestra en
            // función del SRATE el proyecto (44100 por defecto)
            _tailSpeaker.PlayScheduled(AudioSettings.dspTime + clipLength / sRATE);

            _casingSpeaker.clip = casingChosen;
            _casingSpeaker.PlayScheduled(AudioSettings.dspTime + shootLength / sRATE + Random.Range(0, timeVariation));
        }
        if (Input.GetKey(KeyCode.Alpha4) && !_loopSpeaker.isPlaying)
        {
            // Play loop
            AudioClip loopChosen = _loopData[Random.Range(0, _loopData.Length)];
            _loopSpeaker.volume = 1 + Random.Range(-volumeVariation, volumeVariation);
            _loopSpeaker.pitch = 1 + Random.Range(-pitchVariation, pitchVariation);

            _loopSpeaker.clip = loopChosen;

            _loopSpeaker.PlayScheduled(AudioSettings.dspTime + Random.Range(0, timeVariation));
        }
    }

}
