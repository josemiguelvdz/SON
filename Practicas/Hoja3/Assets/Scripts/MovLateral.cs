using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovLateral : MonoBehaviour
{
    public float range;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(Mathf.Sin(Time.time) * range, Mathf.Sin(Time.time) * range, 0);
    }
}
