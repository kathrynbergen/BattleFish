using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public void OnLevelOneCLicked()
    {
        print("start level one");
        SceneManager.LoadScene("Combat");
    }
}
