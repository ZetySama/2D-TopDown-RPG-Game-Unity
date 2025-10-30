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

    // Oyuncu girdilerini (örn. "Attack" butonu) yönetmek için referans.
    private PlayerControls playerControls;

    // Kýlýcýn saldýrý animasyonunu oynatmak için Animator bileþeni.
    private Animator myAnimator;

    // Oyuncunun ana script'ine (PlayerController) eriþmek için referans.
    // Muhtemelen oyuncunun pozisyonunu almak için kullanýlýyor.
    private PlayerController playerController;

    // Silahýn kendisini (veya silahý tutan pivot objeyi) döndürmek için
    // ActiveWeapon script'ine referans.
    private ActiveWeapon activeWeapon;

    private GameObject slashAnim; 
    // --- UNITY METOTLARI ---

    // Script ilk yüklendiðinde (Start'tan önce) çalýþýr.
    // Genellikle referanslarý atamak için kullanýlýr.
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
        // Girdi eylemlerini dinlemeyi durdur (performans ve hata önleme için).
        // playerControls.Combat.Attack.started -= _ => Attack(); // Olay aboneliðini de kaldýrmak daha güvenlidir.
        playerControls.Disable();
    }

    // Ýlk frame güncellemesinden hemen önce çalýþýr.
    void Start()
    {
        // playerControls içindeki "Combat" eylem haritasýndaki "Attack" eylemini dinle.
        // Eylem "started" (baþladýðýnda, örn. tuþa basýldýðýnda) olduðunda, 'Attack()' metodunu çaðýr.
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
        // Animator'a "Attack" isimli trigger'ý (tetikleyiciyi) gönder.
        // Bu, animatördeki saldýrý animasyonunu baþlatýr.
        myAnimator.SetTrigger("Attack");
        slashAnim = Instantiate(slashAnimPrefab, slashAnimSpawnPoint.position, Quaternion.identity); 
        slashAnim.transform.parent = this.transform.parent; 
    }

    public void SwingUpFlipAnim()
    {
        slashAnim.gameObject.transform.rotation = Quaternion.Euler(-180, 0, 0);

        if (playerController.FacingLeft)
        {
            slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    public void SwingDownFlipAnim()
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
        // --- DEÐÝÞÝKLÝK BURADA ---
        // Eski "Input.mousePosition" yerine Yeni Input System'in kodunu kullanýyoruz.
        Vector3 mousePos = Mouse.current.position.ReadValue();
        // -------------------------

        // Oyuncunun dünyadaki pozisyonunu, kameradan ekrandaki piksel pozisyonuna çevir.
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(playerController.transform.position);

        // Farenin ekrandaki pozisyonu ile EKRANIN SOL ALT KÖÞESÝ (0,0) arasýndaki açýyý hesapla.
        // DÝKKAT: Eðer açýyý oyuncuya göre hesaplamak istiyorsanýz,
        // "Mathf.Atan2(mousePos.y, mousePos.x)" yerine
        // "Mathf.Atan2(mousePos.y - playerScreenPoint.y, mousePos.x - playerScreenPoint.x)"
        // kullanmanýz daha doðru bir davranýþ saðlayabilir.
        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

        // Eðer fare, oyuncunun ekran pozisyonunun solundaysa:
        if (mousePos.x < playerScreenPoint.x)
        {
            // ActiveWeapon objesini Y ekseninde 180 derece döndür (ters çevir) 
            // ve Z ekseninde hesaplanan 'angle' açýsýný uygula.
            activeWeapon.transform.rotation = Quaternion.Euler(0, -180, angle);
        }
        else // Eðer fare saðdaysa:
        {
            // ActiveWeapon objesini normal tut (Y'de 0 derece)
            // ve Z ekseninde hesaplanan 'angle' açýsýný uygula.
            activeWeapon.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}