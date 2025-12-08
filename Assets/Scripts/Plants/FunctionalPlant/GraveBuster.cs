using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraveBuster : Plant
{
    private float eatGraveDuration = 5.0f;

    protected override void Awake()
    {
        base.Awake();
        id = PlantID.GraveBuster;
        type = PlantType.Normal;
        cellTypes = new List<CellType> { };
    }

    public override void setState(PlantState state)
    {
        base.setState(state);
        if (state == PlantState.Idle) // ÖÖÏÂ¼´¿Ð·ØÄ¹
        {
            AudioManager.Instance.playClip(ResourceConfig.sound_plant_gravebusterchomp);
            transform.DOMove(cell.transform.position, eatGraveDuration)
                .SetEase(Ease.Linear)
                .OnComplete(() => EatGrave());
        }
    }

    private void EatGrave()
    {
        cell.setCellProp(CellProp.Tombstone, false);
        setState(PlantState.Die);
    }
}
