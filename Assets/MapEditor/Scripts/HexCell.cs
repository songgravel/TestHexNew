using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CobraGame
{
    public class HexCell : MonoBehaviour
    {
        public int index;
        public HexCoordinates coordinates;
        Color color, originColor;
        int mapData = 0;
        int mapPlat = 0;
        float elevation = float.MinValue;
        bool selected;
        public Text label;
        public HexGridChunk chunk;

        [SerializeField]
        HexCell[] neighbors = new HexCell[6];

        public HexCell GetNeighbor(HexDirection direction)
        {
            return neighbors[(int)direction];
        }

        public void SetNeighbor(HexDirection direction, HexCell cell)
        {
            neighbors[(int)direction] = cell;
            cell.neighbors[(int)direction.Opposite()] = this;
        }

        public Color Color
        {
            get
            {
                return color;
            }
            set
            {
                if (color == value)
                {
                    return;
                }
                color = value;
                Refresh();
            }
        }

        public int MapData
        {
            get
            {
                return mapData;
            }
            set
            {
                if (mapData == value)
                {
                    return;
                }
                mapData = value;

                Color newColor = Color.black;
                if (mapData == 0)
                {
                    newColor = Color.black;
                }
                else
                {
                    if ((mapData & (int)HexMapData.ENTRY) > 0)
                    {
                        newColor.r += 1f;
                    }
                    if ((mapData & (int)HexMapData.BASE) > 0)
                    {
                        newColor.b += 1f;
                    }
                    if ((mapData & (int)HexMapData.JOINT) > 0)
                    {
                        newColor.g += 0.5f;
                    }
                    if ((mapData & (int)HexMapData.BUILD) > 0)
                    {
                        newColor.b += 0.5f;
                    }
                    if ((mapData & (int)HexMapData.PATH) > 0)
                    {
                        newColor.r += 0.5f;
                    }
                }
                if (Selected)
                {
                    originColor = newColor;
                    Color = newColor;
                }
                else
                {
                    Color = newColor;
                }
            }
        }

        public int MapPlat
        {
            get
            {
                return mapPlat;
            }
            set
            {
                if (mapPlat == value)
                {
                    return;
                }
                mapPlat = value;
            }
        }

        public float Elevation
        {
            get
            {
                return elevation;
            }
            set
            {
                if (elevation == value)
                {
                    return;
                }

                elevation = value;
                Vector3 position = transform.localPosition;
                position.y = value * HexMetrics.elevationStep;
                transform.localPosition = position;

                Vector3 uiPosition = label.rectTransform.localPosition;
                uiPosition.z = elevation * -HexMetrics.elevationStep - 0.15f;
                label.rectTransform.localPosition = uiPosition;

                Refresh();
            }
        }

        public bool Selected
        {
            get
            {
                return selected;
            }
            set
            {
                if (selected != value)
                {
                    if (value)
                    {
                        originColor = Color;
                        Color = Color.green;
                        label.color = Color.green;
                    }
                    else
                    {
                        Color = originColor;
                        label.color = Color.black;
                    }
                }
                selected = value;

            }
        }

        void Refresh()
        {
            if (chunk)
            {
                chunk.Refresh();
                for (int i = 0; i < neighbors.Length; i++)
                {
                    HexCell neighbor = neighbors[i];
                    if (neighbor != null && neighbor.chunk != chunk)
                    {
                        neighbor.chunk.Refresh();
                    }
                }
            }
        }

        public string MapDataToString()
        {
            string text = MapPlat > 0 ? MapPlat.ToString() : "";
            text += "\n";
            if ((MapData & (int)HexMapData.ENTRY) > 0)
            {
                text += "起 ";
            }
            if ((MapData & (int)HexMapData.BASE) > 0)
            {
                text += "终 ";
            }
            if ((MapData & (int)HexMapData.JOINT) > 0)
            {
                text += "口 ";
            }
            if ((MapData & (int)HexMapData.BUILD) > 0)
            {
                text += "建 ";
            }
            if ((MapData & (int)HexMapData.PATH) > 0)
            {
                text += "路 ";
            }
            return text;
        }
    }
}
