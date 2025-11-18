using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps; // Tilemap kullanmak için bu satýr gerekli

public class TransparentDetection : MonoBehaviour
{
    [Tooltip("Obje þeffaflaþtýðýnda alacaðý alfa (görünürlük) deðeri. 1 = tam görünür, 0 = tam görünmez.")]
    [Range(0, 1)] // Deðerin 0-1 arasýnda kalmasýný saðlar
    [SerializeField] private float transparencyAmount = 0.8f;

    [Tooltip("Þeffaflaþmanýn ne kadar hýzlý (saniye) olacaðý.")]
    [SerializeField] private float fadeTime = .4f;

    // Objenin sprite'ý (örn: bir aðaç)
    private SpriteRenderer spriteRenderer;
    // Objenin tilemap'i (örn: bir çatý katmaný)
    private Tilemap tilemap;

    // Script ilk çalýþtýðýnda referanslarý alýr
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        tilemap = GetComponent<Tilemap>();
    }

    // Oyuncu trigger alanýna girdiðinde
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Giren objenin "PlayerController" script'i var mý? (Yani oyuncu mu?)
        if (other.gameObject.GetComponent<PlayerController>())
        {
            // Eðer bu objede SpriteRenderer varsa onu þeffaflaþtýr
            if (spriteRenderer)
            {
                StartCoroutine(FadeRoutine(spriteRenderer, fadeTime, spriteRenderer.color.a, transparencyAmount));
            }
            // Eðer SpriteRenderer yok ama Tilemap varsa onu þeffaflaþtýr
            else if (tilemap)
            {
                StartCoroutine(FadeRoutine(tilemap, fadeTime, tilemap.color.a, transparencyAmount));
            }
        }
    }

    // Oyuncu trigger alanýndan çýktýðýnda
    private void OnTriggerExit2D(Collider2D other)
    {
        // Çýkan obje oyuncu mu?
        if (other.gameObject.GetComponent<PlayerController>())
        {
            // Sprite'ý tekrar tam görünür yap (Alfa = 1f)
            if (spriteRenderer)
            {
                StartCoroutine(FadeRoutine(spriteRenderer, fadeTime, spriteRenderer.color.a, 1f));
            }
            // Tilemap'i tekrar tam görünür yap (Alfa = 1f)
            else if (tilemap)
            {
                StartCoroutine(FadeRoutine(tilemap, fadeTime, tilemap.color.a, 1f));
            }
        }
    }

    // SpriteRenderer için þeffaflaþtýrma Coroutine'i
    private IEnumerator FadeRoutine(SpriteRenderer spriteRenderer, float fadeTime, float startValue, float targetTransparency)
    {
        float elapsedTime = 0;
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            // Rengi saniyeler içinde yumuþak bir geçiþle (Lerp) deðiþtir
            float newAlpha = Mathf.Lerp(startValue, targetTransparency, elapsedTime / fadeTime);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, newAlpha);
            yield return null;
        }
    }

    // Tilemap için þeffaflaþtýrma Coroutine'i (SpriteRenderer ile ayný mantýk)
    private IEnumerator FadeRoutine(Tilemap tilemap, float fadeTime, float startValue, float targetTransparency)
    {
        float elapsedTime = 0;
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startValue, targetTransparency, elapsedTime / fadeTime);
            tilemap.color = new Color(tilemap.color.r, tilemap.color.g, tilemap.color.b, newAlpha);
            yield return null;
        }
    }
}