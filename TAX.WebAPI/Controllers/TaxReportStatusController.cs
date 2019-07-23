using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using UIDP.BIZModule;
using UIDP.UTILITY;

namespace TAX.WebAPI.Controllers
{
    [Produces("application/json")]
    [Consumes("application/json", "multipart/form-data")]
    [Route("TaxReportStatus")]
    public class TaxReportStatusController : WebApiBaseController
    {
        
    }
}