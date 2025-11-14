using UnityEngine;
using TMPro; // <-- 1. TextMeshPro kullanmak için bunu ekleyin

public class TabelaEtkilesim : MonoBehaviour
{
    [Header("Görsel Ayarlar")]
    [Tooltip("Player yaklaþtýðýnda aktif olacak olan 'Text Balon' objesi.")]
    [SerializeField] private GameObject textBaloonObject; //

    [Tooltip("Metnin yazýlacaðý 'Text (TMP)' bileþeni.")]
    [SerializeField] private TextMeshProUGUI textComponent; // <-- 2. YENÝ ALAN (Text (TMP) objesini buraya atýn)

    [Header("Tabela Metni")]
    [Tooltip("Bu tabelada gösterilecek olan asýl metin.")]
    [SerializeField]
    [TextArea(3, 5)] // Inspector'da 3-5 satýrlýk bir kutu açar
    private string signMessage; // <-- 3. YENÝ ALAN (Metni buraya yazacaksýnýz)

    [Header("Tetikleme Ayarlarý")]
    [Tooltip("Hangi collider'ýn algýlama için kullanýlacaðýný seçin.")]
    [SerializeField] private CapsuleCollider2D triggerAlan;

    // Oyun baþladýðýnda metni ayarla ve baloncuðu gizle
    private void Start()
    {
        // 1. Metni ayarla
        if (textComponent != null)
        {
            textComponent.text = signMessage;
        }

        // 2. Baloncuðu gizle
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