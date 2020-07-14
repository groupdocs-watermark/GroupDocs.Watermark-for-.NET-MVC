using GroupDocs.Watermark.MVC.Products.Common.Util.Comparator;
using GroupDocs.Watermark.MVC.Products.Watermark.Entity.Web;
using GroupDocs.Watermark.MVC.Products.Watermark.Entity.XML;
using GroupDocs.Watermark.MVC.Products.Watermark.Util.Directory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace GroupDocs.Watermark.MVC.Products.Watermark.Loader
{
    /// <summary>
    /// WatermarkLoader.
    /// </summary>
    public class WatermarkLoader
    {
        private readonly string currentPath;
        private readonly Common.Config.GlobalConfiguration globalConfiguration;
        private readonly DirectoryUtils directoryUtils;

        /// <summary>
        /// Initializes a new instance of the <see cref="WatermarkLoader"/> class.
        /// Constructor.
        /// </summary>
        /// <param name="path">string.</param>
        /// <param name="globalConfiguration">Common.Config.GlobalConfiguration.</param>
        /// <param name="directoryUtils">DirectoryUtils.</param>
        public WatermarkLoader(string path, Common.Config.GlobalConfiguration globalConfiguration, DirectoryUtils directoryUtils)
        {
            this.currentPath = path;
            this.globalConfiguration = globalConfiguration;
            this.directoryUtils = directoryUtils;
        }

        /// <summary>
        /// Load image watermarks.
        /// </summary>
        /// <returns>List[WatermarkFileDescriptionEntity].</returns>
        public List<WatermarkFileDescriptionEntity> LoadImageWatermarks()
        {
            string[] files = Directory.GetFiles(this.currentPath, "*.*", SearchOption.TopDirectoryOnly);
            List<string> allFiles = new List<string>(files);
            List<WatermarkFileDescriptionEntity> fileList = new List<WatermarkFileDescriptionEntity>();
            try
            {
                allFiles.Sort(new FileDateComparator());
                allFiles.Sort(new FileNameComparator());

                foreach (string file in allFiles)
                {
                    FileInfo fileInfo = new FileInfo(file);

                    // check if current file/folder is hidden
                    if (fileInfo.Attributes.HasFlag(FileAttributes.Hidden) || file.Equals(this.globalConfiguration.Watermark.dataDirectory))
                    {
                        // ignore current file and skip to next one
                        continue;
                    }
                    else
                    {
                        WatermarkFileDescriptionEntity fileDescription = new WatermarkFileDescriptionEntity();
                        fileDescription.guid = Path.GetFullPath(file);
                        fileDescription.name = Path.GetFileName(file);

                        // set is directory true/false
                        fileDescription.isDirectory = fileInfo.Attributes.HasFlag(FileAttributes.Directory);

                        // set file size
                        fileDescription.size = fileInfo.Length;

                        // get image Base64 incoded string
                        byte[] imageArray = File.ReadAllBytes(file);
                        string base64ImageRepresentation = Convert.ToBase64String(imageArray);
                        fileDescription.image = base64ImageRepresentation;

                        // add object to array list
                        fileList.Add(fileDescription);
                    }
                }

                return fileList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Load digital watermarks or documents.
        /// </summary>
        /// <returns>List[WatermarkFileDescriptionEntity].</returns>
        public List<WatermarkFileDescriptionEntity> LoadFiles()
        {
            List<string> allFiles = new List<string>(Directory.GetFiles(this.currentPath));
            allFiles.AddRange(Directory.GetDirectories(this.currentPath));
            List<WatermarkFileDescriptionEntity> fileList = new List<WatermarkFileDescriptionEntity>();
            string dataDirectory = this.globalConfiguration.Watermark.dataDirectory;

            try
            {
                allFiles.Sort(new Common.Util.Comparator.FileNameComparator());
                allFiles.Sort(new FileDateComparator());

                foreach (string file in allFiles)
                {
                    FileInfo fileInfo = new FileInfo(file);

                    // check if current file/folder is hidden
                    if (!fileInfo.Attributes.HasFlag(FileAttributes.Hidden) &&
                        !Path.GetFileName(file).StartsWith(".") &&
                        !Path.GetFileName(file).Equals(Path.GetFileName(dataDirectory), StringComparison.OrdinalIgnoreCase))
                    {
                        WatermarkFileDescriptionEntity fileDescription = new WatermarkFileDescriptionEntity();
                        fileDescription.guid = Path.GetFullPath(file);
                        fileDescription.name = Path.GetFileName(file);

                        // set is directory true/false
                        fileDescription.isDirectory = fileInfo.Attributes.HasFlag(FileAttributes.Directory);

                        // set file size
                        if (!fileDescription.isDirectory)
                        {
                            fileDescription.size = fileInfo.Length;
                        }

                        // add object to array list
                        fileList.Add(fileDescription);
                    }
                }

                return fileList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// LoadTextWatermarks.
        /// </summary>
        /// <param name="xmlFolder">string.</param>
        /// <returns>List<WatermarkFileDescriptionEntity>.</returns>
        public List<WatermarkFileDescriptionEntity> LoadTextWatermarks(string xmlFolder)
        {
            try
            {
                string xmlPath = this.currentPath + xmlFolder;
                string[] xmlFiles = Directory.GetFiles(xmlPath);

                // get all files from the directory
                List<WatermarkFileDescriptionEntity> fileList = new List<WatermarkFileDescriptionEntity>();
                foreach (string xmlFile in xmlFiles)
                {
                    WatermarkFileDescriptionEntity fileDescription = new WatermarkFileDescriptionEntity();
                    fileDescription.guid = xmlFile;
                    fileDescription.name = Path.GetFileName(xmlFile);

                    // get stream of the xml file
                    StreamReader xmlStream = new StreamReader(xmlFile);

                    // initiate serializer
                    XmlSerializer serializer = new XmlSerializer(typeof(TextXmlEntity));

                    // deserialize XML into the object
                    TextXmlEntity xmlData = (TextXmlEntity)serializer.Deserialize(xmlStream);
                    fileDescription.text = xmlData.text;
                    fileDescription.fontColor = xmlData.fontColor;
                    xmlStream.Close();
                    xmlStream.Dispose();

                    // add object to array list
                    fileList.Add(fileDescription);
                }

                return fileList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}