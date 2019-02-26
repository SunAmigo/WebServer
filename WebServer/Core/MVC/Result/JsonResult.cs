using Newtonsoft.Json;

namespace WebServer.Core.MVC.Result
{
    public class JsonResult : IActionResult
    {
        private dynamic _object;

        public JsonResult(dynamic obj)
        {
            _object = obj;
        }
        public void ExecuteResult(WebContext context)
        {
            var json = JsonConvert.SerializeObject(_object,Formatting.Indented);
            context.Response.Write(json);
        }
    }
}
