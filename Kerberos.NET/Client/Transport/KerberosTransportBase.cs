﻿using Kerberos.NET.Asn1;
using Kerberos.NET.Dns;
using Kerberos.NET.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Kerberos.NET.Transport
{
    public abstract class KerberosTransportBase : IKerberosTransport, IDisposable
    {
        private const int DefaultKerberosPort = 88;

        private readonly string kdc;

        protected KerberosTransportBase(string kdc = null)
        {
            this.kdc = kdc;
        }

        private static readonly ConcurrentDictionary<string, DnsRecord> DomainCache
            = new ConcurrentDictionary<string, DnsRecord>();

        protected ILogger Logger { get; set; }

        public virtual bool TransportFailed { get; set; }

        public virtual KerberosTransportException LastError { get; set; }

        public abstract ProtocolType Protocol { get; }

        public bool Enabled { get; set; }

        public virtual void Dispose() { }

        protected void Log(string log)
        {
            Logger?.WriteLine(KerberosLogSource.Client, log);
        }

        protected static T Decode<T>(byte[] response)
            where T : IAsn1ApplicationEncoder<T>, new()
        {
            if (KrbError.CanDecode(response))
            {
                var error = KrbError.DecodeApplication(response);

                if (error.ErrorCode == KerberosErrorCode.KRB_ERR_RESPONSE_TOO_BIG)
                {
                    throw new KerberosTransportException(error);
                }

                throw new KerberosProtocolException(error);
            }

            return new T().DecodeAsApplication(response);
        }

        public virtual Task<TResponse> SendMessage<TRequest, TResponse>(string domain, IAsn1ApplicationEncoder<TRequest> req)
            where TResponse : IAsn1ApplicationEncoder<TResponse>, new()
        {
            return SendMessage<TResponse>(domain, req.EncodeApplication());
        }

        public abstract Task<T> SendMessage<T>(string domain, ReadOnlyMemory<byte> req)
            where T : IAsn1ApplicationEncoder<T>, new();

        protected DnsRecord QueryDomain(string lookup)
        {
            if (!string.IsNullOrWhiteSpace(kdc))
            {
                var split = kdc.Split(':');

                var record = new DnsRecord
                {
                    Target = split[0]
                };

                if (split.Length > 0)
                {
                    record.Port = int.Parse(split[1]);
                }

                return record;
            }

            return DomainCache.GetOrAdd(lookup, d => QueryDns(d));
        }

        private DnsRecord QueryDns(string lookup)
        {
            var results = Query(lookup);

            var srv = results.FirstOrDefault(s => s.Type == DnsRecordType.SRV);

            if (srv == null)
            {
                throw new KerberosTransportException($"Cannot locate SRV record for {lookup}");
            }

            if (srv.Port <= 0)
            {
                srv.Port = DefaultKerberosPort;
            }

            return srv;
        }

        protected virtual IEnumerable<DnsRecord> Query(string lookup)
        {
            return DnsQuery.QuerySrv(lookup);
        }
    }
}
