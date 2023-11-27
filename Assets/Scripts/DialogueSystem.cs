using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using Proyecto26;
using System;
using System.Text;

public class DialogueSystem : MonoBehaviour
{
    public Quizzes quizzesList;
    public Character characterList;
    public Usuario usuario;

    [SerializeField] private PersistentData_SO playerData;

    public static DialogueSystem Instance;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);//to make it persistent between scenes
    }

    private void Start()
    {
        string userName = GenerateRandomString(12);//creating random identifier
        usuario = new Usuario(00, userName);

        StartCoroutine(MakePostRequest(usuario));
        StartCoroutine(LoadFromJSONChar());
        StartCoroutine(LoadFromJSONQuiz());
    }



    public IEnumerator MakePostRequest(Usuario usuario)

    {
        // The URL of the API endpoint you want to send the PUT request to
        string url = "https://users.json";//to store all the users

        // Create a UnityWebRequest with the HTTP method set to POST

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {

            // Set the timestamp to the current date and time in a readable format
            usuario.timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

            // The data you want to send in the request (in this example, we're sending a JSON payload)
            string jsonPayload = "{\"" + usuario.userName + "\": {" +
                                "\"ID\": \"" + usuario.ID + "\"," +//this ID can track a single session
                                "\"timestamp\": \"" + usuario.timestamp + "\"" + // Include the timestamp as a string
                                "}}";

            // Convert the JSON payload to bytes
            byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(jsonPayload);

            // Set the upload handler to send the JSON data
            request.uploadHandler = new UploadHandlerRaw(jsonBytes);
            request.uploadHandler.contentType = "application/json";

            // Set the download handler to receive the response
            request.downloadHandler = new DownloadHandlerBuffer();

            // Send the PUT request
            yield return request.SendWebRequest();

            // Check for errors
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("POST Request Error: " + request.error);
            }
            else
            {
                /*Debug.Log("POST Request Successful");
                Debug.Log("Response: " + request.downloadHandler.text);*/
            }
        }
    }


    public IEnumerator PostAnswer(Usuario user, AnswerData answerData)

    {
        // The URL of the API endpoint you want to send the PUT request to
        string url = "https://answers.json";//JSON to store answer info

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {

            // Set the timestamp to the current date and time in a readable format
            answerData.timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

            // The data you want to send in the request (in this example, we're sending a JSON payload)
            string jsonPayload = "{\"" + user.userName + "\": {" +
                                "\"qID\": \"" + answerData.questionID + "\"," +
                                "\"aID\": \"" + answerData.answerID + "\"," +
                                "\"correct\": \"" + answerData.correctAnswer + "\"," +
                                "\"timestamp\": \"" + answerData.timestamp + "\"" + // Include the timestamp as a string
                                "}}";

            // Convert the JSON payload to bytes
            byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(jsonPayload);

            // Set the upload handler to send the JSON data
            request.uploadHandler = new UploadHandlerRaw(jsonBytes);
            request.uploadHandler.contentType = "application/json";

            // Set the download handler to receive the response
            request.downloadHandler = new DownloadHandlerBuffer();

            // Send the PUT request
            yield return request.SendWebRequest();

            // Check for errors
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("POST Request Error: " + request.error);
            }
            else
            {
                //Debug.Log("POST Request Successful");
                //Debug.Log("Response: " + request.downloadHandler.text);// You can access the response data using request.downloadHandler.text
            }
        }
    }



    public IEnumerator PostFailure(string phase)

    {
        // The URL of the API endpoint you want to send the PUT request to
        string url = "https://failures.json";//JSON to store loose data

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {

            // Set the timestamp to the current date and time in a readable format
            string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

            string jsonPayload = "{\"" + usuario.userName + "\": {" +
                                "\"lvl\": \"" + playerData.level + "\"," +
                                "\"pts\": \"" + playerData.score + "\"," +
                                "\"phase\": \"" + phase + "\","+
                                "\"timestamp\": \"" + timestamp + "\"" + // Include the timestamp as a string
                                "}}";

            // Convert the JSON payload to bytes
            byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(jsonPayload);

            // Set the upload handler to send the JSON data
            request.uploadHandler = new UploadHandlerRaw(jsonBytes);
            request.uploadHandler.contentType = "application/json";

            // Set the download handler to receive the response
            request.downloadHandler = new DownloadHandlerBuffer();

            // Send the PUT request
            yield return request.SendWebRequest();

            // Check for errors
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("POST Request Error: " + request.error);
            }
            else
            {

            }
        }
    }

    public IEnumerator PostWin()

    {
        // The URL of the API endpoint you want to send the PUT request to
        string url = "https://wins.json";//JSON to store win data

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {

            // Set the timestamp to the current date and time in a readable format
            string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");


            string jsonPayload = "{\"" + usuario.userName + "\": {" +
                                "\"lvl\": \"" + playerData.level + "\"," +
                                "\"pts\": \"" + playerData.score + "\"," +
                                "\"timestamp\": \"" + timestamp + "\"" + // Include the timestamp as a string
                                "}}";

            // Convert the JSON payload to bytes
            byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(jsonPayload);

            // Set the upload handler to send the JSON data
            request.uploadHandler = new UploadHandlerRaw(jsonBytes);
            request.uploadHandler.contentType = "application/json";

            // Set the download handler to receive the response
            request.downloadHandler = new DownloadHandlerBuffer();

            // Send the PUT request
            yield return request.SendWebRequest();

            // Check for errors
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("POST Request Error: " + request.error);
            }
            else
            {

            }
        }
    }


    public IEnumerator SubmitPersonalData(string name, string surname, string email, string nacimiento, string tfno, string laboral, string empresa)

    {
        // The URL of the API endpoint you want to send the PUT request to
        string url = "https://sorteo.json";//contains the info for the winners that submit data

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {

            // The data you want to send in the request (in this example, we're sending a JSON payload)
            string jsonPayload = "{\"name\": \"" + name + "\"," +
                            "\"surname\": \"" + surname + "\"," +
                            "\"email\": \"" + email + "\"," +
                            "\"fecha nacimiento\": \"" + nacimiento + "\"," +
                            "\"tfno\": \"" + tfno + "\"," +
                            "\"laboral\": \"" + laboral + "\"," +
                            "\"empresa\": \"" + empresa + "\"" +
                            "}";

            // Convert the JSON payload to bytes
            byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(jsonPayload);

            // Set the upload handler to send the JSON data
            request.uploadHandler = new UploadHandlerRaw(jsonBytes);
            request.uploadHandler.contentType = "application/json";

            // Set the download handler to receive the response
            request.downloadHandler = new DownloadHandlerBuffer();

            // Send the PUT request
            yield return request.SendWebRequest();

            // Check for errors
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("POST Request Error: " + request.error);
            }
            else
            {
                //Debug.Log("POST Request Successful");
                Debug.Log("Data Uploaded: " + request.downloadHandler.text);// You can access the response data using request.downloadHandler.text
            }
        }
    }




     IEnumerator LoadFromJSONChar()
    {
        var filePath = Path.Combine(Application.streamingAssetsPath, "char.json");

        UnityWebRequest www = UnityWebRequest.Get(filePath);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error loading JSON file: " + www.error);
        }
        else
        {
            string jsonContent = www.downloadHandler.text;

            // Parse the JSON data as needed.
            characterList = JsonUtility.FromJson<Character>(jsonContent);
            www.Dispose();
        }
      
        //Debug.Log("Character Loading Complete");
    }

    IEnumerator LoadFromJSONQuiz()
    {
        var filePath = Path.Combine(Application.streamingAssetsPath, "quiz.json");

        UnityWebRequest www = UnityWebRequest.Get(filePath);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error loading JSON file: " + www.error);
        }
        else
        {
            string jsonContent = www.downloadHandler.text;

            // Parse the JSON data as needed.
            quizzesList = JsonUtility.FromJson<Quizzes>(jsonContent);
            www.Dispose();
        }

    }

    public static string GenerateRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";//the characters allowed
        StringBuilder stringBuilder = new StringBuilder(length);

        for (int i = 0; i < length; i++)
        {
            int index = UnityEngine.Random.Range(0, chars.Length);
            stringBuilder.Append(chars[index]);
        }

        return stringBuilder.ToString();
    }
}

