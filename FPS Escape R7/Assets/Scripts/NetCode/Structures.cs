using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.NetCode
{
    public struct NetRequestToken
    {
        internal NetRequestToken(INetRequestHolder tokenHolder, int reqType, object sendingData)
        {
            TokenHolder=tokenHolder;
            Type=(RequestType)reqType;
            SendData=sendingData;
        }

        public INetRequestHolder TokenHolder { get; }

        public object SendData { get; }

        public RequestType Type { get; }

        /// <summary>
        /// Предлагаю форматировать данные для запросов этим перечислением.
        /// </summary>
        public enum RequestType
        {
            Disconnect,
            GetPlayerData,
            SetPlayerData,//Вот эта штука может быть опасной, думаю, что лучше всё-таки на сервере рассчитывать результаты.
            SyncronizePosition,
            SendInput,
            //...
        }
    }

    public struct NetResponse
    {
        public object Response { get; set; }

        public NetResponse(object response)
        {
            Response=response;
        }
    }
}
