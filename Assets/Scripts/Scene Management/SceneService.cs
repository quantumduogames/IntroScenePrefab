using System;
using System.Threading.Tasks;
using UnityEngine;

// SceneService è la facciata statica che l'applicazione userà.
// Questo modulo rispetta il DIP dipendendo solo dall'interfaccia ISceneLoader.
public static class SceneService
{
    // Dependency Inversion: la dipendenza è sull'interfaccia, non sull'implementazione concreta.
    private static ISceneLoader _loader;

    /// <summary>
    /// Inizializza il servizio con l'implementazione del loader.
    /// Deve essere chiamato all'avvio del gioco.
    /// </summary>
    /// <param name="loaderInstance">L'istanza di un ISceneLoader (e.g., AsyncSceneLoader).</param>
    public static void Initialize(ISceneLoader loaderInstance)
    {
        if (_loader != null)
        {
            Debug.LogWarning("SceneService è già stato inizializzato. Ignoro la nuova istanza.");
            return;
        }
        _loader = loaderInstance ?? throw new ArgumentNullException(nameof(loaderInstance), "Il loader non può essere null.");
    }

    /// <summary>
    /// Carica una scena in modo asincrono.
    /// </summary>
    public static Task ChangeSceneAsync(int buildIndex, Action<float> onProgress = null)
    {
        if (_loader == null)
        {
            throw new InvalidOperationException("SceneService non è stato inizializzato. Chiamare SceneService.Initialize() prima di usare.");
        }
        return _loader.LoadSceneAsync(buildIndex, onProgress);
    }

    /// <summary>
    /// Riavvia la scena corrente in modo asincrono.
    /// </summary>
    public static Task RestartCurrentSceneAsync(Action<float> onProgress = null)
    {
        if (_loader == null)
        {
            throw new InvalidOperationException("SceneService non è stato inizializzato. Chiamare SceneService.Initialize() prima di usare.");
        }
        return _loader.RestartCurrentSceneAsync(onProgress);
    }

    // Puoi mantenere i vecchi metodi sincroni, ma ora usano l'implementazione asincrona
    // bloccando l'esecuzione (DA USARE CON CAUTELA).
    [Obsolete("Evita di usare i metodi sincroni. Usa ChangeSceneAsync per prestazioni migliori.")]
    public static void ChangeScene(int buildIndex)
    {
        // Blocco sincrono per retrocompatibilità. DA EVITARE!
        ChangeSceneAsync(buildIndex).GetAwaiter().GetResult();
    }

    [Obsolete("Evita di usare i metodi sincroni. Usa RestartCurrentSceneAsync per prestazioni migliori.")]
    public static void RestartCurrentScene()
    {
        // Blocco sincrono per retrocompatibilità. DA EVITARE!
        RestartCurrentSceneAsync().GetAwaiter().GetResult();
    }
}