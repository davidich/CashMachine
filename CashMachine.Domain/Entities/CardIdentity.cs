namespace CashMachine.Domain.Entities
{
    using System.Globalization;
    using System.Security.Claims;
    using System.Security.Principal;

    using Microsoft.AspNet.Identity;

    /// <summary>
    /// Class is interdent to simplify usage of current identity via implicit casting
    /// </summary>
    public class CardIdentity : ClaimsIdentity
    {
        //private readonly ClaimsIdentity _identity;

        private static class ClaimTypes
        {
            public const string Id = "card_id";
            public const string Number = "card_number";
        }

        /// <summary>
        /// CardId of currently logged in user
        /// </summary>
        public int Id
        {
            get
            {
                return IsAuthenticated
                    ? int.Parse(FindFirst(ClaimTypes.Id).Value)
                    : -1;
            }
        }

        /// <summary>
        /// CardNumber of currently logged in user
        /// </summary>
        public string Number
        {
            get
            {
                return IsAuthenticated
                    ? FindFirst(ClaimTypes.Number).Value
                    : string.Empty;
            }
        }

        private CardIdentity(string authenticationType)
            : base(authenticationType)
        {

        }

        public CardIdentity(Card card, string authenticationType)
            : this(card.Id, card.Number, authenticationType)
        {

        }

        public CardIdentity(int id, string number, string authenticationType = "Bearer")
            : base(authenticationType)
        {
            AddClaim(ClaimTypes.Id, id.ToString(CultureInfo.InvariantCulture));
            AddClaim(ClaimTypes.Number, number);
        }

        public static CardIdentity FromIdentity(IIdentity identity)
        {
            var claimsIdentity = identity as ClaimsIdentity;

            if (claimsIdentity == null)
            {
                claimsIdentity = new ClaimsIdentity(identity.AuthenticationType);
                claimsIdentity.AddClaim(new Claim(System.Security.Claims.ClaimTypes.Name, identity.Name));
            }

            var cardIdentity = new CardIdentity(identity.AuthenticationType);
            cardIdentity.AddClaims(claimsIdentity.Claims);

            return cardIdentity;
        }

        /// <summary>
        /// Wrapper serves two pursoses: 
        /// 1) make conveniet api
        /// 2) remove virtuality as it is called from Ctor
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        private void AddClaim(string type, string value)
        {
            AddClaim(new Claim(type, value));
        }
    }
}