using Adventurer;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class DialogueSystem : MonoBehaviour
{
    public DialogueData DialogueData;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Text;
    public Image GnomeAdvisor;

    [Header("Text Speed")]
    public float ReplicSpeed;

    [Header("Skip Dialog")]
    public float SkipTime = 2f;
    public Slider SkipProgressSlider;
    public Button SkipButton;

    [SerializeField] private Button reviveButton;

    protected int numReplic { get; private set; }
    protected int numLabel { get; private set; }
    
    protected bool closeOnReplicFinish = true;
    
    private string showReplic;
    private Coroutine replicaCoroutine;
    private Coroutine skipCoroutine;
    
    private bool isSkipButtonHeld = false;
    private float currentSkipProgress = 0f;

    private AudioSource audioSource;
    private AdsReviver adsReviver;

    public Action DialogEnded;

    public virtual void Initialize(AdsReviver adsReviver = null)
    {
        this.adsReviver = adsReviver;
        audioSource = GetComponent<AudioSource>();

        numReplic = 0;
        numLabel = 0;

        Name.text = DialogueData.Name;

        if (SkipProgressSlider != null)
        {
            SkipProgressSlider.gameObject.SetActive(true);
            SkipProgressSlider.value = 0f;
        }

        if (GnomeAdvisor != null)
            GnomeAdvisor.sprite = DialogueData.Label[numLabel].CharacterEmotion.Emotion();

        SetupSkipButton();

        gameObject.SetActive(true);

        replicaCoroutine = StartCoroutine(ShowReplica());

        if (adsReviver != null)
            adsReviver.SubscribeButton(reviveButton);
    }

    private void SetupSkipButton()
    {
        if (SkipButton == null) return;

        SkipButton.onClick.RemoveAllListeners();

        var eventTrigger = SkipButton.gameObject.GetComponent<EventTrigger>();
        if (eventTrigger == null)
            eventTrigger = SkipButton.gameObject.AddComponent<EventTrigger>();

        eventTrigger.triggers.Clear();

        var pointerDownEntry = new EventTrigger.Entry();
        pointerDownEntry.eventID = EventTriggerType.PointerDown;
        pointerDownEntry.callback.AddListener((data) => { OnSkipButtonDown(); });
        eventTrigger.triggers.Add(pointerDownEntry);

        var pointerUpEntry = new EventTrigger.Entry();
        pointerUpEntry.eventID = EventTriggerType.PointerUp;
        pointerUpEntry.callback.AddListener((data) => { OnSkipButtonUp(); });
        eventTrigger.triggers.Add(pointerUpEntry);

        var pointerExitEntry = new EventTrigger.Entry();
        pointerExitEntry.eventID = EventTriggerType.PointerExit;
        pointerExitEntry.callback.AddListener((data) => { OnSkipButtonUp(); });
        eventTrigger.triggers.Add(pointerExitEntry);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            ChangeReplica();

        if (isSkipButtonHeld && SkipProgressSlider != null)
        {
            currentSkipProgress += Time.deltaTime;
            SkipProgressSlider.value = currentSkipProgress / SkipTime;

            if (currentSkipProgress >= SkipTime)
            {
                SkipDialog();
            }
        }
    }

    private void OnSkipButtonDown()
    {
        if (SkipProgressSlider == null) return;

        isSkipButtonHeld = true;
        currentSkipProgress = 0f;
        SkipProgressSlider.value = 0f;
        SkipProgressSlider.gameObject.SetActive(true);
    }

    private void OnSkipButtonUp()
    {
        if (SkipProgressSlider == null) return;

        isSkipButtonHeld = false;
        currentSkipProgress = 0f;
        SkipProgressSlider.value = 0f;
        SkipProgressSlider.gameObject.SetActive(false);
    }

    public void StartDialog()
    {
        gameObject.SetActive(true);
        replicaCoroutine = StartCoroutine(ShowReplica());
    }

    protected virtual void ChangeReplica()
    {
        if (adsReviver != null && IsClickOnUI())
            return;

        if (replicaCoroutine != null)
        {
            StopCoroutine(replicaCoroutine);
            replicaCoroutine = null;

            Text.text = DialogueData.Label[numLabel].Replic[numReplic];
            return;
        }

        if (numReplic < DialogueData.Label[numLabel].Replic.Count - 1)
        {
            numReplic++;
            replicaCoroutine = StartCoroutine(ShowReplica());
            return;
        }

        if (DialogueData.Label[numLabel].NextLabel != 0)
        {
            ChangeLabel(DialogueData.Label[numLabel].NextLabel);
            return;
        }

        if (closeOnReplicFinish)
            SkipDialog();
    }

    private void ChangeLabel(int label)
    {
        numLabel = label;
        numReplic = 0;

        if (GnomeAdvisor != null)
            GnomeAdvisor.sprite = DialogueData.Label[numLabel].CharacterEmotion.Emotion();

        replicaCoroutine = StartCoroutine(ShowReplica());
    }

    protected virtual void SkipDialog()
    {
        if (SkipProgressSlider != null)
            SkipProgressSlider.gameObject.SetActive(false);

        Destroy(gameObject);
        DialogEnded?.Invoke();
    }

    private IEnumerator ShowReplica()
    {
        showReplic = "";
        var targetText = DialogueData.Label[numLabel].Replic[numReplic];

        foreach (char c in targetText)
        {
            yield return new WaitForSeconds(ReplicSpeed);

            showReplic += c;
            Text.text = showReplic;

            if (!audioSource.isPlaying)
                audioSource.Play();
        }

        replicaCoroutine = null;
    }

    private bool IsClickOnUI()
    {
        if (EventSystem.current == null) return false;

        return EventSystem.current.IsPointerOverGameObject() ||
               EventSystem.current.IsPointerOverGameObject(0);
    }

    private void OnDestroy()
    {
        if (replicaCoroutine != null)
            StopCoroutine(replicaCoroutine);
    }
}