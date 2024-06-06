using Newtonsoft.Json;

namespace TechECommerceServer.Application.Exceptions
{
    public class ExceptionModel : ErrorStatusCode
    {
        public IEnumerable<string> Errors { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
