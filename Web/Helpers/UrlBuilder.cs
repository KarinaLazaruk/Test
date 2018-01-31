using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Web.Helpers
{
    public class UrlBuilder
    {
        public class FluentUrl
        {
            private readonly string _mUrl;
            private readonly Dictionary<string, string> _mQueryParams = new Dictionary<string, string>();

            public FluentUrl(string url)
            {
                _mUrl = url;
            }

            public static implicit operator string(FluentUrl url)
            {
                return url.ToString();
            }

            public FluentUrl WithQueryParam(string key, string value)
            {
                if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(value)) return this;

                _mQueryParams[key] = value;

                return this;
            }

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder(_mUrl);

                //Format query params
                if (_mQueryParams.Any())
                {
                    sb.Append('?');
                    foreach (var queryParam in _mQueryParams)
                    {
                        sb.Append(queryParam.Key + "=" + Uri.EscapeDataString(queryParam.Value));
                        sb.Append('&');
                    }
                    sb.Length -= 1;
                }

                return sb.ToString();
            }
        }

        public static FluentUrl ToRestApiUrl(string restUrl)
        {
            return new FluentUrl(restUrl);
        }
    }
}