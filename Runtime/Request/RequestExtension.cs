﻿using System.Text;
using Response;
using Response.Fail;
using UnityEngine;
using Util;

namespace Request
{
    public enum RequestTarget
    {
        client,
        server
    }
    
    public static class RequestExtension
    {
        public static byte[] ToBytes(this object value)
        {
            var jsonForm = JsonUtility.ToJson(value);
            return Encoding.UTF8.GetBytes(jsonForm);
        }

        public static bool IsFormValid(this IRequest request, IValidatedForm form, out IFailResponse fail)
        {
            fail = null;
            if (form.IsValid(out var description))
            {
                return true;
            }

            fail = new InvalidFormFail(description);
            return false;
        }
        
        public static IResponse GetResponse<TSuccessResponse>(this IRequest request) where TSuccessResponse : IResponse
        {
            return request.TryGetFailResponse(out var fail) ? fail : JsonUtility.FromJson<TSuccessResponse>(request.Body.downloadHandler.text);
        }
        
        public static bool TryGetFailResponse(this IRequest request, out IFailResponse fail)
        {
            fail = null;
            var serverFail = JsonUtility.FromJson<ServerFail>(request.Body.downloadHandler.text);
            if (serverFail.HasValue())
            {
                fail = serverFail;
                return true;
            }
            if (!request.Body.error.IsNullOrEmpty())
            {
                fail = UnityWebRequestFail.CreateFromRequest(request.Body);
                return true;
            }
            return false;
        }
    }
}