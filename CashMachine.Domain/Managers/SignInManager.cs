namespace CashMachine.Domain.Managers
{
    using CashMachine.Domain.Entities;

    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin.Security;

    public class SignInManager : SignInManager<Card, int>
    {
        public SignInManager(CardManager cardManager, IAuthenticationManager authenticationManager)
            : base(cardManager, authenticationManager)
        {
        }        
    }
}