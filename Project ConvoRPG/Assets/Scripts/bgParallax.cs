using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bgParallax : MonoBehaviour
{
    private Vector3 startPos;
    public GameObject cam;
    [Range(0, 1)]
    public float parallaxAmount;
    // Start is called before the first frame update
    void Start()
    {
        startPos = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float xdist = (cam.transform.position.x * parallaxAmount);
        float ydist = (cam.transform.position.y * parallaxAmount);

        transform.position = new Vector3(startPos.x + xdist, startPos.y + ydist, startPos.z);
    }
}
