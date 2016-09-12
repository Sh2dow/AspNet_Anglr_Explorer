using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FSExplorer.BL;
using FSExplorer.Models;
using FSExplorer.BL.Logic;
using System.Threading.Tasks;
using System.Web;

namespace AspNet_Anglr_Explorer.Controllers
{
    [RoutePrefix("api/fsitem")]
    public class FSItemController : ApiController
    {
        private IFSItemManager fsManager;

        public FSItemController()
            : this(new LocalFSItemManager(HttpRuntime.AppDomainAppPath))
        {
        }

        public FSItemController(IFSItemManager fsManager)
        {
            this.fsManager = fsManager;
        }

        // GET: api/fsitem
        public async Task<IHttpActionResult> Get()
        {
            var results = await fsManager.Get();
            return Ok(new { fsitems = results });
        }
    }
}
