using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // Unity'nin yeni Input System'ini kullanmak için bu satýr gerekli

// Bu script, kýlýç objesinin (veya kýlýcý tutan kolun) üzerinde olmalý.
public class Sword : MonoBehaviour
{
    // --- DEÐÝÞKENLER ---

    [SerializeField] private GameObject slashAnimPrefab;
    [SerializeField] private Transform slashAnimSpawnPoint;
    [SerializeField] private Transform weaponCollider;

    // --- SES EFEKTÝ ÝÇÝN YENÝ DEÐÝÞKENLER ---
    [Tooltip("Kýlýç sallama ses klibi")]
    [SerializeField] private AudioClip swordSwingSound; // <-- YENÝ EKLENDÝ
    private AudioSource myAudioSource; // <-- YENÝ EKLENDÝ
    // ------------------------------------

    // Oyuncu girdilerini (örn. "Attack" butonu) yönetmek için referans.
    private PlayerControls playerControls;

    // Kýlýcýn saldýrý animasyonunu oynatmak için Animator bileþeni.
    private Animator myAnimator;

    // Oyuncunun ana script'ine (PlayerController) eriþmek için referans.
    private PlayerController playerController;

    // Silahýn kendisini (veya silahý tutan pivot objeyi) döndürmek için
    // ActiveWeapon script'ine referans.
    private ActiveWeapon activeWeapon;

    private GameObject slashAnim;
    // --- UNITY METOTLARI ---

    private void Awake()
    {
        // Bu objenin Parent (Ebeveyn) objelerinden PlayerController script'ini bul ve ata.
        playerController = GetComponentInParent<PlayerController>();

        // Bu objenin Parent objelerinden ActiveWeapon script'ini bul ve ata.
        activeWeapon = GetComponentInParent<ActiveWeapon>();

        // Bu objenin üzerindeki Animator bileþenini bul ve ata.
        myAnimator = GetComponent<Animator>();

        // Yeni bir PlayerControls nesnesi oluþturarak giriþ sistemini baþlat.
        playerControls = new PlayerControls();

        // --- SES EFEKTÝ ÝÇÝN YENÝ SATIR ---
        // Sesi çalacak olan AudioSource bileþenini ana Player objesinden bul.
        myAudioSource = GetComponentInParent<AudioSource>(); // <-- YENÝ EKLENDÝ
    }

    // Obje veya script aktif olduðunda çalýþýr.
    private void OnEnable()
    {
        // Girdi eylemlerini (PlayerControls) dinlemeyi aktifleþtir.
        playerControls.Enable();
    }

    // Obje veya script devre dýþý kaldýðýnda çalýþýr.
    private void OnDisable()
    {
        // Girdi eylemlerini dinlemeyi durdur.
        playerControls.Disable();
    }

    // Ýlk frame güncellemesinden hemen önce çalýþýr.
    void Start()
    {
        // Attack eylemini dinle.
        playerControls.Combat.Attack.started += _ => Attack();
    }

    // Her frame (kare) çalýþýr.
    private void Update()
    {
        // Her frame, silahýn fareyi takip etmesini saðlayan metodu çaðýr.
        MouseFollowWithOffset();
    }

    // --- ÖZEL METOTLAR ---

    // Saldýrý eylemi tetiklendiðinde çaðrýlýr.
    private void Attack()
    {
        // Animasyonu tetikle
        myAnimator.SetTrigger("Attack");
        // Silahýn çarpýþtýrýcýsýný (hasar vuran alaný) aktifleþtir
        weaponCollider.gameObject.SetActive(true);

        // Kýlýç vuruþu "slash" efektini oluþtur (Instantiate)
        slashAnim = Instantiate(slashAnimPrefab, slashAnimSpawnPoint.position, Quaternion.identity);
        slashAnim.transform.parent = this.transform.parent;

        // --- SES EFEKTÝ ÝÇÝN YENÝ KOD ---
        // Ses klibi atanmýþsa ve AudioSource bulunmuþsa, sesi çal.
        if (swordSwingSound != null && myAudioSource != null)
        {
            myAudioSource.PlayOneShot(swordSwingSound); // <-- YENÝ EKLENDÝ
        }
        // --------------------------------
    }

    // Bu fonksiyon, animasyonun "DoneAttacking" event'i tarafýndan çaðrýlýr.
    public void DoneAttackingAnimEvent()
    {
        weaponCollider.gameObject.SetActive(false);
    }

    // Bu fonksiyon, animasyonun "SwingUpFlip" event'i tarafýndan çaðrýlýr.
    public void SwingUpFlipAnimEvent()
    {
        slashAnim.gameObject.transform.rotation = Quaternion.Euler(-180, 0, 0);

        if (playerController.FacingLeft)
        {
            slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    // Bu fonksiyon, animasyonun "SwingDownFlip" event'i tarafýndan çaðrýlýr.
    public void SwingDownFlipAnimEvent()
    {
        slashAnim.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

        if (playerController.FacingLeft)
        {
            slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        }
    }


    // Farenin pozisyonuna göre silahýn yönünü ayarlar.
    private void MouseFollowWithOffset()
    {
        // Yeni Input System ile mouse pozisyonunu al
        Vector3 mousePos = Mouse.current.position.ReadValue();

        // Oyuncunun dünyadaki pozisyonunu ekrandaki piksel pozisyonuna çevir.
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(playerController.transform.position);

        // Açý hesaplamasý
        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

        // Fare, oyuncunun solundaysa
        if (mousePos.x < playerScreenPoint.x)
        {
            activeWeapon.transform.rotation = Quaternion.Euler(0, -180, angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        else // Fare, oyuncunun saðýndaysa
        {
            activeWeapon.transform.rotation = Quaternion.Euler(0, 0, angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}