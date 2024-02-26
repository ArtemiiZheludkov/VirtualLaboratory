using UnityEngine;
using TMPro;

public class Table : MonoBehaviour
{
    [SerializeField] private GameObject _rowPrefab;
    [SerializeField] private Transform _tableContent;

    public void Init()
    {
        foreach (Transform child in _tableContent)
            Destroy(child.gameObject);
    }
    
    public void AddRow(float T, float R)
    {
        GameObject row = Instantiate(_rowPrefab, _tableContent);
        row.transform.GetChild(0).GetComponent<TMP_Text>().text = T.ToString("F1");
        row.transform.GetChild(1).GetComponent<TMP_Text>().text = R.ToString("F2");
    }
}