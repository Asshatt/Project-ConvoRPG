using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;

public enum battleState {start, playerTurn, enemyTurn, win, lose}

public class battleManager : MonoBehaviour
{
    public battleState state;
    public static battleManager instance;
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
    public GameObject playerTransform;
    public GameObject enemyTransform;

    [Header("Character Animation Variables")]
    public GameObject mainCharacterSprite;
    mainCharacterAnimationController mainCharAnim;

    //game objects to be defined in the inspector
    [Header("UI Objects")]
    public UI_opponentDialogue dialogue;
    public UI_opponentDialogue playerDialogue;
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
    public GameObject phaseIndicator;

    [Header("Damage Indicator Prefab")]
    public GameObject damageIndicatorPrefab;
    public Color32 stressTextColor;
    public Color32 socialStatusTextColor;

    [Header("Misc")]
    public Color[] patienceColors;
    [Tooltip("Light Effect that shows up when player chooses the best answer")]
    public GameObject bestAnswerLight;
    public GameObject glitchEffect;

    [Header("Temporary")]
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
    response.responseDisplayText chosenDisplayText;
    int streak = 0;
    //array that holds the stress values
    private float[] StressValues = new float[8];
    public bool isMentalShutdown = false;

    //debug
    int turnCounter = -1;
    void Awake()
    {
        instance = this;
        //manager = GameObject.Find("gameManagerObject").GetComponent<gameManager>();
        //spawn in enemy that is stated by gameManager
        enemy = GameObject.Instantiate(gameManager.instance.enemyPrefab, enemyTransform.transform);
        //
        stress = gameManager.instance.stress;
        lives = gameManager.instance.lives;
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
        playerDialogue.setOpponentDialogue("");
        //turnIndicatorObject.SetActive(false);
        yield return new WaitForSeconds(3.5f);
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
            StartCoroutine(lose());
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
            //TODO delete this debug code

            if(enemyResponse != lastEnemyResponse){ loop = false; }
        }
        lastEnemyResponse = enemyResponse;
        //if player has a mental shutdown, obscure the response
        if (isMentalShutdown) 
        {
            dialogue.setOpponentDialogue("0̵̛̖͑̽̎̍̈̚-̶̡̞͎̟̮̑̇͜2̷̧͚̪̜̗̬͕͔͐3̴̧̛̺͚̗̼͖̮͔̟͍̓̃͒͂́̍̄̍̾͌̊̉͜͠@̴̗̑̋͌̽̄͘̚͝ͅ&̶̡̨̧͖̠͈͎̼͕̫͔̺͌̂ͅ#̵̢̡̩̗͙̳͓̲̇̓͒͒̓͛̐̏͐͑&̷̻͍̺͓͉͊̓̀̈̌́́̀̅͐̚͘͜͠*̵̡̘͈̼̼̗͎̞̰̘̳̗̬͈̹̊͑̊̀̏̾̊̿͋̚(̷̡̛̛̺̻͈̠̮͋̂͂̂́̈́̈̓͛̐͝͝)̸̡͔̯̼̦̙̩̐̓͐͑̅͗̉̓́!̵̡̛̭̟͉̙̝̞̯̐̓̃̽͆̏͑͘͠&̴̨̡̬͔͍̬̍͌̆");
        }
        else
        {
            chosenDisplayText = enemyResponse.chosenDisplayText();
            if (chosenDisplayText == null)
            {
                dialogue.setOpponentDialogue(enemyResponse.responseText);
            }
            else
            {
                dialogue.setOpponentDialogue(chosenDisplayText.text);
            }
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
        if (isMentalShutdown)
        {
            //Mental Shutdown Set False
            glitchEffect.SetActive(false);
            audioManager.audio.transitionToAudioSnapshot(0);
            isMentalShutdown = false;
        }
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
        
        //spawn stress decrease indicator
        GameObject stressDeduction = GameObject.Instantiate(damageIndicatorPrefab, playerTransform.transform);
        stressDeduction.GetComponent<TextMeshPro>().color = stressTextColor;
        stressDeduction.GetComponent<TextMeshPro>().text = "-" + stimValues[stimIndex].ToString();

        mainCharAnim.animateStim(stimIndex);
        Debug.Log(mainCharAnim.currentAnimLength);
        yield return new WaitForSeconds(mainCharAnim.currentAnimLength);
        //contain the stim probability in a temporary value
        //if the enemy response weight is less than one (if the response isn't important) then lower the probability of being spotted
        float stimProbabilityTemp = stimProbability[stimIndex];
        if(enemyResponse.responseWeight < 1f) 
        {
            stimProbabilityTemp *= enemyResponse.responseWeight;
            Debug.Log(stimProbabilityTemp);
        }
        //if the penalty registers as true, subtract a value from social status and add stress
        if (Random.Range(0f, 100f) <= stimProbabilityTemp)
        {
            //spawn social status text
            GameObject socialText = GameObject.Instantiate(damageIndicatorPrefab, enemyTransform.transform);
            socialText.GetComponent<TextMeshPro>().color = socialStatusTextColor;
            if (stimIndex == 3)
            {
                socialText.GetComponent<TextMeshPro>().text = "-" + stimSocialPenalty.ToString();
                socialStatus -= stimSocialPenalty / 100;
            }
            else
            {
                socialText.GetComponent<TextMeshPro>().text = "-" + (stimSocialPenalty/2).ToString();
                socialStatus -= stimSocialPenalty / 200;
            }
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
        if (isMentalShutdown)
        {
            //Mental Shutdown Set False
            glitchEffect.SetActive(false);
            audioManager.audio.transitionToAudioSnapshot(0);
            isMentalShutdown = false;
        }
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
        float socialStatusChange = 0;
        state = 0;
        dialogue.setOpponentDialogue("");

        //Add the proper amount of stress depending on the move, and the value defined in playerTurn()
        mainCharAnim.animateResponse(moveIndex);
        yield return new WaitForSeconds(mainCharAnim.currentAnimLength + 0.15f);
        stress += Mathf.Floor(StressValues[moveIndex] * 100)/100;
        //check if the response is good, decent, bad, or very bad
        if (enemyResponse.correctResponses.Contains(moveIndex))
        {
            //streak 
            socialStatusChange = Mathf.Floor((0.1f * enemyResponse.responseWeight) * 100)/100 + (streak * streakAddition) / 100;
            audioManager.audio.Play("bestAnswer");
            GameObject.Instantiate(bestAnswerLight, enemyTransform.transform);
            streak++;
        }
        else if (enemyResponse.decentResponses.Contains(moveIndex))
        {
            socialStatusChange = Mathf.Floor((0.05f * enemyResponse.responseWeight) * 100)/100 + (streak * streakAddition)/100;
            streak++;
        }
        else if (enemyResponse.badResponses.Contains(moveIndex))
        {
            socialStatusChange = -1 * Mathf.Floor((0.1f * enemyResponse.responseWeight) * 100)/100;
            streak = 0;
        }
        else if (enemyResponse.veryBadResponses.Contains(moveIndex))
        {
            socialStatusChange = -1 * Mathf.Floor((0.2f * enemyResponse.responseWeight) * 100)/100;
            streak = 0;
        }
        //add social status change
        socialStatus += socialStatusChange;

        //spawn social status text
        GameObject socialText = GameObject.Instantiate(damageIndicatorPrefab, enemyTransform.transform);
        socialText.GetComponent<TextMeshPro>().text = (socialStatusChange * 100).ToString();
        socialText.GetComponent<TextMeshPro>().color = socialStatusTextColor;

        if (socialStatus < 0) 
        { 
            socialStatus = 0;
        }
        else if (socialStatus >= 1)
        {
            if (enemyUnit.hasMultiplePhases && !enemyUnit.isFinalPhase)
            {
                streak = 0;
                socialStatus = 1;
                yield return new WaitForSeconds(0.7f);
                phaseIndicator.SetActive(true);
                yield return new WaitForSeconds(1.3f);
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
                StartCoroutine(win());
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
            //Mental Shutdown Set True
            stress = 1;
            yield return new WaitForSeconds(0.7f);
            glitchEffect.SetActive(true);
            mainCharAnim.playDamageAnimation();
            stress = 0;
            audioManager.audio.transitionToAudioSnapshot(1);
            isMentalShutdown = true;
            lives--;
        }

        if (lives <= 0)
        {
            yield return new WaitForSeconds(1);
            state = battleState.lose;
            StartCoroutine(lose());
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

        //updates player dialogue
        //Checks name of currently selected button and changes the player dialogue accordingly
        //yes this code is very bad, I might change it at some point, but don't count on that
        if (chosenDisplayText != null)
        {
            switch (EventSystem.current.currentSelectedGameObject.name)
            {
                case "Friendly":
                    if (isMentalShutdown)
                    {
                        playerDialogue.setOpponentDialogue("Yeah.");
                    }
                    else
                    {
                        playerDialogue.setOpponentDialogue(chosenDisplayText.playerResponseTexts[0]);
                    }
                    break;

                case "Sarcastic":
                    if (isMentalShutdown)
                    {
                        playerDialogue.setOpponentDialogue("Yeah?");
                    }
                    else
                    {
                        playerDialogue.setOpponentDialogue(chosenDisplayText.playerResponseTexts[1]);
                    }
                    break;

                case "Aggressive":
                    if (isMentalShutdown)
                    {
                        playerDialogue.setOpponentDialogue("What?!");
                    }
                    else
                    {
                        playerDialogue.setOpponentDialogue(chosenDisplayText.playerResponseTexts[2]);
                    }
                    break;

                case "Fearful":
                    if (isMentalShutdown)
                    {
                        playerDialogue.setOpponentDialogue("What...?");
                    }
                    else
                    {
                        playerDialogue.setOpponentDialogue(chosenDisplayText.playerResponseTexts[3]);
                    }
                    break;

                case "Deadpan":
                    if (isMentalShutdown)
                    {
                        playerDialogue.setOpponentDialogue("Y-yup...");
                    }
                    else
                    {
                        playerDialogue.setOpponentDialogue(chosenDisplayText.playerResponseTexts[4]);
                    }
                    break;

                case "":
                case null:
                    Debug.LogError("Dialogue not set");
                    break;
                default:
                    playerDialogue.setOpponentDialogue("");
                    break;
            }
        }
        return;
    }
    //void for when the player wins
    IEnumerator win() 
    {
        winIndicator.SetActive(true);
        //pass values from battle manager to game manager. Makes stress and lives persist between battles

        gameManager.instance.lives = lives;
        gameManager.instance.stress = stress;
        yield return new WaitForSeconds(5);
        //TODO make this scene transition dynamic
        SceneManager.LoadSceneAsync("Debug Menu");
    }
    //void for when the player loses
    IEnumerator lose()
    {
        loseIndicator.SetActive(true);

        gameManager.instance.lives = 3;
        gameManager.instance.stress = 0.5f;
        yield return new WaitForSeconds(5);
        SceneManager.LoadSceneAsync("Debug Menu");
    }

    private void Update()
    {
        //TODO make this more efficient/performant
        updateUI();
        Debug.Log(turnCounter);
    }
}
