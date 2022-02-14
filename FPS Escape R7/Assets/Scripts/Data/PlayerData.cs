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
        public ulong ID { get; }
        /// <summary>
        /// Ник игрока, ну с ним всё понятно.
        /// </summary>
        public string Nickame { get; }
        /// <summary>
        /// Ранг игрока (звание). Пока что хз, что там будет.
        /// </summary>
        public PlayerRank Rank { get; }
        //Ну с этими 4 свойствами тоже всё понятно должно быть

        public ulong TotalKills { get; }

        public ulong TotalHeadshots { get; }

        public ulong TotalDeaths { get; }

        public ulong TotalAssists { get; }

        public ulong TotalDamage { get; }

        public ulong TotalDamageReceived { get; }

        /// <summary>
        /// Это свойство показывает, сколько времени отыграл игрок.
        /// </summary>
        public TimeSpan TotalPlayTime { get; }

        public uint TotalMatches { get; }

        public GameRegion PlayerRegion { get; }

        public PlayerData(ulong id, string nickname) : this()
        {
            ID = id;
            Nickame = nickname;
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
