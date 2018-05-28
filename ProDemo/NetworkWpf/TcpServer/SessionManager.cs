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

        private readonly ConcurrentDictionary<string, Session> 
            _sessions = new ConcurrentDictionary<string, Session>();

        private readonly ConcurrentDictionary<string, Dictionary<string, string>> 
            _sessionData = new ConcurrentDictionary<string, Dictionary<string, string>>();

        internal string CreateSession()
        {
            string sessionId = Guid.NewGuid().ToString();
            if (_sessions.TryAdd(sessionId, new Session() { SessionId = sessionId, LastAccessTime = DateTime.UtcNow }))
            {
                return sessionId;
            }
            else {
                return string.Empty;
            }
        }

        internal bool TouchSession(string sessionId)
        {
            //Session  
        }

        internal string ParseSessionData(string sessionId, string requestAction)
        {
            throw new NotImplementedException();
        }

        internal object GetSessionData(string sessionId, string requestAction)
        {
            throw new NotImplementedException();
        }

        internal void CleanupAllSessions()
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
