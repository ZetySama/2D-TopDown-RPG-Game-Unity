// Gerekli Unity k�t�phanelerini i�eri aktar�r
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 'Sword' ad�nda yeni bir class (s�n�f) olu�turur. MonoBehaviour, bu script'in bir Unity objesine eklenebilmesini sa�lar.
public class Sword : MonoBehaviour
{
    // PlayerControls (Unity Input System taraf�ndan olu�turulan) s�n�f�ndan bir de�i�ken.
    // Oyuncunun girdilerini (tu� bas�mlar�, mouse hareketleri vb.) y�netir.
    private PlayerControls playerControls;

    // Animasyonlar� kontrol etmek i�in kullan�lacak Animator bile�eni.
    private Animator myAnimator;

    // Script ilk y�klendi�inde veya obje aktif oldu�unda SADECE B�R KEZ �al���r.
    // Genellikle bile�en referanslar�n� atamak i�in kullan�l�r.
    private void Awake()
    {
        // Bu script'in eklendi�i objenin �zerindeki Animator bile�enini bulur ve 'myAnimator' de�i�kenine atar.
        myAnimator = GetComponent<Animator>();

        // Yeni bir PlayerControls nesnesi olu�turur (Input System'i kullan�ma haz�rlar).
        playerControls = new PlayerControls();
    }

    // Bu script (veya ba�l� oldu�u obje) aktif hale geldi�inde �al���r.
    private void OnEnable()
    {
        // Oyuncu girdilerini dinlemeyi aktifle�tirir.
        playerControls.Enable();
    }

    // �lk frame g�ncellemesinden hemen �nce SADECE B�R KEZ �al���r.
    // Genellikle di�er script'lerle ba�lant� kurmak veya ba�lang�� ayarlar� i�in kullan�l�r.
    void Start()
    {
        // 'playerControls' i�indeki 'Combat' (D�v��) eylem haritas�ndaki 'Attack' (Sald�r�) eylemini dinler.
        // 'started' olay�, oyuncu bu eylemi ba�latt��� anda (�rn. tu�a bast��� anda) tetiklenir.
        // Tetiklendi�inde, 'Attack()' metodunu �a��r�r. 
        // `_ =>` k�sm�, "olaydan gelen parametreleri (varsa) umursama, sadece metodu �a��r" anlam�na gelir.
        playerControls.Combat.Attack.started += _ => Attack();
    }

    // Sald�r� eylemi ger�ekle�ti�inde �a�r�lacak olan �zel metot.
    private void Attack()
    {
        // Animator bile�enine "Attack" isminde bir trigger (tetikleyici) g�nderir.
        // Bu, Animator'da tan�ml� olan sald�r� animasyonunu ba�lat�r.
        myAnimator.SetTrigger("Attack");
    }

    // Not: Kodda OnDisable() metodu eksik. 
    // Objeyi veya script'i kapatt���n�zda girdileri dinlemeyi durdurmak iyi bir pratiktir:
    /*
    private void OnDisable()
    {
        // Oyuncu girdilerini dinlemeyi devre d��� b�rak�r.
        playerControls.Disable();
    }
    */
}