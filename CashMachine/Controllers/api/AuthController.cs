using System.Web.Http;

namespace CashMachine.Controllers.api
{
    using System.Threading.Tasks;

    using CashMachine.Domain.Entities;
    using CashMachine.Domain.Managers;

    [RoutePrefix("api/auth")]
    public class AuthController : ApiController
    {
        private readonly CardManager _cardManager;

        public AuthController(CardManager cardManager)
        {
            _cardManager = cardManager;
        }        

        [AllowAnonymous]
        [HttpGet, Route("validate-card-number")]
        public async Task<CardNumberValidationResult> IsValidCardNumber(string number)
        {
            return await _cardManager.ValidateNumberAsync(number);
        }

        [HttpGet, Route("is-valid-token")]
        public bool IsValidToken()
        {
            return User.Identity.IsAuthenticated;
        }
    }
}