using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Threading.Tasks;
using System.Collections;

// AsyncSceneLoader implementa l'interfaccia ISceneLoader.
// È un MonoBehaviour perché ha bisogno di eseguire Coroutine per gestire l'AsyncOperation di Unity.
public class AsyncSceneLoader : MonoBehaviour, ISceneLoader
{
    // Implementazione del caricamento tramite indice di build
    public Task LoadSceneAsync(int buildIndex, Action<float> onProgress = null)
    {
        return LoadSceneInternal(buildIndex, onProgress);
    }

    // Implementazione del riavvio della scena
    public Task RestartCurrentSceneAsync(Action<float> onProgress = null)
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        return LoadSceneInternal(currentSceneIndex, onProgress);
    }

    // Metodo interno che usa una Coroutine per gestire l'AsyncOperation
    private Task LoadSceneInternal(int buildIndex, Action<float> onProgress)
    {
        // TaskCompletionSource viene usato per "trasformare" un'operazione basata su Coroutine
        // in un Task standard C# che può essere atteso con 'await'.
        var tcs = new TaskCompletionSource<bool>();
        StartCoroutine(LoadSceneCoroutine(buildIndex, onProgress, tcs));
        return tcs.Task;
    }

    private IEnumerator LoadSceneCoroutine(int buildIndex, Action<float> onProgress, TaskCompletionSource<bool> tcs)
    {
        // Controlla che l'indice sia valido (opzionale, ma consigliato)
        if (buildIndex < 0 || buildIndex >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogError($"Tentativo di caricare una scena con indice di build non valido: {buildIndex}");
            tcs.SetException(new IndexOutOfRangeException($"Scena con indice {buildIndex} non trovata."));
            yield break; // Esce dalla Coroutine
        }

        // Avvia l'operazione di caricamento asincrona
        AsyncOperation operation = SceneManager.LoadSceneAsync(buildIndex);

        // Assicura che la scena non si attivi subito dopo il caricamento
        // Questo è utile per mostrare una schermata di caricamento completa.
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            // La progressione di Unity va da 0.0 a 0.9.
            // Quando raggiunge 0.9, è quasi pronta, manca solo l'attivazione.
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            // Riporta la progressione (0.0 a 1.0)
            onProgress?.Invoke(progress);

            // Quando il caricamento del contenuto è completato (progresso >= 0.9)
            if (operation.progress >= 0.9f)
            {
                // In questo punto, potresti attendere un input dell'utente o un tempo minimo
                // prima di impostare allowSceneActivation = true.

                // Riporta la progressione finale (1.0)
                onProgress?.Invoke(1.0f);

                // Attiva la scena (passa allo step successivo del caricamento)
                operation.allowSceneActivation = true;
            }

            yield return null; // Attende il frame successivo
        }

        // Se l'operazione è completata (isDone è true), imposta il Task come successo.
        tcs.SetResult(true);
    }
}