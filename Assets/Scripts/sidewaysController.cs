using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class sidewaysController : MonoBehaviour
{
    private DialogueSystem dialogueSystem;
    [SerializeField] private Slider horizontalSlider; // Reference to the horizontal slider in the Inspector.
    public float lerpSpeed = 5f;   // Speed of lerping between positions.

    private Vector3 targetPosition; // The position to lerp towards.
    [SerializeField] private Slider energySlider;
    [SerializeField] private TMP_Text scoreText;

    public int playerEnergy;
    private Collider playerCollider;
    private bool canCollide=true;

    private int score;
    private int level;
    [SerializeField] private PersistentData_SO playerData;

    [SerializeField] private GameObject bg;
    [SerializeField] private GameObject defeatScreen;

    private void Awake()
    {
        dialogueSystem = GameObject.FindObjectOfType<DialogueSystem>();
    }

    private void Start()
    {
        playerCollider=this.GetComponent<Collider>();
        score = playerData.score;
        scoreText.text = score.ToString();
        playerEnergy = playerData.energy;
        level = playerData.level;
        energySlider.value = playerEnergy;

    }

    private void Update()
    {
        // Get the current position of the slider and map it to the desired range.
        float sliderValue = horizontalSlider.value;
        float minX = horizontalSlider.minValue;
        float maxX = horizontalSlider.maxValue;
        float targetX = Mathf.Lerp(minX, maxX, (sliderValue + 2.3f) / 4.6f);

        // Set the target position for lerping.
        targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);

        // Move the player towards the target position using Lerp.
        transform.position = Vector3.Lerp(transform.position, targetPosition, lerpSpeed * Time.deltaTime);

    }

    private IEnumerator LooseEnergy(int energyLoss)
    {
        if (canCollide)
        {
            if (playerEnergy - energyLoss > 0)
            {
                playerEnergy -= energyLoss;
                energySlider.value = playerEnergy;
                canCollide = false;
                Debug.Log("Can not collide for 1 second");
                yield return new WaitForSeconds(1);
                canCollide = true;
                Debug.Log("Can collide again");
            }
            else
            {
                Debug.Log("YouHaveLost");
                StartCoroutine(dialogueSystem.PostFailure("travel"));
                bg.GetComponent<BGMove>().moveSpeed = 0;
                defeatScreen.SetActive(true);

            }

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string collidedObjectName = collision.gameObject.name;
        Debug.Log("Collision with " + collision.gameObject.name);
        // Check if the collision involves the player character.
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            StartCoroutine(LooseEnergy(15));
            StartCoroutine(FlashRed());

        }
        if (collision.gameObject.CompareTag("Finish"))
        {
            EncounterNewBoss();
        }
        if (collision.gameObject.CompareTag("PickUp"))
        {
            CatchPickUp(collision.gameObject);
            collision.gameObject.SetActive(false);
        }

    }

    private void CatchPickUp(GameObject pickUp)
    {
        int energy = pickUp.GetComponent<PickUp>().enegyBoost;
        int points = pickUp.GetComponent<PickUp>().points;

        

        if (energy > 0)
        {
            if (playerEnergy == 100)//if it is at max
            {
            }

            else if (playerEnergy + energy <= 100)//if it is not going to overload
            {
                playerEnergy += energy;
                energySlider.value = playerEnergy;//visuals
            }
            else//would overload
            {
                playerEnergy = 100;
                energySlider.value = playerEnergy;//visuals
            }
        }
        if (points > 0)
        {
            score += points;
            scoreText.text = score.ToString();
        }

    }

    private void EncounterNewBoss()
    {
        playerData.energy =playerEnergy;
        playerData.score = score;

        SceneManager.LoadSceneAsync(5);//to dialogue scene
    }
    private IEnumerator FlashRed()
    {
        var spriteRend = this.GetComponent<SpriteRenderer>();
        spriteRend.color = Color.red;
        yield return new WaitForSeconds(1);
        spriteRend.color = Color.white;

    }
  
}
