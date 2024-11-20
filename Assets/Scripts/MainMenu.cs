using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject mainPanel;
    [SerializeField] GameObject sortingPanel;

    public void SortingButton()
    {
        sortingPanel.SetActive(true);
        mainPanel.SetActive(false);
    }
}
