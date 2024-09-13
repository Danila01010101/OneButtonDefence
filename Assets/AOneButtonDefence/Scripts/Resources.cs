using UnityEngine;

public class Resources : MonoBehaviour
{
	public static Resources Instance;

    public ResourcesData Data { get; private set; }

    // ������ ���� ���� ������ � �����, �� ������ ����
    // ��� ��������� �� ������� ������� ����� Resources.Instance.Data += x; ��� ��������� ����� �������������

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(Instance);
        }

        Instance = this;
    }


    public class ResourcesData
    {
        public int FoodAmount { get; set; }

        //��������� ������ ����������
    }
}