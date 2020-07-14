using GroupDocs.Watermark.Common;
using GroupDocs.Watermark.MVC.Products.Common.Entity.Web;
using GroupDocs.Watermark.MVC.Products.Common.Resources;
using GroupDocs.Watermark.MVC.Products.Watermark.Config;
using GroupDocs.Watermark.MVC.Products.Watermark.Entity.Directory;
using GroupDocs.Watermark.MVC.Products.Watermark.Entity.Web;
using GroupDocs.Watermark.MVC.Products.Watermark.Entity.XML;
using GroupDocs.Watermark.MVC.Products.Watermark.Loader;
using GroupDocs.Watermark.MVC.Products.Watermark.Util;
using GroupDocs.Watermark.MVC.Products.Watermark.Util.Directory;
using GroupDocs.Watermark.Options;
using GroupDocs.Watermark.Search;
using GroupDocs.Watermark.Watermarks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Xml;
using System.Xml.Serialization;
using static GroupDocs.Watermark.Options.PreviewOptions;

namespace GroupDocs.Watermark.MVC.Products.Watermark.Controllers
{
    /// <summary>
    /// WatermarkApiController.
    /// </summary>
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class WatermarkApiController : ApiController
    {
        private static readonly Common.Config.GlobalConfiguration GlobalConfiguration = new Common.Config.GlobalConfiguration();
        private readonly List<string> supportedImageFormats = new List<string> { ".bmp", ".jpeg", ".jpg", ".tiff", ".tif", ".png" };
        private readonly DirectoryUtils directoryUtils = new DirectoryUtils(GlobalConfiguration.Watermark);

        /// <summary>
        /// Load Watermark configuration.
        /// </summary>
        /// <returns>Watermark configuration.</returns>
        [HttpGet]
        [Route("loadConfig")]
        public WatermarkConfiguration LoadConfig()
        {
            return GlobalConfiguration.Watermark;
        }

        /// <summary>
        /// Get all files and directories from storage.
        /// </summary>
        /// <param name="postedData">WatermarkPostedDataEntity.</param>
        /// <returns>List of files and directories.</returns>
        [HttpPost]
        [Route("loadFileTree")]
        public HttpResponseMessage loadFileTree(WatermarkPostedDataEntity postedData)
        {
            // get request body
            string relDirPath = postedData.path;
            string watermarkType = string.Empty;
            if (!string.IsNullOrEmpty(postedData.watermarkType))
            {
                watermarkType = postedData.watermarkType;
            }

            // get file list from storage path
            try
            {
                string rootDirectory;
                switch (watermarkType)
                {
                    case "image":
                        rootDirectory = this.directoryUtils.DataDirectory.UploadedImageDirectory.Path;
                        break;
                    case "text":
                        rootDirectory = this.directoryUtils.DataDirectory.TextDirectory.Path;
                        break;
                    default:
                        rootDirectory = this.directoryUtils.FilesDirectory.GetPath();
                        break;
                }

                // get all the files from a directory
                if (string.IsNullOrEmpty(relDirPath))
                {
                    relDirPath = rootDirectory;
                }
                else
                {
                    relDirPath = Path.Combine(rootDirectory, relDirPath);
                }

                WatermarkLoader watermarkLoader = new WatermarkLoader(relDirPath, GlobalConfiguration, this.directoryUtils);
                List<WatermarkFileDescriptionEntity> fileList;
                switch (watermarkType)
                {
                    case "image":
                        fileList = watermarkLoader.LoadImageWatermarks();
                        break;
                    case "text":
                        fileList = watermarkLoader.LoadTextWatermarks(DataDirectoryEntity.DATA_XML_FOLDER);
                        break;
                    default:
                        fileList = watermarkLoader.LoadFiles();
                        break;
                }

                return this.Request.CreateResponse(HttpStatusCode.OK, fileList);
            }
            catch (System.Exception ex)
            {
                // set exception message
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new Common.Resources.Resources().GenerateException(ex));
            }
        }

