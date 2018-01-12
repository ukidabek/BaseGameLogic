using UnityEngine.Networking;
using UnityEngine.Networking.Types;


namespace BaseGameLogic.Networking
{
    public static class NetworkUtility
    {
        private static readonly char[] IP_ADRES_SEPARATORS = { ':' };

        public static string GetIPAdress(string ipAdressString)
        {
            string[] ipAdresParts = ipAdressString.Split(IP_ADRES_SEPARATORS);
            int ipAdresIndex = ipAdresParts.Length - 1;
            return ipAdresParts[ipAdresIndex];
        }

        public static NetworkError GetNetworkError(byte error)
        {
            return (NetworkError)error;
        }
    }
}