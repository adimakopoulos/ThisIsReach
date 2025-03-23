using UnityEngine;
using TMPro;
using System.Collections;

public class FloatingTextManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textComponent;
    private float moveSpeed = 0.5f;
    private float fadeTime = 3.0f;
    private Vector3 offset = new Vector3(0, 1, 0);

    private Color originalColor;
    private Transform targetObject;

    private void Awake()
    {
        if (textComponent == null)
            textComponent = GetComponentInChildren<TextMeshProUGUI>();

        originalColor = textComponent.color;
        gameObject.SetActive(false); // Start inactive
    }

    public void ShowText(string message, Transform target)
    {
        // Set text content
        textComponent.text = message;

        // Reset color and scale
        textComponent.color = originalColor;
        //transform.localScale = Vector3.one;

        // Store reference to target
        targetObject = target;

        // Position the text above the target
        UpdatePosition(0);

        // Activate and begin animation
        gameObject.SetActive(true);
        StartCoroutine(AnimateText());
    }

    private void UpdatePosition(float heightOffset)
    {
        if (targetObject != null)
        {
            transform.position = targetObject.position + offset + new Vector3(0, heightOffset, 0);
            // Make text face camera
            transform.forward = Camera.main.transform.forward;
        }
    }

    private IEnumerator AnimateText()
    {
        float elapsed = 0f;

        while (elapsed < fadeTime && targetObject != null)
        {
            // Move upward over time
            float heightOffset = moveSpeed * elapsed;
            UpdatePosition(heightOffset);

            // Fade out gradually
            float alpha = Mathf.Lerp(1, 0, elapsed / fadeTime);
            textComponent.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Deactivate after animation (ready for pooling)
        gameObject.SetActive(false);
    }
}
