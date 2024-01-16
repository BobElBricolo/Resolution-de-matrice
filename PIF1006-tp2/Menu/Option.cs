using System;
using System.Xml.Serialization;

namespace PIF1006_tp2
{
    public class Option
    {
        public string TitreOption { get; }
        public Delegate CodeOption { get; }

        public Option(string titre, Delegate func)
        {
            TitreOption = titre;
            CodeOption = func;
        }
    }
}