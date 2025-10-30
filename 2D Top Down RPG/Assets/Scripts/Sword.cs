using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // Unity'nin yeni Input System'ini kullanmak i�in bu sat�r gerekli

// Bu script, k�l�� objesinin (veya k�l�c� tutan kolun) �zerinde olmal�.
public class Sword : MonoBehaviour
{
    // --- DE���KENLER ---

    [SerializeField] private GameObject slashAnimPrefab;
    [SerializeField] private Transform slashAnimSpawnPoint;

    // Oyuncu girdilerini (�rn. "Attack" butonu) y�netmek i�in referans.
    private PlayerControls playerControls;

    // K�l�c�n sald�r� animasyonunu oynatmak i�in Animator bile�eni.
    private Animator myAnimator;

    // Oyuncunun ana script'ine (PlayerController) eri�mek i�in referans.
    // Muhtemelen oyuncunun pozisyonunu almak i�in kullan�l�yor.
    private PlayerController playerController;

    // Silah�n kendisini (veya silah� tutan pivot objeyi) d�nd�rmek i�in
    // ActiveWeapon script'ine referans.
    private ActiveWeapon activeWeapon;

    private GameObject slashAnim; 
    // --- UNITY METOTLARI ---

    // Script ilk y�klendi�inde (Start'tan �nce) �al���r.
    // Genellikle referanslar� atamak i�in kullan�l�r.
    private void Awake()
    {
        // Bu objenin Parent (Ebeveyn) objelerinden PlayerController script'ini bul ve ata.
        playerController = GetComponentInParent<PlayerController>();

        // Bu objenin Parent objelerinden ActiveWeapon script'ini bul ve ata.
        activeWeapon = GetComponentInParent<ActiveWeapon>();

        // Bu objenin �zerindeki Animator bile�enini bul ve ata.
        myAnimator = GetComponent<Animator>();

        // Yeni bir PlayerControls nesnesi olu�turarak giri� sistemini ba�lat.
        playerControls = new PlayerControls();
    }

    // Obje veya script aktif oldu�unda �al���r.
    private void OnEnable()
    {
        // Girdi eylemlerini (PlayerControls) dinlemeyi aktifle�tir.
        playerControls.Enable();
    }

    // Obje veya script devre d��� kald���nda �al���r.
    private void OnDisable()
    {
        // Girdi eylemlerini dinlemeyi durdur (performans ve hata �nleme i�in).
        // playerControls.Combat.Attack.started -= _ => Attack(); // Olay aboneli�ini de kald�rmak daha g�venlidir.
        playerControls.Disable();
    }

    // �lk frame g�ncellemesinden hemen �nce �al���r.
    void Start()
    {
        // playerControls i�indeki "Combat" eylem haritas�ndaki "Attack" eylemini dinle.
        // Eylem "started" (ba�lad���nda, �rn. tu�a bas�ld���nda) oldu�unda, 'Attack()' metodunu �a��r.
        playerControls.Combat.Attack.started += _ => Attack();
    }

    // Her frame (kare) �al���r.
    private void Update()
    {
        // Her frame, silah�n fareyi takip etmesini sa�layan metodu �a��r.
        MouseFollowWithOffset();
    }

    // --- �ZEL METOTLAR ---

    // Sald�r� eylemi tetiklendi�inde �a�r�l�r.
    private void Attack()
    {
        // Animator'a "Attack" isimli trigger'� (tetikleyiciyi) g�nder.
        // Bu, animat�rdeki sald�r� animasyonunu ba�lat�r.
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


    // Farenin pozisyonuna g�re silah�n y�n�n� ayarlar.
    private void MouseFollowWithOffset()
    {
        // --- DE����KL�K BURADA ---
        // Eski "Input.mousePosition" yerine Yeni Input System'in kodunu kullan�yoruz.
        Vector3 mousePos = Mouse.current.position.ReadValue();
        // -------------------------

        // Oyuncunun d�nyadaki pozisyonunu, kameradan ekrandaki piksel pozisyonuna �evir.
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(playerController.transform.position);

        // Farenin ekrandaki pozisyonu ile EKRANIN SOL ALT K��ES� (0,0) aras�ndaki a��y� hesapla.
        // D�KKAT: E�er a��y� oyuncuya g�re hesaplamak istiyorsan�z,
        // "Mathf.Atan2(mousePos.y, mousePos.x)" yerine
        // "Mathf.Atan2(mousePos.y - playerScreenPoint.y, mousePos.x - playerScreenPoint.x)"
        // kullanman�z daha do�ru bir davran�� sa�layabilir.
        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

        // E�er fare, oyuncunun ekran pozisyonunun solundaysa:
        if (mousePos.x < playerScreenPoint.x)
        {
            // ActiveWeapon objesini Y ekseninde 180 derece d�nd�r (ters �evir) 
            // ve Z ekseninde hesaplanan 'angle' a��s�n� uygula.
            activeWeapon.transform.rotation = Quaternion.Euler(0, -180, angle);
        }
        else // E�er fare sa�daysa:
        {
            // ActiveWeapon objesini normal tut (Y'de 0 derece)
            // ve Z ekseninde hesaplanan 'angle' a��s�n� uygula.
            activeWeapon.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}