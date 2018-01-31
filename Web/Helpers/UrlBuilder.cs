using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Web.Managers;

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

            public FluentUrl WithOptions(RequestOptions options)
            {
                if (options == null) return this;

                if (options.Limit.HasValue && options.Limit > 0) _mQueryParams["limit"] = options.Limit.ToString();

                if (options.Start.HasValue && options.Start >= 0)_mQueryParams["start"] = options.Start.ToString();

                if (!string.IsNullOrWhiteSpace(options.At)) _mQueryParams["at"] = options.At;

                return this;
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

        public static string FormatRestApiUrl(string restUrl, RequestOptions requestOptions = null, params string[] inputs)
        {
            return FormatRestApiUrl(restUrl, true, requestOptions, inputs);
        }

        public static string FormatRestApiUrl(string restUrl, bool escapeUrlData, RequestOptions requestOptions = null, params string[] inputs)
        {
            StringParamsValidator(inputs.Length, inputs);

            var resultingUrl = escapeUrlData ? string.Format(restUrl, UrlEscapeDataParams(inputs)) : string.Format(restUrl, UrlEscapeUriParams(inputs));

            if (requestOptions != null)
            {
                var partialUrl = "";
                var urlHasQueryParams = restUrl.IndexOf('?') > -1;

                if (requestOptions.Limit != null && requestOptions.Limit.Value > 0)
                {
                    partialUrl += string.IsNullOrWhiteSpace(partialUrl) && !urlHasQueryParams ? "?" : "&";
                    partialUrl += $"limit={requestOptions.Limit.Value}";
                }

                if (requestOptions.Start != null && requestOptions.Start.Value >= 0)
                {
                    partialUrl += string.IsNullOrWhiteSpace(partialUrl) && !urlHasQueryParams ? "?" : "&";
                    partialUrl += $"start={requestOptions.Start.Value}";
                }

                if (!String.IsNullOrWhiteSpace(requestOptions.At))
                {
                    partialUrl += string.IsNullOrWhiteSpace(partialUrl) && !urlHasQueryParams ? "?" : "&";
                    partialUrl += $"at={requestOptions.At}";
                }

                resultingUrl += partialUrl;
            }

            return resultingUrl;
        }

        private static void StringParamsValidator(int validParamCount, params string[] inputs)
        {
            if (inputs.Length != validParamCount || inputs.Any(string.IsNullOrWhiteSpace))
            {
                throw new ArgumentException($"Wrong number of parameters passed, expecting exactly '{validParamCount}' parameters");
            }
        }
        
        private static string[] UrlEscapeDataParams(string[] inputs)
        {
            var result = new string[inputs.Length];

            for (var i = 0; i < inputs.Length; i++)
                result[i] = Uri.EscapeDataString(inputs[i]);

            return result;
        }

        private static string[] UrlEscapeUriParams(string[] inputs)
        {
            var result = new string[inputs.Length];

            for (var i = 0; i < inputs.Length; i++)
                result[i] = Uri.EscapeUriString(inputs[i]);

            return result;
        }
    }
}