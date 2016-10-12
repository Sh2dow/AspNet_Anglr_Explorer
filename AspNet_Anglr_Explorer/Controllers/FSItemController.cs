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
            : this(new LocalFSItemManager(""))
        {
        }

        public FSItemController(IFSItemManager fsManager)
        {
            this.fsManager = fsManager;
        }

        public async Task<IHttpActionResult> Get(string path)
        {
            var newpath = (string.IsNullOrEmpty(path)) ? HttpRuntime.AppDomainAppPath : path;
            fsManager = new LocalFSItemManager(newpath);
            
            return Ok(new { fsitems = await fsManager.Get(newpath) });
        }
    }
}
