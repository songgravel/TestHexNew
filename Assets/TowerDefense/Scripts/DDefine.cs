using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Xml;
using System.IO;
using System.Text;
using System.Linq;

namespace CobraGame
{
    public enum GENERAL_CONFIG
    {
        NONE,
        
    }

    public class DDefine
    {
        private DDefine() { }

        //////////////////////////////////////////////////////////////////
        //文字

        //////////////////////////////////////////////////////////////////
        //通用
        //正态分布概率密度函数
        public static double Normal(double x, double miu, double sigma)
        {
            double dRet = 1.0 / (sigma * Math.Sqrt(2 * Math.PI)) * Math.Exp(-1 * (x - miu) * (x - miu) / (2 * sigma * sigma));
            //Debug.Log("test normal x:" + x + " sigma:" + sigma + " ret:" + dRet);
            return dRet;
        }

        //////////////////////////////////////////////////////////////////
        //获取内部数据路径// 
        public static string GetDataPath()
        {
            return Application.dataPath;
        }

        //获取缓存数据路径// 
        public static string GetPersistentDataPath()
        {
            return Application.persistentDataPath;
        }

        //获取流动资源路径// 
        public static string GetStreamingAssetsPath()
        {
            // 安卓特殊处理用于AssetBundle.LoadFromFile同步读取
            if (Application.platform == RuntimePlatform.Android)
            {
                return Application.dataPath + "!assets";
            }
            else
                return Application.streamingAssetsPath;
        }

        //////////////////////////////////////////////////////////////////
        //资源管理
        public static UnityEngine.Object LoadRes(string strPath)
        {
            return ResourceManager.getInstance().LoadRes(strPath);
        }

        public static UnityEngine.Object LoadRes(string strPath, System.Type sType)
        {
            return ResourceManager.getInstance().LoadRes(strPath, sType);
        }

        public static void UnloadRes(UnityEngine.Object pObject)
        {
            ResourceManager.getInstance().UnloadRes(pObject);
        }

        public static void UnloadUnusedRes()
        {
            ResourceManager.getInstance().UnloadUnusedRes();
        }

    }
}
