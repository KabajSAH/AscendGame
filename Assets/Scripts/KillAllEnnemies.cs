using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KillAllEnemies : MonoBehaviour
{
    [SerializeField] private GameObject gameObjectEnemies;

    private List<Enemy> _listEnemies;
    // Start is called before the first frame update
    public void Start()
    {
        _listEnemies = new List<Enemy>(gameObjectEnemies.GetComponentsInChildren<Enemy>());
    }

    // Update is called once per frame
    private void Update()
    {
        _listEnemies.RemoveAll(element => element == null);
        if (!_listEnemies.Any())
        {
            gameObject.SetActive(false);
        }
    }
}
