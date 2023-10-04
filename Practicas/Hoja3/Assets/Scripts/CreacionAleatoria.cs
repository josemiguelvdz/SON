using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreacionAleatoria : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // generar y asignar posicion aleatoria
        transform.position = new Vector3(Random.Range(-30, 30), 0, Random.Range(-30, 30)); 
    }

}
