/*using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class Platform : MonoBehaviour
{
    public static event Action<Platform> OnPlatformClicked;
    [SerializeField] private LayerMask platformLayerMask;
    public static bool towerPanelOpen { get; set; } = false;

    private void Update()
    {
        if (towerPanelOpen) return;

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            RaycastHit2D raycastHit = Physics2D.Raycast(worldPoint, Vector2.zero, Mathf.Infinity, platformLayerMask);

            if (raycastHit.collider != null)
            {
                Platform platform = raycastHit.collider.GetComponent<Platform>();
                if (platform != null)
                {
                    OnPlatformClicked?.Invoke(platform);
                }
            }
        }
    }

    public void PlaceTower(TowerData data)
    {
        Instantiate(data.prefab, transform.position, Quaternion.identity, transform);
    }
}*/
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class Platform : MonoBehaviour
{
    public static event Action<Platform> OnPlatformClicked;
    [SerializeField] private LayerMask platformLayerMask;
    public static bool towerPanelOpen { get; set; } = false;

    private GameObject _currentTower;

    private void Update()
    {
        if (towerPanelOpen) return;

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            RaycastHit2D raycastHit = Physics2D.Raycast(worldPoint, Vector2.zero, Mathf.Infinity, platformLayerMask);

            if (raycastHit.collider != null)
            {
                Platform platform = raycastHit.collider.GetComponent<Platform>();
                if (platform != null)
                {
                    OnPlatformClicked?.Invoke(platform);
                }
            }
        }
    }

    public void PlaceTower(TowerData data)
    {
        if (_currentTower != null)
        {
            Destroy(_currentTower);
        }
        
        _currentTower = Instantiate(data.prefab, transform.position, Quaternion.identity, transform);
    }
}


