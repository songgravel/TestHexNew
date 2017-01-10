using UnityEngine;
using System.Collections;

namespace CobraGame
{
    public class ResourceManager
    {
        private ResourceManager() { }
        private static readonly ResourceManager instance = new ResourceManager();
        public static ResourceManager getInstance()
        {
            return instance;
        }

        public Object LoadRes(string strPath)
        {
            return Resources.Load(strPath);
        }

        public Object LoadRes(string strPath, System.Type sType)
        {
            return Resources.Load(strPath, sType);
        }

        public void UnloadRes(Object pObject)
        {
            Resources.UnloadAsset(pObject);
        }

        public void UnloadUnusedRes()
        {
            Resources.UnloadUnusedAssets();
        }
    }
}