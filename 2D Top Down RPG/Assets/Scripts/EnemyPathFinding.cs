using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// D��man�n fiziksel hareketini ve yol bulmas�n� y�neten s�n�f.
public class EnemyPathfinding : MonoBehaviour
{
    // [SerializeField] sayesinde bu �zel (private) de�i�keni Unity edit�r�nde (Inspector) g�rebilir ve de�i�tirebiliriz.
    [SerializeField] private float moveSpeed = 2f; // D��man�n hareket h�z�.

    // Fiziksel hareketler i�in Rigidbody2D bile�eni gereklidir.
    private Rigidbody2D rb;
    // D��man�n o an hareket etti�i y�n� saklayan vekt�r.
    private Vector2 moveDir;

    // Oyun ba�lad���nda veya nesne aktif oldu�unda bir kez �al���r.
    private void Awake()
    {
        // Bu objeye ekli olan "Rigidbody2D" bile�enini bul ve de�i�kene ata.
        rb = GetComponent<Rigidbody2D>();
    }

    // Fizik g�ncellemeleri i�in kullan�lan, sabit aral�klarla (default 0.02sn) �a�r�lan fonksiyon.
    private void FixedUpdate()
    {
        // Rigidbody'nin pozisyonunu, mevcut pozisyonuna y�n * h�z * zaman ekleyerek g�ncelle.
        // Time.fixedDeltaTime, son FixedUpdate'den bu yana ge�en s�reyi verir, bu da hareketi kare h�z�ndan (FPS) ba��ms�z ve p�r�zs�z hale getirir.
        rb.MovePosition(rb.position + moveDir * (moveSpeed * Time.fixedDeltaTime));
    }

    // D��ar�dan (EnemyAI script'inden) �a�r�labilen public bir fonksiyon.
    // D��man�n gitmesi gereken yeni hedefi (y�n�) ayarlar.
    public void MoveTo(Vector2 targetPosition)
    {
        // Gelen hedef pozisyonunu (asl�nda bir y�n vekt�r�) moveDir de�i�kenine ata.
        // FixedUpdate bu de�i�keni kullanarak hareketi ger�ekle�tirecek.
        moveDir = targetPosition;
    }
}