 using System.Collections;
 using UnityEngine;
using System.Threading.Tasks;
using TMPro;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [Header("SCREENS")] 
    [SerializeField] private ScreenManager GameOverScreen;
    [SerializeField] private ScreenManager NextLevelScreen;
    [SerializeField] private ScreenManager MainMenuScreen;
    public ScreenManager InGame;
    public ScreenManager Pause;
    
    [Header("UI")]
    public LevelSelector LevelSelector;
    private Camera _camera;
    [SerializeField] private TextMeshPro menutext;
    [Header("Game Settings")] 
    [SerializeField] private int moves;
    [SerializeField] private int count=10;
    private bool isProcessing = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    void Start()
    {
        MenuScreen();
        _camera = Camera.main;
        GridController.instance.CreateGrid();
    }

   public void MenuScreen()
    {
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.CompareTag("Respawn") && obj.scene.isLoaded)
            {
                obj.SetActive(true);
            }
        }

        moves = 1;
        count = 1;
        MainMenuScreen.Setup();
        menutext.gameObject.SetActive(true);
        menutext.text = "level "+ LevelSelector.LevelLoad();
        
    }
    
    
    public void StartGame()
    {
        GameObject[] cubes = GameObject.FindGameObjectsWithTag("Respawn");
        foreach (GameObject cube in cubes)
        {
            cube.gameObject.SetActive(false); 
        }
        InGame.gameObject.SetActive(true);
        menutext.gameObject.SetActive(false);
        LevelSelector.LoadLevel(LevelSelector.LevelLoad());
        
    }

    public void EndGame()
    {
        LevelSelector.CollectObects();
        InGame.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isProcessing) return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                isProcessing = true;
                PuzzleBox puzzleBox = hit.collider.GetComponent<PuzzleBox>();

                if (puzzleBox != null)
                {
                    puzzleBox.ShootRay();
                }
                else
                {
                    Debug.Log("This object is not puzzle box won't shoot a ray");
                }

                StartCoroutine(InputDelay());
            }
        }
        else if (count == 0 && GameObject.FindGameObjectsWithTag("Kutu").Count(k => k.activeInHierarchy) == 0)
        {
            EndGame();
            NextLevelScreen.Setup();
        }
        else if (moves == 0 && count != 0)
        {
            EndGame();
            GameOverScreen.Setup(); 
        }
    }

    IEnumerator InputDelay()
    {
        yield return new WaitForSeconds(0.1f);
        isProcessing = false;
    }
        
    

    public int MovesLeft
    {
        get { return moves; }
        set { moves = value; }
    }
    
    public int CountLeft
    {
        get { return count; }
        set { count = value; }
    }
}

