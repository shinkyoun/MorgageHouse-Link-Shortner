using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using MorgageHouse_Link_Shortner.Server.Interfaces;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using System.IO;
using System.Reflection;

namespace MorgageHouse_Link_Shortner.Server.Controllers
{
	[Route("")]
	[Controller]
	public class HomeController : ControllerBase
	{
		private readonly IWebHostEnvironment _webHostEnv;
		private readonly ILogger _logger;
        private readonly IUrlLinkShortnerService _linkShortnerSvc;

		private static List<string> _rootFolderFilenames = new List<string>();

		public HomeController(IWebHostEnvironment hostingEnvironment, ILoggerFactory logger, IUrlLinkShortnerService linkShortnerSvc)
		{
			this._webHostEnv = hostingEnvironment;
			this._logger = logger.CreateLogger("Home");
            this._linkShortnerSvc = linkShortnerSvc;
		}

		private void FillRootFolderFileNames()
		{
			if(HomeController._rootFolderFilenames.Count < 1) 
			{
				lock (HomeController._rootFolderFilenames)
				{
                    if (HomeController._rootFolderFilenames.Count < 1)
					{
						if(this._webHostEnv.WebRootPath != null)
						{
                            var files = Directory.GetFiles(this._webHostEnv.WebRootPath);
                            if (files != null && files.Length > 0)
                            {
                                foreach (var file in files)
                                {
                                    HomeController._rootFolderFilenames.Add(new FileInfo(file).Name.ToLower());
                                }
                            }
                        }
                    }
                }
			}
		}
		private async Task<byte[]?> TryGetStaticFilesAsync(string fileName)
		{
            this._logger.LogInformation($"WebRootPath: ${this._webHostEnv.WebRootPath}");
            try
			{
                var fileInfo = this._webHostEnv.WebRootFileProvider.GetFileInfo(fileName);
                if (fileInfo != null && fileInfo.Exists)
				{
					using (var readStream = fileInfo.CreateReadStream())
					{
						using(var mstream = new MemoryStream())
						{
							await readStream.CopyToAsync(mstream);
							return mstream.ToArray();

                        }
                    }
                }
				return null;
            }
            catch (Exception err)
			{
                return null;
            }
        }

		// GET /5
		[HttpGet("{shortUrl}")]
		public async Task<IActionResult> Get([FromRoute] string shortUrl)
		{
			if (String.IsNullOrWhiteSpace(shortUrl))
			{
                return NotFound();
            }

            this.FillRootFolderFileNames();
			var contains = false;
            lock (HomeController._rootFolderFilenames)
            {
                contains = HomeController._rootFolderFilenames.Contains(shortUrl.ToLower());
            }
            if (contains)
            {
                this._logger.LogInformation($"WebRootPath: {shortUrl} is root path file");
                var fileContent = await this.TryGetStaticFilesAsync(shortUrl);
				if(fileContent == null)
				{
                    return NotFound();
                }
                var fileProvider = new FileExtensionContentTypeProvider();
				string? contentType;
                var isOK = fileProvider.TryGetContentType(shortUrl.ToLower(), out contentType);
				if(contentType == null)
				{
					contentType = "applcation/octet-stream";

                }
				return File(fileContent, contentType);
            }
            this._logger.LogInformation($"WebRootPath: {shortUrl} is not root path file");

            var found = await this._linkShortnerSvc.GetUrlLink(shortUrl);
			if(found != null && !String.IsNullOrWhiteSpace(found.FullUrl))
			{
				return Redirect(found.FullUrl);
			}
			else
			{
				return BadRequest($"Invalid request id, {shortUrl}");
			}
		}
	}
}
