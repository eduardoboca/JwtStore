using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using JwtStore.Core.Contexts.AccountContext.ValueObjects;
using JwtStore.Core.Contexts.SharedContext.Extensions;
using JwtStore.Core.Contexts.SharedContext.ValueObjects;

namespace JwtStore.Core.Contexts.AccountContext.ValueObjects
{
    public partial class Email : ValueObject
    {
        private const string Pattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

        protected Email()
        {
        }

        public Email(string address)
        {
            if (string.IsNullOrEmpty(address))
                throw new Exception("E-mail inválido");

            Address = address.Trim().ToLower();
            if (address.Length < 5)
                throw new Exception("E-mail inválido");

            if (!EmailRegex().IsMatch(Address))
                throw new Exception("E-mail inválido");
        }

        public string Address { get; } = string.Empty;
        public string Hash => Address.ToBase64();

        public Verification Verification { get; private set; } = new();

        public void ResendVerification() => Verification = new Verification();

        public static implicit operator string(Email email) => email.ToString();

        public static implicit operator Email(string address) => new Email(address);

        public override string ToString() => Address;

        [GeneratedRegex(Pattern)]
        private static partial Regex EmailRegex();
    }
}