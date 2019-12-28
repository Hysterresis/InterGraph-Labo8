using System;
using System.Windows.Media;

namespace InterGraph_Labo8
{
    public class StaticLib
    {
        public static bool CheckIPv4(string ip)
        {
            string[] ipBytes = ip.Split('.');
            if (ipBytes.Length != 4)
            {
                return false;
            }
            foreach (var ipByte in ipBytes)
            {
                try
                {
                    Convert.ToByte(ipByte);
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            return true;
        }

        public static Color MixColors(Color colorA, Color colorB)
        {
            const double MixCoefficient = 0.95;
            return Color.FromArgb(255,
                (byte)(MixCoefficient * colorA.R + (1 - MixCoefficient) * colorB.R),
                (byte)(MixCoefficient * colorA.G + (1 - MixCoefficient) * colorB.G),
                (byte)(MixCoefficient * colorA.B + (1 - MixCoefficient) * colorB.B));
        }
    }
}
