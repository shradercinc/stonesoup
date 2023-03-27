using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pg2259Rotate : MonoBehaviour
{
    public static bool exist = false;
    float rotateSpeed = 150f;
    void Update()
    {
        transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
        /*
        foreach(GameObject go in transform)
        {
            go.transform.localPosition = new Vector3(3, 0, 0);
        }*/
    }

    public void AddToRotate(GameObject newRotate)
    {

        newRotate.transform.parent= transform.parent;
        newRotate.transform.localPosition = Random.Range(0f,1f) < 0.5f ? new Vector3(3, 0, 0) : new Vector3(-3, 0, 0);
        newRotate.transform.parent = transform;
    }

}
