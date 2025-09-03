using System.Collections;
using System.Collections.Generic;
using Common2D;
using UnityEngine;

[System.Serializable]
public class TowerItem
{
    public string id;
    public string towerName;
    public int towerLevel;
    public float towerDamage;
    public float towerRange;
    public string direction;
}

[CreateAssetMenu(fileName = "TowerDatabase", menuName = "TowerDatabase/Tower Database")]
public class TowerDatabase : ScriptableObject
{
    [SerializeField] private List<TowerItem> towers = new List<TowerItem>();
    [SerializeField] private Dictionary<string, TowerItem> towerDictionary = new Dictionary<string, TowerItem>();


    private void OnEnable()
    {
        towerDictionary.Clear();
        foreach (var item in towers)
        {
            towerDictionary[item.id] = item;
        }
    }
    public TowerItem GetTower(string id)
    {
        if (towerDictionary.TryGetValue(id, out TowerItem tower))
        {
            return tower;
        }
        return null;
    }

    [ContextMenu("Load data from CSV")]
    void ExecuteAction()
    {
        towers.Clear();
        towers = CSVManager.LoadDatasFromCSV<TowerItem>("TowerDatabase",(lines) =>
        {
            TowerItem newTower = new TowerItem();

            for (int i = 0; i < lines.Length; i++)
            {
                newTower.id = lines[0];
                newTower.towerName = lines[1];
                newTower.towerLevel = int.Parse(lines[2]);
                newTower.towerDamage = float.Parse(lines[3]);
                newTower.towerRange = float.Parse(lines[4]);
                newTower.direction = lines[5];
            }
            return newTower;
        });
    }
}
