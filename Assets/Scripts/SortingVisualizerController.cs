using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class SortingVisualizerController : MonoBehaviour
{
    [SerializeField] TMP_InputField numberInput;
    [SerializeField] Button randomizeButton;
    [SerializeField] Button startButton;
    [SerializeField] Transform[] visualizerContainers;
    [SerializeField] Color barColor;

    [SerializeField] GameObject panel;
    [SerializeField] GameObject mainPanel;

    private List<int> dataset;
    private int numberOfItems;

    private void Start()
    {
        randomizeButton.onClick.AddListener(RandomizeDataset);
        startButton.onClick.AddListener(StartSorting);
    }

    void RandomizeDataset()
    {
        Debug.Log("should generate");
        if (int.TryParse(numberInput.text, out numberOfItems) && numberOfItems >= 10 && numberOfItems <= 100)
        {
            dataset = new List<int>();
            for (int i = 0; i < numberOfItems; i++)
            {
                dataset.Add(Random.Range(10, 100)); // Generate random values between 10 and 100
            }
            GenerateVisuals();
        }
        else
        {
            Debug.LogError("Please enter a valid number between 10 and 100.");
        }
    }

    void GenerateVisuals()
    {
        foreach (Transform container in visualizerContainers)
        {
            // Clear previous bars
            foreach (Transform child in container)
            {
                Destroy(child.gameObject);
            }

            // Get the container's dimensions
            float containerWidth = ((RectTransform)container).rect.width;
            float containerHeight = ((RectTransform)container).rect.height;

            // Calculate bar width and ensure they fit perfectly within the container
            float barWidth = containerWidth / numberOfItems;

            for (int i = 0; i < dataset.Count; i++)
            {
                // Create a new UI Image for the bar
                GameObject bar = new GameObject("Bar", typeof(Image));
                bar.transform.SetParent(container);

                // Set the bar's RectTransform
                RectTransform barRect = bar.GetComponent<RectTransform>();
                barRect.anchorMin = new Vector2(0, 0);
                barRect.anchorMax = new Vector2(0, 0);
                barRect.pivot = new Vector2(0, 0);

                // Normalize the height of the bar relative to the container's height
                float normalizedHeight = (dataset[i] / 100f) * containerHeight;
                barRect.sizeDelta = new Vector2(barWidth, normalizedHeight);

                // Position the bar horizontally based on its index
                barRect.anchoredPosition = new Vector2(i * barWidth, 0);

                // Set the bar color
                Image barImage = bar.GetComponent<Image>();
                barImage.color = barColor;
            }
        }
    }

    void StartSorting()
    {
        foreach (Transform container in visualizerContainers)
        {
            ISortingAlgorithm sorter = container.GetComponent<ISortingAlgorithm>();
            if (sorter != null)
            {
                sorter.StartSorting(new List<int>(dataset)); // Pass a copy of the dataset to each sorter
            }
        }
    }

    public void BackButton()
    {
        panel.SetActive(false);
        mainPanel.SetActive(true);
    }
}
