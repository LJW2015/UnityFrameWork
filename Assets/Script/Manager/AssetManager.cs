using UnityEngine;
using System.Threading.Tasks;
using System;

/// <summary>
/// 资源管理器
/// </summary>
namespace GameFramework
{
    public static class AssetManager
    {
        /// <summary>
        /// 从Resources加载资源
        /// </summary>
        /// <param name="path">资源路径</param>
        /// <returns>资源</returns>
        public static T LoadAsset<T>(string path) where T : UnityEngine.Object
        {
            return Resources.Load<T>(path);
        }

        /// <summary>
        /// 从Resources异步加载资源
        /// </summary>
        /// <param name="path">资源路径</param>
        /// <returns>资源</returns>
        public static async Task<T> LoadAssetAsync<T>(string path) where T : UnityEngine.Object
        {
            ResourceRequest request = Resources.LoadAsync<T>(path);
            await request;
            return request.asset as T;
        }

        /// <summary>
        /// 实例化预制体（异步方法）
        /// </summary>
        /// <param name="path">预制体路径</param>
        /// <param name="parent">父级Transform</param>
        /// <param name="name">实例化名称</param>
        /// <returns>实例化后的GameObject</returns>
        public static async Task<GameObject> InstantiatePrefabAsync(string path, Transform parent = null, string name = null)
        {
            if (parent == null)
            {
                Debug.LogError("Parent transform is required");
                return null;
            }

            if (name == null)
            {
                name = path;
            }

            try
            {
                // 异步加载预制体
                GameObject prefab = await LoadAssetAsync<GameObject>(path);
                if (prefab == null)
                {
                    Debug.LogError($"Failed to load prefab from path: {path}");
                    return null;
                }

                // 实例化
                GameObject instance = GameObject.Instantiate(prefab, parent);
                instance.name = name;
                instance.transform.SetParent(parent);
                instance.transform.localPosition = Vector3.zero;
                return instance;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error instantiating prefab from path {path}: {e.Message}");
                return null;
            }
        }
    }
}
