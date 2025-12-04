using UnityEngine;

public class MoveAndScaleObject : MonoBehaviour
{
    [Header("Configurazione movimento")]
    [Tooltip("Direzione dell'asse locale lungo cui muoversi (es. Y = (0,1,0))")]
    [SerializeField] private Vector3 localAxis = Vector3.up;
    [SerializeField] private float offset = 1.0f;
    [SerializeField] private float speed = 1.0f;

    [Header("Configurazione scala")]
    [Tooltip("Abilita l'animazione della scala Z da 0 al valore originale")]
    [SerializeField] private bool animateScaleZ = true;

    private Vector3 startLocalPos;
    private Vector3 targetLocalPos;
    private Vector3 originalScale;
    [SerializeField] private bool isMoving = true;

    private void Start()
    {
        startLocalPos = transform.localPosition;
        originalScale = transform.localScale;

        Vector3 normalizedAxis = localAxis.normalized;
        targetLocalPos = startLocalPos + normalizedAxis * offset;

        // Inizializza scala Z a 0 se l'animazione è attiva
        if (animateScaleZ)
        {
            Vector3 scaled = originalScale;
            scaled.z = 0f;
            transform.localScale = scaled;
        }
    }

    private void Update()
    {
        if (!isMoving)
            return;

        // Movimento
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetLocalPos, speed * Time.deltaTime);

        // Scala
        if (animateScaleZ)
        {
            Vector3 currentScale = transform.localScale;
            float newZ = Mathf.MoveTowards(currentScale.z, originalScale.z, speed * Time.deltaTime);
            transform.localScale = new Vector3(originalScale.x, originalScale.y, newZ);
        }

        // Fine animazione
        bool positionReached = Vector3.Distance(transform.localPosition, targetLocalPos) < 0.001f;
        bool scaleReached = !animateScaleZ || Mathf.Abs(transform.localScale.z - originalScale.z) < 0.001f;

        if (positionReached && scaleReached)
        {
            transform.localPosition = targetLocalPos;
            transform.localScale = originalScale;
            isMoving = false;
        }
    }
}
