using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TowerCard : MonoBehaviour
{
    [SerializeField] private Image towerImage;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private TMP_Text descriptionText;

    private TowerData _towerData;
    public static event Action<TowerData> OnTowerSelected;

    public void Initialize(TowerData data)
    {
        _towerData = data;
        towerImage.sprite = data.sprite;
        costText.text = data.cost.ToString();
        descriptionText.text = data.description;
    }

    public void PlaceTower()
    {
        OnTowerSelected?.Invoke(_towerData);
    }
}
