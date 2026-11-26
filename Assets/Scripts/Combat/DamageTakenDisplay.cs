using System.Collections;
using TMPro;
using UnityEngine;

public class DamageTakenDisplay : MonoBehaviour
{
    [SerializeField] private float animationDuration = 0.6f;
    [SerializeField] private float riseDistance = 50f;

    private TMP_Text text;
    private RectTransform rectTransform;
    private Vector2 startPosition;
    private Color startColor;
    private Coroutine damageAnimation;
    private int displayedDamage;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
        rectTransform = GetComponent<RectTransform>();
        startPosition = rectTransform.anchoredPosition;
        startColor = text.color;
        SetAlpha(0f);
    }

    private void OnDisable()
    {
        if (damageAnimation != null)
        {
            StopCoroutine(damageAnimation);
            damageAnimation = null;
        }

        SetAlpha(0f);
    }

    public void ShowDamage(int amount)
    {
        if (damageAnimation != null)
        {
            StopCoroutine(damageAnimation);
            displayedDamage += amount;
        }
        else
        {
            displayedDamage = amount;
        }

        damageAnimation = StartCoroutine(AnimateDamage());
    }

    private IEnumerator AnimateDamage()
    {
        text.text = "-" + displayedDamage;
        rectTransform.anchoredPosition = startPosition;
        SetAlpha(1f);

        float elapsed = 0f;
        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsed / animationDuration);
            rectTransform.anchoredPosition = startPosition + Vector2.up * (riseDistance * progress);
            SetAlpha(1f - progress);
            yield return null;
        }

        rectTransform.anchoredPosition = startPosition;
        SetAlpha(0f);
        damageAnimation = null;
    }

    private void SetAlpha(float alpha)
    {
        Color color = startColor;
        color.a = alpha;
        text.color = color;
    }
}
