using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum battleState {start, playerTurn, enemyTurn, win, lose}

public class battleManager : MonoBehaviour
{
    public battleState state;

    [Header("Battle Variables")]
    public int[] stressRange = new int[2];
    public int[] stressRangeInstinctual = new int[2];
    public float[] stimValues = new float[5];
    public float[] stimProbability = new float[5];
    public float stimStressPenalty;
    public float stimSocialPenalty;

    //game objects to be defined in the inspector
    [Header("UI Objects")]
    public UI_opponentDialogue dialogue;
    public GameObject socialStatusBar;
    public GameObject StressBar;
    public TextMeshProUGUI turnIndicator;
    public GameObject baseMenu;
    public GameObject stimMenu;
    public GameObject firstSelectedButton;
    public float sliderSmoothing;

    [Header("Temporary")]
    //TODO: make this dynamic (make the battle manager spawn the enemy to allow for different people to spawn in from the same screen)
    public GameObject enemy;
    //TODO: Made this not shit
    public TextMeshProUGUI stressLevelDisplay;

    Button firstSelectedButtonComponent;

    response enemyResponse;
    response lastEnemyResponse;
    //variables defined in code
    float socialStatus = 0.1f;
    float stress = 0.5f;
    EnemyUnit enemyUnit;
    Slider socialStatusSlider;
    Slider stressSlider;
    //array that holds the stress values
    private float[] StressValues = new float[8];
    bool isMentalShutdown = false;
    private bool isFirstStim = true;

    void Start()
    {     
        enemyUnit = enemy.GetComponent<EnemyUnit>();
        socialStatusSlider = socialStatusBar.GetComponent<Slider>();
        stressSlider = StressBar.GetComponent<Slider>();
        firstSelectedButtonComponent = firstSelectedButton.GetComponent<Button>();
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
        //if player has a mental shutdown, obscure the response
        if (isMentalShutdown) 
        {
            dialogue.setOpponentDialogue("0̵̛̖͑̽̎̍̈̚-̶̡̞͎̟̮̑̇͜2̷̧͚̪̜̗̬͕͔͐3̴̧̛̺͚̗̼͖̮͔̟͍̓̃͒͂́̍̄̍̾͌̊̉͜͠@̴̗̑̋͌̽̄͘̚͝ͅ&̶̡̨̧͖̠͈͎̼͕̫͔̺͌̂ͅ#̵̢̡̩̗͙̳͓̲̇̓͒͒̓͛̐̏͐͑&̷̻͍̺͓͉͊̓̀̈̌́́̀̅͐̚͘͜͠*̵̡̘͈̼̼̗͎̞̰̘̳̗̬͈̹̊͑̊̀̏̾̊̿͋̚(̷̡̛̛̺̻͈̠̮͋̂͂̂́̈́̈̓͛̐͝͝)̸̡͔̯̼̦̙̩̐̓͐͑̅͗̉̓́!̵̡̛̭̟͉̙̝̞̯̐̓̃̽͆̏͑͘͠&̴̨̡̬͔͍̬̍͌̆");
            isMentalShutdown = false;
        }
        else
        { 
            dialogue.setOpponentDialogue(enemyResponse.responseText);
        }
        yield return new WaitForSeconds(0.5f);
        state = battleState.playerTurn;
        playerTurn();
    }

    void playerTurn()
    {
        //set it so that this registers as player's first stim
        isFirstStim = true;
        //Randomly assign stress values to each response
        for (int i = 0; i < 8; i++)
        {
            //checks if response is instinctual. If not, the set the stress cost to be random
            if (enemyResponse.instinctualResponse.Contains(i))
            {
                StressValues[i] = Random.Range(stressRangeInstinctual[0], stressRangeInstinctual[1]);
            }
            else
            {
                StressValues[i] = Random.Range(stressRange[0], stressRange[1]);
            }
            StressValues[i] /= 100;
        }

        //sets the stress level display
        //TODO: make this not shit
        stressLevelDisplay.text =
            "Responses \n \n" +
            "Friendly = " + (StressValues[0] * 100) + "% \n" +
            "Sarcastic = " + (StressValues[1] * 100) + "% \n" +
            "Aggressive = " + (StressValues[2] * 100) + "% \n" +
            "Fearful = " + (StressValues[3] * 100) + "% \n" +
            "Deadpan = " + (StressValues[4] * 100) + "% \n" +
            "\n \n" + "React" + "\n \n" +
            "Stay Quiet = " + (StressValues[5] * 100) + "% \n" +
            "Laugh = " + (StressValues[6] * 100) + "% \n" +
            "Nod = " + (StressValues[7] * 100) + "% \n"
            ;

        baseMenu.SetActive(true);
    }

    //execute whenever player chooses a response
    public void OnPlayerDecision(int moveIndex) 
    {
        if (state != battleState.playerTurn) { return; }
        StartCoroutine(playerRespond(moveIndex));
    }

    //execute whenever player chooses a stim
    public void OnPlayerStim(int stimIndex) 
    {
        if (state != battleState.playerTurn) { return; }
        //if its not player's first stim, and the penalty registers as true, subtract a value from social status and add stress
        if (!isFirstStim && Random.Range(0f, 100f) <= stimProbability[stimIndex]) 
        {
            socialStatus -= stimSocialPenalty/100;
            stress += stimStressPenalty/100;
            state = battleState.enemyTurn;
            stimMenu.SetActive(false);
            firstSelectedButtonComponent.Select();
            StartCoroutine(enemyTurn());
        }
        else
        {
            isFirstStim = false;
        }
        //subtract proper stress level from player depending on stim
        stress -= stimValues[stimIndex] / 100;
    }

    //execute when player responds
    IEnumerator playerRespond(int moveIndex) 
    {
        state = 0;
        dialogue.setOpponentDialogue("");

        //Add the proper amount of stress depending on the move, and the value defined in playerTurn()
        stress += StressValues[moveIndex];
        //if stress is overflowing, reset stress and give player mental shutdown
        if(stress <= 0) 
        { 
            stress = 0; 
        }
        else if (stress >= 1)
        {
            stress = 0;
            isMentalShutdown = true;
        }

        //check if the response is good, decent, bad, or very bad
        if (enemyResponse.correctResponses.Contains(moveIndex))
        {
            socialStatus += 0.1f;
            stress -= 0.1f;
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
            stress += 0.1f;
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
