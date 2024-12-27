using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Repository;

namespace WebApplication1.Controllers.Photos;

/// <summary>
/// This controller manage the upload of photos.
/// </summary>
[Route("api/v1/photo")]
[ApiController]
public class PhotosController : Controller
{
    private readonly IWebHostEnvironment env;

    private readonly IPathPhotoRepository image;

    /// <summary>
    /// This constructor inject the pathphoto repository  to be use by the photos controller.
    /// </summary>
    /// <param name="userRepository"></param>
    /// <param name="env"></param>
    /// <param name="image"></param>
    public PhotosController(IWebHostEnvironment env, IPathPhotoRepository image)
    {
        this.image = image;
        this.env = env;
    }

    /// <summary>
    /// This endpoint adds a new photo in database.
    /// </summary>
    [Route("save-file")]
    [HttpPost]
    public JsonResult saveFile()
    {
        var httpRequest = Request.Form;
        var postedFile = httpRequest.Files[0];
        string filename = postedFile.FileName;
        var physicalPath = env.ContentRootPath + "/Photos/" + filename;

        using (var stream = new FileStream(physicalPath, FileMode.Create))
        {
            postedFile.CopyTo(stream);
        }

        PathPhoto imageSave = new PathPhoto();
        imageSave.name = filename;
        imageSave.type = "U";
        image.addImage(imageSave);

        return new JsonResult(filename);
    }

    /// <summary>
    /// This endpoint adds a new photo in database.
    /// </summary>
    [Route("save-filesTruck")]
    [HttpPost]
    public JsonResult saveFileTruck()
    {
        var httpRequest = Request.Form;
        var postedFile = httpRequest.Files[0];
        string filename = postedFile.FileName;
        var physicalPath = env.ContentRootPath + "/Photos/" + filename;

        using (var stream = new FileStream(physicalPath, FileMode.Create))
        {
            postedFile.CopyTo(stream);
        }

        PathPhoto imageSave = new PathPhoto();
        imageSave.name = filename;
        imageSave.type = "T";
        image.addImage(imageSave);

        return new JsonResult(filename);
    }
}