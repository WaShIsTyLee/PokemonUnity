using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Collector : MonoBehaviour
{
    public int itemsToCollect = 3; // Número de ítems necesarios para completar la misión
    private int itemsCollected = 0; // Contador de ítems recogidos
    public GameObject missionCompleteCanvas;

    private void Start() {
        // Asegura que el Canvas de "Misión Completada" esté desactivado al inicio
        if (missionCompleteCanvas != null) {
            missionCompleteCanvas.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        IItem item = collision.GetComponent<IItem>();
        if (item != null) {
            item.Collect();
            itemsCollected++; // Incrementa el contador al recoger un ítem

            // Comprueba si se alcanzó el objetivo de la misión
            if (itemsCollected >= itemsToCollect) {
                ShowMissionCompleteCanvas();
            }
        }
    }

    // Método para activar el Canvas de "Misión Completada"
    private void ShowMissionCompleteCanvas() {
        if (missionCompleteCanvas != null) {
            missionCompleteCanvas.SetActive(true);
        }
    }

    // Método para el botón "Play Again"
    public void PlayAgain() {
        // Recarga la escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
