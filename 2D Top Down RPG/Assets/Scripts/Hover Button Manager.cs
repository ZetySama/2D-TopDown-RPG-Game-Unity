using UnityEngine;
using UnityEngine.UI; // Image kullanmak i�in
using UnityEngine.EventSystems; // EventTrigger (hover) olaylar� i�in
using System.Collections.Generic; // List kullanmak i�in

public class HoverMenuManager : MonoBehaviour
{
    [Tooltip("�zerine gelince child objesini a�acak olan buton (veya UI) objeleri.")]
    [SerializeField]
    private List<GameObject> menuButtons;

    [Tooltip("Butonlar�n arkas�nda duran ana panel veya arka plan Image'�. " +
             "Mouse buradan ayr�ld���nda t�m pencereleri kapat�r.")]
    [SerializeField]
    private Image backgroundPanel; // Veya GameObject olarak da ayarlayabilirsiniz

    // Butonlar�n alt�ndaki child objeleri haf�zada tutmak i�in bir liste
    private List<GameObject> childObjects;

    void Awake()
    {
        childObjects = new List<GameObject>();

        // 1. ADIM: T�m butonlar� ve arka plan� ayarla
        SetupButtons();
        SetupBackground();
    }

    void SetupButtons()
    {
        foreach (GameObject buttonGO in menuButtons)
        {
            if (buttonGO == null) continue;

            // 2. ADIM: Butonun alt�ndaki ilk child objeyi bul
            if (buttonGO.transform.childCount == 0)
            {
                Debug.LogWarning(buttonGO.name + " objesinin bir child (alt) objesi yok!");
                continue;
            }

            GameObject child = buttonGO.transform.GetChild(0).gameObject;
            childObjects.Add(child);

            // 3. ADIM: Ba�lang��ta t�m child'lar� kapat
            child.SetActive(false);

            // 4. ADIM: Her butona 'EventTrigger' (Olay Tetikleyici) ekle
            EventTrigger trigger = buttonGO.GetComponent<EventTrigger>();
            if (trigger == null)
            {
                trigger = buttonGO.AddComponent<EventTrigger>();
            }

            // 'PointerEnter' (Mouse �zerine Geldi�inde) olay�n� olu�tur
            EventTrigger.Entry pointerEnterEntry = new EventTrigger.Entry();
            pointerEnterEntry.eventID = EventTriggerType.PointerEnter;

            // Olay tetiklendi�inde 'ActivateChild' fonksiyonunu �a��r (ve hangi child'� a�aca��n� bildir)
            pointerEnterEntry.callback.AddListener((data) => { ActivateChild(child); });

            trigger.triggers.Add(pointerEnterEntry);
        }
    }

    void SetupBackground()
    {
        // 5. ADIM: Arka plana da bir 'PointerEnter' olay� ekle
        // (Mouse butonlardan ��k�p arka plana gelince hepsi kapanacak)
        if (backgroundPanel == null) return;

        EventTrigger trigger = backgroundPanel.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = backgroundPanel.gameObject.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry bgEntry = new EventTrigger.Entry();
        bgEntry.eventID = EventTriggerType.PointerEnter;

        // Arka plana gelince 'DeactivateAllChildren' fonksiyonunu �a��r
        bgEntry.callback.AddListener((data) => { DeactivateAllChildren(); });

        trigger.triggers.Add(bgEntry);
    }

    // Bir butonun �zerine gelindi�inde �a�r�l�r
    private void ActivateChild(GameObject childToActivate)
    {
        // T�m child objeleri d�ng�ye al
        foreach (GameObject child in childObjects)
        {
            if (child == null) continue;

            // E�er bu, aktif olmas� gereken child ise A�, de�ilse KAPAT.
            // Bu sat�r "sadece 1 tanesi aktif olacak" kural�n� sa�lar.
            child.SetActive(child == childToActivate);
        }
    }

    // Mouse arka plana geldi�inde �a�r�l�r
    private void DeactivateAllChildren()
    {
        foreach (GameObject child in childObjects)
        {
            if (child != null)
            {
                child.SetActive(false);
            }
        }
    }
}