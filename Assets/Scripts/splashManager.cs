using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class splashManager : MonoBehaviour
{
    void Start()
    {
        // 3 saniye sonra ana sahneye ge�
        Invoke("LoadMainScene", 6f);
    }

    void LoadMainScene()
    {
        SceneManager.LoadScene(1);
    }
}
