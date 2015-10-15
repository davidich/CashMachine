using System.Web.Http;

namespace CashMachine.Controllers.api
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Security;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using CashMachine.Domain.Entities;
    using CashMachine.Domain.Managers;

    [Authorize]
    [RoutePrefix("api/card")]
    public class CardController : ApiController
    {
        private readonly CardManager _cardManager;

        public CardController(CardManager cardManager)
        {
            _cardManager = cardManager;
        }

        [HttpGet, Route("balance")]
        public async Task<BalanceInfo> GetBalanceInfo()
        {
            var cardIdentity = CardIdentity.FromIdentity(User.Identity);
            return await _cardManager.GetBalanceInfo(cardIdentity);
        }

        [HttpPost, Route("withdraw")]
        public async Task<WithdrawalResult> Withdraw(WithdrawRequest request)
        {
            var cardIdentity = CardIdentity.FromIdentity(User.Identity);
            return await _cardManager.WithdrawAsync(cardIdentity.Id, request.Amount);
        }

        [HttpGet, Route("withdraw-report")]
        public async Task<HttpResponseMessage> WithdrawReport(int id)
        {
            var cardIdentity = CardIdentity.FromIdentity(User.Identity);

            try
            {
                var report = await _cardManager.BuildWithdrawReportAsync(cardIdentity.Id, id);
                return Request.CreateResponse(HttpStatusCode.OK, report);
            }
            catch (SecurityException)
            {
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "You can't view this operation");
            }
            catch (InvalidOperationException)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Operation id was not found");
            }
        }
    }
}