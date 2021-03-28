using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum battleState {start, playerTurn, enemyTurn, win, lose}

public class battleManager : MonoBehaviour
{
    gameManager manager;
    public battleState state;

    [Header("Battle Variables")]
    public int lives = 3;
    public int[] stressRange = new int[2];
    public int[] stressRangeInstinctual = new int[2];
    public float[] stimValues = new float[5];
    public float[] stimProbability = new float[5];
    public float stimStressPenalty;
    public float stimSocialPenalty;
    public float streakAddition;

    [Header("Transforms")]
    public GameObject enemyTransform;

    [Header("Character Animation Variables")]
    public GameObject mainCharacterSprite;
    mainCharacterAnimationController mainCharAnim;

    //game objects to be defined in the inspector
    [Header("UI Objects")]
    public UI_opponentDialogue dialogue;
    public GameObject socialStatusBar;
    public TextMeshProUGUI socialStatusLevelDisplay;
    public GameObject StressBar;
    public TextMeshProUGUI stressLevelDisplay;
    public TextMeshProUGUI[] stressMovesDisplay;
    public GameObject baseMenu;
    public GameObject stimMenu;
    public GameObject healthBar;
    public GameObject firstSelectedButton;
    public GameObject stimButton;
    public float sliderSmoothing;
    public GameObject patienceIndicator;
    public float patienceColorSmoothing;

    [Header("UI Text Indicators")]
    public GameObject playerTurnIndicator;
    public GameObject enemyTurnIndicator;
    public GameObject winIndicator;
    public GameObject loseIndicator;

    [Header("UI Particle Systems")]
    [Tooltip("Element 0 is the prefab for the particle, Element 1 is the transform where it will be instantiated")]

    [Header("Misc")]
    public Color[] patienceColors;

    [Header("Temporary")]
    //TODO: make this dynamic (make the battle manager spawn the enemy to allow for different people to spawn in from the same screen)
    public GameObject enemy;
    Button firstSelectedButtonComponent;
    Button stimButtonComponent;

    //enemy variables
    response enemyResponse;
    response lastEnemyResponse;
    int turnLimit;
    int turnLimitMax;

    //variables defined in code
    [Header("Initial Values")]
    public float socialStatus = 0.1f;
    public float stress = 0.5f;

    //misc vars
    EnemyUnit enemyUnit;
    Slider socialStatusSlider;
    Slider stressSlider;
    Slider healthSlider;
    TextMeshProUGUI turnIndicator;
    RawImage patienceImage;
    int streak = 0;
    //array that holds the stress values
    private float[] StressValues = new float[8];
    bool isMentalShutdown = false;

    //debug
    int turnCounter = -1;
    void Awake()
    {
        manager = GameObject.Find("gameManagerObject").GetComponent<gameManager>();
        //spawn in enemy that is stated by gameManager
        enemy = GameObject.Instantiate(manager.enemyPrefab, enemyTransform.transform);
        //
        stress = manager.stress;
        lives = manager.lives;
        //parse components from gameobjects
        enemyUnit = enemy.GetComponent<EnemyUnit>();
        socialStatusSlider = socialStatusBar.GetComponent<Slider>();
        stressSlider = StressBar.GetComponent<Slider>();
        healthSlider = healthBar.GetComponent<Slider>();
        firstSelectedButtonComponent = firstSelectedButton.GetComponent<Button>();
        stimButtonComponent = stimButton.GetComponent<Button>();
        patienceImage = patienceIndicator.GetComponent<RawImage>();
        //turnIndicator = turnIndicatorObject.GetComponent<TextMeshProUGUI>();

        enemyTurnIndicator.GetComponent<TextMeshProUGUI>().text = enemyUnit.name + "'s Turn";

        mainCharAnim = mainCharacterSprite.GetComponent<mainCharacterAnimationController>();

        turnLimit = enemyUnit.turnLimit;
        turnLimitMax = turnLimit - 1;

        state = battleState.start;
        StartCoroutine(setupBattle());
    }

    //void to setup the proper variables for battle
    IEnumerator setupBattle() 
    {
        dialogue.setOpponentDialogue("");
        //turnIndicatorObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        state = battleState.enemyTurn;
        StartCoroutine(enemyTurn());
    }

