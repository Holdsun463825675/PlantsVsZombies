using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ClickPriority : MonoBehaviour
{
    [Header("点击优先级")]
    [Tooltip("数值越大，优先级越高")]
    public int priority = 1;

    [Header("点击设置")]
    public bool isClickable = true;
    public bool showDebug = false;

    void Start()
    {
        if (showDebug)
        {
            Debug.Log($"{gameObject.name} 点击优先级: {priority}");
        }
    }
}

// 可点击接口 - 简化版本，无需冷却检查
public interface IClickable
{
    void OnClick(); // 可无限重复点击
}

public class ClickManager : MonoBehaviour
{
    public static ClickManager Instance;

    [Header("点击设置")]
    public LayerMask clickableLayers = -1;
    public bool enableDebug = false;

    public enum ClickResolutionMode
    {
        Priority,       // 按优先级
        RenderOrder,    // 按渲染顺序
        ZDepth,         // 按Z深度
        Composite       // 综合判断
    }

    public ClickResolutionMode resolutionMode = ClickResolutionMode.Composite;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ResolveAndProcessClick();
        }
    }

    public void ResolveAndProcessClick()
    {
        GameObject target = GetBestClickTarget();
        if (target != null)
        {
            ProcessClick(target);
        }
    }

    public GameObject GetBestClickTarget()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos, Vector2.zero, 0f, clickableLayers);

        if (hits.Length == 0) return null;

        switch (resolutionMode)
        {
            case ClickResolutionMode.Priority:
                return ResolveByPriority(hits);
            case ClickResolutionMode.RenderOrder:
                return ResolveByRenderOrder(hits);
            case ClickResolutionMode.ZDepth:
                return ResolveByZDepth(hits);
            case ClickResolutionMode.Composite:
                return ResolveByComposite(hits);
            default:
                return hits[0].collider.gameObject;
        }
    }

    GameObject ResolveByPriority(RaycastHit2D[] hits)
    {
        return hits.OrderByDescending(h =>
            h.collider.GetComponent<ClickPriority>()?.priority ?? 0
        ).First().collider.gameObject;
    }

    GameObject ResolveByRenderOrder(RaycastHit2D[] hits)
    {
        return hits.OrderByDescending(h =>
            h.collider.GetComponent<SpriteRenderer>()?.sortingOrder ?? 0
        ).First().collider.gameObject;
    }

    GameObject ResolveByZDepth(RaycastHit2D[] hits)
    {
        return hits.OrderBy(h => h.transform.position.z).First().collider.gameObject;
    }

    GameObject ResolveByComposite(RaycastHit2D[] hits)
    {
        // 综合评分系统
        var scoredHits = hits.Select(h => new {
            hit = h,
            score = CalculateClickScore(h.collider.gameObject)
        }).OrderByDescending(x => x.score);

        if (enableDebug)
        {
            foreach (var scored in scoredHits)
            {
                Debug.Log($"{scored.hit.collider.gameObject.name}: {scored.score}");
            }
        }

        return scoredHits.First().hit.collider.gameObject;
    }

    float CalculateClickScore(GameObject obj)
    {
        float score = 0f;

        // 优先级贡献（权重最高）
        ClickPriority priority = obj.GetComponent<ClickPriority>();
        if (priority != null)
        {
            score += priority.priority * 100f;
        }

        // 渲染顺序贡献
        SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            score += renderer.sortingOrder * 10f;
        }

        // Z深度贡献（Z值越小分数越高）
        score += (1f - Mathf.Clamp01(obj.transform.position.z)) * 5f;

        return score;
    }

    void ProcessClick(GameObject target)
    {
        if (enableDebug)
        {
            Debug.Log($"点击处理: {target.name}");
        }

        IClickable clickable = target.GetComponent<IClickable>();
        if (clickable != null)
        {
            clickable.OnClick(); // 无限重复点击
        }
    }

    [ContextMenu("测试点击")]
    public void TestClick()
    {
        ResolveAndProcessClick();
    }
}