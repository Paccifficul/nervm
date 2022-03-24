using System.Collections.Generic;

namespace InGameScripts
{
    public static class GenerateNickName
    {
        public static List<string> NickNames { get; } = new();

        static GenerateNickName()
        {
            NickNames.Add("Numbers");
            NickNames.Add("Brown Sugar");
            NickNames.Add("Amiga");
            NickNames.Add("Cuddle Pig");
            NickNames.Add("Creedence");
            NickNames.Add("Fellow");
            NickNames.Add("Itchy");
            NickNames.Add("Dinga");
            NickNames.Add("Dorito");
            NickNames.Add("Ace");
            NickNames.Add("Rambo");
            NickNames.Add("Freckles");
        }
    }
}