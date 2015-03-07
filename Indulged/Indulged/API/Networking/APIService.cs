using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indulged.API.Networking
{
    public partial class APIService
    {
        public static int PerPage = 20;

        // Common extra parameters
        public static string CommonPhotoExtraParameters = "description,date_taken,views,tags,license,owner_name,o_dims,url_m";

        private static volatile APIService instance;
        private static object syncRoot = new Object();

        /// <summary>
        /// Singleton
        /// </summary>
        public static APIService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new APIService();
                    }
                }

                return instance;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public APIService()
        {

        }
    }
}
