using Adventurer;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class DialogueSystem : MonoBehaviour
{
    public DialogueData DialogueData;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Text;
    public GameObject AnswerPanel;
    [Header("Menu replic chose")]
    public List<Button> Buttons = new List<Button>();
    public List<TextMeshProUGUI> Answers = new List<TextMeshProUGUI>();
    [Header("Text Speed")]
    public float ReplicSpeed;
    [Header("Skip dialog")]
    public float SkipTime;
    public Slider Slider;
    public KeyCode KeyCodePerSkip = KeyCode.G;

    [SerializeField] private DialogNPCSpawner spawner;

    private int numReplic;
    private int numLabel = 0;
    private bool ASPlayingNeed;

    private string showReplic;
    private int countReplic;
    // Корутины
    private Coroutine replicaCoroutine;
    private Coroutine skipReplica;

    private bool activeChangeReplic = true;

    private AudioSource audioSource;

    public Action DialogEnded;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        numReplic = 0;
        
        if (Slider != null)
            Slider.value = 0;

        //Text.text = DialogueData.Label[numLabel].Replic[numReplic];
        Name.text = DialogueData.Name;

        foreach (Button button in Buttons)
        {
            button.gameObject.SetActive(false);
        }

        if (spawner != null)
            spawner.SpawnDialogNPC();

        AnswerPanel.SetActive(false);
        gameObject.SetActive(true);
        replicaCoroutine = StartCoroutine(ShowReplica());


    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ChangeReplica();
        }
        if (Input.GetKeyDown(KeyCodePerSkip) && Slider != null)
        {
            skipReplica = StartCoroutine(Timer(SkipTime));
        }
        if (Input.GetKeyUp(KeyCodePerSkip))
        {
            Slider.value = 0;
            StopCoroutine(skipReplica);
        }

    }

    public void StartDialog()
    {
        gameObject.SetActive(true);
        replicaCoroutine = StartCoroutine(ShowReplica());
    }

    private void ChangeReplica() 
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
                ChangeReplica();
                return;
            }

            showReplic = "";
            DetectReplicaSkip();
            return;
        }
        else
        {
            Destroy(gameObject);
            DialogEnded?.Invoke();
        }
    }

    private void DetectReplicaSkip()
    {
        if (replicaCoroutine != null)
        {
            StopCoroutine(replicaCoroutine);
            replicaCoroutine = null;
            Text.text = DialogueData.Label[numLabel].Replic[numReplic - 1];
        }
        else
        {
            replicaCoroutine = StartCoroutine(ShowReplica());
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

    public void ChooseReplica(int num)
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
        replicaCoroutine = StartCoroutine(ShowReplica());
    }

    private void SkipDialog()
    {
        DetectReplicaSkip();
        Destroy(gameObject);
        DialogEnded?.Invoke();
    }

    private IEnumerator Timer(float time)
    {
        float timerTime = 0;
        while (true) 
        {
            yield return new WaitForEndOfFrame();
            timerTime += Time.deltaTime;
            Slider.value = timerTime / time;
            if (timerTime >= time)
            {
                SkipDialog();
            }
        }
        
    }
    private IEnumerator ShowReplica()
    {
        foreach (var replica in DialogueData.Label[numLabel].Replic[numReplic])
        {
            yield return new WaitForSeconds(ReplicSpeed);
            showReplic += replica;
            if(audioSource.isPlaying == false)
            {
                audioSource.Play();
            }
            Text.text = showReplic;
        }
        
        replicaCoroutine = null;
    }

    private void OnDestroy()
    {
        if (spawner != null)
            spawner.DeleteNPC();
    }
}