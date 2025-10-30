// Gerekli Unity kütüphanelerini içeri aktarýr
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 'Sword' adýnda yeni bir class (sýnýf) oluþturur. MonoBehaviour, bu script'in bir Unity objesine eklenebilmesini saðlar.
public class Sword : MonoBehaviour
{
    // PlayerControls (Unity Input System tarafýndan oluþturulan) sýnýfýndan bir deðiþken.
    // Oyuncunun girdilerini (tuþ basýmlarý, mouse hareketleri vb.) yönetir.
    private PlayerControls playerControls;

    // Animasyonlarý kontrol etmek için kullanýlacak Animator bileþeni.
    private Animator myAnimator;

    // Script ilk yüklendiðinde veya obje aktif olduðunda SADECE BÝR KEZ çalýþýr.
    // Genellikle bileþen referanslarýný atamak için kullanýlýr.
    private void Awake()
    {
        // Bu script'in eklendiði objenin üzerindeki Animator bileþenini bulur ve 'myAnimator' deðiþkenine atar.
        myAnimator = GetComponent<Animator>();

        // Yeni bir PlayerControls nesnesi oluþturur (Input System'i kullanýma hazýrlar).
        playerControls = new PlayerControls();
    }

    // Bu script (veya baðlý olduðu obje) aktif hale geldiðinde çalýþýr.
    private void OnEnable()
    {
        // Oyuncu girdilerini dinlemeyi aktifleþtirir.
        playerControls.Enable();
    }

    // Ýlk frame güncellemesinden hemen önce SADECE BÝR KEZ çalýþýr.
    // Genellikle diðer script'lerle baðlantý kurmak veya baþlangýç ayarlarý için kullanýlýr.
    void Start()
    {
        // 'playerControls' içindeki 'Combat' (Dövüþ) eylem haritasýndaki 'Attack' (Saldýrý) eylemini dinler.
        // 'started' olayý, oyuncu bu eylemi baþlattýðý anda (örn. tuþa bastýðý anda) tetiklenir.
        // Tetiklendiðinde, 'Attack()' metodunu çaðýrýr. 
        // `_ =>` kýsmý, "olaydan gelen parametreleri (varsa) umursama, sadece metodu çaðýr" anlamýna gelir.
        playerControls.Combat.Attack.started += _ => Attack();
    }

    // Saldýrý eylemi gerçekleþtiðinde çaðrýlacak olan özel metot.
    private void Attack()
    {
        // Animator bileþenine "Attack" isminde bir trigger (tetikleyici) gönderir.
        // Bu, Animator'da tanýmlý olan saldýrý animasyonunu baþlatýr.
        myAnimator.SetTrigger("Attack");
    }

    // Not: Kodda OnDisable() metodu eksik. 
    // Objeyi veya script'i kapattýðýnýzda girdileri dinlemeyi durdurmak iyi bir pratiktir:
    /*
    private void OnDisable()
    {
        // Oyuncu girdilerini dinlemeyi devre dýþý býrakýr.
        playerControls.Disable();
    }
    */
}