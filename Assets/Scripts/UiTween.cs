using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiTween : MonoBehaviour
{
    [SerializeField]    private GameObject enemyName, enemyDialogueBox, enemyImage, continueButton, bgPreguntas, bgDialogo;

    [SerializeField]    private List<GameObject> answerBoxes=new List<GameObject>();

    private string characterExpressionImage = "Angry";
    private int characterImageIndex = 1;

    [SerializeField] private SpriteAtlasManager characterPortrait;

    [SerializeField] private Slider energyBar;

    [SerializeField] private GameObject aciertoMark01, aciertoMark02, aciertoMark03;

    [SerializeField] private TextMeshProUGUI scoreUI;
    [SerializeField] private TextMeshProUGUI enemyText,score,pts;

    private Vector2 characterQuestionsScale = new Vector2(0.8f, 0.8f);
    private Vector2 characterDialogueScale = new Vector2(1f, 1f);


    private void Awake()
    {
        enemyName.transform.localScale = Vector2.zero;
        enemyDialogueBox.transform.localScale = Vector2.zero;

        RescaleAnswers();
    }


    private void Start()
    {
        LeanTween.scale(enemyName, new Vector2(1f, 1f), 0.2f).setDelay(.5f).setEase(LeanTweenType.easeOutSine);
        LeanTween.scale(enemyDialogueBox, new Vector2(1f, 1f), 0.2f).setDelay(.6f).setEase(LeanTweenType.easeOutSine);
    }

    public void ShowAnswers()
    {
        LeanTween.scale(answerBoxes[0], new Vector2(1f, 1f), 0.2f).setDelay(.3f).setEase(LeanTweenType.easeOutSine);
        LeanTween.scale(answerBoxes[1], new Vector2(1f, 1f), 0.2f).setDelay(.4f).setEase(LeanTweenType.easeOutSine);
        LeanTween.scale(answerBoxes[2], new Vector2(1f, 1f), 0.2f).setDelay(.5f).setEase(LeanTweenType.easeOutSine);
        LeanTween.scale(answerBoxes[3], new Vector2(1f, 1f), 0.2f).setDelay(.6f).setEase(LeanTweenType.easeOutSine);
    }

    public void RescaleAnswers()
    {
        answerBoxes[0].transform.localScale = Vector2.zero;
        answerBoxes[1].transform.localScale = Vector2.zero;
        answerBoxes[2].transform.localScale = Vector2.zero;
        answerBoxes[3].transform.localScale = Vector2.zero;
    }

    public void SelectAnswer(int index)//changing the aspect when selecting one of the options
    {
        //invert the color of the background and typography
        LeanTween.color(answerBoxes[index].GetComponent<Image>().rectTransform, Color.black, 0.6f);
        answerBoxes[index].GetComponentInChildren<TextMeshProUGUI>().color=Color.white;//change text color

        LeanTween.scale(answerBoxes[index], new Vector2(1.05f, 1.05f), 1f).setEase(LeanTweenType.punch);
    }
    public void ConfirmAnswer(int index, Color color,bool correct)//confirm the selected answer
    {
        //if correct, changes BG to green. If not, to red
        LeanTween.color(answerBoxes[index].GetComponent<Image>().rectTransform, color, 0.6f);
        LeanTween.scale(answerBoxes[index], new Vector2(1.1f, 1.1f), 0.15f).setEase(LeanTweenType.easeOutSine);


        //after that, character should react
        if (correct)
        {
            characterExpressionImage = "Happy";         
        }
        else
        {
            characterExpressionImage="Surprised";
        }
        characterPortrait.ChangeExpression(characterImageIndex,characterExpressionImage);
        LeanTween.scale(enemyImage, characterQuestionsScale + new Vector2(0.1f, 0.1f), 1f).setEase(LeanTweenType.punch);//jump image
    }

    public void ShowCorrectAnswer(int index)
    {
        LeanTween.color(answerBoxes[index].GetComponent<Image>().rectTransform, Color.cyan, 1.2f);
    }

    public void ResetColor()
    {
        LeanTween.color(answerBoxes[0].GetComponent<Image>().rectTransform, Color.white, 0.2f);
        answerBoxes[0].GetComponentInChildren<TextMeshProUGUI>().color = Color.black;//change text color
        LeanTween.color(answerBoxes[1].GetComponent<Image>().rectTransform, Color.white, 0.2f);
        answerBoxes[1].GetComponentInChildren<TextMeshProUGUI>().color = Color.black;//change text color
        LeanTween.color(answerBoxes[2].GetComponent<Image>().rectTransform, Color.white, 0.2f);
        answerBoxes[2].GetComponentInChildren<TextMeshProUGUI>().color = Color.black;//change text color
        LeanTween.color(answerBoxes[3].GetComponent<Image>().rectTransform, Color.white, 0.2f);
        answerBoxes[3].GetComponentInChildren<TextMeshProUGUI>().color = Color.black;//change text color
    }


    public void ResetCharacterImage()//to go back to neutral expression
    {
        if (characterExpressionImage != "Angry")//check if it is already in neutral state
        {
            characterPortrait.ChangeExpression(characterImageIndex, "Angry");//change the image
            characterExpressionImage = "Angry";//save the current expression
            LeanTween.scale(enemyImage, characterQuestionsScale+new Vector2(0.1f,0.1f), 1f).setEase(LeanTweenType.punch);//jump image
        }        
    }
    public void ChangeToNewCharacter(int ID)
    {
        characterImageIndex = ID;
        characterPortrait.ChangeExpression(characterImageIndex, "Angry");
        characterExpressionImage = "Angry";
        LeanTween.scale(enemyImage, characterDialogueScale + new Vector2(0.1f, 0.1f), 1f).setEase(LeanTweenType.punch);//jump image
    }

    public void RestartCharacters()
    {
        characterPortrait.ChangeExpression(1, "Angry");//change the image
        characterExpressionImage = "Angry";//save the current expression
        LeanTween.scale(enemyImage, characterDialogueScale, 1f).setEase(LeanTweenType.punch);//jump image
    }

    public void UpdateEnergyBar(int currentEnergy)
    {
        int t = 1;
        float timeElapsed = 0;
        float smoothedValue = energyBar.value;

        if (timeElapsed < t)
        {
            smoothedValue = Mathf.SmoothStep(energyBar.value, currentEnergy, t);
            timeElapsed += Time.deltaTime;
        }
        else { smoothedValue = currentEnergy; };

        energyBar.value = smoothedValue;
    }

    public void MarkCorrect(int i)
    {
        switch (i)
        {
            case 1:
                aciertoMark01.transform.GetChild(0).gameObject.SetActive(true);
                LeanTween.scale(aciertoMark01,new Vector2(1.3f, 1.3f), 1.5f).setEase(LeanTweenType.punch);//change scale
                break;
            case 2:
                aciertoMark02.transform.GetChild(0).gameObject.SetActive(true);
                LeanTween.scale(aciertoMark02, new Vector2(1.3f, 1.3f), 1.5f).setEase(LeanTweenType.punch);//change scale
                break;
            case 3:
                aciertoMark03.transform.GetChild(0).gameObject.SetActive(true);
                LeanTween.scale(aciertoMark03, new Vector2(1.3f, 1.3f), 1.5f).setEase(LeanTweenType.punch);//change scale
                break;
        }
    }
    public void ResetMarkers()
    {
        aciertoMark01.transform.GetChild(0).gameObject.SetActive(false);
        aciertoMark02.transform.GetChild(0).gameObject.SetActive(false);
        aciertoMark03.transform.GetChild(0).gameObject.SetActive(false);
    }

    public void UpdateScore(int points)
    {
        scoreUI.text = points.ToString();
    }

    public void ChangeScreen(string phase)
    {
        switch (phase)
        {
            case "questions":
                bgPreguntas.SetActive(true);
                bgDialogo.SetActive(false);
                LeanTween.moveLocalX(enemyImage, -335, 1).setEaseInOutQuart();
                LeanTween.scale(enemyImage, characterQuestionsScale, 1).setEaseInOutQuart();
                enemyDialogueBox.GetComponent<Image>().color= Color.white;
                enemyText.color = Color.black;
                enemyName.GetComponent<TextMeshProUGUI>().color= Color.black;
                score.color = Color.black;
                pts.color = Color.black;
                continueButton.GetComponent<Image>().color = Color.black;
                continueButton.GetComponentInChildren< TextMeshProUGUI >().color= Color.white;

                break;
            default:
                bgDialogo.SetActive(true);
                bgPreguntas.SetActive(false);

                LeanTween.moveLocalX(enemyImage, 0, 1).setEaseInOutQuart();
                LeanTween.scale(enemyImage, characterDialogueScale, 1).setEaseInOutQuart();
                enemyDialogueBox.GetComponent<Image>().color = Color.black;
                enemyText.color = Color.white;
                enemyName.GetComponent<TextMeshProUGUI>().color = Color.white;
                score.color = Color.white;
                pts.color = Color.white;
                continueButton.GetComponent<Image>().color = Color.white;
                continueButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;

                break;
        }
    }

}

