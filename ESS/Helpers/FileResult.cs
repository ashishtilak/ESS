﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace ESS.Helpers
{
    public class FileResult : IHttpActionResult
    {
        private readonly string _filePath;
        private readonly string _contentType;

        public FileResult(string filePath, string contentType = null)
        {
            this._filePath = filePath;
            this._contentType = contentType;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                try
                {
                    HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StreamContent(File.OpenRead(_filePath))
                    };

                    var contentType = _contentType ?? MimeMapping.GetMimeMapping(Path.GetExtension(_filePath));
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);

                    return response;
                }
                catch (Exception ex)
                {
                    HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent(ex.ToString()),
                        ReasonPhrase = ex.Message
                    };
                    return response;
                }
            }, cancellationToken);
        }
    }
}