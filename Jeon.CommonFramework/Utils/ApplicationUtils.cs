using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeon.CommonFramework.Utils
{
    public class ApplicationUtils
    {
        /// <summary>
        /// 메모리를 해제합니다.
        /// </summary>
        /// <remarks>[bslee] 2014.01.29</remarks>
        public static void MemCollect()
        {
            try
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();

                WinAPI.SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
            }
            catch (Exception ex)
            {
                Logger.WriteError("[ApplicationUtils.MemCollect] Memory Flushing Fail!! {0}", ex);
            }
        }
        /// <summary>
        /// Deep Copy
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object ObjectClone(object obj)
        {
            if (obj == null) return null;

            try
            {
                using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
                {
                    System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    formatter.Serialize(stream, obj);
                    stream.Position = 0;
                    return formatter.Deserialize(stream);
                }
            }
            catch
            {
                return obj;
            }
        }
    }
}
