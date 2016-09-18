using System.Web.Http;
using System.Threading.Tasks;
using System.Web;
using AspNet_Anglr_Explorer.Logic;

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
        //public async Task<IHttpActionResult> Get()
        //{
        //    var results = await fsManager.Get();

        //    return Ok(new { fsitems = results });
        //}

        public async Task<IHttpActionResult> Get(string path)
        {
            fsManager = new LocalFSItemManager(path);
            var results = await fsManager.Get(path);
            
            return Ok(new { fsitems = results });
        }
    }
}
