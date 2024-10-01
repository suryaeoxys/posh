using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Models.DTO.API
{
    public class CustomException : ApplicationException
    {
        public CustomException()
        {
        }

        public CustomException(string message)
            : base(message)
        {
        }

        public CustomException(string message, Exception? inner)
            : base(message, inner)
        {
        }

        //Overriding the HelpLink Property
        public override string HelpLink
        {
            get
            {
                return "Get More Information from here: https://sitelink.com/customPageForErrorDescription";
            }
        }
    }
}
