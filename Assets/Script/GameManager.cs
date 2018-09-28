using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class GameManager : MonoBehaviour
{
    public static int Mode = 0;

    public GameObject Harpoon;
    public Text question;
    public GameObject timeLimit;
    public GameObject score;
    [SerializeField]
    private GameObject fishPrefab;
    [SerializeField]
    private Canvas fishCanvas;
    public GameObject LosePanel;
    public GameObject highScoreText;
    public GameObject Sprite;
    public GameObject Water;

    int difficulty;
    public int level;
    int num_1, num_2, num_3;
    public float ans;
    int numOperator;
    public bool newQuestion;

    public Vector2 touchStartPos;
    private Vector2 buffer1;
    private float gradient1;
    private float constant1;
    public Vector2 touchEndPos;
    private Vector2 buffer2;
    private float gradient2;
    private float constant2;
    public Vector2 touchDestination;
    private float move;
    public bool directionChosen = false;
    public bool calc = false;
    public bool play = true;
    public int totalFish = 0;
    public bool canControl = true;
    public int scoreValue = 0;
    private bool saved = false;

    // Use this for initialization
    void Start()
    {
        level = 1;
        newQuestion = true;
        if (Mode == 0)
        {
            timeLimit.SetActive(false);
            score.SetActive(false);
        }
        touchEndPos = new Vector2(9999, 9999);
        Harpoon.GetComponent<Rigidbody2D>().gravityScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mode == 0) //story mode
        {
            if(canControl)
            {
                touchControl();
                touchDebug();
            }
            if (level == 1)
            {
                num_1 = 9;
                num_2 = 3;
                ans = num_1 + num_2;
                question.text = num_1 + " + " + num_2 + " = ?";
            }
        }
        else if (Mode == 1)// arcade mode
        {
            
            score.GetComponent<Text>().text = "Score : " + scoreValue;
            if (play)
            {
                if (canControl)
                {
                    touchControl();
                    touchDebug();
                }
                if (newQuestion == true)
                {
                    destroyAllFish();
                    totalFish = 0;
                    if (level <= 5)
                    {
                        difficulty = Random.Range(0, 3);
                        numOperator = Random.Range(0, 2);
                        PlusMinus();
                    }
                    else if (level >= 5 && level <= 10)
                    {
                        difficulty = Random.Range(0, 2);
                        numOperator = Random.Range(0, 2);
                        ProductDivide();
                    }
                    else if (level >= 11 && level <= 15)
                    {
                        difficulty = Random.Range(0, 2);
                        numOperator = Random.Range(0, 4);
                        MixProductPlus();
                    }
                    else if (level >= 16 && level <= 20)
                    {
                        difficulty = Random.Range(0, 2);
                        numOperator = Random.Range(0, 4);
                        MixProductPlus();
                    }
                    else
                    {
                        int x = Random.Range(0, 4);
                        if (x == 0)
                        {
                            difficulty = Random.Range(0, 3);
                            numOperator = Random.Range(0, 2);
                            PlusMinus();
                        }
                        else if (x == 1)
                        {
                            difficulty = Random.Range(0, 2);
                            numOperator = Random.Range(0, 2);
                            ProductDivide();
                        }
                        else if (x == 2)
                        {
                            difficulty = Random.Range(0, 2);
                            numOperator = Random.Range(0, 4);
                            MixProductPlus();
                        }
                        else if (x == 3)
                        {
                            difficulty = Random.Range(0, 2);
                            numOperator = Random.Range(0, 4);
                            MixProductPlus();
                        }
                    }
                    newQuestion = false;
                }
            }
            else if (!play)
            {
                    if (!Directory.Exists(Path.Combine(Application.persistentDataPath,"High Score.txt")))
                    {
                        // Directory.CreateDirectory("Assets\\Resources");
                        Directory.CreateDirectory(Application.persistentDataPath); 
                    }
                    if (!File.Exists(Path.Combine(Application.persistentDataPath, "High Score.txt")))
                    {
                        Debug.Log("masuk 1");
                        saveHighScore();
                        StreamReader sr = new StreamReader(Path.Combine(Application.persistentDataPath, "High Score.txt"));
                        string s = sr.ReadLine();
                        destroyAllFish();
                        Sprite.SetActive(false);
                        LosePanel.SetActive(true);
                        highScoreText.GetComponent<Text>().text = "High Score : " + s;
                    }
                    else
                    {
                        StreamReader sr = new StreamReader(Path.Combine(Application.persistentDataPath, "High Score.txt"));
                        string s = sr.ReadLine();
                        int highScore = int.Parse(s);
                        sr.Close();
                        if (highScore < scoreValue && !saved)
                        {
                            saved = true;
                            saveHighScore();
                        }
                        StreamReader read = new StreamReader(Path.Combine(Application.persistentDataPath, "High Score.txt"));
                        string scr = read.ReadLine();
                        destroyAllFish();
                        Sprite.SetActive(false);
                        LosePanel.SetActive(true);
                        highScoreText.GetComponent<Text>().text = "High Score : " + scr;
                    }
                }
            } 
        }

    void PlusMinus()
    {
        if (difficulty == 0 && numOperator == 0)
        {
            num_1 = Random.Range(0, 101);
            num_2 = Random.Range(0, 101);
            ans = num_1 + num_2;
            question.text = num_1 + " + " + num_2 + " = ?";
            spawnFishPlusMinus();
        }
        else if (difficulty == 0 && numOperator == 1)
        {
            num_1 = Random.Range(0, 101);
            num_2 = Random.Range(0, 101);
            ans = num_1 - num_2;
            question.text = num_1 + " - " + num_2 + " = ?";
            spawnFishPlusMinus();
        }
        else if (difficulty == 1 && numOperator == 0)
        {
            num_1 = Random.Range(0, 101);
            num_2 = Random.Range(0, 101);
            num_3 = Random.Range(0, 101);
            ans = num_1 - num_2 + num_3;
            question.text = num_1 + " - " + num_2 + " + " + num_3 + " = ?";
            spawnFishPlusMinus();
        }
        else if (difficulty == 1 && numOperator == 1)
        {
            num_1 = Random.Range(0, 101);
            num_2 = Random.Range(0, 101);
            num_3 = Random.Range(0, 101);
            ans = num_1 + num_2 - num_3;
            question.text = num_1 + " + " + num_2 + " - " + num_3 + " = ?";
            spawnFishPlusMinus();
        }
        else if (difficulty == 2 && numOperator == 0)
        {
            num_1 = Random.Range(0, 101);
            num_2 = Random.Range(0, 101);
            num_3 = Random.Range(0, 101);
            ans = num_1 + num_2 + num_3;
            question.text = num_1 + " + " + num_2 + " + " + num_3 + " = ?";
            spawnFishPlusMinus();
        }
        else if (difficulty == 2 && numOperator == 1)
        {
            num_1 = Random.Range(0, 101);
            num_2 = Random.Range(0, 101);
            num_3 = Random.Range(0, 101);
            ans = num_1 - num_2 - num_3;
            question.text = num_1 + " - " + num_2 + " - " + num_3 + " = ?";
            spawnFishPlusMinus();
        }
    } //anything below level 5

    void ProductDivide()
    {
        if (difficulty == 0 && numOperator == 0)
        {
            num_1 = Random.Range(0, 11);
            num_2 = Random.Range(0, 11);
            ans = num_1 * num_2;
            question.text = num_1 + " x " + num_2 + " = ?";
            spawnFishProductDivide();
        }
        else if (difficulty == 0 && numOperator == 1)
        {
            num_1 = Random.Range(0, 11);
            num_2 = Random.Range(0, 11);
            ans = num_1 / num_2;
            question.text = num_1 + " / " + num_2 + " = ?";
            spawnFishProductDivide();
        }
        else if (difficulty == 1 && numOperator == 0)
        {
            num_1 = Random.Range(0, 11);
            num_2 = Random.Range(0, 11);
            num_3 = Random.Range(0, 11);
            ans = (float)num_1 * num_2 / num_3;
            question.text = num_1 + " x " + num_2 + " / " + num_3 + " = ?";
            spawnFishProductDivide();
        }
        else if (difficulty == 1 && numOperator == 1)
        {
            num_1 = Random.Range(0, 11);
            num_2 = Random.Range(0, 11);
            num_3 = Random.Range(0, 11);
            ans = (float)num_1 / num_2 * num_3;
            question.text = num_1 + " / " + num_2 + " x " + num_3 + " = ?";
            spawnFishProductDivide();
        }
    } //note* : can add productxproduct and dividexdivide + lvl 6-10

    void MixProductPlus()
    {
        if (difficulty == 0 && numOperator == 0)
        {
            num_1 = Random.Range(0, 11);
            num_2 = Random.Range(0, 11);
            num_3 = Random.Range(0, 11);
            ans = (float)num_1 * num_2 + num_3;
            question.text = num_1 + " x " + num_2 + " + " + num_3 + " = ?";
            spawnFishMixProductPlus();
        }
        else if (difficulty == 0 && numOperator == 1)
        {
            num_1 = Random.Range(0, 11);
            num_2 = Random.Range(0, 11);
            num_3 = Random.Range(0, 11);
            ans = (float)num_1 * num_2 - num_3;
            question.text = num_1 + " x " + num_2 + " - " + num_3 + " = ?";
            spawnFishMixProductPlus();
        }
        else if (difficulty == 0 && numOperator == 2)
        {
            num_1 = Random.Range(0, 11);
            num_2 = Random.Range(0, 11);
            num_3 = Random.Range(0, 11);
            ans = (float)num_1 / num_2 + num_3;
            question.text = num_1 + " / " + num_2 + " + " + num_3 + " = ?";
            spawnFishMixProductPlus();
        }
        else if (difficulty == 0 && numOperator == 3)
        {
            num_1 = Random.Range(0, 11);
            num_2 = Random.Range(0, 11);
            num_3 = Random.Range(0, 11);
            ans = (float)num_1 / num_2 - num_3;
            question.text = num_1 + " / " + num_2 + " - " + num_3 + " = ?";
            spawnFishMixProductPlus();
        }
        else if (difficulty == 1 && numOperator == 0)
        {
            num_1 = Random.Range(0, 11);
            num_2 = Random.Range(0, 11);
            num_3 = Random.Range(0, 11);
            ans = num_2 * num_3;
            ans = (float)num_1 + ans;
            question.text = num_1 + " + " + num_2 + " x " + num_3 + " = ?";
            spawnFishMixProductPlus();
        }
        else if (difficulty == 1 && numOperator == 1)
        {
            num_1 = Random.Range(0, 11);
            num_2 = Random.Range(0, 11);
            num_3 = Random.Range(0, 11);
            ans = (float)num_2 * num_3;
            ans = (float)num_1 - ans;
            question.text = num_1 + " - " + num_2 + " x " + num_3 + " = ?";
            spawnFishMixProductPlus();
        }
        else if (difficulty == 1 && numOperator == 2)
        {
            num_1 = Random.Range(0, 11);
            num_2 = Random.Range(0, 11);
            num_3 = Random.Range(0, 11);
            ans = (float)num_2 / num_3;
            ans = (float)num_1 + ans;
            question.text = num_1 + " + " + num_2 + " / " + num_3 + " = ?";
            spawnFishMixProductPlus();
        }
        else if (difficulty == 1 && numOperator == 3)
        {
            num_1 = Random.Range(0, 11);
            num_2 = Random.Range(0, 11);
            num_3 = Random.Range(0, 11);
            ans = (float)num_2 / num_3;
            ans = (float)num_1 - ans;
            question.text = num_1 + " - " + num_2 + " / " + num_3 + " = ?";
            spawnFishMixProductPlus();
        }
    } //lvl 11-15

    void Brackets()
    {
        if (difficulty == 0 && numOperator == 0)
        {
            num_1 = Random.Range(0, 11);
            num_2 = Random.Range(0, 11);
            num_3 = Random.Range(0, 11);
            ans = (float)num_1 * num_2 + num_3;
            question.text = "(" + num_1 + " x " + num_2 + ")" + " + " + num_3 + " = ?";
            spawnFishBrackets();
        }
        else if (difficulty == 0 && numOperator == 1)
        {
            num_1 = Random.Range(0, 11);
            num_2 = Random.Range(0, 11);
            num_3 = Random.Range(0, 11);
            ans = (float)num_1 * num_2 - num_3;
            question.text = "(" + num_1 + " x " + num_2 + ")" + " - " + num_3 + " = ?";
            spawnFishBrackets();
        }
        else if (difficulty == 0 && numOperator == 2)
        {
            num_1 = Random.Range(0, 11);
            num_2 = Random.Range(0, 11);
            num_3 = Random.Range(0, 11);
            ans = (float)num_1 / num_2 + num_3;
            question.text = "(" + num_1 + " / " + num_2 + ")" + " + " + num_3 + " = ?";
            spawnFishBrackets();
        }
        else if (difficulty == 0 && numOperator == 3)
        {
            num_1 = Random.Range(0, 11);
            num_2 = Random.Range(0, 11);
            num_3 = Random.Range(0, 11);
            ans = (float)num_1 / num_2 - num_3;
            question.text = "(" + num_1 + " / " + num_2 + ")" + " - " + num_3 + " = ?";
            spawnFishBrackets();
        }
        else if (difficulty == 1 && numOperator == 0)
        {
            num_1 = Random.Range(0, 11);
            num_2 = Random.Range(0, 11);
            num_3 = Random.Range(0, 11);
            ans = num_2 + num_3;
            ans = (float)num_1 * ans;
            question.text = num_1 + " x " + "(" + num_2 + " + " + num_3 + ")" + " = ?";
            spawnFishBrackets();
        }
        else if (difficulty == 1 && numOperator == 1)
        {
            num_1 = Random.Range(0, 11);
            num_2 = Random.Range(0, 11);
            num_3 = Random.Range(0, 11);
            ans = num_2 - num_3;
            ans = (float)num_1 * ans;
            question.text = num_1 + " x " + "(" + num_2 + " - " + num_3 + ")" + " = ?";
            spawnFishBrackets();
        }
        else if (difficulty == 1 && numOperator == 2)
        {
            num_1 = Random.Range(0, 11);
            num_2 = Random.Range(0, 11);
            num_3 = Random.Range(0, 11);
            ans = num_2 + num_3;
            ans = (float)num_1 / ans;
            question.text = num_1 + " / " + "(" + num_2 + " + " + num_3 + ")" + " = ?";
            spawnFishBrackets();
        }
        else if (difficulty == 1 && numOperator == 3)
        {
            num_1 = Random.Range(0, 11);
            num_2 = Random.Range(0, 11);
            num_3 = Random.Range(0, 11);
            ans = num_2 + num_3;
            ans = (float)num_1 / ans;
            question.text = num_1 + " / " + "(" + num_2 + " - " + num_3 + ")" + " = ?";
            spawnFishBrackets();
        }
    } //lvl 16-20

    void touchControl()
    {
        if (Input.touchCount > 0 && directionChosen == false)
        {
            Debug.Log(Input.GetTouch(0).position);
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                touchStartPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            }
            else if (Input.touches[0].phase == TouchPhase.Moved) { }
            else if (Input.touches[0].phase == TouchPhase.Ended)
            {
                touchEndPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                directionChosen = true;
            }
        }
        if (directionChosen == true)
        {
            float scale = 1;
            if (calc == false)
            {
                calc = true;
                buffer1 = touchStartPos;
                buffer2 = touchEndPos;
                constant1 = constant2 = 1;
                buffer1 = new Vector2(buffer1.x * touchEndPos.x, buffer1.y * touchEndPos.x);
                constant1 *= touchEndPos.x;
                buffer2 = new Vector2(buffer2.x * touchStartPos.x, buffer2.y * touchStartPos.x);
                constant2 *= touchStartPos.x;
                if (buffer1.x == buffer2.x)
                {
                    buffer1 = new Vector2(buffer1.x - buffer2.x, buffer1.y - buffer2.y);
                    constant1 -= constant2;
                    constant1 = buffer1.y / constant1;
                }
                else
                {
                    buffer1 = new Vector2(buffer1.x + buffer2.x, buffer1.y + buffer2.y);
                    constant1 += constant2;
                    constant1 = buffer1.y / constant1;
                }
                gradient1 = (touchStartPos.y - constant1) / touchStartPos.x;
                touchDestination = new Vector2(((-5 - constant1) / gradient1), (float)-5);

                if (gradient1 < 0 && touchStartPos.y > touchEndPos.y)
                {
                    Vector3 tempscale = Harpoon.transform.localEulerAngles;
                    float x = Mathf.Abs((Harpoon.transform.position.x - touchDestination.x));
                    float y = Harpoon.transform.position.y - touchDestination.y;
                    float h = Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(y, 2));
                    tempscale.z = 180 + Mathf.Asin(x / h) * 180 / Mathf.PI;
                    Harpoon.transform.localEulerAngles = tempscale;
                }
                else if (gradient1 > 0 && touchStartPos.y > touchEndPos.y)
                {
                    Vector3 tempscale = Harpoon.transform.localEulerAngles;
                    float x = Mathf.Abs((Harpoon.transform.position.x - touchDestination.x));
                    float y = Harpoon.transform.position.y - touchDestination.y;
                    float h = Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(y, 2));
                    tempscale.z = 180 - Mathf.Asin(x / h) * 180 / Mathf.PI;
                    Harpoon.transform.localEulerAngles = tempscale;
                }


                float diff1 = Mathf.Abs((Harpoon.transform.position.x - touchDestination.x));
                float diff2 = Mathf.Abs((Harpoon.transform.position.y - touchDestination.y));
                scale = diff1 / diff2;
                move = (float)0.1 * scale;
            }

            if (Harpoon.transform.position.x > touchDestination.x && touchStartPos.y != touchEndPos.y)
            {
                Harpoon.transform.position = new Vector2(Harpoon.transform.position.x - move, Harpoon.transform.position.y);
            }
            if (Harpoon.transform.position.x < touchDestination.x && touchStartPos.y != touchEndPos.y)
            {
                Harpoon.transform.position = new Vector2(Harpoon.transform.position.x + move, Harpoon.transform.position.y);
            }
            if (Harpoon.transform.position.y > touchDestination.y)
            {
                Harpoon.transform.position = new Vector2(Harpoon.transform.position.x, (Harpoon.transform.position.y - (float)0.1));
            }
            if (touchEndPos.y >= touchStartPos.y)
            {
                calc = false;
                Harpoon.transform.position = new Vector2(0, 2);
                touchStartPos = new Vector2(0, 0);
                touchEndPos = new Vector2(9999, 9999);
                touchDestination = new Vector2(0, 0);
                directionChosen = false;
            }
        }
    }

    void touchDebug()
    {
        if (Input.touchCount <= 1 && directionChosen == false)
        {
            if (Input.GetMouseButtonDown(0))
            {
                touchStartPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
            else if (Input.GetMouseButton(0)){}
            else if (Input.GetMouseButtonUp(0))
            {
                touchEndPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                directionChosen = true;
            }
        }
        if (directionChosen == true)
        {
            float scale = 1;
            if (calc == false)
            {
                calc = true;
                buffer1 = touchStartPos;
                buffer2 = touchEndPos;
                constant1 = constant2 = 1;
                buffer1 = new Vector2(buffer1.x * touchEndPos.x, buffer1.y * touchEndPos.x);
                constant1 *= touchEndPos.x;
                buffer2 = new Vector2(buffer2.x * touchStartPos.x, buffer2.y * touchStartPos.x);
                constant2 *= touchStartPos.x;
                if (buffer1.x == buffer2.x)
                {
                    buffer1 = new Vector2(buffer1.x - buffer2.x, buffer1.y - buffer2.y);
                    constant1 -= constant2;
                    constant1 = buffer1.y / constant1;
                }
                else
                {
                    buffer1 = new Vector2(buffer1.x + buffer2.x, buffer1.y + buffer2.y);
                    constant1 += constant2;
                    constant1 = buffer1.y / constant1;
                }
                gradient1 = (touchStartPos.y - constant1) / touchStartPos.x;
                touchDestination = new Vector2(((-5 - constant1) / gradient1), (float)-5);

                if(gradient1 < 0 && touchStartPos.y > touchEndPos.y)
                {
                    Vector3 tempscale = Harpoon.transform.localEulerAngles;
                    float x = Mathf.Abs((Harpoon.transform.position.x - touchDestination.x));
                    float y = Harpoon.transform.position.y - touchDestination.y;
                    float h = Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(y, 2));
                    tempscale.z = 180 + Mathf.Asin(x / h) * 180 / Mathf.PI;
                    Harpoon.transform.localEulerAngles = tempscale;
                }
                else if(gradient1 > 0 && touchStartPos.y > touchEndPos.y)
                {
                    Vector3 tempscale = Harpoon.transform.localEulerAngles;
                    float x = Mathf.Abs((Harpoon.transform.position.x - touchDestination.x));
                    float y = Harpoon.transform.position.y - touchDestination.y;
                    float h = Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(y, 2));
                    tempscale.z = 180 - Mathf.Asin(x / h) * 180 / Mathf.PI ;
                    Harpoon.transform.localEulerAngles = tempscale;
                }


                float diff1 = Mathf.Abs((Harpoon.transform.position.x - touchDestination.x));
                float diff2 = Mathf.Abs((Harpoon.transform.position.y - touchDestination.y));
                scale = diff1 / diff2;
                move = (float)0.1 * scale;
            }

            if (Harpoon.transform.position.x > touchDestination.x && touchStartPos.y != touchEndPos.y)
            {
                Harpoon.transform.position = new Vector2(Harpoon.transform.position.x - move, Harpoon.transform.position.y);
            }
            if (Harpoon.transform.position.x < touchDestination.x && touchStartPos.y != touchEndPos.y)
            {
                Harpoon.transform.position = new Vector2(Harpoon.transform.position.x + move, Harpoon.transform.position.y);
            }
            if (Harpoon.transform.position.y > touchDestination.y)
            {
                Harpoon.transform.position = new Vector2(Harpoon.transform.position.x, (Harpoon.transform.position.y - (float)0.1));
            }
            if (touchEndPos.y >= touchStartPos.y)
            {
                calc = false;
                Harpoon.transform.position = new Vector2(0, 2);
                touchStartPos = new Vector2(0, 0);
                touchEndPos = new Vector2(9999, 9999);
                touchDestination = new Vector2(0, 0);
                directionChosen = false;
            }
        }
    }

    void spawnFishPlusMinus()
    {
        Vector2 fishPos = new Vector2(Random.Range((float)-1.8, 4), Random.Range((float)-4.5, 2));
        GameObject fish;
        fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
        fish.GetComponent<Fish>().answer = ans;
        fish.transform.parent = fishCanvas.transform;
        fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
        fish.tag = ("Fish");
        for (int i = 0; i < 3; i++)
        {
            if (totalFish == 0)
            {
                fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                fish.GetComponent<Fish>().answer = ans + Random.Range(-3, 0);
                fish.transform.parent = fishCanvas.transform;
                fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                fish.tag = ("Fish");
            }
            else if (totalFish == 1)
            {
                fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                fish.GetComponent<Fish>().answer = ans + Random.Range(1, 6);
                fish.transform.parent = fishCanvas.transform;
                fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                fish.tag = ("Fish");
            }
            else
            {
                if (Random.Range(0, 2) == 0)
                {
                    fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                    fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                    fish.GetComponent<Fish>().answer = ans + Random.Range(20, 61);
                    fish.transform.parent = fishCanvas.transform;
                    fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                    fish.tag = ("Fish");
                }
                else
                {
                    fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                    fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                    fish.GetComponent<Fish>().answer = ans + Random.Range(-20, -61);
                    fish.transform.parent = fishCanvas.transform;
                    fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                    fish.tag = ("Fish");
                }
            }
            totalFish++;
        }
       
    }

    void spawnFishProductDivide()
    {
        Vector2 fishPos = new Vector2(Random.Range((float)-1.8, 4), Random.Range((float)-4.5, 2));
        GameObject fish;
        fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
        fish.GetComponent<Fish>().answer = ans;
        fish.transform.parent = fishCanvas.transform;
        fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
        fish.tag = ("Fish");
        for (int i = 0; i < 3; i++)
        {
            if (totalFish == 0)
            {
                fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                fish.GetComponent<Fish>().answer = ans + (num_1 * Random.Range(1, 3));
                fish.transform.parent = fishCanvas.transform;
                fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                fish.tag = ("Fish");
            }
            else if (totalFish == 1)
            {
                fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                fish.GetComponent<Fish>().answer = ans - (num_1 * Random.Range(1, 3));
                fish.transform.parent = fishCanvas.transform;
                fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                fish.tag = ("Fish");
            }
            else
            {
                if (Random.Range(0, 2) == 0)
                {
                    fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                    fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                    fish.GetComponent<Fish>().answer = ans + Random.Range(20, 61);
                    fish.transform.parent = fishCanvas.transform;
                    fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                    fish.tag = ("Fish");
                }
                else
                {
                    fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                    fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                    fish.GetComponent<Fish>().answer = ans + Random.Range(-20, -61);
                    fish.transform.parent = fishCanvas.transform;
                    fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                    fish.tag = ("Fish");
                }
            }
            totalFish++;
        }
    }

    void spawnFishMixProductPlus()
    {
        Vector2 fishPos = new Vector2(Random.Range((float)-1.8, 4), Random.Range((float)-4.5, 2));
        GameObject fish;
        fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
        fish.GetComponent<Fish>().answer = ans;
        fish.transform.parent = fishCanvas.transform;
        fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
        fish.tag = ("Fish");
        if(difficulty == 0 && numOperator == 0)
        {
            for (int i = 0; i < 3; i++)
            {
                if (totalFish == 0)
                {
                    fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                    fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                    fish.GetComponent<Fish>().answer = num_1 * (num_2 + num_3);
                    fish.transform.parent = fishCanvas.transform;
                    fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                    fish.tag = ("Fish");
                }
                else if (totalFish == 1)
                {
                    fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                    fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                    fish.GetComponent<Fish>().answer = num_1 * num_2 + num_3 - 1;
                    fish.transform.parent = fishCanvas.transform;
                    fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                    fish.tag = ("Fish");
                }
                else
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                        fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                        fish.GetComponent<Fish>().answer = num_1 * (num_2 - 1) + num_3;
                        fish.transform.parent = fishCanvas.transform;
                        fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                        fish.tag = ("Fish");
                    }
                    else
                    {
                        fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                        fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                        fish.GetComponent<Fish>().answer = num_1 * (num_2 + 1) + num_3;
                        fish.transform.parent = fishCanvas.transform;
                        fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                        fish.tag = ("Fish");
                    }
                }
                totalFish++;
            }
        }
        else if(difficulty == 0 && numOperator == 1)
        {
            for (int i = 0; i < 3; i++)
            {
                if (totalFish == 0)
                {
                    fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                    fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                    fish.GetComponent<Fish>().answer = num_1 * (num_2 - num_3);
                    fish.transform.parent = fishCanvas.transform;
                    fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                    fish.tag = ("Fish");
                }
                else if (totalFish == 1)
                {
                    fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                    fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                    fish.GetComponent<Fish>().answer = num_1 * num_2 - num_3 - 1;
                    fish.transform.parent = fishCanvas.transform;
                    fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                    fish.tag = ("Fish");
                }
                else
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                        fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                        fish.GetComponent<Fish>().answer = num_1 * (num_2 - 1) - num_3;
                        fish.transform.parent = fishCanvas.transform;
                        fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                        fish.tag = ("Fish");
                    }
                    else
                    {
                        fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                        fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                        fish.GetComponent<Fish>().answer = num_1 * (num_2 + 1) - num_3;
                        fish.transform.parent = fishCanvas.transform;
                        fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                        fish.tag = ("Fish");
                    }
                }
                totalFish++;
            }
        }
        else if(difficulty == 0 && numOperator == 2)
        {
            for (int i = 0; i < 3; i++)
            {
                if (totalFish == 0)
                {
                    fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                    fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                    fish.GetComponent<Fish>().answer = (float)num_1 / (num_2 + num_3);
                    fish.transform.parent = fishCanvas.transform;
                    fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                    fish.tag = ("Fish");
                }
                else if (totalFish == 1)
                {
                    fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                    fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                    fish.GetComponent<Fish>().answer = (float)num_1 / num_2 + num_3 - 1;
                    fish.transform.parent = fishCanvas.transform;
                    fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                    fish.tag = ("Fish");
                }
                else
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                        fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                        fish.GetComponent<Fish>().answer = (float)num_1 / (num_2 - 1) + num_3;
                        fish.transform.parent = fishCanvas.transform;
                        fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                        fish.tag = ("Fish");
                    }
                    else
                    {
                        fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                        fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                        fish.GetComponent<Fish>().answer = (float)num_1 / (num_2 + 1) + num_3;
                        fish.transform.parent = fishCanvas.transform;
                        fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                        fish.tag = ("Fish");
                    }
                }
                totalFish++;
            }
        }
        else if(difficulty == 0 && numOperator == 3)
        {
            for (int i = 0; i < 3; i++)
            {
                if (totalFish == 0)
                {
                    fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                    fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                    fish.GetComponent<Fish>().answer = (float)num_1 / (num_2 - num_3);
                    fish.transform.parent = fishCanvas.transform;
                    fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                    fish.tag = ("Fish");
                }
                else if (totalFish == 1)
                {
                    fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                    fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                    fish.GetComponent<Fish>().answer = (float)num_1 / num_2 - num_3 - 1;
                    fish.transform.parent = fishCanvas.transform;
                    fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                    fish.tag = ("Fish");
                }
                else
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                        fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                        fish.GetComponent<Fish>().answer = (float)num_1 / (num_2 - 1) - num_3;
                        fish.transform.parent = fishCanvas.transform;
                        fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                        fish.tag = ("Fish");
                    }
                    else
                    {
                        fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                        fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                        fish.GetComponent<Fish>().answer = (float)num_1 / (num_2 + 1) - num_3;
                        fish.transform.parent = fishCanvas.transform;
                        fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                        fish.tag = ("Fish");
                    }
                }
                totalFish++;
            }
        }
        else if (difficulty == 1 && numOperator == 0)
        {
            for (int i = 0; i < 3; i++)
            {
                if (totalFish == 0)
                {
                    fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                    fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                    fish.GetComponent<Fish>().answer = (num_1 + num_2) * num_3;
                    fish.transform.parent = fishCanvas.transform;
                    fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                    fish.tag = ("Fish");
                }
                else if (totalFish == 1)
                {
                    fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                    fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                    fish.GetComponent<Fish>().answer = num_1 + (num_2 + 1) * num_3;
                    fish.transform.parent = fishCanvas.transform;
                    fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                    fish.tag = ("Fish");
                }
                else
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                        fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                        fish.GetComponent<Fish>().answer = num_2 + num_1 * num_3;
                        fish.transform.parent = fishCanvas.transform;
                        fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                        fish.tag = ("Fish");
                    }
                    else
                    {
                        fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                        fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                        fish.GetComponent<Fish>().answer = num_1 + (num_2 - 1) * num_3;
                        fish.transform.parent = fishCanvas.transform;
                        fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                        fish.tag = ("Fish");
                    }
                }
                totalFish++;
            }
        }
        else if (difficulty == 1 && numOperator == 1)
        {
            for (int i = 0; i < 3; i++)
            {
                if (totalFish == 0)
                {
                    fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                    fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                    fish.GetComponent<Fish>().answer = (num_1 - num_2) * num_3;
                    fish.transform.parent = fishCanvas.transform;
                    fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                    fish.tag = ("Fish");
                }
                else if (totalFish == 1)
                {
                    fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                    fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                    fish.GetComponent<Fish>().answer = num_1 - num_2 * num_3 - 1;
                    fish.transform.parent = fishCanvas.transform;
                    fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                    fish.tag = ("Fish");
                }
                else
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                        fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                        fish.GetComponent<Fish>().answer = num_1 - (num_2 - 1) * num_3;
                        fish.transform.parent = fishCanvas.transform;
                        fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                        fish.tag = ("Fish");
                    }
                    else
                    {
                        fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                        fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                        fish.GetComponent<Fish>().answer = num_1 - (num_2 + 1) * num_3;
                        fish.transform.parent = fishCanvas.transform;
                        fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                        fish.tag = ("Fish");
                    }
                }
                totalFish++;
            }
        }
        else if (difficulty == 1 && numOperator == 2)
        {
            for (int i = 0; i < 3; i++)
            {
                if (totalFish == 0)
                {
                    fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                    fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                    fish.GetComponent<Fish>().answer = (float)(num_1 + num_2) / num_3;
                    fish.transform.parent = fishCanvas.transform;
                    fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                    fish.tag = ("Fish");
                }
                else if (totalFish == 1)
                {
                    fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                    fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                    fish.GetComponent<Fish>().answer = (float)(num_1 + num_2) / num_3 - 1;
                    fish.transform.parent = fishCanvas.transform;
                    fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                    fish.tag = ("Fish");
                }
                else
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                        fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                        fish.GetComponent<Fish>().answer = (float)num_1 + (num_2 - 1) / num_3;
                        fish.transform.parent = fishCanvas.transform;
                        fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                        fish.tag = ("Fish");
                    }
                    else
                    {
                        fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                        fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                        fish.GetComponent<Fish>().answer = (float)num_1 + (num_2 + 1) / num_3;
                        fish.transform.parent = fishCanvas.transform;
                        fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                        fish.tag = ("Fish");
                    }
                }
                totalFish++;
            }
        }
        else if (difficulty == 1 && numOperator == 3)
        {
            for (int i = 0; i < 3; i++)
            {
                if (totalFish == 0)
                {
                    fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                    fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                    fish.GetComponent<Fish>().answer = (float)(num_1 - num_2) / num_3;
                    fish.transform.parent = fishCanvas.transform;
                    fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                    fish.tag = ("Fish");
                }
                else if (totalFish == 1)
                {
                    fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                    fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                    fish.GetComponent<Fish>().answer = (float)num_1 - num_2 / (num_3 - 1);
                    fish.transform.parent = fishCanvas.transform;
                    fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                    fish.tag = ("Fish");
                }
                else
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                        fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                        fish.GetComponent<Fish>().answer = (float)num_1 - (num_2 - 1) / num_3;
                        fish.transform.parent = fishCanvas.transform;
                        fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                        fish.tag = ("Fish");
                    }
                    else
                    {
                        fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                        fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                        fish.GetComponent<Fish>().answer = (float)num_1 - (num_2 + 1) / num_3;
                        fish.transform.parent = fishCanvas.transform;
                        fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                        fish.tag = ("Fish");
                    }
                }
                totalFish++;
            }
        }
    }

    void spawnFishBrackets()
    {
        Vector2 fishPos = new Vector2(Random.Range((float)-1.8, 4), Random.Range((float)-4.5, 2));
        GameObject fish;
        fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
        fish.GetComponent<Fish>().answer = ans;
        fish.transform.parent = fishCanvas.transform;
        fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
        fish.tag = ("Fish");
        if (difficulty == 0 && numOperator == 0)
        {
            for (int i = 0; i < 3; i++)
            {
                if (totalFish == 0)
                {
                    fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                    fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                    fish.GetComponent<Fish>().answer = num_1 * (num_2 + num_3);
                    fish.transform.parent = fishCanvas.transform;
                    fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                    fish.tag = ("Fish");
                }
                else if (totalFish == 1)
                {
                    fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                    fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                    fish.GetComponent<Fish>().answer = (num_1+Random.Range(1,3)) * num_2 + num_3;
                    fish.transform.parent = fishCanvas.transform;
                    fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                    fish.tag = ("Fish");
                }
                else
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                        fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                        fish.GetComponent<Fish>().answer = num_1 * num_2 + num_3 + Random.Range(1,5);
                        fish.transform.parent = fishCanvas.transform;
                        fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                        fish.tag = ("Fish");
                    }
                    else
                    {
                        fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                        fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                        fish.GetComponent<Fish>().answer = num_1 * (num_2 + 1) + num_3 - Random.Range(1, 5);
                        fish.transform.parent = fishCanvas.transform;
                        fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                        fish.tag = ("Fish");
                    }
                }
                totalFish++;
            }
        }
        else if (difficulty == 0 && numOperator == 1)
        {
            for (int i = 0; i < 3; i++)
            {
                if (totalFish == 0)
                {
                    fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                    fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                    fish.GetComponent<Fish>().answer = num_1 * (num_2 - num_3);
                    fish.transform.parent = fishCanvas.transform;
                    fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                    fish.tag = ("Fish");
                }
                else if (totalFish == 1)
                {
                    fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                    fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                    fish.GetComponent<Fish>().answer = (num_1 + Random.Range(1, 3)) * num_2 - num_3;
                    fish.transform.parent = fishCanvas.transform;
                    fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                    fish.tag = ("Fish");
                }
                else
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                        fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                        fish.GetComponent<Fish>().answer = num_1 * num_2 - num_3 + Random.Range(1, 5);
                        fish.transform.parent = fishCanvas.transform;
                        fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                        fish.tag = ("Fish");
                    }
                    else
                    {
                        fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                        fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                        fish.GetComponent<Fish>().answer = num_1 * num_2 - num_3 - Random.Range(1, 5);
                        fish.transform.parent = fishCanvas.transform;
                        fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                        fish.tag = ("Fish");
                    }
                }
                totalFish++;
            }
        }
        else if (difficulty == 0 && numOperator == 2)
        {
            for (int i = 0; i < 3; i++)
            {
                if (totalFish == 0)
                {
                    fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                    fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                    fish.GetComponent<Fish>().answer = (float)num_1 / (num_2 + num_3);
                    fish.transform.parent = fishCanvas.transform;
                    fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                    fish.tag = ("Fish");
                }
                else if (totalFish == 1)
                {
                    fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                    fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                    fish.GetComponent<Fish>().answer = (float)num_1 / num_2 + num_3 - 1;
                    fish.transform.parent = fishCanvas.transform;
                    fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                    fish.tag = ("Fish");
                }
                else
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                        fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                        fish.GetComponent<Fish>().answer = (float)num_1 / (num_2 - 1) + num_3;
                        fish.transform.parent = fishCanvas.transform;
                        fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                        fish.tag = ("Fish");
                    }
                    else
                    {
                        fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                        fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                        fish.GetComponent<Fish>().answer = (float)num_1 / (num_2 + 1) + num_3;
                        fish.transform.parent = fishCanvas.transform;
                        fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                        fish.tag = ("Fish");
                    }
                }
                totalFish++;
            }
        }
        else if (difficulty == 0 && numOperator == 3)
        {
            for (int i = 0; i < 3; i++)
            {
                if (totalFish == 0)
                {
                    fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                    fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                    fish.GetComponent<Fish>().answer = (float)num_1 / (num_2 - num_3);
                    fish.transform.parent = fishCanvas.transform;
                    fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                    fish.tag = ("Fish");
                }
                else if (totalFish == 1)
                {
                    fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                    fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                    fish.GetComponent<Fish>().answer = (float)num_1 / num_2 - num_3 - Random.Range(1,5);
                    fish.transform.parent = fishCanvas.transform;
                    fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                    fish.tag = ("Fish");
                }
                else
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                        fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                        fish.GetComponent<Fish>().answer = (float)num_1 / (num_2 - 1) - num_3;
                        fish.transform.parent = fishCanvas.transform;
                        fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                        fish.tag = ("Fish");
                    }
                    else
                    {
                        fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                        fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                        fish.GetComponent<Fish>().answer = (float)num_1 / (num_2 + 1) - num_3;
                        fish.transform.parent = fishCanvas.transform;
                        fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                        fish.tag = ("Fish");
                    }
                }
                totalFish++;
            }
        }
        else if (difficulty == 1 && numOperator == 0)
        {
            for (int i = 0; i < 3; i++)
            {
                if (totalFish == 0)
                {
                    fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                    fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                    fish.GetComponent<Fish>().answer = num_1 + (num_2 * num_3);
                    fish.transform.parent = fishCanvas.transform;
                    fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                    fish.tag = ("Fish");
                }
                else if (totalFish == 1)
                {
                    fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                    fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                    fish.GetComponent<Fish>().answer = (num_1 + (num_2 + 1)) * num_3;
                    fish.transform.parent = fishCanvas.transform;
                    fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                    fish.tag = ("Fish");
                }
                else
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                        fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                        fish.GetComponent<Fish>().answer = num_2 + num_1 * num_3 + 2;
                        fish.transform.parent = fishCanvas.transform;
                        fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                        fish.tag = ("Fish");
                    }
                    else
                    {
                        fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                        fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                        fish.GetComponent<Fish>().answer = num_1 + (num_2 - 1) * num_3 - 2;
                        fish.transform.parent = fishCanvas.transform;
                        fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                        fish.tag = ("Fish");
                    }
                }
                totalFish++;
            }
        }
        else if (difficulty == 1 && numOperator == 1)
        {
            for (int i = 0; i < 3; i++)
            {
                if (totalFish == 0)
                {
                    fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                    fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                    fish.GetComponent<Fish>().answer = num_1 - (num_2 * num_3);
                    fish.transform.parent = fishCanvas.transform;
                    fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                    fish.tag = ("Fish");
                }
                else if (totalFish == 1)
                {
                    fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                    fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                    fish.GetComponent<Fish>().answer = num_1 - num_2 * (num_3+1) - 1;
                    fish.transform.parent = fishCanvas.transform;
                    fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                    fish.tag = ("Fish");
                }
                else
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                        fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                        fish.GetComponent<Fish>().answer = num_1 - (num_2 - 1) * num_3;
                        fish.transform.parent = fishCanvas.transform;
                        fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                        fish.tag = ("Fish");
                    }
                    else
                    {
                        fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                        fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                        fish.GetComponent<Fish>().answer = num_1 - (num_2 + 1) * num_3;
                        fish.transform.parent = fishCanvas.transform;
                        fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                        fish.tag = ("Fish");
                    }
                }
                totalFish++;
            }
        }
        else if (difficulty == 1 && numOperator == 2)
        {
            for (int i = 0; i < 3; i++)
            {
                if (totalFish == 0)
                {
                    fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                    fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                    fish.GetComponent<Fish>().answer = (float)num_1 + (num_2 / num_3);
                    fish.transform.parent = fishCanvas.transform;
                    fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                    fish.tag = ("Fish");
                }
                else if (totalFish == 1)
                {
                    fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                    fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                    fish.GetComponent<Fish>().answer = (float)(num_1 + num_2) / (num_3 - 1);
                    fish.transform.parent = fishCanvas.transform;
                    fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                    fish.tag = ("Fish");
                }
                else
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                        fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                        fish.GetComponent<Fish>().answer = (float)(num_1 + (num_2 - 1)) / num_3;
                        fish.transform.parent = fishCanvas.transform;
                        fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                        fish.tag = ("Fish");
                    }
                    else
                    {
                        fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                        fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                        fish.GetComponent<Fish>().answer = (float)(num_1 + (num_2 + 1)) / num_3;
                        fish.transform.parent = fishCanvas.transform;
                        fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                        fish.tag = ("Fish");
                    }
                }
                totalFish++;
            }
        }
        else if (difficulty == 1 && numOperator == 3)
        {
            for (int i = 0; i < 3; i++)
            {
                if (totalFish == 0)
                {
                    fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                    fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                    fish.GetComponent<Fish>().answer = (float)num_1 - (num_2 / num_3);
                    fish.transform.parent = fishCanvas.transform;
                    fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                    fish.tag = ("Fish");
                }
                else if (totalFish == 1)
                {
                    fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                    fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                    fish.GetComponent<Fish>().answer = (float)(num_1 - num_2) / (num_3 - 1);
                    fish.transform.parent = fishCanvas.transform;
                    fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                    fish.tag = ("Fish");
                }
                else
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                        fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                        fish.GetComponent<Fish>().answer = (float)(num_1 - (num_2 - 1)) / num_3;
                        fish.transform.parent = fishCanvas.transform;
                        fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                        fish.tag = ("Fish");
                    }
                    else
                    {
                        fishPos = new Vector2(Random.Range(-3, 4), Random.Range((float)-4.5, 2));
                        fish = Instantiate(fishPrefab, fishPos, Quaternion.identity);
                        fish.GetComponent<Fish>().answer = (float)(num_1 - (num_2 + 1)) / num_3;
                        fish.transform.parent = fishCanvas.transform;
                        fish.GetComponent<Fish>().gameManagerObj = GameObject.Find("GameManager");
                        fish.tag = ("Fish");
                    }
                }
                totalFish++;
            }
        }
    }

    private void destroyAllFish()
    {
        GameObject[] allFish;
        allFish = GameObject.FindGameObjectsWithTag("Fish");
        for (var i = 0; i < allFish.Length; i++) Destroy(allFish[i]);
    }

    void saveHighScore()
    {
        StreamWriter sw = new StreamWriter(Path.Combine(Application.persistentDataPath, "High Score.txt"), false);

        sw.WriteLine(scoreValue);
        sw.Flush();
        sw.Close();
    }
}