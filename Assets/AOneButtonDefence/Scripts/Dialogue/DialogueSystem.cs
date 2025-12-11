using Adventurer;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Как я мог это написать? Я нифига не понимаю, ъуъуъуъ( (С) Mivoky
[RequireComponent(typeof(AudioSource))]
public class DialogueSystem : MonoBehaviour
{
    public DialogueData DialogueData;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Text;
    public Image GnomeAdvisor;
    [Header("Text Speed")]
    public float ReplicSpeed;
    [Header("Skip dialog")]
    public float SkipTime;
    public Slider Slider;
    public KeyCode KeyCodePerSkip = KeyCode.G;

    [SerializeField] private Button reviveButton;

    private int numReplic;
    private int numLabel = 0;
    private bool ASPlayingNeed;
    private AdsReviver adsReviver;

    private string showReplic;
    private Coroutine replicaCoroutine;
    private Coroutine skipReplica;

    private bool activeChangeReplic = true;

    private AudioSource audioSource;

    public Action DialogEnded;

    public void Initialize(AdsReviver adsReviver = null)
    {
        this.adsReviver = adsReviver; 
        audioSource = GetComponent<AudioSource>();
        numReplic = 0;
        Name.text = DialogueData.Name;
        
        if (Slider != null)
            Slider.value = 0;
        
        if (GnomeAdvisor != null)
            GnomeAdvisor.sprite = DialogueData.Label[numLabel].CharacterEmotion.Emotion();

        gameObject.SetActive(true);
        replicaCoroutine = StartCoroutine(ShowReplica());
        
        if (adsReviver != null)
            adsReviver.SubscribeButton(reviveButton);
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
            if (skipReplica != null)
            {
                StopCoroutine(skipReplica);
                skipReplica = null;
            }
        }
    }

    public void StartDialog()
    {
        gameObject.SetActive(true);
        replicaCoroutine = StartCoroutine(ShowReplica());
    }

    private void ChangeReplica() 
    {
        if (adsReviver != null && IsClickOnUI())
            return;
        
        if (activeChangeReplic == false)
        {
            return;
        }

        if (numReplic < DialogueData.Label[numLabel].Replic.Count - 1)
        {
            if (replicaCoroutine != null)
            {
                StopCoroutine(replicaCoroutine);
                replicaCoroutine = null;
            }
            
            numReplic++; 
            DetectReplicaSkip();
            return;
        }
        else if (DialogueData.Label[numLabel].NextLabel != 0)
        {
            ChangeLabel(DialogueData.Label[numLabel].NextLabel);
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

    public void ChooseReplica(int num)
    {

        activeChangeReplic = true;
        numLabel = DialogueData.Label[numLabel].Answers[num].MoveTo;
        numReplic = 0;

        showReplic = "";

        StopCoroutine(replicaCoroutine);
        replicaCoroutine = StartCoroutine(ShowReplica());
    }
    
    private void ChangeLabel(int label)
    {
        numLabel = label;
        numReplic = 0;
        
        if (replicaCoroutine != null)
        {
            StopCoroutine(replicaCoroutine);
            replicaCoroutine = null;
        }
        
        GnomeAdvisor.sprite = DialogueData.Label[numLabel].CharacterEmotion.Emotion();
        replicaCoroutine = StartCoroutine(ShowReplica());
    }

    private void SkipDialog()
    {
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
        showReplic = "";
        foreach (var replica in DialogueData.Label[numLabel].Replic[numReplic])
        {
            yield return new WaitForSeconds(ReplicSpeed);
            showReplic += replica;
            Text.text = showReplic;
            
            if(audioSource.isPlaying == false)
            {
                audioSource.Play();
            }
        }
        
        replicaCoroutine = null;
    }

    private bool IsClickOnUI()
    {
        if (EventSystem.current == null)
            return false;

        if (EventSystem.current.IsPointerOverGameObject())
            return true;

        if (EventSystem.current.IsPointerOverGameObject(0))
            return true;

        return false;
    }

    private void OnDestroy()
    {
        if (replicaCoroutine != null)
        {
            StopCoroutine(replicaCoroutine);
            replicaCoroutine = null;
        }
        
        if (skipReplica != null)
        {
            StopCoroutine(skipReplica);
            skipReplica = null;
        }
    }
}