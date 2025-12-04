using UnityEngine;
using System.Threading.Tasks;

public class SceneUsageExample : MonoBehaviour
{
    // Funzione chiamata da un bottone
    public async void OnLoadNextSceneClicked(int nextSceneBuildIndex)
    {
        // Avvia il caricamento, usa async/await per attendere il completamento
        Debug.Log("Inizio caricamento asincrono...");

        try
        {
            // Chiama il servizio (la facciata) che usa l'implementazione ISceneLoader
            await SceneService.ChangeSceneAsync(nextSceneBuildIndex, OnProgressUpdate);

            Debug.Log("Caricamento scena completato!");
            // Qui puoi fare logica post-caricamento se necessario,
            // anche se l'esecuzione si interromperà al cambio scena.
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Errore durante il caricamento della scena: {ex.Message}");
        }
    }

    // Metodo per aggiornare l'interfaccia utente con la progressione
    private void OnProgressUpdate(float progress)
    {
        // Esempio: Aggiorna una barra di caricamento
        // uiProgressBar.fillAmount = progress;
        Debug.Log($"Progressione caricamento: {progress * 100:F0}%");
    }
}