using System.Collections.Generic;

namespace Extensions
{
    public static class ListExtensions
    {
        public static T GetRandomElement<T>(this List<T> list)
        {
            int index = UnityEngine.Random.Range(0, list.Count);
            return list[index];
        }
        
        public static T GetRandomElement<T>(this T[] list)
        {
            int index = UnityEngine.Random.Range(0, list.Length);
            return list[index];
        }

        public static List<T> GetRandomElements<T>(this List<T> list, int count)
        {
            var result = new List<T>();
            var cloneList = new List<T>(list);
            
            for (int i = 0; i < count; i++)
            {
                var index = UnityEngine.Random.Range(0, cloneList.Count);
                result.Add(cloneList[index]);
                cloneList.RemoveAt(index);
            }
            
            return result;
        }
    }
}