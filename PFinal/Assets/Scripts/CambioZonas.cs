using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FMODUnity;
using UnityEngine;

//Hay que crear los parametros en la interfaz de FMOD studio
public class CambioZonas : MonoBehaviour
{
    public StudioEventEmitter studioEventEmitter; // Un solo StudioEventEmitter para todas las zonas

    enum Zona { Llanura, Monta�a, Nieve };
    Zona zonaActual;

    void Start()
    {
        // Inicia el evento de FMOD y desactiva las otras capas
        zonaActual = Zona.Llanura;
        studioEventEmitter.Play();
        studioEventEmitter.Stop(); // Pausa el evento al inicio
    }

    void ChangeZone(Zona nuevaZona)
    {
        if (nuevaZona != zonaActual)
        {
            // Actualiza la zona actual
            zonaActual = nuevaZona;

            // Aplica efectos de sonido adicionales usando el StudioEventEmitter
            switch (zonaActual)
            {
                case Zona.Llanura:
                    studioEventEmitter.SetParameter("ReverbLevel", 0.1f); // Ajusta la cantidad de reverberaci�n para la llanura
                    studioEventEmitter.SetParameter("Pitch", 1f); // Pitch normal
                    studioEventEmitter.SetParameter("VolumenGradual", 0f); // Volumen gradual apagado
                    studioEventEmitter.SetParameter("LowPassFilter", 22000f); // Filtro de paso bajo desactivado
                    break;
                case Zona.Monta�a:
                    studioEventEmitter.SetParameter("ReverbLevel", 0.5f); // Ajusta la cantidad de reverberaci�n para la monta�a
                    studioEventEmitter.SetParameter("Pitch", 1.2f); // Incrementa el pitch
                    StartCoroutine(PlayRocasEffectGradually());
                    break;
                case Zona.Nieve:
                    studioEventEmitter.SetParameter("ReverbLevel", 0.8f); // Ajusta la cantidad de reverberaci�n para la nieve
                    studioEventEmitter.SetParameter("Pitch", 0.8f); // Reduce el pitch
                    studioEventEmitter.SetParameter("LowPassFilter", 15000f); // Aplica un filtro de paso bajo
                    break;
            }

            // Reanuda el evento y cambia seg�n la zona
            studioEventEmitter.Play();
        }
    }

    IEnumerator PlayRocasEffectGradually()
    {
        float targetVolume = 0.5f; // Volumen m�ximo para el evento de sonido de rocas
        float fadeDuration = 3f; // Duraci�n de la transici�n de volumen

        float startVolume = 0f;
        float currentTime = 0f;

        studioEventEmitter.SetParameter("VolumenGradual", startVolume); // Configura el par�metro de volumen gradual a 0
        studioEventEmitter.Play(); // Reproduce el evento de sonido de rocas

        while (currentTime < fadeDuration)
        {
            currentTime += Time.deltaTime;
            float volume = Mathf.Lerp(startVolume, targetVolume, currentTime / fadeDuration);
            studioEventEmitter.SetParameter("VolumenGradual", volume); // Ajusta el par�metro de volumen gradual
            yield return null;
        }

        studioEventEmitter.SetParameter("VolumenGradual", targetVolume); // Aseg�rate de que el volumen sea el m�ximo al final
    }
}
