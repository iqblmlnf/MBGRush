using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Tooltip("Tipe tombol: 'Gas' untuk melaju ke depan, 'Brake' untuk mengerem/mundur")]
    public string buttonType;

    private PlayerController player;

    void Start()
    {
        FindPlayerReference();
    }

    private void FindPlayerReference()
    {
        if (player == null)
        {
            GameObject pObj = GameObject.FindWithTag("Player");
            if (pObj != null)
            {
                player = pObj.GetComponent<PlayerController>();
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        FindPlayerReference();
        if (player == null) return;

        if (buttonType == "Gas")
        {
            player.SetMobileInput(1f);
        }
        else if (buttonType == "Brake")
        {
            player.SetMobileInput(-1f);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        FindPlayerReference();
        if (player == null) return;

        player.SetMobileInput(0f);
    }
}
