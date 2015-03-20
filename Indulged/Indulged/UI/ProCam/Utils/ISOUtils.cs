using Indulged.UI.ProCam.Models;

namespace Indulged.UI.ProCam.Utils
{
    public static class ISOUtils
    {
        public static string ToISOString(this uint iso)
        {
            if (iso == ProCamConstraints.PROCAM_AUTO_ISO)
            {
                return "AUTO";
            }
            else
            {
                return iso.ToString();
            }
        }
    }
}
