using UnityEngine;

public class TabelaEtkilesim : MonoBehaviour
{
    [Tooltip("Player yaklaþtýðýnda aktif olacak olan 'Text Balon' objesi.")]
    [SerializeField] private GameObject textBaloonObject;

    [Tooltip("Hangi collider'ýn algýlama için kullanýlacaðýný seçin (Capsule Collider 2D).")]
    [SerializeField] private CapsuleCollider2D triggerAlan;
    // Not: Bu satýrý eklemek, hangi collider'ýn ne iþ yaptýðýný garantiler.

    // Oyun baþladýðýnda baloncuðu gizle
    private void Start()
    {
        if (textBaloonObject != null)
        {
            textBaloonObject.SetActive(false);
        }
    }

    /// <summary>
    /// Bir collider bu objenin trigger alanýna girdiðinde çalýþýr.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Giren objenin "Player" olduðundan emin ol (Tag kontrolü)
        if (other.CompareTag("Player"))
        {
            // Baloncuðu göster
            if (textBaloonObject != null)
            {
                textBaloonObject.SetActive(true);
            }
        }
    }

    /// <summary>
    /// Bir collider bu objenin trigger alanýndan çýktýðýnda çalýþýr.
    /// </summary>
    private void OnTriggerExit2D(Collider2D other)
    {
        // Çýkan objenin "Player" olduðundan emin ol
        if (other.CompareTag("Player"))
        {
            // Baloncuðu tekrar gizle
            if (textBaloonObject != null)
            {
                textBaloonObject.SetActive(false);
            }
        }
    }
}