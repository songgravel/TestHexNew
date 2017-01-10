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

        public Slider elevationSlider;
        public InputField elevationInput;

        int activeData;
        int activePlat;
        float activeElevation;
        int brushSize;

        bool applyData = false;
        bool applyPlat = false;
        bool applyElevation = false;
        bool applySelect = true;
        bool showPlat = false;
        bool dataEntry = false;
        bool dataBase = false;
        bool dataJoint = false;
        bool dataBuild = false;
        bool dataPath = false;

        Dictionary<int, HexCell> selectedList = new Dictionary<int, HexCell>();

        void Awake()
        {
            //SelectColor(0);
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

        /*
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
        */

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

        public void SetBrushSize(float size)
        {
            brushSize = (int)size;
        }

        public void SetApplySelect(bool toggle)
        {
            applySelect = toggle;
        }

        public void ClearSelected()
        {
            foreach(KeyValuePair<int, HexCell> kvp in selectedList)
            {
                kvp.Value.Selected = false;
            }
            selectedList.Clear();
        }

        /*
        public void SelectColor(int index)
        {
            applyColor = index >= 0;
            if (applyColor)
            {
                activeColor = colors[index];
            }
        }
        */

        public void SetApplyElevation(bool toggle)
        {
            applyElevation = toggle;
        }

        public void SetElevation(float elevation)
        {
            activeElevation = elevation;
            if (elevationInput)
            {
                elevationInput.text = activeElevation.ToString();
            }
            if (applyElevation)
            {
                foreach (KeyValuePair<int, HexCell> kvp in selectedList)
                {
                    kvp.Value.Elevation = activeElevation;
                }
            }
        }

        public void SetElevation(string elevation)
        {
            activeElevation = float.Parse(elevation);
            if (elevationSlider)
            {
                elevationSlider.value = activeElevation;
            }
            if (applyElevation)
            {
                foreach (KeyValuePair<int, HexCell> kvp in selectedList)
                {
                    kvp.Value.Elevation = activeElevation;
                }
            }
        }
        
        public void SetApplyData(bool toggle)
        {
            applyData = toggle;
        }

        public void SetMapData()
        {
            activeData = 0;
            if (dataEntry)
            {
                activeData |= (int)HexMapData.ENTRY;
            }
            if (dataBase)
            {
                activeData |= (int)HexMapData.BASE;
            }
            if (dataJoint)
            {
                activeData |= (int)HexMapData.JOINT;
            }
            if (dataBuild)
            {
                activeData |= (int)HexMapData.BUILD;
            }
            if (dataPath)
            {
                activeData |= (int)HexMapData.PATH;
            }

            if (applyData)
            {
                foreach (KeyValuePair<int, HexCell> kvp in selectedList)
                {
                    kvp.Value.MapData = activeData;
                }
            }
        }

        public void SetDataEntry(bool toggle)
        {
            //入口单元
            dataEntry = toggle;
            SetMapData();
        }

        public void SetDataBase(bool toggle)
        {
            //主基地单元
            dataBase = toggle;
            SetMapData();
        }

        public void SetDataJoint(bool toggle)
        {
            //平台连接单元
            dataJoint = toggle;
            SetMapData();
        }

        public void SetDataBuild(bool toggle)
        {
            //可建筑单元
            dataBuild = toggle;
            SetMapData();
        }

        public void SetDataPath(bool toggle)
        {
            //可行走单元
            dataPath = toggle;
            SetMapData();
        }

        public void SetApplyPlat(bool toggle)
        {
            applyPlat = toggle;
        }

        public void SetMapPlat(string plat)
        {
            activePlat = int.Parse(plat);
            if (applyPlat)
            {
                foreach (KeyValuePair<int, HexCell> kvp in selectedList)
                {
                    kvp.Value.MapPlat = activePlat;
                    if (showPlat)
                    {
                        kvp.Value.label.text = activePlat.ToString();
                    }
                }
            }
        }

        public void ShowUI(bool visible)
        {
            hexGrid.ShowUI(visible);
        }

        public void ShowPlat(bool visible)
        {
            hexGrid.ShowPlat(visible);
            showPlat = visible;
        }
    }
}
