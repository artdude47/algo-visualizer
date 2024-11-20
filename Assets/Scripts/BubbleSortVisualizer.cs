using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BubbleSortVisualizer : MonoBehaviour, ISortingAlgorithm
{
    public Transform container; // Container holding the bars
    public Color defaultColor = Color.white; // Default color for unsorted bars
    public Color comparisonColor = Color.red; // Color for bars being compared
    public Color swappingColor = Color.yellow; // Color for bars being swapped
    public Color sortedColor = Color.green; // Color for sorted bars

    private float animationSpeed = 0.2f;
    public float slowSpeed = 2.0f;
    public float fastSpeed = 0.2f;

    private List<Transform> bars; // List of bar references
    private List<int> dataset; // Dataset for sorting

    public void StartSorting(List<int> dataset)
    {
        this.dataset = new List<int>(dataset); // Copy the dataset
        GenerateVisuals(); // Generate the bar visuals
        Debug.Log("Dataset before sorting: " + string.Join(", ", dataset));
        StartCoroutine(BubbleSort()); // Start the sorting process
    }

    private void GenerateVisuals()
    {
        bars = new List<Transform>(); // Initialize the bars list

        // Clear previous bars
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }

        // Get container dimensions
        float containerWidth = ((RectTransform)container).rect.width;
        float containerHeight = ((RectTransform)container).rect.height;
        float barWidth = containerWidth / dataset.Count;

        // Create bars
        for (int i = 0; i < dataset.Count; i++)
        {
            GameObject bar = new GameObject("Bar", typeof(Image), typeof(Bar)); // Add Bar component
            bar.transform.SetParent(container);

            RectTransform barRect = bar.GetComponent<RectTransform>();
            barRect.anchorMin = new Vector2(0, 0);
            barRect.anchorMax = new Vector2(0, 0);
            barRect.pivot = new Vector2(0, 0);

            float normalizedHeight = (dataset[i] / 100f) * containerHeight;
            barRect.sizeDelta = new Vector2(barWidth, normalizedHeight);
            barRect.anchoredPosition = new Vector2(i * barWidth, 0);

            Image barImage = bar.GetComponent<Image>();
            barImage.color = defaultColor;

            // Assign the value to the Bar component
            Bar barComponent = bar.GetComponent<Bar>();
            barComponent.value = dataset[i];

            bars.Add(bar.transform); // Add bar to the list
        }

        Debug.Log("Bars generated: " + bars.Count);
    }

    private IEnumerator BubbleSort()
    {
        int n = dataset.Count;

        for (int i = 0; i < n - 1; i++) // Outer loop for passes
        {
            bool swapped = false;

            for (int j = 0; j < n - i - 1; j++) // Inner loop for comparisons
            {
                // Highlight bars being compared
                HighlightBar(j, comparisonColor);
                HighlightBar(j + 1, comparisonColor);

                yield return new WaitForSeconds(animationSpeed); // Delay for visualization

                if (dataset[j] > dataset[j + 1])
                {
                    // Swap dataset values
                    int temp = dataset[j];
                    dataset[j] = dataset[j + 1];
                    dataset[j + 1] = temp;

                    // Swap the bar visuals
                    yield return StartCoroutine(SwapBars(j, j + 1));

                    swapped = true;
                }

                // Reset bar colors after comparison
                HighlightBar(j, defaultColor);
                HighlightBar(j + 1, defaultColor);
            }

            // Mark the last sorted bar as green
            HighlightBar(n - i - 1, sortedColor);

            if (!swapped)
            {
                Debug.Log("Array is already sorted.");
                break; // Early exit if no swaps
            }
        }

        // Mark all bars as sorted
        for (int i = 0; i < n; i++)
        {
            HighlightBar(i, sortedColor);
        }

        Debug.Log("Dataset after sorting: " + string.Join(", ", dataset));
    }

    private IEnumerator SwapBars(int indexA, int indexB)
    {
        Transform barA = bars[indexA];
        Transform barB = bars[indexB];

        // Highlight bars being swapped
        HighlightBar(indexA, swappingColor);
        HighlightBar(indexB, swappingColor);

        yield return new WaitForSeconds(animationSpeed); // Delay to visualize swapping

        // Swap bar heights visually
        float tempHeight = barA.GetComponent<RectTransform>().sizeDelta.y;
        barA.GetComponent<RectTransform>().sizeDelta = new Vector2(barA.GetComponent<RectTransform>().sizeDelta.x, barB.GetComponent<RectTransform>().sizeDelta.y);
        barB.GetComponent<RectTransform>().sizeDelta = new Vector2(barB.GetComponent<RectTransform>().sizeDelta.x, tempHeight);

        // Swap Bar component values for debugging
        Bar barAComponent = barA.GetComponent<Bar>();
        Bar barBComponent = barB.GetComponent<Bar>();
        int tempValue = barAComponent.value;
        barAComponent.value = barBComponent.value;
        barBComponent.value = tempValue;

        // Swap bars in the list
        bars[indexA] = barB;
        bars[indexB] = barA;

        // Reset bar colors after swapping
        HighlightBar(indexA, defaultColor);
        HighlightBar(indexB, defaultColor);

        yield return new WaitForSeconds(animationSpeed); // Delay to visualize swapping

        ResetBarsArray();

    }

    private void HighlightBar(int index, Color color)
    {
        if (index < 0 || index >= bars.Count)
        {
            Debug.LogError($"Invalid bar index: {index}");
            return;
        }

        Transform bar = bars[index];
        Image barImage = bar.GetComponent<Image>();
        barImage.color = color;
    }

    private void ResetBarsArray()
    {
        bars.Clear();
        foreach (Transform child in container)
        {
            bars.Add(child);
        }
    }

    public void ChangeAnimationSpeed() 
    {
        if (animationSpeed == fastSpeed)
        {
            animationSpeed = slowSpeed;
        }
        else
        {
            animationSpeed = fastSpeed;
        }
    }
}
