using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Data
{
    /// <summary>
    /// По идее, этот класс должен хранить данные об игроке. Можно его сделать структурой.
    /// </summary>
    [Serializable]
    public struct PlayerData
    {
        /// <summary>
        /// ID игрока, будет нужен для того, чтобы различать игроков (по-сути, как в дс или ещё где-нибудь).
        /// </summary>
        public ulong ID { get; set; }
        /// <summary>
        /// Ник игрока, ну с ним всё понятно.
        /// </summary>
        public string Nickname { get; set; }
        /// <summary>
        /// Ранг игрока (звание). Пока что хз, что там будет.
        /// </summary>
        public PlayerRank Rank { get; set; }
        //Ну с этими 4 свойствами тоже всё понятно должно быть

        public ulong TotalKills { get; set; }

        public ulong TotalHeadshots { get; set; }

        public ulong TotalDeaths { get; set; }

        public ulong TotalAssists { get; set; }

        public ulong TotalDamage { get; set; }

        public ulong TotalDamageReceived { get; set; }

        /// <summary>
        /// Это свойство показывает, сколько времени отыграл игрок.
        /// </summary>
        public TimeSpan TotalPlayTime { get; set; }

        public uint TotalMatches { get; set; }

        public GameRegion PlayerRegion { get; set; }

        public PlayerData(ulong id, string nickname) : this()
        {
            ID = id;
            Nickname = nickname;
        }

        public enum PlayerRank : byte
        {

        }
    }

    public enum GameRegion : byte
    {
        Europe,
        Asia,
        US,
        //....что-нибудь ещё.
    }
}
