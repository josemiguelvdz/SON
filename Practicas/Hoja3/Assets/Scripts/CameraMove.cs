using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraMove : MonoBehaviour
{
    public float speed = 5.0f;
    void Start() { }
    void Update()
    {
        //Debug.Log(speed);
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += Vector3.left * Time.deltaTime * speed;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.position += Vector3.right * Time.deltaTime * speed;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            transform.position += Vector3.forward * Time.deltaTime * speed;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.position -= Vector3.forward * Time.deltaTime * speed;
        }
    }
}
