using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// D��man�n yapay zeka davran��lar�n� y�neten ana s�n�f.
public class EnemyAI : MonoBehaviour
{
    // D��man�n o an i�inde bulunabilece�i durumlar� tan�mlar.
    // �imdilik sadece "Roaming" (Gezinme) durumu var.
    private enum State
    {
        Roaming
    }

    // D��man�n mevcut durumunu (state) saklamak i�in bir de�i�ken.
    private State state;
    // D��man�n fiziksel hareketini y�netecek olan script'e eri�im i�in.
    private EnemyPathfinding enemyPathfinding;

    // Oyun ba�lad���nda veya nesne aktif oldu�unda bir kez �al���r.
    private void Awake()
    {
        // Bu objeye ekli olan "EnemyPathfinding" bile�enini bul ve de�i�kene ata.
        enemyPathfinding = GetComponent<EnemyPathfinding>();
        // Ba�lang�� durumunu "Roaming" olarak ayarla.
        state = State.Roaming;
    }

    // Awake'den sonra ve ilk Update'den �nce bir kez �al���r.
    private void Start()
    {
        // Gezinme davran���n� y�netecek olan Coroutine'i (e� zamanl� fonksiyon) ba�lat.
        StartCoroutine(RoamingRoutine());
    }

    // Belirli aral�klarla tekrarlanan gezinme i�lemini y�netir.
    private IEnumerator RoamingRoutine()
    {
        // D��man�n durumu "Roaming" oldu�u s�rece bu d�ng� devam eder.
        while (state == State.Roaming)
        {
            // Gidilecek rastgele yeni bir pozisyon belirle.
            Vector2 roamPosition = GetRoamingPosition();

            // Hareket script'ine (EnemyPathfinding) yeni hedef pozisyonu ilet.
            enemyPathfinding.MoveTo(roamPosition);

            // Yeni bir pozisyona gitmeden �nce 2 saniye bekle.
            // Bu, d��man�n s�rekli titremesini engeller ve ona "d���nme" s�resi verir.
            yield return new WaitForSeconds(2f);
        }
    }

    // Rastgele bir y�n vekt�r� olu�turan yard�mc� bir fonksiyon.
    private Vector2 GetRoamingPosition()
    {
        // X ve Y eksenlerinde -1 ile +1 aras�nda rastgele bir de�er se�.
        // .normalized ile bu vekt�r�n uzunlu�unu 1 birim yapar�z (sadece y�n� kal�r).
        // Bu, d��man�n her zaman ayn� h�zda ama farkl� y�nlere gitmesini sa�lar.
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
}