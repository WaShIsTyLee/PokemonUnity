using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public enum GameState { FreeRoam, Battle,Dialog }
public class GameController : MonoBehaviour
{
        [SerializeField] PlayerController playerController;
        [SerializeField] BattleSystem battleSystem;
        [SerializeField] Camera worldCamera;
   int pointsToFinish;
    int pointsEarned;
    GameObject[] gems;
    public GameObject gameOverScreen;

        GameState state;
        private void Start()
        {
                playerController.onEncountered += StartBattle;
                battleSystem.OnBattleOver += EndBattle;

                DialogManager.Instance.OnShowDialog+=()=>{
                        state=GameState.Dialog;
                };
                DialogManager.Instance.OnCloseDialog+=()=>{
                       if(state==GameState.Dialog)
                           state=GameState.FreeRoam;
                };

        pointsToFinish = 3;
        gems = GameObject.FindGameObjectsWithTag("Gem");
        foreach (GameObject gem in gems) {
            pointsToFinish += gem.GetComponent<Gem>().worth;
        }
        pointsEarned = 0;
        Gem.OnGemCollect += IncreasePointsEarned;  

        }


        void StartBattle()
        {
                state = GameState.Battle;
                battleSystem.gameObject.SetActive(true);
                worldCamera.gameObject.SetActive(false);
                var playerParty = playerController.GetComponent<PokemonParty>();
                var wildPokemon = FindObjectOfType<MapArea>().GetComponent<MapArea>().GetRandomWildPokemon();

                battleSystem.StartBattle(playerParty,wildPokemon);

        }
        void EndBattle(bool won)
        {
                state = GameState.FreeRoam;
                battleSystem.gameObject.SetActive(false);
                worldCamera.gameObject.SetActive(true);
        }

        private void Update()
        {
                if (state == GameState.FreeRoam)
                {
                        playerController.HandleUpdate();
                }
                else if (state == GameState.Battle)
                {
                        battleSystem.HandleUpdate();
                }
                else if(state==GameState.Dialog){
                        DialogManager.Instance.HandleUpdate();
                }
        }
 void IncreasePointsEarned(int amount)
{
    pointsEarned += amount;
    
    if (pointsEarned > pointsToFinish)
    {        if (gameOverScreen != null) {
            gameOverScreen.SetActive(true);
        } else {
            Debug.LogWarning("gameOverScreen no ha sido asignado en el Inspector.");
        }
    }
}

}