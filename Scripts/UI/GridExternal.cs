using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public static class GridExternal
{
    public static void GridLoadChild<TComp, TData>(this GridLayoutGroup grid, IEnumerable<TData> dataList, System.Func<TData, string> prefabPath, System.Action<TComp, TData, int> loadData, System.Predicate<TData> dataMatch) where TComp : MonoBehaviour
    {
        var gridTransform = grid.transform;
        int childCount = gridTransform.childCount;
        int gridIndex = 0;
        foreach(var dataItem in dataList)
        {
            if (dataMatch == null || dataMatch(dataItem))
            {
                TComp uiItem;
                if (gridIndex < childCount)
                {
                    uiItem = gridTransform.GetChild(gridIndex).GetComponent<TComp>();
                }
                else
                {
                    uiItem = DynamicPanelController.Create(prefabPath(dataItem), gridTransform).GetComponent<TComp>();
                }
                loadData(uiItem, dataItem, gridIndex + 1);
                gridIndex++;
                uiItem.gameObject.SetActive(true);
            }
        }
        while (gridIndex < childCount) {
            gridTransform.GetChild(gridIndex).gameObject.SetActive(false);
            gridIndex++;
        }
        grid.CalculateLayoutInputVertical();
    }
}
