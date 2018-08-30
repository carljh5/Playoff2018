using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerTextBubbleBehaviour : MonoBehaviour {
    public bool isPlayer1;
    private Transform playerHead;

    TextMeshPro textMesh;

    private void Start()
    {
        if (textMesh == null)
            textMesh = GetComponent<TextMeshPro>();
        if (playerHead == null)
            playerHead = GameManager.GetPlayerHeadTransform(isPlayer1);
    }

    void OnEnable()
    {
        if (textMesh == null)
            textMesh = GetComponent<TextMeshPro>();
        if (playerHead == null)
            playerHead = GameManager.GetPlayerHeadTransform(isPlayer1);
        StartCoroutine(waitForSeconds());
        transform.position = playerHead.position + (Vector3.up * 2.5f);
    }

    IEnumerator waitForSeconds()
    {
        float time = 0;
        float fadeDuration = 0.25f;
        float alpha = 0f;
        while (time < fadeDuration)
        {
            alpha = Mathf.Lerp(0, 1, time / fadeDuration);
            textMesh.alpha = alpha;
            time += Time.deltaTime;
            yield return null;
        }
        alpha = 1f;
        textMesh.alpha = alpha;
        time = 0;
        yield return new WaitForSeconds(2f);
        while (time < fadeDuration)
        {
            alpha = Mathf.Lerp(1, 0, time / fadeDuration);
            textMesh.alpha = alpha;
            time += Time.deltaTime;
            yield return null;
        }
        alpha = 0f;
        textMesh.alpha = alpha;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        Vector3 curVelocity = Vector3.zero;
        transform.position = Vector3.SmoothDamp(transform.position, (playerHead.position + (Vector3.up * 2.5f)), ref curVelocity, 0.1f);
    }
}
