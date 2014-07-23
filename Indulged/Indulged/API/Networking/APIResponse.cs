using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indulged.API.Networking
{
    public class APIResponse
    {
        public string Result { get; set; }
        public string ErrorMessage { get; set; }
        public bool Success { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public APIResponse()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="result"></param>
        /// <param name="errorMessage"></param>
        public APIResponse(string result, string errorMessage, bool success)
        {
            Result = result;
            ErrorMessage = errorMessage;
            Success = success;
        }
    }
}
