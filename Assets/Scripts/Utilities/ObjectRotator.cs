using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    [Header("Rotation in degrees/second")]
    [SerializeField] private Vector3 rotationSpeed = Vector3.zero;

    [Header("Enable/Disable axis")]
    [SerializeField] private bool rotateX = false;
    [SerializeField] private bool rotateY = true;
    [SerializeField] private bool rotateZ = false;

    private void Update()
    {

        if (!this.gameObject.activeInHierarchy) { return; }

        Vector3 rotation = new Vector3(
            rotateX ? rotationSpeed.x : 0f,
            rotateY ? rotationSpeed.y : 0f,
            rotateZ ? rotationSpeed.z : 0f
        );

        // Local Rotation
        transform.Rotate(rotation * Time.deltaTime, Space.Self);
    }
}
