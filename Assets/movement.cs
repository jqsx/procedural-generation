using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    Rigidbody rb;
    float rotationX = 0;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        rb.velocity = (transform.forward * vertical + transform.right * horizontal).normalized * 1000 * Time.deltaTime;
        cameraRotation();
    }

    void cameraRotation()
    {
        rotationX -= Input.GetAxis("Mouse Y");
        Camera.main.transform.localRotation = Quaternion.Euler(Mathf.Clamp(rotationX, -80, 80), 0, 0);
        transform.Rotate(0, Input.GetAxis("Mouse X"), 0);
    }
}
