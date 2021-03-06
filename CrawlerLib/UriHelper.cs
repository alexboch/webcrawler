﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerLib
{
    public static class UriHelper
    {
        /// <summary>
        /// Создает из Uri абсолютное, если оно является относительным
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="baseUri"></param>
        /// <returns></returns>
        public static Uri MakeAbsoluteUriIfNeeded(Uri uri,Uri baseUri)
        {
            if (!uri.IsAbsoluteUri)
                uri = new Uri(baseUri, uri);
            return uri;
        }
        
        public static bool IsSubdomainOf(this Uri uri,Uri domainUri)
        {
            //string schemeString=uri.Scheme.ToString();
            //string noSchemeStr = uri.ToString().Replace(schemeString, "");
            string host = uri.Host;
            string domainHost = domainUri.Host;
            var parts = host.Split('.');
            var domainParts = domainHost.Split('.');
            bool isSubdomainOf = false;
            for(int i=domainParts.Length-1,j=parts.Length-1;i>=0;i--,j--)
            {
                
                if(j<0||domainParts[i]!=parts[j])
                {
                    isSubdomainOf = true;
                    break;
                }
            }
            return isSubdomainOf;
        }
    }
}
