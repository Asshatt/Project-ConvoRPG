using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum battleState {start, playerTurn, enemyTurn, win, lose}

public class battleManager : MonoBehaviour
{
    public battleState state;
    
    //game objects to be defined in the inspector
    [Header("UI Objects")]
    public UI_opponentDialogue dialogue;
    public GameObject socialStatusBar;
    public GameObject StressBar;
    public TextMeshProUGUI turnIndicator;
    public GameObject baseMenu;
    [Header("Temporary")]
    //TODO: make this dynamic (make the battle manager spawn the enemy to allow for different people to spawn in from the same screen)
    public GameObject enemy;


    response enemyResponse;
    //variables defined in code
    float socialStatus = 0.1f;
    float stress = 0.5f;
    EnemyUnit enemyUnit;
    Slider socialStatusSlider;
    Slider stressSlider;

    void Start()
    {     
        enemyUnit = enemy.GetComponent<EnemyUnit>();
        socialStatusSlider = socialStatusBar.GetComponent<Slider>();
        stressSlider = StressBar.GetComponent<Slider>();
        state = battleState.start;
        StartCoroutine(setupBattle());
    }

    //void to setup the proper variables for battle
    IEnumerator setupBattle() 
    {
        dialogue.setOpponentDialogue("");
        yield return new WaitForSeconds(2f);
        state = battleState.enemyTurn;
        StartCoroutine(enemyTurn());
    }

    //void which executes proper action for enemy turn
    IEnumerator enemyTurn()
    {
        baseMenu.SetActive(false);
        //this big ass one liner basically just randomly selects a response from the enemies responses
        yield return new WaitForSeconds(1f);
        enemyResponse = enemyUnit.chooseResponse(enemyUnit.responses);
        dialogue.setOpponentDialogue(enemyResponse.responseText);
        yield return new WaitForSeconds(1f);
        state = battleState.playerTurn;
        playerTurn();
    }

    void playerTurn()
    {
        baseMenu.SetActive(true);
    }

    //execute whenever player chooses something
    //TODO: make this dynamic. Have the input be whichever move the player selected (currently its just a basic attack)
    public void OnPlayerDecision(int moveIndex) 
    {
        if (state != battleState.playerTurn) { return; }
        StartCoroutine(playerRespond(moveIndex));
    }

    //execute when player responds
    //TODO: Make the action here dependent on player action
    IEnumerator playerRespond(int moveIndex) 
    {
        dialogue.setOpponentDialogue("");
        socialStatus += 0.1f;
        if (socialStatus >= 1) 
        {
            state = battleState.win;
            win();
            yield break;
        }

        state = battleState.enemyTurn;
        yield return new WaitForSeconds(1);
        StartCoroutine(enemyTurn());
    }

    //void that updates UI elements
    private void updateUI() 
    {
        //updates status bars
        socialStatusSlider.value = socialStatus;
        stressSlider.value = stress;

        //updates turn indicator
        switch (state)
        {
            case battleState.playerTurn:
                turnIndicator.text = "Your Turn";
                break;

            case battleState.enemyTurn:
                turnIndicator.text = enemyUnit.name + "'s Turn";
                break;

            case battleState.win:
                turnIndicator.text = "You Win!";
                break;

            default:
                turnIndicator.text = "";
                break;
        }
        return;
    }
    //void for when the player wins
    void win() 
    {
        
    }

    private void Update()
    {
        updateUI();
    }
}
