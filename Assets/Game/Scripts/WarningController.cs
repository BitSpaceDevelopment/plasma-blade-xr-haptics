using System.Collections;
using UnityEngine;

public class WarningController : MonoBehaviour
{
    [Header("UI")]
    public GameObject WarningUI;

    public void CallWarning()
    {
        StartCoroutine(ShowWarning());
    }

   public IEnumerator ShowWarning()
    {
        WarningUI.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        WarningUI.SetActive(false);
    }

}
