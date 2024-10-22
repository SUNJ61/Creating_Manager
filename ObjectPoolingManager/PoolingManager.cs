using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    /*
    Ǯ���Ŵ��� �����͸� ��ųʸ��� ������ ȿ������ ������ �� �� �ֵ��� ����.
    PoolingData�� Ǯ���� ������Ʈ�� ���� ����Ʈ, Ǯ���� ������Ʈ, Ǯ���� ������Ʈ���� �θ� ������Ʈ �̸� ������, Ǯ���� ������Ʈ �̸�, Ǯ���� ������ �����Ѵ�.
    Ǯ���Ŵ����� ����� Data ��ųʸ��� ���� Ǯ���� ������Ʈ�� ����Ʈ, ������Ʈ, Ǯ���� ������Ʈ�� �θ� �̸�, Ǯ���� ������Ʈ�� �̸�, Ǯ���� ������Ʈ ������ ��ȯ ���� �� �ִ�.
    */
    public static PoolingManager instance;
    public Dictionary<int, PoolingData> Data = new Dictionary<int, PoolingData>();

    [SerializeField] List<GameObject> EX_Pool;

    GameObject EX_Prefab;

    private readonly int EX_Max = 10;

    private readonly string EX_Group = "EXGroup";
    private readonly string EX_Obj = "EX";
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        EX_Prefab = Resources.Load<GameObject>(EX_Obj);

        Data.Add(0, new PoolingData(EX_Pool, EX_Prefab, EX_Group, EX_Obj, EX_Max));

        for (int i = 0; i < Data.Count; i++)
            Pooling(i, Data);
    }

    private void Pooling(int key, Dictionary<int, PoolingData> data) //��ųʸ��� ������ �����ͷ� ������Ʈ Ǯ��
    {
        GameObject Group = new GameObject(data[key].GroupName);
        for (int i = 0; i < data[key].MaxPool; i++)
        {
            var obj = Instantiate(data[key].Prefab, Group.transform);
            obj.transform.position = new Vector3(0f, -30f, 0f);
            obj.transform.rotation = Quaternion.identity;
            obj.name = $"{data[key].ObjName}";
            data[key].Pool_List.Add(obj);
            obj.SetActive(false);
        }
    }

    public GameObject GetObject(int key) //��ųʸ� ����Ʈ�� ����� ������Ʈ�� ��Ȱ��ȭ�� ������Ʈ ��ȯ.
    {
        foreach (var obj in Data[key].Pool_List)
        {
            if (!obj.activeSelf)
                return obj;
        }
        return null;
    }
}
