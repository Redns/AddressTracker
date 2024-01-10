using AddressTracker.Core.Hosts;
using AddressTracker.Core.Models;

namespace AddressTracker.Core.Factories
{
    public static class CustodianFactory
    {
        public static Custodian CreateHost(CustodianAccountModel model)
        {
            return model.CustodianCode switch
            {
                CustodianCode.DnsPod => new DnsPodCustodian(model.AuthenticationToken),
                _ => throw new NotImplementedException()
            };
        }
    }
}
