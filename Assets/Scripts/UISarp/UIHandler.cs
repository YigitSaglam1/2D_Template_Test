using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIHandler : MonoBehaviour
{
    public void GetOutOfRealityButton()
    {
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }
}
