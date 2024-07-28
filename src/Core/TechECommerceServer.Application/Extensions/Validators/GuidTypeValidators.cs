namespace TechECommerceServer.Application.Extensions.Validators
{
    public static class GuidTypeValidators
    {
        public static bool IsValidGuid(string id)
        {
            return Guid.TryParse(id, out _);
        }
    }
}
