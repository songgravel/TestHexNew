using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CobraGame
{
    public class HexCell : MonoBehaviour
    {
        public HexCoordinates coordinates;
        public Color color;
        public bool selected;
        public int data;
        float elevation;
        public RectTransform uiRect;

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

        public float Elevation
        {
            get
            {
                return elevation;
            }
            set
            {
                elevation = value;
                Vector3 position = transform.localPosition;
                position.y = value * HexMetrics.elevationStep;
                transform.localPosition = position;

                Vector3 uiPosition = uiRect.localPosition;
                uiPosition.z = elevation * -HexMetrics.elevationStep - 0.15f;
                uiRect.localPosition = uiPosition;
            }
        }
    }
}
