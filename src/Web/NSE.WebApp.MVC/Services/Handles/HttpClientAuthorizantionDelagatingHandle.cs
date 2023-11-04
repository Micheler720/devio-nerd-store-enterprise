﻿using NSE.WebAPI.Core.Usuario;
using NSE.WebApp.MVC.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Services.Handles
{
    public class HttpClientAuthorizantionDelagatingHandle : DelegatingHandler
    {
        private readonly IAspNetUser _user;

        public HttpClientAuthorizantionDelagatingHandle(IAspNetUser user)
        {
            _user = user;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var authorationHeader = _user.ObterHttpContext().Request.Headers["Authorization"];
            if(!string.IsNullOrEmpty(authorationHeader))
            {
                request.Headers.Add("Authorization", new List<string>() { authorationHeader });
            }
            var token = _user.ObterUserToken();
            if(token != null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            return base.SendAsync(request, cancellationToken);
        }
    }
}
