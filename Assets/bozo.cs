using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class bozo : MonoBehaviour
{
    public WebCamTexture cam;
    private void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        for (int i = 0; i < devices.Length; i++)
        {
            cam = new WebCamTexture(devices[i].name);
        }
        cam.name = "test";
        cam.Play();
        GetComponent<Renderer>().material.mainTexture = cam;
        GetComponent<Renderer>().material.mainTexture.wrapMode = TextureWrapMode.Repeat;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
