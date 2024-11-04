using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour, Interactable
{
    [SerializeField] Dialog dialog;
    NPCState state;
    float idleTimer=0f;
    Character character;
    private void Awake(){
        character=GetComponent<Character>();
    }
    public void Interact()
    {
        StartCoroutine(DialogManager.Instance.ShowDialog(dialog));
       
    }
    private void Update(){
        if(state==NPCState.Idle){
            idleTimer+=Time.deltaTime;
            if(idleTimer>2f){
                idleTimer=0f;
                StartCoroutine(character.Move(new Vector2(2,0)));
            }
        }
      character.HandleUpdate();
}
}
public enum NPCState{Idle,Walking}