    //void which executes proper action for enemy turn
    IEnumerator enemyTurn()
    {
        enemyTurnIndicator.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        turnCounter++;
        //increment turnlimit down
        turnLimit--;
        //checks if enemy turn limit is depleted
        if (turnLimit < 0) 
        {
            state = battleState.lose;
            dialogue.setOpponentDialogue("I've lost my patience with you.");
            lose();
            yield break;
        }
        bool loop = true;

        baseMenu.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        //this while loop prevents the enemy from choosing the same response twice in a row
        while (loop)
        {
            //this big ass one liner basically just randomly selects a response from the enemies responses
            enemyResponse = enemyUnit.chooseResponse(enemyUnit.responseCategories);
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
        //turnIndicatorObject.SetActive(false);
        state = battleState.playerTurn;
        playerTurn();
    }

    void playerTurn()
    {
        playerTurnIndicator.SetActive(true);
        //turnIndicatorObject.SetActive(true);
        //set stim button to interactable
        stimButtonComponent.interactable = true;
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

        //sets value of all the textboxes to stress values
        for(int i = 0; i < 8; i++) 
        {
            stressMovesDisplay[i].text = (StressValues[i] * 100).ToString("F0");
        }

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
        StartCoroutine(stimMethod(stimIndex));
    }

    IEnumerator stimMethod(int stimIndex) 
    {
        if (state != battleState.playerTurn) { yield break; }
        //subtract proper stress level from player depending on stim
        stress -= stimValues[stimIndex] / 100;
        if (stress < 0) stress = 0;
        mainCharAnim.animateStim(stimIndex);
        Debug.Log(mainCharAnim.currentAnimLength);
        yield return new WaitForSeconds(mainCharAnim.currentAnimLength);
        //if the penalty registers as true, subtract a value from social status and add stress
        if (Random.Range(0f, 100f) <= stimProbability[stimIndex])
        {
            socialStatus -= stimSocialPenalty / 100;
            if (socialStatus < 0)
            {
                socialStatus = 0;
            }
            //turnIndicatorObject.SetActive(false);
            StartCoroutine(stimFail());
            yield break;
        }
        baseMenu.SetActive(true);
        yield break;
    }

    //ngl made this just to make the delay feel smoother
    IEnumerator stimFail() 
    {
        streak = 0;
        firstSelectedButtonComponent.Select();
        dialogue.setOpponentDialogue("?!");
        yield return new WaitForSeconds(0.01f);
        baseMenu.SetActive(false);
        state = 0;
        yield return new WaitForSeconds(0.5f);
        state = battleState.enemyTurn;
        StartCoroutine(enemyTurn());
    }

    //execute when player responds
    IEnumerator playerRespond(int moveIndex) 
    {
        state = 0;
        dialogue.setOpponentDialogue("");

        //Add the proper amount of stress depending on the move, and the value defined in playerTurn()
        mainCharAnim.animateResponse(moveIndex);
        yield return new WaitForSeconds(mainCharAnim.currentAnimLength + 0.15f);
        stress += StressValues[moveIndex];
        //check if the response is good, decent, bad, or very bad
        if (enemyResponse.correctResponses.Contains(moveIndex))
        {
            //streak 
            socialStatus += (0.1f * enemyResponse.responseWeight) + (streak * streakAddition)/100;
            streak++;
        }
        else if (enemyResponse.decentResponses.Contains(moveIndex))
        {
            socialStatus += (0.05f * enemyResponse.responseWeight) + (streak * streakAddition)/100;
            streak++;
        }
        else if (enemyResponse.badResponses.Contains(moveIndex))
        {
            socialStatus -= 0.1f * enemyResponse.responseWeight;
            streak = 0;
        }
        else if (enemyResponse.veryBadResponses.Contains(moveIndex))
        {
            socialStatus -= 0.2f * enemyResponse.responseWeight;
            streak = 0;
        }

        if (socialStatus < 0) 
        { 
            socialStatus = 0;
        }
        else if (socialStatus >= 1)
        {
            if (enemyUnit.hasMultiplePhases && !enemyUnit.isFinalPhase)
            {
                socialStatus = 1;
                yield return new WaitForSeconds(0.7f);
                socialStatus = 0;
                enemyUnit.currentPhase++;
                if(enemyUnit.currentPhase >= enemyUnit.responseCategories.Length - 1)
                {
                    enemyUnit.isFinalPhase = true;
                }
            }
            else
            {
                state = battleState.win;
                socialStatus = 1;
                win();
                yield break;
            }
        }

        //if stress is overflowing, reset stress and give player mental shutdown
        if (stress <= 0.0f)
        {
            stress = 0;
        }
        else if (stress >= 1.0f)
        {
            stress = 1;
            yield return new WaitForSeconds(0.7f);
            mainCharAnim.playDamageAnimation();
            stress = 0;
            isMentalShutdown = true;
            lives--;
        }

        if (lives <= 0)
        {
            yield return new WaitForSeconds(1);
            state = battleState.lose;
            lose();
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
            socialStatusLevelDisplay.text = (socialStatus * 100).ToString("F0") + "%";
            //Lerp function to make status bars move smooth
            socialStatusSlider.value = Mathf.Lerp(socialStatusSlider.value, socialStatus, sliderSmoothing * Time.deltaTime);
        }

        if (stressSlider.value != stress)
        {
            stressLevelDisplay.text = (stress * 100).ToString("F0") + "%";
            stressSlider.value = Mathf.Lerp(stressSlider.value, stress, sliderSmoothing * Time.deltaTime);
        }

        if (healthSlider.value != lives)
        {
            healthSlider.value = Mathf.Lerp(healthSlider.value, lives, sliderSmoothing * Time.deltaTime);
        }
        //updates turn indicator
        /*switch (state)
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

            case battleState.lose:
                turnIndicator.text = "You Lose";
                break;

            default:
                turnIndicator.text = "";
                break;
        }*/

        float turnPercent = (float)turnLimit / (float)turnLimitMax;
        if (turnPercent <= 1f/3f)
        {
            patienceImage.color = Color.Lerp(patienceImage.color, patienceColors[2], patienceColorSmoothing * Time.deltaTime);
        }
        else if (turnPercent <= 2f/3f)
        {
            patienceImage.color = Color.Lerp(patienceImage.color, patienceColors[1], patienceColorSmoothing * Time.deltaTime);
        }
        else
        {
            patienceImage.color = Color.Lerp(patienceImage.color, patienceColors[0], patienceColorSmoothing * Time.deltaTime);
        }
        return;
    }
    //void for when the player wins
    void win() 
    {
        winIndicator.SetActive(true);
    }
    //void for when the player loses
    void lose()
    {
        loseIndicator.SetActive(true);
    }

    private void Update()
    {
        //TODO make this more efficient/performant
        updateUI();
        //Debug.Log(enemyUnit.isFinalPhase);
    }
}
