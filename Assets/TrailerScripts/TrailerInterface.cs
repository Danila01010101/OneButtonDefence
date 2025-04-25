using UnityEngine;

public class TrailerInterface : MonoBehaviour
{
    [SerializeField] private KeyCode showInterface = KeyCode.H;

    private bool isFreeCam = false;
    
    [HideInInspector] public TrailerCamera TrailerCamera;
    [HideInInspector] public GameplayCanvas Interface;
    [HideInInspector] public CameraMovement GameCamera;

    public void Init()
    {
        SetMode(false);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(showInterface))
        {
            isFreeCam = !isFreeCam;
            SetMode(isFreeCam);
        }
    }

    void SetMode(bool freeCam)
    {
        GameCamera.enabled = !freeCam;
        TrailerCamera.enabled = freeCam;
        
        Cursor.visible = !freeCam;
        Interface.gameObject.SetActive(!freeCam);
    }
}
