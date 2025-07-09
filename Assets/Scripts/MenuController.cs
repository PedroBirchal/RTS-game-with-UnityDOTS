using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private GameObject panelGroup;

    public void PlayButton()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void OpenGroupPanel()
    {
        panelGroup.SetActive(!panelGroup.activeInHierarchy);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
