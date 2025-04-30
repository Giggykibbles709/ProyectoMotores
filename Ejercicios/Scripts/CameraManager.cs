using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera camera1; // C�mara 1 perspectiva
    public Camera camera2; // C�mara 2 perspectiva
    public Camera isometricCamera; // C�mara isom�trica
    public Camera orthographicCamera; // C�mara ortogr�fica

    public GameObject ball;
    public GameObject player1;
    public GameObject player2;

    private void Start()
    {
        ActivateSplitScreen(); // Inicia con la pantalla dividida.
    }

    private void Update()
    {
        // Activar c�mara isom�trica.
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ActivateIsometricCamera();
        }

        // Activar c�mara ortogr�fica.
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ActivateOrthographicCamera();
        }

        // Activar pantalla dividida.
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ActivateSplitScreen();
        }
    }

    private void ActivateSplitScreen()
    {
        SetCameraState(camera1, true);
        SetCameraState(camera2, true);
        SetCameraState(isometricCamera, false);
        SetCameraState(orthographicCamera, false);

        ball.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        player1.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        player2.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void ActivateIsometricCamera()
    {
        SetCameraState(camera1, false);
        SetCameraState(camera2, false);
        SetCameraState(isometricCamera, true);
        SetCameraState(orthographicCamera, false);

        ball.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        player1.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        player2.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void ActivateOrthographicCamera()
    {
        SetCameraState(camera1, false);
        SetCameraState(camera2, false);
        SetCameraState(isometricCamera, false);
        SetCameraState(orthographicCamera, true);

        RigidbodyConstraints constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotation;

        ball.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX;
        player1.GetComponent<Rigidbody>().constraints = constraints;
        player2.GetComponent<Rigidbody>().constraints = constraints;
        ball.transform.position = new Vector3(0, ball.transform.position.y, ball.transform.position.z);
        player1.transform.position = new Vector3(0, player1.transform.position.y, player1.transform.position.z);
        player2.transform.position = new Vector3(0, player2.transform.position.y, player2.transform.position.z);
    }

    private void SetCameraState(Camera cam, bool state)
    {
        if (cam != null)
        {
            cam.gameObject.SetActive(state);
        }
    }
}
