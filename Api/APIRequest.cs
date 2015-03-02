using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using WallExporter.Infrastructure;
using WallExporter.Json;

namespace WallExporter.Api
{
    public class APIRequest
    {
        public APIRequest(string methodName)
        {
            //_settings = new Settings(new ProtectDataAdapter());

            _parameters = new Dictionary<string, string>();
            _parameters.Add("method", methodName);
        }

        #region Protected method

        protected bool AddParameter(string name, string value)
        {
            if (!_parameters.ContainsKey(name))
                _parameters.Add(name, value);
            else
                return false;

            return true;
        }

        protected void SetSuccessHandler(Action<string> successHandler)
        {
            _successHandler = successHandler;
        }

        #endregion

        #region Public method

        public void Execute()
        {
            try
            {
                VKWebClient client = new VKWebClient();
                client.SendPostRequest(@"https://api.vk.com/method/" + _parameters["method"], _CreateRequestString(), _ParseResponse);
            }
            catch
            {
                _ParseResponse(null);
            }
        }

        #endregion

        #region Private method

        private string _CreateRequestString()
        {
            string request = string.Empty;
            foreach (var key in _parameters.Keys)
            {
                if (key != "method")
                {
                    request += key + "=";
                    request += _parameters[key] + "&";
                }
            }

            //request += "access_token" + "=";
            //request += _settings.AccessToken;

            string sig = CommonHelper.DoDigest(@"/method/" + _parameters["method"] + "?" + request); // + _settings.Secret);
            request += "&sig" + "=" + sig.ToLower();

            return request;
        }

        private void _ParseResponse(string response)
        {
            try
            {
                if (response == null)
                {
                    _successHandler(null);
                }
                else if (response.Contains("\"error\":"))
                {
                    Debug.WriteLine("Error in response...");

                    // Error
                    var errorResponse = SerializeHelper.Deserialise<APIError>(response);

                    Console.Write(errorResponse.ErrorDescription.error_msg);

                    // Special case for captcha
                    if (errorResponse != null &&
                        !string.IsNullOrEmpty(errorResponse.ErrorDescription.captcha_img) &&
                        !string.IsNullOrEmpty(errorResponse.ErrorDescription.captcha_sid)
                        )
                    {
                        // TODO show captcha.
                    }
                    else
                        _successHandler(null);
                }
                else
                {
                    Debug.Assert(!response.Contains("\"error\":"));
                    Debug.Assert(response.Contains("\"response\":"));

                    // Ok
                    _successHandler(response);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Parse response from APIRequest failed." + ex.Message);
                _successHandler(null);
            }
        }

        #endregion

        #region Private fields

        //private Settings _settings = null;
        private Dictionary<String, String> _parameters;
        private Action<string> _successHandler;

        #endregion
    }
}
