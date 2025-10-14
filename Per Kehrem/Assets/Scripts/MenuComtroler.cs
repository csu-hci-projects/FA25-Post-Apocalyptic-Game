using UnityEngine;

public class MenuComtroler : MonoBehaviour
{
    public GameObject menuCanvas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        menuCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            Debug.Log("Tab pressed");
            menuCanvas.SetActive(!menuCanvas.activeSelf);
        }
    }
}
