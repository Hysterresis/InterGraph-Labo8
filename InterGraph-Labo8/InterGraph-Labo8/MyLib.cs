using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterGraph_Labo8
{
    public class SecureInput
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
    }
}
