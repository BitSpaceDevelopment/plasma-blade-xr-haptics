using UnityEngine;
using UnityEngine.Events;

public class LynxButton : MonoBehaviour
{
    public UnityEvent onPress;
    private MenuManager menuManager;
    public bool isPressDelay = false;

    public void Start()
    {
        menuManager = GameObject.Find("MenuManager").GetComponent<MenuManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        isPressDelay = menuManager.uiDelay;

        if(other.gameObject.tag == "UIInteractor" && !isPressDelay)
        {
            menuManager.DelayUIPress();
            onPress.Invoke();
        }
    }
}