[System.Serializable]
public class Quizzes
{
    public List<Quizz> quizzes = new List<Quizz>();
}


[System.Serializable]
public class Quizz
{
    public int ID;
    public string category;
    public List<Question> questions = new List<Question>();
    public bool canContinue;

}

[System.Serializable]
public class Question
{
    public int questionID;
    public string questionText;
    public List<Answer> answers = new List<Answer>();
    public int correctAnswerID;
}

[System.Serializable]
public class Answer
{
    public int answerID;
    public string answerText;
}

[System.Serializable]
public class Character    
{
    public List<NarrativeCharacter> characters = new List<NarrativeCharacter>();
}


[System.Serializable]
public class NarrativeCharacter
{
    public int ID;
    public string Name;
    public List<string> introNarration = new List<string>();//this are the first sentences, in order
    public List<string> correctReactions = new List<string>();//these are selected at random on correct answer
    public List<string> incorrectReactions = new List<string>();//these are selected at random on incorrect answer
    public List<string> outroNarration = new List<string>();//this are the last sentences, in order
}

[System.Serializable]
public class Usuario
{
    public int ID { get; set; }// Property for ID
    public string userName { get; set; }// Property for Name
    public string timestamp; // Timestamp field
    public Usuario(int id, string name)// Constructor
    {
        ID = id;
        userName = name;
        this.timestamp = ""; // Initialize with 0 or set it to the server timestamp when adding data.
    }
}

[System.Serializable]
public class AnswerData
{
    public int questionID { get; set; }// Property for ID
    public int answerID;
    public bool correctAnswer;
    public string timestamp; // Timestamp field

    public AnswerData(int qID,int aID,bool cAnswer)
    {
        questionID = qID;
        answerID = aID;
        correctAnswer = cAnswer;
    }

}





