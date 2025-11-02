using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Düþmanýn yapay zeka davranýþlarýný yöneten ana sýnýf.
public class EnemyAI : MonoBehaviour
{
    // Düþmanýn o an içinde bulunabileceði durumlarý tanýmlar.
    // Þimdilik sadece "Roaming" (Gezinme) durumu var.
    private enum State
    {
        Roaming
    }

    // Düþmanýn mevcut durumunu (state) saklamak için bir deðiþken.
    private State state;
    // Düþmanýn fiziksel hareketini yönetecek olan script'e eriþim için.
    private EnemyPathfinding enemyPathfinding;

    // Oyun baþladýðýnda veya nesne aktif olduðunda bir kez çalýþýr.
    private void Awake()
    {
        // Bu objeye ekli olan "EnemyPathfinding" bileþenini bul ve deðiþkene ata.
        enemyPathfinding = GetComponent<EnemyPathfinding>();
        // Baþlangýç durumunu "Roaming" olarak ayarla.
        state = State.Roaming;
    }

    // Awake'den sonra ve ilk Update'den önce bir kez çalýþýr.
    private void Start()
    {
        // Gezinme davranýþýný yönetecek olan Coroutine'i (eþ zamanlý fonksiyon) baþlat.
        StartCoroutine(RoamingRoutine());
    }

    // Belirli aralýklarla tekrarlanan gezinme iþlemini yönetir.
    private IEnumerator RoamingRoutine()
    {
        // Düþmanýn durumu "Roaming" olduðu sürece bu döngü devam eder.
        while (state == State.Roaming)
        {
            // Gidilecek rastgele yeni bir pozisyon belirle.
            Vector2 roamPosition = GetRoamingPosition();

            // Hareket script'ine (EnemyPathfinding) yeni hedef pozisyonu ilet.
            enemyPathfinding.MoveTo(roamPosition);

            // Yeni bir pozisyona gitmeden önce 2 saniye bekle.
            // Bu, düþmanýn sürekli titremesini engeller ve ona "düþünme" süresi verir.
            yield return new WaitForSeconds(2f);
        }
    }

    // Rastgele bir yön vektörü oluþturan yardýmcý bir fonksiyon.
    private Vector2 GetRoamingPosition()
    {
        // X ve Y eksenlerinde -1 ile +1 arasýnda rastgele bir deðer seç.
        // .normalized ile bu vektörün uzunluðunu 1 birim yaparýz (sadece yönü kalýr).
        // Bu, düþmanýn her zaman ayný hýzda ama farklý yönlere gitmesini saðlar.
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
}