using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public enum SunSpawnState
{
    Disable,
    Enable
}

public class SunManager : MonoBehaviour
{
    public static SunManager Instance { get; private set; }

    public SunSpawnState state;
    private int sun = 50;
    private bool isDropSun;
    private float dropSunTime;
    private float dropSunTimer;
    private List<Transform> dropSunPositions;

    private List<Sun> sunList = new List<Sun>();
    public int Sun { get { return sun; } }

    public Sun sunPrefab;
    public TextMeshProUGUI sunText;
    public GameObject collectedTarget;
    

    void Awake()
    {
        Instance = this;
        isDropSun = false;
        dropSunTime = 8.0f;
        dropSunTimer = 2.0f;
        state = SunSpawnState.Disable;
    }

    private void Start()
    {
        dropSunPositions = MapManager.Instance.currMap.dropSunPositions;
    }

    void FixedUpdate()
    {
        sunText.text = sun.ToString();
        if (GameManager.Instance.state == GameState.Paused || GameManager.Instance.state == GameState.Losing) return;

        if (state == SunSpawnState.Disable) return;
        dropSunTimer += Time.fixedDeltaTime;
        if (dropSunTimer > dropSunTime)
        {
            dropSunTimer = 0.0f;
            dropSun();
        }
    }

    // ÔÝÍ£¼ÌÐø
    public void Pause()
    {
        foreach (Sun sun in sunList) if (sun) sun.Pause();
    }

    public void Continue()
    {
        foreach (Sun sun in sunList) if (sun) sun.Continue();
    }

    public void addSun(Sun sun)
    {
        sunList.Add(sun);
    }

    public void removeSun(Sun sun)
    {
        sunList.Remove(sun);
    }

    public void setIsDropSun(bool flag)
    {
        this.isDropSun = flag;
    }

    public void setSun(int sun)
    {
        this.sun = sun;
    }

    public void setState(SunSpawnState state)
    {
        if (state == SunSpawnState.Enable && !isDropSun) return;
        this.state = state;
    }

    public void AddSun(int num)
    {
        sun += num;
    }

    public Vector2 getSunCollectedPosition()
    {
        return collectedTarget.transform.position;
    }

    public void dropSun()
    {
        Vector2 begin_position = dropSunPositions[0].position;
        Vector2 end_min_position = dropSunPositions[1].position;
        Vector2 end_max_position = dropSunPositions[2].position;
        float begin_x = Random.Range(end_min_position.x, end_max_position.x), begin_y = begin_position.y;
        float end_y = Random.Range(end_min_position.y, end_max_position.y);
        Sun sun = GameObject.Instantiate(sunPrefab, new Vector3(begin_x, begin_y, 0), Quaternion.identity);
        sun.setState(SunState.Vertical);
        sun.setTargetY(end_y);
    }
}
