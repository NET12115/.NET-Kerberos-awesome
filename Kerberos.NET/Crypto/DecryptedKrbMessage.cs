// -----------------------------------------------------------------------
// Licensed to The .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// -----------------------------------------------------------------------

using System;
using Kerberos.NET.Entities;
using static Kerberos.NET.Entities.KerberosConstants;

namespace Kerberos.NET.Crypto
{
    public abstract class DecryptedKrbMessage
    {
        private Func<DateTimeOffset> nowFunc;

        [KerberosIgnore]
        public Func<DateTimeOffset> Now
        {
            get { return this.nowFunc ?? (this.nowFunc = () => DateTimeOffset.UtcNow); }
            set { this.nowFunc = value; }
        }

        public abstract void Validate(ValidationActions validation);

        public virtual void Decrypt(KeyTable keytab)
        {
            throw new NotSupportedException();
        }

        public abstract void Decrypt(KerberosKey key);

        public virtual TimeSpan Skew { get; protected set; } = TimeSpan.FromMinutes(5);

        protected virtual void ValidateTicketEnd(DateTimeOffset endTime, DateTimeOffset now, TimeSpan skew)
        {
            if (endTime < (now - skew))
            {
                throw new KerberosValidationException(
                    $"Token has expired. End: {endTime}; Now: {now}; Skew: {skew}",
                    nameof(endTime)
                );
            }
        }

        protected virtual void ValidateTicketRenewal(DateTimeOffset? renewTill, DateTimeOffset now, TimeSpan skew)
        {
            if (renewTill == null || renewTill < (now - skew))
            {
                throw new KerberosValidationException(
                    $"Token cannot be renewed any further. Renew Till: {renewTill}; Now: {now}; Skew: {skew}",
                    nameof(renewTill)
                );
            }
        }

        protected virtual void ValidateTicketStart(DateTimeOffset startTime, DateTimeOffset now, TimeSpan skew)
        {
            if (startTime > (now + skew))
            {
                throw new KerberosValidationException(
                    $"Token Start isn't valid yet. Start: {startTime}; Now: {now}; Skew: {skew}",
                    nameof(startTime)
                );
            }
        }

        protected virtual void ValidateRealm(string leftName, string rightName)
        {
            if (!string.Equals(leftName, rightName, StringComparison.OrdinalIgnoreCase))
            {
                throw new KerberosValidationException(
                    $"Ticket ({leftName}) and Authenticator ({rightName}) realms do not match"
                );
            }
        }

        protected virtual void ValidateTicketSkew(DateTimeOffset now, TimeSpan skew, DateTimeOffset ctime)
        {
            if (!WithinSkew(now, ctime, 0, skew))
            {
                throw new KerberosValidationException(
                    $"Token window is greater than allowed skew. Start: {ctime}; End: {now}; Allowed Skew: {skew}",
                    nameof(skew)
                );
            }
        }

        protected virtual void ValidateClientPrincipalIdentifier(KrbPrincipalName leftName, KrbPrincipalName rightName)
        {
            if (leftName == null)
            {
                throw new ArgumentNullException(nameof(leftName));
            }

            if (!leftName.Matches(rightName))
            {
                throw new KerberosValidationException(
                    "Ticket CName " +
                    $"({leftName.Type}: {leftName.Name})" +
                    " does not match Authenticator CName " +
                    $"({rightName?.Type}: {rightName?.Name})",
                    nameof(KrbPrincipalName)
                );
            }
        }
    }
}
