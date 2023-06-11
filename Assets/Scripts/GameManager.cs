using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    const int QUESTION_COUNT = 2;
    const int LANE = 2;

    public int[] carSpawnCount = new int[QUESTION_COUNT];
    int[] carType = new int[QUESTION_COUNT];
    int[] redCarCount = new int[QUESTION_COUNT];
    int[] blueCarCount = new int[QUESTION_COUNT];
    int[] silverCarCount = new int[QUESTION_COUNT];
    int[] blackCarCount = new int[QUESTION_COUNT];

    List<string> questions = new List<string>();
    List<string> option = new List<string>();
    List<List<string>> options = new List<List<string>>();
    List<int> correctOptions = new List<int>();

    [SerializeField] GameObject[] spawnPoints = new GameObject[LANE];
    [SerializeField] List<GameObject> cars = new List<GameObject>();
    [SerializeField] GameObject panel;
    [SerializeField] TextMeshProUGUI questionTxt;
    [SerializeField] TextMeshProUGUI[] optionsTxt = new TextMeshProUGUI[4];
    [SerializeField] GameObject[] optionsBtn = new GameObject[4];
    [SerializeField] TextMeshProUGUI playerName;
    [SerializeField] CarDestroyer carDestroyer;
    [SerializeField] GameObject startPanel;
    [SerializeField] GameObject resultPanel;

    List<GameObject> inGameCars = new List<GameObject>();
    List<GameObject> shuffledCars = new List<GameObject>();
    GameObject newCar;
    public int question;
    int carCounter;
    float carSpawnTime = 3f;

    void Start()
    {
        carSpawnCount[0] = 10;
        carSpawnCount[1] = 12;
        carType[0] = 3;
        carType[1] = 4;
        redCarCount[0] = 2;
        redCarCount[1] = 1;
        blueCarCount[0] = 3;
        blueCarCount[1] = 2;
        silverCarCount[0] = 5;
        silverCarCount[1] = 6;
        blackCarCount[0] = 0;
        blackCarCount[1] = 3;
        question = 0;

        questions.Add("Yağız'ın gördüğü arabaların yüzde kaçı kırmızıdır?");
        questions.Add("Esra'nın gördüğü arabaların yüzde kaçı siyahtır?");
        questions.Add("İkisinin de gördüğü arabaların toplam yüzde kaçı gümüştür?");

        option.Add("10");
        option.Add("20");
        option.Add("30");
        option.Add("40");

        option.Add("15");
        option.Add("25");
        option.Add("35");
        option.Add("45");

        option.Add("20");
        option.Add("30");
        option.Add("40");
        option.Add("50");

        for (int i = 0; i < 3; i++)
        {
            List<string> copyList = new List<string>();
            for (int j = i * 4; j < 4 + i * 4; j++) copyList.Add(option[j]);
            options.Add(copyList);
        }

        correctOptions.Add(1);
        correctOptions.Add(1);
        correctOptions.Add(3);
    }

    void CreateCars()
    {
        carCounter = 0;

        for (int color = 0; color < carType[question]; color++)
        {
            switch (color)
            {
                case 0:
                    for (int number = 0; number < redCarCount[question]; number++)
                    {
                        newCar = Instantiate(cars[color]);
                        int rndLane = Random.Range(0, LANE);
                        newCar.transform.SetParent(spawnPoints[rndLane].transform);
                        newCar.transform.localPosition = Vector3.zero;
                        inGameCars.Add(newCar);
                    }
                    break;
                case 1:
                    for (int number = 0; number < blueCarCount[question]; number++)
                    {
                        newCar = Instantiate(cars[color]);
                        int rndLane = Random.Range(0, LANE);
                        newCar.transform.SetParent(spawnPoints[rndLane].transform);
                        newCar.transform.localPosition = Vector3.zero;
                        inGameCars.Add(newCar);
                    }
                    break;
                case 2:
                    for (int number = 0; number < silverCarCount[question]; number++)
                    {
                        newCar = Instantiate(cars[color]);
                        int rndLane = Random.Range(0, LANE);
                        newCar.transform.SetParent(spawnPoints[rndLane].transform);
                        newCar.transform.localPosition = Vector3.zero;
                        inGameCars.Add(newCar);
                    }
                    break;
                case 3:
                    for (int number = 0; number < blackCarCount[question]; number++)
                    {
                        newCar = Instantiate(cars[color]);
                        int rndLane = Random.Range(0, LANE);
                        newCar.transform.SetParent(spawnPoints[rndLane].transform);
                        newCar.transform.localPosition = Vector3.zero;
                        inGameCars.Add(newCar);
                    }
                    break;
            }
        }

        for(int i = 0; i < carSpawnCount[question]; i++)
        {
            int rndCar = Random.Range(0, inGameCars.Count);
            shuffledCars.Add(inGameCars[rndCar]);
            inGameCars.Remove(inGameCars[rndCar]);
        }

        StartCoroutine(SpawnCar());
    }

    IEnumerator SpawnCar()
    {
        shuffledCars[carCounter].GetComponent<Car>().moveable = true;
        yield return new WaitForSeconds(carSpawnTime);
        carCounter++;
        if (carCounter < carSpawnCount[question]) StartCoroutine(SpawnCar());
    }

    public void SetPanel()
    {
        panel.SetActive(true);
        questionTxt.text = questions[question];
        optionsTxt[0].text = options[question][0];
        optionsTxt[1].text = options[question][1];
        optionsTxt[2].text = options[question][2];
        optionsTxt[3].text = options[question][3];
    }

    public void StartGame()
    {
        Destroy(startPanel);
        CreateCars();
    }

    public void CheckAnswer(int optionNumber)
    {
        bool answerSelected = false;
        for (int i = 0; i < 4; i++)
        {
            if (optionsBtn[i].GetComponent<Image>().color != Color.white)
            {
                answerSelected = true;
                break;
            }
        }
        if (!answerSelected)
        {
            if (correctOptions[question] == optionNumber)
            {
                question++;
                optionsBtn[optionNumber].GetComponent<Image>().color = Color.green;
                if (question < 3) StartCoroutine(NextQuestion(optionNumber));
                else resultPanel.SetActive(true);
            }
            else
            {
                optionsBtn[optionNumber].GetComponent<Image>().color = Color.red;
                StartCoroutine(TurnWhite(optionNumber));
            }
        }
    }

    IEnumerator TurnWhite(int optionNumber)
    {
        yield return new WaitForSeconds(1f);
        optionsBtn[optionNumber].GetComponent<Image>().color = Color.white;
    }

    IEnumerator NextQuestion(int optionNumber)
    {
        yield return new WaitForSeconds(1f);
        optionsBtn[optionNumber].GetComponent<Image>().color = Color.white;
        panel.SetActive(false);
        if (question == 1)
        {
            shuffledCars.Clear();
            CreateCars();
            playerName.text = "Esra";
            carDestroyer.destroyedCar = 0;
        }
        else if(question == 2)
        {
            SetPanel();
        }
    }
}
