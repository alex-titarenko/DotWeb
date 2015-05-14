﻿using System;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Web;


namespace TAlex.Web.Services.Storage
{
    public class LocalStorageService : IStorageService
    {
        #region IStorageService Members

        public bool UploadBlob(Stream stream, string path)
        {
            try
            {
                string mapPath = HttpContext.Current.Server.MapPath(path);
                string dirName = Path.GetDirectoryName(mapPath);
                if (!Directory.Exists(dirName))
                {
                    Directory.CreateDirectory(dirName);
                }

                using (var fileStream = new FileStream(mapPath, FileMode.Create))
                {
                    stream.CopyTo(fileStream);
                    fileStream.Flush();
                }
            }
            catch (Exception exc)
            {
                Trace.TraceError("An exception occurred while uploading blob: {0}", exc);
                return false;
            }

            return true;
        }

        public void DeleteBlob(string path)
        {
            if (path != null)
            {
                try
                {
                    var mapPath = HttpContext.Current.Server.MapPath(path);
                    var dirName = Path.GetDirectoryName(mapPath);
                    File.Delete(mapPath);

                    if (!Directory.GetFiles(dirName, "*.*", SearchOption.AllDirectories).Any())
                    {
                        Directory.Delete(dirName, true);
                    }
                }
                catch (DirectoryNotFoundException)
                {
                }
            }
        }

        #endregion
    }
}