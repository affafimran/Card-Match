using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class GameController : MonoBehaviour
{
    public Slider rowCount;
    public Slider columnCount;
    public Button play_BTN, load_BTN, save_BTN, restart_BTN;
    public GameObject mainPanel, gamePanel, gameTable;
    public TMP_Text rowValue, colValue;


    public SaveManager saveManager;
    private void Start()
    {
        play_BTN.onClick.AddListener(PlayGame);
        load_BTN.onClick.AddListener(LoadGame);
        save_BTN.onClick.AddListener(SaveGame);
        restart_BTN.onClick.AddListener(RestartGame);

        rowCount.onValueChanged.AddListener(UpdateRowCount);
        columnCount.onValueChanged.AddListener(UpdateColumnCount);

    }


    void RestartGame()
    {
        bool check = CardGenerationManager.instance.CheckCompleteStatus();

        //if (check)
        //{
            mainPanel.GetComponent<CanvasGroup>().alpha = 1;
            gamePanel.GetComponent<CanvasGroup>().alpha = 0;
            gameTable.SetActive(false);
        //}
    }
    public void UpdateRowCount(float val)
    {
        rowValue.text = val.ToString();
    }

    public void UpdateColumnCount(float val)
    {
        colValue.text = val.ToString();
    }

    void PlayGame()
    {
        mainPanel.GetComponent<CanvasGroup>().alpha = 0;
        gamePanel.GetComponent<CanvasGroup>().alpha = 1;
        gameTable.SetActive(true);
        CardGenerationManager.instance.StartGame((int)rowCount.value, (int)columnCount.value);
        
    }

    void SaveGame()
    {
        saveManager.SaveGame(CardGenerationManager.instance.SaveData());
    }

    void LoadGame()
    {
        CardGenerationManager.instance.gameData = saveManager.LoadGame();
        
    }

    
}
