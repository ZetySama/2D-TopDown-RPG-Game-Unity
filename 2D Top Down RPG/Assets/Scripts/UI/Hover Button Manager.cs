using UnityEngine;
using UnityEngine.UI; // Image kullanmak için
using UnityEngine.EventSystems; // EventTrigger (hover) olaylarý için
using System.Collections.Generic; // List kullanmak için

public class HoverMenuManager : MonoBehaviour
{
    [Tooltip("Üzerine gelince child objesini açacak olan buton (veya UI) objeleri.")]
    [SerializeField]
    private List<GameObject> menuButtons;

    [Tooltip("Butonlarýn arkasýnda duran ana panel veya arka plan Image'ý. " +
             "Mouse buradan ayrýldýðýnda tüm pencereleri kapatýr.")]
    [SerializeField]
    private Image backgroundPanel; // Veya GameObject olarak da ayarlayabilirsiniz

    // Butonlarýn altýndaki child objeleri hafýzada tutmak için bir liste
    private List<GameObject> childObjects;

    void Awake()
    {
        childObjects = new List<GameObject>();

        // 1. ADIM: Tüm butonlarý ve arka planý ayarla
        SetupButtons();
        SetupBackground();
    }

    void SetupButtons()
    {
        foreach (GameObject buttonGO in menuButtons)
        {
            if (buttonGO == null) continue;

            // 2. ADIM: Butonun altýndaki ilk child objeyi bul
            if (buttonGO.transform.childCount == 0)
            {
                Debug.LogWarning(buttonGO.name + " objesinin bir child (alt) objesi yok!");
                continue;
            }

            GameObject child = buttonGO.transform.GetChild(0).gameObject;
            childObjects.Add(child);

            // 3. ADIM: Baþlangýçta tüm child'larý kapat
            child.SetActive(false);

            // 4. ADIM: Her butona 'EventTrigger' (Olay Tetikleyici) ekle
            EventTrigger trigger = buttonGO.GetComponent<EventTrigger>();
            if (trigger == null)
            {
                trigger = buttonGO.AddComponent<EventTrigger>();
            }

            // 'PointerEnter' (Mouse Üzerine Geldiðinde) olayýný oluþtur
            EventTrigger.Entry pointerEnterEntry = new EventTrigger.Entry();
            pointerEnterEntry.eventID = EventTriggerType.PointerEnter;

            // Olay tetiklendiðinde 'ActivateChild' fonksiyonunu çaðýr (ve hangi child'ý açacaðýný bildir)
            pointerEnterEntry.callback.AddListener((data) => { ActivateChild(child); });

            trigger.triggers.Add(pointerEnterEntry);
        }
    }

    void SetupBackground()
    {
        // 5. ADIM: Arka plana da bir 'PointerEnter' olayý ekle
        // (Mouse butonlardan çýkýp arka plana gelince hepsi kapanacak)
        if (backgroundPanel == null) return;

        EventTrigger trigger = backgroundPanel.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = backgroundPanel.gameObject.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry bgEntry = new EventTrigger.Entry();
        bgEntry.eventID = EventTriggerType.PointerEnter;

        // Arka plana gelince 'DeactivateAllChildren' fonksiyonunu çaðýr
        bgEntry.callback.AddListener((data) => { DeactivateAllChildren(); });

        trigger.triggers.Add(bgEntry);
    }

    // Bir butonun üzerine gelindiðinde çaðrýlýr
    private void ActivateChild(GameObject childToActivate)
    {
        // Tüm child objeleri döngüye al
        foreach (GameObject child in childObjects)
        {
            if (child == null) continue;

            // Eðer bu, aktif olmasý gereken child ise AÇ, deðilse KAPAT.
            // Bu satýr "sadece 1 tanesi aktif olacak" kuralýný saðlar.
            child.SetActive(child == childToActivate);
        }
    }

    // Mouse arka plana geldiðinde çaðrýlýr
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