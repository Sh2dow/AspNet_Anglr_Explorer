using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FSExplorer.BL;
using FSExplorer.Models;

namespace AspNet_Anglr_Explorer.Controllers
{
    public class FSItemController : ApiController
    {
        private IFS photoManager;

        public PhotoController()
            : this(new LocalPhotoManager(HttpRuntime.AppDomainAppPath + @"\Album"))
        {
        }

        public PhotoController(IPhotoManager photoManager)
        {
            this.photoManager = photoManager;
        }

        // GET: api/Photo
        public async Task<IHttpActionResult> Get()
        {
            var results = await photoManager.Get();
            return Ok(new { photos = results });
        }
    }
}
