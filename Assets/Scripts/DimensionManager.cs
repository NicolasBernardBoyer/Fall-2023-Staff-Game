using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimensionManager : MonoBehaviour
{
    private static DimensionManager instance = new DimensionManager();

    [SerializeField] private GameObject baseDim;
    [SerializeField] private GameObject alternateDim;

    public List<GameObject> baseDimEnemies;
    public List<GameObject> alternateDimEnemies;

    [SerializeField] private bool isAlternate = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            baseDim.SetActive(true);
            alternateDim.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Multiple UIManager instances found. Destroying the extra one.");
            Destroy(gameObject);
        }

        baseDimEnemies = new List<GameObject>();
        alternateDimEnemies = new List<GameObject>();
    }

    public static DimensionManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new DimensionManager();
            }
            return instance;
        }
    }

    public void SwitchDimension()
    { 
        if (isAlternate)
        {
            baseDim.SetActive(true);
            alternateDim.SetActive(false);

            ToggleGOList(baseDimEnemies, true);
            ToggleGOList(alternateDimEnemies, false);
        } else
        {
            baseDim.SetActive(false);
            alternateDim.SetActive(true);

            ToggleGOList(baseDimEnemies, false);
            ToggleGOList(alternateDimEnemies, true);
        }
        isAlternate = !isAlternate;
    }

    private void ToggleGOList(List<GameObject> list, bool enable)
    {
        foreach (GameObject GO in list)
        {
            GO.SetActive(enable);
        }
    }
}
