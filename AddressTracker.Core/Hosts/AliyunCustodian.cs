namespace AddressTracker.Core.Hosts
{
    public class AliyunCustodian(string authorizationToken) : Custodian(authorizationToken)
    {
        public override ValueTask<bool> AddDomainAsync(string domain)
        {
            throw new NotImplementedException();
        }

        public override ValueTask<bool> AddRecordAsync(DomainRecord record)
        {
            throw new NotImplementedException();
        }

        public override ValueTask<IEnumerable<string>> GetAllDomainsAsync()
        {
            throw new NotImplementedException();
        }

        public override ValueTask<bool> UpdateRecordAsync(DomainRecord record)
        {
            throw new NotImplementedException();
        }
    }
}
