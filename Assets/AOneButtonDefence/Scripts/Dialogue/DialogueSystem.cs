using Adventurer;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// (https://vk.com/video-149258223_456242495)
public class DialogueSystem : MonoBehaviour
{
    public DialogueData DialogueData;

    public TextMeshProUGUI Name;
    public TextMeshProUGUI Text;
    public GameObject AnswerPanel;
    [Header("Меню выбора реплики")]
    public List<Button> Buttons = new List<Button>();
    public List<TextMeshProUGUI> Answers = new List<TextMeshProUGUI>();
    [Header("Скорость текста")]
    public float ReplicSpeed;
    
    private int numReplic;
    private int numLabel = 0;

    private string showReplic;
    private int countReplic;

    private bool activeChangeReplic = true;

    //До первого кадра
    private void Awake()
    {
    }
    //Первый кадр и дальше
    void Start()
    {
        
        numReplic = 0;
        Text.text = DialogueData.Label[numLabel].Replic[numReplic];

        Name.text = DialogueData.Name;

        foreach (Button button in Buttons)
        {
            button.gameObject.SetActive(false);
        }

        AnswerPanel.SetActive(false);
        gameObject.SetActive(true);
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ChangeReplic();
        }
    }

    public void StartDialog()
    {
        gameObject.SetActive(true);
        StartCoroutine(ShowReplic());
    }

    void ChangeReplic() 
    {
        Debug.Log("numLabel:" + numLabel + "numReplic:" + numReplic);
        if (activeChangeReplic == false)
        {
            return;
        }

        if (numReplic < DialogueData.Label[numLabel].Replic.Count - 1)
        {
            StopAllCoroutines();
            numReplic++; 

            if (DialogueData.Label[numLabel].Replic[numReplic] == DialogueCommands.Debug)
            {
                Debug.Log("Debug!");
                ChangeReplic();
                return;
            }

            showReplic = "";
            StartCoroutine(ShowReplic());
            return;
        }

        //if (DialogueData.Label[numLabel].Answers.Any() == false)
        //{
        //    numLabel = DialogueData.Label[numLabel].NextLabel;
        //    numReplic = 0;
        //    showReplic = "";
        //    StopAllCoroutines();
        //    StartCoroutine(ShowReplic());
        //    return;
        //}

        else
        {
            //ShowMenu();
            Destroy(gameObject);
        }
    }

    void ShowMenu()
    {
        countReplic = 0;
        activeChangeReplic = false;
        AnswerPanel.SetActive(true);
        foreach (Answer answer in DialogueData.Label[numLabel].Answers)
        {
            countReplic++;
        }

        for (int i = 0; i < countReplic; i++)
        {
            Buttons[i].gameObject.SetActive(true);
            Answers[i].text = DialogueData.Label[numLabel].Answers[i].AnswerText;
        }
    }

    public void ChoiseReplic(int num)
    {
        foreach (Button button in Buttons)
        {
            button.gameObject.SetActive(false);
        }

        AnswerPanel.SetActive(false);

        activeChangeReplic = true;
        numLabel = DialogueData.Label[numLabel].Answers[num].MoveTo;
        numReplic = 0;
        countReplic = 0;

        showReplic = "";

        StopAllCoroutines();
        StartCoroutine(ShowReplic());
    }

    IEnumerator ShowReplic()
    {
        foreach (var replic in DialogueData.Label[numLabel].Replic[numReplic])
        {
            yield return new WaitForSeconds(ReplicSpeed);
            
            showReplic += replic;
            Text.text = showReplic;
        }
        yield break;
    }

   
}
