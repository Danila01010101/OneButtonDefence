using UnityEngine;
using DG.Tweening;

public class DeathAnimation : MonoBehaviour
{
    private AudioSource audioSource;
    private float duration;
    
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
          duration = audioSource.clip.length;
    }
    public void StartAnimation() => StartDieAnimation();

    private void StartDieAnimation()
    {
        if (audioSource != null)
        {
            transform.DORotate(new Vector3(transform.rotation.x, transform.rotation.y + 90, transform.rotation.z), duration);
            audioSource.Play();
        }
    }
}
