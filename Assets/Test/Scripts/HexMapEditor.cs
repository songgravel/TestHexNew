using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CobraGame
{
    public class HexMapEditor : MonoBehaviour
    {
        public Color[] colors;

        public HexGrid hexGrid;

        public Text textHeight;

        private Color activeColor;
        float activeElevation;
        int brushSize;

        bool applyColor;
        bool applyElevation = false;
        bool applySelect = true;

        Dictionary<int, HexCell> selectedList = new Dictionary<int, HexCell>();

        void Awake()
        {
            SelectColor(0);
        }

        // Update is called once per frame
        void Update()
        {
            if (
                Input.GetMouseButton(0) &&
                !EventSystem.current.IsPointerOverGameObject()
            )
            {
                HandleInput();
            }
        }

        void HandleInput()
        {
            Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(inputRay, out hit))
            {
                //EditCells(hexGrid.GetCell(hit.point));
                SelectCells(hexGrid.GetCell(hit.point));
            }
        }

        void EditCells(HexCell center)
        {
            int centerX = center.coordinates.X;
            int centerZ = center.coordinates.Z;

            for (int r = 0, z = centerZ - brushSize; z <= centerZ; z++, r++)
            {
                for (int x = centerX - r; x <= centerX + brushSize; x++)
                {
                    EditCell(hexGrid.GetCell(new HexCoordinates(x, z)));
                }
            }
            for (int r = 0, z = centerZ + brushSize; z > centerZ; z--, r++)
            {
                for (int x = centerX - brushSize; x <= centerX + r; x++)
                {
                    EditCell(hexGrid.GetCell(new HexCoordinates(x, z)));
                }
            }
        }

        void EditCell(HexCell cell)
        {
            if (cell)
            {
                if (applyColor)
                {
                    cell.Color = activeColor;
                }
                if (applyElevation)
                {
                    cell.Elevation = activeElevation;
                }
            }
        }

        void SelectCells(HexCell center)
        {
            int centerX = center.coordinates.X;
            int centerZ = center.coordinates.Z;

            for (int r = 0, z = centerZ - brushSize; z <= centerZ; z++, r++)
            {
                for (int x = centerX - r; x <= centerX + brushSize; x++)
                {
                    SelectCell(hexGrid.GetCell(new HexCoordinates(x, z)));
                }
            }
            for (int r = 0, z = centerZ + brushSize; z > centerZ; z--, r++)
            {
                for (int x = centerX - brushSize; x <= centerX + r; x++)
                {
                    SelectCell(hexGrid.GetCell(new HexCoordinates(x, z)));
                }
            }
        }

        void SelectCell(HexCell cell)
        {
            if (cell)
            {
                if (applySelect)
                {
                    cell.Selected = true;
                    if (!selectedList.ContainsKey(cell.index))
                    {
                        selectedList.Add(cell.index, cell);
                    }
                }
                else
                {
                    cell.Selected = false;
                    if (selectedList.ContainsKey(cell.index))
                    {
                        selectedList.Remove(cell.index);
                    }
                }
            }
        }

        public void ClearSelected()
        {
            foreach(KeyValuePair<int, HexCell> kvp in selectedList)
            {
                kvp.Value.Selected = false;
            }
            selectedList.Clear();
        }

        public void SelectColor(int index)
        {
            applyColor = index >= 0;
            if (applyColor)
            {
                activeColor = colors[index];
            }
        }

        public void SetApplyElevation(bool toggle)
        {
            applyElevation = toggle;
        }

        public void SetElevation(float elevation)
        {
            activeElevation = elevation;
            if (textHeight)
            {
                textHeight.text = elevation.ToString();
            }
            if (applyElevation)
            {
                foreach (KeyValuePair<int, HexCell> kvp in selectedList)
                {
                    kvp.Value.Elevation = activeElevation;
                }
            }
        }

        public void ShowUI(bool visible)
        {
            hexGrid.ShowUI(visible);
        }

        public void SetBrushSize(float size)
        {
            brushSize = (int)size;
        }

        public void SetApplySelect(bool toggle)
        {
            applySelect = toggle;
        }
    }
}
