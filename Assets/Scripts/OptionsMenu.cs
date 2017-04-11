using UnityEngine;

public class OptionsMenu : MonoBehaviour {

    [SerializeField]
    private GameObject optionsMenu;
    private bool menuOn;

    private void Start()
    {
        if (!optionsMenu) optionsMenu = GameObject.Find("OptionsMenu");
        ToggleMenu(false);
        
    }

    private void OnEnable()
    {
        EventManager em = EventManager.Instance;
        if (em)
        {
            em.AddListener<EndTurnEvent>(OnEndTurn);
            em.AddListener<OptionsMenuEvent>(OnMenuToggle);
        }
    }

    private void OnDisable()
    {
        EventManager em = EventManager.Instance;
        if (em)
        {
            em.RemoveListener<OptionsMenuEvent>(OnMenuToggle);
            em.RemoveListener<EndTurnEvent>(OnEndTurn);
        }
    }

    private void OnEndTurn(EndTurnEvent e)
    {
        ToggleMenu(false);
    }

    private void OnMenuToggle(OptionsMenuEvent e)
    {
        if (e.ForceOff)
        {
            ToggleMenu(false);
            return;
        }
        ToggleMenu(!menuOn);
        if (menuOn)
        {
            optionsMenu.transform.position = AdjustedMousePosition();
        }
    }

    private Vector3 AdjustedMousePosition()
    {
        Vector3 position = Input.mousePosition;
        Vector2 rect = new Vector2(
            optionsMenu.GetComponent<RectTransform>().rect.width,
            optionsMenu.GetComponent<RectTransform>().rect.height);
        Vector2 limitsPos = position;
        limitsPos.x += rect.x;
        limitsPos.y -= rect.y;
        position.x += rect.x / 2;
        return position + OffScreenAdjustment(limitsPos, rect.x, rect.y);
    }

    private Vector3 OffScreenAdjustment(Vector2 pos, float width, float height)
    {
        Vector3 adjustment = Vector3.zero;
        if (pos.x > Screen.width)
        {
            adjustment.x -= (pos.x - Screen.width);
        }
        if (pos.y < 0)
        {
            adjustment.y -= (pos.y);
        }
        return adjustment;
    }

    private void ToggleMenu(bool menuOn)
    {
        this.menuOn = menuOn;
        if (optionsMenu) optionsMenu.SetActive(menuOn);
    }
}
