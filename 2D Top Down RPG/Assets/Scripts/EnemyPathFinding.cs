using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Düþmanýn fiziksel hareketini ve yol bulmasýný yöneten sýnýf.
public class EnemyPathfinding : MonoBehaviour
{
    // [SerializeField] sayesinde bu özel (private) deðiþkeni Unity editöründe (Inspector) görebilir ve deðiþtirebiliriz.
    [SerializeField] private float moveSpeed = 2f; // Düþmanýn hareket hýzý.

    // Fiziksel hareketler için Rigidbody2D bileþeni gereklidir.
    private Rigidbody2D rb;
    // Düþmanýn o an hareket ettiði yönü saklayan vektör.
    private Vector2 moveDir;

    // Oyun baþladýðýnda veya nesne aktif olduðunda bir kez çalýþýr.
    private void Awake()
    {
        // Bu objeye ekli olan "Rigidbody2D" bileþenini bul ve deðiþkene ata.
        rb = GetComponent<Rigidbody2D>();
    }

    // Fizik güncellemeleri için kullanýlan, sabit aralýklarla (default 0.02sn) çaðrýlan fonksiyon.
    private void FixedUpdate()
    {
        // Rigidbody'nin pozisyonunu, mevcut pozisyonuna yön * hýz * zaman ekleyerek güncelle.
        // Time.fixedDeltaTime, son FixedUpdate'den bu yana geçen süreyi verir, bu da hareketi kare hýzýndan (FPS) baðýmsýz ve pürüzsüz hale getirir.
        rb.MovePosition(rb.position + moveDir * (moveSpeed * Time.fixedDeltaTime));
    }

    // Dýþarýdan (EnemyAI script'inden) çaðrýlabilen public bir fonksiyon.
    // Düþmanýn gitmesi gereken yeni hedefi (yönü) ayarlar.
    public void MoveTo(Vector2 targetPosition)
    {
        // Gelen hedef pozisyonunu (aslýnda bir yön vektörü) moveDir deðiþkenine ata.
        // FixedUpdate bu deðiþkeni kullanarak hareketi gerçekleþtirecek.
        moveDir = targetPosition;
    }
}