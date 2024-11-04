using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public enum BattleState { Start, ActionSelection, MoveSelection, PerfomMove, Busy,PartyScreen,BatteOver }
public enum BattleAction{Move,SwitchPokemon,useItem,Run}
public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] BattleDialogBox dialogBox;
    [SerializeField] PartyScreen partyScreen;
    public event Action<bool> OnBattleOver;

    BattleState state;
    int currentAction;
    int currentMove;
    PokemonParty playerParty;
    Pokemon wildPokemon;
    int currentMember;
    int escapeAttemps;


    public void StartBattle(PokemonParty playerParty, Pokemon wildPokemon)
    {
        this.playerParty = playerParty;
        this.wildPokemon = wildPokemon;
        StartCoroutine(SetupBattle());
    }

    public IEnumerator SetupBattle()
    {
        playerUnit.Setup(playerParty.GetHealthyPokemon());
        enemyUnit.Setup(wildPokemon);
        partyScreen.Init();

        dialogBox.SetMoveNames(playerUnit.Pokemon.Moves);

        yield return dialogBox.TypeDialog($"Un  {enemyUnit.Pokemon.Base.Name} salvaje ha aparecido.");

        ActionSelection();
    }
    void BattleOver(bool won){
        state= BattleState.BatteOver;
        OnBattleOver(won);
    }
    void ActionSelection()
    {
        state = BattleState.ActionSelection;
        dialogBox.SetDialog("Elige una acción");
        dialogBox.EnableActionSelector(true);
    }

    void OpenPartyScreen()
    {
        state=BattleState.PartyScreen;
        partyScreen.SetPartyData(playerParty.Pokemons);
        partyScreen.gameObject.SetActive(true);

    }
    void MoveSelection()
    {
        state = BattleState.MoveSelection;
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableDialogText(false);
        dialogBox.EnableMoveSelector(true);
    }

    IEnumerator PlayerMove()
    {
        state = BattleState.PerfomMove;
        var move = playerUnit.Pokemon.Moves[currentMove];
        yield return RunMove(playerUnit,enemyUnit,move);

        if(state==BattleState.PerfomMove)
        StartCoroutine(EnemyMove());
        
    }
    IEnumerator EnemyMove()
    {
        state = BattleState.PerfomMove;
        var move = enemyUnit.Pokemon.GetRandomMove();
        yield return RunMove(enemyUnit,playerUnit,move);
        //If the battle stat was not changed by RunMove, then go to 
        if(state== BattleState.PerfomMove)
        ActionSelection();
            }

     IEnumerator RunMove(BattleUnit sourceUnit,BattleUnit targetUnit,Move move){
        move.PP--;
        yield return dialogBox.TypeDialog($"{sourceUnit.Pokemon.Base.Name} usó {move.Base.Name}");
        sourceUnit.PlayAttackAnimation();
        yield return new WaitForSeconds(1f);
        targetUnit.PlayHitAnimation();
        var damageDetails = targetUnit.Pokemon.TakeDamage(move, sourceUnit.Pokemon);
        yield return targetUnit.Hud.UpdateHP();
        yield return ShowDamageDetails(damageDetails);
        if (damageDetails.Fainted)
        {
            yield return dialogBox.TypeDialog($"{targetUnit.Pokemon.Base.Name} se debilitó");
            targetUnit.PlayFaintedAnimation();

            yield return new WaitForSeconds(2f);
            OnBattleOver(true);

            CheckForBattleOver(targetUnit);
        }
    }
    void CheckForBattleOver(BattleUnit faintedUnit)
    {
        if(faintedUnit.IsPlayerUnit)
        {
                 var nextPokemon = playerParty.GetHealthyPokemon();
            if (nextPokemon != null)
                OpenPartyScreen();
                else
                    BattleOver(false);
        }
        else
            OnBattleOver(true);
    }
    IEnumerator ShowDamageDetails(Pokemon.DamageDetails damageDetails)
    {
        if (damageDetails.Critical > 1f)
            yield return dialogBox.TypeDialog("¡Un golpé crítico!");
        if (damageDetails.TypeEffectiveness > 1f)
            yield return dialogBox.TypeDialog("¡Es super efectivo!");
        else if (damageDetails.TypeEffectiveness < 1f)
            yield return dialogBox.TypeDialog("No es muy efectivo");

    }

    public void HandleUpdate()
    {
        if (state == BattleState.ActionSelection)
        {
            HandleActionSelection();
        }
        else if (state == BattleState.MoveSelection)
        {
            HandleMoveSelection();
        }
         else if (state == BattleState.PartyScreen)
        {
            HandlePartySelection();
        }

    }
    void HandleActionSelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
            ++currentAction;
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            --currentAction;
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            currentAction += 2;
        else if (Input.GetKeyDown(KeyCode.UpArrow))
            currentAction -= 2;

        currentAction = Mathf.Clamp(currentAction, 0, 3);
        dialogBox.UpdateActionSelection(currentAction);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (currentAction == 0)
            {
                MoveSelection();
            }
            else if (currentAction == 1) { }
            else if (currentAction == 2)
            {
                OpenPartyScreen();
            }
            else if (currentAction == 3) {
                    StartCoroutine(TryToEscape());
             }

        }
    }
    void HandleMoveSelection()
    {

        if (Input.GetKeyDown(KeyCode.RightArrow))
            ++currentMove;
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            --currentMove;
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            currentMove += 2;
        else if (Input.GetKeyDown(KeyCode.UpArrow))
            currentMove -= 2;

        currentMove = Mathf.Clamp(currentMove, 0, playerUnit.Pokemon.Moves.Count - 1);

        dialogBox.UpdateMoveSelection(currentMove, playerUnit.Pokemon.Moves[currentMove]);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableDialogText(true);
            StartCoroutine(PlayerMove());
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableDialogText(true);
            ActionSelection();
        }
    }
        void HandlePartySelection(){
                if (Input.GetKeyDown(KeyCode.RightArrow))
            ++currentMember;
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            --currentMember;
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            currentMember += 2;
        else if (Input.GetKeyDown(KeyCode.UpArrow))
            currentMember -= 2;

        currentMember = Mathf.Clamp(currentMember, 0, playerParty.Pokemons.Count - 1);

        partyScreen.UpdateMemberSelection(currentMember);

            if(Input.GetKeyDown(KeyCode.Z)){
                var selectedMember = playerParty.Pokemons[currentMember];
                if(selectedMember.HP<=0){
                    partyScreen.SetMessageText("No puedes enviar a un pokemon debilitado");
                    return;

                }
                if(selectedMember==playerUnit.Pokemon){
                    partyScreen.SetMessageText("No puedes cambiar  al mismo pokemon");
                    return;
                }

                partyScreen.gameObject.SetActive(false);
                state=BattleState.Busy;
                StartCoroutine(SwitchPokemon(selectedMember));
            }
            else if(Input.GetKeyDown(KeyCode.X)){
            partyScreen.gameObject.SetActive(false);
            ActionSelection();

            }
        }
        IEnumerator SwitchPokemon(Pokemon newPokemon)
        {
            if(playerUnit.Pokemon.HP>0){
            yield return dialogBox.TypeDialog($"Vuelve {playerUnit.Pokemon.Base.Name}");
            playerUnit.PlayFaintedAnimation();
            yield return new WaitForSeconds(2f);
        }
            playerUnit.Setup(newPokemon);
            dialogBox.SetMoveNames(newPokemon.Moves);
            yield return dialogBox.TypeDialog($"¡Adelante {newPokemon.Base.Name}!");

            StartCoroutine(EnemyMove());
        }
        IEnumerator TryToEscape(){
            escapeAttemps++;
        int playerSpeed=playerUnit.Pokemon.Speed;
        int enemySpeed=enemyUnit.Pokemon.Speed;
        
        if(enemySpeed<playerSpeed){
            yield return dialogBox.TypeDialog("Has huido exitosamente");
            BattleOver(true);
        }
            else{
                float f= (playerSpeed* 128) / enemySpeed +30 * escapeAttemps;
                f= f % 256;

                if(UnityEngine.Random.Range(0,256) <f){
                     yield return dialogBox.TypeDialog("Has huido exitosamente");
                     BattleOver(true);   
                }else
                {
                                yield return dialogBox.TypeDialog("No pudiste escapar");
                               StartCoroutine(EnemyMove());

                }
            }
        }
        
}



