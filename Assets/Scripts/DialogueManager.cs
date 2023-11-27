using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    private DialogueSystem dialogueSystem;
    [SerializeField] private PersistentData_SO playerData;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject answersPanel;
    [SerializeField] private GameObject formScreen;
    [SerializeField] private GameObject scoreScreen;
    [SerializeField] private GameObject defeatScreen;
    [SerializeField] private GameObject alertScreen;

    [SerializeField] private TextMeshProUGUI character;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [SerializeField] private UiTween uiAnimator;


    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    [SerializeField] private GameObject[] choicesHighlight;
    private TextMeshProUGUI[] choicesText;
    [SerializeField] private GameObject nextQuestionButton;
    [SerializeField] private GameObject continueButton;

    public List<Quizz> allQuizzes;
    public Quizz currentQuizz;
    private int currentQuizzID=0;
    private int currentQuestion;

    [Header("Characters")]
    public List<NarrativeCharacter> allCharacters;
    public NarrativeCharacter currentCharacter;
    private int currentCharacterID = 0;
    private int indexOfSentence = 0;
    private string phaseOfDialogue = "introNarration";


    [Header("SCORE")]
    public int loseTriggerAmount;
    public int winTriggerAmount;

    public int failures;
    private int totalFailures;
    [SerializeField] private TMP_Text finalScore;
    [SerializeField] private TMP_Text correctRate;

    public int correctAnswers;
    private int totalCorrectAnswers;

    private int maxEnergy;
    private int currentEnergy;
    [SerializeField]private int points=0;
    [SerializeField] private Slider energyBar;


    [Header("FORM")]
    [SerializeField] private TMP_InputField inputName;
    [SerializeField] private TMP_InputField inputSurname;
    [SerializeField] private TMP_InputField inputEmail;
    [SerializeField] private TMP_InputField inputNacimiento;
    [SerializeField] private TMP_InputField inputTfno;
    [SerializeField] private TMP_InputField inputEmpresa;
    [SerializeField] private TMP_Dropdown dropdownLaboral;
    [SerializeField] private Toggle privacidad;
    [SerializeField] private Toggle sorteo;

    private bool dialogueIsPlaying;
    private int highlightedAnswer;//for answering the quizz

    private bool dropdownSelected = false;

    private static DialogueManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Dialogue Manager");
        }
        instance = this;

        dialogueSystem = GameObject.FindObjectOfType<DialogueSystem>();

    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private IEnumerator Start()
    {

        currentEnergy =playerData.energy;
        energyBar.value = currentEnergy;//visuals
        points =playerData.score;
        uiAnimator.UpdateScore(points);

        currentQuizzID = playerData.level;
        currentCharacterID = playerData.level;

        uiAnimator.ChangeToNewCharacter(currentCharacterID);
        Debug.Log("Changing to " + currentCharacterID);

        yield return new WaitForSeconds(0.2f);//waiting for server
        
        allQuizzes = dialogueSystem.quizzesList.quizzes;
        currentQuizz = allQuizzes[currentQuizzID];

        dialogueIsPlaying = false;
        answersPanel.SetActive(false);

        //initialize the characters List
        allCharacters = dialogueSystem.characterList.characters;
        currentCharacter = allCharacters[currentCharacterID];

        choicesText = new TextMeshProUGUI[choices.Length];//set the length of the array
        int index = 0;
        foreach(GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }

        maxEnergy = (int)Mathf.Round(energyBar.maxValue);
        currentEnergy = (int)Mathf.Round(energyBar.value);

        StartConversation();
    }


    private void EndDialogue()
    {
        finalScore.text = points.ToString();
        StartCoroutine(dialogueSystem.PostWin());
        dialogueIsPlaying = false;
        answersPanel.SetActive(false);
        dialogueText.text = "";

        scoreScreen.SetActive(true);

        correctRate.text =((totalCorrectAnswers/(totalCorrectAnswers+totalFailures))*100).ToString()+"%";
        Debug.Log("Número de correctas: "+totalCorrectAnswers+"y el número de errores es: "+totalFailures+". Resultado de la operación: "+(totalCorrectAnswers/(totalCorrectAnswers+totalFailures))*100);

    }

    public void NextQuestion()//called from next question button
    {
        nextQuestionButton.SetActive(false);
        int newQuestion;
        
        highlightedAnswer = 99;//none is highlighted
        uiAnimator.ResetColor();
        //set text for the question (selected randomly)
        int numberOfOptions = currentQuizz.questions.Count;//number of options in the list
            
        newQuestion = Random.Range(0,numberOfOptions);

        if (newQuestion == currentQuestion)//to make sure it never repeats
        {
            if (newQuestion != numberOfOptions-1)//if it is not the last in the group
            {
                Debug.Log("The question picked is: " + newQuestion + ", the last one was: " + currentQuestion + ", and the group contains: " + numberOfOptions);
                newQuestion++;//take next one in list
                currentQuestion = newQuestion;
            }
            else//if it is the last, the the first from the list
            {
                currentQuestion = 0;
                Debug.Log("Random selected= " + newQuestion + ", number if options in the list: " + numberOfOptions);
            }
        }
        else
        {
            currentQuestion = newQuestion;//update question if its not de  same
        }

        dialogueText.text = currentQuizz.questions[currentQuestion].questionText;
        //display choices, if any for this question
        DisplayChoices(currentQuestion);
        ActivateAnswers(true);//make them clickable
        

    }

    private void DisplayChoices(int i)
    {

        List<Answer> currentChoices = currentQuizz.questions[i].answers;
        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("More choices than the UI can support. Number of choices given: " + currentChoices.Count);

        }
        int index = 0;//for each answer
        foreach(Answer choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.answerText;
            index++;
        }
        //if there are other options that should not be visible, set them inactive
        for(int j=index; j < choices.Length; j++)
        {
            choices[j].gameObject.SetActive(false);
        }
        answersPanel.SetActive(true);//activate canvas
        uiAnimator.ShowAnswers();
    }

    public void SelectAnswer(int id)
    {
        uiAnimator.ResetColor();
        UnselectAnswers();

        if(id == highlightedAnswer)
        {
            AnswerQuizz(id);
            return;
        }
        else
        {
            uiAnimator.SelectAnswer(id);
            choicesHighlight[id].gameObject.SetActive(true);
            highlightedAnswer = id;
        }
        uiAnimator.ResetCharacterImage();//to go back to previous expression

    }

    public void AnswerQuizz(int id)
    {
        bool correct;
        if (id== currentQuizz.questions[currentQuestion].correctAnswerID)//if answer is correct
        {
            correct = true;
            uiAnimator.ConfirmAnswer(id, Color.green,correct);
            correctAnswers++;//counts it
            totalCorrectAnswers++;
            points += 35;
            UpdateEnergy(10);
            uiAnimator.MarkCorrect(correctAnswers);
            AnswerData aData= new AnswerData(currentQuizz.questions[currentQuestion].questionID,id,correct);
            StartCoroutine(dialogueSystem.PostAnswer(dialogueSystem.usuario,aData));
        }
        else
        {
            correct = false;
            uiAnimator.ConfirmAnswer(id, Color.red,correct);
            uiAnimator.ShowCorrectAnswer(currentQuizz.questions[currentQuestion].correctAnswerID);
            if (points > 2) { points-= 2; }//substract points
            else { points = 0; }//too few points
            UpdateEnergy(-15);
            failures++;//counts it
            totalFailures++;
            AnswerData aData = new AnswerData(currentQuizz.questions[currentQuestion].questionID, id, correct);
            StartCoroutine(dialogueSystem.PostAnswer(dialogueSystem.usuario, aData));

        }
        ActivateAnswers(false);//make answer temporarily unclickable
        uiAnimator.UpdateScore(points);
        //CHEKING WIN OR LOOSE CONDITIONS
        if (currentEnergy<=0)
        {
            defeatScreen.SetActive(true);
            playerData.energy = 0;
            playerData.score = points;
            playerData.level = currentCharacterID;
            StartCoroutine(dialogueSystem.PostFailure("questions"));
            RestartDialogue();
        }
        else if (correctAnswers >= winTriggerAmount)//WIN CONDITION->Trigger Outro
        {
            phaseOfDialogue = "outroNarration";
            uiAnimator.ChangeScreen(phaseOfDialogue);
            indexOfSentence = 0;
            answersPanel.SetActive(false);
            playerData.energy = currentEnergy;
            playerData.score = points;
            playerData.level = currentCharacterID;

            NextSentence();
        }
        else //if category has not ben won or lost, show appropiate reaction.
        {
            NextReaction(correct);
        }
    }

    private void NextCharacter()
    {
        //UI
        uiAnimator.RescaleAnswers();//reset answer boxes for animation


        //QUESTIONS
        currentQuizzID++;//add one to quizz ID to get next one
        currentQuizz = allQuizzes[currentQuizzID];//pick the appropiate quizz
        //NARRATIVE
        if (indexOfSentence != 0) { indexOfSentence = 0; };//resets to start narrative appropiately
        currentCharacterID++;
        currentCharacter = allCharacters[currentCharacterID];//pick the appropiate character

        //reset the counters
        correctAnswers = 0;
        failures = 0;

        //deactivates the answers so nothing is visible
        dialogueIsPlaying = false;
        answersPanel.SetActive(false);

        StartTravelling();

    }

    private void RestartDialogue()//temporary on losing method
    {
        dialogueText.text = "Oh vaya, parece que no has conseguido pasar esta prueba.";
        correctAnswers = 0;
        failures = 0;
        uiAnimator.ResetMarkers();
        currentQuizzID = 0;
        currentQuizz = allQuizzes[currentQuizzID];//pick the appropiate quizz
        dialogueIsPlaying = false;
        answersPanel.SetActive(false);
        character.text = currentQuizz.category;//name of the character/category in the UI

    }

    public void StartConversation()//called from Start and when changing category
    {
        character.text = allCharacters[currentCharacterID].Name;
        dialogueText.text = "";//empty question text

        dialogueIsPlaying = true;
        NextSentence();//To call for first sentence
    }

    public void NextSentence()
    {
        continueButton.SetActive(true);
        if (phaseOfDialogue == "introNarration")//INTRO
        {
            if(indexOfSentence< currentCharacter.introNarration.Count)
            {
                dialogueText.text = currentCharacter.introNarration[indexOfSentence];//display sentence of dialogue
                indexOfSentence++;        //ads to index of text
            }
            else
            {
                phaseOfDialogue = "questions";
                uiAnimator.ChangeScreen(phaseOfDialogue);
                NextQuestion();//show question
                indexOfSentence = 0;//reset index for next category
                uiAnimator.ResetMarkers();//resets checks for answers
                continueButton.SetActive(false);
            }

        }
        else if(phaseOfDialogue == "outroNarration")//OUTRO
        {
            if (indexOfSentence < currentCharacter.outroNarration.Count)
            {
                dialogueText.text = currentCharacter.outroNarration[indexOfSentence];//display sentence of dialogue
                indexOfSentence++;        //ads to index of text
            }
            else if(!currentQuizz.canContinue)
            {
                answersPanel.SetActive(false);//redundant?
                EndDialogue();//END

            }
            else
            {
                answersPanel.SetActive(false);//redundant?

                indexOfSentence = 0;//reset index for next category
                phaseOfDialogue = "introNarration";
                NextCharacter();//show new character
            }
        }
    }

    private void NextReaction(bool correct)
    {
        //random index
        indexOfSentence = Random.Range(0, 5);
        if (correct)
        {
            dialogueText.text = currentCharacter.correctReactions[indexOfSentence];//display sentence of dialogue
        }
        else
        {
            dialogueText.text = currentCharacter.incorrectReactions[indexOfSentence];//display sentence of dialogue
        }
        //show next question button
        nextQuestionButton.SetActive(true);
    }

    private void ActivateAnswers(bool activation)
    {
        for (int index=0; index < choices.Length; index++)
        {
            choices[index].GetComponent<Button>().enabled = activation; ;
        }
    }

    private void UpdateEnergy(int value)
    {
        if(currentEnergy == maxEnergy&&value>0)
        {
            //Debug.Log("MAXED OUT");
            return;//end function
        }

        else if (maxEnergy <= currentEnergy + value)
        {//if adding results in surpasing maxValue
            currentEnergy = maxEnergy;//sets to max value
            //Debug.Log("setting to max");
        }

        else if (value<0 && currentEnergy <= -value)//not enough energy left
        {
            currentEnergy = 0;
            //Debug.Log("LOST because value=:"+value+" and currentEnergy is: "+currentEnergy);//LOOSE
        }
        else
        {
            currentEnergy += value;
        }

        uiAnimator.UpdateEnergyBar(currentEnergy);//passes the current value to update UI

    }
    private void UnselectAnswers()
    {
        choicesHighlight[0].SetActive(false);
        choicesHighlight[1].SetActive(false);
        choicesHighlight[2].SetActive(false);
        choicesHighlight[3].SetActive(false);
    }
    public void DropdownSelected()
    {
        dropdownSelected = true;
    }

    public void SubmitPersonalData()
    {
        string name =inputName.text;
        string email =inputEmail.text;
        string apellido =inputSurname.text;
        string nacimiento = inputNacimiento.text;
        string tfno = inputTfno.text;
        string laboral = dropdownLaboral.captionText.text;
        string empresa = inputEmpresa.text;

        if (name == "" || email == "" || apellido == "" || nacimiento == "" || laboral == "" || !dropdownSelected||!privacidad.isOn||!sorteo.isOn)
        {
            alertScreen.SetActive(true);
            Debug.Log("Required  fields are empty");
            return;
        }
        StartCoroutine(dialogueSystem.SubmitPersonalData(name,apellido, email,nacimiento,tfno,laboral,empresa));
        ShowHomeScreen();

    }
    public void ShowFormScreen()//from winScrenn button
    {
        scoreScreen.SetActive(false);//closes score screen
        ShowHomeScreen();
    }
    public void ShowHomeScreen()//called to restart
    {
        SceneManager.LoadScene(0);
    }

    public void StartTravelling()
    {
        playerData.energy = currentEnergy;
        playerData.score = points;
        playerData.level = currentQuizzID;

        SceneManager.LoadSceneAsync(currentQuizzID+1);//change to appropriate travel scene
    }


}
