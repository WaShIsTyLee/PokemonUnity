using UnityEngine;

public class Pokeball : MonoBehaviour
{
   private bool hasTriggered;
   
   private void OnTriggerEnter2D(Collider2D collision)
   {
    if(collision.CompareTag("Player")&& ! hasTriggered){
        Debug.Log("FUNCIONA CARAJO");
        hasTriggered=true;
        Destroy(gameObject);
    }
   }
   }

