namespace Entities.Exceptions
{
    public class PriceOutofRangeBadRequestException : BadRequestException
    {
        public PriceOutofRangeBadRequestException() : base("Maxiumum price should be less than 100 and greater than 10.")
        {

        }
    }
}
