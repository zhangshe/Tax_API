using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;
namespace DGPF.WEBAPI
{
    public class AuthFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Microsoft.Extensions.Primitives.StringValues AccessToken;//获取header中某一项的值
            context.HttpContext.Request.Headers.TryGetValue("X-Token", out AccessToken);
            //根据实际需求进行具体实现
            //context.Result = new ObjectResult(new { code = 200, msg = "", result = "adsfdsaf" });

            if (true)
            {
                Message mes = new Message();
                mes.code = 50008;
                mes.message = "非法的token";
                mes.result = "";
                
                mes.code = 50012;
                mes.message = "其他客户端登录了";
                mes.result = "";

                mes.code = 50014;
                mes.message = "Token 过期了";
                mes.result = "";
                
            }
           



        }
    }
    public class Message
    {
        /// <summary>
        /// 2000：合法；50008:非法的token; 50012:其他客户端登录了;  50014:Token 过期了;
        /// </summary>
        public int code { get; set; }
        /// <summary>
        /// 状态信息
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 结果
        /// </summary>
        public string result { get; set; }
    }
}
