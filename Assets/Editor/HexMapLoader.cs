using UnityEditor;
using UnityEngine;
using System.Xml;
using CobraGame;

public class HexMapLoader
{
    [MenuItem("Custom/Load Hex Map")]
    static void LoadHexMap()
    {
        string filePath = EditorUtility.OpenFilePanel("编辑模式加载地图", DDefine.GetDataPath() + @"/TowerDefense/Resources/Configs/Maps/", "xml");
        int nBegin = filePath.LastIndexOf(@"/");
        string fileName = filePath.Substring(nBegin + 1);
        Load(fileName);
    }

    public static bool Load(string strFileName)
    {
        if (strFileName.IndexOf(".xml") >= 0)
        {
            strFileName = strFileName.Substring(0, strFileName.Length - 4);//Resources加载不能带后缀名
        }

        GameObject grid = GameObject.Find("Hex Grid");
        if (grid == null)
        {
            return false;
        }
        HexGrid hexGrid = grid.GetComponent<HexGrid>();

        //string filepath = DDefine.GetDataPath() + @"/TowerDefense/Resources/Configs/Maps/" + strFileName;
        string filepath = string.Format(@"Configs/Maps/{0}", strFileName);
        Debug.Log("Load:" + filepath);
        XmlDocument xdoc = new XmlDocument();
        TextAsset textAsset = DDefine.LoadRes(filepath) as TextAsset;
        if (textAsset)
        {
            xdoc.LoadXml(textAsset.text); //从表格的存放路径来获取

            XmlNodeList list = xdoc.SelectNodes(@"DataList/MapInfo");
            foreach (XmlNode node in list)
            {
                HexMetrics.outerRadius = float.Parse(node.Attributes["radius"].Value);
                HexMetrics.chunkSizeX = int.Parse(node.Attributes["chunk_size_x"].Value);
                HexMetrics.chunkSizeZ = int.Parse(node.Attributes["chunk_size_z"].Value);
                HexMetrics.UpdateRadius();
                hexGrid.chunkCountX = int.Parse(node.Attributes["chunk_count_x"].Value);
                hexGrid.chunkCountZ = int.Parse(node.Attributes["chunk_count_z"].Value);
                float gridX = 0;
                if (node.Attributes["grid_x"] != null)
                {
                    gridX = float.Parse(node.Attributes["grid_x"].Value);
                }
                float gridZ = 0;
                if (node.Attributes["grid_z"] != null)
                {
                    gridZ = float.Parse(node.Attributes["grid_z"].Value);
                }
                hexGrid.transform.localPosition = new Vector3(gridX, 0, gridZ);
                hexGrid.outerRadius = HexMetrics.outerRadius;
                hexGrid.chunkSizeX = HexMetrics.chunkSizeX;
                hexGrid.chunkSizeZ = HexMetrics.chunkSizeZ;
            }

            HexGrid.s_dicCellList.Clear();
            list = xdoc.SelectNodes(@"DataList/CellInfo");
            foreach (XmlNode node in list)
            {
                XmlCellInfo cell = new XmlCellInfo();
                cell.index = int.Parse(node.Attributes["index"].Value);
                cell.mapData = int.Parse(node.Attributes["map_data"].Value);
                cell.mapPlat = int.Parse(node.Attributes["map_plat"].Value);
                cell.elevation = float.Parse(node.Attributes["elevation"].Value);

                HexGrid.s_dicCellList.Add(cell.index, cell);
            }

            Debug.Log("loadMap OK!");
        }
        else
        {
            HexGrid.s_dicCellList.Clear();
            hexGrid.transform.localPosition = Vector3.zero;
            HexMetrics.outerRadius = hexGrid.outerRadius;
            HexMetrics.chunkSizeX = hexGrid.chunkSizeX;
            HexMetrics.chunkSizeZ = hexGrid.chunkSizeZ;
            HexMetrics.UpdateRadius();

            Debug.Log("createMap OK!");
        }

        //删除之前创建的子对象
        for (int i = grid.transform.childCount - 1; i >= 0; i--)
        {
            GameObject go = grid.transform.GetChild(i).gameObject;
            Object.DestroyImmediate(go);
        }
        hexGrid.LoadData();

        return true;
    }
}
