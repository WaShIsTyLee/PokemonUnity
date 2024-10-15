using UnityEngine;
using System.Collections; // Asegúrate de incluir esta línea

public class MovGuarderia : MonoBehaviour
{
    public Vector3[] positions; // Posiciones a las que se moverá el jugador
    public float moveTime = 1f; // Tiempo que tarda en mover
    public float waitTime = 1f; // Tiempo que espera en cada posición
    public bool isPlayerOne = true; // Identificador para el jugador

    
    private int currentTargetIndex = 0;

    void Start()
    {
        if (positions.Length > 0)
        {
            StartCoroutine(Move());
        }
        else
        {
            Debug.LogError("No hay posiciones definidas.");
        }
    }

    private IEnumerator Move()
    {
        while (true)
        {
            Vector3 targetPosition = positions[currentTargetIndex];
            float elapsedTime = 0;
            Vector3 startingPos = transform.position;

            // Moverse a la posición objetivo
            while (elapsedTime < moveTime)
            {
                transform.position = Vector3.Lerp(startingPos, targetPosition, elapsedTime / moveTime);
                elapsedTime += Time.deltaTime;
                yield return null; // Esperar hasta el siguiente frame
            }

            transform.position = targetPosition; // Asegurar que se establece la posición final

            // Esperar en la posición actual
            yield return new WaitForSeconds(waitTime);

            // Cambiar al siguiente índice
            currentTargetIndex = (currentTargetIndex + 1) % positions.Length;

            // Esperar un tiempo adicional antes de que el otro jugador se mueva
            yield return new WaitForSeconds(waitTime);
        }
    }
}
