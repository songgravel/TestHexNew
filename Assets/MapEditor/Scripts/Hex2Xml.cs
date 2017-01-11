using UnityEngine;
using System.IO;
using System.Xml;

namespace CobraGame
{
    public class Hex2Xml
    {
        public static bool Save(string strFileName, HexGrid hexGrid)
        {
            if (strFileName.IndexOf(".xml") < 0)
            {
                strFileName += ".xml";
            }

            string filepath = DDefine.GetDataPath() + @"/TowerDefense/Resources/Configs/Maps/" + strFileName;
            Debug.Log("Save:" + filepath);
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode root = null;
            if (File.Exists(filepath))
            {
                xmlDoc.Load(filepath);
                root = xmlDoc.SelectSingleNode("DataList");
                
                root.RemoveAll();

                Debug.Log("updateXml OK!");
            }
            else
            {
                root = xmlDoc.CreateElement("DataList");
                
                xmlDoc.AppendChild(root);
                Debug.Log("createXml OK!");
            }
            //地图单元格信息
            if (root != null)
            {
                XmlElement elmNew = xmlDoc.CreateElement("MapInfo");
                elmNew.SetAttribute("radius", HexMetrics.outerRadius.ToString());
                elmNew.SetAttribute("chunk_size_x", HexMetrics.chunkSizeX.ToString());
                elmNew.SetAttribute("chunk_size_z", HexMetrics.chunkSizeZ.ToString());
                elmNew.SetAttribute("chunk_count_x", hexGrid.chunkCountX.ToString());
                elmNew.SetAttribute("chunk_count_z", hexGrid.chunkCountZ.ToString());
                elmNew.SetAttribute("grid_x", hexGrid.transform.localPosition.x.ToString());
                elmNew.SetAttribute("grid_z", hexGrid.transform.localPosition.z.ToString());
                root.AppendChild(elmNew);

                foreach (HexCell cell in hexGrid.GetCells())
                {
                    if (cell && 
                        (cell.MapData > 0 || 
                        cell.MapPlat > 0 || 
                        cell.Elevation > 0)
                        )
                    {
                        elmNew = xmlDoc.CreateElement("CellInfo");
                        elmNew.SetAttribute("index", cell.index.ToString());
                        elmNew.SetAttribute("map_data", cell.MapData.ToString());
                        elmNew.SetAttribute("map_plat", cell.MapPlat.ToString());
                        elmNew.SetAttribute("elevation", cell.Elevation.ToString());
                        root.AppendChild(elmNew);
                    }
                }
            }

            xmlDoc.Save(filepath);

            return true;
        }

        public static bool Load(string strFileName, HexGrid hexGrid)
        {
            if (strFileName.IndexOf(".xml") >= 0)
            {
                strFileName = strFileName.Substring(0, strFileName.Length - 4);//Resources加载不能带后缀名
            }

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
            GameObject grid = hexGrid.gameObject;
            for (int i = grid.transform.childCount - 1; i >= 0; i--)
            {
                GameObject go = grid.transform.GetChild(i).gameObject;
                Object.Destroy(go);
            }
            hexGrid.LoadData();

            return true;
        }
    }
}

