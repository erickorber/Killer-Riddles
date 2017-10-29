using UnityEngine;

public class CameraMovement : MonoBehaviour {

    private Transform player;
    private OutlineSystem outline;
    public OutlineSystem boldOutline;
    public GameObject canvas;
    public static float fadeInTime = 1.2f;

    // Use this for initialization
    private void Start () {
        outline = GetComponent<OutlineSystem>();
        iTween.CameraFadeAdd();
        iTween.CameraFadeFrom(iTween.Hash("amount", 1, "time", fadeInTime, "oncompletetarget", gameObject,
            "oncomplete", "EnablePlay"));
        player = GameObject.FindGameObjectWithTag("Player").transform;
	}

    // Update is called once per frame
    private void LateUpdate () {
        transform.position = new Vector3(player.position.x, transform.position.y, transform.position.z);
        outline.SendMessage("OutlineUpdate");
	}

    private void EnablePlay()
    {
        outline.enabled = true;
        boldOutline.enabled = true;
        canvas.SetActive(true);
    }
}
