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
    public float sliderSmoothing;
    [Header("Temporary")]
    //TODO: make this dynamic (make the battle manager spawn the enemy to allow for different people to spawn in from the same screen)
    public GameObject enemy;


    response enemyResponse;
    response lastEnemyResponse;
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
        bool loop = true;

        baseMenu.SetActive(false);
        //this big ass one liner basically just randomly selects a response from the enemies responses
        yield return new WaitForSeconds(0.5f);
        //this while loop prevents the enemy from choosing the same response twice in a row
        while (loop)
        {
            enemyResponse = enemyUnit.chooseResponse(enemyUnit.responses);
            if(enemyResponse != lastEnemyResponse){ loop = false; }
        }
        lastEnemyResponse = enemyResponse;
        dialogue.setOpponentDialogue(enemyResponse.responseText);
        yield return new WaitForSeconds(0.5f);
        state = battleState.playerTurn;
        playerTurn();
    }

    void playerTurn()
    {
        baseMenu.SetActive(true);
    }

    //execute whenever player chooses something
    public void OnPlayerDecision(int moveIndex) 
    {
        if (state != battleState.playerTurn) { return; }
        StartCoroutine(playerRespond(moveIndex));
    }

    //execute when player responds
    IEnumerator playerRespond(int moveIndex) 
    {
        state = 0;
        dialogue.setOpponentDialogue("");

        //check if the response is good, decent, bad, or very bad
        if (enemyResponse.correctResponses.Contains(moveIndex))
        {
            socialStatus += 0.1f;
        }
        else if (enemyResponse.decentResponses.Contains(moveIndex))
        {
            socialStatus += 0.05f;
        }
        else if (enemyResponse.badResponses.Contains(moveIndex))
        {
            socialStatus -= 0.1f;
        }
        else if (enemyResponse.veryBadResponses.Contains(moveIndex))
        {
            socialStatus -= 0.2f;
        }

        if (socialStatus < 0) 
        { 
            socialStatus = 0; 
        }
        else if (socialStatus >= 1)
        {
            state = battleState.win;
            win();
            yield break;
        }

        yield return new WaitForSeconds(0.5f);
        state = battleState.enemyTurn;
        StartCoroutine(enemyTurn());
    }

    //void that updates UI elements
    private void updateUI() 
    {
        //updates status bars
        if (socialStatusSlider.value != socialStatus) 
        {
            //Lerp function to make status bars move smooth
            socialStatusSlider.value = Mathf.Lerp(socialStatusSlider.value, socialStatus, sliderSmoothing * Time.deltaTime);
        }

        if (stressSlider.value != stress)
        {
            stressSlider.value = Mathf.Lerp(stressSlider.value, stress, sliderSmoothing * Time.deltaTime);
        }
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
