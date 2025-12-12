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

    [Header("Skip dialog")]
    public float SkipTime;
    public Slider Slider;
    public KeyCode KeyCodePerSkip = KeyCode.G;

    [SerializeField] private Button reviveButton;

    protected int numReplic { get; private set; }
    protected int numLabel { get; private set; }
    
    protected bool closeOnReplicFinish = true;
    
    private string showReplic;
    private Coroutine replicaCoroutine;
    private Coroutine skipCoroutine;

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
            ChangeReplica();

        if (Input.GetKeyDown(KeyCodePerSkip) && Slider != null)
            skipCoroutine = StartCoroutine(Timer(SkipTime));

        if (Input.GetKeyUp(KeyCodePerSkip))
        {
            Slider.value = 0;
            if (skipCoroutine != null)
            {
                StopCoroutine(skipCoroutine);
                skipCoroutine = null;
            }
        }
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
        Destroy(gameObject);
        DialogEnded?.Invoke();
    }

    private IEnumerator Timer(float time)
    {
        float timer = 0;

        while (true)
        {
            yield return null;
            timer += Time.deltaTime;

            Slider.value = timer / time;

            if (timer >= time)
            {
                SkipDialog();
                yield break;
            }
        }
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

        if (skipCoroutine != null)
            StopCoroutine(skipCoroutine);
    }
}