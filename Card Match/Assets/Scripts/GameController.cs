using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class GameController : MonoBehaviour
{
    public Slider rowCount;
    public Slider columnCount;
    public Button play_BTN;
    public GameObject mainPanel, gamePanel, gameTable;
    public TMP_Text rowValue, colValue;

    private void Start()
    {
        play_BTN.onClick.AddListener(PlayGame);
        rowCount.onValueChanged.AddListener(UpdateRowCount);
        columnCount.onValueChanged.AddListener(UpdateColumnCount);
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
}
