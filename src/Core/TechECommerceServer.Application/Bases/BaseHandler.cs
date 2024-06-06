using AutoMapper;

namespace TechECommerceServer.Application.Bases
{
    public class BaseHandler
    {
        public readonly IMapper _mapper;
        public BaseHandler(IMapper mapper)
        {
            _mapper = mapper;
        }
    }
}
