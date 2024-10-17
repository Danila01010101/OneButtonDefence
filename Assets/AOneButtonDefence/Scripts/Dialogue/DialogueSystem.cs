using Adventurer;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
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

    [Header("Спавн модельки нпс")]
    [SerializeField] private GameObject DialogueNPCPrefab;
    [SerializeField] private Vector2 viewportPosition;
    [SerializeField] private float distanceFromCamera = 5f;

    private GameObject spawnedNPC;
    private Camera mainCamera;
    private int numReplic;
    private int numLabel = 0;
    private bool ASPlayingNeed;

    private string showReplic;
    private int countReplic;

    private bool activeChangeReplic = true;

    private AudioSource audioSource;

    public Action DialogEnded;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        numReplic = 0;
        Text.text = DialogueData.Label[numLabel].Replic[numReplic];

        Name.text = DialogueData.Name;

        foreach (Button button in Buttons)
        {
            button.gameObject.SetActive(false);
        }

        SpawnDialogNPC();
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

    private void ChangeReplic() 
    {
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
                ChangeReplic();
                return;
            }

            showReplic = "";
            StartCoroutine(ShowReplic());
            return;
        }
        else
        {
            Destroy(gameObject);
            DialogEnded?.Invoke();
        }
    }

    private void ShowMenu()
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

    private void SpawnDialogNPC()
    {
        mainCamera = Camera.main;
        Vector3 spawnPosition = new Vector3(viewportPosition.x, viewportPosition.y, distanceFromCamera);
        Vector3 worldPosition = mainCamera.ViewportToWorldPoint(spawnPosition);
        spawnedNPC = Instantiate(DialogueNPCPrefab, worldPosition, Quaternion.identity);
    }

    private IEnumerator ShowReplic()
    {
        foreach (var replic in DialogueData.Label[numLabel].Replic[numReplic])
        {
            yield return new WaitForSeconds(ReplicSpeed);
            showReplic += replic;
            if(audioSource.isPlaying == false)
            {
                audioSource.Play();
            }
            Text.text = showReplic;
        }
        yield break;
    }

    private void OnDestroy()
    {
        Destroy(spawnedNPC);
    }
}