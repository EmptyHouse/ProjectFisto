using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EHTableRow
{
    public string RowId;
}

public class EHDataTable<T> : ScriptableObject where T : EHTableRow
{
    [SerializeField]
    protected T[] DataTableRows;

    private readonly Dictionary<string, T> TableRowsMap = new Dictionary<string, T>();

    public virtual void InitializeDataTable()
    {
        TableRowsMap.Clear();
        foreach (T Row in DataTableRows)
        {
            TableRowsMap.Add(Row.RowId, Row);
        }
    }

    public T FindRow(string RowId, bool ShouldLogMissing = true)
    {
        if (TableRowsMap.ContainsKey(RowId))
        {
            return TableRowsMap[RowId];
        }

        if (ShouldLogMissing)
        {
            Debug.LogWarning("Row Was Not Found In DataTable: " + RowId);
        }
        return null;
    }
}
