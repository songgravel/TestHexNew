using UnityEditor;
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
//using System.Data;
using System.Xml;
//using LitJson;

namespace CobraGame
{
    public class Hex2Xml
    {
        public static bool Save(string strFileName, HexGrid hexGrid)
        {
            Debug.Log("Save ok:" + strFileName);
            return true;
            /*
            Debug.Log("Excel2Xml:" + strExcelName);
            FileStream stream = File.Open(DDefine.GetDataPath() + @"/Editor/Excel2Xml/Inputs/" + strExcelName + ".xlsx", FileMode.Open, FileAccess.Read);
            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

            DataSet result = excelReader.AsDataSet();

            for (int t = 0; t < result.Tables.Count; t++)
            {
                string strTableName = result.Tables[t].TableName;
                Debug.Log("Table:" + strTableName);
                int columns = result.Tables[t].Columns.Count;
                int rows = result.Tables[t].Rows.Count;
                string[] title = new string[columns];
                for (int j = 0; j < columns; j++)
                {
                    string nvalue = result.Tables[t].Rows[2][j].ToString();
                    title[j] = nvalue;
                }

                string filepath = DDefine.GetDataPath() + @"/TowerDefense/Resources/Configs/" + strTableName + ".xml";
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
                if (root != null)
                {
                    for (int i = 3; i < rows; i++)
                    {
                        XmlElement elmNew = xmlDoc.CreateElement("DataInfo");
                        for (int j = 0; j < columns; j++)
                        {
                            string nvalue = result.Tables[t].Rows[i][j].ToString();
                            Debug.Log(nvalue);
                            elmNew.SetAttribute(title[j], nvalue);
                        }
                        root.AppendChild(elmNew);
                    }
                }
                xmlDoc.Save(filepath);
            }
            */
        }

        public static bool Load(string strFileName, HexGrid hexGrid)
        {
            Debug.Log("Load ok:" + strFileName);
            return true;
        }
    }
}

