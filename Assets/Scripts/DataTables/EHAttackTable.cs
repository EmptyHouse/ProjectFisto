using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EHAttackDataRow : EHTableRow
{
    public AnimationClip AnimClip;
    public FAttackData AttackData;
}

public class EHAttackTable : EHDataTable<EHAttackDataRow>
{
    private void OnValidate()
    {
        foreach (EHAttackDataRow Data in DataTableRows)
        {
            if (Data.AnimClip != null)
            {
                Data.RowId = Data.AnimClip.name;
            }
        }
    }

    public EHAttackDataRow FindRowByAnimationClip(AnimationClip AnimClip)
    {
        return FindRow(AnimClip.name);
    }
}
