using System;
using System.Threading.Tasks;

// ISceneLoader definisce il contratto per qualsiasi meccanismo di caricamento di scene.
// Ritorna un Task per consentire l'uso di async/await e prende un'azione per il report della progressione.
public interface ISceneLoader
{
    /// <summary>
    /// Avvia il caricamento asincrono di una scena tramite l'indice di build.
    /// </summary>
    /// <param name="buildIndex">L'indice di build della scena da caricare.</param>
    /// <param name="onProgress">Azione per riportare la percentuale di progresso (0.0 a 1.0).</param>
    /// <returns>Task che si completa al termine del caricamento.</returns>
    Task LoadSceneAsync(int buildIndex, Action<float> onProgress = null);

    /// <summary>
    /// Riavvia in modo asincrono la scena attualmente attiva.
    /// </summary>
    /// <param name="onProgress">Azione per riportare la percentuale di progresso (0.0 a 1.0).</param>
    /// <returns>Task che si completa al termine del caricamento.</returns>
    Task RestartCurrentSceneAsync(Action<float> onProgress = null);
}