using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CobraGame
{
    public class HexCell : MonoBehaviour
    {
        public int index;
        public HexCoordinates coordinates;
        Color color, originColor;
        float elevation = float.MinValue;
        bool selected;
        public RectTransform uiRect;
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

                Vector3 uiPosition = uiRect.localPosition;
                uiPosition.z = elevation * -HexMetrics.elevationStep - 0.15f;
                uiRect.localPosition = uiPosition;

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
                    if (selected)
                    {
                        Color = originColor;
                    }
                    else
                    {
                        originColor = Color;
                        Color = Color.green;
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
    }
}