        /// <summary>
        /// Upload document.
        /// </summary>
        /// <returns>Uploaded document object.</returns>
        [HttpPost]
        [Route("uploadDocument")]
        public HttpResponseMessage UploadDocument()
        {
            try
            {
                // get posted data
                string url = HttpContext.Current.Request.Form["url"];
                string watermarkType = HttpContext.Current.Request.Form["watermarkType"];
                bool rewrite = bool.Parse(HttpContext.Current.Request.Form["rewrite"]);

                // get path for where to save the file
                string fileSavePath = string.Empty;
                switch (watermarkType)
                {
                    case "image":
                        fileSavePath = this.directoryUtils.DataDirectory.UploadedImageDirectory.Path;
                        break;
                    default:
                        fileSavePath = this.directoryUtils.FilesDirectory.GetPath();
                        break;
                }

                // check if file selected or URL
                if (string.IsNullOrEmpty(url))
                {
                    if (HttpContext.Current.Request.Files.AllKeys != null)
                    {
                        // Get the uploaded document from the Files collection
                        var httpPostedFile = HttpContext.Current.Request.Files["file"];
                        if (httpPostedFile != null)
                        {
                            if (rewrite)
                            {
                                // Get the complete file path
                                fileSavePath = Path.Combine(fileSavePath, httpPostedFile.FileName);
                            }
                            else
                            {
                                fileSavePath = Resources.GetFreeFileName(fileSavePath, httpPostedFile.FileName);
                            }

                            // Save the uploaded file to "UploadedFiles" folder
                            httpPostedFile.SaveAs(fileSavePath);
                            httpPostedFile.InputStream.Close();
                        }
                    }
                }
                else
                {
                    using (WebClient client = new WebClient())
                    {
                        // get file name from the URL
                        Uri uri = new Uri(url);
                        string fileName = Path.GetFileName(uri.LocalPath);
                        if (rewrite)
                        {
                            // Get the complete file path
                            fileSavePath = Path.Combine(fileSavePath, fileName);
                        }
                        else
                        {
                            fileSavePath = Resources.GetFreeFileName(fileSavePath, fileName);
                        }

                        // Download the Web resource and save it into the current filesystem folder.
                        client.DownloadFile(url, fileSavePath);
                    }
                }

                // initiate uploaded file description class
                WatermarkFileDescriptionEntity uploadedDocument = new WatermarkFileDescriptionEntity
                {
                    guid = fileSavePath,
                };

                MemoryStream ms = new MemoryStream();
                using (FileStream file = new FileStream(fileSavePath, FileMode.Open, FileAccess.ReadWrite))
                {
                    file.CopyTo(ms);
                    byte[] imageBytes = ms.ToArray();

                    // Convert byte[] to Base64 String
                    uploadedDocument.image = Convert.ToBase64String(imageBytes);
                }

                ms.Close();

                return this.Request.CreateResponse(HttpStatusCode.OK, uploadedDocument);
            }
            catch (System.Exception ex)
            {
                // set exception message
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new Resources().GenerateException(ex));
            }
        }

        /// <summary>
        /// Load document description.
        /// </summary>
        /// <param name="postedData">WatermarkPostedDataEntity.</param>
        /// <returns>All info about the document.</returns>
        [HttpPost]
        [Route("loadDocumentDescription")]
        public HttpResponseMessage LoadDocumentDescription(WatermarkPostedDataEntity postedData)
        {
            string password = string.Empty;
            bool loadAllPages = GlobalConfiguration.Watermark.GetPreloadPageCount() == 0;

            try
            {
                // get/set parameters
                string documentGuid = postedData.guid;
                password = postedData.password;
                LoadDocumentEntity loadDocumentEntity = new LoadDocumentEntity();
                IDocumentInfo info;
                PossibleWatermarkCollection possibleWatermarkCollection;

                using (Watermarker watermarker = new Watermarker(documentGuid, GetLoadOptions(password)))
                {
                    info = watermarker.GetDocumentInfo();
                    possibleWatermarkCollection = watermarker.Search();
                }

                loadDocumentEntity.SetGuid(postedData.guid);

                string documentType = this.supportedImageFormats.Contains(info.FileType.Extension) ? "image" : info.FileType.ToString();

                List<string> pagesContent = new List<string>();

                if (loadAllPages)
                {
                    pagesContent = GetAllPagesContent(password, documentGuid, info);
                }

                for (int i = 0; i < info.PageCount; i++)
                {
                    PageDataDescriptionEntity page = new PageDataDescriptionEntity
                    {
                        number = i + 1,
                        height = 792,
                        width = 612,
                    };

                    // if (possibleWatermarkCollection != null && possibleWatermarkCollection.Count > 0)
                    // {
                    //    page.SetWatermarks(WatermarkMapper.MapForPage(possibleWatermarkCollection));
                    // }

                    if (pagesContent.Count > 0)
                    {
                        page.SetData(pagesContent[i]);
                    }

                    loadDocumentEntity.SetPage(page);
                }

                // return document description
                return this.Request.CreateResponse(HttpStatusCode.OK, loadDocumentEntity);
            }
            catch (Exceptions.InvalidPasswordException ex)
            {
                // set exception message
                return this.Request.CreateResponse(HttpStatusCode.Forbidden, new Resources().GenerateException(ex, password));
            }
            catch (Exception ex)
            {
                // set exception message
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new Resources().GenerateException(ex, password));
            }
        }

        /// <summary>
        /// Load document page.
        /// </summary>
        /// <param name="loadDocumentPageRequest">WatermarkPostedDataEntity.</param>
        /// <returns>Document page image encoded in Base64.</returns>
        [HttpPost]
        [Route("loadDocumentPage")]
        public HttpResponseMessage LoadDocumentPage(WatermarkPostedDataEntity loadDocumentPageRequest)
        {
            string password = string.Empty;
            try
            {
                // get/set parameters
                string documentGuid = loadDocumentPageRequest.guid;
                int pageNumber = loadDocumentPageRequest.page;
                password = loadDocumentPageRequest.password;
                PageDataDescriptionEntity loadedPage = new PageDataDescriptionEntity();

                // get page image
                byte[] bytes;

                using (Watermarker watermarker = new Watermarker(documentGuid, GetLoadOptions(password)))
                {
                    using (var memoryStream = RenderPageToMemoryStream(pageNumber, documentGuid, password))
                    {
                        bytes = memoryStream.ToArray();
                    }

                    IDocumentInfo info = watermarker.GetDocumentInfo();
                    PossibleWatermarkCollection watermarks = watermarker.Search();

                    if (watermarks != null && watermarks.Count > 0)
                    {
                        loadedPage.SetWatermarks(WatermarkMapper.MapForPage(watermarks));
                    }

                    string encodedImage = Convert.ToBase64String(bytes);
                    loadedPage.SetData(encodedImage);

                    loadedPage.height = 842;
                    loadedPage.width = 595;
                    loadedPage.number = pageNumber;
                }

                // return loaded page object
                return this.Request.CreateResponse(HttpStatusCode.OK, loadedPage);
            }
            catch (Exception ex)
            {
                // set exception message
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new Resources().GenerateException(ex, password));
            }
        }

        /// <summary>
        /// Load selected watermark image preview.
        /// </summary>
        /// <param name="postedData">WatermarkPostedDataEntity.</param>
        /// <returns>Watermark image preview in Base64.</returns>
        [HttpPost]
        [Route("loadWatermarkImage")]
        public HttpResponseMessage LoadWatermarkImage(WatermarkPostedDataEntity postedData)
        {
            try
            {
                // get/set parameters
                string documentGuid = postedData.guid;
                WatermarkLoadedPageEntity loadedPage = new WatermarkLoadedPageEntity();
                MemoryStream ms = new MemoryStream();
                using (FileStream file = new FileStream(documentGuid, FileMode.Open, FileAccess.ReadWrite))
                {
                    file.CopyTo(ms);
                    byte[] imageBytes = ms.ToArray();

                    // Convert byte[] to Base64 String
                    loadedPage.SetData(Convert.ToBase64String(imageBytes));
                }

                ms.Close();
                if (postedData.watermarkType.Equals("text"))
                {
                    // get xml data of the Text watermark
                    string xmlFileName = Path.GetFileNameWithoutExtension(Path.GetFileName(documentGuid));
                    string xmlPath = this.directoryUtils.DataDirectory.TextDirectory.XmlPath;

                    // Load xml data
                    TextXmlEntity textData = LoadXmlData<TextXmlEntity>(xmlPath, xmlFileName);
                    loadedPage.props = textData;
                }

                // return loaded page object
                return this.Request.CreateResponse(HttpStatusCode.OK, loadedPage);
            }
            catch (System.Exception ex)
            {
                // set exception message
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new Common.Resources.Resources().GenerateException(ex));
            }
        }

        /// <summary>
        /// Add watermarks.
        /// </summary>
        /// <param name="watermarkDocumentRequest">WatermarkPostedDataEntity.</param>
        /// <returns>Watermarked document info.</returns>
        [HttpPost]
        [Route("saveWatermark")]
        public HttpResponseMessage SaveWatermark(WatermarkPostedDataEntity watermarkDocumentRequest)
        {
            WatermarkedDocumentEntity watermarkedDocument = new WatermarkedDocumentEntity();
            try
            {
                // get/set parameters
                string documentGuid = watermarkDocumentRequest.guid;
                string password = watermarkDocumentRequest.password;
                string documentType = this.supportedImageFormats.Contains(Path.GetExtension(watermarkDocumentRequest.guid)) ? "image" : watermarkDocumentRequest.documentType;
                string tempPath = GetTempPath(documentGuid);

                WatermarkDataEntity[] watermarksData = watermarkDocumentRequest.watermarksData;

                // initiate list of watermarks to add
                List<GroupDocs.Watermark.Watermark> watermarks = new List<GroupDocs.Watermark.Watermark>();

                using (Watermarker watermarker = new Watermarker(documentGuid, GetLoadOptions(password)))
                {
                    IDocumentInfo info = watermarker.GetDocumentInfo();

                    for (int i = 0; i < watermarksData.Length; i++)
                    {
                        WatermarkDataEntity watermarkData = watermarksData[i];

                        // TODO: check supported watermarks?
                        try
                        {
                            // TODO: add enum?
                            if (watermarkData.watermarkType == "text")
                            {
                                string xmlPath = this.directoryUtils.DataDirectory.TextDirectory.XmlPath;
                                string xmlFileName = Path.GetFileNameWithoutExtension(watermarkData.watermarkGuid);
                                TextXmlEntity textData = LoadXmlData<TextXmlEntity>(xmlPath, xmlFileName);
                                var fontStyle = textData.bold ? FontStyle.Bold :
                                                (textData.italic ? FontStyle.Italic : 
                                                (textData.underline ? FontStyle.Underline : FontStyle.Regular));

                                var font = new Font(textData.font, textData.fontSize, fontStyle);

                                TextWatermark watermark = new TextWatermark(textData.text, font)
                                {
                                    // TODO: check why following line leads to disappearing watermark on document.
                                    // watermark.ForegroundColor = Color.FromArgb(watermarkData.fontColor);
                                    ForegroundColor = Color.Red,
                                    X = watermarkData.left,
                                    Y = watermarkData.top,
                                    Width = watermarkData.width,
                                    Height = watermarkData.height,
                                };

                                watermarks.Add(watermark);
                            }
                            else
                            {
                                ImageWatermark imageWatermark = new ImageWatermark(watermarkData.watermarkGuid)
                                {
                                    X = watermarkData.left,
                                    Y = watermarkData.top,
                                    Width = watermarkData.width,
                                    Height = watermarkData.height,
                                };

                                watermarks.Add(imageWatermark);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message, ex);
                        }
                    }
                }

                // Remove watermarks from the document
                this.RemoveWatermarks(documentGuid, password);

                // check if watermarks array contains at least one watermark to add
                if (watermarks.Count != 0)
                {
                    using (Watermarker watermarker = new Watermarker(documentGuid, GetLoadOptions(password)))
                    {
                        foreach (var watermark in watermarks)
                        {
                            watermarker.Add(watermark);
                        }

                        watermarker.Save(tempPath);
                    }

                    if (File.Exists(documentGuid))
                    {
                        File.Delete(documentGuid);
                    }

                    File.Move(tempPath, documentGuid);
                }

                watermarkedDocument = new WatermarkedDocumentEntity();
                watermarkedDocument.guid = documentGuid;
            }
            catch (Exception ex)
            {
                // set exception message
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new Resources().GenerateException(ex));
            }

            return this.Request.CreateResponse(HttpStatusCode.OK, watermarkedDocument);
        }

        /// <summary>
        /// Save text Watermark.
        /// </summary>
        /// <param name="postedData">WatermarkPostedDataEntity.</param>
        /// <returns>Text watermark preview image.</returns>
        [HttpPost]
        [Route("saveText")]
        public HttpResponseMessage SaveText([FromBody] dynamic postedData)
        {
            string xmlPath = this.directoryUtils.DataDirectory.TextDirectory.XmlPath;
            try
            {
                TextXmlEntity textData = JsonConvert.DeserializeObject<TextXmlEntity>(postedData.properties.ToString());
                string[] listOfFiles = Directory.GetFiles(xmlPath);
                string fileName = string.Empty;
                string filePath = string.Empty;
                if (File.Exists(textData.imageGuid))
                {
                    fileName = Path.GetFileNameWithoutExtension(textData.imageGuid);
                    filePath = textData.imageGuid;
                }
                else
                {
                    for (int i = 0; i <= listOfFiles.Length; i++)
                    {
                        int number = i + 1;

                        // set file name, for example 001
                        fileName = string.Format("{0:000}", number);
                        filePath = Path.Combine(xmlPath, fileName + ".xml");

                        // check if file with such name already exists
                        if (File.Exists(filePath))
                        {
                            continue;
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                // Save text data to an xml file
                this.SaveXmlData(xmlPath, fileName, textData);

                // set Text data for response
                textData.imageGuid = filePath;

                // return loaded page object
                return this.Request.CreateResponse(HttpStatusCode.OK, textData);
            }
            catch (System.Exception ex)
            {
                // set exception message
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new Common.Resources.Resources().GenerateException(ex));
            }
        }

        /// <summary>
        /// Delete watermark.
        /// </summary>
        /// <param name="deleteWatermarkFileRequest">DeleteWatermarkFileRequest.</param>
        /// <returns>Text watermark preview image.</returns>
        [HttpPost]
        [Route("deleteWatermarkFile")]
        public HttpResponseMessage DeleteWatermarkFile(DeleteWatermarkFileRequest deleteWatermarkFileRequest)
        {
            try
            {
                string watermarkType = deleteWatermarkFileRequest.getWatermarkType();
                switch (watermarkType)
                {
                    case "image":
                        if (File.Exists(deleteWatermarkFileRequest.getGuid()))
                        {
                            File.Delete(deleteWatermarkFileRequest.getGuid());
                        }

                        break;
                    default:
                        if (File.Exists(deleteWatermarkFileRequest.getGuid()))
                        {
                            File.Delete(deleteWatermarkFileRequest.getGuid());
                        }

                        string xmlFilePath = this.GetXmlFilePath(
                            watermarkType,
                            Path.GetFileNameWithoutExtension(deleteWatermarkFileRequest.getGuid()) + ".xml");
                        File.Delete(xmlFilePath);
                        break;
                }

                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (System.Exception ex)
            {
                // set exception message
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new Common.Resources.Resources().GenerateException(ex));
            }
        }

        /// <summary>
        /// Download document.
        /// </summary>
        /// <param name="path">string.</param>
        /// <returns>HttpResponseMessage.</returns>
        [HttpGet]
        [Route("downloadDocument")]
        public HttpResponseMessage DownloadDocument(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                // check if file exists
                if (File.Exists(path))
                {
                    // prepare response message
                    HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

                    // add file into the response
                    var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    response.Content = new StreamContent(fileStream);
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                    {
                        FileName = Path.GetFileName(path),
                    };

                    return response;
                }
            }

            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }

        private static List<string> GetAllPagesContent(string password, string documentGuid, IDocumentInfo pages)
        {
            List<string> allPages = new List<string>();

            // get page HTML
            for (int i = 0; i < pages.PageCount; i++)
            {
                byte[] bytes;
                using (var memoryStream = RenderPageToMemoryStream(i + 1, documentGuid, password))
                {
                    bytes = memoryStream.ToArray();
                }

                string encodedImage = Convert.ToBase64String(bytes);
                allPages.Add(encodedImage);
            }

            return allPages;
        }

        private static MemoryStream RenderPageToMemoryStream(int pageNumberToRender, string documentGuid, string password)
        {
            MemoryStream result = new MemoryStream();

            using (FileStream outputStream = File.OpenRead(documentGuid))
            {
                using (Watermarker watermarker = new Watermarker(outputStream, GetLoadOptions(password)))
                {
                    PreviewOptions previewOptions = new PreviewOptions(pageNumber => result)
                    {
                        PreviewFormat = PreviewFormats.PNG,
                        PageNumbers = new[] { pageNumberToRender },
                    };

                    watermarker.GeneratePreview(previewOptions);
                }
            }

            return result;
        }

        /// <summary>
        /// Load watermark XML data from file.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="xmlPath">XML path.</param>
        /// <param name="xmlFileName">XML file name.</param>
        /// <returns>Watermark data object.</returns>
        private static T LoadXmlData<T>(string xmlPath, string xmlFileName)
        {
            // initiate return object type
            T returnObject = default(T);
            if (string.IsNullOrEmpty(xmlFileName))
            {
                return default(T);
            }

            try
            {
                // get stream of the xml file
                using (StreamReader xmlStream = new StreamReader(Path.Combine(xmlPath, xmlFileName + ".xml")))
                {
                    // initiate serializer
                    XmlSerializer serializer = new XmlSerializer(typeof(T));

                    // deserialize XML into the object
                    returnObject = (T)serializer.Deserialize(xmlStream);
                }
            }
            catch (System.Exception ex)
            {
                Console.Error.Write(ex.Message);
            }

            return returnObject;
        }

        private static string GetTempPath(string documentGuid)
        {
            string tempFilename = Path.GetFileNameWithoutExtension(documentGuid) + "_tmp";
            string tempPath = Path.Combine(Path.GetDirectoryName(documentGuid), tempFilename + Path.GetExtension(documentGuid));
            return tempPath;
        }

        private static LoadOptions GetLoadOptions(string password)
        {
            LoadOptions loadOptions = new LoadOptions
            {
                Password = password,
            };

            return loadOptions;
        }

        private void RemoveWatermarks(string documentGuid, string password)
        {
            var tempPath = GetTempPath(documentGuid);

            using (Watermarker watermarker = new Watermarker(documentGuid, GetLoadOptions(password)))
            {
                PossibleWatermarkCollection possibleWatermarks = watermarker.Search();

                for (var i = 0; i < possibleWatermarks.Count; i++)
                {
                    possibleWatermarks.RemoveAt(i);
                }

                watermarker.Save(tempPath);
            }

            if (File.Exists(documentGuid))
            {
                File.Delete(documentGuid);
            }

            File.Move(tempPath, documentGuid);
        }

        private string GetXmlFilePath(string watermarkType, string fileName)
        {
            string path;
            switch (watermarkType)
            {
                case "text":
                    path = Path.Combine(this.directoryUtils.DataDirectory.TextDirectory.XmlPath, fileName);
                    break;
                default:
                    throw new ArgumentNullException("Watermark type is not defined");
            }

            return path;
        }

        /// <summary>
        /// Save watermark data into the XML.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="xmlPath">XML path.</param>
        /// <param name="xmlFileName">XML file name.</param>
        /// <param name="serializableObject">Object.</param>
        private void SaveXmlData<T>(string xmlPath, string xmlFileName, T serializableObject)
        {
            if (object.Equals(serializableObject, default(T)))
            {
                return;
            }

            try
            {
                // initiate xml
                XmlDocument xmlDocument = new XmlDocument();

                // initiate serializer
                XmlSerializer serializer = new XmlSerializer(serializableObject.GetType());

                // save xml file
                using (MemoryStream stream = new MemoryStream())
                {
                    // serialize data into the xml
                    serializer.Serialize(stream, serializableObject);
                    stream.Position = 0;
                    xmlDocument.Load(stream);
                    xmlDocument.Save(Path.Combine(xmlPath, xmlFileName + ".xml"));
                    stream.Close();
                }
            }
            catch (System.Exception ex)
            {
                Console.Error.Write(ex.Message);
            }
        }
    }
}