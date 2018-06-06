using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkWpf.TcpServer
{
    public class SessionManager
    {
        public struct Session
        {
            public string SessionId { get; set; }
            public DateTime LastAccessTime { get; set; }
        }
        /// <summary>
        /// SessionManager包含线程安全的字典，其中存储了所有的会话和会话数据。
        /// 使用多个客户端时，字典可以在多个线程中同时访问。所以使用名称空间System.Collections.Concurrent中线程安全的字典。
        /// </summary>
        private readonly ConcurrentDictionary<string, Session>
            _sessions = new ConcurrentDictionary<string, Session>();

        /// <summary>
        /// SessionManager包含线程安全的字典，其中存储了所有的会话和会话数据。
        /// 使用多个客户端时，字典可以在多个线程中同时访问。所以使用名称空间System.Collections.Concurrent中线程安全的字典。
        /// </summary>
        private readonly ConcurrentDictionary<string, Dictionary<string, string>>
            _sessionData = new ConcurrentDictionary<string, Dictionary<string, string>>();

        /// <summary>
        /// Createsession方法创建一个新的会话，并将其添加到sessions字典中：
        /// </summary>
        /// <returns></returns>
        public string CreateSession()
        {
            string sessionId = Guid.NewGuid().ToString();
            if (_sessions.TryAdd(sessionId,
                new Session()
                {
                    SessionId = sessionId,
                    LastAccessTime = DateTime.UtcNow
                }))
            {
                return sessionId;
            }
            else
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// TouchSession方法更新会话的LastAccessTime，如果会话不再有效，就返回false：
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public bool TouchSession(string sessionId)
        {
            Session oldHeader;
            if (!_sessions.TryGetValue(sessionId, out oldHeader))
            {
                return false;
            }

            Session updateHeader = oldHeader;
            updateHeader.LastAccessTime = DateTime.UtcNow;
            _sessions.TryUpdate(sessionId, updateHeader, oldHeader);
            return false;
        }
        /// <summary>
        /// 为了设置会话数据，需要解析请求。会话数据接收的动作包含由等号分隔的键和值，如x=42。
        /// ParseSessionData方法解析它，进而调用SetSessionData方法：
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="requestAction"></param>
        /// <returns></returns>
        public string ParseSessionData(string sessionId, string requestAction)
        {
            string[] sessionData = requestAction.Split('=');
            if (sessionData.Length != 2)
                return CustomProtocol.STATUSUNKNOWN;
            string key = sessionData[0];
            string value = sessionData[1];
            SetSessionData(sessionId, key, value);
            return $"{key}={value}";
        }
        /// <summary>
        /// SetsessionData添加或更新字典中的会话状态。
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetSessionData(string sessionId, string key, string value)
        {
            Dictionary<string, string> data;
            if (_sessionData.TryGetValue(sessionId, out data))
            {
                data = new Dictionary<string, string>();
                data.Add(key, value);
                _sessionData.TryAdd(sessionId, data);
            }
            else
            {
                string val;
                if (data.TryGetValue(key, out val))
                {
                    data.Remove(key);
                }
                data.Add(key, value);
            }
        }

        /// <summary>
        /// GetSessionData检索值，或返回NOTFOUND
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetSessionData(string sessionId, string key)
        {
            Dictionary<string, string> data;
            if (_sessionData.TryGetValue(sessionId, out data))
            {
                string value;
                if (data.TryGetValue(key,out value))
                {
                    return value;
                }
            }
            return CustomProtocol.STATUSNOTFOUND;
        }

        /// <summary>
        /// 从计时器线程中，CleanupAllSessions方法每分钟调用一次，删除最近没有使用的所有会话。
        /// 该方法又调用Cleanupsession，删除单个会话。
        /// 客户端发送BYE信息时也调用Cleanupsession
        /// </summary>
        public void CleanupAllSessions()
        {
            foreach (var item in _sessions)
            {
                if (item.Value.LastAccessTime + CustomProtocol.SessionTimeout >= DateTime.UtcNow)
                {
                    CleanupSession(item.Key);
                }
            }
        }

        private void CleanupSession(string sessionId)
        {
            Dictionary<string, string> removed;
            if (_sessionData.TryRemove(sessionId, out removed))
            {
                //已经删除data
            }
            Session header;
            if (_sessions.TryRemove(sessionId, out header))
            {
                //已经删除session
            }
        }
    }
}
