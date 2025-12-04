using UnityEngine;

public class LookAtCamera : MonoBehaviour
{

    public Camera cameraToLookAt;

    // Update is called once per frame
    void FixedUpdate()
    {
        LookAtCam();
    }

    private void LookAtCam()
    {

        if (cameraToLookAt == null) { return; }


        Vector3 directionToCamera = cameraToLookAt.transform.position - transform.position;


        directionToCamera.y = 0;

        // Calcola la rotazione verso la direzione
        if (directionToCamera.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToCamera);

            // Imposta la rotazione solo sull'asse Y
            transform.rotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
        }
    }
}
