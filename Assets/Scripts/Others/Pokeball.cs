using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pokeball : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Opcional: Muestra un mensaje de debug
            Debug.Log("Pokeball recogida!");

            // Destruye el objeto coleccionable
            Destroy(gameObject);
        }
    }
}