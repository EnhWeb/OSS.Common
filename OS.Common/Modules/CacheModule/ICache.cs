using System;
using System.Collections.Generic;

namespace OS.Common.Modules.CacheModule
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// ��ӻ��棬�Ѵ��ڲ�����
        /// </summary>
        /// <typeparam name="T">��ӻ����������</typeparam>
        /// <param name="key">��Ӷ����key</param>
        /// <param name="obj">ֵ</param>
        /// <param name="slidingExpiration">��Թ��ڵ�TimeSpan ���ʹ�ù̶�ʱ��  =TimeSpan.Zero</param>
        /// <param name="absoluteExpiration"> ���Թ���ʱ�� </param>
        /// <param name="db"> �������db </param>
        /// <returns>�Ƿ���ӳɹ�</returns>
        bool Add<T>(string key, T obj, TimeSpan slidingExpiration, DateTime? absoluteExpiration = null,
            int db = 0);

        /// <summary>
        /// ��ӻ���,������������
        /// </summary>
        /// <typeparam name="T">��ӻ����������</typeparam>
        /// <param name="key">��Ӷ����key</param>
        /// <param name="obj">ֵ</param>
        /// <param name="slidingExpiration">��Թ��ڵ�TimeSpan ���ʹ�ù̶�ʱ��  =TimeSpan.Zero</param>
        /// <param name="absoluteExpiration"> ���Թ���ʱ�� </param>
        /// <param name="regionName"> �������db </param>
        /// <returns>�Ƿ���ӳɹ�</returns>
        bool AddOrUpdate<T>(string key, T obj, TimeSpan slidingExpiration, DateTime? absoluteExpiration = null,
            int db = 0);

        /// <summary>
        /// ��ȡ�������
        /// </summary>
        /// <typeparam name="T">��ȡ�����������</typeparam>
        /// <param name="key">key</param>
        /// <param name="db">�������db</param>
        /// <returns>��ȡָ��key��Ӧ��ֵ </returns>
        T Get<T>(string key, int db=0);

        /// <summary>
        /// ��ȡ�������
        /// </summary>
        /// <typeparam name="T">��ȡ�����������</typeparam>
        /// <param name="keys">  key�б�   </param>
        /// <param name="db">�������db</param>
        /// <returns> ��ȡ�����ͬ����key��Ӧ�Ĳ�ֵͬ </returns>
        IDictionary<string, object> Get<T>(IEnumerable<String> keys, int db=0);

        /// <summary>
        /// �Ƴ��������
        /// </summary>
        /// <param name="key"></param>
        /// <param name="db"></param>
        /// <returns>�Ƿ�ɹ�</returns>
        bool Remove(string key, int db=0);

        /// <summary>
        ///   �ж��Ƿ���ڻ������
        /// </summary>
        /// <param name="key">  keyֵ  </param>
        /// <param name="db"> �������db </param>
        /// <returns></returns>
        bool Contains(string key, int db=0);
    }
}