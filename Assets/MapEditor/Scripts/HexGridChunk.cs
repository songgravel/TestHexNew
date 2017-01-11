using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CobraGame
{
    [ExecuteInEditMode]
    public class HexGridChunk : MonoBehaviour
    {

        HexCell[] cells;

        HexMesh hexMesh;
        Canvas gridCanvas;

        void Awake()
        {
            gridCanvas = GetComponentInChildren<Canvas>();
            hexMesh = GetComponentInChildren<HexMesh>();

            cells = new HexCell[HexMetrics.chunkSizeX * HexMetrics.chunkSizeZ];

            ShowUI(true);
            ShowPlat(false);
        }

        public void AddCell(int index, HexCell cell)
        {
            cells[index] = cell;
            cell.chunk = this;
            cell.transform.SetParent(transform, false);
            cell.label.rectTransform.SetParent(gridCanvas.transform, false);
        }

        public void Refresh()
        {
            enabled = true;
        }

        void LateUpdate()
        {
            hexMesh.Triangulate(cells);
            enabled = false;
        }

        public void ShowUI(bool visible)
        {
            gridCanvas.gameObject.SetActive(visible);
        }

        public void ShowPlat(bool visible)
        {
            foreach(HexCell cell in cells)
            {
                if (cell && cell.label)
                {
                    if (visible)
                    {
                        cell.label.text = cell.MapDataToString();
                    }
                    else
                    {
                        cell.label.text = cell.coordinates.ToStringOnSeparateLines();
                    }
                }
            }
        }
    }
}
