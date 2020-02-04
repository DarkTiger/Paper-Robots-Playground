using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.Collections;

public class VideoPlayerScript : MonoBehaviour
{
    VideoPlayer videoPlayer;

    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
    }

    private IEnumerator Start()
    {
        Cursor.visible = false;
        yield return new WaitForSeconds(1);

        while(true)
        {
            if (!videoPlayer.isPlaying || Input.GetButton("Continue"))
            {
                SceneManager.LoadScene(1);
            }
            yield return null;
        }
    }
}
