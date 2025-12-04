using UnityEngine;

// Questo script è necessario per inizializzare il SceneService al lancio del gioco.
// Dovrebbe essere attaccato a un GameObject nel primo/boot scena e marcato come DontDestroyOnLoad.
public class SceneServiceInitializer : MonoBehaviour
{
    private void Awake()
    {
        // 1. Assicurati che l'oggetto non venga distrutto quando si cambia scena.
        DontDestroyOnLoad(gameObject);

        // 2. Ottieni o aggiungi l'implementazione concreta (AsyncSceneLoader) a questo GameObject.
        AsyncSceneLoader loader = GetComponent<AsyncSceneLoader>();
        if (loader == null)
        {
            loader = gameObject.AddComponent<AsyncSceneLoader>();
        }

        // 3. Inizializza il SceneService con l'istanza del loader.
        // Il SceneService dipenderà ora solo dall'interfaccia ISceneLoader.
        SceneService.Initialize(loader);

        Debug.Log("SceneService inizializzato con AsyncSceneLoader.");
    }
}